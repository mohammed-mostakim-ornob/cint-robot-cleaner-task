using Cint.RobotCleaner.Domains.Robot.Models;
using Cint.RobotCleaner.Domains.Robot.Services.Implementations;
using Cint.RobotCleaner.Domains.Robot.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string reportTextPrefix = "=> Cleaned: ";

var serviceProvider = ConfigureServices();
var robotService = serviceProvider.GetRequiredService<IRobotService>();

var commandCount = ReadCommandCount();
var startingPosition = ReadStartingPosition();
var commands = ReadCommands(commandCount);

robotService
    .SetStaringPosition(startingPosition)
    .SetCommands(commands)
    .Clean();

WriteReport(robotService.CleanedPlacesCount);

int ReadCommandCount()
{
    return Convert.ToInt32(Console.ReadLine());
}

Position ReadStartingPosition()
{
    var startingPositionsStr = Console.ReadLine()!.Split(" ");

    return new Position(Convert.ToInt32(startingPositionsStr[0]), Convert.ToInt32(startingPositionsStr[1]));
}

Command[] ReadCommands(int count)
{
    var commandsArr = new Command[count];

    for (var i = 0; i < count; i++)
    {
        var commandData = Console.ReadLine()!.Split(" ");

        commandsArr[i] = new Command((Direction) Enum.Parse(typeof(Direction), commandData[0]), Convert.ToInt32(commandData[1]));
    }

    return commandsArr;
}

void WriteReport(int placesClearedCount)
{
    Console.WriteLine($"{reportTextPrefix}{placesClearedCount}");
}

IServiceProvider ConfigureServices()
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
            services.AddTransient<IRobotService, RobotService>())
        .Build()
        .Services.CreateScope()
        .ServiceProvider;
}