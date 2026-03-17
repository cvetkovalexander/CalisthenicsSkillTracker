namespace CalisthenicsSkillTracker.Data.Repositories;

public abstract class BaseRepository : IDisposable
{
    private bool isDisposed = false;
    private readonly ApplicationDbContext _context;

    protected BaseRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    protected ApplicationDbContext Context => this._context;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                this._context.Dispose();
            }
        }
        isDisposed = true;
    }
}
