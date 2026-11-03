# Examples

Welcome to the gdUnit4Net examples! This section contains hands-on examples and tutorials to help you learn unit testing
with gdUnit4Net in Godot.

## Getting Started

**New to gdUnit4Net?** Start here:

1. [Basics](Basics/README.md) - Basic step-by-step tutorials and fundamental examples to get you started with unit testing
2. [Advanced](Advanced/README.md) - Ophisticated testing techniques, professional configurations, and complex scenarios
3. [Demos](../Demos/) - Complete project examples

## Example Structure

The examples are organized into the following structure:

```shell
├── Basics/                   # Beginner examples and tutorials
│   ├── Setup/                # Project setup examples
│   └── Tests/                # Basic unit test patterns and techniques
├── Advanced/                 # Advanced testing examples
│   ├── Setup/                # Advanced project configurations
│   └── Tests/                # Complex testing scenarios and patterns
├── Demos/                    # Complete demo projects
│   ├── 2DMenuDemo/           # 2D menu testing demo
│   └── 3DRoomDemo/           # 3D room testing demo
```

## What Each Section Contains

### Basics/

Perfect for developers new to unit testing or gdUnit4Net:

- Setting up your first test project
- Writing simple assertions
- Understanding test attributes and lifecycle
- Testing basic Godot objects and .NET types

### Advanced/

For developers ready to tackle complex scenarios:

- Advanced test configurations
- Mocking and test doubles
- Testing async operations
- Performance and integration testing

### Demos/

Real-world examples showing complete projects with comprehensive test suites:

- **MenuDemo2D**: UI testing, button interactions, menu navigation
- **RoomDemo3D**: 3D physics, player movement, collision detection

## Running the Examples

Before you start, you need to clone the repository.

```bash
git clone https://github.com/MikeSchulze/gdUnit4NetExamples.git
cd gdUnit4NetExamples
dotnet build
```

### Running the Examples in JetBrains Rider

1. **Open the project in Rider**
    - Launch JetBrains Rider
    - Select "Open" and navigate to the cloned repository
    - Open the `GdUnit4NetExamples.sln` solution file

2. **View Test Explorer**
    - Go to the "Tests" tab in the left sidebar
    - The test explorer should display your test classes and methods
    - If tests aren't visible, try refreshing the test explorer

3. **Configure Test Runner Settings**
    - From the test explorer, click the settings gear icon (⚙️)
    - Select "Unit Testing Settings..." from the dropdown menu
      ![Rider Test Settings](assets/rider-test-settings1.png)

4. **Set Up .runsettings File**
    - In the Settings dialog, navigate to Build, Execution, Deployment → Unit Testing → Test Runner
    - Under "Test settings," check "Use specific .runsettings/.testsettings settings file:"
    - Click the browse button and select the `.runsettings` file located in the project root
      ![Rider Test Runner Settings](assets/rider-test-settings2.png)

5. **Configure Additional Test Settings (Optional)**
    - While in the Settings dialog, you can also:
        - Enable "Capture output" to see console output from tests
        - Adjust the logging level as needed
    - Click "Save" to apply your settings

6. **Run the Tests**
    - In the test explorer, you can run tests at different levels:
        - Run all tests by clicking the run button at the top of the test explorer
        - Run a specific test class by right-clicking on it and selecting "Run"
        - Run a single test by right-clicking on the test name and selecting "Run"
    - You can also run tests directly from the code editor by clicking the green "run" icons in the gutter next to test methods or classes

## Next Steps

After running the examples:

- Explore the [detailed documentation](../docs/) for in-depth guides
- Check out the [gdUnit4Net API documentation](https://github.com/MikeSchulze/gdUnit4Net)
- Try creating your own tests based on the patterns you've learned
