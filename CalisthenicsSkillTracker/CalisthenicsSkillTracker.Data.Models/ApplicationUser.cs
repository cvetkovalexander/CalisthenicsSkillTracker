using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CalisthenicsSkillTracker.Data.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<SkillProgress> SkillProgressRecords { get; set; }
        = new List<SkillProgress>();

    public virtual ICollection<Workout> Workouts { get; set; }
        = new List<Workout>();

    [PersonalData]
    [DataType(DataType.Text)]
    [MaxLength(50)]
    public string FullName { get; set; } = null!;

    [Required]
    [PersonalData]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}
