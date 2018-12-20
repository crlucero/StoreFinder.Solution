using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Weed.Models;

namespace Weed.Controllers
{
    public class DispensaryController : Controller
    {
        [HttpGet("/dispensaries")]
        public ActionResult Index()
        {
            string cityName ="Seattle";
            Dictionary<string,object> model = new Dictionary<string,object>{};
            List<Dispensary> allDispensaries = Dispensary.GetAll();
            List<Dispensary> allAddresses = Dispensary.GetAddresses(cityName);
            model.Add("dispensaries", allDispensaries);
            model.Add("addresses", allAddresses);
            return View(model);
        }

        [HttpGet("/dispensaries/{license}")]
        public ActionResult Show(int license)
        {
            List<Comment> foundComments = Comment.GetCommentsByLicense(license);
            Dispensary foundDispensary = Dispensary.FindByLicense(license);
            string name = foundDispensary.GetName();
            Scraper foundScraper = Scraper.FindByName(name);
            Dictionary<string, object> model = new Dictionary<string, object> {};
            model.Add("dispensary", foundDispensary);
            model.Add("comments", foundComments);
            model.Add("scraper", foundScraper);
            return View(model);
        }

        [HttpPost("/dispensaries/searchbyname")]
        public ActionResult SearchName(string storeName)
        {  
            List<Dispensary> allDispensaries = Dispensary.FindByName(storeName);
            
            return View(allDispensaries); 
        }

        [HttpPost("/dispensaries/searchbycity")]
        public ActionResult SearchCity(string city)
        {   
            Dictionary<string, object> model = new Dictionary<string, object>{};
            List<Dispensary> allDispensaries = Dispensary.FindByCity(city);
            model.Add("dispensaries", allDispensaries);
            model.Add("city", city);
            return View(model);
        }

        [HttpPost("/dispensaries/{license}/comments")]
        public ActionResult CreateComment(int license, string review, int rating)
        {
            Comment newComment = new Comment(review, rating, license);
            newComment.Save();
            List<Comment> foundComments = Comment.GetCommentsByLicense(license);
            Dispensary foundDispensary = Dispensary.FindByLicense(license);
            Dictionary<string, object> model = new Dictionary<string, object> {};
            string name = foundDispensary.GetName();
            Scraper foundScraper = Scraper.FindByName(name);
            model.Add("dispensary", foundDispensary);
            model.Add("comments", foundComments);
            model.Add("scraper", foundScraper);
            return View("Show", model); 
        }
    }
}
