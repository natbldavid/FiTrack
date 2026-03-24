namespace FiTrack.Api.Dtos.Users.Responses;

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;
}