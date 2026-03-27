using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CalisthenicsSkillTracker.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    [PersonalData]
    [DataType(DataType.Text)]
    [MaxLength(50)]
    public string FullName { get; set; } = null!;

    [Required]
    [PersonalData]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public virtual ICollection<Workout> Workouts { get; set; }
        = new List<Workout>();

    public virtual ICollection<SkillProgress> SkillProgressRecords { get; set; }
        = new List<SkillProgress>();

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        = new List<ApplicationUserRole>();

}
