using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
namespace CalisthenicsSkillTracker.Services.Core.Services
{
    // TODO: Separate the helper methods in a individual service.

    public class WorkoutService : IWorkoutService
    {
        private readonly ApplicationDbContext _context;

        public WorkoutService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Workout> CreateWorkoutAsync(CreateWorkoutViewModel model, TimeSpan start, TimeSpan end)
        {
            Workout workout = new Workout()
            {
                Date = model.Date,
                UserId = model.UserId,
                Notes = model.Notes,
                Start = start,
                End = end
            };

            await this._context.Workouts.AddAsync(workout);
            await this._context.SaveChangesAsync();

            return workout;
        }

        public async Task CreateWorkoutExerciseAsync(AddWorkoutExerciseViewModel model)
        {
            WorkoutExercise workoutExercise = new WorkoutExercise()
            {
                WorkoutId = model.WorkoutId,
                ExerciseId = model.ExerciseId
            };

            await this._context.WorkoutExercises.AddAsync(workoutExercise);
            await this._context.SaveChangesAsync();
        }

        public async Task CreateWorkoutSetAsync(AddWorkoutSetViewModel model)
        {
            WorkoutSet set = new WorkoutSet()
            {
                WorkoutExerciseId = model.WorkoutExerciseId,
                SetNumber = model.SetNumber,
                Repetitions = model.Repetitions,
                Duration = model.Duration,
                Progression = model.Progression,
                Notes = model.Notes
            };

            await this._context.WorkoutSets.AddAsync(set);
            await this._context.SaveChangesAsync();
        }

        public async Task<Workout> GetWorkoutWithExercisesAsync(Guid id)
        {
            return await this._context.Workouts
                .AsNoTracking()
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstAsync(w => w.Id == id);
        }

        public CreateWorkoutViewModel CreateWorkoutViewModel(string userId)
        {
            return new CreateWorkoutViewModel()
            {
                UserId = userId,
                Date = DateTime.UtcNow
            };
        }

        public async Task<AddWorkoutExerciseViewModel> CreateWorkoutExerciseViewModelAsync(Guid workoutId)
        {
            Workout workout = await this._context
                .Workouts
                .AsNoTracking()
                .Include(w => w.WorkoutExercises)
                .SingleAsync(w => w.Id == workoutId);

            return new AddWorkoutExerciseViewModel()
            {
                AvailabeExercises = await this.FetchExercisesAsync(),
                WorkoutId = workoutId,
                HasExercises = workout.WorkoutExercises.Any()
            };
        }

        public AddWorkoutSetViewModel AddWorkoutSetViewModel(Workout workout)
        {
            return new AddWorkoutSetViewModel()
            {
                WorkoutId = workout.Id,
                Exercises = this.GetWorkoutExercises(workout),
                Progressions = this.FetchProgressions()
            };
        }

        public async Task<WorkoutExercise> GetWorkoutExerciseAsync(Guid workoutId, Guid workoutExerciseId)
        {
            return await this._context
                .WorkoutExercises
                .FirstAsync(we => we.WorkoutId == workoutId && we.Id == workoutExerciseId);
        }

        /* Helper methods */
        public async Task<bool> ExerciseExistsAsync(Guid id)
        {
            return await this._context
                .Exercises
                .AnyAsync(e => e.Id == id);
        }

        public async Task<List<SelectListItem>> FetchExercisesAsync()
        {
            return await this._context
            .Exercises
            .AsNoTracking()
            .Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            })
            .ToListAsync();
        }
        public List<SelectListItem> GetWorkoutExercises(Workout workout)
        {
            return workout.WorkoutExercises
                .Select(we => new SelectListItem
                {
                    Value = we.Id.ToString(),
                    Text = we.Exercise.Name
                })
                .ToList();
        }

        public List<SelectListItem> FetchProgressions()
        {
            return Enum.GetValues<Progression>()
                .Select(p => new SelectListItem
                {
                    Value = p.ToString(),
                    Text = p.ToString()
                })
                .ToList();
        }

        public async Task<List<SelectListItem>> GetWorkoutExercisesAsync(Guid id)
        {
            Workout workout = await this._context
                .Workouts
                .AsNoTracking()
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .SingleAsync(w => w.Id == id);

            return this.GetWorkoutExercises(workout);
        }

        public async Task<bool> WorkoutExerciseExistsAsync(Guid workoutId, Guid workoutExerciseId)
        {
            return await this._context.
                WorkoutExercises
                .AnyAsync(we => we.WorkoutId == workoutId && we.Id == workoutExerciseId);
        }

        public async Task<bool> WorkoutExistsAsync(Guid id)
        {
            return await this._context
                .Workouts
                .AnyAsync(w => w.Id == id);
        }

        public async Task<bool> UserExistsAsync(string id)
        {
            return await this._context
                .Users
                .AnyAsync(u => u.Id == id);
        }

        public bool isTimeValid(string input, out TimeSpan output)
        {
            return TimeSpan.TryParseExact(
                input,
                new[] { @"h\:mm", @"hh\:mm" },
                CultureInfo.InvariantCulture,
                out output);
        }

        public async Task<bool> ExerciseAlreadyAddedAsync(Guid workoutId, Guid exerciseId)
        {
            Workout workout  = await this._context
                .Workouts
                .Include(w => w.WorkoutExercises)
                .AsNoTracking()
                .FirstAsync(w => w.Id == workoutId);

            return workout.WorkoutExercises.Any(e => e.ExerciseId == exerciseId);
        }

        public async Task<IEnumerable<WorkoutDetailsViewModel>> CreateWorkoutDetailsViewModelsAsync(string userId)
        {
            IEnumerable<Workout> workouts = await this._context
                .Workouts
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .ThenByDescending(w => w.Start)
                .AsNoTracking()
                .ToListAsync();

            IEnumerable<WorkoutDetailsViewModel> viewModel = workouts.Select(w => new WorkoutDetailsViewModel
            {
                Id = w.Id,
                Date = w.Date,
                Start = w.Start,
                End = w.End,
                Duration = w.Duration,
                Notes = w.Notes,  
                
            }).ToList();

            return viewModel;
        }

        public async Task<Workout> GetWorkoutWithExercisesAndSetsAsync(Guid id, string userId)
        {
            return await this._context
                .Workouts
                .Where(w => w.UserId == userId && w.Id == id)
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Sets)
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .AsNoTracking()
                .FirstAsync();
        }

        public WorkoutExercisesViewModel GetWorkoutExercisesDetailsViewModel(Workout workout)
        {
            WorkoutExercisesViewModel viewModel = new WorkoutExercisesViewModel
            {
                WorkoutId = workout.Id,
                Exercises = workout.WorkoutExercises.Select(we => new WorkoutExerciseDetailsViewModel
                {
                    Id = we.Id,
                    ExerciseName = we.Exercise.Name,
                    Sets = we.Sets.Select(ws => new WorkoutSetDetailsViewModel
                    {
                        Id = ws.Id,
                        SetNumber = ws.SetNumber,
                        Repetitions = ws.Repetitions,
                        Duration = ws.Duration,
                        Progression = ws.Progression,
                        Notes = ws.Notes,     
                    }).ToList()
                }).ToList()
            };

            return viewModel;
        }
    }
}
