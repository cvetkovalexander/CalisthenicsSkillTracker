using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Services.Tests;

[TestFixture]
public class WorkoutServiceTests
{
    private Mock<IWorkoutRepository> _repositoryMock;
    private IWorkoutService _service;

    [SetUp]
    public void SetUp()
    {
        this._repositoryMock = new Mock<IWorkoutRepository>();

        this._service = new WorkoutService(
            this._repositoryMock.Object);
    }

    [Test]
    public async Task CreateWorkoutAsync_EntitySuccessfullPersist_WhenRepositoryPersistsSuccessfully_DoesNotThrowExceptionAndReturnsEntity()
    {
        var userId = Guid.NewGuid();

        CreateWorkoutViewModel model = new CreateWorkoutViewModel
        {
            UserId = userId.ToString(),
            Date = DateTime.UtcNow,
            Start = "18:00",
            End = "19:15",
            Notes = "Good workout"
        };

        TimeSpan start = new TimeSpan(18, 0, 0);
        TimeSpan end = new TimeSpan(19, 15, 0);

        Workout? createdWorkout = null;

        this._repositoryMock
            .Setup(r => r.AddWorkoutAsync(It.IsAny<Workout>()))
            .Callback<Workout>(w => createdWorkout = w)
            .ReturnsAsync(true);

        Workout result = await this._service.CreateWorkoutAsync(model, start, end);

        Assert.That(result, Is.Not.Null);
        Assert.That(createdWorkout, Is.Not.Null);
        Assert.That(result, Is.SameAs(createdWorkout));

        Assert.That(result.Date, Is.EqualTo(model.Date));
        Assert.That(result.UserId, Is.EqualTo(userId));
        Assert.That(result.Start, Is.EqualTo(start));
        Assert.That(result.End, Is.EqualTo(end));
        Assert.That(result.Notes, Is.EqualTo(model.Notes));

        this._repositoryMock.Verify(
            r => r.AddWorkoutAsync(It.Is<Workout>(w =>
                w.Date == model.Date &&
                w.UserId == userId &&
                w.Start == start &&
                w.End == end &&
                w.Notes == model.Notes)),
            Times.Once);
    }

    [Test]
    public async Task CreateWorkoutAsync_EntityFailPersist_DoesThrowException()
    {
        var userId = Guid.NewGuid();

        CreateWorkoutViewModel model = new CreateWorkoutViewModel
        {
            UserId = userId.ToString(),
            Date = new DateTime(2026, 4, 3),
            Start = "18:00",
            End = "19:15",
            Notes = "Good workout"
        };

        TimeSpan start = new TimeSpan(18, 0, 0);
        TimeSpan end = new TimeSpan(19, 15, 0);

        this._repositoryMock
            .Setup(r => r.AddWorkoutAsync(It.IsAny<Workout>()))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityCreatePersistException>(async () =>
            await this._service.CreateWorkoutAsync(model, start, end));

        this._repositoryMock.Verify(
            r => r.AddWorkoutAsync(It.IsAny<Workout>()),
            Times.Once);
    }

    [Test]
    public async Task CreateWorkoutExerciseAsync_EntitySuccessfullPersist_DoesNotThrowException()
    {
        AddWorkoutExerciseViewModel model = new AddWorkoutExerciseViewModel
        {
            WorkoutId = Guid.NewGuid(),
            ExerciseId = Guid.NewGuid()
        };

        this._repositoryMock
            .Setup(r => r.AddWorkoutExerciseAsync(It.IsAny<WorkoutExercise>()))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.CreateWorkoutExerciseAsync(model));

