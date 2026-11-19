namespace GdUnit4.Examples.Basics.Setup;

// This 'using static' directive allows us to call AssertThat() directly
// instead of writing Assertions.AssertThat(), making test code cleaner
using static Assertions;

/// <summary>
///     A simple example test class demonstrating the basic structure of gdUnit4Net tests.
///     This class serves as an introduction to gdUnit4Net testing framework, showing:
///     - How to mark a class as a test suite using [TestSuite]
///     - How to write basic test methods using [TestCase]
///     - How to use basic assertions with AssertThat()
///     - How to use 'using static' to simplify assertion syntax
///     Note: The 'using static GdUnit4.Assertions;' directive allows us to call
///     AssertThat() directly instead of writing Assertions.AssertThat(). This makes
///     test code cleaner and more readable.
///     This is the minimal example to get started with gdUnit4Net testing.
/// </summary>
[TestSuite]
public class GdUnitTest
{
    /// <summary>
    ///     A basic test demonstrating string equality assertion.
    ///     This test verifies that two identical strings are equal using gdUnit4Net's
    ///     fluent assertion syntax. It's the simplest possible test to verify that
    ///     your testing setup is working correctly.
    ///     Expected result: The test should pass since both strings are identical.
    /// </summary>
    [TestCase]
    public void IsMessageEqual()
        => AssertThat("This is a test").IsEqual("This is a test");
}
