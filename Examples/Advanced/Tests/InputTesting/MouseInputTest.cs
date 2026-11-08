namespace GdUnit4.Examples.Advanced.Tests.InputTesting;

using Godot;

using Resources;

using static Assertions;

/// <summary>
///     Demonstrates comprehensive mouse input testing patterns based on GdUnit4Net capabilities.
///     This test suite covers essential mouse input mechanics including:
///     • Mouse positioning and movement simulation
///     • Multiple mouse button handling (left, right, middle)
///     • Double-click detection and processing
///     • Mouse button press/hold/release sequences
///     • Mouse position tracking and validation
///     • Modifier keys with mouse interactions
///     Tests validate input processing by checking both Godot's Input singleton and scene properties.
/// </summary>
[TestSuite]
[RequireGodotRuntime]
public class MouseInputTest
{
    private ISceneRunner runner = null!;
    private TestSceneWithButton scene = null!;

    /// <summary>
    ///     Suite-level setup: loads the test scene once before all test methods execute.
    /// </summary>
    [Before]
    public void Before()
    {
        runner = ISceneRunner.Load("res://Resources/TestSceneWithButton.tscn", true);
        scene = runner.Scene() as TestSceneWithButton
                ?? throw new InvalidOperationException("Could not cast scene");

        // Maximize view to ensure mouse events are properly handled
        runner.MaximizeView();
    }

    /// <summary>
    ///     Tests mouse positioning with SetMousePosition method.
    ///     Validates that mouse position is correctly set and tracked.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task MousePositioning()
    {
        var targetPos = new Vector2(150, 100);

        runner.SetMousePosition(targetPos);
        await runner.AwaitInputProcessed();

        // Validate through scene property and Godot's viewport
        AssertThat(scene.LastMousePosition).IsEqual(targetPos);
        AssertThat(runner.Scene()?.GetViewport().GetMousePosition()).IsEqual(targetPos);
    }

    /// <summary>
    ///     Tests left mouse button press and release sequence.
    ///     Validates button state tracking through Input singleton.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task LeftMouseButtonPressRelease()
    {
        var clickPos = new Vector2(200, 150);
        runner.SetMousePosition(clickPos);

        // Press and hold
        runner.SimulateMouseButtonPress(MouseButton.Left);
        await runner.AwaitInputProcessed();

        AssertThat(scene.LastMouseButton).IsEqual(MouseButton.Left);
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsTrue();
        AssertThat(Input.GetMouseButtonMask()).IsEqual(MouseButtonMask.Left);

        // Release
        runner.SimulateMouseButtonRelease(MouseButton.Left);
        await runner.AwaitInputProcessed();

        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsFalse();
        AssertThat(Input.GetMouseButtonMask()).IsEqual((MouseButtonMask)0L);
    }

    /// <summary>
    ///     Tests complete mouse button click (press + immediate release).
    ///     Uses SimulateMouseButtonPressed which handles both press and release.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task MouseButtonClick()
    {
        var clickPos = new Vector2(100, 100);
        runner.SetMousePosition(clickPos);
        runner.SimulateMouseButtonPressed(MouseButton.Left);
        await runner.AwaitInputProcessed();

        // After SimulateMouseButtonPressed, button should be released
        AssertThat(scene.LastMouseButton).IsEqual(MouseButton.Left);
        AssertThat(scene.LastMousePosition).IsEqual(clickPos);
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsFalse();
    }

    /// <summary>
    ///     Tests right mouse button functionality.
    ///     Validates that different mouse buttons are properly tracked.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task RightMouseButton()
    {
        runner.SetMousePosition(new Vector2(300, 200));
        runner.SimulateMouseButtonPressed(MouseButton.Right);
        await runner.AwaitInputProcessed();

        AssertThat(scene.LastMouseButton).IsEqual(MouseButton.Right);
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Right)).IsFalse();
    }

    /// <summary>
    ///     Tests double-click functionality.
    ///     Validates that double-click events are properly processed.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task DoubleClickDetection()
    {
        var clickPos = new Vector2(150, 150);
        runner.SetMousePosition(clickPos);

        // Simulate double-click
        runner.SimulateMouseButtonPressed(MouseButton.Left, true);
        await runner.AwaitInputProcessed();

        // Double-click should still result in button being released after processing
        AssertThat(scene.LastMouseButton).IsEqual(MouseButton.Left);
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsFalse();
    }

    /// <summary>
    ///     Tests multiple mouse buttons pressed simultaneously.
    ///     Validates button mask handling for multiple pressed buttons.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task MultipleMouseButtons()
    {
        runner.SetMousePosition(new Vector2(250, 250));

        // Press left button and hold
        runner.SimulateMouseButtonPress(MouseButton.Left);
        await runner.AwaitInputProcessed();
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsTrue();

        // Press right button while left is held
        runner.SimulateMouseButtonPress(MouseButton.Right);
        await runner.AwaitInputProcessed();

        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsTrue();
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Right)).IsTrue();
        AssertThat(Input.GetMouseButtonMask()).IsEqual(MouseButtonMask.Left | MouseButtonMask.Right);

        // Release right button
        runner.SimulateMouseButtonRelease(MouseButton.Right);
        await runner.AwaitInputProcessed();

        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsTrue();
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Right)).IsFalse();
        AssertThat(Input.GetMouseButtonMask()).IsEqual(MouseButtonMask.Left);
    }

    /// <summary>
    ///     Tests mouse movement simulation.
    ///     Validates that mouse movement is properly tracked.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task MouseMovement()
    {
        var startPos = new Vector2(50, 50);
        var endPos = new Vector2(400, 300);

        runner.SetMousePosition(startPos);
        runner.SimulateMouseMove(endPos);
        await runner.AwaitInputProcessed();

        AssertThat(scene.LastMousePosition).IsEqual(endPos);
    }

    /// <summary>
    ///     Tests mouse with modifier keys (Ctrl + click).
    ///     Validates that modifier keys work correctly with mouse input.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task MouseWithModifiers()
    {
        var clickPos = new Vector2(175, 175);

        // Click while Ctrl is held
        runner.SimulateKeyPress(Key.Ctrl);
        runner.SetMousePosition(clickPos);
        runner.SimulateMouseButtonPressed(MouseButton.Left);
        await runner.AwaitInputProcessed();

        // Both Ctrl and mouse click should be processed
        AssertThat(scene.LastMouseButton).IsEqual(MouseButton.Left);
        AssertThat(scene.LastKeyPressed).IsEqual(Key.Ctrl);
        AssertThat(Input.IsKeyPressed(Key.Ctrl)).IsTrue(); // Key ctrl is still pressing
        AssertThat(Input.IsMouseButtonPressed(MouseButton.Left)).IsFalse(); // Released after click
    }

    /// <summary>
    ///     Tests hover detection on UI elements.
    ///     Validates that mouse position affects UI element hover states.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task UiElementHover()
    {
        var button = runner.FindChild("ExampleButton") as Button
                     ?? throw new InvalidOperationException("Could not find ExampleButton");

        var outsidePos = button.Position - new Vector2(50, 50);
        var insidePos = button.Position + (button.Size / 2);

        // Move mouse outside button
        runner.SetMousePosition(outsidePos);
        await runner.AwaitInputProcessed();
        AssertThat(button.IsHovered()).IsFalse();

        // Move mouse inside button
        runner.SetMousePosition(insidePos);
        await runner.AwaitInputProcessed();
        AssertThat(button.IsHovered()).IsTrue();
    }
}
