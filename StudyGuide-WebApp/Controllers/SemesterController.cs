using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyGuide_WebApp.Data;
using StudyGuide_WebApp.Models;

namespace StudyGuide_WebApp.Controllers
{
    public class SemesterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SemesterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Semester
        public async Task<IActionResult> Index()
        {
              return _context.Semesters != null ? 
                          View(await _context.Semesters.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Semesters'  is null.");
        }

        // GET: Semester/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semesterModel = await _context.Semesters
                .FirstOrDefaultAsync(m => m.semester_id == id);
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
        public async Task<IActionResult> Create([Bind("semester_id,weeks,startDate,endDate")] SemesterModel semesterModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(semesterModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(semesterModel);
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
        public async Task<IActionResult> Edit(int id, [Bind("semester_id,weeks,startDate,endDate")] SemesterModel semesterModel)
        {
            if (id != semesterModel.semester_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semesterModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemesterModelExists(semesterModel.semester_id))
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
            return View(semesterModel);
        }

        // GET: Semester/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semesterModel = await _context.Semesters
                .FirstOrDefaultAsync(m => m.semester_id == id);
            if (semesterModel == null)
            {
                return NotFound();
            }

            return View(semesterModel);
        }

        // POST: Semester/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Semesters == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Semesters'  is null.");
            }
            var semesterModel = await _context.Semesters.FindAsync(id);
            if (semesterModel != null)
            {
                _context.Semesters.Remove(semesterModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SemesterModelExists(int id)
        {
          return (_context.Semesters?.Any(e => e.semester_id == id)).GetValueOrDefault();
        }
    }
}
