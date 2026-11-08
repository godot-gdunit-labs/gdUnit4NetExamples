namespace GdUnit4.Examples.Advanced.Setup.MultiProjectSetup.Test;

using Godot;

using static Assertions;

/// <summary>
///     The settings of the game project.godot are applied at test runtime.
/// </summary>
[TestSuite]
[RequireGodotRuntime]
public class ProjectSettingsTest
{
    [TestCase]
    public void InputActionsAreAvailable()
    {
        AssertThat(InputMap.HasAction("move_left")).IsTrue();
        AssertThat(InputMap.HasAction("move_right")).IsTrue();
        AssertThat(InputMap.HasAction("jump")).IsTrue();
    }

    [TestCase]
    public void InputActionEventsAreMapped()
    {
        var keys = InputMap.ActionGetEvents("jump").OfType<InputEventKey>().Select(e => e.PhysicalKeycode).ToList();

        AssertThat(keys).ContainsExactly(Key.Space);
    }

    [TestCase]
    public void CustomSettingIsAvailable()
        => AssertThat(ProjectSettings.GetSetting("game/rules/start_lives").AsInt32()).IsEqual(3);

    [TestCase]
    public void AutoloadIsAvailable()
    {
        var root = ((SceneTree)Engine.GetMainLoop()).Root;

        AssertObject(root.GetNodeOrNull("GameState")).IsInstanceOf<GameState>();
    }

    [TestCase]
    public void TestProjectIdentityIsKept()
    {
        // The test project stays the root project of the Godot runtime
        AssertThat(ProjectSettings.GetSetting("application/config/name").AsString()).IsEqual("ExampleProject.Test");
        AssertThat(ProjectSettings.GetSetting("dotnet/project/assembly_name").AsString()).IsEqual("ExampleProject.Test");
    }
}
