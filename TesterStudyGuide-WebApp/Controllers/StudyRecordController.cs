using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesterLibrary;
using TesterStudyGuide_WebApp.Data;
using TesterStudyGuide_WebApp.Models;

namespace TesterStudyGuide_WebApp.Controllers
{
    public class StudyRecordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudyRecordController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: StudyRecord
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Retrieve record data for the logged-in user
            var recordData = _context.Records.Where(s => s.Id == userId).ToList();

            return recordData != null ?
                          View(recordData) :
                          Problem("Entity set 'ApplicationDbContext.Semesters'  is null.");
        }

        // GET: StudyRecord/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Retrieve record data for the logged-in user
            var recordData = _context.Records.Where(s => s.Id == userId).ToList();

            if (id == null || recordData == null || recordData.Count == 0)
            {
                return NotFound();
            }

            var moduleModel = recordData.First();

            if (moduleModel == null)
            {
                return NotFound();
            }

            return View(moduleModel);
        }

        // GET: StudyRecord/Create
        public IActionResult Create()
        {
            ViewData["code"] = new SelectList(_context.Modules, "code", "code");
            return View();
        }

        // POST: StudyRecord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("calendar_id,studyDate,hoursStudied,code,Id")] StudyRecordModel studyRecordModel)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    // Handle the case where the user is not authenticated
                    Console.WriteLine("User is not authenticated");
                    return View(studyRecordModel);
                }

                string userId = "";

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userId = user.Id;
                    studyRecordModel.Id = userId;
                }
                else
                {
                    userId = "Couldn't retrieve user Id information!";
                }

                // Validate data manually if needed
                if (string.IsNullOrWhiteSpace(studyRecordModel.Id) || studyRecordModel.hoursStudied <= 0)
                {
                    // Handle validation error
                    Console.WriteLine("Validation Error: Invalid data");
                    return View(studyRecordModel); // or redirect to an error page
                }

                // Perform the insert using ExecuteSqlInterpolated
                int affectedRows = _context.Database.ExecuteSqlInterpolated($@"
                INSERT INTO Records (studyDate,hoursStudied,code,Id)
                VALUES ({studyRecordModel.studyDate}, {studyRecordModel.hoursStudied}, {studyRecordModel.code}, {studyRecordModel.Id}
                )");

                if(affectedRows > 0)
                {
                    _context.Database.ExecuteSqlInterpolated($@"
                    UPDATE Modules
                    SET totalStudyHours = totalStudyHours - {studyRecordModel.hoursStudied}
                    WHERE code = {studyRecordModel.code}
                    ");
                }

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

        // GET: StudyRecord/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var studyRecordModel = await _context.Records.FindAsync(id);
            if (studyRecordModel == null)
            {
                return NotFound();
            }
            ViewData["code"] = new SelectList(_context.Modules, "code", "code", studyRecordModel.code);
            return View(studyRecordModel);
        }

        // POST: StudyRecord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("calendar_id,studyDate,hoursStudied")] StudyRecordModel studyRecordModel)
        {
            try
            {
                string userId = "";

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userId = user.Id;
                    studyRecordModel.Id = userId;
                }
                else
                {
                    userId = "Couldn't retrieve user Id information!";
                }

                // Validate data manually if needed
                if (string.IsNullOrWhiteSpace(studyRecordModel.Id) || studyRecordModel.hoursStudied <= 0)
                {
                    // Handle validation error
                    Console.WriteLine("Validation Error: Invalid data");
                    return View(studyRecordModel); // or redirect to an error page
                }

                // Perform the update using ExecuteSqlInterpolated
                _context.Database.ExecuteSqlInterpolated($@"
            UPDATE Records
            SET calendar_id = {studyRecordModel.calendar_id}, 
                studyDate = {studyRecordModel.studyDate},
                hoursStudied = {studyRecordModel.hoursStudied},
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

        // GET: StudyRecord/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var studyRecordModel = await _context.Records
                .Include(s => s.Module)
                .FirstOrDefaultAsync(m => m.calendar_id == id);
            if (studyRecordModel == null)
            {
                return NotFound();
            }

            return View(studyRecordModel);
        }

        // POST: StudyRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Records == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Records' is null.");
            }

            var studyRecordModel = await _context.Records.FindAsync(id);

            if (studyRecordModel == null)
            {
                // Handle the case where the record is not found
                return NotFound();
            }

            // Retrieve the HoursStudied before removing the record
            double hoursStudied = studyRecordModel.hoursStudied;

            _context.Records.Remove(studyRecordModel);

            await _context.SaveChangesAsync();

            // Update the corresponding ModuleModel by adding back the HoursStudied
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
        UPDATE Modules
        SET totalStudyHours = totalStudyHours + {hoursStudied}
        WHERE code = {studyRecordModel.code}
    ");

            return RedirectToAction(nameof(Index));
        }

        private bool StudyRecordModelExists(int id)
        {
            return (_context.Records?.Any(e => e.calendar_id == id)).GetValueOrDefault();
        }

    }
}
