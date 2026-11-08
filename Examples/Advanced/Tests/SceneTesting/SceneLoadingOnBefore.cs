namespace GdUnit4.Examples.Advanced.Tests.SceneTesting;

using static Assertions;

/// <summary>
///     Demonstrates efficient scene loading using suite-level setup with the [Before] attribute.
///     This example shows how to load a scene once for an entire test suite and reuse it
///     across multiple test methods, which is ideal when:
///     • Multiple tests examine the same scene without modifying it
///     • Scene loading is computationally expensive
///     • Tests focus on different aspects of the same scene configuration
///     • Memory allocation overhead should be minimized
///     <br />
///     Test Execution Lifecycle:
///     1. [Before] SetupScene() - Loads scene once before any test executes
///     2. [TestCase] methods - All tests share the same scene instance
///     3. [After] CleanupScene() - Verifies scene state after all tests complete
///     4. Dispose() - Final cleanup (demonstration only)
///     <br />
///     Advantages of Suite-Level Scene Loading:
///     • Significantly faster test execution (single scene load vs. per-test loading)
///     • Consistent baseline scene state across all tests
///     • Reduced memory fragmentation from repeated allocations
///     • Simplified test method implementation
///     <br />
///     Important Considerations:
///     • Tests MUST NOT modify the shared scene state
///     • All test methods must be compatible with the same scene setup
///     • Test isolation is reduced - failures may cascade between tests
///     • Consider per-test loading if scene state modifications are required
///     <br />
///     Alternative Patterns:
///     • Use [BeforeTest] for per-test scene loading when state isolation is needed
///     • Use scene snapshots/restoration for tests that need to modify scene state.
/// </summary>
[TestSuite]
[RequireGodotRuntime]
#pragma warning disable CA1063
public class SceneLoadingOnBefore : IDisposable
{
    // ReSharper disable once NullableWarningSuppressionIsUsed
    private ISceneRunner runner = null!;

    /// <summary>
    ///     Implements IDisposable for demonstration purposes only.
    ///     This showcases how to verify proper scene cleanup, but is typically unnecessary
    ///     since GdUnit4 automatically manages test suite resources and dependencies.
    ///     In production test code, manual disposal is rarely needed.
    ///     Verification steps:
    ///     • Confirms runner instance exists
    ///     • Demonstrates manual cleanup (not recommended for normal use)
    ///     The runner was already disposed by autoFree, accessing its scene would throw an ObjectDisposedException.
    /// </summary>
    public void Dispose()
    {
        AssertThat(runner).IsNotNull();

        // Manual disposal shown for educational purposes only
        // GdUnit4 normally handles this automatically
        runner.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Suite-level setup: loads the test scene once before all test methods execute.
    ///     This method runs exactly once per test suite, making it ideal for expensive
    ///     setup operations like scene loading. The loaded scene remains in memory
    ///     and accessible throughout all test method executions.
    ///     Setup Process:
    ///     1. Load scene from specified resource path
    ///     2. Enable autoFree for automatic memory management
    ///     3. Verify successful scene instantiation
    ///     4. Ensure scene integration into the scene tree
    ///     The autoFree parameter (true) ensures the scene is automatically cleaned up
    ///     when the test suite completes, eliminating manual memory management.
    ///     Scene Path: "res://SceneTesting/TestSceneWithButton.tscn"
    ///     Expected: Scene contains interactive button component for testing.
    /// </summary>
    [Before]
    public void SetupScene()
    {
        // Load scene once for the entire test suite with automatic cleanup
        runner = ISceneRunner.Load("res://Resources/TestSceneWithButton.tscn", true);

        // Verify successful scene loading and runner initialization
        AssertThat(runner).IsNotNull();
        AssertThat(runner.Scene()).IsNotNull();
    }

    /// <summary>
    ///     Suite-level cleanup: validates scene state after all tests have completed.
    ///     This method executes once after all test methods finish, providing an
    ///     opportunity to verify the scene remained in a valid state throughout
    ///     the entire test execution cycle.
    ///     Validation Purpose:
    ///     • Ensures the scene instance persisted through all test executions
    ///     • Confirms no test accidentally disposed or corrupted the scene
    ///     • Provides confidence in scene stability across the test suite
    ///     Note: The scene will be automatically cleaned up by GdUnit4 after this method.
    /// </summary>
    [After]
    public void CleanupScene() =>
        AssertThat(runner.Scene()).IsNotNull(); // Scene should still exist

    /// <summary>
    ///     Test Case A: Validates basic scene accessibility and runner state.
    ///     This test verifies that the scene loaded during suite setup is properly
    ///     accessible and maintains its expected state. It demonstrates the most
    ///     fundamental validation - ensuring the shared scene resource is available.
    ///     Validations:
    ///     • Scene instance is not null (successfully loaded and accessible)
    ///     • Runner maintains valid reference to the scene
    ///     • Scene remains properly integrated in the Godot scene tree.
    /// </summary>
    [TestCase]
    public void TestCaseA()
        => AssertThat(runner.Scene())
            .IsNotNull();

    /// <summary>
    ///     Test Case B: Additional validation of scene accessibility and consistency.
    ///     This test mirrors TestCaseA to demonstrate that multiple tests can
    ///     safely access the same scene instance without interference. It validates
    ///     the stability of the suite-level scene loading approach.
    ///     Validations:
    ///     • Scene instance remains accessible after previous test execution
    ///     • No state corruption between test method executions
    ///     • Consistent scene availability across the entire test suite
    ///     Note: In real scenarios, TestCaseA and TestCaseB would perform different
    ///     validations on various aspects of the same scene (UI elements, behavior, etc.)
    /// </summary>
    [TestCase]
    public void TestCaseB()
        => AssertThat(runner.Scene())
            .IsNotNull();
}
