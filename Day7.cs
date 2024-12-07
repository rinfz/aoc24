class Day7
{
    public (long, long[]) ParseLine(string line)
    {
        var parts = line.Split(": ");
        return (long.Parse(parts[0]), parts[1].Split(' ').Select(long.Parse).ToArray());
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/7").Select(ParseLine);
        Console.WriteLine("Part 1: {0}", input.Where(x => CanReachTarget(x.Item1, x.Item2, false)).Sum(x => x.Item1));
        Console.WriteLine("Part 1: {0}", input.Where(x => CanReachTarget(x.Item1, x.Item2, true)).Sum(x => x.Item1));
    }

    private bool CanReachTarget(long target, long[] numbers, bool part2)
    {
        var dp = new HashSet<long> { numbers[0] };

        foreach (var num in numbers[1..])
        {
            var nextDp = new HashSet<long>();

            foreach (var val in dp)
            {
                nextDp.Add(val + num);
                nextDp.Add(val * num);
                if (part2)
                {
                    // concat
                    nextDp.Add(long.Parse(val.ToString() + num));
                }
            }

            dp = nextDp;
        }

        return dp.Contains(target);
    }
}