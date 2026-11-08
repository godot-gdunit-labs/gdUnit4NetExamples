namespace GdUnit4.Examples.Advanced.Tests.SignalTesting;

using Godot;

using Resources;

using static Assertions;

/// <summary>
///     Demonstrates comprehensive signal testing patterns for scene communication.
///     This test suite focuses on signal emission and monitoring including:
///     • Basic signal emission validation with timeouts
///     • Signal chaining and workflow testing
///     • Signal emission counting and frequency
///     • Asynchronous signal processing patterns
///     • Signal parameter validation (if applicable)
///     • Multiple signal monitoring scenarios
///     Tests focus purely on signal behavior and communication patterns.
/// </summary>
[TestSuite]
[RequireGodotRuntime]
public class SignalTest
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
    }

    /// <summary>
    ///     Tests basic signal emission through direct method call.
    ///     Validates that signals are emitted immediately when triggered programmatically.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task BasicSignalEmission()
    {
        var monitor = AssertSignal(scene).StartMonitoring();

        // Trigger signal emission directly (not through input)
        scene.OnButtonPressed();

        await monitor.IsEmitted(TestSceneWithButton.SignalName.GameStarted).WithTimeout(50);
    }

    /// <summary>
    ///     Tests signal chaining workflow from GameStarted to GameStopped.
    ///     Validates that one signal automatically triggers another through signal connections.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task SignalChaining()
    {
        var monitor = AssertSignal(scene).StartMonitoring();

        // Trigger initial signal - should automatically chain to GameStopped after timer
        scene.OnButtonPressed();

        // Wait for the chained signal after async processing (100ms timer + margin)
        await monitor.IsEmitted(TestSceneWithButton.SignalName.GameStopped).WithTimeout(200);
    }

    /// <summary>
    ///     Tests signal emission timing and sequence.
    ///     Validates that signals are emitted in the expected order with proper timing.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task SignalSequenceTiming()
    {
        var monitor = AssertSignal(scene).StartMonitoring();

        // Trigger workflow
        scene.OnButtonPressed();

        // GameStarted should emit first (immediately)
        await monitor.IsEmitted(TestSceneWithButton.SignalName.GameStarted).WithTimeout(50);

        // GameStopped should emit after the timer delay (100ms + margin)
        await monitor.IsEmitted(TestSceneWithButton.SignalName.GameStopped).WithTimeout(150);
    }

    /// <summary>
    ///     Tests signal emission counting and multiple triggers.
    ///     Validates signal behavior when triggered multiple times rapidly.
    /// </summary>
    [TestCase]
    [IgnoreUntil(Description = "We skip this test unit 'IsCountEmitted' is fixed")]
    public void MultipleSignalEmissions()
    {
        var monitor = AssertSignal(scene).StartMonitoring();

        // Trigger signal multiple times rapidly
        scene.OnButtonPressed();
        scene.OnButtonPressed();
        scene.OnButtonPressed();

        // Verify signal was emitted expected number of times
        monitor.IsCountEmitted(3, TestSceneWithButton.SignalName.GameStarted);
    }

    /// <summary>
    ///     Tests signal emission through keyboard input trigger.
    ///     Validates that input events can trigger signal emissions as expected.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task SignalThroughKeyboardInput()
    {
        var monitor = AssertSignal(scene).StartMonitoring();

        // Trigger signal through Space key (as defined in scene's _Input method)
        runner.SimulateKeyPressed(Key.Space);

        await monitor.IsEmitted(TestSceneWithButton.SignalName.GameStarted).WithTimeout(50);
    }

    /// <summary>
    ///     Tests signal emission through mouse button interaction on UI elements.
    ///     Validates that UI button press events correctly emit their expected signals.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous test operation.
    /// </returns>
    [TestCase]
    public async Task MouseButtonPressedSignalIsEmitted()
    {
        // Locate the target button within the pre-loaded scene
        var button = runner.FindChild("ExampleButton") as Button
                     ?? throw new InvalidOperationException("Could not find ExampleButton in scene");

        // Verify successful button discovery and type casting
        AssertThat(button).IsNotNull();

        // Initialize signal monitoring before interaction simulation
        var monitor = AssertSignal(button).StartMonitoring();

        // Simulate realistic user mouse interaction
        runner
            .SetMousePosition(button.Position + new Vector2(5, 5)) // Position mouse inside button bounds
            .SimulateMouseButtonPressed(MouseButton.Left); // Generate left mouse button press event

        // Assert signal emission within reasonable timeout window
        await monitor.IsEmitted(BaseButton.SignalName.Pressed).WithTimeout(50);
    }
}
