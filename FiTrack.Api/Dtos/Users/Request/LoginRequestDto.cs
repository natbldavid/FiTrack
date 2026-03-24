using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Users.Requests;

public class LoginRequestDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Passcode { get; set; } = null!;
}