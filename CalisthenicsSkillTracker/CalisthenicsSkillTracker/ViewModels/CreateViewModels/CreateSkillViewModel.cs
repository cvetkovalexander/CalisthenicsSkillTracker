namespace CalisthenicsSkillTracker.ViewModels.CreateViewModels;

using CalisthenicsSkillTracker.Models.Enums;
using CalisthenicsSkillTracker.Utilities.Attributes;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static Common.EntityValidation.Skill;

public class CreateSkillViewModel : ISkillViewModel
{
    [Required]
    [ValidSkillName]
    public string Name { get; set; } = null!;

    [MinLength(DescriptionMinLength)]
    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    public Measurement Measurement { get; set;}

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
