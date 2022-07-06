using Cint.RobotCleaner.Domains.Robot.Models;

namespace Cint.RobotCleaner.Domains.Robot.Services.Interfaces;

public interface IRobotService
{
    Position? StartingPosition { get; }
    Command[]? Commands { get; }
    int CleanedPlacesCount { get; }
    IRobotService SetStaringPosition(Position position);
    IRobotService SetCommands(Command[] commands);
    void Clean();
}