using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillOutputService : ISkillOutputService
{
    private readonly ISkillRepository _repository;

    public SkillOutputService(ISkillRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IEnumerable<ListTableItemViewModel>> GetAllSkillsAsync(string? filter)
    {
        IEnumerable<ListTableItemViewModel> skills = await this._repository
            .GetAllSkills()
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
        Skill skill = await this._repository
            .GetSkillWithExercisesByIdAsync(id);

        DetailsSkillViewModel model = new DetailsSkillViewModel()
        {
            Name = skill.Name,
            Description = skill.Description,
            ImageUrl = skill.ImageUrl,
            Measurement = skill.MeasurementType,
            Category = skill.Category,
            SkillType = skill.SkillType,
            Difficulty = skill.Difficulty,
            Exercises = skill.Exercises
        };

        return model;
    }

    public async Task<bool> SkillExistsAsync(Guid id)
        => await this._repository.SkillExistsAsync(id);
}
