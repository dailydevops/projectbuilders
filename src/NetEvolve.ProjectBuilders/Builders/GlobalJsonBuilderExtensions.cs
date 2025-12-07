namespace NetEvolve.ProjectBuilders.Builders;

using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Provides extension methods for <see cref="IGlobalJsonBuilder"/> to simplify common configurations.
/// </summary>
/// <remarks>
/// This static class provides convenience methods for applying standard global.json configurations,
/// such as default settings that are commonly used in test projects.
/// </remarks>
public static class GlobalJsonBuilderExtensions
{
    /// <summary>
    /// Applies default configuration to the global.json builder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method applies the following defaults:
    /// <list type="bullet">
    /// <item><description>SDK version: <see cref="Constants.RuntimeSdkDefault"/></description></item>
    /// <item><description>Allow prerelease: false</description></item>
    /// <item><description>Roll-forward policy: <see cref="RollForward.LatestMinor"/></description></item>
    /// </list>
    /// </para>
    /// <para>
    /// These defaults are suitable for most test scenarios where you want to use the default
    /// SDK version and allow rolling forward to newer patch and feature versions within the same major.minor.
    /// </para>
    /// </remarks>
    /// <param name="builder">The global.json builder to configure. Must not be <see langword="null"/>.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
    /// </exception>
    public static IGlobalJsonBuilder WithDefaults(this IGlobalJsonBuilder builder)
    {
        Argument.ThrowIfNull(builder);

        return builder
            .SetRuntimeSdk(Constants.RuntimeSdkDefault)
            .SetAllowPrerelease(false)
            .SetRollForward(RollForward.LatestMinor);
    }
}
