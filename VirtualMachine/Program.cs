namespace VirtualMachine;

public static class Runner
{
    public static void Main(string[] args)
    {
        var argsDict = args.Select(s => s.Split('=')).ToDictionary(s => s[0], s => s[1]);

        var sourceFile = argsDict["source"];
        var sourceMap = argsDict["sourceMap"];
    }
}