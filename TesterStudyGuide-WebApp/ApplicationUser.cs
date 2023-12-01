using Microsoft.AspNetCore.Identity;
using System.Reflection;
using TesterStudyGuide_WebApp.Models;

namespace TesterStudyGuide_WebApp
{
    public class ApplicationUser : IdentityUser
    {
        // Relationship with Semesters
        public List<SemesterModel> Semesters { get; set; }

        // Relationship with Modules
        public List<ModuleModel> Modules { get; set; }

        // Relationship with StudyRecords
        public List<StudyRecordModel> Records { get; set; }
    }
}
