var normalColor = Console.ForegroundColor;

if (args.Length < 1)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Please provide the path to check as an argument.");
    Console.ForegroundColor = normalColor;
    return;
}

var path = args[0];


var repos = PushMonitor.CheckReposStatus(path);

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
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case GitRepoStatus.Uncommited:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Console.WriteLine($"{repo.Path} : {repo.Status}");
    }
}

if (somethingFound)
    Console.WriteLine();
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Nothing to push.");
}

Console.ForegroundColor = normalColor;

Console.WriteLine("Done. Press any key to exit.");

Console.ReadKey();