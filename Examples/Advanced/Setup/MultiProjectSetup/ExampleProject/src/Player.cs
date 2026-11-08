namespace GdUnit4.Examples.Advanced.Setup.MultiProjectSetup;

using Godot;

/// <summary>
///     The player uses the input actions, the custom project setting and the autoload defined in the game project.godot.
/// </summary>
public partial class Player : Node2D
{
    [Export]
    public PlayerStats Stats { get; set; } = new();

    public int JumpCount { get; private set; }

    public int Lives { get; private set; }

    public override void _Ready()
        => Lives = ProjectSettings.GetSetting("game/rules/start_lives", 1).AsInt32();

    public override void _Process(double delta)
    {
        var direction = Input.GetAxis("move_left", "move_right");
        Position += new Vector2(direction * Stats.Speed * (float)delta, 0);
    }

    public override void _Input(InputEvent @event)
    {
        if (!@event.IsActionPressed("jump"))
            return;

        JumpCount++;
        GetNode<GameState>("/root/GameState").AddScore(10);
    }
}
