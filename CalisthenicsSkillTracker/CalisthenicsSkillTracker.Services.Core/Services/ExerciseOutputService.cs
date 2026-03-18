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
        IEnumerable<ListTableItemViewModel> exercises = await this._repository
            .GetAllExercises()
            .OrderBy(e => e.Name)
            .Select(e => new ListTableItemViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Difficulty = e.Difficulty
            })
            .ToArrayAsync();

        if (filter is not null)
            exercises = exercises.Where(s => s.Name.ToLower().Contains(filter.ToLower()));

        return exercises;
    }

    public async Task<DetailsExerciseViewModel> GetExerciseDetailsAsync(Guid id)
    {
        Exercise exercise = await this._repository
            .GetExerciseByIdAsync(id);

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