        this._repositoryMock.Verify(
            r => r.AddWorkoutExerciseAsync(It.Is<WorkoutExercise>(we =>
                we.WorkoutId == model.WorkoutId &&
                we.ExerciseId == model.ExerciseId)),
            Times.Once);
    }

    [Test]
    public async Task CreateWorkoutExerciseAsync_EntityFailPersist_DoesThrowException()
    {
        AddWorkoutExerciseViewModel model = new AddWorkoutExerciseViewModel
        {
            WorkoutId = Guid.NewGuid(),
            ExerciseId = Guid.NewGuid()
        };

        this._repositoryMock
            .Setup(r => r.AddWorkoutExerciseAsync(It.IsAny<WorkoutExercise>()))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityCreatePersistException>(async () =>
            await this._service.CreateWorkoutExerciseAsync(model));

        this._repositoryMock.Verify(
            r => r.AddWorkoutExerciseAsync(It.Is<WorkoutExercise>(we =>
                we.WorkoutId == model.WorkoutId &&
                we.ExerciseId == model.ExerciseId)),
            Times.Once);
    }

    [Test]
    public async Task CreateWorkoutSetAsync_EntitySuccessfullPersist_DoesNotThrowException()
    {
        AddWorkoutSetViewModel model = new AddWorkoutSetViewModel
        {
            WorkoutExerciseId = Guid.NewGuid(),
            Repetitions = 10,
            Notes = "Felt good"
        };
        this._repositoryMock
            .Setup(r => r.AddWorkoutSetAsync(It.IsAny<WorkoutSet>()))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.CreateWorkoutSetAsync(model));

        this._repositoryMock.Verify(
            r => r.AddWorkoutSetAsync(It.Is<WorkoutSet>(ws =>
                ws.WorkoutExerciseId == model.WorkoutExerciseId &&
                ws.Repetitions == model.Repetitions &&
                ws.Notes == model.Notes)),
            Times.Once);
    }

    [Test]
    public async Task CreateWorkoutSetAsync_EntityFailPersist_DoesThrowException()
    {
        AddWorkoutSetViewModel model = new AddWorkoutSetViewModel
        {
            WorkoutExerciseId = Guid.NewGuid(),
            Repetitions = 10,
            Notes = "Felt good"
        };
        this._repositoryMock
            .Setup(r => r.AddWorkoutSetAsync(It.IsAny<WorkoutSet>()))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityCreatePersistException>(async () =>
            await this._service.CreateWorkoutSetAsync(model));

        this._repositoryMock.Verify(
            r => r.AddWorkoutSetAsync(It.Is<WorkoutSet>(ws =>
                ws.WorkoutExerciseId == model.WorkoutExerciseId &&
                ws.Repetitions == model.Repetitions &&
                ws.Notes == model.Notes)),
            Times.Once);
    }

    [Test]
    public void CreateWorkoutViewModel_UserIdIsProvided_DoesReturnViewModelWithUserIdAndCurrentUtcDate()
    {
        string userId = Guid.NewGuid().ToString();
        DateTime beforeCall = DateTime.UtcNow;

        CreateWorkoutViewModel result = this._service.CreateWorkoutViewModel(userId);

        DateTime afterCall = DateTime.UtcNow;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(userId));
        Assert.That(result.Date, Is.GreaterThanOrEqualTo(beforeCall));
        Assert.That(result.Date, Is.LessThanOrEqualTo(afterCall));
    }

    [Test]
    public void GetWorkoutExercisesDetailsViewModel_WorkoutContainsExercisesAndSets_DoesReturnMappedViewModel()
    {
        Guid workoutId = Guid.NewGuid();
        Guid workoutExerciseId = Guid.NewGuid();
        Guid exerciseId = Guid.NewGuid();
        Guid firstSetId = Guid.NewGuid();
        Guid secondSetId = Guid.NewGuid();

        Workout workout = new Workout
        {
            Id = workoutId,
            WorkoutExercises = new List<WorkoutExercise>
        {
            new WorkoutExercise
            {
                Id = workoutExerciseId,
                WorkoutId = workoutId,
                ExerciseId = exerciseId,
                Exercise = new Exercise
                {
                    Id = exerciseId,
                    Name = "Pull Ups",
                },
                Sets = new List<WorkoutSet>
                {
                    new WorkoutSet
                    {
                        Id = firstSetId,
                        SetNumber = 1,
                        Repetitions = 8,
                        Duration = null,
                        Progression = Progression.AdvancedTuck,
                        Notes = "First set",
                    },
                    new WorkoutSet
                    {
                        Id = secondSetId,
                        SetNumber = 2,
                        Repetitions = null,
                        Duration = 20,
                        Progression = null,
                        Notes = "Second set",
                    },
                },
            },
        },
        };

        WorkoutExercisesViewModel result = this._service.GetWorkoutExercisesDetailsViewModel(workout);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.WorkoutId, Is.EqualTo(workoutId));
        Assert.That(result.Exercises, Is.Not.Null);
        Assert.That(result.Exercises.Count, Is.EqualTo(1));

        WorkoutExerciseDetailsViewModel exerciseViewModel = result.Exercises.Single();

        Assert.That(exerciseViewModel.WorkoutId, Is.EqualTo(workoutId));
        Assert.That(exerciseViewModel.Id, Is.EqualTo(workoutExerciseId));
        Assert.That(exerciseViewModel.ExerciseId, Is.EqualTo(exerciseId));
        Assert.That(exerciseViewModel.ExerciseName, Is.EqualTo("Pull Ups"));
        Assert.That(exerciseViewModel.Sets, Is.Not.Null);
        Assert.That(exerciseViewModel.Sets.Count, Is.EqualTo(2));

        WorkoutSetDetailsViewModel firstSet = exerciseViewModel.Sets.Single(s => s.Id == firstSetId);
        Assert.That(firstSet.SetNumber, Is.EqualTo(1));
        Assert.That(firstSet.Repetitions, Is.EqualTo(8));
        Assert.That(firstSet.Duration, Is.Null);
        Assert.That(firstSet.Progression, Is.EqualTo(Progression.AdvancedTuck));
        Assert.That(firstSet.Notes, Is.EqualTo("First set"));

        WorkoutSetDetailsViewModel secondSet = exerciseViewModel.Sets.Single(s => s.Id == secondSetId);
        Assert.That(secondSet.SetNumber, Is.EqualTo(2));
        Assert.That(secondSet.Repetitions, Is.Null);
        Assert.That(secondSet.Duration, Is.EqualTo(20));
        Assert.That(secondSet.Progression, Is.Null);
        Assert.That(secondSet.Notes, Is.EqualTo("Second set"));
    }

    [Test]
    public void GetWorkoutExercisesDetailsViewModel_WorkoutContainsNoExercises_DoesReturnViewModelWithEmptyExercisesCollection()
    {
        Guid workoutId = Guid.NewGuid();

        Workout workout = new Workout
        {
            Id = workoutId,
            WorkoutExercises = new List<WorkoutExercise>(),
        };

        WorkoutExercisesViewModel result = this._service.GetWorkoutExercisesDetailsViewModel(workout);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.WorkoutId, Is.EqualTo(workoutId));
        Assert.That(result.Exercises, Is.Not.Null);
        Assert.That(result.Exercises, Is.Empty);
    }

    [Test]
    public void GetWorkoutExercisesDetailsViewModel_ExerciseContainsNoSets_DoesReturnExerciseWithEmptySetsCollection()
    {
        Guid workoutId = Guid.NewGuid();
        Guid workoutExerciseId = Guid.NewGuid();
        Guid exerciseId = Guid.NewGuid();

        Workout workout = new Workout
        {
            Id = workoutId,
            WorkoutExercises = new List<WorkoutExercise>
        {
            new WorkoutExercise
            {
                Id = workoutExerciseId,
                WorkoutId = workoutId,
                ExerciseId = exerciseId,
                Exercise = new Exercise
                {
                    Id = exerciseId,
                    Name = "Push Ups",
                },
                Sets = new List<WorkoutSet>(),
            },
        },
        };

        WorkoutExercisesViewModel result = this._service.GetWorkoutExercisesDetailsViewModel(workout);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.WorkoutId, Is.EqualTo(workoutId));
        Assert.That(result.Exercises, Is.Not.Null);
        Assert.That(result.Exercises.Count, Is.EqualTo(1));

        WorkoutExerciseDetailsViewModel exerciseViewModel = result.Exercises.Single();

        Assert.That(exerciseViewModel.WorkoutId, Is.EqualTo(workoutId));
        Assert.That(exerciseViewModel.Id, Is.EqualTo(workoutExerciseId));
        Assert.That(exerciseViewModel.ExerciseId, Is.EqualTo(exerciseId));
        Assert.That(exerciseViewModel.ExerciseName, Is.EqualTo("Push Ups"));
        Assert.That(exerciseViewModel.Sets, Is.Not.Null);
        Assert.That(exerciseViewModel.Sets, Is.Empty);
    }

    [TestCase("1:05", 1, 5)]
    [TestCase("9:30", 9, 30)]
    [TestCase("01:05", 1, 5)]
    [TestCase("12:45", 12, 45)]
    [TestCase("00:00", 00, 00)]
    public void IsTimeValid_InputMatchesSupportedFormat_DoesReturnTrue(string input, int hours, int minutes)
    {
        bool result = this._service.IsTimeValid(input, out TimeSpan output);

        Assert.That(result, Is.True);
        Assert.That(output, Is.EqualTo(new TimeSpan(hours, minutes, 0)));
    }

    [TestCase("1:5")]
    [TestCase("123:45")]
    [TestCase("ab:cd")]
    [TestCase("10-30")]
    [TestCase("")]
    [TestCase(" ")]
    public void IsTimeValid_InputDoesNotMatchSupportedFormat_DoesReturnFalse(string input)
    {
        bool result = this._service.IsTimeValid(input, out TimeSpan output);

        Assert.That(result, Is.False);
        Assert.That(output, Is.EqualTo(default(TimeSpan)));
    }

    [Test]
    public void IsTimeValid_InputIsNull_DoesReturnFalse()
    {
        bool result = this._service.IsTimeValid(null!, out TimeSpan output);

        Assert.That(result, Is.False);
        Assert.That(output, Is.EqualTo(default(TimeSpan)));
    }

    [Test]
    public async Task CreateWorkoutDetailsViewModelsAsync_UserHasWorkouts_DoesReturnOrderedWorkoutDetailsViewModels()
    {
        string userId = Guid.NewGuid().ToString();

        DateTime firstDate = new DateTime(2026, 4, 1);
        DateTime secondDate = new DateTime(2026, 4, 2);

        List<Workout> workouts = new List<Workout>
    {
        new Workout
        {
            Id = Guid.NewGuid(),
            Date = firstDate,
            Start = new TimeSpan(8, 0, 0),
            End = new TimeSpan(9, 0, 0),
            Duration = new TimeSpan(1, 0, 0),
            Notes = "First workout",
        },
        new Workout
        {
            Id = Guid.NewGuid(),
            Date = secondDate,
            Start = new TimeSpan(7, 0, 0),
            End = new TimeSpan(8, 0, 0),
            Duration = new TimeSpan(1, 0, 0),
            Notes = "Second workout",
        },
        new Workout
        {
            Id = Guid.NewGuid(),
            Date = secondDate,
            Start = new TimeSpan(9, 0, 0),
            End = new TimeSpan(10, 0, 0),
            Duration = new TimeSpan(1, 0, 0),
            Notes = "Third workout",
        },
    };

        this._repositoryMock
            .Setup(r => r.GetAllUserWorkoutsWithProjectionAsync(
                userId,
                It.IsAny<Func<Workout, Workout>?>()))
            .ReturnsAsync((string _, Func<Workout, Workout>? projectFunc) =>
                projectFunc is null
                    ? workouts
                    : workouts.Select(projectFunc).ToList());

        List<WorkoutDetailsViewModel> result =
            (await this._service.CreateWorkoutDetailsViewModelsAsync(userId)).ToList();

        Assert.That(result, Has.Count.EqualTo(3));

        Assert.That(result[0].Date, Is.EqualTo(secondDate));
        Assert.That(result[0].Start, Is.EqualTo(new TimeSpan(9, 0, 0)));
        Assert.That(result[0].Notes, Is.EqualTo("Third workout"));

        Assert.That(result[1].Date, Is.EqualTo(secondDate));
        Assert.That(result[1].Start, Is.EqualTo(new TimeSpan(7, 0, 0)));
        Assert.That(result[1].Notes, Is.EqualTo("Second workout"));

        Assert.That(result[2].Date, Is.EqualTo(firstDate));
        Assert.That(result[2].Start, Is.EqualTo(new TimeSpan(8, 0, 0)));
        Assert.That(result[2].Notes, Is.EqualTo("First workout"));
    }

    [Test]
    public async Task CreateWorkoutDetailsViewModelsAsync_UserHasNoWorkouts_DoesReturnEmptyCollectionButNotNull()
    {
        string userId = Guid.NewGuid().ToString();
        List<Workout> workouts = new List<Workout>();

        this._repositoryMock
            .Setup(r => r.GetAllUserWorkoutsWithProjectionAsync(
                userId,
                It.IsAny<Func<Workout, Workout>?>()))
            .ReturnsAsync((string _, Func<Workout, Workout>? projectFunc) =>
                projectFunc is null
                    ? workouts
                    : workouts.Select(projectFunc).ToList());

        IEnumerable<WorkoutDetailsViewModel> result =
            await this._service.CreateWorkoutDetailsViewModelsAsync(userId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task ExerciseAlreadyAddedAsync_ExerciseDoesExistsInWorkout_DoesReturnTrue()
    {
        Guid workoutId = Guid.NewGuid();
        Guid existingExerciseId = Guid.NewGuid();

        Workout workout = new Workout
        {
            Id = workoutId,
            WorkoutExercises = new List<WorkoutExercise>
        {
            new WorkoutExercise
            {
                ExerciseId = existingExerciseId,
                Exercise = new Exercise
                {
                    Id = existingExerciseId,
                    Name = "Pull Ups",
                },
            },
            new WorkoutExercise
            {
                ExerciseId = Guid.NewGuid(),
                Exercise = new Exercise
                {
                    Id = Guid.NewGuid(),
                    Name = "Push Ups",
                },
            },
        },
        };

        this._repositoryMock
            .Setup(r => r.GetWorkoutWithExercisesAsync(workoutId))
            .ReturnsAsync(workout);

        bool result = await this._service.ExerciseAlreadyAddedAsync(workoutId, existingExerciseId);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.GetWorkoutWithExercisesAsync(workoutId), Times.Once);
    }

    [Test]
    public async Task ExerciseAlreadyAddedAsync_ExerciseDoesNotExistInWorkout_DoesReturnFalse()
    {
        Guid workoutId = Guid.NewGuid();
        Guid searchedExerciseId = Guid.NewGuid();

        Workout workout = new Workout
        {
            Id = workoutId,
            WorkoutExercises = new List<WorkoutExercise>
        {
            new WorkoutExercise
            {
                ExerciseId = Guid.NewGuid(),
                Exercise = new Exercise
                {
                    Id = Guid.NewGuid(),
                    Name = "Pull Ups",
                },
            },
            new WorkoutExercise
            {
                ExerciseId = Guid.NewGuid(),
                Exercise = new Exercise
                {
                    Id = Guid.NewGuid(),
                    Name = "Push Ups",
                },
            },
        },
        };

        this._repositoryMock
            .Setup(r => r.GetWorkoutWithExercisesAsync(workoutId))
            .ReturnsAsync(workout);

        bool result = await this._service.ExerciseAlreadyAddedAsync(workoutId, searchedExerciseId);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.GetWorkoutWithExercisesAsync(workoutId), Times.Once);
    }

    [Test]
    public async Task EntityExistsAsync_EntityMatchingPredicateExists_DoesReturnTrue()
    {
        Expression<Func<Workout, bool>> predicate = w => w.Id == Guid.NewGuid();

        this._repositoryMock
            .Setup(r => r.EntityExistsAsync(predicate))
            .ReturnsAsync(true);

        bool result = await this._service.EntityExistsAsync(predicate);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.EntityExistsAsync(predicate), Times.Once);
    }

    [Test]
    public async Task EntityExistsAsync_EntityMatchingPredicateDoesNotExist_DoesReturnFalse()
    {
        Expression<Func<Workout, bool>> predicate = w => w.Id == Guid.NewGuid();

        this._repositoryMock
            .Setup(r => r.EntityExistsAsync(predicate))
            .ReturnsAsync(false);

        bool result = await this._service.EntityExistsAsync(predicate);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.EntityExistsAsync(predicate), Times.Once);
    }

    [Test]
    public async Task GetWorkoutExerciseAsync_WorkoutExerciseExists_DoesReturnWorkoutExercise()
    {
        Guid workoutId = Guid.NewGuid();
        Guid workoutExerciseId = Guid.NewGuid();

        WorkoutExercise workoutExercise = new WorkoutExercise
        {
            Id = workoutExerciseId,
            WorkoutId = workoutId,
            ExerciseId = Guid.NewGuid(),
        };

        this._repositoryMock
            .Setup(r => r.GetWorkoutExerciseAsync(workoutId, workoutExerciseId))
            .ReturnsAsync(workoutExercise);

        WorkoutExercise result = await this._service.GetWorkoutExerciseAsync(workoutId, workoutExerciseId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(workoutExercise));

        this._repositoryMock.Verify(r => r.GetWorkoutExerciseAsync(workoutId, workoutExerciseId), Times.Once);
    }
}
