using Point = (int x, int y);

class Day6
{
    public Point GetStartPosition(string[] input)
    {
        foreach (var (j, line) in input.Index())
        {
            foreach (var (i, c) in line.Index())
            {
                if (c == '^')
                {
                    return (i, j);
                }
            }
        }
        throw new Exception("Start position not found");
    }

    public Point GetNextPosition(int x, int y, Direction direction)
    {
        return direction switch
        {
            Direction.North => (x, y - 1),
            Direction.East => (x + 1, y),
            Direction.South => (x, y + 1),
            Direction.West => (x - 1, y),
            _ => throw new NotImplementedException(),
        };
    }

    public Direction RotateCW90(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new NotImplementedException(),
        };
    }

    public (Point, Direction) Move(string[] input, int x, int y, Direction direction, Point? extraObs = null)
    {
        var next = GetNextPosition(x, y, direction);
        if (OOB(input, next))
        {
            return (next, direction);
        }
        while (input[next.y][next.x] == '#' || (extraObs != null && (next.x, next.y) == extraObs))
        {
            direction = RotateCW90(direction);
            next = GetNextPosition(x, y, direction);
        }
        return (next, direction);
    }

    public bool OOB(string[] input, Point position)
    {
        return position.y < 0 || position.y >= input.Length || position.x < 0 || position.x >= input[position.y].Length;
    }

    public HashSet<Point> Part1(string[] input)
    {
        var position = GetStartPosition(input);
        var direction = Direction.North;
        var visited = new HashSet<Point> { position };
        while (true)
        {
            (position, direction) = Move(input, position.x, position.y, direction);
            if (OOB(input, position))
            {
                break;
            }
            visited.Add(position);
        }
        return visited;
    }

    public bool RunTerminates(string[] input, Point point)
    {
        var position = GetStartPosition(input);
        var direction = Direction.North;
        var iter = 0;
        // getting it finished is better than getting it right
        while (iter < 10000)
        {
            (position, direction) = Move(input, position.x, position.y, direction, point);
            if (OOB(input, position))
            {
                return true;
            }
            iter++;
        }
        return false;
    }

    public int Part2(string[] input, HashSet<Point> visited)
    {
        return visited.Where(point => input[point.y][point.x] == '.').Select(point =>
        {
            return (point.x, point.y);
        }).AsParallel().Count(point =>
        {
            return !RunTerminates(input, point);
        });
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/6");
        var visited = Part1(input);
        Console.WriteLine("Part 1: {0}", visited.Count);
        Console.WriteLine("Part 2: {0}", Part2(input, visited));
    }
}

internal enum Direction
{
    North,
    East,
    South,
    West
}
