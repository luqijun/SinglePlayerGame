using SinglePlayerGame.Helpers;
using SinglePlayerGame.Models;
using SinglePlayerGame.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePlayerGame.Solvers
{
    /// <summary>
    /// 反向解决器
    /// </summary>
    public class BackwardSolver
    {
        //棋盘
        Chessboard _chessboard;

        //待处理的棋子集合
        ChessPieceCollection _pieces = new ChessPieceCollection();

        public BackwardSolver(Chessboard chessboard)
        {
            _chessboard = chessboard;
        }

        /// <summary>
        /// 算方思路：从最终状态蔓延到初始状态
        /// </summary>
        public void Solve()
        {
            //最终状态
            SetFinalResult();

            //开始蔓延
            StartPropagation();
        }

        /// <summary>
        /// 设置为最终的结果数据
        /// </summary>
        public void SetFinalResult()
        {
            int length = Chessboard.Length;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (_chessboard.IsInValid(i, j))
                        _chessboard.SetValue(i, j, -1);
                    else
                        _chessboard.SetValue(i, j, 0);
                }
            }

            _chessboard.SetValue(3, 3, 1);

            CalculateDistance(_chessboard[3, 3]);
            _pieces.Add(_chessboard[3, 3]);

            PrintChessboard();//打印棋盘
        }

        /// <summary>
        /// 开始蔓延
        /// </summary>
        public void StartPropagation()
        {
            while (_pieces.Count > 0)
            {
                ChessPiece piece = GetFirstAvailableChessPiece(out MoveStepInfo stepInfo);
                if (piece == null)
                    break;

                //移动
                MoveChangeInfo moveChangeInfo = GetMoveChangeInfo(piece, stepInfo);
                int adjRow = piece.Row + moveChangeInfo.MovedRows / 2;
                int adjCol = piece.Column + moveChangeInfo.MovedCols / 2;
                int newRow = piece.Row + moveChangeInfo.MovedRows;
                int newCol = piece.Column + moveChangeInfo.MovedCols;
                _chessboard.SetValue(piece.Row, piece.Column, 0);
                _chessboard.SetValue(adjRow, adjCol, 1);
                _chessboard.SetValue(newRow, newCol, 1);

                //删除/添加集合
                ChessPiece newPiece1 = _chessboard[adjRow, adjCol];
                ChessPiece newPiece2 = _chessboard[newRow, newCol];
                CalculateDistance(newPiece1);
                CalculateDistance(newPiece2);
                _pieces.Remove(piece);
                _pieces.Add(newPiece1);
                _pieces.Add(newPiece2);

                //检测新添加的棋子的周围环境
                RefreshPiece(piece);
                RefreshPiece(newPiece1);
                RefreshPiece(newPiece2);
            }

            PrintChessboard();
        }

        /// <summary>
        /// 获取第一可处理的棋子
        /// </summary>
        /// <param name="stepInfo"></param>
        /// <returns></returns>
        public ChessPiece GetFirstAvailableChessPiece(out MoveStepInfo stepInfo)
        {
            stepInfo = null;
            foreach (ChessPiece piece in _pieces)
            {
                stepInfo = GetMoveStepInfo(piece);
                if (!stepInfo.CanMove)
                    continue;

                return piece;
            }
            return null;
        }


        /// <summary>
        /// 获取可移动的步骤信息
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public MoveStepInfo GetMoveStepInfo(ChessPiece piece)
        {
            int row = piece.Row;
            int col = piece.Column;

            MoveStepInfo stepInfo = new MoveStepInfo();
            if (row + 2 < 7 && _chessboard[row + 1, col].Value == 0 && _chessboard[row + 2, col].Value == 0)
            {
                stepInfo.MoveChangeInfos.Add(new MoveChangeInfo(2, 0));
            }
            if (row - 2 >= 0 && _chessboard[row - 1, col].Value == 0 && _chessboard[row - 2, col].Value == 0)
            {
                stepInfo.MoveChangeInfos.Add(new MoveChangeInfo(-2, 0));
            }
            if (col + 2 < 7 && _chessboard[row, col + 1].Value == 0 && _chessboard[row, col + 2].Value == 0)
            {
                stepInfo.MoveChangeInfos.Add(new MoveChangeInfo(0, 2));
            }
            if (col - 2 >= 0 && _chessboard[row, col - 1].Value == 0 && _chessboard[row, col - 2].Value == 0)
            {
                stepInfo.MoveChangeInfos.Add(new MoveChangeInfo(0, -2));
            }

            return stepInfo;
        }

        public MoveChangeInfo GetMoveChangeInfo(ChessPiece piece, MoveStepInfo stepInfo)
        {
            MoveChangeInfo moveChangeInfo = null;

            if (piece.Row <= 3)
            {
                moveChangeInfo = stepInfo.MoveChangeInfos.FirstOrDefault(o => o.MovedRows < 0);
                if (moveChangeInfo != null)
                    return moveChangeInfo;
            }
            else
            {
                moveChangeInfo = stepInfo.MoveChangeInfos.FirstOrDefault(o => o.MovedRows > 0);
                if (moveChangeInfo != null)
                    return moveChangeInfo;
            }

            if (piece.Column <= 3)
            {
                moveChangeInfo = stepInfo.MoveChangeInfos.FirstOrDefault(o => o.MovedCols < 0);
                if (moveChangeInfo != null)
                    return moveChangeInfo;
            }
            else
            {
                moveChangeInfo = stepInfo.MoveChangeInfos.FirstOrDefault(o => o.MovedCols > 0);
                if (moveChangeInfo != null)
                    return moveChangeInfo;
            }


            //默认返回第一个
            return stepInfo.MoveChangeInfos.FirstOrDefault();
        }

        /// <summary>
        /// 刷新棋子
        /// </summary>
        /// <param name="piece"></param>
        public void RefreshPiece(ChessPiece piece)
        {
            MoveStepInfo moveStepInfo = GetMoveStepInfo(piece);
            if (moveStepInfo == null)
            {
                _pieces.Remove(piece);
                RefreshAdjacentPieces(piece);
            }
        }

        /// <summary>
        /// 递归刷新邻接区域
        /// </summary>
        /// <param name="piece"></param>
        public void RefreshAdjacentPieces(ChessPiece piece)
        {
            foreach (var tempPiece in _pieces)
            {
                if (tempPiece == piece || !IsEightAdjacent(piece, tempPiece))
                    continue;

                MoveStepInfo moveStepInfo = GetMoveStepInfo(tempPiece);
                if (moveStepInfo == null)
                {
                    _pieces.Remove(tempPiece);
                    RefreshAdjacentPieces(tempPiece);
                }
            }
        }

        /// <summary>
        /// 是否在8领域内
        /// </summary>
        /// <returns></returns>
        public bool IsEightAdjacent(ChessPiece originPiece, ChessPiece targetPiece)
        {
            if (Math.Abs(targetPiece.Row - originPiece.Row) > 1)
                return false;
            if (Math.Abs(targetPiece.Column - originPiece.Column) > 1)
                return false;

            return true;
        }

        public double CalculateDistance(ChessPiece piece)
        {
            double dist = CalculateDistance(piece.Row, piece.Column);
            piece.SetDistance(dist);
            return dist;
        }

        public double CalculateDistance(int row, int col)
        {
            return Math.Sqrt(Math.Pow(row - 3, 2) + Math.Pow(col - 3, 2));
        }

        public void PrintChessboard()
        {
            LogHelper.ShowText("打印棋盘信息...");
            int length = Chessboard.Length;
            for (int i = 0; i < length; i++)
            {
                string text = " ";
                for (int j = 0; j < length; j++)
                {
                    if (_chessboard[i, j].Value == -1)
                        text += "5  ";
                    else
                        text +=$"{_chessboard[i, j].Value}  " ;

                }
                LogHelper.ShowText(text);
            }
        }
    }

}
