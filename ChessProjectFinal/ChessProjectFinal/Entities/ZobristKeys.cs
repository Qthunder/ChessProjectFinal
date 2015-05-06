using System;
using System.ComponentModel;

namespace ChessProjectFinal.Entities
{
    public static class ZobristKeys
    {
        public static UInt64[,,,] Pieces =new UInt64[6,2,8,8];
        public static UInt64[] WCastlingRights = new UInt64[4];
        public static UInt64[] BCastlingRights = new UInt64[4];
        public static UInt64[,] EnPassant=new UInt64[8,8];
        public static UInt64 Side;

        public static UInt64 NextInt64(this Random rnd)
        {
            var buffer = new byte[sizeof(UInt64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        static ZobristKeys()
        {
            var rnd= new Random();
           
            for (var i=0; i<6; i++)
                for (var j=0; j<2; j++)
                    for (var x = 0; x < 8; x++)
                        for (var y=0; y<8; y++)
                            Pieces[i, j, x,y] = rnd.NextInt64();
            
            for (var i = 0; i < 4; i++)
                WCastlingRights[i] = rnd.NextInt64();
            for (var i = 0; i < 4; i++)
                BCastlingRights[i] = rnd.NextInt64();
            for (var x = 0; x < 8; x++)
                for (var y = 0; y < 8; y++)
                     EnPassant[x,y] = rnd.NextInt64();
            Side = rnd.NextInt64();



        }
    }
}
