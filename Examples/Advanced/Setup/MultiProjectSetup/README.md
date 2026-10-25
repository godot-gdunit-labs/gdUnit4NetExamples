# Multi-Project Test Setup with Symbolic Links

An advanced example demonstrating how to test Godot projects in a multi-project solution using symbolic links to preserve resource paths
and maintain clean separation between production and test code.

## What This Example Shows

This project demonstrates:

- **Multi-project test architecture** with separate test and production projects
- **Symbolic link strategy** for resource path preservation
- **Single assembly compilation** without ProjectReference
- **Cross-platform compatibility** for Windows, Linux, and macOS
- **Git-friendly setup** with proper .gitignore configuration
- **CI/CD ready structure** with automated symlink creation

## Why Use Multi-Project Setup?

### Advantages over Single Project

- **Clean Separation**: Tests don't bloat production builds
- **Independent Dependencies**: Test-only packages stay in test project
- **Better Organization**: Clear boundaries between test and production code
- **Flexible Deployment**: Ship production without test dependencies
- **IDE Performance**: Faster IntelliSense with smaller project scopes

### When to Use

- ✅ Large projects requiring extensive test coverage
- ✅ Team projects with dedicated QA/test developers
- ✅ Production builds where size and dependencies matter
- ✅ Projects following enterprise architecture patterns
- ✅ When you need different build configurations for test vs production

## Files Overview

### Main Project (ExampleProject/)

- **`ExampleProject.csproj`** - Production project with Godot SDK
- **`project.godot`** - Godot project configuration
- **`src/`** - Source code, scenes, and resources

### Test Project (ExampleProject.Test/)

- **`ExampleProject.Test.csproj`** - Test project with GdUnit4 configuration and InitialTargets
- **`.gitignore`** - Excludes symlinked folders from version control
- **`test/`** - Test files and test-specific resources
- **`src/`** - Symlink to main project's src folder (created automatically before build)

## Key Configuration: InitialTargets

The critical part of this setup is using `InitialTargets` to create the symlink **before** MSBuild evaluates which files to compile:

```xml
<Project Sdk="Godot.NET.Sdk/4.7.0" InitialTargets="CreateSymlinks">
```

This ensures:

- ✅ Symlink exists before MSBuild scans for .cs files
- ✅ No explicit `<Compile Include>` directives needed
- ✅ Works consistently in both local and CI environments

## Key Configuration Sections

### 1. Test Project Configuration

```xml
<Project Sdk="Godot.NET.Sdk/4.7.0" InitialTargets="CreateSymlinks">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <RootNamespace>GdUnit4.Examples.Advanced.Setup.MultiProjectSetup</RootNamespace>
  </PropertyGroup>

  <PropertyGroup>
    <!-- Ensures all dependencies are copied to output for test execution -->
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
    <!-- Identifies this as a GdUnit4 test project for test discovery -->
    <TestFramework>GdUnit4</TestFramework>
  </PropertyGroup>
  
  <ItemGroup>
    <!-- NO ProjectReference - we compile from symlinked source instead -->
    
    <!-- Core .NET testing infrastructure -->
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.0.1"/>
    <!-- Test frameworks -->
    <PackageReference Include="gdUnit4.api" Version="5.1.0-rc6"/>
    <!-- Test adapter to integrate GdUnit4 with .NET test runners -->
    <PackageReference Include="gdUnit4.test.adapter" Version="3.2.0"/>
  </ItemGroup>
</Project>
```

### 2. Symbolic Link Creation (InitialTargets)

```xml
<!-- This target runs BEFORE MSBuild evaluates files, thanks to InitialTargets -->
<Target Name="CreateSymlinks">
  <PropertyGroup>
    <SrcSource>$([System.IO.Path]::GetFullPath('$(MSBuildProjectDirectory)/../ExampleProject/src'))</SrcSource>
    <SrcTarget>$(MSBuildProjectDirectory)/src</SrcTarget>
  </PropertyGroup>
  
  <!-- Windows: Creates directory symlink or junction as fallback -->
  <Exec Command="if not exist &quot;$(SrcTarget)&quot; mklink /D &quot;$(SrcTarget)&quot; &quot;$(SrcSource)&quot;"
        Condition="'$(OS)' == 'Windows_NT'"
        ContinueOnError="true"/>
  
  <!-- Unix/Linux/Mac: Creates symbolic link -->
  <Exec Command="[ ! -L '$(SrcTarget)' ] &amp;&amp; ln -s '$(SrcSource)' '$(SrcTarget)' || true"
        Condition="'$(OS)' != 'Windows_NT'"
        ContinueOnError="true"/>
</Target>
```

### 3. Git Configuration

```gitignore
# .gitignore in test project
.godot/
src        # Symlinked folder - don't commit
```

## Project Structure

```shell
Solution/
├── ExampleProject/
│   ├── ExampleProject.csproj
│   ├── project.godot
│   └── src/
│       ├── ExampleScene.tscn
│       ├── ExampleScene.cs
│       ├── Calculator.cs
│       └── scenes/
│           └── LabelScene.tscn
│
├── ExampleProject.Test/
│   ├── ExampleProject.Test.csproj  [with InitialTargets="CreateSymlinks"]
│   ├── .gitignore
│   ├── src -> ../ExampleProject/src  [SYMLINK - created before build]
│   └── test/
│       ├── CalculatorTest.cs
│       └── ExampleSceneTest.cs
│
└── ExampleProject.sln
```

## How It Works

1. **Build starts** → `InitialTargets="CreateSymlinks"` runs first
2. **Symlink created** → `src/` folder now points to main project
3. **MSBuild evaluates** → Finds all .cs files in `src/` and `test/`
4. **Compilation** → All source files compiled into test assembly
5. **Tests run** → Can access both code and resources

