using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyGuide_WebApp.Models
{
    public class SemesterModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        //add in semester name (that will be displayed to the user in the semdster tab and when creating and in the module tab)
        public int semesterId { get; set; }
        [Required]
        public int weeks { get; set; }
        [Required]
        public DateTime startDate { get; set; }


        [Required]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }

        public List<ModuleModel> Modules { get; set; } = new List<ModuleModel>();
    }
}
