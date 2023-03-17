using Spectre.Console;

namespace AutoInit.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rule = new Rule("AutoInit - Version 1.0");
            rule.Alignment = Justify.Right;
            AnsiConsole.Write(rule);

        }
    }
}