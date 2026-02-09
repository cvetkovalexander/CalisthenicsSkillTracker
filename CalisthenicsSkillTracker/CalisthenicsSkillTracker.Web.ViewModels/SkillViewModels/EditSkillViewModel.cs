using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Utilities.Attributes;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.SkillViewModels;

using static GCommon.EntityValidation.Skill;

public class EditSkillViewModel : ISkillViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [ValidSkillName]
    public string Name { get; set; } = null!;

    [MinLength(DescriptionMinLength)]
    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    public Measurement Measurement { get; set; }

    public List<SelectListItem> MeasurementOptions { get; set; } = new List<SelectListItem>();

    [Required]
    public Category Category { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new List<SelectListItem>();

    [Required]
    public SkillType SkillType { get; set; }

    public List<SelectListItem> SkillTypeOptions { get; set; } = new List<SelectListItem>();

    [Required]
    public Difficulty Difficulty { get; set; }

    public List<SelectListItem> DifficultyOptions { get; set; } = new List<SelectListItem>();
}
