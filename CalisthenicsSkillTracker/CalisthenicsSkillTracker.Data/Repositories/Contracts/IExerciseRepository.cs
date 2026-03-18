using CalisthenicsSkillTracker.Data.Models;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface IExerciseRepository
{
    Task<bool> AddExerciseAsync(Exercise exercise);

    Task<bool> EditExerciseAsync(Exercise exercise, bool isAdded);

    // TODO: Consider adding isDeleted property to Exercise and implementing soft delete instead of hard delete
    Task<bool> SoftDeleteExerciseAsync(Exercise exercise);

    Task<bool> HardDeleteExerciseAsync(Exercise exercise);

    Task<bool> ExerciseNameExistsAsync(string name);

    string RemoveWhitespaces(string input);

    Task<Exercise> GetExerciseByIdAsync(Guid id);

    Task<bool> ExerciseNameExcludingCurrentExistsAsync(Guid id, string name);

    IQueryable<Skill> GetAllSkills();

    IQueryable<Exercise> GetAllExercises();

    Task<Skill> GetSkillAsync(Guid id);

    Task<bool> SkillExistsAsync(Guid id);

    Task<Exercise> GetExerciseWithSkillsAsync(Guid id);

    Task<bool> ExerciseExistsAsync(Guid id);
}
