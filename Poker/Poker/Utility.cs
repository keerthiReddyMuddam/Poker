using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;

namespace Poker
{
    class Card
    {
        public Card() { }
        public Card(string suit, string Value)
        {
            _cardSuit = suit;
            _cardValue = Value;
        }
        private string _cardSuit;
        private string _cardValue;

        public string CardSuit { get { return _cardSuit; } }
        public string CardValue { get { return _cardValue; } }

        public int CardNumericValue { get { return Scores.Score[CardValue.ToUpper()]; } } //Just Card Numeric Value  without considering Suit
        public int CardSuitValue { get { return Scores.Suit[CardSuit.ToUpper()]; } } //Just Card Numeric Value  without considering Suit

        public int CardStrength { get { return Scores.Score[CardValue.ToUpper()] + Scores.Suit[CardSuit.ToUpper()]; } } //Card OverAll Value including Suit

        public bool isValid
        {
            get
            {
                if (!isValidType() || !isValidNumber())
                    return false;
                return true;
            }
        }

        private bool isValidNumber()
        {
            if (!Scores.Score.ContainsKey(CardValue.ToUpper()))
                return false;
            return true;
        }

        private bool isValidType()
        {
            var x = new Scores();
            if (!Scores.Suit.ContainsKey(CardSuit.ToUpper()))
                return false;
            return true;
        }
    }
    class Scores
    {
        public static Dictionary<string, int> Score = null;
        public static Dictionary<string, int> Suit = null;
        public Scores()
        {
            if (Score != null && Suit != null)
                return;
            Score = new Dictionary<string, int>();
            Score.Add("1", 1);
            Score.Add("2", 2);
            Score.Add("3", 3);
            Score.Add("4", 4);
            Score.Add("5", 5);
            Score.Add("6", 6);
            Score.Add("7", 7);
            Score.Add("8", 8);
            Score.Add("9", 9);
            Score.Add("10", 10);

            Score.Add("J", 11);
            Score.Add("Q", 12);
            Score.Add("K", 13);
            Score.Add("A", 14);

            Suit = new Dictionary<string, int>();
            Suit.Add("C", 10);
            Suit.Add("D", 20);
            Suit.Add("H", 30);
            Suit.Add("S", 40);
        }
    }
    class PlayerCardDetails
    {
        public static List<Player> Players = new List<Player>();
        //public static Dictionary<string, List<Card>> Players = new Dictionary<string, List<Card>>();
    }
    class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }

        public List<Card> GetSortedCards()
        {
            List<Card> cards = new List<Card>();
            //Ordering and Adding the of repeated suit/type value first
            var sameSuitCards = Hand.FindAll(x => x.CardSuit == HighestRepeatedSuit.Key).OrderByDescending(y => y.CardStrength);
            foreach (var item in sameSuitCards)
            {
                cards.Add(item);
            }
            //Ordering and Adding the remaining cards of the hand
            var otherSuitCards = Hand.FindAll(x => x.CardSuit != HighestRepeatedSuit.Key).OrderByDescending(y => y.CardStrength);
            foreach (var item in otherSuitCards)
            {
                cards.Add(item);
            }

            return cards;
        }

        public KeyValuePair<string, int> HighestRepeatedSuit
        {
            get
            {
                //getting the highest count and suit value by strength
                var q = (from card in Hand
                         group card by card.CardSuitValue into gp 
                         let count = gp.Count()
                         orderby count descending, gp.Key descending
                         select new { Value = gp.Key, Count = count }
                         ).FirstOrDefault();

                var SuitName = Scores.Suit.First(x => x.Value == q.Value).Key;
                return new KeyValuePair<string, int>(SuitName, q.Count);
            }
        }
    }

    class UtilityMethods
    {
        // Get the Each players Player Names Vs highest Suit and count details 
        public Dictionary<string, KeyValuePair<string, int>> GetSuitType(List<Player> Players)
        {
            Dictionary<string, KeyValuePair<string, int>> PlayerCount = new Dictionary<string, KeyValuePair<string, int>>();
            foreach (var player in Players)
            {
                PlayerCount.Add(player.Name, player.HighestRepeatedSuit);
            }
            return PlayerCount;
        }

        //Request and Reads the players and card details from console.
        public List<Player> ReadPlayerAndCardDetails()
        {

            // assuming that player names must be unique
            string PlayerName, CardType, CardInput, CardNumber;
            int NumberOfPlayers;

            System.Console.WriteLine("Please enter the number of players");

            //Validate the input is a number
            while (!int.TryParse(Console.ReadLine(), out NumberOfPlayers))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please enter a number!Try Again!");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please enter the number of players again");
            }

            for (int j = 0; j < NumberOfPlayers; j++)
            {
                Console.WriteLine("Please enter a Name");
                PlayerName = Console.ReadLine();
                //SortedList<int,string> Suit = new SortedList<int,string>();
                while (PlayerName == "")
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Please enter a name!Try Again!");

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    PlayerName = Console.ReadLine();
                }
                var inputLength = 0;
                List<Card> Hand = new List<Card>();
                if (!(PlayerCardDetails.Players.Count(x => x.Name == PlayerName) > 0))
                {
                    Console.WriteLine("Enter five card details");
                    for (int i = 0; i < 5; i++)
                    {

                        Card c = new Card();
                        var isValidCard = true;
                        do
                        {
                            isValidCard = true;
                            Console.WriteLine("Card " + (i + 1) + " Details");
                            CardInput = Console.ReadLine();
                            inputLength = CardInput.Length;
                            if (inputLength == 2 || inputLength == 3) // Valid Input Length -> Proceed
                            {
                                CardNumber = CardInput.Substring(0, inputLength - 1);
                                CardType = CardInput.Substring(inputLength - 1);
                                c = new Card(CardType, CardNumber);
                                //Check if the card type and number are valid
                                if (!c.isValid)
                                    isValidCard = false;
                            }
                            else// InValid Input Length -> Warn and Ask Input again
                            {
                                isValidCard = false;
                            }

                            if (!isValidCard)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Card not valid! ReTry!");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        } while (!isValidCard);
                        Hand.Add(c);
                    }
                    PlayerCardDetails.Players.Add(new Player { Name = PlayerName, Hand = Hand });
                }
                else
                {
                    Console.WriteLine("Player Name needs to be unique!ReTry!");
                    j--;
                }
            }
            return PlayerCardDetails.Players;
        }

        public void CalltheGame()
        {
            PlayerCardDetails.Players.Clear();
            Card c = new Card();
            Casino casino = new Casino();
            Dictionary<int, string> CasinoRules = casino.Rules();
            //request the player details from console
            List<Player> PlayerVsCardDetails;
            //get Player and hand details from console
            PlayerVsCardDetails = ReadPlayerAndCardDetails();

            //getting the highest card for repeating suit
            var PlayervsCardCount = GetSuitType(PlayerVsCardDetails);
            int maxcount = PlayervsCardCount.Max(y => y.Value.Value);

            var x = from C in PlayervsCardCount
                    where C.Value.Value == maxcount //&& CasinoRules.ContainsKey(C.Value)(can exclude 3 by chceking this and taking the next value as highest)
                    select C;
 
            var WinningHands = x.ToList();
            //Single player with highest hand
            if (WinningHands.Count == 1)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
                var Reason = CasinoRules[WinningHands[0].Value.Value];

                Console.WriteLine("The Winner is : " + WinningHands[0].Key + " [" + Reason + "]");
            }
            else
            {
               
                var Winners = GetHighestCardWinner(PlayerVsCardDetails, WinningHands);

                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("The Winner is : " + Winners);
            }


        }

        public string GetHighestCardWinner(List<Player> Players, List<KeyValuePair<string, KeyValuePair<string, int>>> SelectedWinners)
        {
           // Dictionary<string, string> EligibleWinners = new Dictionary<string, string>();
            //getting only the players who have Flush or any kind and are on tie
            var PlayersInTie = new List<Player>();
            Player temp;
            foreach (var item in SelectedWinners)
            {
                temp = Players.Find(x => x.Name == item.Key);
                PlayersInTie.Add(new Player { Name = temp.Name, Hand = temp.GetSortedCards() });
            }
            var numberOfMatchedPlayers = PlayersInTie.Count;
            var tempCard = new Card();
            var maxCardHolderId = 0;

            var LowerRankPlayers = new List<int>();

            for (int i = 0; i < 5; i++)
            {
                if (LowerRankPlayers.Count == PlayersInTie.Count - 1)
                    break;
                maxCardHolderId = 0;
                //leastCard = PlayersInTie[0].Hand[i];
                for (int j = 1; j < numberOfMatchedPlayers; j++)
                {
                    if (PlayersInTie[maxCardHolderId].Hand[i].CardStrength != PlayersInTie[j].Hand[i].CardStrength)
                    {
                        if (PlayersInTie[maxCardHolderId].Hand[i].CardStrength > PlayersInTie[j].Hand[i].CardStrength) //Not needed to replace
                        {
                            if (!LowerRankPlayers.Contains(j))
                                LowerRankPlayers.Add(j);
                        }
                        else
                        {
                            if (!LowerRankPlayers.Contains(maxCardHolderId))
                                LowerRankPlayers.Add(maxCardHolderId);
                            maxCardHolderId = j;
                        }
                    }
                }
            }

            var PlayersInTieFinal = new List<Player>();
            // Get all the players who are not in the deleted list
            for (int i = 0; i < PlayersInTie.Count; i++)
            {
                if (!LowerRankPlayers.Contains(i))
                    PlayersInTieFinal.Add(PlayersInTie[i]);
            }

           
            var items = (from x in PlayersInTieFinal
                         select x.Name);
            return items.Aggregate((i, j) => i + "," + j);
        }

    }


}
