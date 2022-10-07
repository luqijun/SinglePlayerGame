using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePlayerGame.Models
{
    public class MoveStepInfo
    {
        public bool CanMove => MoveChangeInfos.Count > 0;

        public List<MoveChangeInfo> MoveChangeInfos { get; set; } = new List<MoveChangeInfo>();
    }

    public class MoveChangeInfo
    {
        public int MovedRows { get; set; }

        public int MovedCols { get; set; }

        public MoveChangeInfo(int rows, int cols)
        {
            this.MovedRows = rows;
            this.MovedCols = cols;
        }
    }
}
