namespace ChessProjectFinal.Model
{
    public interface ISquare
    {
        IPiece Occupant { get; set; }

        bool IsOccupied { get; }

        int Row { get; set; }

        int Column { get; set; }

        System.Windows.Point Coords { get; }

        bool IsValidMove { get; set; }

    }
}
