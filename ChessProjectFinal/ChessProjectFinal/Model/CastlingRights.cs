namespace ChessProjectFinal.Model
{
    public struct CastlingRights
    {
        private  bool whitePlayer;
        private  bool blackPlayer;
       
        public CastlingRights(bool whitePlayer, bool blackPlayer)
        {
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;

        }

        public bool this[Player index]
        {
            get { return index == Player.White ? whitePlayer : blackPlayer; }
            set
            {
                if (index == Player.White)
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
