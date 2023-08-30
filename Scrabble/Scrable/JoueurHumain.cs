using Scrable.ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrable
{
    class JoueurHumain : Joueur
    {
        public static List<JoueurHumain> FromFile(string path)
        {
            string[] line = File.ReadAllLines(path);
            List<JoueurHumain> joueurs = new List<JoueurHumain>(line.Length / 3);
            
            for (int i = 0; i < line.Length; i += 3)
                joueurs.Add(FromLines(line[i], line[i + 1], line[i + 2]));

            return joueurs;
        }

        private static JoueurHumain FromLines(string line1, string line2, string line3)
        {
            string[] nomEtScore = line1.Split(";");

            JoueurHumain joueur = new JoueurHumain(nomEtScore[0])
            {
                Score = Convert.ToInt32(nomEtScore[1])
            };

            foreach (string mot in line2.Split(";"))
                joueur.Add_Mot(mot);

            foreach (string lettre in line3.Split(";"))
                joueur.Add_Main_Courante(lettre[0]);

            return joueur;
        }

        public JoueurHumain(string nom) : base(nom) { }

        public override ChoixJoueur Choix(Jeu jeu)
        {
            PlayingScreen ps = jeu.PlayingScreen;

            ps.Update(0);
            ps.Print();
            string mot = Console.ReadLine();
            if (mot == "-")
                return null;
            ps.Update(1);
            ps.Print();
            char direction = ' ';
            while (direction != 'h' && direction != 'v')
                direction = Console.ReadKey().KeyChar.ToString().ToLower()[0];

            ps.PosX = 7;
            ps.PosY = 7;
            ps.Update(2);
            ps.Print();
            ConsoleKey key;
            while ((key = Console.ReadKey().Key) != ConsoleKey.Enter)
            {
                if (key == ConsoleKey.LeftArrow)
                    ps.PosX = ps.PosX == 0 ? 14 : ps.PosX - 1;
                if (key == ConsoleKey.RightArrow)
                    ps.PosX = ps.PosX == 14 ? 0 : ps.PosX + 1;
                if (key == ConsoleKey.UpArrow)
                    ps.PosY = ps.PosY == 0 ? 14 : ps.PosY - 1;
                if (key == ConsoleKey.DownArrow)
                    ps.PosY = ps.PosY == 14 ? 0 : ps.PosY + 1;

                ps.Update(2);
                ps.Print();
            }

            ChoixJoueur.Directions d = direction == 'h' ? ChoixJoueur.Directions.Hozizontal : ChoixJoueur.Directions.Vertical;
            return new ChoixJoueur(this, mot, ps.PosY, ps.PosX, d);
        }

        private static int RequestInput(int min, int max)
        {
            int input = Int32.MinValue;
            while (input < min || input > max)
                try
                {
                    input = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Veuillez entrer un nombre entre 1 et " + Plateau.TAILLE);
                    Console.ForegroundColor = ConsoleColor.Black;
                }

            return input;
        }
    }
}
