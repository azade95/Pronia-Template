using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels.Employee;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Employee> employees = _context.Employees.Include(e=>e.Position).ToList();
            return View(employees);
        }
        public IActionResult Create()
        {
            List<Position> positions = _context.Positions.ToList();
            ViewData["Positions"] = positions;
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeVM employeeVM)
        {
            Employee employee = new Employee
            {
                
                Name = employeeVM.Name,
                PositionId= employeeVM.PositionId,
            };
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            Employee existed= _context.Employees.FirstOrDefault(e=>e.Id==id);
            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM
            {
                Name = existed.Name,
                PositionId = existed.PositionId,
            };
            List<Position> positions = _context.Positions.ToList();
            ViewData["Positions"] = positions;
            return View(employeeVM);
        }
        [HttpPost]
        public IActionResult Update(int? id,UpdateEmployeeVM employeeVM)
        {
            Employee existed = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (existed == null)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                ViewData["Positions"] = _context.Positions.ToList(); ;
                return View(employeeVM);
            }
            existed.Name=employeeVM.Name;
            existed.PositionId=employeeVM.PositionId;
            _context.Employees.Update(existed);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id < 0) return BadRequest();
            Employee existed = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (existed == null) return NotFound();
            _context.Employees.Remove(existed);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
