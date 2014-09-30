using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BgDeal.Models;
using BgDeal.Data;
using BgDeal.Web.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Net;
using System.Web.Helpers;
using System.IO;

namespace BgDeal.Web.Controllers
{
    public class ProfileController : BaseController
    {
        public ActionResult UserDetails(string id)
        {
            var modelList = this.Data.DealUsers.All().Where(x => x.Author == id)
                .Select(x => new PublicProfile
                {
                    DateRegister = x.DateRgister,
                    FirstName = x.FirstName,
                    HeadPicture = x.Image,
                    Location = x.Location,
                    Mobile = x.Mobile,
                    SurName = x.Surname,
                    Id = x.AuthorId,
                    Author = x.Author,
                    
                    


                });

            var AllDeals = this.Data.Deals.All().Where(c => c.Author == id)
                .Select(c => new DealViewModel
                {
                    Active=c.Active,
                    Author = c.Author,
                    Id = c.Id,
                    ImageUrl = c.HeadImage,
                    Price = c.Price,
                    Title = c.Title.Substring(0,20),
                });


            var messages= 0;

            if (User.Identity.Name == id)
            {
                messages = this.Data.Messages.All().Where(x => x.MessageTo == id && x.MessageRead == false).Count();
            }

            
            ViewBag.Message = messages;
            
            ViewBag.AllDeals = AllDeals;

            ViewBag.DealsNumber = AllDeals.Count(); 
            
            return View(modelList);

            
        }

        const int MessageSize = 10; // Kolko syobshteniq da se poqwqwat

        [HttpGet]
        public ActionResult Messages(string id, int? id2)
        {
            if (object.Equals(id, User.Identity.Name))
            {


                var message = this.Data.Messages.All().OrderByDescending(x=>x.DateSend).Where(x => x.MessageTo == id)
                    .Select(x => new MessageViewModel
                    {
                        AuthorId = x.AuthorId,
                        Content = x.Content,
                        DateSend = x.DateSend,
                        DealId = x.DealId,
                        Id = x.Id,
                        MessageFrom = x.MessageFrom,
                        MessageRead = x.MessageRead,
                        MessageTo = x.MessageTo,
                        MessageToId = x.MessageToId,
                        Title = x.Title,

                    });

                int pageNumber = id2.GetValueOrDefault(1);

                var viewModel = message.Skip((pageNumber - 1) * MessageSize).Take(MessageSize);
                ViewBag.Pages = Math.Ceiling((double)message.Count() / MessageSize);


                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult MessagesRead(int id)
        {



            var read = this.Data.Messages.All().Where(x => x.Id == id);

            
            
            foreach (var ii in read)
            {
                if (object.Equals(ii.MessageTo, User.Identity.Name))
                {
                    ii.MessageRead = true;
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            this.Data.SaveChanges();

            var result = this.Data.Messages.All().Where(x => x.Id == id)
                .Select(x => new MessageViewModel
                {
                    AuthorId = x.AuthorId,
                    Content = x.Content,
                    DateSend = x.DateSend,
                    DealId = x.DealId,
                    Id = x.Id,
                    MessageFrom = x.MessageFrom,
                    MessageTo = x.MessageTo,
                    MessageToId = x.MessageToId,
                    Title = x.Title,
                    DealTitle = x.DealTitle,
                });
            
            
            return View(result);
        }

        public ActionResult MessagesOut(string id, int? id2)
        {

            if (object.Equals(id, User.Identity.Name))
            {
                var result = this.Data.MessagesOut
                    .All().OrderByDescending(x=>x.DateSend)
                    .Where(x => x.MessageFrom == id)
                    .Select(x => new MessageViewModel
                    {
                        AuthorId = x.AuthorId,
                        Content = x.Content,
                        DateSend = x.DateSend,
                        DealId = x.DealId,
                        Id = x.Id,
                        MessageFrom = x.MessageFrom,
                        MessageTo = x.MessageTo,
                        MessageToId = x.MessageToId,
                        Title = x.Title,
                        DealTitle = x.DealTitle,
                    });

                int pageNumber = id2.GetValueOrDefault(1);

                var viewModel = result.Skip((pageNumber - 1) * MessageSize).Take(MessageSize);
                ViewBag.Pages = Math.Ceiling((double)result.Count() / MessageSize);
                
                
                
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");

            
        }

        public ActionResult MessagesReadOut(int id)
        {



            var read = this.Data.MessagesOut.All().Where(x => x.Id == id);



            foreach (var ii in read)
            {
                if (object.Equals(ii.MessageFrom, User.Identity.Name))
                {


                    var result = this.Data.MessagesOut.All().Where(x => x.Id == id)
                        .Select(x => new MessageViewModel
                        {
                            AuthorId = x.AuthorId,
                            Content = x.Content,
                            DateSend = x.DateSend,
                            DealId = x.DealId,
                            Id = x.Id,
                            MessageFrom = x.MessageFrom,
                            MessageTo = x.MessageTo,
                            MessageToId = x.MessageToId,
                            Title = x.Title,
                            DealTitle = x.DealTitle,
                        });


                    return View(result);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        private BgDeal.Data.ApplicationDbContext db = new BgDeal.Data.ApplicationDbContext();

        public ActionResult DelleteMessage(int[] Id)
        {
            
            

            foreach(var ii in Id)
            {


                Message message = db.Messages.Find(ii);
                db.Messages.Remove(message);
                db.SaveChanges();

            }
            

            return RedirectToAction("Messages/"+@User.Identity.Name);

            
        }

        public ActionResult DelleteMessageSent(int[] Id)
        {
            foreach(var ii in Id)
            {
                MessageOut message = db.MessagesOut.Find(ii);
                db.MessagesOut.Remove(message);
                db.SaveChanges();
            }

            return RedirectToAction("MessageOut/" + @User.Identity.Name);
        }

        public ActionResult EditProfile(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DealUser dealUser = db.DealUsers.Find(id);

            if(dealUser == null)
            {
                return HttpNotFound();
            }
            if (object.Equals(dealUser.Author, User.Identity.Name))
            {
                return View(dealUser);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(DealUser dealUser, HttpPostedFileBase Image)
        {

            if (object.Equals(dealUser.Author, User.Identity.Name))
            {

                if(Image == null)
                {

                }
                else
                {
                    int usernumber = dealUser.AuthorId;

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

                    dealUser.Image = img;

                }
                                
                if (ModelState.IsValid)
                {
                    db.Entry(dealUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("UserDetails/"+User.Identity.Name);
                }
            }

            return View(dealUser);

        }

        public ActionResult AllDeals()
        {
            var result = this.Data.Deals.All()
                .Where(x => x.Author == User.Identity.Name)
                .Select(x => new DealViewModel
                         {
                             Active = x.Active,
                             Author = x.Author,
                             Id = x.Id,
                             ImageUrl = x.HeadImage,
                             Price = x.Price,
                             Title = x.Title
                         }
                    );

            return View(result);
        }
    }

    

}