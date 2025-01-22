// Blackjack-korttipeli C#-ohjelma


using System;
using System.Collections.Generic;

// Kortin määrittely
class Card
{
    public string Suit { get; set; } // Kortin maa (esim. "H" = Hearts)
    public string Rank { get; set; } // Kortin arvo (esim. "A" = Ace)

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }

    // Palauttaa kortin kuvauksen
    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

// Pakka-luokka
class Deck
{
    private List<Card> cards; // Lista kaikista korteista
    private static string[] suits = { "H", "D", "C", "S" }; // Hearts, Diamonds, Clubs, Spades
    private static string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" }; // Korttien arvot

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

    // Sekoittaa pakan
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

    // Jakaa kortin pakasta
    public Card Deal()
    {
        if (cards.Count == 0) throw new InvalidOperationException("Pakka on tyhjä!");
        var card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    // Palauttaa jäljellä olevien korttien määrän
    public int RemainingCards()
    {
        return cards.Count;
    }
}

// Pelaaja-luokka
class Player
{
    public List<Card> Hand { get; private set; } = new List<Card>(); // Pelaajan kädessä olevat kortit

    public void AddCard(Card card)
    {
        Hand.Add(card);
    }

    // Laskee pelaajan käden arvon
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
                total += 10; // J, Q, K = 10
            }
        }

        // Muuntaa ässien arvon, jos summa ylittää 21
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

        Deck deck = new Deck(); // Luo pakka
        deck.Shuffle(); // Sekoittaa pakan

        Player player = new Player(); // Luo pelaaja

        // Pelaajalle jaetaan kaksi korttia
        player.AddCard(deck.Deal());
        player.AddCard(deck.Deal());

        // Näyttää pelaajan kortit
        Console.WriteLine("Korttisi ovat:");
        foreach (var card in player.Hand)
        {
            Console.WriteLine(card);
        }

        // Näyttää pelaajan käden arvon
        Console.WriteLine($"Kätesi arvo: {player.CalculateHandValue()}");

        // Näyttää jäljellä olevien korttien määrän
        Console.WriteLine($"Pakkaan jääneiden korttien määrä: {deck.RemainingCards()}");
    }
}
