using BgDeal.Models;
using BgDeal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Helpers;
using System.IO;
using System.Net;



namespace BgDeal.Web.Controllers
{
    [Authorize]
    public class CreateDealController : BaseController
    {

        //Създаваме DropDownList за създаване на сделка 

        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = this.Data.Topics.All().Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name
            });

            ViewBag.Topics = items;
            return View();

        }

        //Създаване на нова сделка
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Index(CreateDealModel deal, HttpPostedFileBase HeadImage, CreateImagesModel ime, IEnumerable<HttpPostedFileBase> files)
        {

            var img = Path.GetFileName(HeadImage.FileName);

            if (ModelState.IsValid)
            {
                var result = this.Data.Deals.All().Count();
                //Задаваме час и дата кога е създадена сделката
                deal.DateAdded = DateTime.Now;
                //Вземаме Ид-то на текущият потребител
                var userid = this.User.Identity.GetUserId();
                //Проверяваме дали сме подали снимка
                if (HeadImage != null && HeadImage.ContentLength > 0 && HeadImage.ContentType.ToLower() == "image/jpeg")
                {

                    //Оразмеряваме снимката до 1000 пиксела
                    WebImage imga = new WebImage(HeadImage.InputStream);
                    if (imga.Width > 585)
                    {
                        imga.Resize(585, 520, true, false);
                    }

                    //Създавам директория
                    Directory.CreateDirectory(Server.MapPath("~/Content/productImages/" + ((int)result + 1)));
                    //задавам пътя къде да се запише снимката
                    var path = Path.Combine(Server.MapPath("~/Content/productImages/" + ((int)result + 1)),
                                           System.IO.Path.GetFileName(HeadImage.FileName));

                    //записвам снимката
                    imga.Save(path);

                    if (imga.Width > 160)
                    {
                        imga.Resize(160, 160, true, false);
                    }

                    var path2 = Path.Combine(Server.MapPath("~/Content/productImages/" + ((int)result + 1)),
                                           System.IO.Path.GetFileName("thumb_" + HeadImage.FileName));

                    imga.Save(path2);

                    //подаваме как се казва снимката
                    deal.HeadImage = img;
                }

                else
                {
                    throw new FileLoadException("Грешен формат на заглавната снимка!");
                }

                var currentUser = this.User.Identity.Name;

                var ff = this.Data.DealUsers.All().Where(x => x.Author == currentUser);

                int gosho = 0;

                foreach (var imer in ff)
                {
                    gosho = imer.AuthorId;
                }

                if (deal.DealTitle.Length > 60)
                {
                    throw new FileLoadException("Заглавието е повече от 60 символа");
                }
                if (deal.Phone.Length > 15)
                {
                    throw new FileLoadException("Телефона неможе да съдържа повече от 15 цифри");
                }
                if (deal.Price.ToString().Length > 15)
                {
                    throw new FileLoadException("Няма такава цена");
                }
                if (deal.Location.Length > 30)
                {
                    throw new FileLoadException("Няма такова село");
                }
                if (deal.Content.Length > 3000)
                {
                    throw new FileLoadException("Роман ли пишем? Точно кратко и ясно, благодаря!");
                }


                //Проекция на това което ще записваме в базата
                this.Data.Deals.Add(new Deal()
                {
                    // Идва от модела който работи с базата = от локалният модел
                    AuthorId = userid,
                    Content = deal.Content,
                    DateAdded = DateTime.Now,
                    HeadImage = deal.HeadImage,
                    Location = deal.Location,
                    Price = deal.Price,
                    Title = deal.DealTitle,
                    Topic = deal.TopicSearch,
                    Phone = deal.Phone,
                    Author = currentUser,
                    PotrebitelskiNomer = gosho,
                    Active = true,
                });
                this.Data.SaveChanges();


            }



            if (files != null)
            {
                foreach (var file in files)
                {

                    var result = this.Data.Deals.All().Count();
                    //Задаваме час и дата кога е създадена сделката
                    var img2 = Path.GetFileName(file.FileName);
                    //Вземаме Ид-то на текущият потребител
                    var userid = this.User.Identity.GetUserId();
                    //Проверяваме дали сме подали снимка
                    if (files != null && file.ContentLength > 0 && HeadImage.ContentType.ToLower() == "image/jpeg")
                    {

                        //Оразмеряваме снимката до 1000 пиксела
                        WebImage imga = new WebImage(file.InputStream);
                        if (imga.Width > 585)
                        {
                            imga.Resize(585, 520, true, false);
                        }



                        //задавам пътя къде да се запише снимката
                        var path = Path.Combine(Server.MapPath("~/Content/productImages/" + ((int)result)),
                                               System.IO.Path.GetFileName(file.FileName));

                        //записвам снимката
                        imga.Save(path);

                        if (imga.Width > 160)
                        {
                            imga.Resize(160, 160, true, false);
                        }

                        var path2 = Path.Combine(Server.MapPath("~/Content/productImages/" + ((int)result)),
                                               System.IO.Path.GetFileName("thumb_" + file.FileName));

                        imga.Save(path2);

                        //подаваме как се казва снимката
                        ime.ImageName = img2;

                        this.Data.Images.Add(new Image
                        {
                            DealId = result,
                            ImageUrl = ime.ImageName,

                        });
                        this.Data.SaveChanges();
                    }

                    else
                    {
                        throw new FileLoadException("Грешен формат на заглавната снимка!");
                    }

                }
                //Krai proba




            }

            return RedirectToAction("Index", "Home");
        }

        private BgDeal.Data.ApplicationDbContext db = new BgDeal.Data.ApplicationDbContext();

        public ActionResult EditDeal(int? id)
        {

            IEnumerable<SelectListItem> items = this.Data.Topics.All().Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name
            });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Deal deals = db.Deals.Find(id);

            var img = this.Data.Images.All().Where(x => x.DealId == id).Select(x => new ImagesEditModel { ImageUrl = x.ImageUrl, Id = x.Id, DealId = x.DealId });


            if (deals == null)
            {
                return HttpNotFound();
            }
            if (object.Equals(deals.Author, User.Identity.Name))
            {
                ViewBag.Images = img;
                ViewBag.Topics = items;
                return View(deals);
            }



            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditDeal(Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deal).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AllDeals", "Profile");
            }



            return RedirectToAction("AllDeals", "Profile");
        }




        public ActionResult EditHeadImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Deal deals = db.Deals.Find(id);

            if (deals == null)
            {
                return HttpNotFound();
            }
            if (object.Equals(deals.Author, User.Identity.Name))
            {
                return View(deals);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditHeadImage(Deal deals, HttpPostedFileBase HeadImage, CreateImagesModel ime)
        {



            if (HeadImage != null && HeadImage.ContentLength > 0 && HeadImage.ContentType.ToLower() == "image/jpeg")
            {
                var img = Path.GetFileName(HeadImage.FileName);
                //Оразмеряваме снимката до 1000 пиксела
                WebImage imga = new WebImage(HeadImage.InputStream);
                if (imga.Width > 585)
                {
                    imga.Resize(585, 520, true, false);
                }

                //Създавам директория
                Directory.CreateDirectory(Server.MapPath("~/Content/productImages/" + deals.Id));
                //задавам пътя къде да се запише снимката
                var path = Path.Combine(Server.MapPath("~/Content/productImages/" + deals.Id),
                                       System.IO.Path.GetFileName(HeadImage.FileName));

                //записвам снимката
                imga.Save(path);

                if (imga.Width > 160)
                {
                    imga.Resize(160, 160, true, false);
                }

                var path2 = Path.Combine(Server.MapPath("~/Content/productImages/" + deals.Id),
                                       System.IO.Path.GetFileName("thumb_" + HeadImage.FileName));

                imga.Save(path2);

                //подаваме как се казва снимката
                deals.HeadImage = img;
            }

            else
            {
                throw new FileLoadException("Грешен формат на заглавната снимка!");
            }

            if (ModelState.IsValid)
            {
                db.Entry(deals).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AllDeals", "Profile");
            }



            return RedirectToAction("AllDeals", "Profile");


        }



        //Незнам за какво са. Да ги пробвам!

        public ActionResult Images()
        {
            return View();
        }


        public ActionResult Async()
        {
            return View();
        }



        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/productImages/"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }



    }
}