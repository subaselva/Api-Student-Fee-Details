🎓 Student Fee Management System

A role-based fee management system built with ASP.NET Core (Web API) and Blazor WebAssembly.
This system manages student details, student fee details, and provides different features for CEO and Accountant roles.

🚀 Features
🔑 Authentication & Authorization

JWT Authentication used for secure login.

Two roles: CEO and Accountant.

CEO email is fixed in the system.

CEO registers first → assigns Accountant role.

Only registered users can log in.

👨‍💼 CEO Role

User Management (via CEO Dashboard)

Add, update, and delete user roles.

Approve/reject Accountant activities.

View Audit Logs (track logins and user actions).

Student & Fee Management

Add, update, and delete student details directly.

View student details and fee records.

Approve/reject Accountant’s edits before saving to the database.

Recovery System

Recover deleted items.

Hangfire is used for background jobs such as backup and recovery.

Calendar System

Define due dates for 1st, 2nd, and 3rd term fees.

View student fee details for each specific term.

Dashboard

Overview of total students, pending fees, and pending approvals.

🧾 Accountant Role

Enter student details and fee details.

Edit and delete student fee records (subject to CEO approval).

View student details.

Export student details to Excel.

📊 Dashboard Overview

Total Students

Total Pending Fees

Pending Approvals (waiting for CEO action)

⚙️ Technologies Used

ASP.NET Core 9 (Web API) – Backend

Blazor WebAssembly – Frontend

Entity Framework Core – Database ORM

PostgreSQL / SQL Server – Database

JWT Authentication – Secure login

Hangfire – Background jobs (backup, recovery)

Excel Export – Student details export
