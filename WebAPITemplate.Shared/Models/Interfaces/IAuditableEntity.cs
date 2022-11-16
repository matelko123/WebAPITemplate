namespace WebAPITemplate.Shared.Models.Interfaces;

public interface IAuditableEntity
{
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastModificationUtc { get; set; }
}