using Microsoft.AspNetCore.Identity;
using StudyGuide_WebApp.Models;

namespace StudyGuide_WebApp
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