## Setting Up the Project

### Prerequisites

- .NET 9.0 SDK or later
- Godot 4.5+ (compatible with your Godot.NET.Sdk version)
- GdUnit4 test adapter
- **Windows**: Administrator privileges or Developer Mode enabled (for symlinks)
- **Linux/macOS**: Standard user permissions

### Initial Setup

1. **Clone the repository**

   ```bash
   git clone <repository>
   cd MultiProjectSetup
   ```

2. **Build the solution**

   ```bash
   dotnet build
   ```

   The symlink will be created automatically before the build starts.

3. **Run tests**

   ```bash
   cd ExampleProject.Test
   dotnet test
   ```

### Windows-Specific Setup

If symlink creation fails on Windows:

#### Option 1: Enable Developer Mode (Recommended)

1. Open Settings → Update & Security → For Developers
2. Enable "Developer Mode"
3. Rebuild the project

#### Option 2: Run as Administrator

1. Open terminal as Administrator
2. Navigate to project directory
3. Run `dotnet build`

#### Option 3: Junction (Automatic Fallback)

The project automatically falls back to junctions if symlinks fail:

```cmd
mklink /J src ..\ExampleProject\src
```

## Resource Path Resolution

The symlink strategy ensures resource paths work identically in both projects:

**In Main Project:**

```csharp
var scene = GD.Load<PackedScene>("res://src/scenes/LabelScene.tscn");
var icon = GD.Load<Texture2D>("res://src/assets/icon.png");
```

**In Test Project (with symlink):**

```csharp
// Same paths work because of the symlink!
var scene = GD.Load<PackedScene>("res://src/scenes/LabelScene.tscn");
var icon = GD.Load<Texture2D>("res://src/assets/icon.png");
```

### Why Not Use ProjectReference?

Using `<ProjectReference>` would cause:

- **CS0436 warnings** - Types compiled twice (from reference AND symlink)
- **Assembly conflicts** - Two versions of the same types
- **Complexity** - Need to exclude files from compilation

Our approach compiles everything once in the test project.

## Running Tests

### Command Line

```bash
# From solution root
dotnet test

# With specific verbosity
dotnet test -v detailed

# With filter
dotnet test --filter "FullyQualifiedName~CalculatorTest"
```

### Visual Studio / Rider

1. Open the solution file
2. Build the solution (symlinks created automatically via InitialTargets)
3. Use Test Explorer to run tests

### VS Code

1. Install C# and .NET Test Explorer extensions
2. Open the workspace
3. Tests appear in Testing sidebar
4. Run individually or all at once

## CI/CD Integration

The setup works automatically in CI/CD pipelines thanks to InitialTargets:

```yaml
# GitHub Actions example
- name: Build
  run: dotnet build  # Symlink created automatically
  
- name: Test
  run: dotnet test
```

No manual symlink creation needed!

## Comparison with Other Setups

| Aspect | Single Project | Multi-Project with ProjectReference | Multi-Project with Symlinks (This) |
| --- | --- | --- | --- |
| **Resource Paths** | Direct access | Need adjustment | Original paths preserved |
| **Build Complexity** | Simple | CS0436 warnings | Clean build |
| **Assembly Count** | One | Two | One (test only) |
| **Disk Usage** | Minimal | Minimal | Minimal |
| **Git Complexity** | Simple | Simple | Simple (symlinks ignored) |
| **CI/CD Setup** | Simple | Simple | Simple (InitialTargets) |
| **Team Workflow** | Mixed code | Clean separation | Clean separation |

## Troubleshooting

### Symlink Not Created

**Problem:** Build fails with "type or namespace not found" errors

**Solution:** Ensure InitialTargets is set in the project file:

```xml
<Project Sdk="Godot.NET.Sdk/4.7.0" InitialTargets="CreateSymlinks">
```

### Windows Symlink Issues

**Problem:** "You do not have sufficient privilege"

**Solutions:**

1. Enable Developer Mode (best option)
2. Run as Administrator
3. Let it fall back to junction (automatic)

### Resources Not Found

**Problem:** "Cannot load resource: res://src/..."

**Check:**

- Symlink exists: `ls -la ExampleProject.Test/`
- Manually create if needed: `ln -s ../ExampleProject/src src`
- Verify .gitignore isn't excluding needed files

### Test Discovery Issues

- Ensure GdUnit4 test adapter is installed
- Check test class has `[TestSuite]` attribute
- Verify test methods have `[TestCase]` attribute
- Rebuild solution if tests aren't detected

## Best Practices

1. **No ProjectReference**: Don't add reference to main project
2. **Use InitialTargets**: Ensures symlink exists before build
3. **Keep Tests Isolated**: Don't modify shared resources in tests
4. **Test Data**: Create test-specific data when needed
5. **Clean State**: Reset any modified state after tests
6. **Documentation**: Document any special setup requirements

## Key Takeaways

- **InitialTargets** is crucial - runs before MSBuild file evaluation
- **No ProjectReference** needed - avoid duplicate compilation
- **Symlinks preserve paths** - resources load identically
- **Single assembly** - test project contains everything
- **Works everywhere** - local, CI/CD, all platforms

## Additional Resources

- [GdUnit4 Documentation](https://github.com/MikeSchulze/gdUnit4Net)
- [Godot Testing Best Practices](https://docs.godotengine.org/en/stable/tutorials/best_practices/)
- [.NET Testing Documentation](https://docs.microsoft.com/en-us/dotnet/core/testing/)
- [MSBuild InitialTargets](https://docs.microsoft.com/en-us/visualstudio/msbuild/target-build-order)
- [Symbolic Links on Windows](https://docs.microsoft.com/en-us/windows/security/threat-protection/security-policy-settings/create-symbolic-links)
