using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants.EnumKeys;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class ExerciseInputService : IExerciseInputService
{
    private readonly IExerciseRepository _repository;

    public ExerciseInputService(IExerciseRepository repository)
    {
        this._repository = repository;
    }
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
            ImageUrl = model.ImageUrl
        };

        bool isAdded = false;
        if (model.SkillId is not null) 
        {
            Skill skill = await this._repository.GetSkillAsync(Guid.Parse(model.SkillId!));
            exercise.Skills.Add(skill);
            isAdded = true;
        }

        bool successfulAdd = await this._repository.AddExerciseAsync(exercise, isAdded);
        if (!successfulAdd)
            throw new EntityCreatePersistException();
    }

    public async Task EditExerciseDataAsync(EditExerciseViewModel model)
    {
        Exercise exercise = await this._repository.GetExerciseByIdAsync(model.Id);

        exercise.Name = model.Name;
        exercise.Description = model.Description;
        exercise.ImageUrl = model.ImageUrl;
        exercise.MeasurementType = model.Measurement;
        exercise.Category = model.Category;
        exercise.ExerciseType = model.ExerciseType;
        exercise.Difficulty = model.Difficulty;

        bool isAdded = false;
        if (model.SkillId is not null)
        {
            Skill skill = await this._repository.GetSkillAsync(Guid.Parse(model.SkillId!));
            exercise.Skills.Add(skill);
            skill.Exercises.Add(exercise);
            isAdded = true;
        }

        bool successfulEdit = await this._repository.EditExerciseAsync(exercise, isAdded);
        if (!successfulEdit)
            throw new EntityEditPersistException();
    }

    public async Task<CreateExerciseViewModel> CreateExerciseViewModelWithEnumsAsync()
    {
        CreateExerciseViewModel model = new CreateExerciseViewModel();

        model.AvailableExercises = await this.GetAvailableSkillsAsync();
        this.FetchEnums(model);

        return model;
    }

    public async Task<bool> ToggleFavoriteAsync(Guid exerciseId, Guid userId)
    {
        ApplicationUser user = await this._repository.GetUserWithFavoriteExercisesAsync(userId);
        Exercise exercise = await this._repository.GetTrackedExerciseByIdAsync(exerciseId);

        Exercise? existingFavorite = user.FavoriteExercises
            .FirstOrDefault(e => e.Id == exerciseId);

        if (existingFavorite is not null)
        {
            bool removed = await this._repository.RemoveExerciseFromFavorites(exercise, user);
            if (!removed)
                throw new EntityFavoritePersistException();

            return false;
        }

        bool added = await this._repository.AddExerciseToFavorites(exercise, user);
        if (!added)
            throw new EntityFavoritePersistException();

        return true;
    }

    public void FetchEnums(IExerciseViewModel model)
    {
        model.MeasurementOptions = this.FetchSelectedEnum(MeasurementKey);
        model.CategoryOptions = this.FetchSelectedEnum(CategoryKey);
        model.ExerciseTypeOptions = this.FetchSelectedEnum(SkillTypeKey);
        model.DifficultyOptions = this.FetchSelectedEnum(DifficultyKey);
    }

    public async Task<bool> ExerciseNameExistsAsync(string name)
        => await this._repository.ExerciseNameExistsAsync(name);

    public async Task<EditExerciseViewModel> CreateEditExerciseViewModelAsync(Guid id)
    {
        Exercise exerciseEntity = await this._repository.GetExerciseByIdAsync(id);

        EditExerciseViewModel model = new EditExerciseViewModel()
        {
            Id = exerciseEntity.Id,
            Name = exerciseEntity.Name,
            Description = exerciseEntity.Description,
            ImageUrl= exerciseEntity.ImageUrl,
            Measurement = exerciseEntity.MeasurementType,
            Category = exerciseEntity.Category,
            ExerciseType = exerciseEntity.ExerciseType,
            Difficulty = exerciseEntity.Difficulty,
            AvailableExercises = await this.GetAvailableSkillsAsync()
        };

        this.FetchEnums(model);

        return model;
    }

    public async Task<bool> ExerciseNameExcludingCurrentExistsAsync(Guid id, string name)
        => await this._repository.ExerciseNameExcludingCurrentExistsAsync(id, name);

    public async Task DeleteExerciseAsync(Guid id)
    {
        Exercise exerciseEntity = await this._repository.GetExerciseByIdAsync(id);

        bool successfulDelete = await this._repository.HardDeleteExerciseAsync(exerciseEntity);

        if (!successfulDelete)
            throw new EntityDeleteException();
    }

    public async Task<List<SelectListItem>> GetAvailableSkillsAsync()
    {
        return await this._repository
            .GetAllSkills()
            .Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            })
            .ToListAsync();

    }

    public async Task<bool> SkillExistsAsync(Guid id)
        => await this._repository.SkillExistsAsync(id);
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
}
