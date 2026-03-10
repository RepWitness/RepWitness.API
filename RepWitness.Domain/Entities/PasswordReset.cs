namespace RepWitness.Domain.Entities;

public class PasswordReset
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Link { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsDeleted { get; set; }
}
