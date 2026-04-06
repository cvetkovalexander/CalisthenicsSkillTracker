using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using Moq;

namespace CalisthenicsSkillTracker.Services.Tests;

[TestFixture]
public class ExerciseOutputServiceTests
{
    private Mock<IExerciseRepository> _repositoryMock;

    private IExerciseOutputService _service;

    [SetUp]
    public void Setup()
    {
        this._repositoryMock = new Mock<IExerciseRepository>();
        this._service = new ExerciseOutputService(this._repositoryMock.Object);
    }

    [Test]
    public async Task GetExerciseDetailsAsync_ExerciseHasSkills_DoesReturnMappedDetailsExerciseViewModel()
    {
        Guid exerciseId = Guid.NewGuid();

        List<Skill> skills = new List<Skill>
    {
        new Skill
        {
            Id = Guid.NewGuid(),
            Name = "Planche",
        },
        new Skill
        {
            Id = Guid.NewGuid(),
            Name = "Front Lever",
        },
    };

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Pseudo Push Ups",
            Description = "Exercise description",
            ImageUrl = "image-url",
            MeasurementType = Measurement.Repetitions,
            Category = Category.Balance,
            ExerciseType = SkillType.Push,
            Difficulty = Difficulty.Intermediate,
            Skills = skills,
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseWithSkillsAsync(exerciseId))
            .ReturnsAsync(exercise);

        DetailsExerciseViewModel result = await this._service.GetExerciseDetailsAsync(exerciseId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(exercise.Name));
        Assert.That(result.Description, Is.EqualTo(exercise.Description));
        Assert.That(result.ImageUrl, Is.EqualTo(exercise.ImageUrl));
        Assert.That(result.Measurement, Is.EqualTo(exercise.MeasurementType));
        Assert.That(result.Category, Is.EqualTo(exercise.Category));
        Assert.That(result.ExerciseType, Is.EqualTo(exercise.ExerciseType));
        Assert.That(result.Difficulty, Is.EqualTo(exercise.Difficulty));

        Assert.That(result.Skills, Is.Not.Null);
        Assert.That(result.Skills.Count, Is.EqualTo(2));
        Assert.That(result.Skills.Any(s => s.Name == "Planche"), Is.True);
        Assert.That(result.Skills.Any(s => s.Name == "Front Lever"), Is.True);

