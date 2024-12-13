using Point = (int x, int y);

class Day10
{
    public int RateTrailhead(List<List<int>> input, Point start, Point end)
    {
        var current = input[start.y][start.x];
        if (start == end)
        {
            return 1;
        }
        int paths = 0;
        // traverse left
        if (start.x > 0 && input[start.y][start.x - 1] - current == 1)
        {
            paths += RateTrailhead(input, (start.x - 1, start.y), end);
        }
        // traverse right
        if (start.x < input[0].Count - 1 && input[start.y][start.x + 1] - current == 1)
        {
            paths += RateTrailhead(input, (start.x + 1, start.y), end);
        }
        // traverse up
        if (start.y > 0 && input[start.y - 1][start.x] - current == 1)
        {
            paths += RateTrailhead(input, (start.x, start.y - 1), end);
        }
        // traverse down
        if (start.y < input.Count - 1 && input[start.y + 1][start.x] - current == 1)
        {
            paths += RateTrailhead(input, (start.x, start.y + 1), end);
        }

        return paths;
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/10").Select(line => line.Select(c => int.Parse(c.ToString())).ToList()).ToList();
        var trailheads = new List<Point>();
        var ends = new List<Point>();
        foreach (var (y, line) in input.Index())
        {
            foreach (var (x, c) in line.Index())
            {
                if (c == 0)
                {
                    trailheads.Add((x, y));
                }
                else if (c == 9)
                {
                    ends.Add((x, y));
                }
            }
        }

        Console.WriteLine("Part 1: {0}", trailheads.Select(th => ends.Sum(e => RateTrailhead(input, th, e) > 0 ? 1 : 0)).Sum());
        Console.WriteLine("Part 2: {0}", trailheads.Select(th => ends.Sum(e => RateTrailhead(input, th, e))).Sum());
    }
}