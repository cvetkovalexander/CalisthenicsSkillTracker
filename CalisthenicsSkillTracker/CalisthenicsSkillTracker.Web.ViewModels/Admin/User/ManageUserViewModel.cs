namespace CalisthenicsSkillTracker.ViewModels.Admin.User;

public class ManageUserViewModel
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public IEnumerable<string> Roles { get; set; }
         = new List<string>();
}
