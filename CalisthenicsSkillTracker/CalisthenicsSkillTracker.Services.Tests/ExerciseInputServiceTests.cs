using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants.EnumKeys;

namespace CalisthenicsSkillTracker.Services.Tests;

[TestFixture]
public class ExerciseInputServiceTests
{
    private Mock<IExerciseRepository> _repositoryMock;
    private IExerciseInputService _service;

    [SetUp]
    public void SetUp()
    {
        this._repositoryMock = new Mock<IExerciseRepository>();

        this._service = new ExerciseInputService(
            this._repositoryMock.Object);
    }

    [Test]
    public async Task CreateExerciseAsync_EntityIsSuccessfullyPersistedWithoutSkill_DoesNotThrowExceptionWithOneEntity()
    {
        CreateExerciseViewModel model = new CreateExerciseViewModel
        {
            Name = "Push Up",
            Description = "Basic pushing exercise",
            SkillId = null
        };

        Exercise? createdExercise = null;

        this._repositoryMock
            .Setup(r => r.AddExerciseAsync(It.IsAny<Exercise>(), false))
            .Callback<Exercise, bool>((e, _) => createdExercise = e)
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.CreateExerciseAsync(model));

        Assert.That(createdExercise, Is.Not.Null);
        Assert.That(createdExercise!.Name, Is.EqualTo(model.Name));
        Assert.That(createdExercise.Description, Is.EqualTo(model.Description));
        Assert.That(createdExercise.ImageUrl, Is.EqualTo(model.ImageUrl));
        Assert.That(createdExercise.Skills, Is.Empty);

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(It.IsAny<Guid>()),
            Times.Never);

        this._repositoryMock.Verify(
            r => r.AddExerciseAsync(
                It.Is<Exercise>(e =>
                    e.Name == model.Name &&
                    e.Description == model.Description &&
                    e.ImageUrl == model.ImageUrl &&
                    !e.Skills.Any()),
                false),
            Times.Once);
    }

    [Test]
    public async Task CreateExerciseAsync_EntityIsSuccessfullyPersistedWithSkill_DoesNotThrowExceptionWithTwoEntities()
    {
        Guid skillId = Guid.NewGuid();

        CreateExerciseViewModel model = new CreateExerciseViewModel
        {
            Name = "Pull Up",
            Description = "Basic pulling exercise",
            ImageUrl = "https://example.com/pullup.jpg",
            SkillId = skillId.ToString(),
        };

        Skill skill = new Skill
        {
            Id = skillId,
            Name = "Front Lever",
            MeasurementType = Measurement.Duration,
            Category = Category.Strength,
            SkillType = SkillType.Core,
            Difficulty = Difficulty.Intermediate
        };

        Exercise? createdExercise = null;

        this._repositoryMock
            .Setup(r => r.GetSkillAsync(skillId))
            .ReturnsAsync(skill);

        this._repositoryMock
            .Setup(r => r.AddExerciseAsync(It.IsAny<Exercise>(), true))
            .Callback<Exercise, bool>((e, _) => createdExercise = e)
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.CreateExerciseAsync(model));

        Assert.That(createdExercise, Is.Not.Null);
        Assert.That(createdExercise!.Name, Is.EqualTo(model.Name));
        Assert.That(createdExercise.Description, Is.EqualTo(model.Description));
        Assert.That(createdExercise.ImageUrl, Is.EqualTo(model.ImageUrl));

        Assert.That(createdExercise.Skills.Any(s => s.Id == skillId), Is.True);
        //Assert.That(skill.Exercises.Any(e => e == createdExercise), Is.True);

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(skillId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.AddExerciseAsync(
                It.Is<Exercise>(e =>
                    e.Name == model.Name &&
                    e.Description == model.Description &&
                    e.ImageUrl == model.ImageUrl &&
                    e.Skills.Any(s => s.Id == skillId)),
                true),
            Times.Once);
    }

    [Test]
    public async Task CreateExerciseAsync_EntityFailPersistWithoutSkill_DoesThrowException()
    {
        CreateExerciseViewModel model = new CreateExerciseViewModel
        {
            Name = "Push Up",
            Description = "Basic pushing exercise",
            ImageUrl = "https://example.com/pushup.jpg",
            SkillId = null
        };

        this._repositoryMock
            .Setup(r => r.AddExerciseAsync(It.IsAny<Exercise>(), false))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityCreatePersistException>(async () =>
            await this._service.CreateExerciseAsync(model));

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(It.IsAny<Guid>()),
            Times.Never);

        this._repositoryMock.Verify(
            r => r.AddExerciseAsync(It.IsAny<Exercise>(), false),
            Times.Once);
    }

    [Test]
    public async Task CreateExerciseAsync_EntityFailPersistWithSkill_DoesThrowException()
    {
        Guid skillId = Guid.NewGuid();

        CreateExerciseViewModel model = new CreateExerciseViewModel
        {
            Name = "Pull Up",
            Description = "Basic pulling exercise",
            SkillId = skillId.ToString()
        };

        Skill skill = new Skill
        {
            Id = skillId,
            Name = "Front Lever",
            MeasurementType = Measurement.Duration,
            Category = Category.Strength,
            SkillType = SkillType.Core,
            Difficulty = Difficulty.Intermediate
        };

        this._repositoryMock
            .Setup(r => r.GetSkillAsync(skillId))
            .ReturnsAsync(skill);

        this._repositoryMock
            .Setup(r => r.AddExerciseAsync(It.IsAny<Exercise>(), true))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityCreatePersistException>(async () =>
            await this._service.CreateExerciseAsync(model));

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(skillId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.AddExerciseAsync(It.IsAny<Exercise>(), true),
            Times.Once);
    }

    [Test]
    public async Task EditExerciseDataAsync_EntityIsSuccessfullyEditedWithoutSkill_DoesNotThrowExceptionWithOneEntity()
    {
        Guid exerciseId = Guid.NewGuid();

        EditExerciseViewModel model = new EditExerciseViewModel
        {
            Id = exerciseId,
            Name = "Updated Push Up",
            Description = "Updated description",
            ImageUrl = "https://example.com/updated-pushup.jpg",
            SkillId = null
        };

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Old Name",
            Description = "Old description",
            ImageUrl = "https://example.com/old.jpg"
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseByIdAsync(exerciseId))
            .ReturnsAsync(exercise);

        this._repositoryMock
            .Setup(r => r.EditExerciseAsync(exercise, false))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.EditExerciseDataAsync(model));

        Assert.That(exercise.Name, Is.EqualTo(model.Name));
        Assert.That(exercise.Description, Is.EqualTo(model.Description));
        Assert.That(exercise.ImageUrl, Is.EqualTo(model.ImageUrl));

        this._repositoryMock.Verify(
            r => r.GetExerciseByIdAsync(exerciseId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(It.IsAny<Guid>()),
            Times.Never);

        this._repositoryMock.Verify(
            r => r.EditExerciseAsync(
                It.Is<Exercise>(e =>
                    e.Id == exerciseId &&
                    e.Name == model.Name &&
                    e.Description == model.Description &&
                    e.ImageUrl == model.ImageUrl),
                false),
            Times.Once);
    }

    [Test]
    public async Task EditExerciseDataAsync_EntityIsSuccessfullyEditedWithSkill_DoesNotThrowExceptionWithTwoEntities()
    {
        Guid exerciseId = Guid.NewGuid();
        Guid skillId = Guid.NewGuid();

        EditExerciseViewModel model = new EditExerciseViewModel
        {
            Id = exerciseId,
            Name = "Updated Pull Up",
            Description = "Updated description",
            ImageUrl = "https://example.com/updated-pullup.jpg",
            SkillId = skillId.ToString(),
        };

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Old Name",
            Description = "Old description",
            ImageUrl = "https://example.com/old.jpg"
        };

        Skill skill = new Skill
        {
            Id = skillId,
            Name = "Front Lever",
            MeasurementType = Measurement.Duration,
            Category = Category.Strength,
            SkillType = SkillType.Core,
            Difficulty = Difficulty.Intermediate
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseByIdAsync(exerciseId))
            .ReturnsAsync(exercise);

        this._repositoryMock
            .Setup(r => r.GetSkillAsync(skillId))
            .ReturnsAsync(skill);

        this._repositoryMock
            .Setup(r => r.EditExerciseAsync(exercise, true))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.EditExerciseDataAsync(model));

        Assert.That(exercise.Name, Is.EqualTo(model.Name));
        Assert.That(exercise.Description, Is.EqualTo(model.Description));
        Assert.That(exercise.ImageUrl, Is.EqualTo(model.ImageUrl));
        Assert.That(exercise.Skills.Any(s => s.Id == skillId), Is.True);
        Assert.That(skill.Exercises.Any(e => e.Id == exerciseId), Is.True);

        this._repositoryMock.Verify(
            r => r.GetExerciseByIdAsync(exerciseId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(skillId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.EditExerciseAsync(
                It.Is<Exercise>(e =>
                    e.Id == exerciseId &&
                    e.Name == model.Name &&
                    e.Description == model.Description &&
                    e.ImageUrl == model.ImageUrl &&
                    e.Skills.Any(s => s.Id == skillId)),
                true),
            Times.Once);
    }

    [Test]
    public async Task EditExerciseDataAsync_EntityFailEditWithoutSkill_DoesThrowException()
    {
        Guid exerciseId = Guid.NewGuid();

        EditExerciseViewModel model = new EditExerciseViewModel
        {
            Id = exerciseId,
            Name = "Updated Push Up",
            Description = "Updated description",
            ImageUrl = "https://example.com/updated-pushup.jpg",
            SkillId = null
        };

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Old Name",
            Description = "Old description",
            ImageUrl = "https://example.com/old.jpg"
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseByIdAsync(exerciseId))
            .ReturnsAsync(exercise);

        this._repositoryMock
            .Setup(r => r.EditExerciseAsync(exercise, false))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityEditPersistException>(async () =>
            await this._service.EditExerciseDataAsync(model));

        this._repositoryMock.Verify(
            r => r.GetExerciseByIdAsync(exerciseId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(It.IsAny<Guid>()),
            Times.Never);

        this._repositoryMock.Verify(
            r => r.EditExerciseAsync(exercise, false),
            Times.Once);
    }

    [Test]
    public async Task EditExerciseDataAsync_EntityFailEditWithSkill_DoesThrowException()
    {
        Guid exerciseId = Guid.NewGuid();
        Guid skillId = Guid.NewGuid();

        EditExerciseViewModel model = new EditExerciseViewModel
        {
            Id = exerciseId,
            Name = "Updated Pull Up",
            Description = "Updated description",
            ImageUrl = "https://example.com/updated-pullup.jpg",
            SkillId = skillId.ToString()
        };

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Old Name",
            Description = "Old description",
            ImageUrl = "https://example.com/old.jpg"
        };

        Skill skill = new Skill
        {
            Id = skillId,
            Name = "Front Lever",
            MeasurementType = Measurement.Duration,
            Category = Category.Coordination,
            SkillType = SkillType.Core,
            Difficulty = Difficulty.Intermediate
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseByIdAsync(exerciseId))
            .ReturnsAsync(exercise);

        this._repositoryMock
            .Setup(r => r.GetSkillAsync(skillId))
            .ReturnsAsync(skill);

        this._repositoryMock
            .Setup(r => r.EditExerciseAsync(exercise, true))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityEditPersistException>(async () =>
            await this._service.EditExerciseDataAsync(model));

        this._repositoryMock.Verify(
            r => r.GetExerciseByIdAsync(exerciseId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.GetSkillAsync(skillId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.EditExerciseAsync(exercise, true),
            Times.Once);
    }

    [Test]
    public async Task DeleteExerciseAsync_EntityIsSuccessfullyDeleted_DoesNotThrowException()
    {
        Guid exerciseId = Guid.NewGuid();

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Push Up",
            Description = "Basic pushing exercise",
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseByIdAsync(exerciseId))
            .ReturnsAsync(exercise);

        this._repositoryMock
            .Setup(r => r.HardDeleteExerciseAsync(exercise))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.DeleteExerciseAsync(exerciseId));

        this._repositoryMock.Verify(
            r => r.GetExerciseByIdAsync(exerciseId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.HardDeleteExerciseAsync(exercise),
            Times.Once);
    }

    [Test]
    public async Task DeleteExerciseAsync_EntityFailDelete_DoesThrowException()
    {
        Guid exerciseId = Guid.NewGuid();

        Exercise exercise = new Exercise
        {
            Id = exerciseId,
            Name = "Push Up",
            Description = "Basic pushing exercise",
        };

        this._repositoryMock
            .Setup(r => r.GetExerciseByIdAsync(exerciseId))
            .ReturnsAsync(exercise);

        this._repositoryMock
            .Setup(r => r.HardDeleteExerciseAsync(exercise))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityDeleteException>(async () =>
            await this._service.DeleteExerciseAsync(exerciseId));

        this._repositoryMock.Verify(
            r => r.GetExerciseByIdAsync(exerciseId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.HardDeleteExerciseAsync(exercise),
            Times.Once);
    }

    [Test]
    public async Task ExerciseNameExistsAsync_NameExists_ReturnsTrue()
    {
        string exerciseName = "Push up";

        this._repositoryMock
            .Setup(r => r.ExerciseNameExistsAsync(exerciseName))
            .ReturnsAsync(true);

        bool result = await this._service.ExerciseNameExistsAsync(exerciseName);

        Assert.IsTrue(result);

        this._repositoryMock.Verify(
            r => r.ExerciseNameExistsAsync(exerciseName),
            Times.Once);
    }

    [Test]
    public async Task ExerciseNameExistsAsync_NameDoesNotExist_ReturnsFalse()
    {
        string exerciseName = "Push up";

        this._repositoryMock
            .Setup(r => r.ExerciseNameExistsAsync(exerciseName))
            .ReturnsAsync(false);

        bool result = await this._service.ExerciseNameExistsAsync(exerciseName);

        Assert.IsFalse(result);

        this._repositoryMock.Verify(
            r => r.ExerciseNameExistsAsync(exerciseName),
            Times.Once);
    }

    [Test]
    public async Task ExerciseNameExcludingCurrentExistsAsync_NameExists_ReturnsTrue()
    {
        Guid exerciseId = Guid.NewGuid();

        string exerciseName = "Push up";
        this._repositoryMock
            .Setup(r => r.ExerciseNameExcludingCurrentExistsAsync(exerciseId, exerciseName))
            .ReturnsAsync(true);

        bool result = await this._service.ExerciseNameExcludingCurrentExistsAsync(exerciseId, exerciseName);

        Assert.IsTrue(result);

        this._repositoryMock.Verify(
            r => r.ExerciseNameExcludingCurrentExistsAsync(exerciseId, exerciseName),
            Times.Once);
    }

    [Test]
    public async Task ExerciseNameExcludingCurrentExistsAsync_NameDoesNotExist_ReturnsFalse()
    {
        Guid exerciseId = Guid.NewGuid();

        string exerciseName = "Push up";

        this._repositoryMock
            .Setup(r => r.ExerciseNameExcludingCurrentExistsAsync(exerciseId, exerciseName))
            .ReturnsAsync(false);

        bool result = await this._service.ExerciseNameExcludingCurrentExistsAsync(exerciseId, exerciseName);

        Assert.IsFalse(result);

        this._repositoryMock.Verify(
            r => r.ExerciseNameExcludingCurrentExistsAsync(exerciseId, exerciseName),
            Times.Once);
    }

    [TestCaseSource(nameof(FetchSelectedEnumTestCases))]
    public void FetchSelectedEnum_ValidKeyIsProvided_DoesReturnExpectedSelectListItems(
    string key,
    List<SelectListItem> expected)
    {
        List<SelectListItem> result = this._service.FetchSelectedEnum(key);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count));

        for (int i = 0; i < expected.Count; i++)
        {
            Assert.That(result[i].Value, Is.EqualTo(expected[i].Value));
            Assert.That(result[i].Text, Is.EqualTo(expected[i].Text));
        }
    }

    [Test]
    public void FetchEnums_ModelIsProvided_DoesPopulateAllEnumOptionCollections()
    {
        IExerciseViewModel model = new CreateExerciseViewModel();

        this._service.FetchEnums(model);

        List<SelectListItem> expectedMeasurementOptions = Enum
            .GetValues(typeof(Measurement))
            .Cast<Measurement>()
            .Select(m => new SelectListItem
            {
                Value = ((int)m).ToString(),
                Text = m.ToString(),
            })
            .ToList();

        List<SelectListItem> expectedCategoryOptions = Enum
            .GetValues(typeof(Category))
            .Cast<Category>()
            .Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = c.ToString(),
            })
            .ToList();

        List<SelectListItem> expectedExerciseTypeOptions = Enum
            .GetValues(typeof(SkillType))
            .Cast<SkillType>()
            .Select(sk => new SelectListItem
            {
                Value = ((int)sk).ToString(),
                Text = sk.ToString(),
            })
            .ToList();

        List<SelectListItem> expectedDifficultyOptions = Enum
            .GetValues(typeof(Difficulty))
            .Cast<Difficulty>()
            .Select(d => new SelectListItem
            {
                Value = ((int)d).ToString(),
                Text = d.ToString(),
            })
            .ToList();

        Assert.That(model.MeasurementOptions, Is.Not.Null);
        Assert.That(model.MeasurementOptions.Count, Is.EqualTo(expectedMeasurementOptions.Count));
        for (int i = 0; i < expectedMeasurementOptions.Count; i++)
        {
            Assert.That(model.MeasurementOptions[i].Value, Is.EqualTo(expectedMeasurementOptions[i].Value));
            Assert.That(model.MeasurementOptions[i].Text, Is.EqualTo(expectedMeasurementOptions[i].Text));
        }

        Assert.That(model.CategoryOptions, Is.Not.Null);
        Assert.That(model.CategoryOptions.Count, Is.EqualTo(expectedCategoryOptions.Count));
        for (int i = 0; i < expectedCategoryOptions.Count; i++)
        {
            Assert.That(model.CategoryOptions[i].Value, Is.EqualTo(expectedCategoryOptions[i].Value));
            Assert.That(model.CategoryOptions[i].Text, Is.EqualTo(expectedCategoryOptions[i].Text));
        }

        Assert.That(model.ExerciseTypeOptions, Is.Not.Null);
        Assert.That(model.ExerciseTypeOptions.Count, Is.EqualTo(expectedExerciseTypeOptions.Count));
        for (int i = 0; i < expectedExerciseTypeOptions.Count; i++)
        {
            Assert.That(model.ExerciseTypeOptions[i].Value, Is.EqualTo(expectedExerciseTypeOptions[i].Value));
            Assert.That(model.ExerciseTypeOptions[i].Text, Is.EqualTo(expectedExerciseTypeOptions[i].Text));
        }

        Assert.That(model.DifficultyOptions, Is.Not.Null);
        Assert.That(model.DifficultyOptions.Count, Is.EqualTo(expectedDifficultyOptions.Count));
        for (int i = 0; i < expectedDifficultyOptions.Count; i++)
        {
            Assert.That(model.DifficultyOptions[i].Value, Is.EqualTo(expectedDifficultyOptions[i].Value));
            Assert.That(model.DifficultyOptions[i].Text, Is.EqualTo(expectedDifficultyOptions[i].Text));
        }
    }

    /* Helper methods */

    private static IEnumerable<TestCaseData> FetchSelectedEnumTestCases()
    {
        yield return new TestCaseData(
            MeasurementKey,
            Enum.GetValues(typeof(Measurement))
                .Cast<Measurement>()
                .Select(m => new SelectListItem
                {
                    Value = ((int)m).ToString(),
                    Text = m.ToString()
                })
                .ToList());

        yield return new TestCaseData(
            CategoryKey,
            Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Select(c => new SelectListItem
                {
                    Value = ((int)c).ToString(),
                    Text = c.ToString()
                })
                .ToList());

        yield return new TestCaseData(
            SkillTypeKey,
            Enum.GetValues(typeof(SkillType))
                .Cast<SkillType>()
                .Select(sk => new SelectListItem
                {
                    Value = ((int)sk).ToString(),
                    Text = sk.ToString()
                })
                .ToList());

        yield return new TestCaseData(
            DifficultyKey,
            Enum.GetValues(typeof(Difficulty))
                .Cast<Difficulty>()
                .Select(d => new SelectListItem
                {
                    Value = ((int)d).ToString(),
                    Text = d.ToString()
                })
                .ToList());
    }
}
