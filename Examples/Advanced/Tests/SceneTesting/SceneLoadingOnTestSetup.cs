namespace GdUnit4.Examples.Advanced.Tests.SceneTesting;

using static Assertions;

/// <summary>
///     Demonstrates advanced scene testing with setup and teardown lifecycle management.
///     This test class shows how to use gdUnit4Net's lifecycle attributes for efficient scene testing:
///     - [Before]/[After] for test suite-level setup and cleanup
///     - [BeforeTest]/[AfterTest] for individual test setup and cleanup
///     - Shared scene runner instance across multiple test methods
///     - Proper scene lifecycle management and validation
///     Key concepts demonstrated:
///     - Test lifecycle attributes and their execution order
///     - Shared resource management across test methods
///     - Scene state validation at different lifecycle stages
///     - Efficient scene reuse for multiple test cases
///     Lifecycle execution order:
///     1. [Before] - Once per test suite
///     2. [BeforeTest] - Before each test method
///     3. [TestCase] - Individual test method
///     4. [AfterTest] - After each test method
///     5. [After] - Once per test suite (after all tests)
///     This pattern is ideal for testing scenarios where:
///     - Multiple tests need the same scene setup
///     - Scene loading is expensive and should be reused
///     - You need to verify scene state consistency across tests.
/// </summary>
[TestSuite]
[RequireGodotRuntime]
public class SceneLoadingOnTestSetup
{
    // ReSharper disable once NullableWarningSuppressionIsUsed
    private ISceneRunner runner = null!;

    /// <summary>
    ///     Suite-level setup method executed once before all tests in this suite.
    ///     Verifies that the shared runner instance starts in a clean state.
    ///     This ensures test isolation and prevents interference between test runs.
    ///     Expected state:
    ///     - Runner should be null (clean initial state)
    ///     - No previous test execution artifacts.
    /// </summary>
    [Before]
    public void Setup() => AssertThat(runner).IsNull();

    /// <summary>
    ///     Suite-level teardown method executed once after all tests complete.
    ///     Validates that the scene has been properly cleaned up after test execution.
    ///     This verifies that the autoFree mechanism worked correctly and no
    ///     memory leaks or hanging references remain.
    ///     Expected state:
    ///     - The runner has been disposed by autoFree
    ///     - Accessing the scene of a disposed runner throws an ObjectDisposedException.
    /// </summary>
    [After]
    public void Teardown()
        => AssertThrown(() => runner.Scene())
            .IsInstanceOf<ObjectDisposedException>();

    /// <summary>
    ///     Per-test setup method executed before each individual test case.
    ///     Loads the test scene and prepares it for testing. This ensures each
    ///     test starts with a fresh, properly initialized scene instance.
    ///     The scene is loaded with autoFree=true for automatic cleanup,
    ///     eliminating the need for manual resource management in tests.
    ///     Setup actions:
    ///     - Load scene from resource path
    ///     - Verify successful scene loading
    ///     - Prepare runner for test execution.
    /// </summary>
    [BeforeTest]
    public void TestSetup()
    {
        // Example to load a scene by resource path, set autoFree to true
        runner = ISceneRunner.Load("res://Resources/TestSceneWithButton.tscn", true);
        AssertThat(runner).IsNotNull();
    }

    /// <summary>
    ///     Per-test cleanup method executed after each individual test case.
    ///     Validates that the runner instance remains valid after test execution.
    ///     This ensures that the test didn't inadvertently corrupt or destroy
    ///     the runner, which could affect subsequent lifecycle operations.
    ///     Expected state:
    ///     - Runner should still exist (not null)
    ///     - Runner should be in a consistent state
    ///     - Ready for cleanup by [After] method.
    /// </summary>
    [AfterTest]
    public void TestCleanup() => AssertThat(runner).IsNotNull();

    /// <summary>
    ///     Tests basic scene accessibility and state.
    ///     Verifies that the scene loaded in [Before] is properly accessible
    ///     and integrated into the scene tree for testing.
    ///     This test validates:
    ///     - Scene instance is available
    ///     - Scene is properly integrated (IsInsideTree)
    ///     - Runner maintains valid state.
    /// </summary>
    [TestCase]
    public void TestCaseA()
        => AssertThat(runner.Scene())
            .IsNotNull();

    /// <summary>
    ///     Tests basic scene accessibility and state.
    ///     Verifies that the scene loaded in [Before] is properly accessible
    ///     and integrated into the scene tree for testing.
    ///     This test validates:
    ///     - Scene instance is available
    ///     - Scene is properly integrated (IsInsideTree)
    ///     - Runner maintains valid state.
    /// </summary>
    [TestCase]
    public void TestCaseB()
        => AssertThat(runner.Scene())
            .IsNotNull();
}
