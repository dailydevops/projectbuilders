namespace NetEvolve.ProjectBuilders.Tests.Integration.Builders;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;
using NetEvolve.ProjectBuilders.TUnit;

[ClassDataSource<TemporaryDirectory>]
public class GlobalJsonBuilderTests(TemporaryDirectory directory)
{
    [Test]
    [MatrixDataSource]
    public async Task CreateAsync_Theory_Expected(
        [Matrix(Constants.RuntimeSdkDefault, "10.0.100")] string runtimeVersion,
        bool allowPrerelease,
        RollForward rollForward
    )
    {
        var subdirectory = directory.CreateDirectory($"{nameof(CreateAsync_Theory_Expected)}");
        await using var builder = new GlobalJsonBuilder(subdirectory, runtimeVersion);
        await builder.SetAllowPrerelease(allowPrerelease).SetRollForward(rollForward).CreateAsync();
        _ = await VerifyFile(builder.FullPath)
            .UseParameters(allowPrerelease, rollForward, runtimeVersion)
            .HashParameters();
    }
}
