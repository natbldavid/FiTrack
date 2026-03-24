using FiTrack.Api.Models.Users;
using FiTrack.Api.Models.Food;
using FiTrack.Api.Models.Gym;
using Microsoft.EntityFrameworkCore;
using FiTrack.Api.Models.Activities;

namespace FiTrack.Api.Data;

public class FiTrackDbContext : DbContext
{
    public FiTrackDbContext(DbContextOptions<FiTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public DbSet<WeightLog> WeightLogs => Set<WeightLog>();

    public DbSet<UserGoalsHistory> UserGoalsHistory => Set<UserGoalsHistory>();

    public DbSet<Food> Foods => Set<Food>();

    public DbSet<Meal> Meals => Set<Meal>();

    public DbSet<MealItem> MealItems => Set<MealItem>();

    public DbSet<FoodLog> FoodLogs => Set<FoodLog>();

    public DbSet<ExerciseCatalog> ExerciseCatalog => Set<ExerciseCatalog>();

    public DbSet<WorkoutDay> WorkoutDays => Set<WorkoutDay>();

    public DbSet<WorkoutDayExercise> WorkoutDayExercises => Set<WorkoutDayExercise>();

    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();

    public DbSet<WorkoutSessionExercise> WorkoutSessionExercises => Set<WorkoutSessionExercise>();

    public DbSet<WorkoutSetLog> WorkoutSetLogs => Set<WorkoutSetLog>();

    public DbSet<ActivityType> ActivityTypes => Set<ActivityType>();

    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FiTrackDbContext).Assembly);
    }
}