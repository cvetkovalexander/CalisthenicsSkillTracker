using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;

using Microsoft.EntityFrameworkCore;

using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillOutputService : ISkillOutputService
{
    private readonly ISkillRepository _repository;

    public SkillOutputService(ISkillRepository repository)
    {
        this._repository = repository;
    }

    public async Task<PaginationResultViewModel<ListTableItemViewModel>> GetAllSkillsAsync(string? indexName, Guid? indexId, bool isPreviousPage, Guid? userId, string? filter = null, int pageSize = DefaultPageSize)
    {
        IQueryable<Skill> query = this._repository.GetAllSkills();

        query = ApplyFiltering(query, filter);
        query = ApplyOrdering(query, isPreviousPage);
        query = ApplyPagination(query, indexName, indexId, isPreviousPage);

        IQueryable<ListTableItemViewModel> projectedQuery = ProjectSkills(query);

        List<ListTableItemViewModel> items = await GetPagedSkillsAsync(projectedQuery, pageSize, isPreviousPage);

        if (userId.HasValue)
        {
            HashSet<Guid> favoritedSkillsIds = await this._repository.GetUserFavoriteSkills(userId.Value);

            foreach (ListTableItemViewModel item in items)
                item.IsFavorited = favoritedSkillsIds.Contains(item.Id);
        }

        return CreatePaginationViewModel(items, filter, pageSize, indexName, indexId, isPreviousPage);
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

    private static IQueryable<Skill> ApplyFiltering(IQueryable<Skill> query, string? filter)
    {
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(s => EF.Functions.Like(s.Name, $"%{filter}%"));
        }
        return query;
    }

    private static IQueryable<Skill> ApplyOrdering(IQueryable<Skill> query, bool isPreviousPage) 
    {
        if (isPreviousPage)
            query = query.OrderByDescending(s => s.Name).ThenByDescending(s => s.Id);
        else
            query = query.OrderBy(s => s.Name).ThenBy(s => s.Id);

        return query;
    }

    private static IQueryable<Skill> ApplyPagination(IQueryable<Skill> query, string? indexName, Guid? indexId, bool isPreviousPage) 
    {
        if (string.IsNullOrWhiteSpace(indexName) || !indexId.HasValue)
            return query;

        if (isPreviousPage)
            query = query.Where(s =>
                string.Compare(s.Name, indexName) < 0 ||
                (s.Name == indexName && s.Id.CompareTo(indexId.Value) < 0));
        else
            query = query.Where(s =>
                string.Compare(s.Name, indexName) > 0 ||
                (s.Name == indexName && s.Id.CompareTo(indexId.Value) > 0));

        return query;
    }

    private static IQueryable<ListTableItemViewModel> ProjectSkills(IQueryable<Skill> query) 
    {
        return query.Select(s => new ListTableItemViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Difficulty = s.Difficulty,
            ImageUrl = s.ImageUrl,
        });
    }

    private async Task<List<ListTableItemViewModel>> GetPagedSkillsAsync(IQueryable<ListTableItemViewModel> query, int pageSize, bool isPreviousPage) 
    {
        List<ListTableItemViewModel> skills = await query
            .Take(pageSize + 1)
            .ToListAsync();

        if (isPreviousPage)
            skills = skills
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Id)
                .ToList();

        return skills;
    }

    private static PaginationResultViewModel<ListTableItemViewModel> CreatePaginationViewModel(List<ListTableItemViewModel> items, string? filter, int pageSize, string? indexName, Guid? indexId, bool isPreviousPage) 
    {
        bool hasMoreItems = items.Count > pageSize;

        if (hasMoreItems)
            items = items.Take(pageSize).ToList();

        ListTableItemViewModel? firstItem = items.FirstOrDefault();
        ListTableItemViewModel? lastItem = items.LastOrDefault();

        bool hasIndex = !string.IsNullOrWhiteSpace(indexName) && indexId.HasValue;

        bool hasPreviousPage = isPreviousPage ? hasMoreItems : hasIndex;
        bool hasNextPage = isPreviousPage ? hasIndex : hasMoreItems;

        return new PaginationResultViewModel<ListTableItemViewModel>
        {
            Items = items,
            Filter = filter,
            PageSize = pageSize,
            HasNextPage = hasNextPage,
            HasPreviousPage = hasPreviousPage,
            NextIndexName = hasNextPage ? lastItem?.Name : null,
            NextIndexId = hasNextPage ? lastItem?.Id : null,
            PreviousIndexName = hasPreviousPage ? firstItem?.Name : null,
            PreviousIndexId = hasPreviousPage ? firstItem?.Id : null
        };
    }
}
