namespace CalisthenicsSkillTracker.ViewModels;

public class PaginationResultViewModel<T> where T : class
{
    public IEnumerable<T> Items { get; set; }
        = new List<T>();

    public string? Filter { get; set; }

    public int PageSize { get; set; }

    public bool HasNextPage { get; set; }

    public bool HasPreviousPage { get; set; }

    public string? PreviousIndexName { get; set; }

    public string? NextIndexName { get; set; }

    public string? SortOrder { get; set; }

    public string? DifficultyFilter { get; set; }

    public Guid? PreviousIndexId { get; set; }

    public Guid? NextIndexId { get; set; }
}
