using Point = (int x, int y);
using WidePoint = (int x1, int x2, int y);

class Day15b
{
    HashSet<WidePoint> Walls { get; set; } = new HashSet<WidePoint>();
    HashSet<WidePoint> Boxes { get; set; } = new HashSet<WidePoint>();
    Point Player { get; set; }
    int Width { get; set; }
    int Height { get; set; }

    public void Solve()
    {
        var input = File.ReadAllText("inputs/15");
        var parts = input.Split("\n\n");
        var commands = parts[1];
        var grid = parts[0].Split("\n");
        Width = grid[0].Length*2;
        Height = grid.Length;

        foreach (var (y, line) in grid.Index())
        {
            foreach (var (x, c) in line.Index())
            {
                if (c == '#') Walls.Add((2*x, 2*x+1, y));
                else if (c == 'O') Boxes.Add((2*x, 2*x+1, y));
                else if (c == '@') Player = (2*x, y);
            }
        }

        foreach (var (i, cmd) in commands.Index())
        {
            if (cmd == 'v') MoveDown();
            else if (cmd == '^') MoveUp();
            else if (cmd == '<') MoveLeft();
            else if (cmd == '>') MoveRight();
        }

        PrintGrid();
        Console.WriteLine(Boxes.Sum(b => 100 * b.y + b.x1));
    }

    public void PrintGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Walls.Contains((x,x+1, y))) Console.Write("#");
                else if (Walls.Contains((x-1,x, y))) Console.Write("#");
                else if (Boxes.Contains((x,x+1, y))) Console.Write("[");
                else if (Boxes.Contains((x-1,x, y))) Console.Write("]");
                else if (Player == (x, y)) Console.Write("@");
                else Console.Write(".");
            }
            Console.WriteLine();
        }
    }

    public void MoveLeft()
    {
        if (Walls.Contains((Player.x - 2, Player.x - 1, Player.y)))
        {
            return;
        }
        var boxesToMove = new List<WidePoint>();
        var minX = Player.x;
        var dx = -2;

        for (int x = Player.x - 1; x >= 0; x-=2)
        {
            if (Boxes.Contains((x-1, x, Player.y)))
            {
                boxesToMove.Add((x-1, x, Player.y));
                minX = x;
                dx = -3;
            }
            else
            {
                break;
            }
        }

        if (!Walls.Contains((minX + dx, minX + dx + 1, Player.y)))
        {
            Player = (Player.x - 1, Player.y);
            foreach (var box in boxesToMove)
            {
                Boxes.Remove(box);
            }
            foreach (var box in boxesToMove)
            {
                Boxes.Add((box.x1 - 1, box.x2 - 1, box.y));
            }
        }
    }

    public void MoveRight()
    {
        if (Walls.Contains((Player.x + 1, Player.x + 2, Player.y))) return;
        var boxesToMove = new List<WidePoint>();
        var maxX = Player.x;
        var dx = 1;

        for (int x = Player.x + 1; x < Width; x+=2)
        {
            if (Boxes.Contains((x, x+1, Player.y)))
            {
                boxesToMove.Add((x, x+1, Player.y));
                maxX = x;
                dx = 2;
            }
            else
            {
                break;
            }
        }

        if (!Walls.Contains((maxX + dx, maxX + dx + 1, Player.y)))
        {
            Player = (Player.x + 1, Player.y);
            foreach (var box in boxesToMove)
            {
                Boxes.Remove(box);
            }
            foreach (var box in boxesToMove)
            {
                Boxes.Add((box.x1 + 1, box.x2 + 1, box.y));
            }
        }
    }

    public ((int, int), HashSet<WidePoint>) CheckColumn(HashSet<WidePoint> obj, (int, int) r, int y)
    {
        var result = new HashSet<WidePoint>();
        for (int x = r.Item1; x <= r.Item2; x++)
        {
            if (obj.Contains((x, x+1, y)))
            {
                result.Add((x, x+1, y));
            }
            if (obj.Contains((x-1, x, y)))
            {
                result.Add((x-1, x, y));
            }
        }
        if (!result.Any()) return ((r.Item1, r.Item2), result);
        var t0 = result.Select(b => b.x1).Min();
        var t1 = result.Select(b => b.x2).Max();
        return ((t0, t1), result);
    }

    public void MoveUp()
    {
        var t = (Player.x, Player.x);
        if (CheckColumn(Walls, t, Player.y).Item2.Any()) return;
        var boxesToMove = new List<WidePoint>();
        var minY = Player.y;

        for (int y = Player.y - 1; y >= 0; y--)
        {
            if (CheckColumn(Walls, t, y).Item2.Any()) break;
            var check = CheckColumn(Boxes, t, y);
            if (check.Item2.Any())
            {
                boxesToMove.AddRange(check.Item2);
                minY = y;
                t = check.Item1;
            }
            else
            {
                break;
            }
        }

        var (_, blockingWalls) = CheckColumn(Walls, t, minY-1);
        if (!blockingWalls.Any())
        {
            Player = (Player.x, Player.y - 1);
            foreach (var box in boxesToMove)
            {
                Boxes.Remove(box);
            }
            foreach (var box in boxesToMove)
            {
                Boxes.Add((box.x1, box.x2, box.y - 1));
            }
        }
    }

    public void MoveDown()
    {
        var t = (Player.x, Player.x);
        if (CheckColumn(Walls, t, Player.y).Item2.Any()) return;
        var boxesToMove = new List<WidePoint>();
        var maxY = Player.y;

        for (int y = Player.y + 1; y < Height; y++)
        {
            if (CheckColumn(Walls, t, y).Item2.Any()) break;
            var check = CheckColumn(Boxes, t, y);
            if (check.Item2.Any())
            {
                boxesToMove.AddRange(check.Item2);
                maxY = y;
                t = check.Item1;
            }
            else
            {
                break;
            }
        }

        var (_, blockingWalls) = CheckColumn(Walls, t, maxY+1);
        if (!blockingWalls.Any())
        {
            Player = (Player.x, Player.y + 1);
            foreach (var box in boxesToMove)
            {
                Boxes.Remove(box);
            }
            foreach (var box in boxesToMove)
            {
                Boxes.Add((box.x1, box.x2, box.y + 1));
            }
        }
    }
}