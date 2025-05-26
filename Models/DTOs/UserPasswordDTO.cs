namespace EventFlow_API.DTOs;

public class UserPasswordUpdateDTO
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
