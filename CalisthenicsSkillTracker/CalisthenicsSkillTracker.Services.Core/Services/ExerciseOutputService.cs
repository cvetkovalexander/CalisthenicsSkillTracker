using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class ExerciseOutputService : IExerciseOutputService
{
    private readonly IExerciseRepository _repository;

    public ExerciseOutputService(IExerciseRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IEnumerable<ListTableItemViewModel>> GetAllExercisesAsync(string? filter)
    {
        IQueryable<Exercise> exercisesQuery = this._repository
            .GetAllExercises()
            .OrderBy(e => e.Name);

        if (filter is not null)
            exercisesQuery = exercisesQuery.Where(e => EF.Functions.Like(e.Name, $"%{filter}%"));

        IEnumerable<ListTableItemViewModel> exercises = await exercisesQuery
            .Select(e => new ListTableItemViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Difficulty = e.Difficulty
            })
            .ToArrayAsync();

        return exercises;
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
}
