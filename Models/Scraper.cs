using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Weed;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace Weed.Models
{
    public class Scraper
    {
        private int _id;
        private string _name;
        private string _description;
        private string _schedule;

        public static ChromeDriver driver = new ChromeDriver("/Users/Guest/Desktop/CS-StoreFinder");


        public Scraper(string name, string description, string schedule, int id =0) 
        {
            _id = id;
            _name = name;
            _description = description;
            _schedule = schedule;
        }

        public string GetDescription()
        {
            return _description;
        }

        public void SetDescription(string description)
        {
            _description = description;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetSchedule(string schedule)
        {
            _schedule = schedule;
        }
        public string GetSchedule()
        {
            return _schedule;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO scraper (storeName, description, schedule) VALUES (@storeName, @description, @schedule);";
            cmd.Parameters.AddWithValue("@storeName", this._name);
            cmd.Parameters.AddWithValue("@schedule", this._schedule);
            cmd.Parameters.AddWithValue("@description", this._description);
            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Scraper> searchStores(string locationName, int number)
        {   
            List<Scraper> allScrapers = new List<Scraper> {};
            driver.Navigate().GoToUrl("http://www.yelp.com");
            IWebElement searchField = driver.FindElement(By.CssSelector("#find_desc")); 
            searchField.SendKeys("Cannabis Dispensaries");
            IWebElement searchLocation = driver.FindElement(By.CssSelector("#dropperText_Mast")); 
            searchLocation.Clear();
            searchLocation.SendKeys(locationName);
            searchLocation.Submit();
            
            string monday = "Monday";
            string tuesday = "Tuesday";
            string wednesday = "Wednesday";
            string thursday = "Thursday";
            string friday = "Friday";
            string saturday = "Saturday";
            string sunday = "Sunday";

            string name = "blah";
            string description = "description";
            string schedule = "no schedule";

            List<string> daysOfWeek = new List<string>{monday, tuesday, wednesday, thursday, friday, saturday, sunday};


            for(int x = 1; x <= number ; x++)
            {   

                IWebElement foundStores = driver.FindElement(By.CssSelector("#wrap > div:nth-child(4) > div.lemon--div__373c0__6Tkil.spinner-container__373c0__N6Hff.border-color--default__373c0__2oFDT > div.lemon--div__373c0__6Tkil.container__373c0__13FCe.space3__373c0__DeVwY > div > div.lemon--div__373c0__6Tkil.mainContentContainer__373c0__32Mqa.arrange__373c0__UHqhV.gutter-30__373c0__2PiuS.border-color--default__373c0__2oFDT > div.lemon--div__373c0__6Tkil.mapColumnTransition__373c0__10KHB.arrange-unit__373c0__1piwO.arrange-unit-fill__373c0__17z0h.border-color--default__373c0__2oFDT > div > ul > li:nth-child(" + x + ") > div > div > div > div.lemon--div__373c0__6Tkil.arrange__373c0__UHqhV.border-color--default__373c0__2oFDT > div.lemon--div__373c0__6Tkil.arrange-unit__373c0__1piwO.arrange-unit-fill__373c0__17z0h.border-color--default__373c0__2oFDT > div > div.lemon--div__373c0__6Tkil.mainAttributes__373c0__1r0QA.arrange-unit__373c0__1piwO.arrange-unit-fill__373c0__17z0h.border-color--default__373c0__2oFDT > div:nth-child(1) > div.lemon--div__373c0__6Tkil.businessName__373c0__1fTgn.border-color--default__373c0__2oFDT > h3 > a"));
                name = foundStores.Text;
                foundStores.Click();

                IWebElement foundDescription = driver.FindElement(By.CssSelector("#super-container > div > div > div.column.column-beta.sidebar > div.bordered-rail > div.ywidget.js-from-biz-owner > p"));
                description = foundDescription.Text;

                Scraper newScraper = new Scraper(name, description, schedule);
                int y = 1;
                string tempSchedule = "";
                schedule = "";
                foreach(string day in daysOfWeek)
                {
                    IWebElement openTime = driver.FindElement(By.XPath("//*[@id='super-container']/div/div/div[2]/div[2]/div[1]/table/tbody/tr[" + y + "]/td[1]/span[1]"));

                    IWebElement closeTime = driver.FindElement(By.XPath("//*[@id='super-container']/div/div/div[2]/div[2]/div[1]/table/tbody/tr[" + y + "]/td[1]/span[2]"));
                    y = y+1;
                    tempSchedule = string.Concat(day," ", openTime.Text," - ", closeTime.Text +" -- ");
                    schedule = string.Concat(schedule, tempSchedule);

                }
                newScraper.SetSchedule(schedule);
                allScrapers.Add(newScraper);
                newScraper.Save();
                driver.Navigate().Back();
            } 
            driver.Quit();
            return allScrapers;

        } 

        public static Scraper FindByName(string storeName)
         {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM scraper WHERE (storeName) LIKE '%" + storeName + "%';";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string name = "";
            string description = "";
            string schedule = "";

            while(rdr.Read())
            {
                name = rdr.GetString(1);
                description = rdr.GetString(3);
                schedule = rdr.GetString(2);
                id = rdr.GetInt32(0);
            }

            Scraper newScraper = new Scraper(name, description, schedule);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newScraper;
        }
    }
}
