using System;
using System.Collections.Generic;
using System.Windows;

namespace ChessProjectFinal.Model
{
    public class BoardState
    {
        public PieceStruct[,] PieceBoard;
        public bool EnPassant;
        public Point EnPassantSquare;
        public Dictionary<Player, Boolean> CastleQueenSide = new Dictionary<Player, bool>();
        public Dictionary<Player, Boolean> CastleKingSide = new Dictionary<Player, bool>();

        public BoardState(PieceStruct[,] pieceBoard,bool enPassant,Point enPassantSquare,Dictionary<Player,Boolean> castleQueenSide, Dictionary<Player,Boolean> castleKingSide)
        {
            PieceBoard = (PieceStruct[,])pieceBoard.Clone();
            EnPassant = enPassant;
            EnPassantSquare = enPassantSquare;
            CastleKingSide = castleKingSide;
            CastleQueenSide = castleQueenSide;
        }
    }
}
