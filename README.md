#### Dispensary Finder and Scraper 12/20/18

#### By **Daniel Lira, Kenny Wolfenberger, Cristian Lucero, Maxwell Dubin**

## Description

Allows users to search Database for Dispensaries by Location and Name. Also allows for database updates through a web scraper.

### Specs

* User inputs a search term (location/store name)
* Program will return list of stores that meet those parameters
* Allows user to find top stores results from yelp through location.
* Users can click on results to see individual shop details including dispensary license, description, shop hours, and reviews. 
* Allows a user to leave reviews to individual stores.
* Includes dynamic map that shows store location using HERE MAPS

## Setup/Installation Requirements

1. Clone this repository.
2. From the command line, navigate to StoreFinder.Solution or Click this Link 
   {https://github.com/kwolfenb/Store-Finder.git}
3. From the command line, enter _dotnet restore_ to install necessary packages
4. Import the Weed.Sql database using PhpMyAdmin
5. In the Model file "scraper.cs" on line 20 is a filepath referencing the chromedriver.exe file (included). Update this filepath to match the location where you downloaded this repository.
6. The ChromeDriver included in this repository is for Mac. If you are on a Windows machine download a new copy of the ChromeDriver here: http://chromedriver.chromium.org/downloads. 
6. From the command line, enter _dotnet build_
7. From the command line, enter _dotnet run_
8. In the Chrome browser, navigate to localhost:5000
9. When done using the program, enter _Ctrl + C_ in the command line to exit 

## Known Bugs
* Scraper can crash due to unpredictable xpath variations.

## Technologies Used
* HTML
* CSS
* Bootstrap
* C#
* Selenium WebDriver
* ASP.Net Core MVC
* SQL
* HereMaps API
* JavaScript
* Razor

## Support and Contact Details


_Daniel Lira: devidra87@gmail.com_

_Kenny Wolfenberger: kennywolfenberger@gmail.com_

_Cristian Lucero: cristianjlucero32@gmail.com_

_Maxwell Dubin: maxdhs@gmail.com_

### License

*MIT License*

Copyright (c) 2018 **_Daniel Lira, Kenny Wolfenberger, Cristian Lucero, Maxwell Dubin_**
