using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Furniture_Gallery.Controllers
{
    public class RegisterLoginController : Controller
    {

        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public RegisterLoginController(ModelContext context , IWebHostEnvironment hostEnvironment )
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }    


        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Firstname,Lastname,Email,ImagePath,Username,Password,RoleId ,ImageUser")] Useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                if (useraccount.ImageUser != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + useraccount.ImageUser.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await useraccount.ImageUser.CopyToAsync(fileStream);
                    }
                    useraccount.ImagePath = imageName;
                }

                 var user = _context.Useraccounts.Where(E=>E.Email ==useraccount.Email).FirstOrDefault();
                if (user != null)
                {
                  
                    ViewBag.message = "EmailAlreadyExists";

                }

                else {
                    useraccount.RoleId = 3;
                    _context.Add(useraccount);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Login));

                    }

                    
               
                //_context.Add(useraccount);
                //await _context.SaveChangesAsync();
                ////return RedirectToAction(nameof(Index));



            }
            return View(useraccount);

        }



        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login([Bind("Email,Password")] Useraccount useraccount)
        {
            if (ModelState.IsValid)
            {


                var user2 = _context.Useraccounts.Where(u => u.Email == useraccount.Email && u.Password == useraccount.Password).SingleOrDefault();


                if (user2 != null)
                {
                    var customer = _context.Useraccounts.Where(x => x.Id == user2.Id).SingleOrDefault();



                    switch (user2.RoleId)
                    {
                        case 3://CUSTOMER 
                            HttpContext.Session.SetInt32("IdCustomer", (int)customer.Id);
                            HttpContext.Session.SetString("fname", user2.Firstname + user2.Lastname);
                            HttpContext.Session.SetString("UserName", "Hello ," + user2.Email);


                            return RedirectToAction("GetProduct", "Home");
							
						case 1://ADMIN

                            HttpContext.Session.SetInt32("IdCustomer", (int)customer.Id);
                            HttpContext.Session.SetString("fname", user2.Firstname + user2.Lastname);
                            HttpContext.Session.SetString("UserName", "Hello ," + user2.Email);
                            return RedirectToAction("Index", "Admin");
                    }
                    return View();
                }

                else
                {
                   
                    ViewBag.message = "Invalid email or password";

                    return View();

                }


            }
            _context.Add(useraccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "RegisterLogin");
        }


    }
}
