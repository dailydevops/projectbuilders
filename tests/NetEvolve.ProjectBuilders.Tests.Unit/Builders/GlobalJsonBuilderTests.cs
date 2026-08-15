namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

public class GlobalJsonBuilderTests
{
    [Test]
    public async Task CreateAsync_CreatesGlobalJsonFile(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkDefault);

        // Act
        await builder.CreateAsync(cancellationToken: cancellationToken);

        // Assert
        _ = await Assert.That(File.Exists(builder.FullPath)).IsTrue();
    }

    [Test]
    public async Task CreateAsync_GeneratesValidJson(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        const string runtimeVersion = "8.0.204";
        await using var builder = new GlobalJsonBuilder(directory, runtimeVersion);

        // Act
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);
        var json = JsonDocument.Parse(content);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert
                .That(json.RootElement.GetProperty("sdk").GetProperty("version").GetString())
                .IsEqualTo(runtimeVersion);
        }
    }

    [Test]
    public async Task SetAllowPrerelease_WithTrue_IncludesInOutput(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkDefault);

        // Act
        _ = builder.SetAllowPrerelease(true);
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);
        var json = JsonDocument.Parse(content);

        // Assert
        _ = await Assert.That(json.RootElement.GetProperty("sdk").GetProperty("allowPrerelease").GetBoolean()).IsTrue();
    }

    [Test]
    public async Task SetAllowPrerelease_WithFalse_IncludesInOutput(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkDefault);

        // Act
        _ = builder.SetAllowPrerelease(false);
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);
        var json = JsonDocument.Parse(content);

        // Assert
        _ = await Assert
            .That(json.RootElement.GetProperty("sdk").GetProperty("allowPrerelease").GetBoolean())
            .IsFalse();
    }

    [Test]
    public async Task SetRollForward_WithLatestPatch_IncludesInOutput(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkDefault);

        // Act
        _ = builder.SetRollForward(RollForward.LatestPatch);
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);
        var json = JsonDocument.Parse(content);

        // Assert
        _ = await Assert
            .That(json.RootElement.GetProperty("sdk").GetProperty("rollForward").GetString())
            .IsEqualTo("latestPatch");
    }

    [Test]
    public async Task SetRollForward_WithLatestMinor_IncludesInOutput(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkDefault);

        // Act
        _ = builder.SetRollForward(RollForward.LatestMinor);
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);
        var json = JsonDocument.Parse(content);

        // Assert
        _ = await Assert
            .That(json.RootElement.GetProperty("sdk").GetProperty("rollForward").GetString())
            .IsEqualTo("latestMinor");
    }

    [Test]
    public async Task SetRuntimeSdk_UpdatesVersion(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkDefault);
        const string newVersion = Constants.RuntimeSdkLTS;

        // Act
        _ = builder.SetRuntimeSdk(newVersion);
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);
        var json = JsonDocument.Parse(content);

        // Assert
        _ = await Assert
            .That(json.RootElement.GetProperty("sdk").GetProperty("version").GetString())
            .IsEqualTo(newVersion);
    }

    [Test]
    public async Task FullPath_EndsWithGlobalJsonFileName()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new GlobalJsonBuilder(directory, "8.0.204");

        // Act
        var fullPath = builder.FullPath;

        // Assert
        _ = await Assert.That(fullPath).EndsWith("global.json");
    }

    [Test]
    public async Task CreateAsync_WithMultipleOptions_IncludesAll(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkLTS);

        // Act
        _ = builder.SetAllowPrerelease(true);
        _ = builder.SetRollForward(RollForward.LatestMinor);
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);
        var json = JsonDocument.Parse(content);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert
                .That(json.RootElement.GetProperty("sdk").GetProperty("version").GetString())
                .IsEqualTo(Constants.RuntimeSdkLTS);
            _ = await Assert
                .That(json.RootElement.GetProperty("sdk").GetProperty("allowPrerelease").GetBoolean())
                .IsTrue();
            _ = await Assert
                .That(json.RootElement.GetProperty("sdk").GetProperty("rollForward").GetString())
                .IsEqualTo("latestMinor");
        }
    }

    [Test]
    public async Task CreateAsync_WithoutOptionalSettings_OnlyIncludesVersion(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken: cancellationToken);
        await using var builder = new GlobalJsonBuilder(directory, Constants.RuntimeSdkDefault);

        // Act
        await builder.CreateAsync(cancellationToken: cancellationToken);
        var content = await File.ReadAllTextAsync(builder.FullPath, cancellationToken: cancellationToken);

        // Assert
        _ = await Assert.That(content).Contains("\"version\"");
    }
}
