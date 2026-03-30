using CalisthenicsSkillTracker.Data.Models;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;

public interface IStatsRepository
{
    Task<IEnumerable<Workout>> GetAllWorkoutsAsync(Expression<Func<Workout, bool>>? filterQuery = null, Expression<Func<Workout, Workout>>? projectionQuery = null);

    Task<IEnumerable<SkillProgress>> GetAllSkillRecordsAsync(Expression<Func<SkillProgress, bool>>? filterQuery = null, Expression<Func<SkillProgress, SkillProgress>>? projectionQuery = null);

    Task<int> CountAsync<T>() where T : class;
}
