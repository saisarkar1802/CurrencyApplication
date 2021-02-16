# Shaibal_Sarkar

The repository contains two solutions and one database scripts directory:
a) FetchCurrencyWeb: ASP .Net Core MVC Web application.
b) ExchangeRatesProject: This solution contains two console application and one web API.
c) Database: This contains the scripts for creating the database tables and indexes.

# ExchangeRatesProject
This is a .Net Core Solution consisting of three projects as follows:
# a) CurrencyConsoleApplication: 
      This is a .Net Core console application build to retrieve the currency rates from fixer.io api and calculate the amount based on the rates. The application has two modules one for retrieving the latest and another for retrieving historical rates of a currency. The application is a .Net core console application implementing dependency injection to initialize service class objects and also uses Interfaces to abstract the actual service class.
# b) FetchCurrencyDataJob
      This is also a .Net Core console application which can be scheduled using any scheduler to run everyday, this API fetches the the latest rates for all the currencies available on the fixer.io api and stores it in the database. This application uses Entity Framework Core to execute query execution in the SQL database.
# c) CurrencyExchangeAPI
      This is a Rest WebAPI build in .Net Core Framework which has multiple functions to execute operations like the console application project as well as to retrieve data from the database which is inserted by the Job. Dependency Injection has been used to initialize service classes and dbContext class. This project as well as the FetchCurrencyDataJob uses shared project CurrencyExchangeDAL(also a .Net core class library) for database operations.

# FetchCurrencyWeb
This is created in a seperate solution, just for the sake of testing as this web utilizes WebAPI for different tasks. This is a ASP .Net Core MVC Web Application using Razor pages as UI, this application uses the WebAPI to enable the users to do the currency conversion as well as have a graphical representation of a currency in history based on the data stored by the scheduled job. The graphical representation uses Line Chart from HighCharts(www.highcharts.com) to show the data.
