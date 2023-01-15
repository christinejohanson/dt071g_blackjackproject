/* 
BLACK JACK GAME
Code by christine johanson chjo2104
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    class Program
    {
        //namn
        static string name = "Kicki";
        //startpeng
        const double startMoney = 1000.00;
        //spelpott
        static double playMoney = startMoney;
        static string[] cardSuits = { "♣", "♦", "♠", "♥" };
        static string[] playingCards = { "Ess", "Två", "Tre", "Fyra", "Fem", "Sex", "Sju", "Åtta", "Nio", "Tio", "Knekt", "Dam", "Kung" };

        //whileloop för fortsatt spel 
        static bool isGoing = true;

        //listor
        static List<int> playerCardScores = new List<int>();
        static List<int> dealerCardScores = new List<int>();

        static int playerTotalCardScore = 0;
        static int dealerTotalCardScore = 0;
        //vad varje spelomgång kostar
        static int bettingMoney = 50;

        static void Main(string[] args)
        {
            //anropa metod välkommen
            Printwelcome();

            //fråga efter namn
            Namesetting();

            while(isGoing)
            {
                //skriv ut meny och fråga efter menyval
                MenuChoice();

                //metod för nytt spel
                TheGame();
                //Console.Clear();
            }
        }

        private static void TheGame()
        {
            Console.WriteLine("Gör menyval och avsluta med <enter>");
            //svaret från menyval
            string menuChoice = Console.ReadLine();
            //switchsats för menyval
            switch (menuChoice)
            {
                case "1":
                    NewRound();
                    break;

                case "2":
                    Console.WriteLine("Avslutar Black Jack");
                    isGoing = false;
                    break;
            }
        }

        //ny runda
        private static void NewRound()
        {
            //kollar om spelaren har råd att spela
            if (bettingMoney > playMoney)
            {
                Console.WriteLine("Nä nu har du inte råd att spela mer. Börja om från början");
                Console.WriteLine("Tryck på valfri tangent för att avsluta...");
                Console.ReadKey();
            }

            Console.WriteLine("Dealern blandar korten...");

            //rensar scores inför ny runda.
            playerCardScores.Clear();
            dealerCardScores.Clear();
            playerTotalCardScore = 0;
            dealerTotalCardScore = 0;

            //metod hitrandom, ge kort till dealern
            HitRandom("Dealer");

            bool canHit = true;
            while(canHit)
            {
                HitRandom();
                //metoden canhitagain för att dra fler kort
                canHit = CanHitAgain();
            }

            //om dealern har mindre än 17, dra kort. 
            while(dealerTotalCardScore < 17)
            {
                HitRandom("Dealer");
            }

            //för att skriva ut totalpoäng för både spelare
            PrintTotalScore();
            PrintTotalScore("Dealer");

            //resultatmetod
            CalculateThisResult();
        }

        //om totalen är under 21
        private static bool CanHitAgain()
        {
            if(playerTotalCardScore < 21)
            {
                Console.WriteLine("Vill du ha ett till kort?\n1. yes 2. no");
                var hitAgain = Console.ReadLine();
                if(hitAgain == "1")
                {
                    //om spelaren vill ha ett till kort returnera true. 
                    return true;
                }
            }
            // totalen inte är mindre än 21. 
            return false;
        }

        //metod för att räkna ihop resultatet
        private static void CalculateThisResult()
        {
            //om det är mer än 21 eller lägre han än dealerns.
            if (playerTotalCardScore > 21 || playerTotalCardScore <= dealerTotalCardScore && dealerTotalCardScore < 22)
            //if (playerTotalCardScore > 21)
            {
                playMoney -= bettingMoney;
                //metod om spelaren förlorar
                PrintRoundLost();
            } else
            {                       
            playMoney += bettingMoney;
            //metod om spelaren vinner
            PrintRoundWon();
            }
        }

        //metod om spelaren vinner
        private static void PrintRoundWon()
        {
            Console.WriteLine($"YOU WIN... {bettingMoney} kr. Din pott är nu {playMoney} kr.\n Tryck valfri tangent för att fortsätta.");
            Console.ReadKey();
        }

        //metod om spelaren förlorar
        private static void PrintRoundLost()
        {
            Console.WriteLine($"GAME OVER... \nDu förlorade {bettingMoney} kr! Din pott är nu {playMoney} kr.\nTryck valfri tangent för att fortsätta");
            Console.ReadKey();
        }

        //metod för att skriva ut totalpoäng
        private static void PrintTotalScore(string cardRole = "Player")
        {
            if (cardRole == "Player")
            {
                Console.WriteLine($"{cardRole} totala poäng är: {playerTotalCardScore}");
            }
            else
            {
                Console.WriteLine($"{cardRole} totala poäng är: {dealerTotalCardScore}");
            }
        }

        //method för att generera random kort. 
        private static void HitRandom(string cardRole = "Player")
        {
            //Skapa en randomerare som kan slumpa tal med
            var randomNumber = new Random();
            //random cardfärg
            var cardSuit = cardSuits[randomNumber.Next(cardSuits.Length)];
            var playingCardIndex = randomNumber.Next(playingCards.Length);
            var playingCard = playingCards[playingCardIndex];
            int cardScore;
            int totalCardScore;
            List<int> cardScores;

            //om det är ett ess
            if(playingCardIndex == 0)
            {
                cardScore = 11;
            }
            //om det är 2-9
            else if(playingCardIndex < 9)
            {
                cardScore = playingCardIndex + 1;
            } 
            //de övriga klädda korten får värde 10.
            else
            {
                cardScore = 10;
            }

            //om det är player
            if(cardRole == "Player")
            {
                playerCardScores.Add(cardScore);
                CalculateCardHit();
                //räknar ut tottipoäng
                totalCardScore = playerTotalCardScore;
                cardScores = playerCardScores;
            }
            //annars är det ju dealer
            else
            {
                dealerCardScores.Add(cardScore);
                CalculateCardHit("Dealer");
                //räknar ut tottipoäng
                totalCardScore = dealerTotalCardScore;
                cardScores = dealerCardScores;
            }

            Console.WriteLine($"\n{cardRole} drar ett kort. Kortet är {cardSuit}{playingCard}");
            //listar alla dragna kort och skriver ut i foreachloop
            Console.Write("Dragna kort: |");
            foreach(var card in cardScores)
            {
                Console.Write($" {card} |");
            }
            Console.WriteLine($"\n{cardRole} drog | {cardSuit}{playingCard} | ({cardScore}).");
            Console.WriteLine($"[{cardRole}] -> Totala kortpoängen är: {totalCardScore}\n");

        }

        //metod för att räkna ut kortsumma
        private static void CalculateCardHit(string cardRole = "Player")
        {
            //om cardrole är player
            if (cardRole == "Player")
            {
                playerTotalCardScore = CalculateCurrentTotalCardScore(playerCardScores);
            }
            //annars är cardrole dealer
            else
            {
                dealerTotalCardScore = CalculateCurrentTotalCardScore(dealerCardScores);
            }
        }

        //metod om esset måste bli 1 istället för 11. 
        private static int CalculateCurrentTotalCardScore(List<int> cardScores)
        {
            var totalCardScore = cardScores.Sum();
            //om summan är över 21 och esset måste bli 1 istället för 11. 
            if(totalCardScore > 21)
            {
                //om esset fått värde 11. 
                var aceCard = cardScores.FirstOrDefault(cs => cs == 11);
                cardScores.Remove(aceCard);
                //1 istället för 11. 
                cardScores.Add(1);
                totalCardScore = cardScores.Sum();
                
            }
            return totalCardScore;
        }

        private static void MenuChoice()
        {
            Console.WriteLine("\n-------- MENY --------");
            Console.WriteLine("1. Nytt spel");
            Console.WriteLine("2. Exit");         
        }

        private static void Namesetting()
        {
            //fråga efter spelarens namn
            Console.WriteLine("Fyll i ditt namn, avsluta med <enter>");
            name = Console.ReadLine();
            //kontrollera om namn är ifyllt o sätt till anonymt.
            if (name == "")
            {
                name = "John Doe";
            }
            Console.WriteLine($"Hej {name}, ditt saldo är {playMoney} kronor. \nVarje spel kostar {bettingMoney} kronor.");
        }

        private static void Printwelcome()
        {
            //färg på text och bakgrund
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            //titel
            Console.Title = "Black Jack Game";
            Console.WriteLine("Välkommen till Black Jack!");
        }
    }
}

