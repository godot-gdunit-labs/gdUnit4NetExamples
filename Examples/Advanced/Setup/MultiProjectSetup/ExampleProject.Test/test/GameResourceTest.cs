namespace GdUnit4.Examples.Advanced.Setup.MultiProjectSetup.Test;

using Godot;

using static Assertions;

/// <summary>
///     Loads resources of the referenced game project by 'res://' and 'uid://' paths.
/// </summary>
[TestSuite]
[RequireGodotRuntime]
public class GameResourceTest
{
    [TestCase]
    public void LoadGameResourceByPath()
    {
        var stats = GD.Load<PlayerStats>("res://src/player_stats.tres");

        AssertThat(stats.Speed).IsEqual(200f);
        AssertThat(stats.JumpVelocity).IsEqual(450f);
    }

    [TestCase]
    public void LoadGameResourceByUid()
        => AssertThat(GD.Load<PlayerStats>("uid://mpstats001").Speed).IsEqual(200f);

    [TestCase]
    public void LoadImportedGameAsset()
        => AssertObject(GD.Load<Texture2D>("res://icon.svg")).IsNotNull();

    [TestCase]
    public void LoadGameSubScene()
    {
        var labelScene = AutoFree(GD.Load<PackedScene>("res://src/scenes/LabelScene.tscn").Instantiate())!;

        AssertThat(labelScene.GetNode<Label>("Label").Text).IsEqual("Hello World");
    }
}
