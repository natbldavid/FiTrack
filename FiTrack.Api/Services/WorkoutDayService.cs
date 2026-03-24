using FiTrack.Api.Data;
using FiTrack.Api.Dtos.WorkoutDays.Requests;
using FiTrack.Api.Dtos.WorkoutDays.Responses;
using FiTrack.Api.Models.Gym;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class WorkoutDayService : IWorkoutDayService
{
    private readonly FiTrackDbContext _context;

    public WorkoutDayService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutDayDetailResponseDto> CreateWorkoutDayAsync(int userId, CreateWorkoutDayRequestDto request)
    {
        await ValidateExercisesAsync(request.Exercises);

        var now = DateTime.UtcNow;

        var workoutDay = new WorkoutDay
        {
            UserId = userId,
            Name = request.Name,
            MuscleFocus = request.MuscleFocus,
            SortOrder = request.SortOrder,
            CreatedAt = now,
            UpdatedAt = now,
            WorkoutDayExercises = request.Exercises.Select(x => new WorkoutDayExercise
            {
                ExerciseId = x.ExerciseId,
                ExerciseOrder = x.ExerciseOrder,
                TargetSets = x.TargetSets,
                TargetRepsMin = x.TargetRepsMin,
                TargetRepsMax = x.TargetRepsMax,
                InitialWeight = x.InitialWeight,
                CurrentWorkingWeight = x.CurrentWorkingWeight,
                Notes = x.Notes,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList()
        };

        _context.WorkoutDays.Add(workoutDay);
        await _context.SaveChangesAsync();

        return await GetWorkoutDayByIdAsync(userId, workoutDay.Id)
            ?? throw new InvalidOperationException("Workout day was created but could not be loaded.");
    }

    public async Task<WorkoutDayDetailResponseDto?> UpdateWorkoutDayAsync(int userId, int workoutDayId, UpdateWorkoutDayRequestDto request)
    {
        var workoutDay = await _context.WorkoutDays
            .Include(w => w.WorkoutDayExercises)
            .FirstOrDefaultAsync(w => w.Id == workoutDayId && w.UserId == userId);

        if (workoutDay is null)
        {
            return null;
        }

        await ValidateExercisesAsync(request.Exercises);

        var now = DateTime.UtcNow;

        workoutDay.Name = request.Name;
        workoutDay.MuscleFocus = request.MuscleFocus;
        workoutDay.SortOrder = request.SortOrder;
        workoutDay.UpdatedAt = now;

        _context.Set<WorkoutDayExercise>().RemoveRange(workoutDay.WorkoutDayExercises);

        workoutDay.WorkoutDayExercises = request.Exercises.Select(x => new WorkoutDayExercise
        {
            WorkoutDayId = workoutDay.Id,
            ExerciseId = x.ExerciseId,
            ExerciseOrder = x.ExerciseOrder,
            TargetSets = x.TargetSets,
            TargetRepsMin = x.TargetRepsMin,
            TargetRepsMax = x.TargetRepsMax,
            InitialWeight = x.InitialWeight,
            CurrentWorkingWeight = x.CurrentWorkingWeight,
            Notes = x.Notes,
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();

        await _context.SaveChangesAsync();

        return await GetWorkoutDayByIdAsync(userId, workoutDay.Id);
    }

    public async Task<List<WorkoutDaySummaryResponseDto>> GetWorkoutDaysAsync(int userId)
    {
        var workoutDays = await _context.WorkoutDays
            .Where(w => w.UserId == userId)
            .Include(w => w.WorkoutDayExercises)
            .Include(w => w.WorkoutSessions)
            .OrderBy(w => w.SortOrder)
            .ThenBy(w => w.Name)
            .ToListAsync();

        return workoutDays.Select(w => new WorkoutDaySummaryResponseDto
        {
            Id = w.Id,
            Name = w.Name,
            MuscleFocus = w.MuscleFocus,
            SortOrder = w.SortOrder,
            ExerciseCount = w.WorkoutDayExercises.Count,
            LastCompletedDate = w.WorkoutSessions
                .Where(s => s.CompletedAt.HasValue)
                .OrderByDescending(s => s.SessionDate)
                .Select(s => (DateOnly?)s.SessionDate)
                .FirstOrDefault()
        }).ToList();
    }

    public async Task<WorkoutDayDetailResponseDto?> GetWorkoutDayByIdAsync(int userId, int workoutDayId)
    {
        var workoutDay = await _context.WorkoutDays
            .Include(w => w.WorkoutDayExercises)
                .ThenInclude(x => x.ExerciseCatalog)
            .FirstOrDefaultAsync(w => w.Id == workoutDayId && w.UserId == userId);

        if (workoutDay is null)
        {
            return null;
        }

        return new WorkoutDayDetailResponseDto
        {
            Id = workoutDay.Id,
            Name = workoutDay.Name,
            MuscleFocus = workoutDay.MuscleFocus,
            SortOrder = workoutDay.SortOrder,
            Exercises = workoutDay.WorkoutDayExercises
                .OrderBy(x => x.ExerciseOrder)
                .Select(x => new WorkoutDayExerciseResponseDto
                {
                    Id = x.Id,
                    ExerciseId = x.ExerciseId,
                    ExerciseName = x.ExerciseCatalog.Name,
                    BodyPart = x.ExerciseCatalog.BodyPart,
                    ExerciseType = x.ExerciseCatalog.ExerciseType,
                    ExerciseOrder = x.ExerciseOrder,
                    TargetSets = x.TargetSets,
                    TargetRepsMin = x.TargetRepsMin,
                    TargetRepsMax = x.TargetRepsMax,
                    InitialWeight = x.InitialWeight,
                    CurrentWorkingWeight = x.CurrentWorkingWeight,
                    Notes = x.Notes
                })
                .ToList()
        };
    }

    private async Task ValidateExercisesAsync(List<WorkoutDayExerciseRequestDto> exercises)
    {
        var exerciseIds = exercises
            .Select(x => x.ExerciseId)
            .Distinct()
            .ToList();

        var validIds = await _context.Set<ExerciseCatalog>()
            .Where(e => e.IsActive && exerciseIds.Contains(e.Id))
            .Select(e => e.Id)
            .ToListAsync();

        if (validIds.Count != exerciseIds.Count)
        {
            throw new KeyNotFoundException("One or more exercises were not found or are inactive.");
        }

        if (exercises.Any(x => x.TargetRepsMin > x.TargetRepsMax))
        {
            throw new InvalidOperationException("TargetRepsMin cannot be greater than TargetRepsMax.");
        }
    }
}