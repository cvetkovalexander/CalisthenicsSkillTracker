using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class ExerciseInputService : IExerciseInputService
{
    private const string MeasurementKey = "Measurement";

    private const string CategoryKey = "Category";

    private const string SkillTypeKey = "SkillType";

    private const string DifficultyKey = "Difficulty";

    private readonly ApplicationDbContext _context;

    public ExerciseInputService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<CreateExerciseViewModel> CreateExerciseViewModelWithEnumsAsync()
    {
        CreateExerciseViewModel model = new CreateExerciseViewModel();

        model.AvailableExercises = await this.GetAvailableSkillsAsync();
        this.FetchEnums(model);

        return model;
    }

    public List<SelectListItem> FetchSelectedEnum(string key)
    {
        Dictionary<string, List<SelectListItem>> enums = new Dictionary<string, List<SelectListItem>>
        {
            [MeasurementKey] = Enum
                .GetValues(typeof(Measurement))
                .Cast<Measurement>()
                .Select(m => new SelectListItem
                {
                    Value = ((int)m).ToString(),
                    Text = m.ToString()
                })
                .ToList(),

            [CategoryKey] = Enum
                .GetValues(typeof(Category))
                .Cast<Category>()
                .Select(c => new SelectListItem
                {
                    Value = ((int)c).ToString(),
                    Text = c.ToString()
                })
                .ToList(),

            [SkillTypeKey] = Enum
                .GetValues(typeof(SkillType))
                .Cast<SkillType>()
                .Select(sk => new SelectListItem
                {
                    Value = ((int)sk).ToString(),
                    Text = sk.ToString()
                })
                .ToList(),

            [DifficultyKey] = Enum
                .GetValues(typeof(Difficulty))
                .Cast<Difficulty>()
                .Select(d => new SelectListItem
                {
                    Value = ((int)d).ToString(),
                    Text = d.ToString()
                })
                .ToList()
        };

        return enums[key];
    }

    public void FetchEnums(IExerciseViewModel model)
    {
        model.MeasurementOptions = this.FetchSelectedEnum(MeasurementKey);
        model.CategoryOptions = this.FetchSelectedEnum(CategoryKey);
        model.ExerciseTypeOptions = this.FetchSelectedEnum(SkillTypeKey);
        model.DifficultyOptions = this.FetchSelectedEnum(DifficultyKey);
    }

    public async Task<bool> ExerciseNameExistsAsync(string name)
        => await this._context.Exercises.AnyAsync(s => s.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());

    public string RemoveWhitespaces(string input)
        => string.Concat(input.Where(c => !char.IsWhiteSpace(c)));

    public async Task CreateExerciseAsync(CreateExerciseViewModel model)
    {
        Exercise exercise = new Exercise()
        {
            Name = model.Name,
            Description = model.Description,
            MeasurementType = model.Measurement,
            Category = model.Category,
            ExerciseType = model.ExerciseType,
            Difficulty = model.Difficulty,
        };

        Skill skill = await this.GetSkillAsync(Guid.Parse(model.SkillId!));
        exercise.Skills.Add(skill);
        skill.Exercises.Add(exercise);

        await this._context.Exercises.AddAsync(exercise);
        await this._context.SaveChangesAsync();
    }

    public async Task<EditExerciseViewModel> CreateEditExerciseViewModelAsync(Guid id)
    {
        Exercise exerciseEntity = await this.GetExerciseByIdAsync(id);

        EditExerciseViewModel model = new EditExerciseViewModel()
        {
            Id = exerciseEntity.Id,
            Name = exerciseEntity.Name,
            Description = exerciseEntity.Description,
            Measurement = exerciseEntity.MeasurementType,
            Category = exerciseEntity.Category,
            ExerciseType = exerciseEntity.ExerciseType,
            Difficulty = exerciseEntity.Difficulty
        };

        this.FetchEnums(model);

        return model;
    }

    public async Task<Exercise> GetExerciseByIdAsync(Guid id)
    {
        return await this._context
            .Exercises
            .AsNoTracking()
            .SingleAsync(e => e.Id == id);
    }

    public async Task<bool> ExerciseNameExcludingCurrentExistsAsync(Guid id, string name)
    {
        return await this._context
            .Exercises
            .AsNoTracking()
            .AnyAsync(e => e.Id != id && e.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());
    }

    public async Task EditExerciseDataAsync(EditExerciseViewModel model)
    {
        Exercise exercise = await this.GetExerciseByIdAsync(model.Id);

        exercise.Name = model.Name;
        exercise.Description = model.Description;
        exercise.MeasurementType = model.Measurement;
        exercise.Category = model.Category;
        exercise.ExerciseType = model.ExerciseType;
        exercise.Difficulty = model.Difficulty;

        this._context.Exercises.Update(exercise);
        await this._context.SaveChangesAsync();
    }

    public async Task DeleteExerciseAsync(Guid id)
    {
        Exercise exerciseEntity = await this.GetExerciseByIdAsync(id);

        this._context.Exercises.Remove(exerciseEntity);

        await this._context.SaveChangesAsync();
    }

    public async Task<List<SelectListItem>> GetAvailableSkillsAsync()
    {
        return await this._context
            .Skills
            .AsNoTracking()
            .Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            })
            .ToListAsync();

    }

    public async Task<Skill> GetSkillAsync(Guid id)
    {
        return await this._context
            .Skills
            .FirstAsync(s => s.Id == id);
    }

    public async Task<bool> SkillExistsAsync(Guid id)
    {
        return await this._context
            .Skills
            .AnyAsync(s => s.Id == id);
    }
}
