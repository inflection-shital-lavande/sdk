using System.ComponentModel.DataAnnotations;

namespace sdk.demo.src.api.user.UserModel;

public class UserCreateModel
{
    public int RoleId { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(100, MinimumLength = 1)]
    public string Email { get; set; } = string.Empty;

    [StringLength(10, MinimumLength = 1)]
    public string Gender { get; set; } = string.Empty;

    [StringLength(5, MinimumLength = 1)]
    public string CountryCode { get; set; } = string.Empty;

    [StringLength(15, MinimumLength = 1)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}

public class UserUpdateModel
{
    public int RoleId { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 0)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(100, MinimumLength = 0)]
    public string Email { get; set; } = string.Empty;

    [StringLength(10, MinimumLength = 0)]
    public string Gender { get; set; } = string.Empty;

    [StringLength(5, MinimumLength = 0)]
    public string CountryCode { get; set; } = string.Empty;

    [StringLength(15, MinimumLength = 0)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}

public class UserSearchFilters
{
    [StringLength(50, MinimumLength = 0)]
    public string? FirstName { get; set; }
}
