using System.Windows;

namespace ChessProjectFinal.Entities
{
    public class Move :IMove
    {
        public override string ToString()
        {
            if (IsKingSideCastle)
                return "0-0";
            if (IsQueenSideCastle)
                return "0-0-0";
            var s = "";
            switch (Piece.PieceType)
            {
                    case PieceType.PAWN:
                    if (capturedPiece != null)
                        s += From;
                    break;
                    case PieceType.BISHOP:
                        s += "B";
                    break;
                    case PieceType.KNIGHT:
                        s += "N";
                    break;
                    case PieceType.ROOK:
                         s += "R";
                    break;
                    case PieceType.QUEEN:
                         s += "Q";
                    break;
                    case PieceType.KING:
                    s += "K";
                    break;
            }
            if (capturedPiece != null)
                s += "x";
            s += (char) (((int) (To.Y)) + 97);
            s += ((int) To.X + 1);
            if (Promotion!=null)
                switch (Promotion.PieceType)
                {
                    case PieceType.PAWN:
                        break;
                    case PieceType.BISHOP:
                        s += "B";
                        break;
                    case PieceType.KNIGHT:
                        s += "N";
                        break;
                    case PieceType.ROOK:
                        s += "R";
                        break;
                    case PieceType.QUEEN:
                        s += "Q";
                        break;
                }
            return s;
        }

        protected bool Equals(Move other)
        {
            return Equals(Promotion, other.Promotion) && From.Equals(other.From) && To.Equals(other.To) && Equals(Piece, other.Piece) && Equals(CapturedPiece,other.CapturedPiece);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Promotion != null ? Promotion.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ From.GetHashCode();
                hashCode = (hashCode*397) ^ To.GetHashCode();
                return hashCode;
            }
        }

        private readonly bool isEnPassant;
        private readonly bool isKingSideCastle;
        private readonly bool isQueenSideCastle;
        private readonly Piece piece;
        private readonly Piece capturedPiece;
        private readonly Piece promotion;
        private readonly Point from;
        private readonly Point to;

        public bool IsEnPassant
        {
            get { return isEnPassant; }
        }

        public bool IsKingSideCastle
        {
            get { return isKingSideCastle; }
            
        }

        public bool IsQueenSideCastle
        {
            get { return isQueenSideCastle; }
        }

        public Piece Piece
        {
            get { return piece; }
        }

        public Piece CapturedPiece
        {
            get
            {
                return capturedPiece;
                
            }
        }

        public Piece Promotion
        {
            get { return promotion; }
        }

        public Point From { get { return from; }  }
        public Point To { get { return to; } }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Move) obj);
        }


        public Move(Point from, Point to, Piece piece,Piece capturedPiece) :this(from,to,piece,capturedPiece,false,false,false,null)
        {
            
        }

        public Move(Point from, Point to, Piece piece, Piece capturedPiece,bool isEnPassant, bool isKingSideCastle, bool isQueenSideCastle, Piece promotion  )
        {
            this.from = from;
            this.to = to;
            this.piece = piece;
            this.isEnPassant = isEnPassant;
            this.isKingSideCastle = isKingSideCastle;
            this.isQueenSideCastle = isQueenSideCastle;
            this.capturedPiece = capturedPiece;
            this.promotion = promotion;
        }
    }
}
