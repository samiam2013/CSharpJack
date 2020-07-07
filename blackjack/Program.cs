using System;

namespace blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            deck.Shuffle();
            deck.Shuffle();
            //deck.PrintDeck();

            Hand userHand = new Hand(deck.deck, deck.position);
            deck.position += 2;
            Hand compHand = new Hand(deck.deck, deck.position);
            deck.position += 2;

            int score = userHand.Score();
            int compScore = compHand.Score();
            if(score == 21){
                if(compScore == 21){
                    Console.WriteLine("Draw @ 21!");
                } else {
                    Console.WriteLine("21! You win! computer score: "+compScore);
                }
            } else {
                //bool lost = false;
                while(score < 21){
                    userHand.Print();
                    int moved = userHand.Move(deck.deck, deck.position, false);
                    if (moved > 0){
                        deck.position += moved;
                    } else {
                        break;
                    }
                    score = userHand.Score();
                    if(score > 21){
                        break;
                    }
                }
                while(compScore < 17){
                    compHand.Move(deck.deck, deck.position, true);
                    deck.position++;
                    compScore = compHand.Score();
                }
            }
            if (score < 22){
                if (compScore > 21){
                    Console.WriteLine("Computer Bust! You Win!");
                } else if (score > compScore){
                    Console.WriteLine("You win!");
                } else {
                    Console.WriteLine("You lost :(");
                }
            } else {
                Console.WriteLine("Bust!");
            }
            Console.WriteLine("End of game. Your score: "+score+"  Computer Score: "+compScore);
           
        }
    }

    class Hand
    {
        public Card[] hand = new Card[11];
        private int handPos;
        public Hand(Card[] deck, int deckPos){
            hand[0] = deck[deckPos];
            hand[1] = deck[deckPos+1];
            for(int i = 2; i < hand.Length; i++){
                hand[i] = null;
            }
            handPos = 2;
        }

        public void Print(){
            Console.WriteLine("Your cards: ");
            int i = 0;
            while(hand[i] != null){
                Console.WriteLine(hand[i].rank+" of "+hand[i].suit);
                i++;
            }
        }

        //returns the amount of cards taken from deck
        public int Move(Card[] deck, int deckPos, bool computer){
            if(!computer){
                Console.Write("[H]it or [P]ass?: ");
                string input = Console.ReadLine();
                input = input.ToLower();
                if(input == "p" || input == "pass"){
                    return 0;
                } else if(input == "h" || input == "hit"){
                    hand[handPos] = deck[deckPos];
                    handPos++;
                    Print();
                    return 1;
                } else {
                    Console.WriteLine("Invalid input!");
                    return Move(deck,deckPos,false);
                }
            } else {
                hand[handPos] = deck[deckPos];
                handPos++;
                return 1;
            }

        }

        public int Score(){
            int sum = 0;
            int i = 0;
            int aces = 0;
            while(hand[i] != null){
                string cardRank = hand[i].rank;
                i++;
                switch(cardRank)
                {
                    case "ace":
                        aces++;
                        break;
                    case "two":
                        sum += 2;
                        break;
                    case "three":
                        sum += 3;
                        break;
                    case "four":
                        sum += 4;
                        break;
                    case "five":
                        sum += 5;
                        break;
                    case "six":
                        sum += 6;
                        break;
                    case "seven":
                        sum += 7;
                        break;
                    case "eight":
                        sum += 8;
                        break;
                    case "nine":
                        sum += 9;
                        break;
                    default:
                        sum += 10;
                        break;
                }
            }
            
            if(aces > 0){
                if(aces == 1){
                    if(sum > 11){
                        return sum + aces;
                    } else {
                        return sum + 11;
                    }
                } else {
                    return sum + 11 + (aces - 1);
                }
            }
            return sum;
        }
    }

    class Deck
    {
        public Card[] deck = new Card[48];
        public int position = 0;
        public Deck()
        {
            string[] suits = { "spades", "hearts", "clubs", "diamonds" };
            string[] ranks = { "ace", "two", "three", "four", "five", "six", 
                "seven", "eight", "nine", "jack", "queen", "king"};

            for (int s = 0; s < suits.Length; s++){
                for(int r = 0; r < ranks.Length; r++ ){
                    int cardNumber = (s * ranks.Length) + r;
                    deck[cardNumber] = new Card(suits[s],ranks[r]);
                }
            }
        }

        public void PrintDeck ()
        {
            for(int i = 0; i < deck.Length; i++){
                Console.WriteLine(deck[i].rank+" of "+deck[i].suit);
            }
        }

        static Random random = new Random();
        public void Shuffle()
        {
            int len = deck.Length;
            for (int i = 0; i < len; i++)
            {
                int rand = i + random.Next(len - i);
                Card card = deck[rand];
                deck[rand] = deck[i];
                deck[i] = card;
            }
        }
    }

    class Card
    {
        public string suit;
        public string rank;
        public Card(string suitName, string rankName){
            suit = suitName;
            rank = rankName;
        }
    }
}
