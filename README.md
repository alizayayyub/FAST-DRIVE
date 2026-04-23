Fast Drive – Ride Sharing Management System
Fast Drive is a full-stack web application built using ASP.NET Core Razor Pages and SQL Server.
It simulates a ride-sharing system where customers can book journeys managed by drivers and vehicles, with automated payment handling.
________________________________________
 Features
•	 Customer Management (Create, Read, Update, Delete) 
•	 Driver Management 
•	 Vehicle Management 
•	 Journey Management (linking customer, driver, vehicle) 
•	 Payment Automation using SQL Trigger 
•	 Data filtering, sorting, and analytics queries 
________________________________________
 Tech Stack
•	Frontend: ASP.NET Core Razor Pages 
•	Backend: C# (.NET) 
•	Database: SQL Server 
•	Tools: Visual Studio / VS Code 
________________________________________
 Database Design
The system uses a relational database with the following tables:
•	Customer 
•	Driver 
•	Vehicle 
•	Journey 
•	Payment 
 Relationships:
•	A Journey is linked to: 
o	Customer (customer_id) 
o	Driver (driver_id) 
o	Vehicle (vehicle_id) 
•	Payment is linked to Journey 
________________________________________
 Key Functionalities
 Journey Management
•	Create and manage journeys 
•	Track pickup and drop-off locations (stored as city names) 
•	Store journey status (Completed, Pending, etc.) 
 Payment Automation
•	SQL Trigger automatically creates a payment record when a journey starts 
SQL Queries Implemented
•	JOIN queries across multiple tables 
•	Aggregation (SUM, COUNT, AVG) 
•	Filtering using WHERE clause 
•	Subqueries and GROUP BY 
________________________________________
How to Run the Project
1.	Clone the repository: 
git clone https://github.com/your-username/fast-drive.git
2.	Open project in Visual Studio / VS Code 
3.	Update connection string in: 
appsettings.json
Example:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;"
}
4.	Run the project: 
dotnet run
________________________________________
 Sample SQL Trigger
CREATE TRIGGER paymentupdate
ON Journey
AFTER UPDATE
AS
BEGIN
    INSERT INTO Payment (journey_id, amount)
    SELECT journey_id, cost
    FROM inserted
    WHERE start_time IS NOT NULL;
END;
________________________________________
Challenges Faced
•	Fixed SQL Server connection timeout issues 
•	Resolved data type mismatches (geometry → string conversion) 
•	Handled Razor Pages routing and binding errors 
•	Debugged casting issues (double → string) 
________________________________________
Learning Outcomes
•	Strong understanding of CRUD operations 
•	Hands-on experience with relational database design (3NF) 
•	Writing optimized SQL queries and triggers 
•	Debugging real-world backend issues 
________________________________________
Author
Alizay Ayyub
________________________________________
Contribute / Feedback
Feel free to fork the repo or suggest improvements!

