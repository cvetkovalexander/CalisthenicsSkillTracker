using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;
using CalisthenicsSkillTracker.Services.Core.Contracts.Admin;
using CalisthenicsSkillTracker.Services.Core.Services.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.Stats;
using Moq;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Services.Tests.Admin;

[TestFixture]
public class StatsServiceTests
{
    private Mock<IStatsRepository> _repositoryMock;

    private IStatsService _service;

    [SetUp]
    public void SetUp() 
    {
        this._repositoryMock = new Mock<IStatsRepository>();
        this._service = new StatsService(this._repositoryMock.Object);
    }

    [Test]
    public async Task CreateOverviewViewModelAsync_DataExists_DoesReturnPopulatedViewModel()
    {
        this._repositoryMock.Setup(r => r.CountAsync<Exercise>()).ReturnsAsync(5);
        this._repositoryMock.Setup(r => r.CountAsync<Skill>()).ReturnsAsync(4);
        this._repositoryMock.Setup(r => r.CountAsync<Workout>()).ReturnsAsync(3);
        this._repositoryMock.Setup(r => r.CountAsync<SkillProgress>()).ReturnsAsync(2);

        List<Workout> workouts = new List<Workout>
        {
            new Workout
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Notes = "Workout notes",
                User = new ApplicationUser { UserName = "User1" },
                WorkoutExercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        Exercise = new Exercise { Name = "Push Ups" }
                    }
                }
            }
        };

            List<SkillProgress> skillRecords = new List<SkillProgress>
        {
            new SkillProgress
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Notes = "Skill notes",
                PerformedBy = new ApplicationUser { UserName = "User1" },
                Skill = new Skill { Name = "Planche" },
                Progression = Progression.AdvancedTuck
            }
        };

        this._repositoryMock
            .Setup(r => r.GetAllWorkoutsAsync(
                It.IsAny<Expression<Func<Workout, bool>>>(),
                It.IsAny<Expression<Func<Workout, Workout>>?>()))
            .ReturnsAsync(workouts);

        this._repositoryMock
            .Setup(r => r.GetAllSkillRecordsAsync(
                It.IsAny<Expression<Func<SkillProgress, bool>>>(),
                It.IsAny<Expression<Func<SkillProgress, SkillProgress>>?>()))
            .ReturnsAsync(skillRecords);

        OverviewViewModel result = await this._service.CreateOverviewViewModelAsync();

        Assert.That(result, Is.Not.Null);

        Assert.That(result.TotalExercises, Is.EqualTo(5));
        Assert.That(result.TotalSkills, Is.EqualTo(4));
        Assert.That(result.TotalWorkouts, Is.EqualTo(3));
        Assert.That(result.TotalSkillRecords, Is.EqualTo(2));

        Assert.That(result.Workouts, Is.Not.Null);
        Assert.That(result.Workouts.Any(), Is.True);

        Assert.That(result.SkillRecords, Is.Not.Null);
        Assert.That(result.SkillRecords.Any(), Is.True);
    }

    [Test]
    public async Task CreateOverviewViewModelAsync_NoData_DoesReturnEmptyCollectionsButNotNull()
    {
        this._repositoryMock.Setup(r => r.CountAsync<Exercise>()).ReturnsAsync(0);
        this._repositoryMock.Setup(r => r.CountAsync<Skill>()).ReturnsAsync(0);
        this._repositoryMock.Setup(r => r.CountAsync<Workout>()).ReturnsAsync(0);
        this._repositoryMock.Setup(r => r.CountAsync<SkillProgress>()).ReturnsAsync(0);

        this._repositoryMock
            .Setup(r => r.GetAllWorkoutsAsync(
                It.IsAny<Expression<Func<Workout, bool>>>(),
                It.IsAny<Expression<Func<Workout, Workout>>?>()))
            .ReturnsAsync(new List<Workout>());

        this._repositoryMock
            .Setup(r => r.GetAllSkillRecordsAsync(
                It.IsAny<Expression<Func<SkillProgress, bool>>>(),
                It.IsAny<Expression<Func<SkillProgress, SkillProgress>>?>()))
            .ReturnsAsync(new List<SkillProgress>());

        OverviewViewModel result = await this._service.CreateOverviewViewModelAsync();

        Assert.That(result, Is.Not.Null);

        Assert.That(result.TotalExercises, Is.EqualTo(0));
        Assert.That(result.TotalSkills, Is.EqualTo(0));
        Assert.That(result.TotalWorkouts, Is.EqualTo(0));
        Assert.That(result.TotalSkillRecords, Is.EqualTo(0));

        Assert.That(result.Workouts, Is.Not.Null);
        Assert.That(result.Workouts.Any(), Is.False);

        Assert.That(result.SkillRecords, Is.Not.Null);
        Assert.That(result.SkillRecords.Any(), Is.False);
    }

    [Test]
    public void ProgressDisplayName_NoValuesExist_DoesReturnEmptyString()
    {
        SkillProgress skillProgress = new SkillProgress
        {
            Progression = null,
            Repetitions = null,
            Duration = null,
        };

        string result = this._service.ProgressDisplayName(skillProgress);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void ProgressDisplayName_ProgressionRepetitionsAndDurationExist_DoesReturnFormattedString()
    {
        SkillProgress skillProgress = new SkillProgress
        {
            Progression = Progression.AdvancedTuck,
            Repetitions = 5,
            Duration = 10,
        };

        string result = this._service.ProgressDisplayName(skillProgress);

        Assert.That(result, Is.EqualTo("AdvancedTuck 5 reps 10 secs"));
    }

    [Test]
    public async Task CreateWorkoutViewModelsAsync_WorkoutsExist_DoesReturnMappedWorkoutViewModels()
    {
        DateTime submittedOn = DateTime.UtcNow;

        List<Workout> workouts = new List<Workout>
        {
            new Workout
            {
                Id = Guid.NewGuid(),
                Date = submittedOn,
                Notes = "Workout notes",
                User = new ApplicationUser
                {
                    UserName = "TestUser",
                },
                WorkoutExercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        Exercise = new Exercise
                        {
                            Name = "Push Ups",
                        },
                    },
                    new WorkoutExercise
                    {
                        Exercise = new Exercise
                        {
                            Name = "Pull Ups",
                        },
                    },
                },
            },
        };

        this._repositoryMock
            .Setup(r => r.GetAllWorkoutsAsync(
                It.IsAny<Expression<Func<Workout, bool>>>(),
                It.IsAny<Expression<Func<Workout, Workout>>?>()))
            .ReturnsAsync(workouts);

        IEnumerable<WorkoutViewModel> result = await this._service.CreateWorkoutViewModelsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));

        WorkoutViewModel workoutViewModel = result.First();

        Assert.That(workoutViewModel.UserName, Is.EqualTo("TestUser"));
        Assert.That(workoutViewModel.SubmittedOn, Is.EqualTo(submittedOn));
        Assert.That(workoutViewModel.Notes, Is.EqualTo("Workout notes"));
        Assert.That(workoutViewModel.Exercises, Is.Not.Null);
        Assert.That(workoutViewModel.Exercises.Count, Is.EqualTo(2));
        Assert.That(workoutViewModel.Exercises, Does.Contain("Push Ups"));
        Assert.That(workoutViewModel.Exercises, Does.Contain("Pull Ups"));

        this._repositoryMock.Verify(r => r.GetAllWorkoutsAsync(
            It.IsAny<Expression<Func<Workout, bool>>>(),
            It.IsAny<Expression<Func<Workout, Workout>>?>()),
            Times.Once);
    }

    [Test]
    public async Task CreateWorkoutViewModelsAsync_NoWorkoutsExist_DoesReturnEmptyCollectionButNotNull()
    {
        this._repositoryMock
            .Setup(r => r.GetAllWorkoutsAsync(
                It.IsAny<Expression<Func<Workout, bool>>>(),
                It.IsAny<Expression<Func<Workout, Workout>>?>()))
            .ReturnsAsync(new List<Workout>());

        IEnumerable<WorkoutViewModel> result = await this._service.CreateWorkoutViewModelsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Any(), Is.False);

        this._repositoryMock.Verify(r => r.GetAllWorkoutsAsync(
            It.IsAny<Expression<Func<Workout, bool>>>(),
            It.IsAny<Expression<Func<Workout, Workout>>?>()),
            Times.Once);
    }
}
