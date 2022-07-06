using Cint.RobotCleaner.Domains.Robot.Models;
using Cint.RobotCleaner.Domains.Robot.Services.Interfaces;

namespace Cint.RobotCleaner.Domains.Robot.Services.Implementations;

public class RobotService : IRobotService
{   
    private readonly List<string> _placesCovered;

    public RobotService()
    {
        _placesCovered = new List<string>();
    }

    public Position? StartingPosition { get; private set; }

    public Command[]? Commands { get; private set; }

    public int CleanedPlacesCount => _placesCovered.Distinct().Count();

    public IRobotService SetStaringPosition(Position position)
    {
        StartingPosition = position;

        return this;
    }

    public IRobotService SetCommands(Command[] commands)
    {
        Commands = commands;

        return this;
    }

    public void Clean()
    {
        ValidatesInputsForCleaning();
        ClearCoveredPlacesList();
        
        _placesCovered.Add($"{StartingPosition!.X},{StartingPosition.Y}");

        ExecuteCommands();
    }

    // Clears the "_placesCovered" list for starting a new cleaning task 
    private void ClearCoveredPlacesList()
    {
        _placesCovered.Clear();
    }

    // Validates inputs (e.g. starting position, commands) for cleaning
    private void ValidatesInputsForCleaning()
    {
        if (StartingPosition == null)
            throw new InvalidOperationException();

        if (Commands == null)
            throw new InvalidOperationException();
    }

    // Executes all the commands with the given direction and step count
    private void ExecuteCommands()
    {
        var currentPosition = StartingPosition;

        foreach (var command in Commands!)
        {
            for (var i = 1; i <= command.StepCount; i++)
            {
                currentPosition = TakeStep(command.Direction, currentPosition!);
                
                _placesCovered.Add($"{currentPosition.X},{currentPosition.Y}");
            }
        }
    }

    // Takes step in the direction and returns new position
    private static Position TakeStep(Direction direction, Position currentPosition)
    {
        return direction switch
        {
            Direction.N => TakeStepInNorth(currentPosition),
            Direction.S => TakeStepInSouth(currentPosition),
            Direction.E => TakeStepInEast(currentPosition),
            _ => TakeStepInWest(currentPosition)
        };
    }

    // Takes step in North and returns new position
    private static Position TakeStepInNorth(Position currentPosition)
    {
        return currentPosition with {Y = currentPosition.Y + 1};
    }
    
    // Takes step in South and returns new position
    private static Position TakeStepInSouth(Position currentPosition)
    {
        return currentPosition with {Y = currentPosition.Y -1};
    }
    
    // Takes step in East and returns new position
    private static Position TakeStepInEast(Position currentPosition)
    {
        return currentPosition with {X = currentPosition.X + 1};
    }
    
    // Takes step in West and returns new position
    private static Position TakeStepInWest(Position currentPosition)
    {
        return currentPosition with {X = currentPosition.X - 1};
    }
}