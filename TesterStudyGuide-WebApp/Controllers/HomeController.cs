using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TesterStudyGuide_WebApp.Data;
using TesterStudyGuide_WebApp.Models;

namespace TesterStudyGuide_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var userModules = _context.Modules.Where(m => m.Id == userId).ToList();
                var currentDay = DateTime.Now.DayOfWeek.ToString();

                int index = -1;
                int cnter = 0;

                foreach (var item in userModules)
                {
                    if (item.StudyDays.Equals(currentDay))
                    {
                        index = cnter;
                    }
                    cnter++;
                }

                if (index != -1)
                {
                    // Display a positive message
                    TempData["Message"] = $"Today is a study day for module {userModules[index].name}";
                }
                else
                {
                    // Display a negative message
                    TempData["Message"] = "Today is not a study day for any module.";
                }

                // Pass the userModules to the view if needed
                ViewBag.UserModules = userModules;
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string ModuleInfo()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Retrieve module data for the logged-in user
            var moduleData = _context.Modules.Where(s => s.Id == userId).ToList();
            var currentDay = DateTime.Now.DayOfWeek.ToString();

            int index = -1;
            int cnter = 0;
            foreach (var item in moduleData)
            {
                if (item.StudyDays.Equals(currentDay))
                {
                    index = cnter;
                }
                cnter++;
            }

            if(index == -1)
            {
                return "";
            }
            else
            {
                return moduleData[index].name;
            }
        }
    }
}