using SinglePlayerGame.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePlayerGame.Structures
{
    /// <summary>
    /// 构造一个排序集合
    /// </summary>
    public class ChessPieceCollection : IEnumerable<ChessPiece>
    {
        SortedSet<ChessPiece> _list = new SortedSet<ChessPiece>(new PieceComparer());

        public int Count => _list.Count;

        public ChessPieceCollection()
        {

        }

        public void Add(ChessPiece piece)
        {
            _list.Add(piece);
        }

        public void Remove(ChessPiece piece)
        {
            _list.Remove(piece);
        }

        public IEnumerator<ChessPiece> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PieceComparer : IComparer<ChessPiece>
    {
        public int Compare(ChessPiece? x, ChessPiece? y)
        {
            if (x == null || y == null)
                throw new Exception("参数不合法！");

            if (x == y)
                return 0;

            if (x.Distance >= y.Distance)
                return -1;
            else
                return 1;
        }
    }
}
