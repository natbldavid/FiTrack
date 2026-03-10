using FiTrack.Api.Data;
using FiTrack.Api.Dtos.WorkoutSessions.Requests;
using FiTrack.Api.Dtos.WorkoutSessions.Responses;
using FiTrack.Api.Models.Gym;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class WorkoutSessionService : IWorkoutSessionService
{
    private readonly FiTrackDbContext _context;

    public WorkoutSessionService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutSessionResponseDto> StartWorkoutSessionAsync(int userId, StartWorkoutSessionRequestDto request)
    {
        var workoutDay = await _context.WorkoutDays
            .Include(w => w.WorkoutDayExercises)
                .ThenInclude(x => x.ExerciseCatalog)
            .FirstOrDefaultAsync(w => w.Id == request.WorkoutDayId && w.UserId == userId);

        if (workoutDay is null)
        {
            throw new KeyNotFoundException("Workout day not found.");
        }

        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);

        var session = new WorkoutSession
        {
            UserId = userId,
            WorkoutDayId = workoutDay.Id,
            SessionName = workoutDay.Name,
            SessionDate = today,
            StartedAt = now,
            Status = "in_progress",
            CreatedAt = now,
            UpdatedAt = now,
            WorkoutSessionExercises = workoutDay.WorkoutDayExercises
                .OrderBy(x => x.ExerciseOrder)
                .Select(x => new WorkoutSessionExercise
                {
                    ExerciseId = x.ExerciseId,
                    ExerciseNameSnapshot = x.ExerciseCatalog.Name,
                    BodyPartSnapshot = x.ExerciseCatalog.BodyPart,
                    ExerciseTypeSnapshot = x.ExerciseCatalog.ExerciseType,
                    ExerciseOrder = x.ExerciseOrder,
                    TargetSets = x.TargetSets,
                    TargetRepsMin = x.TargetRepsMin,
                    TargetRepsMax = x.TargetRepsMax,
                    PlannedWorkingWeight = x.CurrentWorkingWeight,
                    ActualWorkingWeight = null,
                    Notes = x.Notes,
                    CreatedAt = now,
                    UpdatedAt = now
                }).ToList()
        };

        _context.WorkoutSessions.Add(session);
        await _context.SaveChangesAsync();

        return await GetWorkoutSessionByIdAsync(userId, session.Id)
            ?? throw new InvalidOperationException("Workout session was created but could not be loaded.");
    }

    public async Task<WorkoutSessionResponseDto?> GetWorkoutSessionByIdAsync(int userId, int workoutSessionId)
    {
        var session = await _context.WorkoutSessions
            .Include(s => s.WorkoutSessionExercises)
                .ThenInclude(x => x.WorkoutSetLogs)
            .FirstOrDefaultAsync(s => s.Id == workoutSessionId && s.UserId == userId);

        if (session is null)
        {
            return null;
        }

        return MapToWorkoutSessionResponseDto(session);
    }

    public async Task<List<WorkoutSessionSummaryResponseDto>> GetWorkoutSessionsAsync(int userId)
    {
        var sessions = await _context.WorkoutSessions
            .Where(s => s.UserId == userId)
            .Include(s => s.WorkoutSessionExercises)
            .OrderByDescending(s => s.SessionDate)
            .ThenByDescending(s => s.StartedAt)
            .ToListAsync();

        return sessions.Select(s => new WorkoutSessionSummaryResponseDto
        {
            Id = s.Id,
            SessionName = s.SessionName,
            SessionDate = s.SessionDate,
            StartedAt = s.StartedAt,
            CompletedAt = s.CompletedAt,
            Status = s.Status,
            ExerciseCount = s.WorkoutSessionExercises.Count
        }).ToList();
    }

    public async Task<WorkoutSessionResponseDto?> UpsertWorkoutSetLogAsync(int userId, int workoutSessionId, UpdateWorkoutSetLogRequestDto request)
    {
        var session = await _context.WorkoutSessions
            .Include(s => s.WorkoutSessionExercises)
                .ThenInclude(x => x.WorkoutSetLogs)
            .FirstOrDefaultAsync(s => s.Id == workoutSessionId && s.UserId == userId);

        if (session is null)
        {
            return null;
        }

        if (session.Status == "completed")
        {
            throw new InvalidOperationException("Completed sessions cannot be modified.");
        }

        var sessionExercise = session.WorkoutSessionExercises
            .FirstOrDefault(x => x.Id == request.WorkoutSessionExerciseId);

        if (sessionExercise is null)
        {
            throw new KeyNotFoundException("Workout session exercise not found.");
        }

        var now = DateTime.UtcNow;

        var setLog = sessionExercise.WorkoutSetLogs
            .FirstOrDefault(s => s.SetNumber == request.SetNumber);

        if (setLog is null)
        {
            setLog = new WorkoutSetLog
            {
                WorkoutSessionExerciseId = sessionExercise.Id,
                SetNumber = request.SetNumber,
                Reps = request.Reps,
                Weight = request.Weight,
                Completed = request.Completed,
                CreatedAt = now,
                UpdatedAt = now
            };

            sessionExercise.WorkoutSetLogs.Add(setLog);
        }
        else
        {
            setLog.Reps = request.Reps;
            setLog.Weight = request.Weight;
            setLog.Completed = request.Completed;
            setLog.UpdatedAt = now;
        }

        sessionExercise.ActualWorkingWeight = request.Weight;
        sessionExercise.UpdatedAt = now;

        session.UpdatedAt = now;

        await _context.SaveChangesAsync();

        return MapToWorkoutSessionResponseDto(session);
    }

    public async Task<WorkoutSessionResponseDto?> CompleteWorkoutSessionAsync(int userId, int workoutSessionId, CompleteWorkoutSessionRequestDto request)
    {
        var session = await _context.WorkoutSessions
            .Include(s => s.WorkoutSessionExercises)
            .FirstOrDefaultAsync(s => s.Id == workoutSessionId && s.UserId == userId);

        if (session is null)
        {
            return null;
        }

        if (session.Status == "completed")
        {
            throw new InvalidOperationException("Session is already completed.");
        }

        var now = DateTime.UtcNow;

        session.Status = "completed";
        session.CompletedAt = now;
        session.Notes = request.Notes;
        session.UpdatedAt = now;

        if (session.WorkoutDayId.HasValue && request.WorkingWeightUpdates.Count > 0)
        {
            var workoutDay = await _context.WorkoutDays
                .Include(w => w.WorkoutDayExercises)
                .FirstOrDefaultAsync(w => w.Id == session.WorkoutDayId.Value && w.UserId == userId);

            if (workoutDay is not null)
            {
                foreach (var update in request.WorkingWeightUpdates)
                {
                    var workoutDayExercise = workoutDay.WorkoutDayExercises
                        .FirstOrDefault(x => x.Id == update.WorkoutDayExerciseId);

                    if (workoutDayExercise is not null)
                    {
                        workoutDayExercise.CurrentWorkingWeight = update.CurrentWorkingWeight;
                        workoutDayExercise.UpdatedAt = now;
                    }
                }
            }
        }

        await _context.SaveChangesAsync();

        return await GetWorkoutSessionByIdAsync(userId, session.Id);
    }

    private static WorkoutSessionResponseDto MapToWorkoutSessionResponseDto(WorkoutSession session)
    {
        return new WorkoutSessionResponseDto
        {
            Id = session.Id,
            WorkoutDayId = session.WorkoutDayId,
            SessionName = session.SessionName,
            SessionDate = session.SessionDate,
            StartedAt = session.StartedAt,
            CompletedAt = session.CompletedAt,
            Status = session.Status,
            Notes = session.Notes,
            Exercises = session.WorkoutSessionExercises
                .OrderBy(x => x.ExerciseOrder)
                .Select(x => new WorkoutSessionExerciseResponseDto
                {
                    Id = x.Id,
                    ExerciseId = x.ExerciseId,
                    ExerciseNameSnapshot = x.ExerciseNameSnapshot,
                    BodyPartSnapshot = x.BodyPartSnapshot,
                    ExerciseTypeSnapshot = x.ExerciseTypeSnapshot,
                    ExerciseOrder = x.ExerciseOrder,
                    TargetSets = x.TargetSets,
                    TargetRepsMin = x.TargetRepsMin,
                    TargetRepsMax = x.TargetRepsMax,
                    PlannedWorkingWeight = x.PlannedWorkingWeight,
                    ActualWorkingWeight = x.ActualWorkingWeight,
                    Notes = x.Notes,
                    Sets = x.WorkoutSetLogs
                        .OrderBy(s => s.SetNumber)
                        .Select(s => new WorkoutSetLogResponseDto
                        {
                            Id = s.Id,
                            SetNumber = s.SetNumber,
                            Reps = s.Reps,
                            Weight = s.Weight,
                            Completed = s.Completed
                        }).ToList()
                }).ToList()
        };
    }
}