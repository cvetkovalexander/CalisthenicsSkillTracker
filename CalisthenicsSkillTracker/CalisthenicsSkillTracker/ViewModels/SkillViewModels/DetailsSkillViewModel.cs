using CalisthenicsSkillTracker.Models;
using CalisthenicsSkillTracker.Models.Enums;
using CalisthenicsSkillTracker.Utilities.Attributes;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.SkillViewModels;

using static Common.EntityValidation.Skill;

public class DetailsSkillViewModel : ISkillViewModel
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Measurement Measurement { get; set; }

    public List<SelectListItem> MeasurementOptions { get; set; } = new List<SelectListItem>();
    public Category Category { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new List<SelectListItem>();

    public SkillType SkillType { get; set; }

    public List<SelectListItem> SkillTypeOptions { get; set; } = new List<SelectListItem>();

    public Difficulty Difficulty { get; set; }

    public List<SelectListItem> DifficultyOptions { get; set; } = new List<SelectListItem>();

    public IReadOnlyCollection<SkillProgress> SkillRecords { get; set; }
        = new List<SkillProgress>();
}
