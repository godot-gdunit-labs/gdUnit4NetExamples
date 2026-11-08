namespace GdUnit4.Examples.Advanced.Setup.MultiProjectSetup;

using Godot;

/// <summary>
///     Custom resource holding the player configuration, stored as 'res://src/player_stats.tres'.
/// </summary>
[GlobalClass]
public partial class PlayerStats : Resource
{
    [Export]
    public float Speed { get; set; } = 100f;

    [Export]
    public float JumpVelocity { get; set; } = 300f;
}
