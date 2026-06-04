namespace SharedKernel.Infrastructure.Persistence;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    Guid CreatedBy { get; set; }
    DateTime? ModifiedAt { get; set; }
    Guid? ModifiedBy { get; set; }
}