using System.Globalization;
using ChessExample;

CheckerBoardPosition? pos1;
CheckerBoardPosition? pos2;

Console.Write("Введите название фигуры: ");
string nameOfPiece = Console.ReadLine()!;
Console.Write("Введите начальную позицию: ");
if (!CheckerBoardPosition.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out pos1))
{
    Console.WriteLine("Начальная позиция введена неверно");
    Environment.Exit(0);
}
Console.Write("Введите конечную позицию: ");
if (!CheckerBoardPosition.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out pos2))
{
    Console.WriteLine("Конечная позиция введена неверно");
    Environment.Exit(0);
}
ChessPiece? piece = nameOfPiece.ToLower() switch
{
    "конь"  => new Knight(pos1),
    "слон"  => new Bishop(pos1),
    "ладья" => new Rook(pos1),
    "ферзь" => new Queen(pos1),
    "пешка" => new Pawn(pos1),
    "король" => new King(pos1),
    _       => null
};
if (piece != null)
{
    if (piece.CanMoveTo(pos2))
    {
        Console.WriteLine("Ход возможен");
    }
    else
    {
        Console.WriteLine("Ход невозможен");
    }
}
else
{
    Console.Write("Имя фигуры введено неверно");
}
abstract class ChessPiece
{
    protected CheckerBoardPosition Position { get; }

    protected ChessPiece(CheckerBoardPosition position)
    {
        Position = position;
    }

    public bool CanMoveTo(CheckerBoardPosition endPosition)
    {
        if (Position.X == endPosition.X && Position.Y == endPosition.Y)
            return false;

        return CanMoveToInternal(endPosition);
    }

    protected abstract bool CanMoveToInternal(CheckerBoardPosition endPosition);
}


class King : ChessPiece
{
    public King(CheckerBoardPosition position) : base(position) {}

    protected override bool CanMoveToInternal(CheckerBoardPosition endPosition)
    {
        return Math.Abs(Position.X - endPosition.X) <= 1 && Math.Abs(Position.Y - endPosition.Y) <= 1;
    }
}

class Queen : ChessPiece
{
    public Queen(CheckerBoardPosition position) : base(position) {}

    protected override bool CanMoveToInternal(CheckerBoardPosition endPosition)
    {
        return Math.Abs(endPosition.X - Position.X) == Math.Abs(Position.Y - endPosition.Y) ||
                endPosition.X - Position.X == 0 || endPosition.Y - Position.Y == 0;
    }
}

class Rook : ChessPiece
{
    public Rook(CheckerBoardPosition position) : base(position) {}

    protected override bool CanMoveToInternal(CheckerBoardPosition endPosition)
    {
        return endPosition.X - Position.X == 0 || endPosition.Y - Position.Y == 0;
    }
}

class Bishop : ChessPiece
{
    public Bishop(CheckerBoardPosition position) : base(position) {}

    protected override bool CanMoveToInternal(CheckerBoardPosition endPosition)
    {
        return Math.Abs(endPosition.X - Position.X) == Math.Abs(Position.Y - endPosition.Y);
    }
}

class Knight : ChessPiece
{
    public Knight(CheckerBoardPosition position) : base(position) {}

    protected override bool CanMoveToInternal(CheckerBoardPosition endPosition)
    {
        return (Math.Abs(endPosition.X - Position.X) == 2 && Math.Abs(endPosition.Y - Position.Y) == 1) ||
                (Math.Abs(endPosition.X - Position.X) == 1 && Math.Abs(endPosition.Y - Position.Y) == 2);
    }
}

class Pawn : ChessPiece
{
    public Pawn(CheckerBoardPosition position) : base(position) {}

    protected override bool CanMoveToInternal(CheckerBoardPosition endPosition)
    {
        Console.WriteLine("Введите цвет пешки (белый/чёрный)");
        string color = Console.ReadLine()!.ToLower();

        if (color != "белый" && color != "чёрный")
        {
            Console.WriteLine("Вы ввели несуществующий цвет пешки");
            return false;
        }
        
        int direction = color == "белый" ? 1 : -1;
        int startRow  = color == "белый" ? 2 : 7;
        int dy = endPosition.Y - Position.Y;
        
        if (endPosition.X != Position.X || dy * direction <= 0)
            return false;
        
        if (Position.Y == startRow)
            return dy == direction || dy == 2 * direction;

        return dy == direction;
    }
}

