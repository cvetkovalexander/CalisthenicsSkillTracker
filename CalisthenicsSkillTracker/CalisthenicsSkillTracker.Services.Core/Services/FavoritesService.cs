using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Services.Core.Contracts;
using CalisthenicsSkillTracker.ViewModels.Favorites;

namespace CalisthenicsSkillTracker.Services.Core.Services;

public class FavoritesService : IFavoritesService
{
    private readonly IExerciseRepository _exerciseRepository;

    private readonly ISkillRepository _skillRepository;

    public FavoritesService(IExerciseRepository exerciseRepository, ISkillRepository skillRepository)   
    {
        this._exerciseRepository = exerciseRepository;
        this._skillRepository = skillRepository;
    }

    public async Task<FavoritesViewModel> GetUserFavoritesAsync(Guid userId)
        => new FavoritesViewModel
        {
            FavoriteExercises = await this.GetUserFavoriteExercisesAsync(userId),
            FavoriteSkills = await this.GetUserFavoriteSkillsAsync(userId)
        };

    public async Task<List<FavoriteExerciseViewModel>> GetUserFavoriteExercisesAsync(Guid userId) 
    {
        ApplicationUser user = await this._exerciseRepository.GetUserWithFavoriteExercisesAsync(userId);

        return user.FavoriteExercises.Select(e => new FavoriteExerciseViewModel
        {
            Id = e.Id,
            Name = e.Name,
            ImageUrl = e.ImageUrl
        })
        .ToList();
    }
            

    public async Task<List<FavoriteSkillViewModel>> GetUserFavoriteSkillsAsync(Guid userId)
    {
        ApplicationUser user = await this._skillRepository.GetUserWithFavoriteSkillsAsync(userId);

        return user.FavoriteSkills.Select(s => new FavoriteSkillViewModel
        {
            Id = s.Id,
            Name = s.Name,
            ImageUrl = s.ImageUrl
        })
        .ToList();
    }
}
