# Advanced Examples

Welcome to the advanced gdUnit4Net examples! This section covers sophisticated testing techniques, professional configurations,
and complex scenarios you'll encounter in real-world Godot projects.

## Prerequisites

Before diving into advanced examples, ensure you understand:

- Basic gdUnit4Net concepts from [Basics](../Basics/README.md)
- Test project setup and configuration
- Environment variables vs configuration files
- Basic Godot object lifecycle and memory management

## Getting Started with Advanced Topics

**Ready for more complex testing?** Explore in this order:

1. [Global Test Settings](Setup/TestWithRunsettings/README.md) - Professional test configuration using .runsettings files
2. [Compile-time Analysis](Setup/TestWithAnalyzers/README.md) - Static analysis and error prevention with gdUnit4.analyzers
3. [Multi-Project Testing](Setup/MultiProjectSetup/README.md) - Separate game and test projects with a project reference
4. [Complex Test Scenarios](Tests/) - Advanced testing patterns and techniques

## Advanced Topics Structure

The advanced examples are organized into:

```shell
├── Setup/              # Advanced project configurations
│   ├── TestWithRunsettings/    # Professional test settings configuration
│   ├── TestWithAnalyzers/      # Compile-time validation and code analysis 
│   └── MultiProjectSetup/      # Multi-project architecture with a project reference
└── Tests/              # Complex testing scenarios and patterns
    ├── SceneTesting/           # Scene loading, lifecycle, and interaction testing
    ├── InputTesting/           # Mouse, keyboard, and user input simulation
    ├── SignalTesting/          # Signal emission, monitoring, and workflow testing
    └── ExceptionTesting/       # Standard and Godot-specific exception handling
```

## What Each Section Covers

### Setup/

Professional project configurations for production-ready test environments:

- **TestWithRunsettings**: Centralized configuration using .runsettings files
  - Alternative to environment variables for team and CI/CD scenarios
  - Multiple output formats and advanced logging
  - Environment-specific configurations
  - Better IDE integration and consistent team settings

- **TestWithAnalyzers**: Compile-time validation with gdUnit4.analyzers
  - Static analysis to catch common testing mistakes at compile time
  - Prevents runtime errors by detecting missing [RequireGodotRuntime] attributes
  - IDE integration with real-time feedback and quick fixes
  - Enforces gdUnit4Net best practices automatically
  - Improves developer productivity and code quality

- **MultiProjectSetup**: Separate test and production projects architecture
  - Clean separation between production and test code
  - Test project references the game project with a plain `ProjectReference`
  - Game scenes, resources, input map, autoloads and project settings available at test runtime
  - Requires `gdUnit4.api` 5.1.0-rc7 or later

### Tests/

Advanced testing patterns and complex scenarios:

- **Scene Testing**: Loading, managing, and testing Godot scenes
  - Suite-level vs per-test scene loading strategies
  - Scene lifecycle management and memory optimization

- **Input Testing**: Comprehensive user input simulation and validation
  - Mouse input: positioning, clicking, hover detection
  - Keyboard input: key combinations, modifiers, text input processing
  - Input state validation through both Godot's Input singleton and scene properties
  - UI element interaction testing with precise event simulation

- **Signal Testing**: Godot signal communication and workflow validation
  - Basic signal emission and monitoring with timeout handling
  - Signal chaining and asynchronous workflow testing
  - Signal parameter validation and emission counting
  - Integration between input events and signal-driven game logic

- **Exception Testing**: Robust error handling and exception validation
  - Standard C# exception testing (ArgumentException, NullReferenceException, etc.)
  - Godot-specific exception monitoring with [GodotExceptionMonitor] attribute
  - Silent exception detection during frame processing and node lifecycle
  - Integration between Godot error reporting (GD.PushError) and test failures

## Key Concepts You'll Learn

### Project Architecture

- Multi-project solution organization for large codebases
- Referencing a game project to use its resources and settings in tests
- Clean separation of concerns between test and production
- Managing dependencies and references effectively

### Configuration Management

- Using .runsettings for team consistency and CI/CD integration
- Environment-specific test configurations
- Advanced logging and reporting options
- Static analysis integration for error prevention

### Code Quality and Analysis

- Compile-time validation of test code
- Preventing common gdUnit4Net mistakes before runtime
- IDE integration for immediate feedback
- Enforcing coding standards and best practices

