using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePlayerGame.Helpers
{
    public class LogHelper
    {
        public static Action<string> ShowTextAction { get; set; }
        public static void ShowText(string message)
        {
            ShowTextAction?.Invoke(message);
        }
    }
}
