using RepWitness.Domain.Enums;

namespace RepWitness.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public string Username { get; set; } = null!; 
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int Height { get; set; }
    public float Weight { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public bool IsDeleted { get; set; }
}
