namespace GdUnit4.Examples.Advanced.Setup.MultiProjectSetup.Test;

using Godot;

using static Assertions;

/// <summary>
///     Runs the game's player scene, which uses game resources, input actions, a custom project setting and an autoload.
/// </summary>
[TestSuite]
[RequireGodotRuntime]
public class PlayerSceneTest
{
    private static GameState GameState => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<GameState>("GameState");

    [BeforeTest]
    public void ResetGameState() => GameState.Reset();

    [TestCase]
    public void LoadGameSceneByUid()
        => AssertObject(ISceneRunner.Load("uid://mpplayer001", true).Scene()).IsInstanceOf<Player>();

    [TestCase]
    public void GameSceneUsesGameResource()
    {
        var player = ISceneRunner.Load<Player>("res://src/Player.tscn", true).Scene();

        AssertThat(player.Stats.Speed).IsEqual(200f);
    }

    [TestCase]
    public void GameSceneReadsProjectSetting()
    {
        var player = ISceneRunner.Load<Player>("res://src/Player.tscn", true).Scene();

        AssertThat(player.Lives).IsEqual(3);
    }

    [TestCase]
    public async Task GameSceneReactsOnInputAction()
    {
        var runner = ISceneRunner.Load<Player>("res://src/Player.tscn", true);

        runner.SimulateActionPressed("jump");
        await runner.AwaitInputProcessed();

        AssertThat(runner.Scene().JumpCount).IsEqual(1);
        AssertThat(GameState.Score).IsEqual(10);
    }

    [TestCase]
    public async Task GameSceneReactsOnKeyMappedByGameInputMap()
    {
        var runner = ISceneRunner.Load<Player>("res://src/Player.tscn", true);

        runner.SimulateKeyPressed(Key.Space);
        await runner.AwaitInputProcessed();

        AssertThat(runner.Scene().JumpCount).IsEqual(1);
    }

    [TestCase]
    public async Task GameSceneMovesOnHeldAction()
    {
        var runner = ISceneRunner.Load<Player>("res://src/Player.tscn", true);
        var player = runner.Scene();
        var start = player.Position.X;

        runner.SimulateActionPress("move_right");
        await runner.SimulateFrames(10);
        runner.SimulateActionRelease("move_right");

        AssertThat(player.Position.X).IsGreater(start);
    }
}
