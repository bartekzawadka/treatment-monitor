using System.IO;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;

[CheckBuildProjectConfigurations]
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

    [Solution("src/backend/Treatment.Monitor/Treatment.Monitor.sln")]
    readonly Solution Solution;
    readonly AbsolutePath SourceDirectory = RootDirectory / "src";
    readonly AbsolutePath ApiDirectory = RootDirectory / "src" / "backend" / "Treatment.Monitor";
    readonly AbsolutePath AppDirectory = RootDirectory / "src" / "app" / "treatment-monitor";
    readonly AbsolutePath PublishDirectory = RootDirectory / "publish";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj", "**/dist").ForEach(path => Directory.Delete(path, true));
            var nodeModulesPath = AppDirectory / "node_modules";
            if (Directory.Exists(nodeModulesPath))
            {
                DeleteDirectory(nodeModulesPath);
            }

            var distPath = AppDirectory / "dist";
            if (Directory.Exists(distPath))
            {
                Directory.Delete(distPath);
            }

            if (Directory.Exists(PublishDirectory))
            {
                DeleteDirectory(PublishDirectory);
            }
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            NpmTasks.NpmInstall(settings =>
                settings
                    .EnableProcessLogOutput()
                    .SetProcessWorkingDirectory(AppDirectory));

            NuGetTasks.NuGetRestore(settings =>
                settings
                    .SetTargetPath(Solution)
                    .EnableNoCache());
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            NpmTasks.NpmRun(settings =>
                settings
                    .SetCommand("build")
                    .SetProcessWorkingDirectory(AppDirectory)
                    .EnableProcessLogOutput());

            DotNetTasks.DotNetBuild(settings =>
                settings
                    .SetProjectFile(Solution.GetProject("Treatment.Monitor.Api")?.Path)
                    .SetConfiguration(Configuration)
                    .SetVerbosity(DotNetVerbosity.Minimal));

            DotNetTasks.DotNetBuild(settings =>
                settings
                    .SetProjectFile(Solution.GetProject("Treatment.Monitor.Notifier")?.Path)
                    .SetConfiguration(Configuration)
                    .SetVerbosity(DotNetVerbosity.Minimal));
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            AbsolutePath srcDir = AppDirectory / "dist";
            AbsolutePath destDir = PublishDirectory / "app";

            if (Directory.Exists(destDir))
            {
                DeleteDirectory(destDir);
            }

            CopyDirectoryRecursively(srcDir, destDir);

            DotNetTasks.DotNetPublish(settings =>
                settings
                    .SetConfiguration(Configuration)
                    .SetVerbosity(DotNetVerbosity.Minimal)
                    .SetFramework("net5.0")
                    .SetRuntime("linux-x64")
                    .SetProject(Solution.GetProject("Treatment.Monitor.Api"))
                    .SetOutput(PublishDirectory / "api"));

            DotNetTasks.DotNetPublish(settings =>
                settings
                    .SetConfiguration(Configuration)
                    .SetVerbosity(DotNetVerbosity.Minimal)
                    .SetFramework("net5.0")
                    .SetRuntime("linux-x64")
                    .SetProject(Solution.GetProject("Treatment.Monitor.Notifier"))
                    .SetOutput(PublishDirectory / "notifier"));
        });
}
