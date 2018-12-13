using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab7.Models.DataAccess;
using Lab7.Models;
using Microsoft.AspNetCore.Http;



namespace Lab7.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly StudentRecordContext _context;

        public EmployeesController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            EmployeeRoleSelections employeeRolesSelections = new EmployeeRoleSelections();
            return View(employeeRolesSelections);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeRoleSelections employeeRolesSelections)
        {
            if (!employeeRolesSelections.roleSelections.Any(m=>m.Selected))
            {
                ModelState.AddModelError("roleSelectoions", "You must selected at least one role!");
            }
            if (_context.Employee.Any(e=>e.UserName == employeeRolesSelections.employee.UserName))
            {
                ModelState.AddModelError("employee.Username", "This user name already exixtes!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(employeeRolesSelections.employee);
                _context.SaveChanges();
                foreach (RoleSelection roleSelection in employeeRolesSelections.roleSelections)
                {
                    if (roleSelection.Selected)
                    {
                        EmployeeRole employeeRole = new EmployeeRole
                                                    {
                                                      RoleId = roleSelection.role.Id,
                                                      EmployeeId = employeeRolesSelections.employee.Id
                                                     };
                        _context.EmployeeRole.Add(employeeRole);
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeRolesSelections);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            List<EmployeeRole> employeeRoles = (_context.EmployeeRole.Where(m => m.EmployeeId == id)).ToList();

            EmployeeRoleSelections employeeRoleSelections = new EmployeeRoleSelections();
            employeeRoleSelections.employee = employee;

            for (int i = 0; i < employeeRoleSelections.roleSelections.Count; i++)
            {
                if(employeeRoles.Exists(m=>m.RoleId == employeeRoleSelections.roleSelections[i].role.Id))
                {
                    employeeRoleSelections.roleSelections[i].Selected = true;
                }
            }
            return View(employeeRoleSelections);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserName,Password")] Employee employee)
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

            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
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
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
