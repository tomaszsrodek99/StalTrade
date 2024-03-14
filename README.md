# StalTrade
Dedicated web application for managing a company specializing in matrix forging trade.
This application is intended to streamline processes related to invoice management, analyze financial aspects, monitor inventory levels, record sales, and support other business processes by creating a centralized repository for storing data.

## How to run?
1. Download the project from GitHub or clone the repository in the console: git clone https://github.com/tomaszsrodek99/StalTrade.git
2. If there is no reference between the UI and API, set it  

![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/bf6fd12d-cd69-491d-ae02-a35a0f219c6b)

3. Make sure you have a database named StalTrade
4. Change the connection string in the configuration file in the API to your own if it is need
5. Set the startup project to <Multiple startup projects>

![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/aec9d592-7519-40e0-bd1e-150342bae2e6)

6. Install the required NuGet packages manually or by running the following command in the local project directory:   
UI

![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/de07234e-d291-4200-bb43-c766c2d71f24)

API

![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/512d0157-5946-4e1a-ba1a-1b113e61bcf1)

7. If there is a conflict with other projects on the same ports, change the ports in the launchSettings.json files
8. Execute the database migration by entering the following commands in the Package Manager Console: Add-Migration Init and Update-Database, or run a script with sample data
9. After launching the application, add a user using Swagger or, if you ran the database script, log in using the following credentials:
User
Login: user@example.com
Hasło: string

## Structure of the database:
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/45d99048-b027-4a1a-8a9f-970fa1d9a1c2)

## Technologies:
- ASP.NET 6.0
- Entity Framework Core
- LINQ
- JavaScript
- MSSQL Server
- AutoMapper
- Git
- Jwt Token
- REST API

## Swagger
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/d638b454-152f-418e-a026-a5032cce843e)
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/ab2351dd-cca4-4ddf-8736-d3365701651d)
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/81a095c5-b612-4b0b-948c-2cc00b3abf7f)
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/7b418a4f-2df0-46ce-9652-d9e051e0484d)

## Application screenshots
### Login view:
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/5ad55729-fd3c-4e36-8260-aec8a0c534aa)
### Selected tabs:
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/ccf17f14-f442-4b61-bd0c-5f0443409a2d)
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/65af994b-fbea-4345-ac7c-9325165cb205)
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/4917b59c-12da-432c-82c6-a0f6eb38f3c4)
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/65d233a6-69ee-4423-b72b-0f916679c857)
![image](https://github.com/tomaszsrodek99/StalTrade/assets/98595791/c61615d8-7963-47de-9e42-5f8805fb30bf)




