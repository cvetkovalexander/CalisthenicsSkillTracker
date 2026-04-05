using CalisthenicsSkillTracker.ViewModels.Favorites;

namespace CalisthenicsSkillTracker.Services.Core.Contracts;

public interface IFavoritesService
{
    Task<FavoritesViewModel> GetUserFavoritesAsync(Guid userId);

    Task<List<FavoriteSkillViewModel>> GetUserFavoriteSkillsAsync(Guid userId);

    Task<List<FavoriteExerciseViewModel>> GetUserFavoriteExercisesAsync(Guid userId);
}
