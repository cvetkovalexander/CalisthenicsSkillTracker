using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;

using Moq;

namespace CalisthenicsSkillTracker.Services.Tests;

[TestFixture]
public class SkillInputServiceTests
{
    private Mock<ISkillRepository> _repositoryMock;
    private ISkillInputService _service;

    [SetUp]
    public void SetUp()
    {
        this._repositoryMock = new Mock<ISkillRepository>();

        this._service = new SkillInputService(
            this._repositoryMock.Object);
    }

    [Test]
    public async Task CreateSkillAsync_EntityIsSuccessfullyPersisted_DoesNotThrowException()
    {
        CreateSkillViewModel model = new CreateSkillViewModel
        {
            Name = "Muscle Up",
            Description = "A muscle up is a compound exercise that combines a pull-up with a dip, allowing you to transition from hanging below the bar to pushing yourself above it.",
            Measurement = Measurement.Repetitions,
            Category = Category.Endurance,
            SkillType = SkillType.Pull,
            Difficulty = Difficulty.Advanced
        };

        this._repositoryMock
            .Setup(r => r.AddSkillAsync(It.IsAny<Skill>()))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.CreateSkillAsync(model));

        this._repositoryMock.Verify(
            r => r.AddSkillAsync(It.Is<Skill>(s =>
                s.Name == model.Name &&
                s.Description == model.Description &&
                s.ImageUrl == model.ImageUrl &&
                s.MeasurementType == model.Measurement &&
                s.Category == model.Category &&
                s.SkillType == model.SkillType &&
                s.Difficulty == model.Difficulty)),
            Times.Once);
    }

    [Test]
    public async Task CreateSkillAsync_EntityFailPersist_DoesThrowException()
    {
        CreateSkillViewModel model = new CreateSkillViewModel
        {
            Name = "Muscle Up",
            Description = "A muscle up is a compound exercise that combines a pull-up with a dip, allowing you to transition from hanging below the bar to pushing yourself above it.",
            Measurement = Measurement.Repetitions,
            Category = Category.Endurance,
            SkillType = SkillType.Pull,
            Difficulty = Difficulty.Advanced
        };

        this._repositoryMock
            .Setup(r => r.AddSkillAsync(It.IsAny<Skill>()))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityCreatePersistException>(async () =>
            await this._service.CreateSkillAsync(model));

        this._repositoryMock.Verify(
            r => r.AddSkillAsync(It.IsAny<Skill>()),
            Times.Once);
    }

    [Test]
    public async Task EditSkillDataAsync_EntityIsSuccessfullyEdited_DoesNotThrowException()
    {
        EditSkillViewModel model = new EditSkillViewModel
        {
            Id = Guid.NewGuid(),
            Name = "Muscle Up",
            Description = "A muscle up is a compound exercise that combines a pull-up with a dip, allowing you to transition from hanging below the bar to pushing yourself above it.",
            Measurement = Measurement.Repetitions,
            Category = Category.Endurance,
            SkillType = SkillType.Pull,
            Difficulty = Difficulty.Advanced
        };

        this._repositoryMock
            .Setup(r => r.GetSkillByIdAsync(model.Id))
            .ReturnsAsync(new Skill
            {
                Id = model.Id,
                Name = "Old Name",
                Description = "Old Description",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Push,
                Difficulty = Difficulty.Intermediate
            });

        this._repositoryMock
            .Setup(r => r.EditSkillAsync(It.IsAny<Skill>()))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.EditSkillDataAsync(model));

        this._repositoryMock.Verify(
            r => r.EditSkillAsync(It.Is<Skill>(s =>
                s.Id == model.Id &&
                s.Name == model.Name &&
                s.Description == model.Description &&
                s.ImageUrl == model.ImageUrl &&
                s.MeasurementType == model.Measurement &&
                s.Category == model.Category &&
                s.SkillType == model.SkillType &&
                s.Difficulty == model.Difficulty)),
            Times.Once);
    }

    [Test]
    public async Task EditSkillDataAsync_EntityFailEdit_DoesThrowException() 
    {
        EditSkillViewModel model = new EditSkillViewModel
        {
            Id = Guid.NewGuid(),
            Name = "Muscle Up",
            Description = "A muscle up is a compound exercise that combines a pull-up with a dip, allowing you to transition from hanging below the bar to pushing yourself above it.",
            Measurement = Measurement.Repetitions,
            Category = Category.Endurance,
            SkillType = SkillType.Pull,
            Difficulty = Difficulty.Advanced
        };

        this._repositoryMock
            .Setup(r => r.GetSkillByIdAsync(model.Id))
            .ReturnsAsync(new Skill
            {
                Id = model.Id,
                Name = "Old Name",
                Description = "Old Description",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Push,
                Difficulty = Difficulty.Intermediate
            });

        this._repositoryMock
            .Setup(r => r.EditSkillAsync(It.IsAny<Skill>()))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityEditPersistException>(async () =>
            await this._service.EditSkillDataAsync(model));

        this._repositoryMock.Verify(
            r => r.EditSkillAsync(It.IsAny<Skill>()),
            Times.Once);
    }

    [Test]
    public async Task DeleteSkillAsync_EntityIsSuccessfullyDeleted_DoesNotThrowException()
    {
        Guid skillId = Guid.NewGuid();

        Skill skill = new Skill
        {
            Id = skillId,
            Name = "Planche",
            MeasurementType = Measurement.Duration,
            Category = Category.Balance,
            SkillType = SkillType.Core,
            Difficulty = Difficulty.Advanced
        };

        this._repositoryMock
            .Setup(r => r.GetSkillByIdAsync(skillId))
            .ReturnsAsync(skill);

        this._repositoryMock
            .Setup(r => r.HardDeleteSkillAsync(skill))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this._service.DeleteSkillAsync(skillId));

        this._repositoryMock.Verify(
            r => r.GetSkillByIdAsync(skillId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.HardDeleteSkillAsync(skill),
            Times.Once);
    }

    [Test]
    public async Task DeleteSkillAsync_EntityFailDelete_DoesThrowException()
    {
        Guid skillId = Guid.NewGuid();

        Skill skill = new Skill
        {
            Id = skillId,
            Name = "Planche",
            MeasurementType = Measurement.Duration,
            Category = Category.Balance,
            SkillType = SkillType.Core,
            Difficulty = Difficulty.Advanced
        };

        this._repositoryMock
            .Setup(r => r.GetSkillByIdAsync(skillId))
            .ReturnsAsync(skill);

        this._repositoryMock
            .Setup(r => r.HardDeleteSkillAsync(skill))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityDeleteException>(async () =>
            await this._service.DeleteSkillAsync(skillId));

        this._repositoryMock.Verify(
            r => r.GetSkillByIdAsync(skillId),
            Times.Once);

        this._repositoryMock.Verify(
            r => r.HardDeleteSkillAsync(skill),
            Times.Once);
    }

    [Test]
    public async Task SkillNameExistsAsync_NameExists_ReturnsTrue()
    {
        string skillName = "Handstand";

        this._repositoryMock
            .Setup(r => r.SkillNameExistsAsync(skillName))
            .ReturnsAsync(true);

        bool result = await this._service.SkillNameExistsAsync(skillName);

        Assert.IsTrue(result);

        this._repositoryMock.Verify(
            r => r.SkillNameExistsAsync(skillName),
            Times.Once);
    }

    [Test]
    public async Task SkillNameExistsAsync_NameDoesNotExist_ReturnsFalse()
    {
        string skillName = "Handstand";

        this._repositoryMock
            .Setup(r => r.SkillNameExistsAsync(skillName))
            .ReturnsAsync(false);

        bool result = await this._service.SkillNameExistsAsync(skillName);

        Assert.IsFalse(result);

        this._repositoryMock.Verify(
            r => r.SkillNameExistsAsync(skillName),
            Times.Once);
    }

    [Test]
    public async Task SkillNameExcludingCurrentExistsAsync_NameExists_ReturnsTrue()
    {
        Guid skillId = Guid.NewGuid();

        string skillName = "Handstand";
        this._repositoryMock
            .Setup(r => r.SkillNameExcludingCurrentExistsAsync(skillId, skillName))
            .ReturnsAsync(true);

        bool result = await this._service.SkillNameExcludingCurrentExistsAsync(skillId, skillName);

        Assert.IsTrue(result);

        this._repositoryMock.Verify(
            r => r.SkillNameExcludingCurrentExistsAsync(skillId, skillName),
            Times.Once);
    } 

    [Test]
    public async Task SkillNameExcludingCurrentExistsAsync_NameDoesNotExist_ReturnsFalse()
    {
        Guid skillId = Guid.NewGuid();

        string skillName = "Handstand";

        this._repositoryMock
            .Setup(r => r.SkillNameExcludingCurrentExistsAsync(skillId, skillName))
            .ReturnsAsync(false);

        bool result = await this._service.SkillNameExcludingCurrentExistsAsync(skillId, skillName);

        Assert.IsFalse(result);

        this._repositoryMock.Verify(
            r => r.SkillNameExcludingCurrentExistsAsync(skillId, skillName),
            Times.Once);
    }

    [Test]
    public async Task CreateEditSkillViewModelAsync_SkillExists_ReturnsEditSkillViewModel()
    {
        Guid skillId = Guid.NewGuid();

        Skill skill = new Skill
        {
            Id = skillId,
            Name = "L-Sit",
            Description = "An L-sit is a static hold exercise that targets the core muscles, particularly the abdominal muscles and hip flexors. It is performed by sitting on the ground with your legs extended in front of you and lifting your body off the ground using your arms while keeping your legs straight.",
            ImageUrl = "https://example.com/l-sit.jpg",
            MeasurementType = Measurement.Duration,
            Category = Category.Strength,
            SkillType = SkillType.Core,
            Difficulty = Difficulty.Intermediate
        };
        this._repositoryMock
            .Setup(r => r.GetSkillByIdAsync(skillId))
            .ReturnsAsync(skill);

        EditSkillViewModel result = await this._service.CreateEditSkillViewModelAsync(skillId);

        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(skill.Id));
        Assert.That(result.Name, Is.EqualTo(skill.Name));
        Assert.That(result.Description, Is.EqualTo(skill.Description));
        Assert.That(result.ImageUrl, Is.EqualTo(skill.ImageUrl));
        Assert.That(result.Measurement, Is.EqualTo(skill.MeasurementType));
        Assert.That(result.Category, Is.EqualTo(skill.Category));
        Assert.That(result.SkillType, Is.EqualTo(skill.SkillType));
        Assert.That(result.Difficulty, Is.EqualTo(skill.Difficulty));

        this._repositoryMock.Verify(
            r => r.GetSkillByIdAsync(skillId),
            Times.Once);
    }
}
