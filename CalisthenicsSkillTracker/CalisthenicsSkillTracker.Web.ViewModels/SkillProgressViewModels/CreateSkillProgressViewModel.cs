using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Utilities.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;

using static GCommon.EntityValidation.SkillSet;

public class CreateSkillProgressViewModel
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public Guid SkillId { get; set; }

    [ValidNullableEnumType]
    public Progression? Progression { get; set; }

    [Range(RepetitionsMinValue, RepetitionsMaxValue)]
    public int? Repetitions { get; set; }

    [Range(DurationMinValue, DurationMaxValue)]
    public int? Duration { get; set; }

    [MinLength(NotesMinLength)]
    [MaxLength(NotesMaxLength)]
    public string? Notes { get; set; }

    public IEnumerable<SelectListItem>? Users { get; set; }

    public IEnumerable<SelectListItem>? Skills { get; set; }

    public IEnumerable<SelectListItem>? Progressions { get; set; }

}
