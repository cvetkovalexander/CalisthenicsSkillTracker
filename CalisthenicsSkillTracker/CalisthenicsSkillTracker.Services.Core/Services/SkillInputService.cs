using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillInputService : ISkillInputService
{
    private const string MeasurementKey = "Measurement";

    private const string CategoryKey = "Category";

    private const string SkillTypeKey = "SkillType";

    private const string DifficultyKey = "Difficulty";

    private readonly ISkillRepository _repository;

    public SkillInputService(ISkillRepository repository)
    {
        this._repository = repository;
    }

    public async Task CreateSkillAsync(CreateSkillViewModel model)
    {
        throw new Exception();

        Skill skill = new Skill()
        {
            Name = model.Name,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            MeasurementType = model.Measurement,
            Category = model.Category,
            SkillType = model.SkillType,
            Difficulty = model.Difficulty
        };

        bool successfulAdd = await this._repository.AddSkillAsync(skill);
        if (!successfulAdd)
            throw new EntityCreatePersistException();
    }

    public async Task<EditSkillViewModel> CreateEditSkillViewModelAsync(Guid id)
    {
        Skill skillEntity = await this._repository.GetSkillByIdAsync(id);

        EditSkillViewModel model = new EditSkillViewModel()
        {
            Id = skillEntity.Id,
            Name = skillEntity.Name,
            Description = skillEntity.Description,
            ImageUrl = skillEntity.ImageUrl,
            Measurement = skillEntity.MeasurementType,
            Category = skillEntity.Category,
            SkillType = skillEntity.SkillType,
            Difficulty = skillEntity.Difficulty
        };

        this.FetchEnums(model);

        return model;
    }

    public async Task EditSkillDataAsync(EditSkillViewModel model)
    {
        Skill skill = await this._repository.GetSkillByIdAsync(model.Id);

        skill.Name = model.Name;
        skill.Description = model.Description;
        skill.ImageUrl = model.ImageUrl;
        skill.MeasurementType = model.Measurement;
        skill.Category = model.Category;
        skill.SkillType = model.SkillType;
        skill.Difficulty = model.Difficulty;

        bool successfulEdit = await this._repository.EditSkillAsync(skill);
        if (!successfulEdit)
            throw new EntityEditPersistException();
    }

    // TODO: Implement soft and hard delete.
    public async Task DeleteSkillAsync(Guid id)
    {
        Skill skillEntity = await this._repository.GetSkillByIdAsync(id);

        bool successfulDelete = await this._repository.HardDeleteSkillAsync(skillEntity);
        if (!successfulDelete)
            throw new EntityDeleteException();
    }

    public async Task<bool> ToggleFavoriteAsync(Guid skillId, Guid userId)
    {
        ApplicationUser user = await this._repository.GetUserWithFavoriteSkillsAsync(userId);
        Skill skill = await this._repository.GetTrackedSkillByIdAsync(skillId);

        Skill? existingFavorite = user.FavoriteSkills
            .FirstOrDefault(s => s.Id == skillId);

        if (existingFavorite is not null)
        {
            bool removed = await this._repository.RemoveSkillFromFavorites(skill, user);
            if (!removed)
                throw new EntityFavoritePersistException();

            return false;
        }

        bool added = await this._repository.AddSkillToFavorites(skill, user);
        if (!added)
            throw new EntityFavoritePersistException();

        return true;
    }

    /* Helper methods */
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

    public CreateSkillViewModel CreateSkillViewModelWithEnums()
    {
        CreateSkillViewModel model = new CreateSkillViewModel();

        this.FetchEnums(model);

        return model;
    }

    public void FetchEnums(ISkillViewModel model)
    {
        model.MeasurementOptions = this.FetchSelectedEnum(MeasurementKey);
        model.CategoryOptions = this.FetchSelectedEnum(CategoryKey);
        model.SkillTypeOptions = this.FetchSelectedEnum(SkillTypeKey);
        model.DifficultyOptions = this.FetchSelectedEnum(DifficultyKey);
    }
    public async Task<bool> SkillNameExistsAsync(string name)
        => await this._repository.SkillNameExistsAsync(name);

    public async Task<bool> SkillNameExcludingCurrentExistsAsync(Guid id, string name)
        => await this._repository.SkillNameExcludingCurrentExistsAsync(id, name);
}
