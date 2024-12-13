using Point = (int x, int y);
using Dir = (int dx, int dy);

class Day8
{
    (Point, Point, Dir, Dir) Antinodes(Point a, Point b)
    {
        var dx = Math.Abs(b.x - a.x);
        var dy = Math.Abs(b.y - a.y);
        int p1dx, p1dy, p2dx, p2dy;
        if (b.y > a.y)
        {
            if (b.x > a.x)
            {
                // b SE
                p1dx = -dx;
                p1dy = -dy;
                p2dx = dx;
                p2dy = dy;
            }
            else
            {
                // b SW
                p1dx = dx;
                p1dy = -dy;
                p2dx = -dx;
                p2dy = dy;
            }
        }
        else
        {
            if (b.x > a.x)
            {
                // b NE
                p1dx = -dx;
                p1dy = dy;
                p2dx = dx;
                p2dy = -dy;
            }
            else
            {
                // b NW
                p1dx = dx;
                p1dy = dy;
                p2dx = -dx;
                p2dy = -dy;
            }
        }
        return ((a.x + p1dx, a.y + p1dy), (b.x + p2dx, b.y + p2dy), (p1dx, p1dy), (p2dx, p2dy));
    }

    bool InBounds(string[] input, Point p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < input[0].Length && p.y < input.Length;
    }

    HashSet<Point> Part1(string[] input, Dictionary<char, List<Point>> antennas)
    {
        var result = new HashSet<Point>();
        foreach (var coords in antennas.Values)
        {
            // iterate over the cartesian product of the coords List
            var prod = from a in coords from b in coords select (a, b);
            foreach (var (a, b) in prod)
            {
                if (a != b)
                {
                    var (an1, an2, _, _) = Antinodes(a, b);
                    if (InBounds(input, an1))
                    {
                        result.Add(an1);
                    }
                    if (InBounds(input, an2))
                    {
                        result.Add(an2);
                    }
                }
            }
        }
        return result;
    }

    HashSet<Point> Part2(string[] input, Dictionary<char, List<Point>> antennas)
    {
        var result = new HashSet<Point>();
        foreach (var coords in antennas.Values)
        {
            // iterate over the cartesian product of the coords List
            var prod = from a in coords from b in coords select (a, b);
            foreach (var (a, b) in prod)
            {
                if (a != b)
                {
                    var (an1, an2, d1, d2) = Antinodes(a, b);
                    result.Add(a);
                    result.Add(b);
                    while (InBounds(input, an1))
                    {
                        result.Add(an1);
                        an1 = (an1.x + d1.dx, an1.y + d1.dy);
                    }
                    while (InBounds(input, an2))
                    {
                        result.Add(an2);
                        an2 = (an2.x + d2.dx, an2.y + d2.dy);
                    }
                }
            }
        }
        return result;
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/8");
        var antennas = new Dictionary<char, List<Point>>();
        foreach (var (y, line) in input.Index())
        {
            foreach (var (x, c) in line.Index())
            {
                if (c != '.')
                {
                    if (!antennas.ContainsKey(c))
                    {
                        antennas[c] = new List<Point>();
                    }
                    antennas[c].Add((x, y));
                }
            }
        }

        Console.WriteLine("Part 1: {0}", Part1(input, antennas).Count);
        Console.WriteLine("Part 1: {0}", Part2(input, antennas).Count);
    }
}