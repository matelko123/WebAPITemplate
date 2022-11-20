using System.ComponentModel.DataAnnotations;
using WebAPITemplate.Shared.Models.Interfaces;

namespace WebAPITemplate.Shared.Models.Entities;

public class User : IAuditableEntity
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [MinLength(5)]
    public string Username { get; set; }
    [MinLength(3)]
    public string FirstName { get; set; }
    [MinLength(3)]
    public string LastName { get; set; }
    [EmailAddress]
    [MinLength(10)]
    public string Email { get; set; }
    public bool IsEnabled { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastModificationUtc { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}
