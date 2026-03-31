namespace CalisthenicsSkillTracker.ViewModels;

public class PaginationResultViewModel<T> where T : class
{
    public IEnumerable<T> Items { get; set; }
        = new List<T>();

    public string? Filter { get; set; }

    public int PageSize { get; set; }

    public bool HasNextPage { get; set; }

    public string? NextIndexName { get; set; }

    public Guid? NextIndexId { get; set; }
}
