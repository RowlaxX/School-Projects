using Scrable.ui;
using System;
using System.Text;
using System.Threading;

namespace Scrable
{
    class Program
    {
        public static bool PromptYesNo()
        {
            char answer = Console.ReadKey().KeyChar;
            return answer == 'o' || answer == 'O'; 
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Jeu.Builder builder = PromptBuilder();
            
            do
            {
                Console.WriteLine("Création de la partie...");
                builder.BuildGame().Start();
                Console.WriteLine("Appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Voulez-vous rejouer avec les mêmes paramètres ? (o/n)");
            } while (PromptYesNo());
        }

        static Jeu.Builder PromptBuilder()
        {
            Jeu.Builder builder = Jeu.Builder.newBuilder()
                .PathDictionnaire("Francais.txt")
                .Langue("Français")
                .PathSacJeton("Jetons.txt");
            PromptPlayers(builder);
            return builder;
        }

        static void PromptPlayers(Jeu.Builder builder)
        {
            int nombreJoueur = Jeu.Builder.MIN_JOUEURS;
            Console.WriteLine("Veuillez saisir le nombre de joueurs (Entre " + Jeu.Builder.MIN_JOUEURS + " et " + Jeu.Builder.MAX_JOUEURS + ").");
            do
            {
                if (nombreJoueur != Jeu.Builder.MIN_JOUEURS)
                    Console.WriteLine("Erreur : la saisie est incorrect.");
                nombreJoueur = Convert.ToInt32(Console.ReadLine());
            } while (nombreJoueur < Jeu.Builder.MIN_JOUEURS || nombreJoueur > Jeu.Builder.MAX_JOUEURS);
            builder.NombreJoueur(nombreJoueur);

            string input;
            for (int i = 0; i < nombreJoueur; i++)
            {
                Console.WriteLine("Entrer le nom du joueur " + (i + 1) + " (Laissez vide pour ajouter des IAs) : ");
                input = Console.ReadLine();

                if (input.Length == 0)
                    break;
                builder.AjouterJoueur(input);
            }
        }
    }
}
