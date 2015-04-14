using System.Collections.Generic;

namespace ChessProjectFinal.Entities
{
    public class Piece
    {


        public PieceType PieceType { get; set; }
        public Player Player { get; set; }



        private Piece() { }
       



        #region STATIC CONSTRUCTOR
        static Piece()
        {
            WHITE_DICTIONARY.Add(PieceType.ROOK, WHITE_ROOK);
            WHITE_DICTIONARY.Add(PieceType.KNIGHT, WHITE_KNIGHT);
            WHITE_DICTIONARY.Add(PieceType.BISHOP, WHITE_BISHOP);
            WHITE_DICTIONARY.Add(PieceType.KING, WHITE_KING);
            WHITE_DICTIONARY.Add(PieceType.QUEEN, WHITE_QUEEN);
            WHITE_DICTIONARY.Add(PieceType.PAWN, WHITE_PAWN);

            BLACK_DICTIONARY.Add(PieceType.ROOK, BLACK_ROOK);
            BLACK_DICTIONARY.Add(PieceType.KNIGHT, BLACK_KNIGHT);
            BLACK_DICTIONARY.Add(PieceType.BISHOP, BLACK_BISHOP);
            BLACK_DICTIONARY.Add(PieceType.KING, BLACK_KING);
            BLACK_DICTIONARY.Add(PieceType.QUEEN, BLACK_QUEEN);
            BLACK_DICTIONARY.Add(PieceType.PAWN, BLACK_PAWN);
            PIECES.Add(Player.WHITE, WHITE_DICTIONARY);
            PIECES.Add(Player.BLACK,BLACK_DICTIONARY);
        }

        private static readonly Dictionary<PieceType, Piece> BLACK_DICTIONARY = new Dictionary<PieceType, Piece>();
        private static readonly Dictionary<PieceType, Piece> WHITE_DICTIONARY = new Dictionary<PieceType, Piece>();
        #endregion

        public static readonly Dictionary<Player,Dictionary<PieceType,Piece>> PIECES =new Dictionary<Player,Dictionary<PieceType,Piece>>(); 


        public readonly static Piece BLACK_ROOK = new Piece {PieceType = PieceType.ROOK, Player = Player.BLACK};
        public readonly static Piece BLACK_KNIGHT = new Piece { PieceType = PieceType.KNIGHT, Player = Player.BLACK };
        public readonly static Piece BLACK_QUEEN = new Piece { PieceType = PieceType.QUEEN, Player = Player.BLACK };
        public readonly static Piece BLACK_BISHOP = new Piece { PieceType = PieceType.BISHOP, Player = Player.BLACK };
        public readonly static Piece BLACK_KING = new Piece { PieceType = PieceType.KING, Player = Player.BLACK };
        public readonly static Piece BLACK_PAWN=new Piece{ PieceType=PieceType.PAWN,Player=Player.BLACK};

        public readonly static Piece WHITE_ROOK = new Piece { PieceType = PieceType.ROOK, Player = Player.WHITE };
        public readonly static Piece WHITE_KNIGHT = new Piece { PieceType = PieceType.KNIGHT, Player = Player.WHITE };
        public readonly static Piece WHITE_QUEEN = new Piece { PieceType = PieceType.QUEEN, Player = Player.WHITE };
        public readonly static Piece WHITE_BISHOP = new Piece { PieceType = PieceType.BISHOP, Player = Player.WHITE };
        public readonly static Piece WHITE_KING = new Piece { PieceType = PieceType.KING, Player = Player.WHITE };
        public readonly static Piece WHITE_PAWN = new Piece { PieceType = PieceType.PAWN, Player = Player.WHITE };

    }
}
