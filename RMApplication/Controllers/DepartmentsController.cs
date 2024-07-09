using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using RMApplication.Controllers;
using RMApplication.Models;
using RMApplication.Services;
using System;

namespace RMApplication.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.SubDepartments)
                .ToListAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Logo,ParentDepartmentId")] Department department)
        {
            //if (ModelState.IsValid)
            //{
            if (department != null)
            {
                //var departnameexit=_context.Departments.Any(x=>x.Name== department.Name);
                //if (!departnameexit)
                //{
                    _context.Add(department);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                //}
                
            }
           // }
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,ParentDepartmentId")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await _context.Departments
                .Include(d => d.SubDepartments)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            var parents = new List<Department>();
            var parent = department.ParentDepartment;
            while (parent != null)
            {
                parents.Add(parent);
                parent = parent.ParentDepartment;
            }

            var viewModel = new DepartmentDetailsViewModel
            {
                Department = department,
                ParentDepartments = parents
            };

            return View(viewModel);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }

}



