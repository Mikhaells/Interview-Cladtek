# Overtime Management System

An ASP.NET MVC application designed to manage employee overtime records, departments, and generate overtime calculations for payroll purposes.

## Overview

This system provides a centralized platform for:
- Managing employee data and departmental information
- Recording and tracking employee overtime entries
- Calculating overtime hours with a maximum limit of 3 hours per entry
- Generating reports on overtime usage across departments

## Features

### Dashboard
- Total employee count
- Total department count
- Total overtime entries
- Total overtime hours summary
- Recent overtime entries display

### Employee Management
- Create new employee records with unique NIK (employee ID)
- Edit employee information
- Delete employees (with validation to prevent deletion of employees with overtime records)
- Track employee benefits including laptop and meal allowances
- Position-based allowance restrictions:
  - Laptop allowance: Available for Supervisor and Manager positions only
  - Meal allowance: Available for all positions except Manager

### Overtime Management
- Create overtime entries with employee selection
- Automatic calculation of actual overtime hours from start and finish times
- Calculated overtime hours multiplied by 2 for payroll purposes
- Maximum overtime limit: 3 hours per entry
- Edit existing overtime records
- Delete overtime entries
- Overtime date tracking and history

### Data Management
- Pagination support for large datasets (10 records per page)
- Data validation and error handling
- Anti-forgery token protection for form submissions
- Relational database with proper foreign key constraints

## Technology Stack

### Backend
- **Framework**: ASP.NET MVC 5
- **Database**: PostgreSQL
- **ORM**: Entity Framework 6
- **Validation**: Data Annotations

### Frontend
- **CSS Framework**: Bootstrap 3.3.7
- **Template Engine**: Razor
- **UI Theme**: AdminLTE 2.4.18
- **Icons**: Font Awesome 4.7.0
- **Date/Time Picker**: Bootstrap DatetimePicker 4.17.47
- **Pagination**: PagedList

## Database Schema

### Department Table
```
- DepartmentId (Primary Key)
- DepartmentName (Required, Max 100 chars)
```

### Employee Table
```
- EmployeeId (Primary Key)
- NIK (Required, Unique, Max 20 chars)
- EmployeeName (Required, Max 150 chars)
- DepartmentId (Foreign Key)
- Position (Required, Max 50 chars)
- Email (Required, Email format)
- HasLaptop (Boolean, Default: false)
- HasMealAllowance (Boolean, Default: false)
- CreatedDate (DateTime)
- ModifiedDate (DateTime, Nullable)
```

### Overtime Table
```
- OvertimeId (Primary Key)
- EmployeeId (Foreign Key)
- OvertimeDate (Required)
- TimeStart (Required)
- TimeFinish (Required)
- ActualOTHours (Required, Decimal)
- CalculatedOTHours (Decimal - ActualOTHours × 2)
- Description (Max 500 chars)
- CreatedDate (DateTime)
- ModifiedDate (DateTime, Nullable)
```

## Installation & Setup

### Prerequisites
- .NET Framework 4.7.2+
- Visual Studio 2019 or later
- PostgreSQL 10+
- NuGet Package Manager

### Database Setup

1. Create PostgreSQL database:
```sql
CREATE DATABASE OvertimeManagementDb;
```

2. Execute the SQL script located in `PostgreSQL Script/script.txt` to create tables and insert sample data.

3. Update connection string in `Web.config`:
```xml
<connectionStrings>
    <add name="OvertimeManagementDb"
         connectionString="Server=localhost;Port=5432;Database=OvertimeManagementDb;User Id=postgres;Password=postgres;"
         providerName="Npgsql" />
</connectionStrings>
```

### Application Setup

1. Clone or download the project
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run the application (F5 or Ctrl+F5)

## Project Structure

```
├── App_Start/
│   ├── BundleConfig.cs          (CSS/JS bundling configuration)
│   ├── FilterConfig.cs          (Global filter configuration)
│   └── RouteConfig.cs           (Route configuration)
│
├── Controllers/
│   ├── HomeController.cs        (Dashboard)
│   ├── EmployeeController.cs    (Employee CRUD operations)
│   └── OvertimeController.cs    (Overtime CRUD operations)
│
├── Models/
│   └── OvertimeManagementContext.cs  (Database context and entity models)
│
├── Views/
│   ├── Home/
│   │   └── Index.cshtml         (Dashboard view)
│   ├── Employee/
│   │   ├── Index.cshtml         (Employee list)
│   │   ├── Create.cshtml        (Employee creation form)
│   │   └── Edit.cshtml          (Employee edit form)
│   ├── Overtime/
│   │   ├── Index.cshtml         (Overtime list)
│   │   ├── Create.cshtml        (Overtime entry form)
│   │   └── Edit.cshtml          (Overtime edit form)
│   └── Shared/
│       ├── _Layout.cshtml       (Master layout)
│       └── Error.cshtml         (Error page)
│
└── PostgreSQL Script/
    └── script.txt               (Database initialization script)
```

## Usage

### Managing Employees

1. **Add Employee**: Navigate to Employee menu → Click "Add New Employee" button
2. **Edit Employee**: Click the edit icon next to an employee record
3. **Delete Employee**: Click the delete icon (only if employee has no overtime records)

### Managing Overtime

1. **Add Overtime**: Navigate to Overtime Entry menu → Click "Add New Overtime" button
2. **Auto-calculation**: Enter start and finish times for automatic hour calculation
3. **Edit Entry**: Click the edit icon to modify overtime details
4. **Delete Entry**: Click the delete icon to remove an overtime record

### Dashboard Monitoring

The dashboard provides quick statistics:
- Monitor total employees and departments
- Track total overtime entries and hours
- View the 5 most recent overtime entries

## Security Features

- Anti-forgery token protection on all forms
- Input validation using Data Annotations
- Unique constraint on employee NIK field
- Foreign key constraints to maintain data integrity
- Error handling with proper exception management

## Known Limitations

- Maximum overtime per entry: 3 hours
- Pagination limit: 10 records per page
- Single database context per request

## Support & Troubleshooting

### Common Issues

1. **Database Connection Error**: Verify PostgreSQL is running and connection string matches your database configuration
2. **Entity Framework Migration Error**: Ensure database tables are created using the provided SQL script
3. **Page Not Loading**: Check that all CSS/JS CDN links are accessible

## Future Enhancements

- Advanced reporting and analytics
- Email notifications for overtime approvals
- Overtime approval workflow
- Multiple department support with hierarchies
- Export overtime data to Excel/PDF
- User authentication and role-based access control

## Version

**Version**: 1.0  
**Release Date**: 2025

## License

This project is provided as-is for internal use.

## Support

For technical support or inquiries, please contact the development team.