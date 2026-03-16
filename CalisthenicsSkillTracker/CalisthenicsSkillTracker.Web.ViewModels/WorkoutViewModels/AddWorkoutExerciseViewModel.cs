using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

public class AddWorkoutExerciseViewModel
{
    [Required(ErrorMessage = "Please select an exercise.")]
    [Display(Name = "Exercise")]
    public Guid ExerciseId { get; set; }

    [Required]
    public Guid WorkoutId { get; set; }

    public string Action { get; set; } = null!;

    public bool HasExercises { get; set; }

    public List<SelectListItem> AvailableExercises { get; set; } 
        = new List<SelectListItem>();
}
