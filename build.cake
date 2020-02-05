#tool "nuget:?package=GitVersion.CommandLine&version=5.1.3"

var target = Argument("target", "test");
var configuration = Argument("configuration", "Release");
var nugetFeed = EnvironmentVariable("nugetfeed") ?? Argument<string>("nugetfeed", null);
var nugetApiKey = EnvironmentVariable("nugetapikey") ?? Argument<string>("nugetapikey", null);

var solution = "./OCS.Net.Addressing.sln";
GitVersion version = null;

Task("version")
    .Does(() =>
{
    version = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });
    Information($@"
    Detected
        Nuget: {version.NuGetVersion}
        Sem:   {version.SemVer}
        Info:  {version.InformationalVersion}
    "
    );
});

Task("hardclean")
.Does(() =>
{
    var dirsToClean = GetDirectories("./**/bin");
    dirsToClean.Add(GetDirectories("./**/obj"));

    foreach(var dir in dirsToClean) {
        Console.WriteLine(dir);
    }
 
    CleanDirectories(dirsToClean);

    DeleteDirectories(
        dirsToClean,
        new DeleteDirectorySettings {
            Recursive = true,
            Force = true
        }
    );
});

Task("clean")
.Does(() =>
{
    DotNetCoreClean(solution);
});


Task("restore")
.Does(() =>
{
    DotNetCoreRestore(solution);
});
    
Task("build")
.IsDependentOn("clean")
.IsDependentOn("restore")
.Does(() =>
{
    DotNetCoreBuild(
        solution,
        new DotNetCoreBuildSettings() {
            Configuration = configuration
        }
    );
});

Task("test")
.IsDependentOn("build")
.Does(() =>
{
    var projects = GetFiles("./src/**/*Tests.csproj");
    foreach(var project in projects)
    {
        DotNetCoreTest(
            project.FullPath,
            new DotNetCoreTestSettings()
            {
                Configuration = configuration,
                NoBuild = true
            }
        );
    }
});

Task("release")
.IsDependentOn("version")
.IsDependentOn("test")

.Does(() =>
{
    DotNetCorePack(
        solution, 
        new DotNetCorePackSettings
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true, 
            
            ArgumentCustomization =
                args => args
                    .Append("/p:PackageVersion={0}", version.NuGetVersion)
        });

    var packages = GetFiles("./src/**/*.nupkg");

    foreach (var package in packages)
    {
        DotNetCoreNuGetPush(
            package.ToString(),
            new DotNetCoreNuGetPushSettings
            {
                Source = nugetFeed,
                ApiKey = nugetApiKey
            }
        );
    }
});

RunTarget(target);