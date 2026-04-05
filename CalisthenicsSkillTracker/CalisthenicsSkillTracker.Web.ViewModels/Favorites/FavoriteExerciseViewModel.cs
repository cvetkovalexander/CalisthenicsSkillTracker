namespace CalisthenicsSkillTracker.ViewModels.Favorites;

public class FavoriteExerciseViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }
}
