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

    public CreateSkillProgressViewModel CreateSkillProgressViewModel()
    {
        CreateSkillProgressViewModel model = new CreateSkillProgressViewModel();
        this.PopulateSelectListItems(model);

        return model;
    }

    /* Helper methods */

    public void PopulateSelectListItems(CreateSkillProgressViewModel model)
    {
        model.Users = this._context
            .Users
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Username
            })
            .ToList();
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

    public async Task<bool> UserExistsAsync(Guid id)
    {
        return await this._context
            .Users
            .AnyAsync(u => u.Id == id);
    }
}
