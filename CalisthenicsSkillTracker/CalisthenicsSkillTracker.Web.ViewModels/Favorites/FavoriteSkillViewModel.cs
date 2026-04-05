namespace CalisthenicsSkillTracker.ViewModels.Favorites;

public class FavoriteSkillViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }
}
