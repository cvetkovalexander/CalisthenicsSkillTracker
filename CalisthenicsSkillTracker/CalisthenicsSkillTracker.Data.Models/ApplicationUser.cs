using Microsoft.AspNetCore.Identity;

namespace CalisthenicsSkillTracker.Data.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<SkillProgress> SkillProgressRecords { get; set; }
        = new List<SkillProgress>();

    public virtual ICollection<Workout> Workouts { get; set; }
        = new List<Workout>();
}
