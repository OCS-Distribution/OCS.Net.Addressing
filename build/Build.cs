using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] Solution Solution;
    [GitVersion(Framework = "net6.0", NoFetch = true)] readonly GitVersion GitVersion;
    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";


    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            OutputDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());

        });

    Target Test => _ => _
        .DependsOn(Compile)
        .After(Compile)
        .Before(Publish)
        .Produces(OutputDirectory / "*-results.xml")
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration)
                .SetVerbosity(DotNetVerbosity.Minimal)
                .SetResultsDirectory(OutputDirectory)
                .When(IsServerBuild, c => c.EnableUseSourceLink())
                .EnableNoRestore()
                .EnableNoBuild()
                .CombineWith(Solution.GetProjects("*.Tests"), (_, p) => _
                    .SetProjectFile(p)
                    .SetLoggers($"junit;LogFileName={p.Name}-results.xml;MethodFormat=Class;FailureBodyFormat=Verbose")
                )
            );
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Triggers(Test)
        .Requires(() => Configuration == Configuration.Release)
        .Produces(OutputDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetConfiguration(Configuration)
                .SetOutputDirectory(OutputDirectory)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetVersion(GitVersion.NuGetVersionV2)
            );
        });

    [Parameter("Nuget feed for publish package", Name = "feed")] string NugetFeed = @"https://nuget.pkg.github.com/OCS-Distribution/index.json";
    [Parameter("Nuget api key for publishing package", Name = "key")] string NugetApiKey;
    Target Publish => _ => _
        .DependsOn(Pack)
        .Requires(() => !String.IsNullOrWhiteSpace(NugetFeed))
        .Requires(() => !String.IsNullOrWhiteSpace(NugetApiKey))
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                .SetSource(NugetFeed)
                .SetApiKey(NugetApiKey)
                .CombineWith(
                    OutputDirectory.GlobFiles("*.nupkg"), 
                    (cs, f) => cs.SetTargetPath(f)
                )
            );
        });
}
