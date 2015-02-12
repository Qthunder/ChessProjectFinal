using System.Collections.Generic;
using System.Windows.Documents;

namespace ChessProjectFinal.Model
{
    public class Piece
    {


        public PieceType PieceType { get; set; }
        public Player Player { get; set; }



        private Piece() { }
       



        #region STATIC CONSTRUCTOR
        static Piece()
        {
            WHITE_DICTIONARY.Add(PieceType.Rook, WHITE_ROOK);
            WHITE_DICTIONARY.Add(PieceType.Knight, WHITE_KNIGHT);
            WHITE_DICTIONARY.Add(PieceType.Bishop, WHITE_BISHOP);
            WHITE_DICTIONARY.Add(PieceType.King, WHITE_KING);
            WHITE_DICTIONARY.Add(PieceType.Queen, WHITE_QUEEN);
            WHITE_DICTIONARY.Add(PieceType.Pawn, WHITE_PAWN);

            BLACK_DICTIONARY.Add(PieceType.Rook, BLACK_ROOK);
            BLACK_DICTIONARY.Add(PieceType.Knight, BLACK_KNIGHT);
            BLACK_DICTIONARY.Add(PieceType.Bishop, BLACK_BISHOP);
            BLACK_DICTIONARY.Add(PieceType.King, BLACK_KING);
            BLACK_DICTIONARY.Add(PieceType.Queen, BLACK_QUEEN);
            BLACK_DICTIONARY.Add(PieceType.Pawn, BLACK_PAWN);
            PIECES.Add(Player.White, WHITE_DICTIONARY);
            PIECES.Add(Player.Black,BLACK_DICTIONARY);
        }

        private static readonly Dictionary<PieceType, Piece> BLACK_DICTIONARY = new Dictionary<PieceType, Piece>();
        private static readonly Dictionary<PieceType, Piece> WHITE_DICTIONARY = new Dictionary<PieceType, Piece>();
        #endregion

        public static readonly Dictionary<Player,Dictionary<PieceType,Piece>> PIECES =new Dictionary<Player,Dictionary<PieceType,Piece>>(); 


        public readonly static Piece BLACK_ROOK = new Piece {PieceType = PieceType.Rook, Player = Player.Black};
        public readonly static Piece BLACK_KNIGHT = new Piece { PieceType = PieceType.Knight, Player = Player.Black };
        public readonly static Piece BLACK_QUEEN = new Piece { PieceType = PieceType.Queen, Player = Player.Black };
        public readonly static Piece BLACK_BISHOP = new Piece { PieceType = PieceType.Bishop, Player = Player.Black };
        public readonly static Piece BLACK_KING = new Piece { PieceType = PieceType.King, Player = Player.Black };
        public readonly static Piece BLACK_PAWN=new Piece{ PieceType=PieceType.Pawn,Player=Player.Black};

        public readonly static Piece WHITE_ROOK = new Piece { PieceType = PieceType.Rook, Player = Player.White };
        public readonly static Piece WHITE_KNIGHT = new Piece { PieceType = PieceType.Knight, Player = Player.White };
        public readonly static Piece WHITE_QUEEN = new Piece { PieceType = PieceType.Queen, Player = Player.White };
        public readonly static Piece WHITE_BISHOP = new Piece { PieceType = PieceType.Bishop, Player = Player.White };
        public readonly static Piece WHITE_KING = new Piece { PieceType = PieceType.King, Player = Player.White };
        public readonly static Piece WHITE_PAWN = new Piece { PieceType = PieceType.Pawn, Player = Player.White };

    }
}
