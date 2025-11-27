using Cladtek_Interview.Models; 
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OvertimeManagementApp.Controllers
{
    public class EmployeeController : Controller
    {
        private OvertimeManagementContext db = new OvertimeManagementContext();
        private const int PageSize = 10;

        // GET: Employee
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            var employees = db.Employees
                .Include("Department")
                .OrderByDescending(e => e.CreatedDate)
                .ToPagedList(pageNumber, PageSize);

            return View(employees);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            LoadDepartments();
            var employee = new Employee();
            employee.EmployeeId = 0;
            return View(employee);
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check duplicate NIK
                if (db.Employees.Any(e => e.NIK == employee.NIK))
                {
                    ModelState.AddModelError("NIK", "NIK already exists");
                    LoadDepartments();
                    return View(employee);
                }

                employee.CreatedDate = DateTime.Now;
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadDepartments();
            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();

            LoadDepartments();
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check duplicate NIK (excluding current employee)
                if (db.Employees.Any(e => e.NIK == employee.NIK && e.EmployeeId != employee.EmployeeId))
                {
                    ModelState.AddModelError("NIK", "NIK already exists");
                    LoadDepartments();
                    return View(employee);
                }

                var existingEmployee = db.Employees.Find(employee.EmployeeId);
                if (existingEmployee != null)
                {
                    existingEmployee.NIK = employee.NIK;
                    existingEmployee.EmployeeName = employee.EmployeeName;
                    existingEmployee.DepartmentId = employee.DepartmentId;
                    existingEmployee.Position = employee.Position;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.HasLaptop = employee.HasLaptop;
                    existingEmployee.HasMealAllowance = employee.HasMealAllowance;
                    existingEmployee.ModifiedDate = DateTime.Now;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            LoadDepartments();
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        // HAPUS BARIS INI:
        // [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var employee = db.Employees.Find(id);
                if (employee == null)
                    return Json(new { success = false, message = "Employee not found" }, JsonRequestBehavior.AllowGet);

                // Check if employee has overtime entry
                if (db.Overtimes.Any(o => o.EmployeeId == id))
                {
                    return Json(new { success = false, message = "Cannot delete employee with overtime entry" }, JsonRequestBehavior.AllowGet);
                }

                db.Employees.Remove(employee);
                db.SaveChanges();

                return Json(new { success = true, message = "Employee deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void LoadDepartments()
        {
            ViewBag.Departments = db.Departments.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}