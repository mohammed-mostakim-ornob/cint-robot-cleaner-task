using System;
using System.Collections.Generic;
using AutoFixture;
using Cint.RobotCleaner.Domains.Robot.Models;
using Cint.RobotCleaner.Domains.Robot.Services.Implementations;
using FluentAssertions;
using Xunit;

namespace Cint.RobotCleaner.Tests;

public class RobotServiceTests
{
    private readonly Fixture _fixture;
    private readonly RobotService _sut;

    public RobotServiceTests()
    {
        _fixture = new Fixture();
        _sut = new RobotService();
    }

    [Fact]
    public void SetStaringPosition_Sets_StaringPosition()
    {
        // setup
        var testData = _fixture.Create<Position>();

        // action
        _sut.SetStaringPosition(testData);

        // assert
        _sut.StartingPosition.Should().NotBeNull();
        _sut.StartingPosition.Should().Be(testData);
    }

    [Fact]
    public void SetCommands_Sets_Command()
    {
        // setup
        var testData = new[] { _fixture.Create<Command>(), _fixture.Create<Command>() };
        
        // action
        _sut.SetCommands(testData);

        // assert
        _sut.Commands!.Length.Should().Be(testData.Length);
        for (var i = 0; i < testData.Length; i++)
        {
            _sut.Commands[i].Should().Be(testData[i]);
        }
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Clean_Cleans_BasedOnStartingPositionAndCommands(Position startingPosition, Command[] commands, int areasCleared)
    {
        // setup
        _sut.SetStaringPosition(startingPosition)
            .SetCommands(commands);

        // action
        var act = () => _sut.Clean();

        // assert
        act.Should().NotThrow<InvalidOperationException>();
        _sut.CleanedPlacesCount.Should().Be(areasCleared);
    }

    [Fact]
    public void Clean_Throws_InvalidOperationException_For_NullStartingPosition()
    {
        // setup
        var testData = new[] { _fixture.Create<Command>(), _fixture.Create<Command>() };
        _sut.SetCommands(testData);

        // action
        var act = () => _sut.Clean();

        // assert
        _sut.StartingPosition.Should().BeNull();
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Clean_Throws_InvalidOperationException_For_NullCommands()
    {
        // setup
        var testData = _fixture.Create<Position>();
        _sut.SetStaringPosition(testData);

        // action
        var act = () => _sut.Clean();

        // assert
        _sut.Commands.Should().BeNull();
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Clean_Throws_InvalidOperationException_For_NullStartingPositionAndNullCommands()
    {
        // action
        var act = () => _sut.Clean();
        
        // assert
        _sut.Commands.Should().BeNull();
        _sut.StartingPosition.Should().BeNull();
        act.Should().Throw<InvalidOperationException>();
    }
    
    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[]
        {
            new Position(1000000, -1000000),
            Array.Empty<Command>(),
            1
        };
        
        yield return new object[] {
            new Position(0, 0),
            new[]
            {
                new Command(Direction.E, 5),
                new Command(Direction.S, 10)
            },
            16
        };

        yield return new object[]
        {
            new Position(10000, -1000),
            new[]
            {
                new Command(Direction.E, 100),
                new Command(Direction.N, 400),
                new Command(Direction.S, 40),
                new Command(Direction.E, 10)
            },
            511
        };
    }
}