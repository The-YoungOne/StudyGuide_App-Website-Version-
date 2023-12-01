using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyGuide_WebApp.Models
{
    public class ModuleModel
    {
        [Key]
        public string code { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public int credits { get; set; }
        [Required]
        public int classHours { get; set; }
        [Required]
        public int totalStudyHours { get; set; }
        [Required]
        public string StudyDays { get; set; }
        public int semesterId { get; set; }
        public SemesterModel Semester { get; set; }

        [Required]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }
    }
}
