class Day2
{
    public bool AllIncreasing(int[] report)
    {
        return report.Zip(report.Skip(1), (a, b) => a < b).All(x => x);
    }

    public bool AllDecreasing(int[] report)
    {
        return report.Zip(report.Skip(1), (a, b) => a > b).All(x => x);
    }

    public bool IsSafe(int[] report)
    {
        var ordered = AllIncreasing(report) || AllDecreasing(report);
        return ordered && report.Zip(report.Skip(1), (a, b) =>
        {
            var delta = Math.Abs(b - a);
            return delta >= 1 && delta <= 3;
        }).All(x => x);
    }

    public bool SafeWhenDampened(int[] report)
    {
        for (int i = 0; i < report.Length; i++)
        {
            var dampened = report.Take(i).Concat(report.Skip(i + 1)).ToArray();
            if (IsSafe(dampened))
            {
                return true;
            }
        }
        return false;
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/2");
        var reports = input.Select(line => line.Split(' ').Select(int.Parse).ToArray()).ToArray();
        Console.WriteLine("Part 1: {0}", reports.Where(IsSafe).Count());
        Console.WriteLine("Part 2: {0}", reports.Where(SafeWhenDampened).Count());
    }
}
