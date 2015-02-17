using System;
using System.Collections.Generic;
using System.Windows;

namespace ChessProjectFinal.Model
{
    public class BoardState
    {
        public Piece[,] PieceBoard;
        public bool EnPassant;
        public Point EnPassantSquare;
        public CastlingRights CastleQueenSide = new CastlingRights(false,false);
        public CastlingRights CastleKingSide = new CastlingRights(false,false);
        public Player CurrentPlayer;

        public BoardState(Piece[,] pieceBoard,bool enPassant,Point enPassantSquare,CastlingRights castleQueenSide, CastlingRights castleKingSide,Player player)
        {
            CurrentPlayer = player;
            PieceBoard = (Piece[,])pieceBoard.Clone();
            EnPassant = enPassant;
            EnPassantSquare = enPassantSquare;
            CastleKingSide = castleKingSide;
            CastleQueenSide = castleQueenSide;
        }
        public BoardState(BoardState that) : this(that.PieceBoard, that.EnPassant, that.EnPassantSquare, that.CastleQueenSide,that.CastleKingSide, that.CurrentPlayer)
        {
           
        }
       




    }
}
