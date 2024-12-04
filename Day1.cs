class Day1
{
    public int Part1(int[] left, int[] right)
    {
        return left.Zip(right, (a, b) => Math.Abs(a - b)).Sum();
    }

    public int Part2(int[] left, int[] right)
    {
        var counts = right.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        return left.Sum(x => x * counts.GetValueOrDefault(x, 0));
    }

    public void solve()
    {
        var input = File.ReadAllLines("inputs/1").Select(line =>
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToArray();
        var left = (from x in input
                    orderby x.Item1
                    select x.Item1).ToArray();
        var right = (from x in input
                     orderby x.Item2
                     select x.Item2).ToArray();
        Console.WriteLine("Part 1: {0}", Part1(left, right));
        Console.WriteLine("Part 2: {0}", Part2(left, right));
    }
}
