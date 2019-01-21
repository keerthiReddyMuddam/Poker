using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    interface IPokerRules
    {
        //Opted for a dictionary because this can be a multiple player set 
        //Dictionary<string, List<Card>> flush(Dictionary<string, List<Card>> PlayerDetails);
        //string ThreeOfaKind(Dictionary<string, List<Card>> PlayerDetails);
        //string OnePair(Dictionary<string, List<Card>> PlayerDetails);
        //string HighestCard(Dictionary<string, List<Card>> PlayerDetails);

        //Rank,Name(Flush,4ofkind etc),count of same cards which it needs to have
        Dictionary<int, string> Rules();
    }
}
