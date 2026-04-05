using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillProgressService : ISkillProgressService
{

    private readonly ISkillProgressRepository _repository;

    public SkillProgressService(ISkillProgressRepository repository)
    {
        this._repository = repository;
    }

    public async Task CreateSkillProgressAsync(CreateSkillProgressViewModel model)
    {
        SkillProgress record = new SkillProgress
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(model.UserId),
            SkillId = model.SkillId,
            Date = DateTime.UtcNow,
            Progression = model.Progression,
            Repetitions = model.Repetitions,
            Duration = model.Duration,
            Notes = model.Notes
        };

        bool successfulAdd = await this._repository.AddSkillProgressAsync(record);
        if (!successfulAdd)
            throw new EntityCreatePersistException();
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
        SkillProgress skillProgress = await this._repository
            .GetSkillRecordAsync(id);

        bool successfulDelete = await this._repository.HardDeleteSkillProgressAsync(skillProgress);

        if (!successfulDelete)
            throw new EntityDeleteException();
    }

    public async Task<IEnumerable<ListRecordViewModel>> GetRecordsAsync(string userId)
    {
        return await this._repository
            .GetUserSkillProgressRecords(userId)
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
        model.Skills = this._repository
            .GetAllSkills()
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
        => await this._repository
            .SkillExistsAsync(id);

    public async Task<bool> SkillRecordExistsAsync(Guid id)
        => await this._repository
            .SkillRecordExistsAsync(id);
}
