using System.Text.RegularExpressions;

class Day3
{
    public int Part1(string input)
    {
        var re = new Regex(@"mul\((?<a>\d{1,3}),(?<b>\d{1,3})\)");
        var matches = re.Matches(input);
        return matches.Select(m => int.Parse(m.Groups["a"].Value) * int.Parse(m.Groups["b"].Value)).Sum();
    }

    public int Part2(string input)
    {
        var re = new Regex(@"don't\(\)|do\(\)");
        var parts = re.Split(input);
        var delimiters = new[] { "do()" }.Concat(re.Matches(input).Select(m => m.Value)).ToList();
        var enabled = string.Join("", parts.Zip(delimiters).Where(p => p.Item2 == "do()").Select(p => p.Item1));

        return Part1(enabled);
    }

    public void solve()
    {
        var input = File.ReadAllText("inputs/3");
        Console.WriteLine(Part1(input));
        Console.WriteLine(Part2(input));
    }
}