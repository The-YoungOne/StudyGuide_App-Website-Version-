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

namespace TesterStudyGuide_WebApp.Controllers
{
    [Authorize]
    public class SemesterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SemesterController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Semester
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Retrieve semester data for the logged-in user
            var semesterData = _context.Semesters.Where(s => s.Id == userId).ToList();

            return semesterData != null ? 
                          View(semesterData) :
                          Problem("Entity set 'ApplicationDbContext.Semesters'  is null.");
        }

        // GET: Semester/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Retrieve semester data for the logged-in user
            var semesterData = _context.Semesters.Where(s => s.Id == userId).ToList();

            // Only the user with the matching UserId can access this action and see their data
            if (id == null || semesterData == null || semesterData.Count == 0)
            {
                return NotFound();
            }

            var semesterModel = semesterData.First();

            if (semesterModel == null)
            {
                return NotFound();
            }

            return View(semesterModel);
        }

        // GET: Semester/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Semester/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("semesterId,weeks,startDate,Id")] SemesterModel semesterModel)
        {
            try
            {
                string userId = "";

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userId = user.Id;
                    semesterModel.Id = userId;
                }
                else
                {
                    userId = "Couldn't retrieve user Id information!";
                }

                // Validate data manually if needed
                if (string.IsNullOrWhiteSpace(semesterModel.Id) || semesterModel.weeks <= 0)
                {
                    // Handle validation error
                    Console.WriteLine("Validation Error: Invalid data");
                    return View(semesterModel); // or redirect to an error page
                }

                // Perform the insert using ExecuteSqlInterpolated
                _context.Database.ExecuteSqlInterpolated($@"
                INSERT INTO Semesters (weeks, startDate, Id)
                VALUES ({semesterModel.weeks}, {semesterModel.startDate}, {semesterModel.Id})
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


        // GET: Semester/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semesterModel = await _context.Semesters.FindAsync(id);
            if (semesterModel == null)
            {
                return NotFound();
            }
            return View(semesterModel);
        }

        // POST: Semester/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("semesterId,weeks,startDate,Id")] SemesterModel semesterModel)
        {
            try
            {
                string userId = "";

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userId = user.Id;
                    semesterModel.Id = userId;
                }
                else
                {
                    userId = "Couldn't retrieve user Id information!";
                }

                // Validate data manually if needed
                if (string.IsNullOrWhiteSpace(semesterModel.Id) || semesterModel.weeks <= 0)
                {
                    // Handle validation error
                    Console.WriteLine("Validation Error: Invalid data");
                    return View(semesterModel); // or redirect to an error page
                }

                // Perform the update using ExecuteSqlInterpolated
                _context.Database.ExecuteSqlInterpolated($@"
            UPDATE Semesters
            SET weeks = {semesterModel.weeks}, 
                startDate = {semesterModel.startDate},
                Id = {semesterModel.Id}
            WHERE semesterId = {id}
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


        // GET: Semester/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semesterModel = await _context.Semesters
                .FirstOrDefaultAsync(m => m.semesterId == id);
            if (semesterModel == null)
            {
                return NotFound();
            }

            return View("Delete", semesterModel);
        }


        // POST: Semester/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Semesters == null || _context.Modules == null || _context.Records == null)
            {
                return Problem("Entity sets are null.");
            }

            // Find the semester
            var semesterModel = await _context.Semesters.FindAsync(id);

            if (semesterModel == null)
            {
                // Handle the case where the semester is not found
                return NotFound();
            }

            // Find all modules associated with the semester
            var modulesToDelete = _context.Modules.Where(m => m.semesterId == id);

            // Find all StudyRecords associated with the modules
            var studyRecordsToDelete = _context.Records.Where(r => modulesToDelete.Any(m => m.code == r.code));

            // Remove each StudyRecord
            foreach (var studyRecord in studyRecordsToDelete)
            {
                _context.Records.Remove(studyRecord);
            }

            // Remove each module
            foreach (var module in modulesToDelete)
            {
                _context.Modules.Remove(module);
            }

            // Remove the semester
            _context.Semesters.Remove(semesterModel);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SemesterModelExists(int id)
        {
            return (_context.Semesters?.Any(e => e.semesterId == id)).GetValueOrDefault();
        }


    }
}
