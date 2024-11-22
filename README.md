Project Overview
The Contract Monthly Claim System is a web-based application designed for lecturers to submit their monthly work claims. The system allows administrators to manage these claims by approving or rejecting them. The platform simplifies the workflow for claim submissions, document management, and status tracking.

Features
Lecturer Dashboard:

Submit claims for the hours worked.
Upload supporting documents (e.g., invoices, timesheets).
View feedback on submitted claims.
Admin Dashboard:

View all submitted claims in a structured table format.
Approve or reject claims based on the provided information.
Generate reports for specific claims.
View supporting documents uploaded by lecturers.
Dynamic Calculation:

Automatic calculation of the total claim amount based on the hours worked and hourly rate.
User Notifications:

Admins can provide immediate feedback on the claim status.
Success or error messages are displayed for specific actions.
Technologies Used
Frontend:

Razor Views (ASP.NET Core)
HTML5, CSS3, Bootstrap 5 for styling and responsiveness.
Backend:

ASP.NET Core MVC Framework
C# for business logic and model handling.
Database:

(Optional: SQL Server or any in-memory storage for claim records, not implemented in the pasted code).
Folder Structure
markdown
Copy code
Project
│
├── Controllers
│   └── AdminController.cs
│
├── Models
│   └── ClaimViewModel.cs
│
├── Services
│   └── ClaimService.cs
│
├── Views
│   ├── Admin
│   │   ├── ManageClaims.cshtml
│   └── Lecturer
│       ├── Dashboard.cshtml
│
└── wwwroot
    ├── css
    │   └── site.css
    ├── js
    │   └── site.js
    ├── lib
        └── Bootstrap (External library for styling)
Setup Instructions
Clone the Repository:

bash
Copy code
git clone https://github.com/your-repo/contract-monthly-claim-system.git
cd contract-monthly-claim-system
Install Dependencies:

Ensure you have .NET SDK installed.
Restore NuGet packages:
bash
Copy code
dotnet restore
Run the Application:

bash
Copy code
dotnet run
Access the Application:

Open your browser and navigate to:
arduino
Copy code
http://localhost:5000
Replace 5000 with your assigned port if different.
Code Description
1. Lecturer Dashboard (Dashboard.cshtml)
A lecturer can:
Enter the number of hours worked.
Specify the hourly rate and month of the claim.
Upload supporting documentation (optional).
The claim form is submitted to the SubmitClaim action in the LecturerController.
2. Admin Dashboard (ManageClaims.cshtml)
Admins can:
View a table listing all submitted claims with details such as:
Lecturer name, department, hours worked, monthly claim total, and status.
Approve or reject claims using the buttons in the "Actions" column.
View uploaded documents or see "No Document" if none were provided.
3. Controller (AdminController.cs)
Handles all admin-side functionalities:
Retrieves all claims using the ClaimService.
Updates claim statuses based on admin input (approve/reject actions).
Uses TempData for success messages after updating a claim.
4. Model (ClaimViewModel.cs)
Represents the structure of a claim, including:
Id: Unique identifier for the claim.
LecturerName: Name of the lecturer submitting the claim.
HoursWorked: Number of hours worked in the month.
HourlyRate: Payment rate per hour.
TotalClaim: Automatically calculated as HoursWorked * HourlyRate.
SupportingDocument: URL or path to the uploaded file.
5. Service (ClaimService.cs)
Manages claims data storage and retrieval.
Provides:
Methods to fetch all claims.
Methods to update the status of a specific claim (e.g., Approved/Rejected).
Usage Instructions
For Lecturers:

Navigate to the dashboard.
Fill out the claim form with all required details.
Upload supporting documentation (if available).
Submit the claim.
For Admins:

Log in to the admin dashboard.
View submitted claims in the table.
Approve or reject claims using the buttons in the "Actions" column.
Optionally, view the uploaded supporting documents.
Future Enhancements
Add authentication and role-based authorization for:
Lecturers (restricted to their claims only).
Admins (full claim management).
Implement a database (e.g., SQL Server) for persistent claim storage.
Provide email notifications for claim status updates.
Add filtering and search functionality for claims in the admin dashboard.
Contributors
Your Name (Project Lead)
Add more contributors as necessary.
