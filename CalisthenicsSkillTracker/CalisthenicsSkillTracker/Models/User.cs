namespace CalisthenicsSkillTracker.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityConstants;
using static Common.EntityValidation.User;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(UsernameMaxLength)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [MaxLength(LastNameMaxLength)]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(EmailMaxLength)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(PasswordMaxLength)]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [Column(TypeName = DateTimeColumnType)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<SkillProgress> SkillProgressRecords { get; set; }
        = new List<SkillProgress>();
}
