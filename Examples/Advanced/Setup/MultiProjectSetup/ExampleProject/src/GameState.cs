namespace GdUnit4.Examples.Advanced.Setup.MultiProjectSetup;

using Godot;

/// <summary>
///     Global game state, registered as autoload 'GameState' in the game project.godot.
/// </summary>
public partial class GameState : Node
{
    public int Score { get; private set; }

    public void AddScore(int value) => Score += value;

    public void Reset() => Score = 0;
}
