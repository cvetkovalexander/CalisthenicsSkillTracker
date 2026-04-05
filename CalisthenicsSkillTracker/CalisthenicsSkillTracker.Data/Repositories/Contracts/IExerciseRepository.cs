using CalisthenicsSkillTracker.Data.Models;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface IExerciseRepository
{
    Task<bool> AddExerciseAsync(Exercise exercise, bool isAdded);

    Task<bool> EditExerciseAsync(Exercise exercise, bool isAdded);

    // TODO: Consider adding isDeleted property to Exercise and implementing soft delete instead of hard delete
    Task<bool> SoftDeleteExerciseAsync(Exercise exercise);

    Task<bool> HardDeleteExerciseAsync(Exercise exercise);

    Task<ApplicationUser> GetUserWithFavoriteExercisesAsync(Guid userId);

    Task<HashSet<Guid>> GetUserFavoriteExercises(Guid userId);

    Task<bool> RemoveExerciseFromFavorites(Exercise exercise, ApplicationUser user);

    Task<bool> AddExerciseToFavorites(Exercise exercise, ApplicationUser user);

    Task<bool> ExerciseNameExistsAsync(string name);

    string RemoveWhitespaces(string input);

    Task<Exercise> GetExerciseByIdAsync(Guid id);

    Task<Exercise> GetTrackedExerciseByIdAsync(Guid id);

    Task<bool> ExerciseNameExcludingCurrentExistsAsync(Guid id, string name);

    IQueryable<Skill> GetAllSkills();

    IQueryable<Exercise> GetAllExercises(Expression<Func<Exercise, bool>>? filterQuery = null, Expression<Func<Exercise, Exercise>>? projectionQuery = null);

    Task<Skill> GetSkillAsync(Guid id);

    Task<bool> SkillExistsAsync(Guid id);

    Task<Exercise> GetExerciseWithSkillsAsync(Guid id);

    Task<bool> ExerciseExistsAsync(Guid id);
}
