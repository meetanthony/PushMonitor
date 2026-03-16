void Info(string message)
{
    var normalColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(message);
    Console.ForegroundColor = normalColor;
}

void Error(string message)
{
    var normalColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ForegroundColor = normalColor;
}

void Warn(string message)
{
    var normalColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(message);
    Console.ForegroundColor = normalColor;
}


if (args.Length < 1)
{
    Error("Please provide the path to check as an argument.");
    return;
}

var path = args[0];


string? absolutePath = null;
try
{
    absolutePath = Path.GetFullPath(path);
    if (absolutePath == null)
    {
        throw new Exception();
    }
}
catch (Exception)
{
    Error($"Invalid path: {path}");

    Console.WriteLine();
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();

    return;
}

Console.WriteLine($"Scanning {absolutePath} for Git repositories...");
Console.WriteLine();

List<RepoStatus> repos;

try
{
    repos = PushMonitor.CheckReposStatus(absolutePath);
}
catch (Exception e)
{
    Error($"Scanning error: {Environment.NewLine} {e}");

    Console.WriteLine();
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();

    return;
}

bool somethingFound = false;
foreach (var repo in repos)
{
    if (repo.Status != GitRepoStatus.Pushed)
    {
        somethingFound = true;
        switch (repo.Status)
        {
            case GitRepoStatus.Undefined:
            case GitRepoStatus.NotPushed:
                Error($"{repo.Path} : {repo.Status}");
                break;
            case GitRepoStatus.Uncommited:
                Warn($"{repo.Path} : {repo.Status}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

if (somethingFound)
{
    Console.WriteLine();
}
else
{
    Info("Nothing to push.");
    Console.WriteLine();
}


Console.WriteLine("Done. Press any key to exit.");

Console.ReadKey();