namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System;
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
    [MethodDataSource<TargetFramework>(nameof(TargetFramework.Values))]
    public async ValueTask Value_IsWellFormatted_EndsWithSuffix(TargetFramework tf, string suffix) =>
        await Assert.That(tf.Value).EndsWith(suffix, Ordinal);

    //[Test]
    //[MethodDataSource(nameof(TargetFrameworkData))]
    //public async Task Value_IsWellFormated(TargetFramework tf)
    //{
    //    using (Assert.Multiple())
    //    {
    //        _ = await Assert.That(tf.Value).IsNotNullOrWhiteSpace();
    //    }

    //    Assert.Multiple(
    //        () => Assert.Matches(_targetFrameworkPattern, tf.Value),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.Windows, tf.Value.EndsWith("-windows", Ordinal)),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.Android, tf.Value.EndsWith("-android", Ordinal)),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.iOS, tf.Value.EndsWith("-ios", Ordinal)),
    //        () =>
    //            Assert.Equal(
    //                tf.Platform!.Value == TargetPlatform.MacCatalyst,
    //                tf.Value.EndsWith("-maccatalyst", Ordinal)
    //            ),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.MacOs, tf.Value.EndsWith("-macos", Ordinal)),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.Tizen, tf.Value.EndsWith("-tizen", Ordinal)),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.tvOS, tf.Value.EndsWith("-tvos", Ordinal)),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.Browser, tf.Value.EndsWith("-browser", Ordinal)),
    //        () => Assert.Equal(tf.Platform!.Value == TargetPlatform.Windows, tf.Value.EndsWith("-windows", Ordinal))
    //    );
    //}

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
