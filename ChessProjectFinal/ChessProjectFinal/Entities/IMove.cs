using System;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.Entities
{
    public interface IMove
    {
        Boolean IsEnPassant { get;  }
        Boolean IsKingSideCastle{get;}
        Boolean IsQueenSideCastle { get; }
        Piece Piece { get; }
        Piece CapturedPiece { get; }
        Piece Promotion { get;  }
        System.Windows.Point From { get;  }
        System.Windows.Point To { get;  }
        
        


    }
}
