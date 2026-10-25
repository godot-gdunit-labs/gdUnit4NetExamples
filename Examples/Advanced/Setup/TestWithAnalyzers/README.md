# Test With Analyzers

An advanced example demonstrating how to use gdUnit4Net analyzers to catch common testing mistakes at compile time and improve code quality.

## What This Example Shows

This project demonstrates:

- **Compile-time error detection** using gdUnit4.analyzers
- **Static analysis** to prevent runtime test failures
- **Best practice enforcement** with automatic code validation
- **IDE integration** with real-time feedback and quick fixes
- **Developer productivity improvements** through early error detection

## Why Use gdUnit4.analyzers ?

### Advantages of Compile-Time Analysis

- **Catch Errors Early**: Detect mistakes during compilation, not at runtime
- **Clear Error Messages**: Get specific, actionable feedback about what's wrong
- **IDE Integration**: Real-time warnings and suggestions as you type
- **Enforce Best Practices**: Automatically ensure proper gdUnit4Net patterns
- **Reduce Debugging Time**: Prevent common mistakes before they cause test failures
- **Team Consistency**: Enforce coding standards across all developers

### When to Use

- ✅ Teams wanting to enforce gdUnit4Net best practices automatically
- ✅ Projects requiring high test reliability and quality
- ✅ Developers who want immediate feedback on testing mistakes
- ✅ Codebases with many contributors to maintain consistency
- ✅ CI/CD pipelines that should fail fast on test configuration errors

## Files Overview

