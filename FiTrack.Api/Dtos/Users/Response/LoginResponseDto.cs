namespace FiTrack.Api.Dtos.Users.Responses;

public class LoginResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}