using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class SkillInputService : ISkillInputService
{
    private const string MeasurementKey = "Measurement";

    private const string CategoryKey = "Category";

    private const string SkillTypeKey = "SkillType";

    private const string DifficultyKey = "Difficulty";

    private readonly ApplicationDbContext _context;

    public SkillInputService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task CreateSkillAsync(CreateSkillViewModel model)
    {
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

        await this._context.Skills.AddAsync(skill);
        await this._context.SaveChangesAsync();
    }

    public async Task<EditSkillViewModel> CreateEditSkillViewModelAsync(Guid id)
    {
        Skill skillEntity = await this.GetSkillByIdAsync(id);

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
        Skill skill = await this.GetSkillByIdAsync(model.Id);

        skill.Name = model.Name;
        skill.Description = model.Description;
        skill.ImageUrl = model.ImageUrl;
        skill.MeasurementType = model.Measurement;
        skill.Category = model.Category;
        skill.SkillType = model.SkillType;
        skill.Difficulty = model.Difficulty;

        this._context.Skills.Update(skill);
        await this._context.SaveChangesAsync();
    }

    public async Task DeleteSkillAsync(Guid id)
    {
        Skill skillEntity = await this.GetSkillByIdAsync(id);

        this._context.Skills.Remove(skillEntity);

        await this._context.SaveChangesAsync();
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

    public string RemoveWhitespaces(string input)
        => string.Concat(input.Where(c => !char.IsWhiteSpace(c)));

    public async Task<bool> SkillNameExistsAsync(string name)
        => await this._context.Skills.AnyAsync(s => s.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());

    public async Task<bool> SkillNameExcludingCurrentExistsAsync(Guid id, string name)
    {
        return await this._context
            .Skills
            .AsNoTracking()
            .AnyAsync(s => s.Id != id && s.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());
    }

    public async Task<Skill> GetSkillByIdAsync(Guid id)
    {
        return await this._context
            .Skills
            .AsNoTracking()
            .SingleAsync(s => s.Id == id);
    }

    public async Task<List<SelectListItem>> GetAvailableExercisesAsync()
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
}
