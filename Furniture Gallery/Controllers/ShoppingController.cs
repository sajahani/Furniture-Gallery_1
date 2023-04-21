using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using System.Net.Mail;

using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace Furniture_Gallery.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
		private readonly object _logger;

		public ShoppingController(ModelContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
 
        [HttpGet]

        public ActionResult AddToItem(decimal id)
        {
			ViewBag.user = HttpContext.Session.GetString("fname");
			 decimal num = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			   var product =  _context.Furnitureproducts.Where(m => m.Id == id).FirstOrDefault();
            return View(product);
		}


		[HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> AddToItem(Productorder productorder)
        {
            ViewBag.user = HttpContext.Session.GetString("fname");

            productorder.Id = 0;
            // Get the current user's id
            var userId = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));

            // Create a new Order object
            var order = new Furnitureorder
            {
                OrderDate = DateTime.Now,
                OrderStatus = "UnPaid",
                UseraccountId = userId
            };

            
              _context.Furnitureorders.Add(order);
                 await _context.SaveChangesAsync();

            // Update the ProductOrder object with the OrderId
            productorder.OrderId = order.Id;
           
            productorder.TotalAmount = productorder.Quantity *  _context.Furnitureproducts.Find(productorder.ProductId).Price;

            // Add the ProductOrder object to the database
            _context.Productorders.Add(productorder);
              await _context.SaveChangesAsync();

            return RedirectToAction("Join2", "Shopping");
        }

        [HttpGet]
		public ActionResult Join2(decimal userId)
		{
           

            ViewBag.user = HttpContext.Session.GetString("fname");
			userId = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			var orders = _context.Furnitureorders.ToList();
            var products = _context.Furnitureproducts.ToList();
            var productorders = _context.Productorders.ToList();

			var allorder = (from o in _context.Furnitureorders
						 join po in _context.Productorders on o.Id equals po.OrderId
						 join p in _context.Furnitureproducts on po.ProductId equals p.Id
                        
						
						 where o.UseraccountId == userId && o.OrderStatus == "UnPaid" 
						 select new Join2
						 {
							 order = o,
							 product = p,
							 productorder = po ,
                           
						 }).ToList();

			if (allorder == null)
			{
				return NotFound();
			}
			else 
			{
				return View(allorder);
			}

			

		}

		public async Task<IActionResult> DeleteConfirmed(decimal id)
		{
            ViewBag.user = HttpContext.Session.GetString("fname");
            var furnitureorder = await _context.Furnitureorders.FindAsync(id);
			_context.Furnitureorders.Remove(furnitureorder);
			await _context.SaveChangesAsync();
			return RedirectToAction("Join2", "Shopping");
		}


	

		public void  SendEmail(FileStreamResult file, decimal? userid)
		{
			

			var user = _context.Useraccounts.Where(c => c.Id == userid).SingleOrDefault();
			
				var attachmentfile = file.FileStream;
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
				mail.From = new MailAddress("furnitureshop889@gmail.com");
				mail.To.Add(user.Email);
				mail.Subject = "Furniture Shop Invoice";
				mail.Body = "Thanks for your visit";
				mail.IsBodyHtml = false;

				System.Net.Mail.Attachment attachment;
				attachment = new Attachment(attachmentfile, "invoice.pdf");
				mail.Attachments.Add(attachment);

				SmtpServer.Port = 587;
				SmtpServer.Host = "smtp.gmail.com";
				SmtpServer.UseDefaultCredentials = true;
				SmtpServer.Credentials = new System.Net.NetworkCredential("furnitureshop889@gmail.com", "dxtudadvpuitsnxb");
				SmtpServer.EnableSsl = true;

				SmtpServer.Send(mail);
			
			

			

		}


		public IActionResult pdf(Furniturepayment furniturepayment)
		{
	
			var userId = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			var infouser = _context.Useraccounts.Where(x => x.Id == furniturepayment.UseraId).SingleOrDefault();
			//var infoinvoice = _context.Productorders.Include(x => x.Product).Include(x=>x.Payment).Include(x => x.Order).Where(x => x.Order.UseraccountId == x.Payment.UseraId && x.Order.OrderStatus == "Paid").ToList();

			var orders = _context.Furnitureorders.ToList();
			var products = _context.Furnitureproducts.ToList();
			var productorders = _context.Productorders.ToList();

			var order = (from o in _context.Furnitureorders
							join po in _context.Productorders on o.Id equals po.OrderId
							join p in _context.Furnitureproducts on po.ProductId equals p.Id
							


							where o.UseraccountId == userId &&  o.OrderStatus == "Paid"
							select new Join2
							{
								order = o,
								product = p,
								productorder = po,

							}).ToList();


			// Example using iTextSharp
			MemoryStream workStream = new MemoryStream();
			    var document = new iTextSharp.text.Document();
		    	PdfWriter.GetInstance(document, workStream).CloseStream = false;
		        document.Open();
				document.Add(new Paragraph("Invoice Number " + furniturepayment.Id));
				document.Add(new Paragraph("Customer Name : " + infouser.Firstname + infouser.Lastname));
				document.Add(new Paragraph("Date: " + furniturepayment.PaymentDate.ToString()));
				document.Add(new Paragraph("Total  " + furniturepayment.PaymentAmount));
			//var image = iTextSharp.text.Image.GetInstance("~/AssetsDash/img/faces/logo (2).png");
			//image.Alignment = Element.ALIGN_CENTER;
			//document.Add(image);
				document.Add(new Paragraph(" "));
			
			var table = new PdfPTable(3);
				table.AddCell("Item");
				table.AddCell("Quantity");
				table.AddCell("Price");
				foreach (var item in order)
				{
					table.AddCell(item.product.Productname);
					table.AddCell(item.productorder.Quantity.ToString());
					table.AddCell(item.product.Price.ToString());
				}
				document.Add(table);

				document.Close();

			byte[] byteInfo = workStream.ToArray();
			workStream.Write(byteInfo, 0, byteInfo.Length);
			workStream.Position = 0;


			return new FileStreamResult(workStream, "invoice/pdf");
		


		}






		[HttpGet]
		public IActionResult CheckOut()
		{
            ViewBag.user = HttpContext.Session.GetString("fname");
            ViewBag.userid =HttpContext.Session.GetInt32("IdCustomer");
			return View();
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CheckOut(decimal? creditcard, decimal? cvv, decimal? userid)
		{
            ViewBag.user = HttpContext.Session.GetString("fname");
            var total = _context.Productorders.Include(x => x.Product).Include(x => x.Order).
		Where(x => x.Order.UseraccountId == userid && x.Order.OrderStatus == "UnPaid").Sum(x => x.TotalAmount);


			//var paymentid = _context.Furnitureorders.Include(x => x.Productorders).SingleOrDefault(x => x.UseraccountId == userid).Id;
			//	var dbCVV = _context.Banks.SingleOrDefault(b => b.Id == id2).Cvv;var dbCreditCard = _context.Banks.SingleOrDefault(b => b.Id == id2).CreditCard;
			//	var dbCVV = _context.Banks.SingleOrDefault(b => b.Id == id2).Cvv;var dbCreditCard = _context.Banks.SingleOrDefault(b => b.Id == id2).CreditCard;

			var bank = _context.Banks.Where(x => x.Cvv == cvv && x.CreditCard == creditcard).SingleOrDefault();

			if (bank !=null)
			{
				if (bank.Balance > total)
				{
					bank.Balance = bank.Balance - total;
					_context.SaveChanges();
					var payment = new Furniturepayment
					{
						PaymentDate = DateTime.Now,
						PaymentAmount = total,
						 UseraId = userid,

					};
					_context.Furniturepayments.AddAsync(payment);
					await _context.SaveChangesAsync();

					pdf(payment);

					SendEmail((FileStreamResult)pdf(payment), userid);

					Productorder productorder = new Productorder();
				          productorder.PaymentId = payment.Id;
					await _context.SaveChangesAsync();

					var orderStatus = _context.Productorders.Include(p => p.Product).Include(p => p.Order).Where(p => p.Order.UseraccountId == userid && p.Order.OrderStatus == "UnPaid");
					if (orderStatus != null)
					{
						foreach (var item in orderStatus)
						{
							item.Order.OrderStatus = "Paid";

							_context.Update(item);
							await _context.SaveChangesAsync();
						}
						


					}
					
					TempData["SuccessMessage"] = "Email sent successfully!";

					return RedirectToAction("CheckOut", "Shopping");

				}
				else
				{
					ViewBag.message2 = " The Balance  Not  enough";

				}
			}
			
			else
			{
				ViewBag.message = "CreditCard or CVV  Not Exists";
				
			}
			ViewBag.message3 = "Email Sent Successfully";
			return RedirectToAction("Index" , "Home");
			



		}


		


		



	}
}
