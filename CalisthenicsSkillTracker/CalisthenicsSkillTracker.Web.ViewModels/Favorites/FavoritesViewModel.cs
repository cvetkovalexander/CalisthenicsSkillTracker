namespace CalisthenicsSkillTracker.ViewModels.Favorites;

public class FavoritesViewModel
{
    public ICollection<FavoriteExerciseViewModel> FavoriteExercises { get; set; }
        = new List<FavoriteExerciseViewModel>();

    public ICollection<FavoriteSkillViewModel> FavoriteSkills { get; set; }
        = new List<FavoriteSkillViewModel>();
}
