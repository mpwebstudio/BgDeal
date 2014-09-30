using BgDeal.Models;
using BgDeal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Resources;
using Kendo.Mvc.Infrastructure;
using System.Net.Http;


namespace BgDeal.Web.Controllers
{
    
    public class HomeController : BaseController
    {



        private IQueryable<DealViewModel> GetAllDeals()
        {
            var listOfDeals = this.Data.Deals.All()
                .OrderByDescending(x => x.DateAdded)
                .Select(x => new DealViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImageUrl = x.HeadImage,
                    Price = x.Price,
                    Author = x.Author,
                    Active = x.Active
                });


            return listOfDeals;
        }


        public ActionResult Details(int id)
        {



            
            var viewModel = this.Data.Deals.All().Where(x => x.Id == id)
                .Select(x => new DetailsViewModel
                {
                    Id = id,
                    AuthorId = x.PotrebitelskiNomer, 
                    Author = x.Author,
                    Title = x.Title,
                    Price = x.Price,
                    Location = x.Location,
                    HeadImage = x.HeadImage,
                    Content = x.Content,
                    Comments = x.Comments.Select(z => new CommentViewModel { AuthorUsername = z.Author.UserName, Content = z.Content, DateAdded = z.DateAdded, PotrebitelID = z.PotrebitelID }),
                    Phone = x.Phone,
                    DateAdded = x.DateAdded,
                    Images = x.Images.Select(y => new CreateImagesModel { ImageName = y.ImageUrl, Id = y.Id })

                }).FirstOrDefault();


            

            return View(viewModel);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Message (DetailsViewModel message)
        {
            var curUser = this.User.Identity.GetUserName();

            var pencho = this.Data.DealUsers.All().Where(x => x.Author == curUser);
            var useId = 0;

            int currentPage = message.CurrentPage;

            foreach (var userd in pencho)
            {
                 useId = userd.AuthorId;
            }


            this.Data.Messages.Add(new Message()
                {
                    AuthorId = useId, 
                    Content = message.MessageContent,
                    DateSend = DateTime.Now,
                    MessageFrom = curUser,
                    MessageTo = message.Author,
                    MessageToId = message.AuthorId,
                    Title = message.MessageTitle,
                    DealId = currentPage,
                    DealTitle = message.Title,
                    
                    

                });
                       
                        
            this.Data.MessagesOut.Add(new MessageOut()
            {
                AuthorId = useId,
                Content = message.MessageContent,
                DateSend = DateTime.Now,
                MessageFrom = curUser,
                MessageTo = message.Author,
                MessageToId = message.AuthorId,
                Title = message.MessageTitle,
                DealId = currentPage,
                DealTitle = message.Title,



            });

            this.Data.SaveChanges();
            
            return RedirectToAction("Details/"+currentPage, "Home");
        }



        const int PageSize = 16; // Колко сделки да се появяват на първа страница

        //Странициране
        public ActionResult Index(int? id)
        {


            int pageNumber = id.GetValueOrDefault(1);

            var viewModel = GetAllDeals().Skip((pageNumber - 1) * PageSize).Take(PageSize);
            //Използваме закръгляне (Ceiling), ако резултата е 12.2 то всъщност подаваме стойност 13 Пример ( 14 / 12 = 2)
            ViewBag.Pages = Math.Ceiling((double)GetAllDeals().Count() / PageSize);

            return View(viewModel);
        }



        //Всеки който се опитва да постне коментар трябвя да е влязъл в системата
        [Authorize]
        //Задаваме уникално value(стойност) на формата за всеки потребител като тука се проверява тя дали съвпада с тази от формата
        //едната стойност се пази на сървара, а другата на компютъра на потребителя. Така неможе да се потпъхнат данни дори и ако откраднат сесията(cookies)
        [ValidateAntiForgeryToken]
        //Като го сложим в SubmitCommentModel позволяваме на MWC- то и defaul binder да види какви пропъртита имаме и чрез атрибутите 
        //които сме сложили за съответните пропъртита да валидира данните
        [ValidateInput(false)]
        public ActionResult PostComment(SubmitCommentModel commentModel)
        {
            if(commentModel.Comment.Length < 10)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ModelState.Values.First().ToString());
            }
            if(commentModel.Comment.Length > 2500)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ModelState.Values.First().ToString());
            }
            //Ако модела е валиден
            if (ModelState.IsValid)
            {
                //Вземаме текущият потребител, добавяме using Microsoft.AspNet.Identity;
                var username = this.User.Identity.GetUserName();
                var userId = this.User.Identity.GetUserId();

                var ff = this.Data.DealUsers.All().Where(x => x.Author == username);

                int gosho = 0;

                foreach (var imer in ff)
                {
                    gosho = imer.AuthorId;
                }
                


                
                //Добавяме нов коментар чрез Comment.cs
                this.Data.Comments.Add(new Comment()
                {
                    AuthorId = userId,
                    Content = commentModel.Comment,
                    // това идва от hidden полето
                    DealId = commentModel.DealId,
                    DateAdded = DateTime.Now,
                    PotrebitelID = gosho,

                });

                this.Data.SaveChanges();

                //Създаваме нов модел за PartialView - to
                var viewModel = new CommentViewModel { AuthorUsername = username, Content = commentModel.Comment, DateAdded = DateTime.Now, PotrebitelID = commentModel.PotrebitelID };

                //return RedirectToAction("Index");

                return PartialView("_CommentPartial", viewModel);
            }

            //Връщаме грешка която чрез OnFailure функцията я прехващаме в AJAX заявката която сме пуснали в Details
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ModelState.Values.First().ToString());
        }


        public JsonResult GetDealData(string text)
        {
            var result = this.Data.Deals.All()
                .Where(x => x.Title.ToLower().Contains(text.ToLower()))
                .Select(x => new
                {
                    Title = x.Title
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Search(SubmitSearchDeal text)
        {
            var result = this.Data.Deals.All()
                .Where(x => x.Title.ToLower().Contains(text.DealSearch.ToLower()))
                .Select(x => new DealViewModel
                {
                    Id = x.Id,
                    ImageUrl = x.HeadImage,
                    Price = x.Price,
                    Title = x.Title
                });

            return View(result);
        }

        [HttpPost]
        public ActionResult MessageCheck()
        {
            var curuser = this.User.Identity.Name;

            var result = this.Data.Messages.All().Where(x => x.MessageTo == curuser && x.MessageRead == false).Count();

            if(result > 0)
            {
                return Json("chamare", JsonRequestBehavior.AllowGet);
            }

            return View();
        }

        

        //Странициране
        public ActionResult All(string id, int? id2)
        {


            var result = this.Data.Deals.All().Where(x => x.Topic == id)
                .OrderByDescending(x => x.DateAdded)
                .Select(x => new DealViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImageUrl = x.HeadImage,
                    Price = x.Price

                });





            int pageNumber = id2.GetValueOrDefault(1);

            var viewModel = result.Skip((pageNumber - 1) * PageSize).Take(PageSize);
            //Използваме закръгляне (Ceiling), ако резултата е 12.2 то всъщност подаваме стойност 13 Пример ( 14 / 12 = 2)
            ViewBag.Pages = Math.Ceiling((double)result.Count() / PageSize);

            return View(viewModel);
        }

    }
}