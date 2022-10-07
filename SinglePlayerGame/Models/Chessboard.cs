using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePlayerGame.Models
{
    /// <summary>
    /// 棋盘结构
    /// </summary>
    public class Chessboard
    {
        //边长
        public const int Length = 7;

        //棋盘每一格数据 
        ChessPiece[,] _data = new ChessPiece[Length, Length];

        public ChessPiece this[int row, int col]
        {
            get
            {
                return _data[row, col];
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Chessboard()
        {
            InitData();
        }

        /// <summary>
        /// 初始化棋盘数据
        /// </summary>
        public void InitData()
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (IsInValid(i, j))
                        SetValue(i, j, -1);
                    else
                    {
                        SetValue(i, j, 1);
                    }
                }
            }

            SetValue(3, 3, 0);
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void SetValue(int row, int col, int value)
        {
            if (_data[row, col] == null)
                _data[row, col] = new ChessPiece(row, col);

            _data[row, col].SetValue(value);
        }


        /// <summary>
        /// 是否在有效区域
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsInValid(int row, int col)
        {
            if (row <= 1 || row >= 5)
                if (col <= 1 || col >= 5)
                    return true;

            return false;
        }

        /// <summary>
        /// 是否是空白
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsBlank(int row, int col)
        {
            if (IsInValid(row, col))
                throw new Exception("参数越界！！！");

            return _data[row, col].Value == 0;
        }
    }
}
