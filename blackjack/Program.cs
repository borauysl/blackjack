using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> playerHand = new List<int>();
            List<int> dealerHand = new List<int>();

            List<int> deck = GenerateShuffledDeck();
            int chips = 100;

            while (true)
            {
                Console.WriteLine("\nJeton Sayısı: " + chips);
                int bet = PlaceBet(chips);

                playerHand.Clear();
                dealerHand.Clear();

                playerHand.Add(DealCard(deck));
                dealerHand.Add(DealCard(deck));
                playerHand.Add(DealCard(deck));
                dealerHand.Add(DealCard(deck));

                Console.WriteLine("\nOyuncunun Kartları:");
                DisplayHand(playerHand);

                Console.WriteLine("\nKurpiyerin Kartları:");
                Console.WriteLine("Kart 1: " + dealerHand[0]);
                Console.WriteLine("Kart 2: Gizli");

                int playerTotal = CalculateHandTotal(playerHand);

                while (true)
                {
                    if (playerTotal > 21)
                    {
                        Console.WriteLine("Oyuncu Patladı! Toplam Değer: " + playerTotal);
                        chips -= bet;
                        break;
                    }

                    if (playerTotal == 21)
                    {
                        Console.WriteLine("Toplam değer 21 olduğu için kart alamazsınız.");
                        break;
                    }

                    Console.Write("\nHit (H) | Stand (S) | Double (D): ");
                    string choice = Console.ReadLine().ToUpper();

                    if (choice == "H")
                    {
                        int nextCardValue = DealCard(deck);
                        playerHand.Add(nextCardValue);
                        Console.WriteLine("Yeni Kart: " + nextCardValue);
                        playerTotal = CalculateHandTotal(playerHand);
                        Console.WriteLine("Toplam Değer: " + playerTotal);
                    }
                    else if (choice == "S")
                    {
                        break;
                    }
                    else if (choice == "D")
                    {
                        bet *= 2;
                        int nextCardValue = DealCard(deck);
                        playerHand.Add(nextCardValue);
                        Console.WriteLine("Yeni Kart: " + nextCardValue);
                        playerTotal = CalculateHandTotal(playerHand);
                        Console.WriteLine("Toplam Değer: " + playerTotal);
                        break;
                    }
                }

                int dealerTotal = CalculateHandTotal(dealerHand);
                Console.WriteLine("\nKurpiyerin Kartları:");
                DisplayHand(dealerHand);
                Console.WriteLine("Toplam Değer: " + dealerTotal);

                while (dealerTotal < 17)
                {
                    dealerHand.Add(DealCard(deck));
                    dealerTotal = CalculateHandTotal(dealerHand);
                    Console.WriteLine("Kurpiyer Yeni Kart: " + dealerHand[dealerHand.Count - 1]);
                    Console.WriteLine("Toplam Değer: " + dealerTotal);
                }

                if (dealerTotal > 21 || (playerTotal <= 21 && playerTotal > dealerTotal))
                {
                    Console.WriteLine("\nOyuncu Kazandı!");
                    chips += bet;
                }
                else if (playerTotal == dealerTotal)
                {
                    Console.WriteLine("\nBerabere!");
                }
                else
                {
                    Console.WriteLine("\nKurpiyer Kazandı!");
                    chips -= bet;
                }

                if (chips <= 0)
                {
                    Console.WriteLine("Jetonunuz bitti çıkmak için bir tuşa basın...");
                    Console.ReadKey();
                    break;
                }

                Console.WriteLine("\nDevam etmek için Enter'a basın, çıkmak için 'Q' tuşuna basın.");
                if (Console.ReadLine().ToUpper() == "Q")
                    break;
            }
        }

        static int PlaceBet(int chips)
        {
            while (true)
            {
                Console.Write("\nBahis miktarını girin (Mevcut Jeton: " + chips + "): ");
                if (int.TryParse(Console.ReadLine(), out int bet))
                {
                    if (bet > 0 && bet <= chips)
                        return bet;
                    else
                        Console.WriteLine("Geçersiz bahis miktarı!");
                }
                else
                {
                    Console.WriteLine("Geçersiz bir değer girdiniz!");
                }
            }
        }

        static int CalculateHandTotal(List<int> hand)
        {
            int total = 0;
            int aceCount = 0;

            foreach (int card in hand)
            {
                if (card == 1)
                {
                    aceCount++;
                    total += 11;
                }
                else if (card > 10)
                {
                    total += 10;
                }
                else
                {
                    total += card;
                }
            }

            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            return total;
        }

        static int DealCard(List<int> deck)
        {
            int card = deck[0];
            deck.RemoveAt(0);
            return card;
        }

        static List<int> GenerateShuffledDeck()
        {
            List<int> deck = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    deck.Add(j);
                }
            }

            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }

            return deck;
        }

        static void DisplayHand(List<int> hand)
        {
            foreach (int card in hand)
            {
                Console.Write(card + " ");
            }
            Console.WriteLine();
        }
    }
}