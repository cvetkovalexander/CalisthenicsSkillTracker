using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class ExerciseOutputService : IExerciseOutputService
{
    private readonly ApplicationDbContext _context;

    public ExerciseOutputService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<ListTableItemViewModel>> GetAllExercisesAsync(string? filter)
    {
        IEnumerable<ListTableItemViewModel> exercises = await this._context
            .Exercises
            .AsNoTracking()
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
        Exercise exercise = await this._context
            .Exercises
            .Include(e => e.Skills)
            .AsNoTracking()
            .SingleAsync(e => e.Id == id);

        DetailsExerciseViewModel model = new DetailsExerciseViewModel()
        {
            Name = exercise.Name,
            Description = exercise.Description,
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
    {
        return await this._context
            .Exercises
            .AnyAsync(e => e.Id == id);
    }
}
