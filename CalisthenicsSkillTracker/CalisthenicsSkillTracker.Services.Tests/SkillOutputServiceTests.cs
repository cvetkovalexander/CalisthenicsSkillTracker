using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using CalisthenicsSkillTracker.ViewModels;
using Moq;

namespace CalisthenicsSkillTracker.Services.Tests;

[TestFixture]
public class SkillOutputServiceTests
{
    private Mock<ISkillRepository> _repositoryMock;
    private ISkillOutputService _service;

    [SetUp]
    public void SetUp()
    {
        this._repositoryMock = new Mock<ISkillRepository>();

        this._service = new SkillOutputService(
            this._repositoryMock.Object);
    }

    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("   ", null)]
    [TestCase(null, "")]
    [TestCase(null, "   ")]
    public void ApplyFiltering_NoValidFiltersAreProvided_DoesReturnUnmodifiedQuery(string? filter, string? difficultyFilter)
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Planche",
                Difficulty = Difficulty.Advanced
            },
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Front Lever",
                Difficulty = Difficulty.Intermediate
            }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyFiltering(query, filter, difficultyFilter);

        List<Skill> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.Select(s => s.Name), Is.EquivalentTo(new[] { "Planche", "Front Lever" }));
    }

    [Test]
    public void ApplyFiltering_ValidDifficultyFilterIsProvided_DoesReturnSkillsWithMatchingDifficulty()
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Planche",
                Difficulty = Difficulty.Advanced
            },
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Front Lever",
                Difficulty = Difficulty.Intermediate
            },
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Handstand",
                Difficulty = Difficulty.Advanced
            }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyFiltering(query, null, Difficulty.Advanced.ToString());

        List<Skill> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.All(s => s.Difficulty == Difficulty.Advanced), Is.True);
    }

    [Test]
    public void ApplyFiltering_InvalidDifficultyFilterIsProvided_DoesIgnoreDifficultyFilter()
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Planche",
                Difficulty = Difficulty.Advanced
            },
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Front Lever",
                Difficulty = Difficulty.Intermediate
            }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyFiltering(query, null, "InvalidDifficulty");

        List<Skill> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
    }

    [Test]
    public void ApplyOrdering_SortOrderIsAscendingAndIsPreviousPageIsFalse_DoesReturnAscendingOrderedQuery()
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000003-0000-0000-0000-000000000000"), Name = "C" },
            new Skill { Id = Guid.Parse("00000002-0000-0000-0000-000000000000"), Name = "B" },
            new Skill { Id = Guid.Parse("00000001-0000-0000-0000-000000000000"), Name = "A" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyOrdering(query, false, null);

        List<Skill> resultList = result.ToList();

        Assert.That(resultList[0].Name, Is.EqualTo("A"));
        Assert.That(resultList[1].Name, Is.EqualTo("B"));
        Assert.That(resultList[2].Name, Is.EqualTo("C"));
    }

    [Test]
    public void ApplyOrdering_SortOrderIsDescendingAndIsPreviousPageIsFalse_DoesReturnDescendingOrderedQuery()
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000001-0000-0000-0000-000000000000"), Name = "A" },
            new Skill { Id = Guid.Parse("00000002-0000-0000-0000-000000000000"), Name = "B" },
            new Skill { Id = Guid.Parse("00000003-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyOrdering(query, false, "name-desc");

        List<Skill> resultList = result.ToList();

        Assert.That(resultList[0].Name, Is.EqualTo("C"));
        Assert.That(resultList[1].Name, Is.EqualTo("B"));
        Assert.That(resultList[2].Name, Is.EqualTo("A"));
    }

    [Test]
    public void ApplyOrdering_SortOrderIsAscendingAndIsPreviousPageIsTrue_DoesReturnDescendingOrderedQuery()
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000001-0000-0000-0000-000000000000"), Name = "A" },
            new Skill { Id = Guid.Parse("00000002-0000-0000-0000-000000000000"), Name = "B" },
            new Skill { Id = Guid.Parse("00000003-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyOrdering(query, true, null);

        List<Skill> resultList = result.ToList();

        Assert.That(resultList[0].Name, Is.EqualTo("C"));
        Assert.That(resultList[1].Name, Is.EqualTo("B"));
        Assert.That(resultList[2].Name, Is.EqualTo("A"));
    }

    [Test]
    public void ApplyOrdering_SortOrderIsDescendingAndIsPreviousPageIsTrue_DoesReturnAscendingOrderedQuery()
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000003-0000-0000-0000-000000000000"), Name = "C" },
            new Skill { Id = Guid.Parse("00000002-0000-0000-0000-000000000000"), Name = "B" },
            new Skill { Id = Guid.Parse("00000001-0000-0000-0000-000000000000"), Name = "A" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyOrdering(query, true, "name-desc");

        List<Skill> resultList = result.ToList();

        Assert.That(resultList[0].Name, Is.EqualTo("A"));
        Assert.That(resultList[1].Name, Is.EqualTo("B"));
        Assert.That(resultList[2].Name, Is.EqualTo("C"));
    }

    [Test]
    public void ApplyOrdering_SameNameAscending_DoesOrderById()
    {
        Guid lowerId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = higherId, Name = "Same" },
            new Skill { Id = lowerId, Name = "Same" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyOrdering(query, false, null);

        List<Skill> resultList = result.ToList();

        Assert.That(resultList[0].Id, Is.EqualTo(lowerId));
        Assert.That(resultList[1].Id, Is.EqualTo(higherId));
    }

    [Test]
    public void ApplyOrdering_SameNameDescending_DoesOrderByIdDescending()
    {
        Guid lowerId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = lowerId, Name = "Same" },
            new Skill { Id = higherId, Name = "Same" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyOrdering(query, false, "name-desc");

        List<Skill> resultList = result.ToList();

        Assert.That(resultList[0].Id, Is.EqualTo(higherId));
        Assert.That(resultList[1].Id, Is.EqualTo(lowerId));
    }

    [Test]
    public void ApplyPagination_IndexNameIsNullOrIndexIdIsNull_DoesReturnUnmodifiedQuery()
    {
        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000001-0000-0000-0000-000000000000"), Name = "A" },
            new Skill { Id = Guid.Parse("00000002-0000-0000-0000-000000000000"), Name = "B" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> resultWithNullName = SkillOutputService.ApplyPagination(
            query,
            null,
            Guid.Parse("00000001-0000-0000-0000-000000000000"),
            false,
            null);

        IQueryable<Skill> resultWithNullId = SkillOutputService.ApplyPagination(
            query,
            "A",
            null,
            false,
            null);

        Assert.That(resultWithNullName.ToList().Count, Is.EqualTo(2));
        Assert.That(resultWithNullId.ToList().Count, Is.EqualTo(2));
    }

    [Test]
    public void ApplyPagination_AscendingSortAndNextPage_DoesReturnItemsAfterCursor()
    {
        Guid lowerSameNameId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherSameNameId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000010-0000-0000-0000-000000000000"), Name = "A" },
            new Skill { Id = lowerSameNameId, Name = "B" },
            new Skill { Id = higherSameNameId, Name = "B" },
            new Skill { Id = Guid.Parse("00000020-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyPagination(
            query,
            "B",
            lowerSameNameId,
            false,
            null);

        List<Skill> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.Any(s => s.Name == "B" && s.Id == higherSameNameId), Is.True);
        Assert.That(resultList.Any(s => s.Name == "C"), Is.True);
    }

    [Test]
    public void ApplyPagination_AscendingSortAndPreviousPage_DoesReturnItemsBeforeCursor()
    {
        Guid lowerSameNameId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherSameNameId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000010-0000-0000-0000-000000000000"), Name = "A" },
            new Skill { Id = lowerSameNameId, Name = "B" },
            new Skill { Id = higherSameNameId, Name = "B" },
            new Skill { Id = Guid.Parse("00000020-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyPagination(
            query,
            "B",
            higherSameNameId,
            true,
            null);

        List<Skill> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.Any(s => s.Name == "A"), Is.True);
        Assert.That(resultList.Any(s => s.Name == "B" && s.Id == lowerSameNameId), Is.True);
    }

    [Test]
    public void ApplyPagination_DescendingSortAndNextPage_DoesReturnItemsBeforeCursor()
    {
        Guid lowerSameNameId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherSameNameId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000010-0000-0000-0000-000000000000"), Name = "A" },
            new Skill { Id = lowerSameNameId, Name = "B" },
            new Skill { Id = higherSameNameId, Name = "B" },
            new Skill { Id = Guid.Parse("00000020-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyPagination(
            query,
            "B",
            higherSameNameId,
            false,
            "name-desc");

        List<Skill> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.Any(s => s.Name == "A"), Is.True);
        Assert.That(resultList.Any(s => s.Name == "B" && s.Id == lowerSameNameId), Is.True);
    }

    [Test]
    public void ApplyPagination_DescendingSortAndPreviousPage_DoesReturnItemsAfterCursor()
    {
        Guid lowerSameNameId = Guid.Parse("00000001-0000-0000-0000-000000000000");
        Guid higherSameNameId = Guid.Parse("00000002-0000-0000-0000-000000000000");

        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("00000010-0000-0000-0000-000000000000"), Name = "A" },
            new Skill { Id = lowerSameNameId, Name = "B" },
            new Skill { Id = higherSameNameId, Name = "B" },
            new Skill { Id = Guid.Parse("00000020-0000-0000-0000-000000000000"), Name = "C" }
        };

        IQueryable<Skill> query = skills.AsQueryable();

        IQueryable<Skill> result = SkillOutputService.ApplyPagination(
            query,
            "B",
            lowerSameNameId,
            true,
            "name-desc");

        List<Skill> resultList = result.ToList();

        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.Any(s => s.Name == "B" && s.Id == higherSameNameId), Is.True);
        Assert.That(resultList.Any(s => s.Name == "C"), Is.True);
    }

    [Test]
    public void CreatePaginationViewModel_NextPageHasMoreItems_DoesTakeFirstPageSizeItems()
    {
        int pageSize = 2;

        List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel { Id = Guid.Parse("1".PadLeft(32, '0')), Name = "A" },
            new ListTableItemViewModel { Id = Guid.Parse("2".PadLeft(32, '0')), Name = "B" },
            new ListTableItemViewModel { Id = Guid.Parse("3".PadLeft(32, '0')), Name = "C" }
        };

        PaginationResultViewModel<ListTableItemViewModel> result = SkillOutputService.CreatePaginationViewModel(
            items,
            "filter",
            pageSize,
            null,
            null,
            false,
            "name-desc",
            "Advanced");

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.Items.ToArray()[0].Name, Is.EqualTo("A"));
        Assert.That(result.Items.ToArray()[1].Name, Is.EqualTo("B"));

        Assert.That(result.HasNextPage, Is.True);
        Assert.That(result.HasPreviousPage, Is.False);

        Assert.That(result.NextIndexName, Is.EqualTo("B"));
        Assert.That(result.PreviousIndexName, Is.Null);

        Assert.That(result.SortOrder, Is.EqualTo("name-desc"));
        Assert.That(result.DifficultyFilter, Is.EqualTo("Advanced"));
    }

    [Test]
    public void CreatePaginationViewModel_PreviousPageHasMoreItems_DoesSkipFirstItemAndTakePageSize()
    {
        int pageSize = 2;

        List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel { Id = Guid.Parse("1".PadLeft(32, '0')), Name = "A" },
            new ListTableItemViewModel { Id = Guid.Parse("2".PadLeft(32, '0')), Name = "B" },
            new ListTableItemViewModel { Id = Guid.Parse("3".PadLeft(32, '0')), Name = "C" }
        };

        PaginationResultViewModel<ListTableItemViewModel> result = SkillOutputService.CreatePaginationViewModel(
            items,
            null,
            pageSize,
            "Cursor",
            Guid.NewGuid(),
            true,
            null,
            null);

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.Items.ToArray()[0].Name, Is.EqualTo("B"));
        Assert.That(result.Items.ToArray()[1].Name, Is.EqualTo("C"));

        Assert.That(result.HasNextPage, Is.True);
        Assert.That(result.HasPreviousPage, Is.True);

        Assert.That(result.PreviousIndexName, Is.EqualTo("B"));
        Assert.That(result.NextIndexName, Is.EqualTo("C"));
    }

    [Test]
    public void CreatePaginationViewModel_NoMoreItemsButHasIndex_DoesSetOnlyPreviousPage()
    {
        int pageSize = 3;

        List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel { Id = Guid.Parse("1".PadLeft(32, '0')), Name = "A" },
            new ListTableItemViewModel { Id = Guid.Parse("2".PadLeft(32, '0')), Name = "B" }
        };

        PaginationResultViewModel<ListTableItemViewModel> result = SkillOutputService.CreatePaginationViewModel(
            items,
            null,
            pageSize,
            "Cursor",
            Guid.NewGuid(),
            false,
            null,
            null);

        Assert.That(result.HasPreviousPage, Is.True);
        Assert.That(result.HasNextPage, Is.False);

        Assert.That(result.PreviousIndexName, Is.EqualTo("A"));
        Assert.That(result.NextIndexName, Is.Null);
    }

    [Test]
    public void CreatePaginationViewModel_NoIndexAndNoMoreItems_DoesSetNoPaginationFlags()
    {
        int pageSize = 3;

        List<ListTableItemViewModel> items = new List<ListTableItemViewModel>
        {
            new ListTableItemViewModel { Id = Guid.Parse("1".PadLeft(32, '0')), Name = "A" },
            new ListTableItemViewModel { Id = Guid.Parse("2".PadLeft(32, '0')), Name = "B" }
        };

        PaginationResultViewModel<ListTableItemViewModel> result = SkillOutputService.CreatePaginationViewModel(
            items,
            null,
            pageSize,
            null,
            null,
            true,
            null,
            null);

        Assert.That(result.HasPreviousPage, Is.False);
        Assert.That(result.HasNextPage, Is.False);

        Assert.That(result.PreviousIndexName, Is.Null);
        Assert.That(result.NextIndexName, Is.Null);
    }
}
