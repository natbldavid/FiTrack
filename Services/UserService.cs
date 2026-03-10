using System;
using FiTrack.Api.Data;
using FiTrack.Api.Dtos.Users.Requests;
using FiTrack.Api.Dtos.Users.Responses;
using FiTrack.Api.Models.Users;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FiTrack.Api.Services;

public class UserService : IUserService
{
    private readonly FiTrackDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public UserService(
        FiTrackDbContext context,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<UserResponseDto> RegisterUserAsync(RegisterUserRequestDto request)
    {
        var usernameExists = await _context.Users
            .AnyAsync(u => u.Username == request.Username);

        if (usernameExists)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var now = DateTime.UtcNow;

        var user = new User
        {
            Username = request.Username,
            CreatedAt = now,
            UpdatedAt = now
        };

        user.PasscodeHash = _passwordHasher.HashPassword(user, request.Passcode);

        var hasAnyProfileData =
            request.CurrentWeight.HasValue ||
            request.DailyCalorieGoal.HasValue ||
            request.DailyProteinGoal.HasValue ||
            request.DailyCarbGoal.HasValue ||
            request.DailyFatGoal.HasValue ||
            request.WeightGoal.HasValue ||
            request.WeeklyExerciseGoal.HasValue;

        if (hasAnyProfileData)
        {
            user.Profile = new UserProfile
            {
                CurrentWeight = request.CurrentWeight,
                DailyCalorieGoal = request.DailyCalorieGoal,
                DailyProteinGoal = request.DailyProteinGoal,
                DailyCarbGoal = request.DailyCarbGoal,
                DailyFatGoal = request.DailyFatGoal,
                WeightGoal = request.WeightGoal,
                WeeklyExerciseGoal = request.WeeklyExerciseGoal,
                UpdatedAt = now
            };
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return MapToUserResponseDto(user);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            return null;
        }

        return MapToUserResponseDto(user);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = jwtSettings["Key"]
            ?? throw new InvalidOperationException("JWT Key is not configured.");

        var issuer = jwtSettings["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is not configured.");

        var audience = jwtSettings["Audience"]
            ?? throw new InvalidOperationException("JWT Audience is not configured.");

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid username or passcode.");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasscodeHash,
            request.Passcode);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid username or passcode.");
        }

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Token = token
        };
    }

    public async Task<UserProfileResponseDto?> GetUserProfileAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null || user.Profile is null)
        {
            return null;
        }

        return new UserProfileResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            CurrentWeight = user.Profile.CurrentWeight,
            DailyCalorieGoal = user.Profile.DailyCalorieGoal,
            DailyProteinGoal = user.Profile.DailyProteinGoal,
            DailyCarbGoal = user.Profile.DailyCarbGoal,
            DailyFatGoal = user.Profile.DailyFatGoal,
            WeightGoal = user.Profile.WeightGoal,
            WeeklyExerciseGoal = user.Profile.WeeklyExerciseGoal,
            UpdatedAt = user.Profile.UpdatedAt
        };
    }

    public async Task<UserProfileResponseDto?> UpdateUserProfileAsync(int userId, UpdateUserProfileRequestDto request)
    {
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return null;
        }

        var now = DateTime.UtcNow;

        if (user.Profile is null)
        {
            user.Profile = new UserProfile
            {
                UserId = user.Id,
                UpdatedAt = now
            };
        }

        user.Profile.CurrentWeight = request.CurrentWeight;
        user.Profile.DailyCalorieGoal = request.DailyCalorieGoal;
        user.Profile.DailyProteinGoal = request.DailyProteinGoal;
        user.Profile.DailyCarbGoal = request.DailyCarbGoal;
        user.Profile.DailyFatGoal = request.DailyFatGoal;
        user.Profile.WeightGoal = request.WeightGoal;
        user.Profile.WeeklyExerciseGoal = request.WeeklyExerciseGoal;
        user.Profile.UpdatedAt = now;

        user.UpdatedAt = now;

        await _context.SaveChangesAsync();

        return new UserProfileResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            CurrentWeight = user.Profile.CurrentWeight,
            DailyCalorieGoal = user.Profile.DailyCalorieGoal,
            DailyProteinGoal = user.Profile.DailyProteinGoal,
            DailyCarbGoal = user.Profile.DailyCarbGoal,
            DailyFatGoal = user.Profile.DailyFatGoal,
            WeightGoal = user.Profile.WeightGoal,
            WeeklyExerciseGoal = user.Profile.WeeklyExerciseGoal,
            UpdatedAt = user.Profile.UpdatedAt
        };
    }

    public async Task<bool> ChangePasscodeAsync(int userId, ChangePasscodeRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return false;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasscodeHash,
            request.CurrentPasscode);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Current passcode is incorrect.");
        }

        if (request.CurrentPasscode == request.NewPasscode)
        {
            throw new InvalidOperationException("New passcode must be different from the current passcode.");
        }

        user.PasscodeHash = _passwordHasher.HashPassword(user, request.NewPasscode);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static UserResponseDto MapToUserResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            Profile = user.Profile is null
                ? null
                : new UserProfileResponseDto
                {
                    CurrentWeight = user.Profile.CurrentWeight,
                    DailyCalorieGoal = user.Profile.DailyCalorieGoal,
                    DailyProteinGoal = user.Profile.DailyProteinGoal,
                    DailyCarbGoal = user.Profile.DailyCarbGoal,
                    DailyFatGoal = user.Profile.DailyFatGoal,
                    WeightGoal = user.Profile.WeightGoal,
                    WeeklyExerciseGoal = user.Profile.WeeklyExerciseGoal,
                    UpdatedAt = user.Profile.UpdatedAt
                }
        };
    }


}