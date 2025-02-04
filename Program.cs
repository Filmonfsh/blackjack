// Blackjack-korttipeli 

using System;
using System.Collections.Generic;

// Kortin määrittely
class Card
{
    public string Suit { get; set; }
    public string Rank { get; set; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

// Pakka-luokka
class Deck
{
    private List<Card> cards;
    private static string[] suits = { "H", "D", "C", "S" };
    private static string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

    public Deck()
    {
        cards = new List<Card>();
        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        Random random = new Random();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    public Card Deal()
    {
        if (cards.Count == 0) throw new InvalidOperationException("Pakka on tyhjä!");
        var card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}

// Pelaaja-luokka
class Player
{
    public List<Card> Hand { get; private set; } = new List<Card>();

    public void AddCard(Card card)
    {
        Hand.Add(card);
    }

    public int CalculateHandValue()
    {
        int total = 0;
        int aceCount = 0;
        foreach (var card in Hand)
        {
            if (int.TryParse(card.Rank, out int value))
            {
                total += value;
            }
            else if (card.Rank == "A")
            {
                aceCount++;
                total += 11;
            }
            else
            {
                total += 10;
            }
        }

        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }
}

// Pääohjelma
class Program
{
    static void Main()
    {
        Console.WriteLine("Tervetuloa Blackjack-peliin!");

        Deck deck = new Deck();
        deck.Shuffle();

        Player player = new Player();
        Player dealer = new Player();

        // Pelaajalle ja jakajalle jaetaan kaksi korttia
        player.AddCard(deck.Deal());
        player.AddCard(deck.Deal());
        dealer.AddCard(deck.Deal());
        dealer.AddCard(deck.Deal());

        // Näytetään pelaajan ja jakajan käsi
        Console.WriteLine("\nKorttisi:");
        foreach (var card in player.Hand)
        {
            Console.WriteLine(card);
        }
        Console.WriteLine($"Kätesi arvo: {player.CalculateHandValue()}");

        Console.WriteLine("\nJakajan ensimmäinen kortti:");
        Console.WriteLine(dealer.Hand[0]);

        // Pelaajan vuoro
        while (true)
        {
            Console.WriteLine("\nValitse: (H)it tai (S)tand");
            string choice = Console.ReadLine()?.ToUpper();

            if (choice == "H")
            {
                player.AddCard(deck.Deal());
                Console.WriteLine("Sait uuden kortin:");
                Console.WriteLine(player.Hand[^1]);

                int playerValue = player.CalculateHandValue();
                Console.WriteLine($"Kätesi arvo: {playerValue}");

                if (playerValue > 21)
                {
                    Console.WriteLine("Ylitit 21! Hävisit pelin.");
                    return;
                }
            }
            else if (choice == "S")
            {
                break;
            }
            else
            {
                Console.WriteLine("Virheellinen valinta, yritä uudelleen.");
            }
        }

        // Jakajan vuoro
        Console.WriteLine("\nJakajan vuoro:");
        foreach (var card in dealer.Hand)
        {
            Console.WriteLine(card);
        }

        while (dealer.CalculateHandValue() < 17)
        {
            Console.WriteLine("Jakaja nostaa kortin...");
            dealer.AddCard(deck.Deal());
            Console.WriteLine(dealer.Hand[^1]);
        }

        int dealerValue = dealer.CalculateHandValue();
        Console.WriteLine($"Jakajan käden arvo: {dealerValue}");

        // Lopputulos
        int playerValueFinal = player.CalculateHandValue();
        if (dealerValue > 21 || playerValueFinal > dealerValue)
        {
            Console.WriteLine("\nOnneksi olkoon, voitit pelin!");
        }
        else if (playerValueFinal == dealerValue)
        {
            Console.WriteLine("\nTasapeli!");
        }
        else
        {
            Console.WriteLine("\nHävisit pelin!");
        }
    }
}
