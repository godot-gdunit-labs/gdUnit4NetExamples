# Multi-Project Test Setup

An advanced example demonstrating how to test a Godot game from a separate test project. The test project references the game project, and the
game's scenes, resources, scripts, input map, autoloads and project settings are available at test runtime without any further setup.

> Multi-project support requires `gdUnit4.api` 5.1.0-rc7 or later. For the full background, read the
> [multi-project guide](https://github.com/godot-gdunit-labs/gdUnit4Net/blob/master/Api/doc/multi-project.md).

## What This Example Shows

- **Separate game and test projects**, each with its own `project.godot` and C# project
- **A plain `ProjectReference`** from the test project to the game project, no file copying or symbolic links
- **Game scenes and resources** loaded by `res://` and `uid://` paths
- **Game project settings** at test runtime: input actions, autoloads and custom settings
- **Input simulation** on a game scene using the game's input actions

## Why Use Multi-Project Setup?

- **Clean Separation**: Tests and test-only packages don't end up in the game build
- **Better Organization**: Clear boundaries between game and test code
- **Flexible Deployment**: Ship the game without test dependencies

## Project Structure

```shell
MultiProjectSetup/
├── ExampleProject/                    # The game project
│   ├── ExampleProject.csproj
│   ├── project.godot                  # Input map, autoload and custom settings
│   ├── icon.svg
│   └── src/
│       ├── Calculator.cs              # Plain C# logic
│       ├── ExampleScene.tscn/.cs      # Scene with a game sub-scene
│       ├── GameState.cs               # Autoload 'GameState'
│       ├── Player.tscn/.cs            # Uses input actions, settings and the autoload
│       ├── PlayerStats.cs             # Custom resource type
│       ├── player_stats.tres          # Custom resource instance
│       └── scenes/LabelScene.tscn
│
├── ExampleProject.Test/               # The test project
│   ├── ExampleProject.Test.csproj     # References ExampleProject.csproj
│   ├── project.godot                  # The test project's own Godot project
│   └── test/
│       ├── CalculatorTest.cs          # Logic-only test, runs without Godot
│       ├── ExampleSceneTest.cs        # Runs a game scene
│       ├── GameResourceTest.cs        # Loads game resources by path and uid
│       ├── PlayerSceneTest.cs         # Game scene with input actions and autoload
│       └── ProjectSettingsTest.cs     # Game input map, autoload and settings
│
└── ExampleProject.sln
```

## Configure the Test Project

Add a project reference to the game project in the test project's C# project file:

```xml
<Project Sdk="Godot.NET.Sdk/4.7.0">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
    <TestFramework>GdUnit4</TestFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- The game project: gdUnit4 mounts its resources, scenes and settings at test runtime -->
    <ProjectReference Include="../ExampleProject/ExampleProject.csproj"/>
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk"/>
    <PackageReference Include="gdUnit4.api"/>
    <PackageReference Include="gdUnit4.test.adapter"/>
  </ItemGroup>
</Project>
```

The package versions come from the central [Directory.Packages.props](../../../../Directory.Packages.props) of this repository.

The `gdUnit4.api` package detects referenced projects that contain a `project.godot` file and prepares them before a test run.

## Using the Game Project in Tests

### Scenes and Resources

The game files keep their own `res://` paths:

```csharp
var stats = GD.Load<PlayerStats>("res://src/player_stats.tres");
var byUid = GD.Load<PlayerStats>("uid://mpstats001");
var runner = ISceneRunner.Load<Player>("res://src/Player.tscn", true);
```

### Input Actions

The game's input map is applied, so a game scene can be driven by its own actions:

```csharp
[TestCase]
public async Task GameSceneReactsOnInputAction()
{
    var runner = ISceneRunner.Load<Player>("res://src/Player.tscn", true);

    runner.SimulateActionPressed("jump");
    await runner.AwaitInputProcessed();

    AssertThat(runner.Scene().JumpCount).IsEqual(1);
}
```

### Autoloads and Project Settings

```csharp
var gameState = ((SceneTree)Engine.GetMainLoop()).Root.GetNode<GameState>("GameState");
var lives = ProjectSettings.GetSetting("game/rules/start_lives").AsInt32();
```

## Rules and Limitations

- The test project remains the runtime's root project. Settings marked by Godot as requiring a restart, such as `application/config/name`, keep the test project's
  values (see `ProjectSettingsTest.TestProjectIdentityIsKept`).
- A file in the test project hides a game file with the same `res://` path. gdUnit4 reports these collisions as warnings.
- GDExtensions of the game project are not loaded dynamically. Add required extensions to the test project.
- Project preparation runs only for test suites marked `[RequireGodotRuntime]`. Logic-only tests like `CalculatorTest` run without starting Godot.
- To disable mounting referenced projects, set `<GdUnit4MountGodotProjectReferences>false</GdUnit4MountGodotProjectReferences>` in the test project.

See the [multi-project guide](https://github.com/godot-gdunit-labs/gdUnit4Net/blob/master/Api/doc/multi-project.md) for details on runtime behavior and
performance.

## Running Tests

### Prerequisites

- .NET 9.0 SDK or later
- Godot 4.7 .NET (matching the `Godot.NET.Sdk` version)
- `GODOT_BIN` pointing to the Godot executable, set as environment variable or in `.runsettings`

### Command Line

```bash
# Build the solution
dotnet build

# Run the tests
dotnet test ExampleProject.Test

# With filter
dotnet test ExampleProject.Test --filter "FullyQualifiedName~PlayerSceneTest"
```

### Visual Studio / Rider / VS Code

Open `ExampleProject.sln`, build the solution and run the tests from the test explorer.

## Troubleshooting

### Resources Not Found

**Problem:** "Cannot load resource: res://src/..."

**Check:**

- The test project has a `ProjectReference` to the game project
- The game project folder contains a `project.godot`
- `gdUnit4.api` is 5.1.0-rc7 or later
- The test suite is marked with `[RequireGodotRuntime]`

### Test Discovery Issues

- Ensure the gdUnit4 test adapter is installed
- Check the test class has the `[TestSuite]` attribute
- Verify test methods have the `[TestCase]` attribute
- Rebuild the solution if tests aren't detected

## Additional Resources

- [Multi-project guide](https://github.com/godot-gdunit-labs/gdUnit4Net/blob/master/Api/doc/multi-project.md)
- [gdUnit4Net Documentation](https://github.com/godot-gdunit-labs/gdUnit4Net)
- [.NET Testing Documentation](https://docs.microsoft.com/en-us/dotnet/core/testing/)
