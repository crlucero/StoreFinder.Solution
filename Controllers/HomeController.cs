using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Weed.Models;
using System;

namespace Weed.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {   
            return View();
        }

        [HttpGet("/scraper")]
        public ActionResult ScraperIndex()
        {   
            return View();
        }

        [HttpPost("/scraper/results")]
        public ActionResult ShowScraper(string locationName, int number)
        {
            List <Scraper> foundScrapers = Scraper.searchStores(locationName, number);
            return View(foundScrapers);
        }
    }
}
