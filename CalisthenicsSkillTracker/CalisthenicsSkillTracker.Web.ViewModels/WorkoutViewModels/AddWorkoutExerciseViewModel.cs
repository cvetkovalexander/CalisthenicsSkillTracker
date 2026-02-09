using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

public class AddWorkoutExerciseViewModel
{
    [Required]
    public Guid ExerciseId { get; set; }

    [Required]
    public Guid WorkoutId { get; set; }

    public string Action { get; set; } = null!;

    public List<SelectListItem> AvailabeExercises { get; set; } 
        = new List<SelectListItem>();
}
