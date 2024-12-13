using System.Text.RegularExpressions;
using Delta = (long x, long y);
using Steps = (long A, long B);

class Machine
{
    public Delta A { get; set; }
    public Delta B { get; set; }
    public Delta Target { get; set; }
}

class Day13
{
    Regex buttonRegex = new Regex(@"^Button .: X\+(\d+), Y\+(\d+)$");
    Regex prizeRegex = new Regex(@"X=(\d+), Y=(\d+)");

    public Machine ParseMachine(string input, long inc)
    {
        var lines = input.Split("\n");
        var machine = new Machine();
        var buttonMatchA = buttonRegex.Match(lines[0]);
        machine.A = (int.Parse(buttonMatchA.Groups[1].Value), int.Parse(buttonMatchA.Groups[2].Value));
        var buttonMatchB = buttonRegex.Match(lines[1]);
        machine.B = (int.Parse(buttonMatchB.Groups[1].Value), int.Parse(buttonMatchB.Groups[2].Value));
        var prizeMatch = prizeRegex.Match(lines[2]);
        machine.Target = (long.Parse(prizeMatch.Groups[1].Value) + inc, long.Parse(prizeMatch.Groups[2].Value) + inc);
        return machine;
    }

    public Steps Calculate(Machine machine)
    {
        // x * A1 + y * B1 = C1
        // x * A2 + y * B2 = C2

        // By Cramer's rule (determinants):
        // x = C1B2 - C2B1 / A1B2 - A2B1
        // y = A1C2 - C1A2 / A1B2 - A2B1

        var A1 = machine.A.x;
        var A2 = machine.A.y;
        var B1 = machine.B.x;
        var B2 = machine.B.y;
        var C1 = machine.Target.x;
        var C2 = machine.Target.y;

        var x = (C1 * B2 - C2 * B1) / (A1 * B2 - A2 * B1);
        var y = (A1 * C2 - C1 * A2) / (A1 * B2 - A2 * B1);

        if (x * machine.A.x + y * machine.B.x != machine.Target.x || x * machine.A.y + y * machine.B.y != machine.Target.y)
        {
            return default;
        }

        return (x, y);
    }

    public void Solve()
    {
        var input = File.ReadAllText("inputs/13").Split("\n\n");
        var part1 = input.Select(c => ParseMachine(c, 0)).Select(Calculate).Where(x => x != default).Sum(x => x.Item1*3 + x.Item2);
        Console.WriteLine("Part 1: {0}", part1);
        var part2 = input.Select(c => ParseMachine(c, 10000000000000)).Select(Calculate).Where(x => x != default).Sum(x => x.Item1*3 + x.Item2);
        Console.WriteLine("Part 2: {0}", part2);
    }
}