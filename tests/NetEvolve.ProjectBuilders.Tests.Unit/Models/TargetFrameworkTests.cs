namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models;
using TUnit.Assertions.Extensions;
using static System.StringComparison;

public partial class TargetFrameworkTests
{
    [GeneratedRegex(@"^[a-z0-9\.-]+[a-z0-9]$", RegexOptions.Compiled)]
    private static partial Regex TargetFrameworkPattern();

    [Test]
    [MethodDataSource<TargetFramework>(nameof(TargetFramework.Values))]
    public async ValueTask Value_IsWellFormatted_IsNotNullOrWhiteSpace(TargetFramework tf) =>
        _ = await Assert.That(tf.Value).IsNotNullOrWhiteSpace();

    [Test]
    [MethodDataSource<TargetFramework>(nameof(TargetFramework.Values))]
    public async ValueTask Value_IsWellformatted_MatchesPattern(TargetFramework tf) =>
        _ = await Assert.That(tf.Value).Matches(TargetFrameworkPattern());

    [Test]
    [MethodDataSource(nameof(GetTargetFrameworksWithSuffix))]
    public async ValueTask Value_IsWellFormatted_EndsWithSuffix(TargetFramework tf, string suffix) =>
        await Assert.That(tf.Value).EndsWith(suffix, Ordinal);

    public static IEnumerable<(TargetFramework, string)> GetTargetFrameworksWithSuffix()
    {
        foreach (
            var value in TargetFramework.Values.Where(tf =>
                !tf.Name.StartsWith("NetFramework", Ordinal)
                && tf.Platform.HasValue
                && !tf.Platform.Equals(TargetPlatform.None)
            )
        )
        {
            yield return (
                value,
                value.Platform switch
                {
                    TargetPlatform.Android => "-android",
                    TargetPlatform.Windows => "-windows",
                    TargetPlatform.iOS => "-ios",
                    TargetPlatform.MacCatalyst => "-maccatalyst",
                    TargetPlatform.MacOs => "-macos",
                    TargetPlatform.tvOS => "-tvos",
                    TargetPlatform.Tizen => "-tizen",
                    TargetPlatform.Browser => "-browser",
                    _ => throw new NotSupportedException($"Platform '{value.Platform}' is not supported."),
                }
            );
        }
    }

    public static IEnumerable<TargetFramework> TargetFrameworkData => TargetFramework.Values;

    [Test]
    public void Create_Duplicate_ThrowsArgumentException() =>
        _ = Assert.Throws<ArgumentException>(() => TargetFramework.Create(nameof(TargetFramework.Net5), "net5.0"));

    [Test]
    public void Create_NullName_ThrowsArgumentNullException() =>
        _ = Assert.Throws<ArgumentNullException>(() => TargetFramework.Create(null!, "net5.0"));

    [Test]
    public void Create_NullValue_ThrowsArgumentNullException() =>
        _ = Assert.Throws<ArgumentNullException>(() => TargetFramework.Create(nameof(TargetFramework.Net5), null!));

    [Test]
    public void Create_EmptyName_ThrowsArgumentException() =>
        _ = Assert.Throws<ArgumentException>(() => TargetFramework.Create(string.Empty, "net5.0"));

    [Test]
    public void Create_EmptyValue_ThrowsArgumentException() =>
        _ = Assert.Throws<ArgumentException>(() => TargetFramework.Create(nameof(TargetFramework.Net5), string.Empty));
}
