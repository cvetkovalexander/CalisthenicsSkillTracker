using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Moq;

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
}
