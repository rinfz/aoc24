using Point = (int x, int y);
using Region = (int area, int perimeter);
enum Direc { Up, Down, Left, Right }

class Day12
{
    public bool PointSeen(Point p, HashSet<(Point, Direc)> seen)
    {
        return new[] { Direc.Left, Direc.Right, Direc.Up, Direc.Down }
                    .Select(d => seen.Contains((p, d)))
                    .All(p => p);
    }
    
    public bool AreaSeen(Point p, HashSet<(Point, Direc)> seen)
    {
        return new[] { Direc.Left, Direc.Right, Direc.Up, Direc.Down }
                    .Select(d => seen.Contains((p, d)))
                    .Any(p => p);
    }

    public bool Matches(string[] input, Point start, Point cmp)
    {
        if (start == cmp)
            return true;
        return cmp.x >= 0 && cmp.y >= 0 && cmp.x < input[0].Length && cmp.y < input.Length
            && input[start.y][start.x] == input[cmp.y][cmp.x];
    }

    public bool IsEdge(string[] input, Point start, Point cmp)
    {
        if (cmp.x < 0 || cmp.y < 0 || cmp.x >= input[0].Length || cmp.y >= input.Length) return true;
        return input[start.y][start.x] != input[cmp.y][cmp.x];
    }

    public (Region, int) ProcessRegion(string[] input, HashSet<(Point, Direc)> seen, Dictionary<Point, HashSet<(Point, Direc)>> sides, Point start, Point region)
    {
        var perimeter = 0;
        var area = AreaSeen(start, seen) ? 0 : 1;
        var sideCount = 0;

        (Point, Direc) l = ((start.x - 1, start.y), Direc.Left);
        (Point, Direc) r = ((start.x + 1, start.y), Direc.Right);
        (Point, Direc) u = ((start.x, start.y - 1), Direc.Up);
        (Point, Direc) d = ((start.x, start.y + 1), Direc.Down);

        foreach (var (p, dir) in new[] { l, r, u, d })
        {
            if (!seen.Contains((start, dir)))
            {
                seen.Add((start, dir));
                if (p.x < 0 || p.y < 0 || p.x >= input[0].Length || p.y >= input.Length
                    || input[p.y][p.x] != input[start.y][start.x])
                {
                    perimeter += 1;
                    if (!sides[region].Contains((start, dir)))
                    {
                        // new side
                        sideCount += 1;

                        if (dir == Direc.Left || dir == Direc.Right)
                        {
                            var delta = 0;
                            while (true)
                            {
                                Point colP = (start.x, start.y + delta);
                                Point cmp = (p.x, p.y + delta);
                                if (Matches(input, start, colP) && IsEdge(input, start, cmp))
                                {
                                    sides[region].Add((colP, dir));
                                    delta += 1;
                                }
                                else break;
                            }
                            delta = 0;
                            while (true)
                            {
                                Point colP = (start.x, start.y - delta);
                                Point cmp = (p.x, p.y - delta);
                                if (Matches(input, start, colP) && IsEdge(input, start, cmp))
                                {
                                    sides[region].Add((colP, dir));
                                    delta += 1;
                                }
                                else break;
                            }
                        }

                        if (dir == Direc.Up || dir == Direc.Down)
                        {
                            var delta = 0;
                            while (true)
                            {
                                Point colP = (start.x + delta, start.y);
                                Point cmp = (p.x + delta, p.y);
                                if (Matches(input, start, colP) && IsEdge(input, start, cmp))
                                {
                                    sides[region].Add((colP, dir));
                                    delta += 1;
                                }
                                else break;
                            }
                            delta = 0;
                            while (true)
                            {
                                Point colP = (start.x - delta, start.y);
                                Point cmp = (p.x - delta, p.y);
                                if (Matches(input, start, colP) && IsEdge(input, start, cmp))
                                {
                                    sides[region].Add((colP, dir));
                                    delta += 1;
                                }
                                else break;
                            }
                        }
                    }
                }
                else
                {
                    var ((nextA, nextP), sc) = ProcessRegion(input, seen, sides, p, region);
                    area += nextA;
                    perimeter += nextP;
                    sideCount += sc;
                }
            }
        }

        return ((area, perimeter), sideCount);
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/12");

        var seen = new HashSet<(Point, Direc)>();
        var sides = new Dictionary<Point, HashSet<(Point, Direc)>>();

        var part1 = 0;
        var part2 = 0;
        foreach (var (y, line) in input.Index())
        {
            foreach (var (x, c) in line.Index())
            {
                if (PointSeen((x, y), seen)) continue;
                sides[(x, y)] = new HashSet<(Point, Direc)>();
                var (r, sc) = ProcessRegion(input, seen, sides, (x, y), (x, y));
                part1 += r.area * r.perimeter;
                part2 += r.area * sc;
            }
        }

        Console.WriteLine("Part 1: {0}", part1);
        Console.WriteLine("Part 2: {0}", part2);
    }
}