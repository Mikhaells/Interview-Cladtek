using Cladtek_Interview.Models; 
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OvertimeManagementApp.Controllers
{
    public class OvertimeController : Controller
    {
        private OvertimeManagementContext db = new OvertimeManagementContext();
        private const int PageSize = 10;
        private const decimal MaxOTHours = 3;

        // GET: Overtime
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            var overtimes = db.Overtimes
                .Include("Employee")
                .Include("Employee.Department")
                .OrderByDescending(o => o.OvertimeDate)
                .ToPagedList(pageNumber, PageSize);

            return View(overtimes);
        }

        // GET: Overtime/Create
        public ActionResult Create()
        {
            var overtime = new Overtime();
            LoadEmployees();
            return View(overtime);
        }

        // POST: Overtime/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Overtime overtime)
        {
            if (ModelState.IsValid)
            {
                decimal actualOTHours = CalculateOTHours(overtime.TimeStart, overtime.TimeFinish);

                if (actualOTHours > MaxOTHours)
                {
                    ModelState.AddModelError("ActualOTHours", $"Maximum OT hours is {MaxOTHours} hours");
                    LoadEmployees();
                    return View(overtime);
                }

                overtime.ActualOTHours = actualOTHours;
                overtime.CalculatedOTHours = actualOTHours * 2;
                overtime.CreatedDate = DateTime.Now;

                db.Overtimes.Add(overtime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadEmployees();
            return View(overtime);
        }

        // GET: Overtime/Edit/5
        public ActionResult Edit(int id)
        {
            var overtime = db.Overtimes.Find(id);
            if (overtime == null)
                return HttpNotFound();

            LoadEmployees();
            return View(overtime);
        }

        // POST: Overtime/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Overtime overtime)
        {
            if (ModelState.IsValid)
            {
                decimal actualOTHours = CalculateOTHours(overtime.TimeStart, overtime.TimeFinish);

                if (actualOTHours > MaxOTHours)
                {
                    ModelState.AddModelError("ActualOTHours", $"Maximum OT hours is {MaxOTHours} hours");
                    LoadEmployees();
                    return View(overtime);
                }

                var existingOvertime = db.Overtimes.Find(overtime.OvertimeId);
                if (existingOvertime != null)
                {
                    existingOvertime.EmployeeId = overtime.EmployeeId;
                    existingOvertime.OvertimeDate = overtime.OvertimeDate;
                    existingOvertime.TimeStart = overtime.TimeStart;
                    existingOvertime.TimeFinish = overtime.TimeFinish;
                    existingOvertime.ActualOTHours = actualOTHours;
                    existingOvertime.CalculatedOTHours = actualOTHours * 2;
                    existingOvertime.Description = overtime.Description;
                    existingOvertime.ModifiedDate = DateTime.Now;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            LoadEmployees();
            return View(overtime);
        }

        // POST: Overtime/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var overtime = db.Overtimes.Find(id);
            if (overtime == null)
                return Json(new { success = false, message = "Overtime not found" });

            db.Overtimes.Remove(overtime);
            db.SaveChanges();
            return Json(new { success = true, message = "Overtime deleted successfully" });
        }

        // Calculate OT Hours
        private decimal CalculateOTHours(DateTime timeStart, DateTime timeFinish)
        {
            TimeSpan ts = timeFinish - timeStart;
            return (decimal)ts.TotalHours;
        }

        private void LoadEmployees()
        {
            ViewBag.Employees = db.Employees
                .Include("Department")
                .OrderBy(e => e.EmployeeName)
                .ToList();
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