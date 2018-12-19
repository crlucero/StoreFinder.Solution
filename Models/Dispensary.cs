using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Weed;

namespace Weed.Models
{
    public class Dispensary
    {
        private int _license;
        private string _name;
        private string _city;
        private string _address;

    // public static int GetViolationsByLicense(int license)
    // {
    //     return 
    // }

    public Dispensary(string name, string address, int license = 0)
    {
        _name = name;
        _address = address;
        _license = license;
    }
    public Dispensary(string name, string address, string city, int license = 0)
    {
        _name = name;
        _address = address;
        _city = city;
        _license = license;
    }

    public Dispensary(int license = 0)
    {
        _license = license;
    }

    public string GetName()
    {
        return _name;
    }

    public int GetLicense()
    {
        return _license;
    }

    public string GetAddress()
    {
        return _address;
    }

    public string GetCity()
    {
        return _city;
    }

    public static List<Dispensary> GetAll()
        {
            List<Dispensary> allDispensarys = new List<Dispensary> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM dispensaries;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int license = rdr.GetInt32(2);
                string name = rdr.GetString(0);
                string address = rdr.GetString(5);
                string city = rdr.GetString(7);
                
                Dispensary newDispensary = new Dispensary(name, address, city, license);
                allDispensarys.Add(newDispensary);
            }
            conn.Close();
            if (conn !=null)
            {
                conn.Dispose();
            }
            return allDispensarys;
        }
    public static List<Dispensary> GetAddresses(string cityName)
        {
            List<Dispensary> allAddresses = new List<Dispensary> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM dispensaries WHERE City=@cityName;";
            cmd.Parameters.AddWithValue("@cityName", cityName);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int license = rdr.GetInt32(2);
                string name = rdr.GetString(0);
                string address = rdr.GetString(5);
                string city = rdr.GetString(7);
                Dispensary newAddress = new Dispensary(name, address, city, license);
                allAddresses.Add(newAddress);
            }
            conn.Close();
            if (conn !=null)
            {
                conn.Dispose();
            }
            return allAddresses;
        }

        public static Dispensary FindByLicense(int license)
         {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM dispensaries WHERE License = (@searchLicense);";
            MySqlParameter searchLicense = new MySqlParameter();
            searchLicense.ParameterName = "@searchLicense";
            searchLicense.Value = license;
            cmd.Parameters.Add(searchLicense);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int DispensaryLicense = 0;
            string DispensaryName = "";
            string DispensaryAddress = "";
            

            while(rdr.Read())
            {
                DispensaryLicense = rdr.GetInt32(2);
                DispensaryName = rdr.GetString(0);
                DispensaryAddress = rdr.GetString(5) + " " + rdr.GetString(6) + ", " + rdr.GetString(7) + ", " + rdr.GetString(8) + ", " + rdr.GetString(9);
            }

            Dispensary newDispensary = new Dispensary(DispensaryName, DispensaryAddress, DispensaryLicense);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newDispensary;
        }
        
        public static List<Dispensary> FindByName(string storeName)
         {
            List<Dispensary> allDispensaries = new List<Dispensary>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM dispensaries WHERE Organization LIKE '%" + storeName + "%';";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int DispensaryLicense = rdr.GetInt32(2);
                string DispensaryName = rdr.GetString(0);
                string DispensaryAddress = rdr.GetString(5);
                Dispensary newDispensary = new Dispensary(DispensaryName, DispensaryAddress, DispensaryLicense);
                allDispensaries.Add(newDispensary);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDispensaries;
        }

        public static List<Dispensary> FindByCity(string city)
         {
             List<Dispensary> allDispensaries = new List<Dispensary> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM dispensaries WHERE City = (@searchCity);";
            MySqlParameter searchCity = new MySqlParameter();
            searchCity.ParameterName = "@searchCity";
            searchCity.Value = city;
            cmd.Parameters.Add(searchCity);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            
            string DispensaryName = "No Matches Found!";
            string DispensaryAddress = "";
            string DispensaryCity = "Seattle, Wa";
            int License = 0;
            

            while(rdr.Read())
            {
                
                DispensaryName = rdr.GetString(0);
                DispensaryAddress = rdr.GetString(5);
                DispensaryCity = rdr.GetString(8);
                License = rdr.GetInt32(2);
                Dispensary newDispensary = new Dispensary(DispensaryName, DispensaryAddress, DispensaryCity, License);
                allDispensaries.Add(newDispensary);
            }

           
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDispensaries;
        }
    }
}
