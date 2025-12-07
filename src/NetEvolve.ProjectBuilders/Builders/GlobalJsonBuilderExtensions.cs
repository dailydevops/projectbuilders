namespace NetEvolve.ProjectBuilders.Builders;

using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

public static class GlobalJsonBuilderExtensions
{
    public static IGlobalJsonBuilder WithDefaults(this IGlobalJsonBuilder builder)
    {
        Argument.ThrowIfNull(builder);

        return builder
            .SetRuntimeSdk(Constants.RuntimeSdkDefault)
            .SetAllowPrerelease(false)
            .SetRollForward(RollForward.LatestMinor);
    }
}
