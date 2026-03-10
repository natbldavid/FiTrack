using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Users.Requests;

public class ChangePasscodeRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string CurrentPasscode { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPasscode { get; set; } = null!;
}