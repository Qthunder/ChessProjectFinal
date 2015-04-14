using ChessProjectFinal.Entities;

namespace ChessProjectFinal.Model
{
    public struct CastlingRights
    {
        public override int GetHashCode()
        {
            unchecked
            {
                return (whitePlayer.GetHashCode()*397) ^ blackPlayer.GetHashCode();
            }
        }

        public bool Equals(CastlingRights other)
        {
            return whitePlayer.Equals(other.whitePlayer) && blackPlayer.Equals(other.blackPlayer);
        }

       

        private  bool whitePlayer;
        private  bool blackPlayer;
       
        public CastlingRights(bool whitePlayer, bool blackPlayer)
        {
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CastlingRights && Equals((CastlingRights) obj);
        }

        public bool this[Player index]
        {
            get { return index == Player.WHITE ? whitePlayer : blackPlayer; }
            set
            {
                if (index == Player.WHITE)
                {
                    this.whitePlayer = value;
                }
                else
                {
                    this.blackPlayer = value;
                }
            }
        }

        

    }
}
