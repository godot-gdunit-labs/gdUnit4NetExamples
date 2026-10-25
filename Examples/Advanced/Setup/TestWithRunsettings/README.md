# Test With Runsettings Configuration

An advanced example demonstrating how to configure gdUnit4Net test execution using `.runsettings` files instead of environment variables.

## What This Example Shows

This project demonstrates:

- **Advanced test configuration** using `.runsettings` files
- **Alternative to environment variables** for GODOT_BIN configuration
- **Multiple output formats** (Console, HTML, TRX) for test results
- **CI/CD friendly setup** with centralized configuration
- **Test execution optimization** for Godot runtime requirements

## Why Use .runsettings?

### Advantages over Environment Variables

- **Version Control**: Configuration is committed with your code
- **Team Consistency**: Everyone uses the same settings
- **CI/CD Integration**: No need to configure environment variables in build systems
- **Multiple Configurations**: Easy to have different settings for different environments
- **IDE Integration**: Better support in Visual Studio, Rider, and VS Code

### When to Use

- ✅ Team projects where everyone needs consistent test configuration
- ✅ CI/CD pipelines that need reliable test execution
- ✅ Projects requiring specific test result formats
- ✅ Advanced logging and debugging scenarios

## Files Overview

- **`.runsettings`** - Comprehensive test configuration with detailed comments
- **`TestWithRunsettings.csproj`** - Test project configured for advanced scenarios
- **`project.godot`** - Godot project configuration

## Key Configuration Sections

### 1. RunConfiguration

```xml
<MaxCpuCount>1</MaxCpuCount>              <!-- Prevents Godot conflicts -->
<TestSessionTimeout>1800000</TestSessionTimeout>  <!-- 30min timeout for long test suites -->
<EnvironmentVariables>
    <GODOT_BIN>path/to/godot.exe</GODOT_BIN>      <!-- Alternative to system env var -->
</EnvironmentVariables>
```

### 2. LoggerRunSettings

```xml
<Logger friendlyName="html" enabled="True">       <!-- Rich HTML reports -->
<Logger friendlyName="trx" enabled="True">        <!-- CI/CD integration -->
```

### 3. GdUnit4 Specific

```xml
<Parameters></Parameters>                         <!-- Custom Godot startup args -->
<DisplayName>FullyQualifiedName</DisplayName>     <!-- Better test identification -->
```

For more detailed information please read the [GdUnit Test Adapter Documentaion](https://github.com/MikeSchulze/gdUnit4Net/tree/master/TestAdapter)

## Running Tests with .runsettings

### Command Line

```bash
dotnet test --settings .runsettings
```

### Visual Studio / Rider

1. Right-click project → "Configure Run Settings"
2. Select the `.runsettings` file in the project root
3. Run tests normally

### VS Code

Add to `.vscode/settings.json`:

```json
{
    "dotnet-test-explorer.runSettingsPath": ".runsettings"
}
```

## Configuration Customization

### For Different Environments

**Development (.runsettings.dev):**

```xml
<GODOT_BIN>C:\Godot\Godot.exe</GODOT_BIN>
<Verbosity>detailed</Verbosity>
```

**CI/CD (.runsettings.ci):**

```xml
<GODOT_BIN>/usr/local/bin/godot</GODOT_BIN>
<Parameters>--headless --disable-render-loop</Parameters>
```

**Usage:**

```bash
dotnet test --settings .runsettings.ci
```

### Common Godot Parameters

- `--headless` - Run without GUI (CI/CD)
- `--disable-render-loop` - Skip rendering (faster tests)
- `--verbose` - Detailed Godot logging
- `--debug` - Enable debug mode

## Test Results

After running tests, check the `./TestResults` directory for:

- **test-result.html** - Rich, interactive HTML report
- **test-result.trx** - Machine-readable format for CI/CD
- **Console output** - Real-time feedback during execution

## Comparison with Basic Setup

| Aspect | RequireGodotTestProjectSetup | TestWithRunsettings |
| --- | --- | --- |
| **Configuration** | Environment variables | .runsettings file |
| **Team Sharing** | Manual setup required | Automatic with code |
| **CI/CD Setup** | Environment configuration needed | Works out of the box |
| **Test Reports** | Basic console output | Multiple formats (HTML, TRX) |
| **Debugging** | Limited options | Advanced logging controls |
| **Godot Parameters** | Fixed | Configurable per environment |

## Troubleshooting

**Tests not using .runsettings?**

- Verify the file is in the project root
- Check IDE settings point to the correct file
- Use `--settings` parameter with `dotnet test`

**Long execution times?**

- Increase `TestSessionTimeout` for large test suites
- Consider using `--headless` parameter for faster execution

**Permission errors with GODOT_BIN path?**

- Ensure the path in .runsettings uses forward slashes or escaped backslashes
- Verify the Godot executable is accessible from the test environment

## Next Steps

After mastering .runsettings configuration:

- Explore environment-specific configurations
- Learn about integrating with CI/CD systems
- Check out the [Demos](../../../Demos/) for real-world examples
