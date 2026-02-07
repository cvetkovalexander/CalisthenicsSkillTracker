using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Models;
using CalisthenicsSkillTracker.Models.Enums;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CalisthenicsSkillTracker.Controllers;

public class WorkoutsController : Controller
{
    private readonly ApplicationDbContext _context;

    public WorkoutsController(ApplicationDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public IActionResult Log()
    {
        CreateWorkoutViewModel model = new CreateWorkoutViewModel()
        {
            Users = this.FetchUsers(),
            Date = DateTime.UtcNow
        };
        return this.View(model);
    }

    [HttpPost]
    public IActionResult Log(CreateWorkoutViewModel model)
    {
        model.Users = this.FetchUsers();

        if (!ModelState.IsValid)
        {
            return this.View(model);
        }

        User? user = this._context.Users.Find(model.UserId);
        if (user is null) 
        {
            ModelState.AddModelError(nameof(model.UserId), "Invalid user id!");

            return this.View(model);
        }

        Guid workoutId = Guid.NewGuid();

        try 
        {
            Workout workout = new Workout()
            {
                Id = workoutId,
                Date = model.Date,
                UserId = model.UserId,
                Notes = model.Notes
            };

            this._context.Workouts.Add(workout);
            this._context.SaveChanges();
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);

            ModelState.AddModelError(string.Empty, "An error occurred while creating the workout. Please try again.");

            return this.View(model);
        }

        return this.RedirectToAction("AddExercises", new { workoutId });
    }

    [HttpGet]
    public IActionResult AddExercises(Guid workoutId)
    {
        AddWorkoutExerciseViewModel model = new AddWorkoutExerciseViewModel()
        {
            WorkoutId = workoutId,
            AvailabeExercises = this.FetchAvailableExercises()
        };

        return this.View(model);
    }

    [HttpPost]
    public IActionResult AddExercises(AddWorkoutExerciseViewModel model)
    {
        model.AvailabeExercises = this.FetchAvailableExercises();
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please select exercise!");

            return this.View(model);
        }

        Exercise? exercise = this._context.Exercises.Find(model.ExerciseId);
        if (exercise is null) 
        {
            ModelState.AddModelError(string.Empty, "Invalid exercise id!");

            return this.View(model);
        }

        Guid workoutExerciseId = Guid.NewGuid();
        try 
        {
            WorkoutExercise workoutExercise = new WorkoutExercise()
            {
                Id = workoutExerciseId,
                WorkoutId = model.WorkoutId,
                ExerciseId = model.ExerciseId
            };

            this._context.WorkoutExercises.Add(workoutExercise);
            this._context.SaveChanges();
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            ModelState.AddModelError(string.Empty, "An error occurred while adding the exercise to the workout. Please try again.");
            return this.View(model);
        }

        TempData["SuccessMessage"] = "Exercise added successfully! You can add more exercises or proceed to add sets.";

        return this.RedirectToAction("AddExercises", new { workoutId = model.WorkoutId });
    }

    [HttpGet]
    public IActionResult AddSets(Guid workoutId)
    {
        Workout? workout = this._context.Workouts
            .AsNoTracking()
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .FirstOrDefault(w => w.Id == workoutId);

        if (workout == null) 
            return this.NotFound();

        AddWorkoutSetViewModel model = new AddWorkoutSetViewModel()
        {
            WorkoutId = workoutId,
            Exercises = workout.WorkoutExercises.Select(we => new SelectListItem
            {
                Value = we.Id.ToString(),
                Text = we.Exercise.Name
            }).ToList(),
            Progressions = Enum.GetValues<Progression>()
                .Select(p => new SelectListItem
                {
                    Value = p.ToString(),
                    Text = p.ToString()
                })
                .ToList()
        };

        return this.View(model);
    }

    [HttpPost]
    public IActionResult AddSets(AddWorkoutSetViewModel model)
    {
        model.Exercises = this._context.WorkoutExercises
            .AsNoTracking()
            .Where(we => we.WorkoutId == model.WorkoutId)
            .Include(we => we.Exercise)
            .Select(we => new SelectListItem
            {
                Value = we.ExerciseId.ToString(),
                Text = we.Exercise.Name
            })
            .ToList();

        if (!ModelState.IsValid) 
        {
            return this.View(model);
        }

        WorkoutExercise? workoutExercise = this._context.WorkoutExercises
            .FirstOrDefault(we => we.WorkoutId == model.WorkoutId && we.ExerciseId == model.WorkoutExerciseId);

        if (workoutExercise is null) 
        {
            ModelState.AddModelError(string.Empty, "Invalid exercise id!");

            return this.View(model);
        }

        try
        {
            WorkoutSet set = new WorkoutSet()
            {
                WorkoutExerciseId = workoutExercise.Id,
                SetNumber = model.SetNumber,
                Repetitions = model.Repetitions,
                Duration = model.Duration,
                Progression = model.Progression,
                Notes = model.Notes
            };

            this._context.WorkoutSets.Add(set);
            this._context.SaveChanges();
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            ModelState.AddModelError(string.Empty, "An error occurred while adding the set to the workout. Please try again.");

            return this.View(model);
        }

        return this.RedirectToAction("AddSets", new { workoutId = model.WorkoutId });
    }

    private List<SelectListItem> FetchAvailableExercises()
    {
        return this._context
            .Exercises
            .AsNoTracking()
            .Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            })
            .ToList();
    }

    private List<SelectListItem> FetchUsers() 
    {
        return this._context
            .Users
            .AsNoTracking()
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Username
            })
            .ToList();
    }
}
