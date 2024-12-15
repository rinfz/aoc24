using Point = (int x, int y);

class Day15
{
    HashSet<Point> Walls { get; set; } = new HashSet<Point>();
    HashSet<Point> Boxes { get; set; } = new HashSet<Point>();
    Point Player { get; set; }
    int Width { get; set; }
    int Height { get; set; }

    public void Solve()
    {
        var input = File.ReadAllText("inputs/15");
        var parts = input.Split("\n\n");
        var commands = parts[1];
        var grid = parts[0].Split("\n");
        Width = grid[0].Length;
        Height = grid.Length;

        foreach (var (y, line) in grid.Index())
        {
            foreach (var (x, c) in line.Index())
            {
                if (c == '#') Walls.Add((x, y));
                else if (c == 'O') Boxes.Add((x, y));
                else if (c == '@') Player = (x, y);
            }
        }

        foreach (var (i, cmd) in commands.Index())
        {
            if (cmd == 'v') MoveDown();
            else if (cmd == '^') MoveUp();
            else if (cmd == '<') MoveLeft();
            else if (cmd == '>') MoveRight();
        }

        Console.WriteLine(Boxes.Sum(b => 100 * b.y + b.x));
    }

    public void PrintGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Walls.Contains((x, y))) Console.Write('#');
                else if (Boxes.Contains((x, y))) Console.Write('O');
                else if (Player == (x, y)) Console.Write('@');
                else Console.Write('.');
            }
            Console.WriteLine();
        }
    }

    private void DoMove(List<Point> boxesToMove, int cmpX, int cmpY, int dx, int dy)
    {
        if (!Walls.Contains((cmpX + dx, cmpY + dy)))
        {
            Player = (Player.x + dx, Player.y + dy);
            foreach (var box in boxesToMove)
            {
                Boxes.Remove(box);
            }
            foreach (var box in boxesToMove)
            {
                Boxes.Add((box.x + dx, box.y + dy));
            }
        }
    }

    public void MoveLeft()
    {
        if (Walls.Contains((Player.x - 1, Player.y))) return;
        var boxesToMove = new List<Point>();
        var minX = Player.x;

        for (int x = Player.x - 1; x >= 0; x--)
        {
            if (Boxes.Contains((x, Player.y)))
            {
                boxesToMove.Add((x, Player.y));
                minX = x;
            }
            else
            {
                break;
            }
        }

        DoMove(boxesToMove, minX, Player.y, -1, 0);
    }

    public void MoveRight()
    {
        if (Walls.Contains((Player.x + 1, Player.y))) return;
        var boxesToMove = new List<Point>();
        var maxX = Player.x;

        for (int x = Player.x + 1; x < Width; x++)
        {
            if (Boxes.Contains((x, Player.y)))
            {
                boxesToMove.Add((x, Player.y));
                maxX = x;
            }
            else
            {
                break;
            }
        }

        DoMove(boxesToMove, maxX, Player.y, +1, 0);
    }
    
    public void MoveUp()
    {
        if (Walls.Contains((Player.x, Player.y - 1))) return;
        var boxesToMove = new List<Point>();
        var minY = Player.y;

        for (int y = Player.y - 1; y >= 0; y--)
        {
            if (Boxes.Contains((Player.x, y)))
            {
                boxesToMove.Add((Player.x, y));
                minY = y;
            }
            else
            {
                break;
            }
        }

        DoMove(boxesToMove, Player.x, minY, 0, -1);
    }

    public void MoveDown()
    {
        if (Walls.Contains((Player.x, Player.y + 1))) return;
        var boxesToMove = new List<Point>();
        var maxY = Player.y;

        for (int y = Player.y + 1; y < Height; y++)
        {
            if (Boxes.Contains((Player.x, y)))
            {
                boxesToMove.Add((Player.x, y));
                maxY = y;
            }
            else
            {
                break;
            }
        }

        DoMove(boxesToMove, Player.x, maxY, 0, +1);
    }
}