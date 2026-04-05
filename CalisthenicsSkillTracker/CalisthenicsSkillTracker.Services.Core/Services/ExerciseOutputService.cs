using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using Microsoft.EntityFrameworkCore;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class ExerciseOutputService : IExerciseOutputService
{
    private readonly IExerciseRepository _repository;

    public ExerciseOutputService(IExerciseRepository repository)
    {
        this._repository = repository;
    }

    public async Task<PaginationResultViewModel<ListTableItemViewModel>> GetAllExercisesAsync(string? indexName, Guid? indexId, bool isPreviousPage, Guid? userId, string? filter = null, int pageSize = DefaultPageSize)
    {
        IQueryable<Exercise> query = this._repository.GetAllExercises();

        query = ApplyFiltering(query, filter);
        query = ApplyOrdering(query, isPreviousPage);
        query = ApplyPagination(query, indexName, indexId, isPreviousPage);

        IQueryable<ListTableItemViewModel> projectedQuery = ProjectExercises(query);

        List<ListTableItemViewModel> items = await GetPagedExercisesAsync(projectedQuery, pageSize, isPreviousPage);

        if (userId.HasValue) 
        {
            HashSet<Guid> favoritedExerciseIds = await this._repository.GetUserFavoriteExercises(userId.Value);

            foreach (ListTableItemViewModel item in items)
                item.IsFavorited = favoritedExerciseIds.Contains(item.Id);
        }

        return CreatePaginationViewModel(items, filter, pageSize, indexName, indexId, isPreviousPage);
    }
   
    public async Task<DetailsExerciseViewModel> GetExerciseDetailsAsync(Guid id)
    {
        Exercise exercise = await this._repository
            .GetExerciseWithSkillsAsync(id);

        DetailsExerciseViewModel model = new DetailsExerciseViewModel()
        {
            Name = exercise.Name,
            Description = exercise.Description,
            ImageUrl = exercise.ImageUrl,
            Difficulty = exercise.Difficulty,
            Measurement = exercise.MeasurementType,
            Category = exercise.Category,
            ExerciseType = exercise.ExerciseType,
            Skills = exercise
                .Skills
        };

        return model;
    }

    /* Helper methods */

    public async Task<bool> ExerciseExistsAsync(Guid id)
        => await this._repository.ExerciseExistsAsync(id);

    public static IQueryable<Exercise> ApplyFiltering(IQueryable<Exercise> query, string? filter)
    {
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(e => e.Name.Contains(filter));
        }
        return query;
    }

    public static IQueryable<Exercise> ApplyOrdering(IQueryable<Exercise> query, bool isPreviousPage)
    {
        if (isPreviousPage)
            query = query.OrderByDescending(e => e.Name).ThenByDescending(e => e.Id);
        else
            query = query.OrderBy(e => e.Name).ThenBy(e => e.Id);

        return query;
    }

    public static IQueryable<Exercise> ApplyPagination(IQueryable<Exercise> query, string? indexName, Guid? indexId, bool isPreviousPage)
    {
        if (string.IsNullOrWhiteSpace(indexName) || !indexId.HasValue)
            return query;

        if (isPreviousPage)
            query = query.Where(e =>
                string.Compare(e.Name, indexName) < 0 ||
                (e.Name == indexName && e.Id.CompareTo(indexId.Value) < 0));
        else
            query = query.Where(e =>
                string.Compare(e.Name, indexName) > 0 ||
                (e.Name == indexName && e.Id.CompareTo(indexId.Value) > 0));

        return query;
    }

    public static IQueryable<ListTableItemViewModel> ProjectExercises(IQueryable<Exercise> query)
    {
        return query.Select(e => new ListTableItemViewModel
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Difficulty = e.Difficulty,
            ImageUrl = e.ImageUrl
        });
    }

    public async Task<List<ListTableItemViewModel>> GetPagedExercisesAsync(IQueryable<ListTableItemViewModel> query, int pageSize, bool isPreviousPage)
    {
        List<ListTableItemViewModel> exercises = await query
            .Take(pageSize + 1)
            .ToListAsync();

        if (isPreviousPage)
            exercises = exercises
                .OrderBy(e => e.Name)
                .ThenBy(e => e.Id)
                .ToList();

        return exercises;
    }

    public static PaginationResultViewModel<ListTableItemViewModel> CreatePaginationViewModel(List<ListTableItemViewModel> items, string? filter, int pageSize, string? indexName, Guid? indexId, bool isPreviousPage)
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
