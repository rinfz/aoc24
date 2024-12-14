using Point = (long x, long y);
using Velocity = (long dx, long dy);

class Day14
{
    public void Solve()
    {
        var input = File.ReadAllLines("inputs/14");
        var robots = input.Select(Robot.Parse).ToList();

        var width = 101;
        var height = 103;
        var time = 100;
        foreach (var r in robots)
        {
            r.Simulate(width, height, time);
        }

        Console.WriteLine("Part 1: {0}", CountQuadrants(robots, width, height));

        foreach (var r in robots)
        {
            r.Reset();
        }

        for (var i = 1; i < 10000; i++)
        {
            foreach (var r in robots)
            {
                r.Simulate(width, height, 1);
            }

            var locations = new HashSet<Point>(robots.Select(r => r.Position));
            if (locations.Count == robots.Count)
            {
                PrintGrid(robots, width, height);
                Console.WriteLine("Part 2: {0}", i);
                break;
            }
        }
    }

    public long CountQuadrants(List<Robot> robots, long width, long height)
    {
        var topLeft = (0, 0, width / 2, height / 2);
        var topRight = (width / 2 + 1, 0, width, height / 2);
        var bottomLeft = (0, height / 2 + 1, width / 2, height);
        var bottomRight = (width / 2 + 1, height / 2 + 1, width, height);

        List<long> counts = [];
        foreach (var (startX, startY, endX, endY) in new[] { topLeft, topRight, bottomLeft, bottomRight })
        {
            var count = 0;
            foreach (var robot in robots)
            {
                if (robot.Position.x >= startX && robot.Position.x < endX && robot.Position.y >= startY && robot.Position.y < endY)
                {
                    count++;
                }
            }
            counts.Add(count);
        }

        return counts.Aggregate((a, b) => a * b);
    }

    public void PrintGrid(List<Robot> robots, long width, long height)
    {
        var grid = new char[height][];
        for (var i = 0; i < height; i++)
        {
            grid[i] = new char[width];
            for (var j = 0; j < width; j++)
            {
                grid[i][j] = '.';
            }
        }

        foreach (var robot in robots)
        {
            grid[robot.Position.y][robot.Position.x] = '#';
        }

        for (var i = 0; i < height; i++)
        {
            Console.WriteLine(new string(grid[i]));
        }
    }
}

class Robot
{
    private Point InitialPosition { get; set; }
    public Point Position { get; set; }
    public Velocity Velocity { get; set; }

    private long Mod(long a, long b)
    {
        return (a % b + b) % b;
    }

    public void Simulate(long width, long height, long seconds)
    {
        InitialPosition = Position;
        Position = (Mod(Position.x + Velocity.dx * seconds, width), Mod(Position.y + Velocity.dy * seconds, height));
    }

    public void Reset()
    {
        Position = InitialPosition;
    }

    public static Robot Parse(string line)
    {
        var result = new Robot();
        var a = line.Split(" ");
        var b = a[0].Split("=");
        var c = b[1].Split(",");
        var d = a[1].Split("=");
        var e = d[1].Split(",");
        result.Position = (long.Parse(c[0]), long.Parse(c[1]));
        result.Velocity = (long.Parse(e[0]), long.Parse(e[1]));
        return result;
    }
}