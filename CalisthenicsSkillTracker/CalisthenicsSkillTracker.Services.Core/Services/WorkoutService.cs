using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Services.Core.Services
{
    // TODO: Separate the helper methods in a individual service.

    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _repository;

        public WorkoutService(IWorkoutRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Workout> CreateWorkoutAsync(CreateWorkoutViewModel model, TimeSpan start, TimeSpan end)
        {
            Workout workout = new Workout()
            {
                Date = model.Date,
                UserId = Guid.Parse(model.UserId),
                Notes = model.Notes,
                Start = start,
                End = end
            };

            bool successfulAdd = await this._repository.AddWorkoutAsync(workout);
            if (!successfulAdd)
                throw new EntityCreatePersistException();

            return workout;
        }

        public async Task CreateWorkoutExerciseAsync(AddWorkoutExerciseViewModel model)
        {
            WorkoutExercise workoutExercise = new WorkoutExercise()
            {
                WorkoutId = model.WorkoutId,
                ExerciseId = model.ExerciseId
            };

            bool successfulAdd = await this._repository.AddWorkoutExerciseAsync(workoutExercise);
            if (!successfulAdd)
                throw new EntityCreatePersistException();

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

            bool successfulAdd = await this._repository.AddWorkoutSetAsync(set);
            if (!successfulAdd)
                throw new EntityCreatePersistException();
        }

        public async Task<Workout> GetWorkoutWithExercisesAsync(Guid id)
        {
            return await this._repository
                .GetWorkoutWithExercisesAsync(id);
        }

        public async Task<WorkoutExercise> GetWorkoutExerciseAsync(Guid workoutId, Guid workoutExerciseId)
        {
            return await this._repository
                .GetWorkoutExerciseAsync(workoutId, workoutExerciseId);
        }

        public CreateWorkoutViewModel CreateWorkoutViewModel(string userId)
        {
            return new CreateWorkoutViewModel()
            {
                UserId = userId,
                Date = DateTime.UtcNow
            };
        }

        public AddWorkoutSetViewModel CreateAddWorkoutSetViewModel(Workout workout)
        {
            return new AddWorkoutSetViewModel()
            {
                WorkoutId = workout.Id,
                Exercises = this.FetchWorkoutExercises(workout),
                Progressions = this.FetchProgressions(),
                ExerciseMeasurementTypes = this.FetchWorkoutMeasurementTypes(workout.Id)
            };
        }

        public async Task<AddWorkoutExerciseViewModel> CreateWorkoutExerciseViewModelAsync(Guid workoutId)
        {
            Workout workout = await this._repository
                .GetWorkoutWithExercisesAsync(workoutId);

            return new AddWorkoutExerciseViewModel()
            {
                AvailableExercises = await this.FetchExercisesAsync(),
                WorkoutId = workoutId,
                HasExercises = workout.WorkoutExercises.Any()
            };
        }

        public async Task<IEnumerable<WorkoutDetailsViewModel>> CreateWorkoutDetailsViewModelsAsync(string userId)
        {
            // Fetch data
            IEnumerable<Workout> workouts = await this._repository
                .GetAllUserWorkoutsWithProjectionAsync(userId, w => new Workout
                {
                    Id = w.Id,
                    Date = w.Date,
                    Start = w.Start,
                    End = w.End,
                    Duration = w.Duration,
                    Notes = w.Notes,
                });

            // Process data
            IEnumerable<WorkoutDetailsViewModel> viewModels = workouts
                .Select(w => new WorkoutDetailsViewModel 
                {
                    Id = w.Id,
                    Date = w.Date,
                    Start = w.Start,
                    End = w.End,
                    Duration = w.Duration,
                    Notes = w.Notes,
                })
                .OrderByDescending(w => w.Date)
                .ThenByDescending(w => w.Start)
                .ToArray();

            // Return data
            return viewModels;
        }

        public async Task<Workout> GetWorkoutWithExercisesAndSetsAsync(Guid id, string userId)
        {
            return await this._repository
                .GetWorkoutWithExercisesAndSetsAsync(id, userId);
        }

        public WorkoutExercisesViewModel GetWorkoutExercisesDetailsViewModel(Workout workout)
        {
            WorkoutExercisesViewModel viewModel = new WorkoutExercisesViewModel
            {
                WorkoutId = workout.Id,
                Exercises = workout.WorkoutExercises.Select(we => new WorkoutExerciseDetailsViewModel
                {
                    Id = we.Id,
                    WorkoutId = we.WorkoutId,
                    ExerciseId = we.ExerciseId,
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

        public async Task<List<SelectListItem>> FetchExercisesAsync()
        {
            return await this._repository
                .GetAllExercisesAsNoTracking()
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                })
                .ToListAsync();
        }

        public List<SelectListItem> FetchWorkoutExercises(Workout workout)
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

        public Dictionary<Guid, string> FetchWorkoutMeasurementTypes(Guid workoutId)
            =>  this._repository.GetWorkoutWithExercisesAsync(workoutId)
                .Result
                .WorkoutExercises
                .ToDictionary(
                    we => we.Id,
                    we => we.Exercise.MeasurementType.ToString());

        public async Task<List<SelectListItem>> GetWorkoutExercisesAsync(Guid id)
        {
            Workout workout = await this._repository
                .GetWorkoutWithExercisesAsync(id);

            return this.FetchWorkoutExercises(workout);
        }

        public async Task<bool> WorkoutExerciseExistsAsync(Guid workoutId, Guid workoutExerciseId)
        {
            return await this._repository
                .WorkoutExerciseExistsAsync(workoutId, workoutExerciseId);
        }

        public bool IsTimeValid(string input, out TimeSpan output)
        {
            return TimeSpan.TryParseExact(
                input,
                new[] { @"h\:mm", @"hh\:mm" },
                CultureInfo.InvariantCulture,
                out output);
        }

        public async Task<bool> ExerciseAlreadyAddedAsync(Guid workoutId, Guid exerciseId)
        {
            Workout workout = await this._repository
                .GetWorkoutWithExercisesAsync(workoutId);

            return workout.WorkoutExercises.Any(e => e.ExerciseId == exerciseId);
        }

        public async Task<bool> EntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
            => await this._repository.EntityExistsAsync(predicate);
    }
}
