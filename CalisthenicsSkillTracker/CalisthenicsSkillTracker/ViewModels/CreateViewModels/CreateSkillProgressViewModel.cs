using CalisthenicsSkillTracker.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.CreateViewModels;

public class CreateSkillProgressViewModel
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid SkillId { get; set; }

    public Progression? Progression { get; set; }

    public int? Repetitions { get; set; }

    public int? Duration { get; set; }

    public string? Notes { get; set; }

    public IEnumerable<SelectListItem>? Users { get; set; }

    public IEnumerable<SelectListItem>? Skills { get; set; }

    public IEnumerable<SelectListItem>? Progressions { get; set; }

}
