using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePlayerGame.Models
{
    /// <summary>
    /// 棋子
    /// </summary>
    public class ChessPiece
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public int Value { get; set; }

        public double Distance { get; private set; }

        public ChessPiece(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetDistance(double distance)
        {
            Distance = distance;
        }
    }
}
