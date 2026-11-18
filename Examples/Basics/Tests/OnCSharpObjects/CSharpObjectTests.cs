namespace GdUnit4.Examples.Basics.Tests.OnCSharpObjects;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using static Assertions;

/// <summary>
///     Demonstrates testing standard C# objects and their properties.
///     This test class shows how to test:
///     - C# object creation and property validation
///     - Object equality and property comparisons
///     - Testing custom C# classes (no Godot runtime required)
///     - Standard .NET object patterns and memory management
///     Essential patterns for testing pure C# functionality without Godot dependencies.
///     Note: These tests don't require Godot runtime as they test standard C# types.
/// </summary>
[TestSuite]
public class CSharpObjectTests
{
    /// <summary>
    ///     Tests basic C# object creation and properties.
    ///     Demonstrates fundamental object testing patterns for standard C# classes.
    ///     Shows how to validate object state after creation.
    ///     Expected result: Object is created with correct properties and no memory management needed.
    /// </summary>
    [TestCase]
    public void TestBasicObjectCreation()
    {
        // Create a C# object - no AutoFree() needed for standard objects
        var player = new Player("Hero", 10, 100.0f, true);

        // Test is equal
        AssertThat(player)
            .IsNotNull()
            .IsEqual(new Player("Hero", 10, 100.0f, true));
    }

    /// <summary>
    ///     Tests C# collections containing custom objects.
    ///     Demonstrates how to test lists and collections of custom C# objects.
    ///     Shows proper validation of complex data structures.
    ///     Expected result: Collections work correctly with custom objects and maintain proper references.
    /// </summary>
    [TestCase]
    public void TestObjectCollections()
    {
        // Create collection of objects
        var players = new List<Player>
        {
            new("Tank", 20, 150.0f, true),
            new("Healer", 18, 80.0f, true),
            new("DPS", 22, 90.0f, false)
        };

        // Test collection properties
        AssertThat(players).HasSize(3);
        AssertThat(players[0].Name).IsEqual("Tank");
        AssertThat(players[1].Name).IsEqual("Healer");
        AssertThat(players[2].Name).IsEqual("DPS");
    }

    /// <summary>
    ///     Tests C# object equality and comparison patterns.
    ///     Demonstrates different approaches to object comparison in tests.
    ///     Shows how reference equality differs from value equality.
    ///     Expected result: Object references and values behave according to C# semantics.
    /// </summary>
    [TestCase]
    public void TestObjectEqualityAndComparison()
    {
        // Create objects with same values
        var player1 = new Player("Mage", 15, 75.0f, true);
        var player2 = new Player("Mage", 15, 75.0f, true);
        var player3 = new Player("Mage", 15, 75.0f, false);
        var player4 = player1; // Same reference

        // Test reference equality
        // Test reference equality
        AssertObject(player1)
            .IsNotSame(player2) // Different instances
            .IsSame(player4) // Same reference
            .IsEqual(player2) // equal by properties
            .IsNotEqual(player3); // Different properties
    }

    /// <summary>
    ///     Tests specific C# types (HashSet, Hashtable).
    ///     Demonstrates testing inheritance and type-specific functionality.
    ///     Shows proper use of fluent assertion chaining for related properties.
    ///     Expected result: Different node types have their expected default properties and pass type checks.
    /// </summary>
    [TestCase]
    [RequireGodotRuntime]
    public void TestSpecificNodeTypes()
    {
        // Test HashSet - demonstrates fluent chaining for instance type validation
        var hashSet = new HashSet<int>();
        AssertObject(hashSet)
            .IsInstanceOf<HashSet<int>>()
            .IsInstanceOf<ISerializable>();

        // Test Hashtable - shows inheritance chain validation
        var hashtable = new Hashtable();
        AssertObject(hashtable)
            .IsInstanceOf<Hashtable>()
            .IsInstanceOf<ISerializable>();
    }
}

/// <summary>
///     Simple example class for testing C# objects.
///     Demonstrates a basic C# class with properties that can be tested.
/// </summary>
/// <param name="name">The player name.</param>
/// <param name="level">The player level.</param>
/// <param name="health">The player health.</param>
/// <param name="isAlive">Whether the player is alive.</param>
#pragma warning disable CS1591, SA1402, SA1600
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Reviewed.")]
public class Player(string name, int level, float health, bool isAlive)
{
    public string Name { get; set; } = name;

    public int Level { get; set; } = level;

    public float Health { get; set; } = health;

    public bool IsAlive { get; set; } = isAlive;
}
#pragma warning restore CS1591, SA1402, SA1600
