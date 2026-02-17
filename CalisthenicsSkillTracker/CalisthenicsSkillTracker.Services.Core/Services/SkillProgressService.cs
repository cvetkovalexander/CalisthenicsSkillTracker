using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillProgressService : ISkillProgressService
{
    private readonly ApplicationDbContext _context;

    public SkillProgressService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task CreateSkillProgress(CreateSkillProgressViewModel model)
    {
        SkillProgress record = new SkillProgress
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            SkillId = model.SkillId,
            Date = DateTime.UtcNow,
            Progression = model.Progression,
            Repetitions = model.Repetitions,
            Duration = model.Duration,
            Notes = model.Notes
        };

        await this._context.SkillProgressRecords.AddAsync(record);
        await this._context.SaveChangesAsync();
    }

    public CreateSkillProgressViewModel CreateSkillProgressViewModel(string userId)
    {
        CreateSkillProgressViewModel model = new CreateSkillProgressViewModel 
        {
            UserId = userId,
        };
        this.PopulateSelectListItems(model);

        return model;
    }

    public async Task DeleteSkillRecordAsync(Guid id)
    {
        SkillProgress skillProgress = await this._context.SkillProgressRecords.FirstAsync(sp => sp.Id == id);

        this._context.SkillProgressRecords.Remove(skillProgress);

        await this._context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ListRecordViewModel>> GetRecordsAsync(string userId)
    {
        return await this._context
            .SkillProgressRecords
            .Where(sp => sp.UserId == userId)
            .Include(sp => sp.Skill)
            .OrderByDescending(sp => sp.Date)
            .Select(sp => new ListRecordViewModel
            {
                Id = sp.Id,
                SkillName = sp.Skill.Name,
                Date = sp.Date,
                Progression = sp.Progression.ToString(),
                Repetitions = sp.Repetitions,
                Duration = sp.Duration,
                Notes = sp.Notes
            })
            .ToListAsync();
    }

    /* Helper methods */

    public void PopulateSelectListItems(CreateSkillProgressViewModel model)
    {
        model.Skills = this._context
            .Skills
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            })
            .ToList();
        model.Progressions = Enum.GetValues<Progression>()
            .Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            })
            .ToList();
    }

    public async Task<bool> SkillExistsAsync(Guid id)
    {
        return await this._context
            .Skills
            .AnyAsync(s => s.Id == id);
    }

    public async Task<bool> SkillRecordExistsAsync(Guid id)
    {
        return await this._context
            .SkillProgressRecords
            .AnyAsync(sp => sp.Id == id);
    }
}