        this._repositoryMock.Verify(r => r.GetExerciseWithSkillsAsync(exerciseId), Times.Once);
    }

    [Test]
    public async Task GetExerciseDetailsAsync_ExerciseHasNoSkills_DoesReturnViewModelWithEmptySkillsCollection()
    {
        Guid exerciseId = Guid.NewGuid();

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Push Ups",
            Description = "Exercise description",
            ImageUrl = "image-url",
            MeasurementType = Measurement.Repetitions,
            Category = Category.Strength,
            ExerciseType = SkillType.Push,
            Difficulty = Difficulty.Beginner,
            Skills = new List<Skill>(),
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseWithSkillsAsync(exerciseId))
            .ReturnsAsync(exercise);

        DetailsExerciseViewModel result = await this._service.GetExerciseDetailsAsync(exerciseId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(exercise.Name));
        Assert.That(result.Description, Is.EqualTo(exercise.Description));
        Assert.That(result.ImageUrl, Is.EqualTo(exercise.ImageUrl));
        Assert.That(result.Measurement, Is.EqualTo(exercise.MeasurementType));
        Assert.That(result.Category, Is.EqualTo(exercise.Category));
        Assert.That(result.ExerciseType, Is.EqualTo(exercise.ExerciseType));
        Assert.That(result.Difficulty, Is.EqualTo(exercise.Difficulty));

        Assert.That(result.Skills, Is.Not.Null);
        Assert.That(result.Skills, Is.Empty);

        this._repositoryMock.Verify(r => r.GetExerciseWithSkillsAsync(exerciseId), Times.Once);
    }

    [Test]
    public void ApplyFiltering_FilterIsProvided_DoesReturnFilteredQuery()
    {
        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Name = "Push Ups" },
            new Exercise { Name = "Pull Ups" },
            new Exercise { Name = "Squats" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService
            .ApplyFiltering(query, "Push", null);

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(1));
        Assert.That(resultList[0].Name, Is.EqualTo("Push Ups"));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void ApplyFiltering_FilterIsNullOrWhiteSpace_DoesReturnUnmodifiedQuery(string? filter)
    {
        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Name = "Push Ups" },
            new Exercise { Name = "Pull Ups" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService
            .ApplyFiltering(query, filter, null);

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
    }

    [Test]
    public void ApplyOrdering_IsPreviousPageFalse_DoesReturnAscendingOrderedQuery()
    {
        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.NewGuid(), Name = "B" },
            new Exercise { Id = Guid.NewGuid(), Name = "A" },
            new Exercise { Id = Guid.NewGuid(), Name = "C" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService
            .ApplyOrdering(query, false, "name-asc");

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList[0].Name, Is.EqualTo("A"));
        Assert.That(resultList[1].Name, Is.EqualTo("B"));
        Assert.That(resultList[2].Name, Is.EqualTo("C"));
    }

    [Test]
    public void ApplyOrdering_IsPreviousPageTrue_DoesReturnDescendingOrderedQuery()
    {
        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.NewGuid(), Name = "B" },
            new Exercise { Id = Guid.NewGuid(), Name = "A" },
            new Exercise { Id = Guid.NewGuid(), Name = "C" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService
            .ApplyOrdering(query, true, "name-desc");

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList[0].Name, Is.EqualTo("A"));
        Assert.That(resultList[1].Name, Is.EqualTo("B"));
        Assert.That(resultList[2].Name, Is.EqualTo("C"));
    }

    [Test]
    public void ApplyOrdering_SameNameAscending_DoesOrderById()
    {
        Guid firstId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid secondId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = secondId, Name = "Same" },
            new Exercise { Id = firstId, Name = "Same" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService
            .ApplyOrdering(query, false, "name-asc");

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList[0].Id, Is.EqualTo(firstId));
        Assert.That(resultList[1].Id, Is.EqualTo(secondId));
    }

    [Test]
    public void ApplyOrdering_SameNameDescending_DoesOrderByIdDescending()
    {
        Guid firstId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid secondId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = firstId, Name = "Same" },
            new Exercise { Id = secondId, Name = "Same" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService
            .ApplyOrdering(query, true, "name-desc");

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList[0].Id, Is.EqualTo(firstId));
        Assert.That(resultList[1].Id, Is.EqualTo(secondId));
    }

    [Test]
    public void ApplyPagination_IndexNameIsNullOrIndexIdIsNull_DoesReturnUnmodifiedQuery()
    {
        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.Parse("00000001-0000-0000-0000-000000000000"), Name = "A" },
            new Exercise { Id = Guid.Parse("00000002-0000-0000-0000-000000000000"), Name = "B" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> resultWithNullName = ExerciseOutputService.ApplyPagination(
            query,
            null,
            Guid.Parse("00000001-0000-0000-0000-000000000000"),
            false,
            "name-asc");

        IQueryable<Exercise> resultWithNullId = ExerciseOutputService.ApplyPagination(
            query,
            "A",
            null,
            false,
            "name-asc");

        Assert.That(resultWithNullName.ToList().Count, Is.EqualTo(2));
        Assert.That(resultWithNullId.ToList().Count, Is.EqualTo(2));
    }

    [Test]
    public void ApplyPagination_NextPageIsRequested_DoesReturnItemsAfterCursor()
    {
        Guid lowerSameNameId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherSameNameId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.Parse("00000010-0000-0000-0000-000000000000"), Name = "A" },
            new Exercise { Id = lowerSameNameId, Name = "B" },
            new Exercise { Id = higherSameNameId, Name = "B" },
            new Exercise { Id = Guid.Parse("00000020-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService.ApplyPagination(
            query,
            "B",
            lowerSameNameId,
            false,
            "name-asc");

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.Any(e => e.Name == "B" && e.Id == higherSameNameId), Is.True);
        Assert.That(resultList.Any(e => e.Name == "C"), Is.True);
    }

    [Test]
    public void ApplyPagination_PreviousPageIsRequested_DoesReturnItemsBeforeCursor()
    {
        Guid lowerSameNameId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherSameNameId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.Parse("00000010-0000-0000-0000-000000000000"), Name = "A" },
            new Exercise { Id = lowerSameNameId, Name = "B" },
            new Exercise { Id = higherSameNameId, Name = "B" },
            new Exercise { Id = Guid.Parse("00000020-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<Exercise> result = ExerciseOutputService.ApplyPagination(
            query,
            "B",
            higherSameNameId,
            true,
            "name-asc");

        List<Exercise> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.Any(e => e.Name == "A"), Is.True);
        Assert.That(resultList.Any(e => e.Name == "B" && e.Id == lowerSameNameId), Is.True);
    }

    [Test]
    public void ProjectExercises_ExercisesAreProvided_DoesReturnMappedViewModels()
    {
        List<Exercise> exercises = new List<Exercise>
        {
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Push Ups",
                Description = "Desc 1",
                Difficulty = Difficulty.Beginner,
                ImageUrl = "img1"
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Pull Ups",
                Description = "Desc 2",
                Difficulty = Difficulty.Intermediate,
                ImageUrl = "img2"
            }
        };

        IQueryable<Exercise> query = exercises.AsQueryable();

        IQueryable<ListTableItemViewModel> result = ExerciseOutputService.ProjectExercises(query);

        List<ListTableItemViewModel> resultList = result.ToList();

        Assert.That(resultList, Is.Not.Null);
        Assert.That(resultList.Count, Is.EqualTo(2));

        for (int i = 0; i < exercises.Count; i++)
        {
            Assert.That(resultList[i].Id, Is.EqualTo(exercises[i].Id));
            Assert.That(resultList[i].Name, Is.EqualTo(exercises[i].Name));
            Assert.That(resultList[i].Description, Is.EqualTo(exercises[i].Description));
            Assert.That(resultList[i].Difficulty, Is.EqualTo(exercises[i].Difficulty));
            Assert.That(resultList[i].ImageUrl, Is.EqualTo(exercises[i].ImageUrl));
        }
    }

    [Test]
    public void ProjectExercises_NoExercisesProvided_DoesReturnEmptyCollection()
    {
        IQueryable<Exercise> query = new List<Exercise>().AsQueryable();

        IQueryable<ListTableItemViewModel> result = ExerciseOutputService.ProjectExercises(query);

        List<ListTableItemViewModel> resultList = result.ToList();

        Assert.That(resultList, Is.Not.Null);
        Assert.That(resultList, Is.Empty);
    }

    [Test]
    public void CreatePaginationViewModel_NextPageHasMoreItems_DoesTrimItemsAndSetNextPageData()
    {
        int pageSize = 2;

        List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000001-0000-0000-0000-000000000000"),
                Name = "A",
            },
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000002-0000-0000-0000-000000000000"),
                Name = "B",
            },
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000003-0000-0000-0000-000000000000"),
                Name = "C",
            },
        };

        PaginationResultViewModel<ListTableItemViewModel> result = ExerciseOutputService.CreatePaginationViewModel(
            items,
            "filter",
            pageSize,
            null,
            null,
            false,
            "name-asc",
            null);

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.Items.ToArray()[0].Name, Is.EqualTo("A"));
        Assert.That(result.Items.ToArray()[1].Name, Is.EqualTo("B"));

        Assert.That(result.Filter, Is.EqualTo("filter"));
        Assert.That(result.PageSize, Is.EqualTo(pageSize));
        Assert.That(result.SortOrder, Is.EqualTo("name-asc"));
        Assert.That(result.DifficultyFilter, Is.Null);

        Assert.That(result.HasPreviousPage, Is.False);
        Assert.That(result.HasNextPage, Is.True);

        Assert.That(result.NextIndexName, Is.EqualTo("B"));
        Assert.That(result.NextIndexId, Is.EqualTo(Guid.Parse("00000002-0000-0000-0000-000000000000")));
        Assert.That(result.PreviousIndexName, Is.Null);
        Assert.That(result.PreviousIndexId, Is.Null);
    }

    [Test]
    public void CreatePaginationViewModel_NextPageHasIndexButNoMoreItems_DoesSetPreviousPageOnly()
    {
        int pageSize = 3;
        Guid indexId = Guid.Parse("00000009-0000-0000-0000-000000000000");

            List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000001-0000-0000-0000-000000000000"),
                Name = "A",
            },
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000002-0000-0000-0000-000000000000"),
                Name = "B",
            },
        };

        PaginationResultViewModel<ListTableItemViewModel> result = ExerciseOutputService.CreatePaginationViewModel(
            items,
            null,
            pageSize,
            "Cursor",
            indexId,
            false,
            "name-asc",
            null);

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.HasPreviousPage, Is.True);
        Assert.That(result.HasNextPage, Is.False);

        Assert.That(result.SortOrder, Is.EqualTo("name-asc"));
        Assert.That(result.DifficultyFilter, Is.Null);

        Assert.That(result.PreviousIndexName, Is.EqualTo("A"));
        Assert.That(result.PreviousIndexId, Is.EqualTo(Guid.Parse("00000001-0000-0000-0000-000000000000")));
        Assert.That(result.NextIndexName, Is.Null);
        Assert.That(result.NextIndexId, Is.Null);
    }

    [Test]
    public void CreatePaginationViewModel_PreviousPageHasMoreItemsAndIndex_DoesSetBothPreviousAndNextPageData()
    {
        int pageSize = 2;
        Guid indexId = Guid.Parse("00000009-0000-0000-0000-000000000000");

        List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000001-0000-0000-0000-000000000000"),
                Name = "A",
            },
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000002-0000-0000-0000-000000000000"),
                Name = "B",
            },
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000003-0000-0000-0000-000000000000"),
                Name = "C",
            },
        };

        PaginationResultViewModel<ListTableItemViewModel> result = ExerciseOutputService.CreatePaginationViewModel(
            items,
            null,
            pageSize,
            "Cursor",
            indexId,
            true,
            "name-asc",
            null);

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.Items.ToArray()[0].Name, Is.EqualTo("B"));
        Assert.That(result.Items.ToArray()[1].Name, Is.EqualTo("C"));

        Assert.That(result.SortOrder, Is.EqualTo("name-asc"));
        Assert.That(result.DifficultyFilter, Is.Null);

        Assert.That(result.HasPreviousPage, Is.True);
        Assert.That(result.HasNextPage, Is.True);

        Assert.That(result.PreviousIndexName, Is.EqualTo("B"));
        Assert.That(result.PreviousIndexId, Is.EqualTo(Guid.Parse("00000002-0000-0000-0000-000000000000")));
        Assert.That(result.NextIndexName, Is.EqualTo("C"));
        Assert.That(result.NextIndexId, Is.EqualTo(Guid.Parse("00000003-0000-0000-0000-000000000000")));
    }

    [Test]
    public void CreatePaginationViewModel_PreviousPageHasNoIndexAndNoMoreItems_DoesSetNoPaginationFlags()
    {
        int pageSize = 3;

        List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000001-0000-0000-0000-000000000000"),
                Name = "A",
            },
            new ListTableItemViewModel
            {
                Id = Guid.Parse("00000002-0000-0000-0000-000000000000"),
                Name = "B",
            },
        };

        PaginationResultViewModel<ListTableItemViewModel> result = ExerciseOutputService.CreatePaginationViewModel(
            items,
            null,
            pageSize,
            null,
            null,
            true,
            "name-asc",
            null);

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.SortOrder, Is.EqualTo("name-asc"));
        Assert.That(result.DifficultyFilter, Is.Null);
        Assert.That(result.HasPreviousPage, Is.False);
        Assert.That(result.HasNextPage, Is.False);
        Assert.That(result.PreviousIndexName, Is.Null);
        Assert.That(result.PreviousIndexId, Is.Null);
        Assert.That(result.NextIndexName, Is.Null);
        Assert.That(result.NextIndexId, Is.Null);
    }
}
