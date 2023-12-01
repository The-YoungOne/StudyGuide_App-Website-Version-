using TesterStudyGuide_WebApp.Data;
using TesterStudyGuide_WebApp.Models;

namespace TesterStudyGuide_WebApp.Services
{
    // ModuleService.cs
    public class ModuleService
    {
        private readonly ApplicationDbContext _context;

        public ModuleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ModuleModel> GetModulesForUser(string userId)
        {
            return _context.Modules
                .Where(m => m.Id == userId)
                .ToList();
        }
    }

}
