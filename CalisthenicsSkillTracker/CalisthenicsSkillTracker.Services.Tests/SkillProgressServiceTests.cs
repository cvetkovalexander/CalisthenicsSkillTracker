using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;

using Moq;

namespace CalisthenicsSkillTracker.Services.Tests;

[TestFixture]
public class SkillProgressServiceTests
{
    private Mock<ISkillProgressRepository> skillProgressRepositoryMock;
    private ISkillProgressService skillProgressService;

    [SetUp]
    public void SetUp()
    {
        this.skillProgressRepositoryMock = new Mock<ISkillProgressRepository>();

        this.skillProgressService = new SkillProgressService(
            this.skillProgressRepositoryMock.Object);
    }

    [Test]
    public async Task CreateSkillProgressAsync_EntityIsSuccessfullyPersisted_DoesNotThrowException()
    {
        CreateSkillProgressViewModel inputModel = new CreateSkillProgressViewModel
        {
            SkillId = Guid.NewGuid(),
            UserId = "6640bea9-d34d-468b-8c31-b045be0203fb",
            Notes = "Good session"
        };

        this.skillProgressRepositoryMock
            .Setup(r => r.AddSkillProgressAsync(It.IsAny<SkillProgress>()))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this.skillProgressService.CreateSkillProgressAsync(inputModel));

        this.skillProgressRepositoryMock.Verify(
            r => r.AddSkillProgressAsync(It.Is<SkillProgress>(sp =>
                sp.SkillId == inputModel.SkillId &&
                sp.UserId == Guid.Parse(inputModel.UserId) &&
                sp.Notes == inputModel.Notes)),
            Times.Once);
    }

    [Test]
    public async Task CreateSkillProgressAsync_EntityFailPersist_DoesThrowException()
    {
        CreateSkillProgressViewModel inputModel = new CreateSkillProgressViewModel
        {
            SkillId = Guid.NewGuid(),
            UserId = "6640bea9-d34d-468b-8c31-b045be0203fb",
            Notes = "Test note"
        };

        this.skillProgressRepositoryMock
            .Setup(r => r.AddSkillProgressAsync(It.IsAny<SkillProgress>()))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityCreatePersistException>(async () =>
            await this.skillProgressService.CreateSkillProgressAsync(inputModel));

        this.skillProgressRepositoryMock.Verify(
            r => r.AddSkillProgressAsync(It.IsAny<SkillProgress>()),
            Times.Once);
    }

    [Test]
    public async Task DeleteSkillRecordAsync_EntityIsSuccessfullyDeleted_DoesNotThrowException()
    {
        Guid skillRecordId = Guid.Parse("2BBCF634-3A24-4356-911F-2606035A15DD");

        SkillProgress skillProgress = new SkillProgress
        {
            Id = skillRecordId
        };

        this.skillProgressRepositoryMock
            .Setup(r => r.GetSkillRecordAsync(skillRecordId))
            .ReturnsAsync(skillProgress);

        this.skillProgressRepositoryMock
            .Setup(r => r.HardDeleteSkillProgressAsync(skillProgress))
            .ReturnsAsync(true);

        Assert.DoesNotThrowAsync(async () =>
            await this.skillProgressService.DeleteSkillRecordAsync(skillRecordId));

        this.skillProgressRepositoryMock.Verify(
            r => r.GetSkillRecordAsync(skillRecordId),
            Times.Once);

        this.skillProgressRepositoryMock.Verify(
            r => r.HardDeleteSkillProgressAsync(skillProgress),
            Times.Once);
    }

    [Test]
    public async Task DeleteSkillRecordAsync_EntityFailDelete_DoesThrowException()
    {
        Guid skillRecordId = Guid.Parse("2BBCF634-3A24-4356-911F-2606035A15DD");

        SkillProgress skillProgress = new SkillProgress
        {
            Id = skillRecordId
        };

        this.skillProgressRepositoryMock
            .Setup(r => r.GetSkillRecordAsync(skillRecordId))
            .ReturnsAsync(skillProgress);

        this.skillProgressRepositoryMock
            .Setup(r => r.HardDeleteSkillProgressAsync(skillProgress))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<EntityDeleteException>(async () =>
            await this.skillProgressService.DeleteSkillRecordAsync(skillRecordId));

        this.skillProgressRepositoryMock.Verify(
            r => r.GetSkillRecordAsync(skillRecordId),
            Times.Once);

        this.skillProgressRepositoryMock.Verify(
            r => r.HardDeleteSkillProgressAsync(skillProgress),
            Times.Once);
    }

    [Test]
    public void CreateSkillProgressViewModel_ShouldReturnViewModelWithPopulatedSelectListsAndUserId_ReturnsValidViewModel()
    {
        string userId = "test-user-id";

        this.skillProgressRepositoryMock
            .Setup(r => r.GetAllSkills())
            .Returns(new List<Skill>
                {
                    new Skill { Id = Guid.NewGuid(), Name = "Skill 1" },
                    new Skill { Id = Guid.NewGuid(), Name = "Skill 2" }
                }
            .AsQueryable());

        CreateSkillProgressViewModel result = this.skillProgressService.CreateSkillProgressViewModel(userId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(userId));

        Assert.That(result.Skills, Is.Not.Null);
        Assert.That(result.Skills, Is.Not.Empty);

        Assert.That(result.Progressions, Is.Not.Null);
        Assert.That(result.Progressions, Is.Not.Empty);
    }
}