### Testing Strategies

- Scene-based testing patterns for UI and game logic
- Input simulation for realistic user interaction testing
- Signal-driven architecture testing and validation
- Exception monitoring for robust error handling
- Isolation techniques for complex dependencies

### Production Readiness

- Test organization for large codebases
- Continuous integration best practices
- Test reporting and documentation
- Debugging complex test failures

## Complexity Progression

| Level        | Focus                   | Examples                                                                                         |
|--------------|-------------------------|--------------------------------------------------------------------------------------------------|
| **Basics**   | Learning fundamentals   | Simple assertions, basic setup                                                                   |
| **Advanced** | Professional techniques | Multi-project architecture, configuration management, static analysis, complex testing scenarios |

## When to Use Advanced Techniques

### Use Multi-Project Setup When

- ✅ Large projects requiring extensive test coverage
- ✅ Team projects with dedicated QA/test developers
- ✅ Production builds where size and dependencies matter
- ✅ Projects following enterprise architecture patterns
- ✅ Need different build configurations for test vs production

### Use Advanced Configuration When

- ✅ Working in a team environment
- ✅ Setting up CI/CD pipelines
- ✅ Need multiple test output formats
- ✅ Managing different environments (dev, staging, prod)
- ✅ Want consistent configuration across team members

### Use Static Analysis When

- ✅ Teams wanting to enforce gdUnit4Net best practices automatically
- ✅ Projects requiring high test reliability and quality
- ✅ Developers who want immediate feedback on testing mistakes
- ✅ Codebases with many contributors to maintain consistency
- ✅ CI/CD pipelines that should fail fast on test configuration errors

### Use Advanced Testing When

- ✅ Testing complex Godot scenes with multiple components
- ✅ Validating user input handling and UI interactions
- ✅ Testing signal-driven game logic and communication patterns
- ✅ Need comprehensive exception handling and error recovery testing
- ✅ Working with large, complex game systems requiring isolation

## Testing Patterns Overview

| Pattern               | Use Case                      | Key Benefits                                           |
|-----------------------|-------------------------------|--------------------------------------------------------|
| **Scene Testing**     | UI components, game objects   | Realistic testing environment, component integration   |
| **Input Testing**     | User interactions, controls   | Precise input simulation, state validation             |
| **Signal Testing**    | Event-driven logic, workflows | Communication pattern validation, timing verification  |
| **Exception Testing** | Error handling, robustness    | Silent failure detection, comprehensive error coverage |

## Setup Examples Comparison

| Aspect                   | TestWithRunsettings          | MultiProjectSetup           | TestWithAnalyzers                 |
|--------------------------|------------------------------|-----------------------------|-----------------------------------|
| **Primary Focus**        | Test execution configuration | Project architecture        | Code quality and error prevention |
| **Configuration Method** | .runsettings files           | ProjectReference in .csproj | gdUnit4.analyzers package         |
| **Team Benefits**        | Consistent test settings     | Clean code separation       | Enforced coding standards         |
| **CI/CD Integration**    | Multiple output formats      | Isolated test environment   | Early error detection             |
| **Developer Experience** | Professional test reports    | Better IDE performance      | Real-time feedback in IDE         |
| **Error Prevention**     | Runtime configuration issues | Test code in game builds    | Compile-time validation           |
| **Best For**             | Team environments            | Large projects              | Code quality enforcement          |

## Project Architecture Comparison

| Aspect              | Single Project       | Multi-Project with ProjectReference |
|---------------------|----------------------|-------------------------------------|
| **Resource Paths**  | Direct access        | Original paths preserved            |
| **Game Settings**   | Direct access        | Applied at test runtime             |
| **Build Setup**     | Simple               | Simple (one `ProjectReference`)     |
| **Git Complexity**  | Simple               | Simple                              |
| **Team Workflow**   | Mixed code           | Clean separation                    |
| **Game Build Size** | Includes test code   | Game only                           |

## Next Steps

After mastering advanced techniques:

- Apply learned concepts to your own projects
- Combine multi-project architecture with .runsettings configuration
- Add analyzer validation to your test projects
- Integrate scene, input, signal, and exception testing patterns
- Explore integration with external testing tools
- Share your testing strategies with the community

## Contributing Advanced Examples

Have an advanced testing scenario not covered here?
We welcome contributions!
