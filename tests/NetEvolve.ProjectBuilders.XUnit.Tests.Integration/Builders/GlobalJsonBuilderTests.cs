namespace NetEvolve.ProjectBuilders.XUnit.Tests.Integration.Builders;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

public class GlobalJsonBuilderTests(TemporaryDirectoryFixture directory) : IClassFixture<TemporaryDirectoryFixture>
{
    [Theory]
    [MemberData(nameof(GetTheoryData))]
    public async Task CreateAsync_Theory_Expected(
        string runtimeVersion,
        bool allowPrerelease,
        RollForward rollForward
    )
    {
        var subdirectory = directory.CreateDirectory($"{nameof(CreateAsync_Theory_Expected)}{runtimeVersion}{allowPrerelease}{rollForward}");
        await using var builder = new GlobalJsonBuilder(subdirectory, runtimeVersion);
        await builder.SetAllowPrerelease(allowPrerelease).SetRollForward(rollForward).CreateAsync(cancellationToken: TestContext.Current.CancellationToken);
        _ = await VerifyFile(builder.FullPath)
            .UseParameters(allowPrerelease, rollForward, runtimeVersion)
            .HashParameters();
    }

    public static TheoryData<string, bool, RollForward> GetTheoryData()
    {
        var data = new TheoryData<string, bool, RollForward>();
        var runtimeVersions = new[] { Constants.RuntimeSdkDefault, "10.0.100" };
        var rollForwardOptions = Enum.GetValues<RollForward>();

        foreach (var runtimeVersion in runtimeVersions)
        {
            foreach (var rollForward in rollForwardOptions)
            {
                data.Add(runtimeVersion, true, rollForward);
                data.Add(runtimeVersion, false, rollForward);
            }
        }

        return data;
    }
}