- **`ExampleTestSuite.cs`** - Test examples showing correct and incorrect patterns
- **`TestWithAnalyzers.csproj`** - Project configured with gdUnit4.analyzers package
- **Analyzer documentation** - [Detailed analyzer rules and configuration](https://github.com/MikeSchulze/gdUnit4Net/blob/master/Analyzers/documentation/index.md)

## Key Analyzer Features

### 1. Missing [RequireGodotRuntime] Detection

```csharp
// ❌ ANALYZER ERROR: Missing attribute when using Godot types
[TestCase]
public void IsNodeNotNullInvalidTest()  // GdUnit0501 will flag this!
{
    var node = AutoFree(new Node());    // Uses Godot type without [RequireGodotRuntime]
    AssertThat(node).IsNotNull();
}

// ✅ CORRECT: Proper attribute usage
[TestCase]
[RequireGodotRuntime]
public void IsNodeNotNull()             // Analyzer approves this pattern
{
    var node = AutoFree(new Node());
    AssertThat(node).IsNotNull();
}
```

### 2. DataPoint and TestCase Validation

```csharp
// ❌ ANALYZER ERROR: Invalid attribute combination
[TestCase]
[TestCase]  // GdUnit0201: Multiple TestCase attributes not allowed with DataPoint
[DataPoint]
public void InvalidCombination() { }

// ✅ CORRECT: Proper attribute usage
[TestCase]
public void ValidTest() { }
```

### 3. Pure C# Test Validation

```csharp
// ✅ CORRECT: Pure C# test doesn't need special attributes
[TestCase]
public void TestPureCSharpObject()      // No Godot types = no warnings
{
    var player = new { Name = "Hero", Level = 10 };
    AssertThat(player.Name).IsEqual("Hero");
}
```

## Analyzer Rules

### Core Rules

| Rule ID | Description | Severity |
| --- | --- | --- |
| **GdUnit0201** | Multiple TestCase attributes not allowed with DataPoint | Error |
| **GdUnit0500** | Godot Runtime Required for Test Class | Error |
| **GdUnit0501** | Godot Runtime Required for Test Method | Error |

### What Gets Analyzed

- **Godot Type Detection**: Node, Vector3, PackedScene, Resource, etc.
- **Attribute Validation**: [RequireGodotRuntime], [TestCase], [TestSuite], [DataPoint]
- **Attribute Combinations**: Prevents invalid combinations like multiple [TestCase] with [DataPoint]
- **Runtime Requirements**: Ensures Godot runtime is available when needed

## IDE Integration

### Visual Studio

- **Real-time Analysis**: Squiggly underlines appear as you type
- **Error List Integration**: All analyzer messages appear in Error List
- **Quick Fixes**: Right-click → Quick Actions and Refactorings
- **Build Integration**: Errors prevent successful compilation

### JetBrains Rider

- **Inline Diagnostics**: Immediate feedback with detailed explanations
- **Solution-wide Analysis**: Check entire solution for issues
- **Inspection Results**: Dedicated tool window for all analyzer findings
- **Context Actions**: Alt+Enter for quick fixes

### VS Code

- **OmniSharp Integration**: Real-time diagnostics via C# extension
- **Problems Panel**: All analyzer messages grouped by severity
- **Code Actions**: Lightbulb icon for available fixes
- **Terminal Output**: Build-time analyzer results

## Configuration Options

### Enable/Disable Specific Rules

Create `.editorconfig` in your project root:

```ini
[*.cs]
# Promote errors to warnings for stricter enforcement
dotnet_diagnostic.GdUnit0501.severity = warning

# Disable specific rules if needed
dotnet_diagnostic.GdUnit0201.severity = none
```

### Project-Level Configuration

Add to your `.csproj`:

```xml
<PropertyGroup>
  <!-- Treat all analyzer warnings as errors -->
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  
  <!-- Or be specific about analyzer warnings -->
  <WarningsAsErrors>GdUnit0501;GdUnit0500</WarningsAsErrors>
</PropertyGroup>
```

### Ruleset Files

For complex projects, use `.ruleset` files:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RuleSet Name="gdUnit4Net Rules" ToolsVersion="16.0">
  <Rules AnalyzerId="gdUnit4.analyzers" RuleNamespace="gdUnit4.analyzers">
    <Rule Id="GdUnit0501" Action="Error" />
    <Rule Id="GdUnit0500" Action="Warning" />
    <Rule Id="GdUnit0201" Action="Error" />
  </Rules>
</RuleSet>
```

## Common Scenarios

### Scenario 1: New Developer Onboarding

**Without Analyzers:**

```csharp
[TestCase]  // Missing [RequireGodotRuntime]
public void TestNode() 
{
    var node = new Node();  // Runtime error: Godot not initialized!
    AssertThat(node).IsNotNull();
}
```

Result: *Confusing runtime errors, debugging sessions, frustrated developers*

**With Analyzers:**

```cmd
Error GdUnit0501: Test method 'TestNode' uses Godot types but is missing [RequireGodotRuntime] attribute
```

Result: *Clear compile-time error with exact fix instructions*

### Scenario 2: Refactoring Existing Tests

When converting pure C# tests to use Godot types:

- Analyzer immediately flags missing attributes
- Provides suggestions for proper patterns
- Prevents accidental runtime failures

### Scenario 3: CI/CD Integration

```bash
# Build fails fast with clear analyzer errors
dotnet build
# Error: GdUnit0501: Missing [RequireGodotRuntime] in TestClass.TestMethod
# Build FAILED - 1 Error(s), 0 Warning(s)
```

## Performance Impact

### Compile Time

- **Minimal Overhead**: Analyzers add ~5-10% to build time
- **Incremental Analysis**: Only changed files are re-analyzed
- **Parallel Execution**: Runs alongside normal compilation

### Runtime

- **No Impact**: Analyzers only run during compilation
- **Better Performance**: Prevents runtime errors that slow down test execution

## Troubleshooting

**Analyzers not running?**

- Verify `gdUnit4.analyzers` package is installed
- Check IDE extensions are up to date
- Restart IDE after adding analyzer package

**False positives?**

- Review analyzer rules documentation
- Configure specific rules in `.editorconfig`
- Report issues to gdUnit4Net repository

**Missing errors in IDE but showing in build?**

- Clear Visual Studio ComponentModelCache
- Reload project files in IDE
- Check OmniSharp restart in VS Code

## Comparison with Basic Setup

| Aspect | Without Analyzers | With Analyzers |
| --- | --- | --- |
| **Error Detection** | Runtime only | Compile-time + Runtime |
| **Error Messages** | Generic exceptions | Specific, actionable |
| **Developer Experience** | Debug → Fix → Retry | Fix → Success |
| **Team Consistency** | Manual code reviews | Automated enforcement |
| **Learning Curve** | Trial and error | Guided best practices |
| **CI/CD Reliability** | May pass with broken tests | Fails fast on mistakes |

## Next Steps

After mastering analyzers:

- Explore advanced .editorconfig rules
- Set up custom analyzer configurations per environment
- Integrate with code quality tools (SonarQube, etc.)
- Check out the [Advanced Examples](../../) for complex testing scenarios
- Learn about [Custom Test Attributes](https://github.com/MikeSchulze/gdUnit4Net/blob/master/api/src/attributes/README.md)
