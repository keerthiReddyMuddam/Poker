using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Casino : IPokerRules
    {
        public Dictionary<int, string> Rules()
        {
            Dictionary<int, string> Rules = new Dictionary<int, string>();
            Rules.Add(5, "Flush");
            Rules.Add(4, "Four of a Kind");
            Rules.Add(3, "Three Of a Kind");
            Rules.Add(2, "Pair");
            return Rules;
        }
    }
}
