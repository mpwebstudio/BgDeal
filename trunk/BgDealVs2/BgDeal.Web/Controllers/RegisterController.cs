using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BgDeal.Data;
using BgDeal.Web.Models;
using BgDeal.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Net;
using System.Web.Security;
using System.IO;
using System.Web.Helpers;



namespace BgDeal.Web.Controllers
{
    
    [Authorize]
    public class RegisterController : BaseController
    {
        

        //След регистрация потребителя идва директно тука,
        //като му записваме името в базата данни и му даваме ID
        public ActionResult UserInfo()
        {
            var current = this.User.Identity.GetUserName();

            int pencho = this.Data.DealUsers.All().Where(x => x.Author == current).Count();

            //Проверяваме дали вече потребителят не съществува
            if (pencho == 0)
            {
                int pesho = this.Data.DealUsers.All().Count() + 1;
                
                //Записваме му името
                this.Data.DealUsers.Add(new DealUser()
                                         {
                                             Author = current,
                                             AuthorId = pesho,
                                             DateRgister = DateTime.Now.ToString(),
                                         });
                this.Data.SaveChanges();

                
                return RedirectToAction("InfoSave/"+pesho);
            }
            // Ако вече има регистрация за този потребител го препращаме към заглавната
            else
            {
                return RedirectToAction("Index", "Home");
            }
        
        }
        

        public ActionResult Info()
        {

            return View();
        }

        //Подавам номера на потребителя. Изпълнява се този скрипт, а не долният защото няма [HttpPost] атрибут, след като ние препращаме 
        //към тази страница без POST.
        //Този вариянт може да се използва за UPDATE на запис в базата 
        [Authorize]
        public ActionResult InfoSave(int? id)
        {

            var us = this.User.Identity.Name;

            
            //Инстанция на базата

            BgDeal.Data.ApplicationDbContext db = new Data.ApplicationDbContext();

            //Вземаме записа който отговаря на потребителя
            DealUser dealUser = db.DealUsers.Find(id);

            if (dealUser == null)
            {
                return HttpNotFound();
            }
            

            if(!object.Equals(dealUser.Author,us))
            {
                return RedirectToAction("Index", "Home");
            }
           
            return View(dealUser);


        }
        
        
        [Authorize]
        [HttpPost]
        public ActionResult InfoSave(DealUser dealuser, HttpPostedFileBase Image)
        {
            

            int usernumber = dealuser.AuthorId;
            
            var img = Path.GetFileName(Image.FileName);

            WebImage imga = new WebImage(Image.InputStream);
            if (imga.Width > 585)
            {
                imga.Resize(585, 585, true, false);
            }

            //Създавам директория
            Directory.CreateDirectory(Server.MapPath("~/Content/UserImages/" + usernumber));
            //задавам пътя къде да се запише снимката
            var path = Path.Combine(Server.MapPath("~/Content/UserImages/" + usernumber),
                                   System.IO.Path.GetFileName(Image.FileName));

            //записвам снимката
            imga.Save(path);

            if (imga.Width > 120)
            {
                imga.Resize(120, 120, true, false);
            }

            var path2 = Path.Combine(Server.MapPath("~/Content/UserImages/" + usernumber),
                                   System.IO.Path.GetFileName("thumb_" + Image.FileName));

            imga.Save(path2);


            if (imga.Width > 45)
            {
                imga.Resize(45, 45, true, false);
            }

            var path3 = Path.Combine(Server.MapPath("~/Content/UserImages/" + usernumber),
                                   System.IO.Path.GetFileName("uInfo_" + Image.FileName));

            imga.Save(path3);



                 dealuser.Image = img;
                 dealuser.DateRgister = DateTime.Now.ToString();
                 dealuser.Author = this.User.Identity.GetUserName();




                 BgDeal.Data.ApplicationDbContext db = new Data.ApplicationDbContext();

                 db.Entry(dealuser).State = System.Data.Entity.EntityState.Modified;

                 //this.Data.DealUsers.Add(dealuser);


                 db.SaveChanges();
             

            return RedirectToAction("Index", "Home");
        }
    }
}