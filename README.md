🎓 **Student Fee Management System**

A role-based fee management system built with ASP.NET Core (Web API) and Blazor WebAssembly.
This system manages student details and student fee records, with separate permissions for CEO and Accountant roles.
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## 🚀 Features

# 🔑 Authentication & Authorization
- JWT Authentication used for secure login
- Roles: CEO and Accountant
- CEO email is fixed in the system
- CEO registers first → assigns Accountant role
- Only registered users can log in

# 👨‍💼 CEO Role
- Manage user roles (Add, Update, Delete)
- Approve or reject Accountant activities (via Dashboard)
- View Audit Logs (track logins & user actions)
- Manage Student & Fee records directly
- Approve Accountant’s edits before saving to DB
- Recover deleted items (via Hangfire background jobs)
- Set term fee dates (1st, 2nd, 3rd) in Calendar
- View fee details by term
- Dashboard: Total Students, Pending Fees, Pending Approvals

# 🧾 Accountant Role
- Enter Student & Fee details
- Edit/Delete fee records (requires CEO approval)
- View Student details
- Export student details to Excel

# 📊 Dashboard Overview
- Total Students
- Total Pending Fees
- Pending Approvals (waiting for CEO action)

⚙️ Technologies Used
- ASP.NET Core 9 (Web API)      → Backend
- Blazor WebAssembly            → Frontend
- Entity Framework Core         → ORM
- PostgreSQL / SQL Server       → Database
- JWT Authentication            → Secure login
- Hangfire                      → Background jobs (backup & recovery)
- Excel Export                  → Student details export

🏗️ System Design
Frontend   →  Blazor WebAssembly (WASM)
Backend    →  ASP.NET Core Web API
Database   →  PostgreSQL / SQL Server
Auth       →  JWT (JSON Web Token)
Jobs       →  Hangfire (background tasks)

# Workflow
1. CEO registers first → assigns Accountant
2. Accountant enters Student & Fee details
3. CEO approves/rejects Accountant edits
4. CEO can also directly manage Student & Fee details
5. Deleted items can be recovered via Recovery System
6. Dashboard shows quick stats

         ┌──────────────────────────┐
         │        Blazor WASM       │
         │  (Frontend - Client)     │
         └─────────────┬────────────┘
                       │
                       ▼
         ┌──────────────────────────┐
         │   ASP.NET Core Web API   │
         │ (Backend - Business Logic) │
         └─────────────┬────────────┘
                       │
         ┌─────────────▼─────────────┐
         │  Entity Framework Core    │
         │   (ORM - Data Access)     │
         └─────────────┬─────────────┘
                       │
         ┌─────────────▼─────────────┐
         │   PostgreSQL / SQLServer  │
         │   (Relational Database)   │
         └───────────────────────────┘

 Background Jobs: Hangfire → Recovery / Backup  
 Authentication: JWT → Secure Role-Based Access
