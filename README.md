# MVCMarketing - ERP Distribution Management System

## ?? Project Overview
**MVCMarketing** is a comprehensive Enterprise Resource Planning (ERP) system built with ASP.NET MVC 5, designed for distribution and marketing management. The system handles complete business operations including inventory, distributor management, sales, expenses, and multi-level organizational workflows.

## ??? Technology Stack
- **Framework:** ASP.NET MVC 5
- **Target Framework:** .NET Framework 4.7.2
- **C# Version:** 7.3
- **Database:** SQL Server (ADO.NET with Stored Procedures)
- **Frontend:** jQuery, jQuery DataTables, Bootstrap, AdminLTE
- **Architecture:** Traditional 3-Tier Architecture

## ? Key Features

### ?? Multi-Role Authentication System
14 distinct user roles with dedicated dashboards:
- Admin
- Account Manager
- Operation Manager
- Project Coordinator
- Project Designing
- Project Manager
- Sales Field
- Sales Head
- Sales Office
- Site Supervisor
- Store Manager
- Back Office
- Distributor
- Transport

### ?? Core Modules
- **Distributor Management** - Complete CRUD, credit limits, discount structures, route assignments
- **Inventory Management** - Item master, categories, brands, stock tracking
- **Sales & Distribution** - Order processing, route-based distribution, field visits
- **Financial Management** - Expenses, payments, invoices, credit/debit notes
- **Bulk Import System** - Excel-based data import for 15+ modules
- **Employee Management** - Multi-designation support with profile assignments
- **Reporting System** - Role-specific dashboards and comprehensive reports

## ?? Getting Started

### Prerequisites
- Visual Studio 2017 or later
- SQL Server 2014 or later
- .NET Framework 4.7.2
- IIS (for deployment)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/devpatel22112004/ERP-Client-and-Distributor.git
   cd ERP-Client-and-Distributor
   ```

2. **Configure Database Connection**
   - Open `MVCMarketing\Web.config`
   - Update the connection string:
     ```xml
     <connectionStrings>
       <add name="SqlConn" 
            connectionString="Data Source=YOUR_SERVER;Initial Catalog=EzzworkMarketing;User ID=YOUR_USER;Password=YOUR_PASSWORD;" 
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```

3. **Setup Firebase Credentials (Optional)**
   - Copy `MVCMarketing\FirebaseJson\adminsdk.json.template` to `adminsdk.json`
   - Add your Firebase service account credentials

4. **Run Database Scripts**
   - Execute the stored procedures required for the application
   - Create the `EzzworkMarketing` database

5. **Build and Run**
   ```bash
   # Open in Visual Studio
   # Build the solution (Ctrl+Shift+B)
   # Run (F5)
   ```

## ?? Project Structure
```
MVCMarketing/
??? Controllers/          # MVC Controllers (20+ modules)
??? Models/              # Data models and bulk insert classes
??? Views/               # Razor views with 14 role-specific layouts
??? Common/              # SessionTimeoutAttribute and utilities
??? Configuration/       # Web API and routing configuration
??? App_Start/           # Application startup configurations
??? plugins/             # jQuery plugins, DataTables, FullCalendar
??? dist/                # Static assets (CSS, JS, images)
```

## ?? Security Features
- Parameterized queries (SQL injection protection)
- Session-based authentication
- Role-based access control
- Account locking mechanism
- Custom error handling
- Firebase credentials excluded from repository

## ?? Database Architecture
- **100% Stored Procedure based** - No inline SQL
- **ADO.NET SqlCommand** - Direct SQL Server connection
- Pagination support for large datasets
- Action-based stored procedures (SELECT, INSERT, UPDATE, DELETE)

## ?? Business Domain
This system is designed for:
- Distribution network management
- Marketing and sales operations
- Multi-channel sales tracking
- Field operations management
- Financial transaction handling
- Inventory and warehouse management

## ?? Configuration

### App Settings (Web.config)
```xml
<add key="URL_DOCUMENT_CHALLAN" value="DOCUMENT/STORE-OUTWARD" />
<add key="URL_EXCEL_BULKINSERT" value="DOCUMENT/BULKINSERT" />
<add key="URL_IMAGES_ITEM" value="DOCUMENT/IMAGES-ITEM" />
```

### System Settings
- **Max Request Length:** 1GB (supports large file uploads)
- **Execution Timeout:** 500 seconds
- **Session Timeout:** 2000 minutes (configurable)

## ?? Contributing
Contributions, issues, and feature requests are welcome!

## ?? Contact
**Developer:** Dev Patel  
**GitHub:** [@devpatel22112004](https://github.com/devpatel22112004)

## ?? License
This project is proprietary software. All rights reserved.

## ?? Important Notes
- Keep `adminsdk.json` secure and never commit to repository
- Update connection strings before deployment
- Review and update stored procedures as needed
- Configure IIS properly for production deployment

---
**Built with ?? using ASP.NET MVC**
