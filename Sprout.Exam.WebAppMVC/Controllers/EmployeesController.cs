using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.SalaryComputation;
using Sprout.Exam.WebAppMVC.Data;
using Sprout.Exam.WebAppMVC.Models;

namespace Sprout.Exam.WebAppMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly SproutExamDbContext _context;

        public EmployeesController(SproutExamDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var result = _context.Employees.Where(w => !w.IsDeleted).Include(e => e.EmployeeType);
            if (result.Count() == 0)
            {
                //Add initial records
                var init = StaticEmployees.ResultList.Select(s => new Employee() { FullName = s.FullName, Birthdate = DateTime.Parse(s.Birthdate), EmployeeTypeId = s.TypeId, Tin = s.Tin });

                await _context.Employees.AddRangeAsync(init);
                _context.SaveChanges();

                //get newly added records
                result = _context.Employees.Where(w => !w.IsDeleted).Include(e => e.EmployeeType);
            }
            return View(await result.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Calculate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.EmployeeType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            var model = new CalculateViewModel();
            model.Employee = employee;
            model.Regular = new Business.SalaryComputation.RegularSalary() { BasicMonthlyRate = 20000, DaysOfWork = 22, Tax = 0.12, AbsentDays = 0  } ;
            model.Contractual = new Business.SalaryComputation.ContractualSalary() { RatePerDay = 500, WorkedDays = 0 };

            return View(model);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["EmployeeTypeId"] = new SelectList(_context.EmployeeTypes, "Id", "TypeName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Birthdate,Tin,EmployeeTypeId,IsDeleted")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeTypeId"] = new SelectList(_context.EmployeeTypes, "Id", "TypeName", employee.EmployeeTypeId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeTypeId"] = new SelectList(_context.EmployeeTypes, "Id", "TypeName", employee.EmployeeTypeId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Birthdate,Tin,EmployeeTypeId,IsDeleted")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeTypeId"] = new SelectList(_context.EmployeeTypes, "Id", "TypeName", employee.EmployeeTypeId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.EmployeeType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            //_context.Employees.Remove(employee);
            employee.IsDeleted = true;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<string> CalculateEmployeeSalary(int id, int absent = 0, float workedHours = 0)
        {
            var result = "";
            var employee = await _context.Employees.Include(e => e.EmployeeType).FirstOrDefaultAsync(m => m.Id == id);
            if (employee != null)
            {
                ComputeSalary computeSalary = new ComputeSalary();
                if(employee.EmployeeTypeId == (int)Common.Enums.EmployeeType.Regular)
                {
                    computeSalary.employeeType = Common.Enums.EmployeeType.Regular;
                    computeSalary.RegularSalary = new Business.SalaryComputation.RegularSalary() { BasicMonthlyRate = 20000, DaysOfWork = 22, Tax = 0.12, AbsentDays = absent };
                }
                else if (employee.EmployeeTypeId == (int)Common.Enums.EmployeeType.Contractual)
                {
                    computeSalary.employeeType = Common.Enums.EmployeeType.Contractual;
                    computeSalary.ContractualSalary = new Business.SalaryComputation.ContractualSalary() { RatePerDay = 500, WorkedDays = workedHours };
                }

                result = String.Format("{0:0,0.00}", computeSalary.ComputationResult());

            }
            return result;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
