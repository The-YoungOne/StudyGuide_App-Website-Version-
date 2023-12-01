using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesterStudyGuide_WebApp.Data;
using TesterStudyGuide_WebApp.Models;
using TesterLibrary;

namespace TesterStudyGuide_WebApp.Controllers
{
    [Authorize]
    public class ModuleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ModuleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Module
        public async Task<IActionResult> Index()
        {

            var userId = _userManager.GetUserId(HttpContext.User);

            // Retrieve module data for the logged-in user
            var moduleData = _context.Modules.Where(s => s.Id == userId).ToList();

            return moduleData != null ?
                          View(moduleData) :
                          Problem("Entity set 'ApplicationDbContext.Semesters'  is null.");
        }

        // GET: Module/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Retrieve module data for the logged-in user
            var moduleData = _context.Modules.Where(s => s.Id == userId).ToList();

            if (id == null || moduleData == null || moduleData.Count == 0)
            {
                return NotFound();
            }

            var moduleModel = moduleData.First();

            if (moduleModel == null)
            {
                return NotFound();
            }

            return View(moduleModel);
        }

        // GET: Module/Create
        public IActionResult Create()
        {
            ViewData["semesterId"] = new SelectList(_context.Semesters, "semesterId", "semesterId");
            return View();
        }

        // POST: Module/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("code,name,credits,classHours,semesterId,Id,totalStudyHours,StudyDays")] ModuleModel moduleModel)
        {
            try
            {
                //object of the dll library
                ClassMethods obj = new ClassMethods();
                string userId = "";

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userId = user.Id;
                    moduleModel.Id = userId;
                }
                else
                {
                    userId = "Couldn't retrieve user Id information!";
                }

                //calculates and adds the total study hours
                moduleModel.totalStudyHours = obj.totStudyHours(moduleModel.classHours);

                // Validate data manually if needed
                if (string.IsNullOrWhiteSpace(moduleModel.Id) || moduleModel.classHours <= 0 || moduleModel.credits <= 0)
                {
                    // Handle validation error
                    Console.WriteLine("Validation Error: Invalid data");
                    return View(moduleModel); // or redirect to an error page
                }

                // Perform the insert using ExecuteSqlInterpolated
                _context.Database.ExecuteSqlInterpolated($@"
                INSERT INTO Modules (code,name,credits,classHours,semesterId, Id, totalStudyHours, StudyDays)
                VALUES ({moduleModel.code}, {moduleModel.name}, {moduleModel.credits}, {moduleModel.classHours},
                {moduleModel.semesterId}, {moduleModel.Id}, {moduleModel.totalStudyHours}, {moduleModel.StudyDays})");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log or print the exception details for debugging
                Console.WriteLine($"Error: {ex.Message}");
                // Optionally, redirect to an error page or return a specific view
                return View("Error");
            }
        }

        // GET: Module/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var moduleModel = await _context.Modules.FindAsync(id);
            if (moduleModel == null)
            {
                return NotFound();
            }
            ViewData["semesterId"] = new SelectList(_context.Semesters, "semesterId", "semesterId", moduleModel.semesterId);
            return View(moduleModel);
        }

        // POST: Module/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("code,name,credits,classHours,semesterId")] ModuleModel moduleModel)
        {
            try
            {
                string userId = "";

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userId = user.Id;
                    moduleModel.Id = userId;
                }
                else
                {
                    userId = "Couldn't retrieve user Id information!";
                }

                // Validate data manually if needed
                if (string.IsNullOrWhiteSpace(moduleModel.Id) || moduleModel.classHours <= 0 || moduleModel.credits <= 0)
                {
                    // Handle validation error
                    Console.WriteLine("Validation Error: Invalid data");
                    return View(moduleModel); // or redirect to an error page
                }

                // Perform the update using ExecuteSqlInterpolated
                _context.Database.ExecuteSqlInterpolated($@"
            UPDATE Modules
            SET code = {moduleModel.code}, 
                name = {moduleModel.name},
                credits = {moduleModel.credits},
                classHours = {moduleModel.classHours},
                semesterId = {moduleModel.semesterId}
            WHERE code = {id}
        ");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log or print the exception details for debugging
                Console.WriteLine($"Error: {ex.Message}");
                // Optionally, redirect to an error page or return a specific view
                return View("Error");
            }
        }


        // GET: Module/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var moduleModel = await _context.Modules
                .Include(m => m.Semester)
                .FirstOrDefaultAsync(m => m.code == id);
            if (moduleModel == null)
            {
                return NotFound();
            }

            return View(moduleModel);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Modules == null || _context.Records == null)
            {
                return Problem("Entity sets are null.");
            }

            // Find the module
            var moduleModel = await _context.Modules.FindAsync(id);

            if (moduleModel == null)
            {
                // Handle the case where the module is not found
                return NotFound();
            }

            // Find all StudyRecords associated with the module
            var studyRecordsToDelete = _context.Records.Where(r => r.code == id);

            // Remove each StudyRecord
            foreach (var studyRecord in studyRecordsToDelete)
            {
                _context.Records.Remove(studyRecord);
            }

            // Remove the module
            _context.Modules.Remove(moduleModel);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ModuleModelExists(string id)
        {
            return (_context.Modules?.Any(e => e.code == id)).GetValueOrDefault();
        }

    }
}
