using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Weed;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

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
            int a = 5;

            List<string> daysOfWeek = new List<string>{monday, tuesday, wednesday, thursday, friday, saturday, sunday};

            int x = 1;
            int z=1;

            while(z <= number)
            {   

                IWebElement foundStores = driver.FindElement(By.XPath("//*[@id='wrap']/div[3]/div[2]/div[2]/div/div[1]/div[1]/div/ul/li[" + x + "]/div/div/div/div[1]/div[2]/div/div[1]/div[1]/div[1]/h3/a"));

                name = foundStores.Text;
                foundStores.Click();

                try
                {
                    IWebElement foundDescription = driver.FindElement(By.CssSelector("#super-container > div > div > div.column.column-beta.sidebar > div.bordered-rail > div.ywidget.js-from-biz-owner > p"));
                    description = foundDescription.Text;
                }

                catch
                {
                    description = "none";
                }

                Scraper newScraper = new Scraper(name, description, schedule);
                int y = 1;
                string tempSchedule = "";
                schedule = "";
                foreach(string day in daysOfWeek)
                try
                {
                        IWebElement openTime = driver.FindElement(By.XPath("//*[@id='super-container']/div/div/div[2]/div[2]/div[1]/table/tbody/tr[" + y + "]/td[1]/span[1]"));
                        IWebElement closeTime = driver.FindElement(By.XPath("//*[@id='super-container']/div/div/div[2]/div[2]/div[1]/table/tbody/tr[" + y + "]/td[1]/span[2]"));
                        tempSchedule = string.Concat(day," ", openTime.Text," - ", closeTime.Text +" -- ");
                        schedule = string.Concat(schedule, tempSchedule);
                }
                catch
                {
                    schedule = "closed";
                }

                newScraper.SetSchedule(schedule);
                allScrapers.Add(newScraper);
                newScraper.Save();
                driver.Navigate().Back();
                

                if(x%10==0)
                {   
                    try
                    {
                        IWebElement nextButton = driver.FindElement(By.XPath("//*[@id='wrap']/div[3]/div[2]/div[2]/div/div[1]/div[1]/div/div[1]/div/div[2]/div/div["+(a+1)+"]/a/div/span"));
                        x=1;
                        nextButton.Click();
                        Thread.Sleep(5000);

                    }
                    catch
                    {
                        
                        IWebElement nextButton = driver.FindElement(By.XPath("//*[@id='wrap']/div[3]/div[2]/div[2]/div/div[1]/div[1]/div/div[1]/div/div[2]/div/div["+(a+2)+"]/a/div/span"));
                        x=1;
                        nextButton.Click();
                        Thread.Sleep(5000);
                    }
                    
                }


                x=x+1;
                z=z+1;

            } 
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
