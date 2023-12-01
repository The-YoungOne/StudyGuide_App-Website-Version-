using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace TesterStudyGuide_WebApp.Models
{
    public class StudyRecordModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int calendar_id { get; set; }
        [Required]
        public DateTime studyDate { get; set; }
        [Required]
        public double hoursStudied { get; set; }

        [Required]
        public string code { get; set; }
        [ForeignKey("code")]
        public ModuleModel Module { get; set; }

        [Required]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }
    }
}
