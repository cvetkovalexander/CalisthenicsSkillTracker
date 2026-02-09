using CalisthenicsSkillTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Configuration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    private readonly User[] _users =
    {
        new User
        {
            Id = Guid.Parse("c7cebde1-1c04-4bff-a388-d42961e5ece7"),
            PasswordHash = "Pass123!",
            Username = "athlete01",
            FirstName = "Alex",
            LastName = "Petrov",
            Email = "alex.petrov@example.com",
            CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = Guid.Parse("82042202-ba9c-44f5-86fb-4b191dbf966c"),
            PasswordHash = "Fit2026!",
            Username = "fitgirl92",
            FirstName = "Maria",
            LastName = "Ivanova",
            Email = "maria.ivanova@example.com",
            CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = Guid.Parse("91fca50a-7cdd-430f-b08c-dbdaa597ba26"),
            PasswordHash = "GytTime1!",
            Username = "gymking",
            FirstName = "Daniel",
            LastName = "Georgiev",
            Email = "daniel.georgiev@example.com",
            CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = Guid.Parse("776ada73-4ef6-4c12-8934-1e79dbb5e03e"),
            PasswordHash = "Calis2026!",
            Username = "calisthenics_pro",
            FirstName = "Elena",
            LastName = "Dimitrova",
            Email = "elena.dimitrova@example.com",
            CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = Guid.Parse("e70607c0-c065-43ce-b602-15b689784194"),
            PasswordHash = "StrongMuscle1!",
            Username = "muscle_man",
            FirstName = "Ivan",
            LastName = "Kostov",
            Email = "ivan.kostov@example.com",
            CreatedAt = DateTime.UtcNow
        }
    };

    public void Configure(EntityTypeBuilder<User> builder)
    {
        //builder
        //    .HasData(this._users);
    }
}
