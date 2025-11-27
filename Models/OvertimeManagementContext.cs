using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Cladtek_Interview.Models
{ 
    [Table("Department", Schema = "public")]
    public class Department
    {
        [Key]
        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("department_name")]
        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }

    [Table("Employee", Schema = "public")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [Column("nik")]
        [Required(ErrorMessage = "NIK is required")]
        [StringLength(20)]
        [Index(IsUnique = true)]
        public string NIK { get; set; }

        [Column("employee_name")]
        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(150)]
        public string EmployeeName { get; set; }

        [Column("department_id")]
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        [Column("position")]
        [Required(ErrorMessage = "Position is required")]
        [StringLength(50)]
        public string Position { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Column("has_laptop")]
        public bool HasLaptop { get; set; }

        [Column("has_meal_allowance")]
        public bool HasMealAllowance { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Overtime> Overtimes { get; set; }
    }

    [Table("Overtime", Schema = "public")]
    public class Overtime
    {
        [Key]
        [Column("overtime_id")]
        public int OvertimeId { get; set; }

        [Column("employee_id")]
        [Required]
        public int EmployeeId { get; set; }

        [Column("overtime_date")]
        [Required]
        public DateTime OvertimeDate { get; set; }

        [Column("time_start")]
        [Required]
        public DateTime TimeStart { get; set; }

        [Column("time_finish")]
        [Required]
        public DateTime TimeFinish { get; set; }

        [Column("actual_ot_hours")]
        [Required]
        public decimal ActualOTHours { get; set; }

        [Column("calculated_ot_hours")]
        public decimal CalculatedOTHours { get; set; }

        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
     
    public class OvertimeManagementContext : DbContext
    {
        public OvertimeManagementContext() : base("OvertimeManagementDb")
        { 
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<OvertimeManagementContext>(null);
            Database.CommandTimeout = 30;
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Overtime> Overtimes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurasi untuk PostgreSQL
            modelBuilder.Entity<Department>().ToTable("Department", "public");
            modelBuilder.Entity<Employee>().ToTable("Employee", "public");
            modelBuilder.Entity<Overtime>().ToTable("Overtime", "public");
             
            modelBuilder.Entity<Employee>()
                .Property(e => e.CreatedDate);

            modelBuilder.Entity<Employee>()
                .Property(e => e.ModifiedDate);

            modelBuilder.Entity<Overtime>()
                .Property(o => o.CreatedDate);

            modelBuilder.Entity<Overtime>()
                .Property(o => o.ModifiedDate);

            modelBuilder.Entity<Department>()
                .Property(d => d.DepartmentName);

            modelBuilder.Entity<Employee>()
                .Property(e => e.NIK);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeName);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Position);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email);

            modelBuilder.Entity<Overtime>()
                .Property(o => o.Description);
        }

        public static OvertimeManagementContext Create()
        {
            return new OvertimeManagementContext();
        }
    }
}