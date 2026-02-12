using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillOutputService : ISkillOutputService
{
    private readonly ApplicationDbContext _context;

    public SkillOutputService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<ListTableItemViewModel>> GetAllSkillsAsync(string? filter)
    {
        IEnumerable<ListTableItemViewModel> skills = await this._context
            .Skills
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new ListTableItemViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Difficulty = s.Difficulty
            })
            .ToArrayAsync();

        if (filter is not null)
            skills = skills.Where(s => s.Name.ToLower().Contains(filter.ToLower()));

        return skills;
    }

    public async Task<DetailsSkillViewModel> GetSkillDetailsAsync(Guid id)
    {
        Skill skill = await this._context
            .Skills
            .AsNoTracking()
            .Include(s => s.Exercises)
            .SingleAsync(s => s.Id == id);

        DetailsSkillViewModel model = new DetailsSkillViewModel()
        {
            Name = skill.Name,
            Description = skill.Description,
            Measurement = skill.MeasurementType,
            Category = skill.Category,
            SkillType = skill.SkillType,
            Difficulty = skill.Difficulty,
            Exercises = skill.Exercises,
            SkillRecords = this._context
                .SkillProgressRecords
                .AsNoTracking()
                .Include(r => r.PerformedBy)
                .Where(r => r.SkillId == skill.Id)
                .OrderByDescending(r => r.Date)
                .ToArray()
        };

        return model;
    }

    public async Task<bool> SkillExistsAsync(Guid id)
    {
        return await this._context
            .Skills
            .AnyAsync(s => s.Id == id);
    }
}
