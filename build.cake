//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var _target = Argument("target", "Default");
var _configuration = Argument("configuration", "Debug");
var _verbosity = Argument<string>("verbosity", "Minimal");

// MSBuild settings
var _msBuildVerbosity = Verbosity.Minimal;
var _targetPlatform = PlatformTarget.x64;

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var _solutionDir = Directory(".");
var _solutionFile = _solutionDir + File($"Shelly.Net.sln");
var _mainProjDir = _solutionDir + Directory("src") + Directory ("Shelly.Net");
var _artifactDir = _solutionDir + Directory("artifacts");
var _version = EnvironmentVariable<string>("build_number", "");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    var objDirs = GetDirectories($"{_solutionDir}/**/obj/**/{_configuration}");
    var binDirs = GetDirectories($"{_solutionDir}/**/bin/**/{_configuration}");

    var directories = binDirs + objDirs;
	CleanDirectory(_artifactDir);
    CleanDirectories(directories);
});

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore(_solutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
	{
		Configuration = _configuration,
	};

    DotNetCoreBuild(_solutionFile, settings);
});

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = _configuration
    };
    var unitTestProjects = GetFiles("./tests/**/*Tests.Unit.csproj");
    foreach(var file in unitTestProjects)
    {
        DotNetCoreTest(file.FullPath, settings);
    }
});

Task("Run-Integration-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = _configuration
    };
    var unitTestProjects = GetFiles("./tests/**/*Tests.Integration.csproj");
    foreach(var file in unitTestProjects)
    {
        DotNetCoreTest(file.FullPath, settings);
    }
});

Task("Pack-Release")
    .Does(() => 
{       
    string version = EnvironmentVariable("PACKAGE_VERSION");
    if (string.IsNullOrEmpty(version))
    {
        throw new Exception("Environment variable PACKAGE_VERSION must be defined");
    }

    string outputDirectory = Directory("artifacts");

    if (string.IsNullOrEmpty(outputDirectory))
    {
        throw new Exception("Environment variable LOCAL_NUGET_PACKAGES must be defined");
    }

    var settings = new DotNetCorePackSettings
    {
        ArgumentCustomization = args=>args.Append($"-p:Version={version}"),
        Configuration = _configuration,
        IncludeSymbols = true,
        OutputDirectory = outputDirectory
    };

    DotNetCorePack(_solutionFile, settings);
});

Task("Nuget-Push")
    .Does(() => 
{
    string outputDirectory = Directory("artifacts");
    
    var package = outputDirectory + File("/Shelly.Net.*.nupkg");
    var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");
    
    // Push the package.
    NuGetPush(package, new NuGetPushSettings {
        Source = "https://api.nuget.org/v3/index.json",
        ApiKey = nugetApiKey
    });
});

Task("Pack-Local")
    .Description("Builds nuget package locally for testing")
    .Does(() => 
{        
    string buildNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
    string version = $"2.0.0-build-{buildNumber}";

    string outputDirectory = EnvironmentVariable<string>("LOCAL_NUGET_PACKAGES", "");

    if (string.IsNullOrEmpty(outputDirectory))
    {
        throw new Exception("Environment variable LOCAL_NUGET_PACKAGES must be defined");
    }

    var settings = new DotNetCorePackSettings
    {
        ArgumentCustomization = args=>args.Append($"-p:Version={version}"),
        Configuration = _configuration,
        IncludeSymbols = true,
        OutputDirectory = outputDirectory
    };

    DotNetCorePack(_solutionFile, settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(_target);