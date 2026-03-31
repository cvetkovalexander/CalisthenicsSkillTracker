using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillOutputService : ISkillOutputService
{
    private readonly ISkillRepository _repository;

    public SkillOutputService(ISkillRepository repository)
    {
        this._repository = repository;
    }

    public async Task<PaginationResultViewModel<ListTableItemViewModel>> GetAllSkillsAsync(string? lastName, Guid? lastId, string? filter = null, int pageSize = DefaultPageSize)
    {
        IQueryable<Skill> skillsQuery = this._repository
            .GetAllSkills()
            .OrderBy(s => s.Name)
            .ThenBy(s => s.Id);

        if (filter is not null)
            skillsQuery = skillsQuery.Where(s => EF.Functions.Like(s.Name, $"%{filter}%"));

        if (!string.IsNullOrWhiteSpace(lastName) && lastId.HasValue) 
        {
            skillsQuery = skillsQuery
                .Where(s => string.Compare(s.Name, lastName) > 0 
                || (s.Name == lastName && s.Id > lastId.Value));
        }

        IEnumerable<ListTableItemViewModel> skills = await skillsQuery
            .Select(s => new ListTableItemViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Difficulty = s.Difficulty
            })
            .Take(pageSize + 1)
            .ToArrayAsync();

        bool hasNextPage = skills.Count() > pageSize;

        if (hasNextPage)
            skills = skills.Take(pageSize).ToArray();

        ListTableItemViewModel? lastSkill = skills.LastOrDefault();

        PaginationResultViewModel<ListTableItemViewModel> model = new PaginationResultViewModel<ListTableItemViewModel>
        {
            Items = skills,
            Filter = filter,
            PageSize = pageSize,
            HasNextPage = hasNextPage,
            NextIndexName = hasNextPage ? lastSkill?.Name : null,
            NextIndexId = hasNextPage ? lastSkill?.Id : null
        };

        return model;
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
