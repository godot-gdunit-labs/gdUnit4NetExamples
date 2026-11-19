# Multi-Project Test Setup

An advanced example demonstrating how to set up two sets of MSBuild configurations (Release/Debug and ReleaseExport/ReleaseDebug), one of which exports the project without any test-related artifacts, while keeping everything in a single project file.


## What This Example Shows

## Why Use Multi-Config Setup?

- **Clean Separation**: Tests and test-only packages don't end up in the game build
- **Better Organization**: Clear boundaries between game and test code
- **Flexible Deployment**: Ship the game without test dependencies

## Project Structure

```shell
MultiConfigSetup/
├── tests/                             # All tests should go in this folder
│   └── GdUnitTest.cs                  # Example unit test
├── project.godot
├── MultiConfigProject.csproj          # Project file with conditional rules for ./tests/
└── MultiConfigProject.sln             # Solution file defining the configs
```

## Configuration

## Writing Tests

### Scenes and Resources

### Input Actions

### Autoloads and Project Settings

## Rules and Limitations

## Running Tests

### Prerequisites

- .NET 9.0 SDK or later
- Godot 4.7 .NET (matching the `Godot.NET.Sdk` version)
- `GODOT_BIN` pointing to the Godot executable, set as environment variable or in `.runsettings`

### Command Line

### Visual Studio / Rider / VS Code

Open `ExampleProject.sln`, build the solution and run the tests from the test explorer.

## Troubleshooting

## Additional Resources

- [Multi-project guide](https://github.com/godot-gdunit-labs/gdUnit4Net/blob/master/Api/doc/multi-project.md)
- [gdUnit4Net Documentation](https://github.com/godot-gdunit-labs/gdUnit4Net)
- [.NET Testing Documentation](https://docs.microsoft.com/en-us/dotnet/core/testing/)
