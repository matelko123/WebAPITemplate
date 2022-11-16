using System.ComponentModel.DataAnnotations;
using WebAPITemplate.Shared.Models.Interfaces;

namespace WebAPITemplate.Shared.Models.Entities;

public class User : IAuditableEntity
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsEnabled { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastModificationUtc { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}
