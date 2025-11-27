using System.Web.Mvc;
using Cladtek_Interview.Models;
using System.Linq;

namespace OvertimeManagementApp.Controllers
{
    public class HomeController : Controller
    {
        private OvertimeManagementContext db = new OvertimeManagementContext();

        // GET: Home
        public ActionResult Index()
        {
            // Dashboard Statistics
            int totalEmployees = db.Employees.Count();
            int totalDepartments = db.Departments.Count();
            int totalOvertimes = db.Overtimes.Count();
            decimal totalOTHours = db.Overtimes.Sum(o => (decimal?)o.ActualOTHours) ?? 0;

            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.TotalDepartments = totalDepartments;
            ViewBag.TotalOvertimes = totalOvertimes;
            ViewBag.TotalOTHours = totalOTHours.ToString();

            // Recent Overtimes
            var recentOvertimes = db.Overtimes
                .Include("Employee")
                .Include("Employee.Department")
                .OrderByDescending(o => o.OvertimeDate)
                .Take(5)
                .ToList();

            return View(recentOvertimes);
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