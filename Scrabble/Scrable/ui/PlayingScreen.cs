using System;
using System.Collections.Generic;
using System.Text;

namespace Scrable.ui
{
    class PlayingScreen : Screen
    {

        private Jeu jeu;
        private int cursorX = 0;
        private int cursorY = 0;
        public int PosX { get; set; } = 0;
        public int PosY { get; set; } = 0;
        public string Erreur { get; set; } = null;

        public PlayingScreen(Jeu jeu)
        {
            if (jeu == null)
                throw new ArgumentNullException("jeu may not be null.");
            this.jeu = jeu;
        }

        public new void Print()
        {
            base.Print();
            Console.SetCursorPosition(cursorX, cursorY);
        }

        public new void Update()
        {
            Update(0);
        }

        public void Update(int step)
        {
            //On ajoute le plateau
            base.Update();

            Pixel[,] plateau = jeu.Plateau.ToPixels();
            int startPlateauX = (Screen.Width - plateau.GetLength(1)) / 2 - 10;
            int startPlateauY = (Screen.Height - plateau.GetLength(0)) / 2 - 4;
            Draw(plateau, startPlateauY, startPlateauX);

            //On ajoute la liste des joueurs et leurs score
            int startJoueurX = startPlateauX + plateau.GetLength(1) + 5;
            int startJoueurY = startPlateauY + (plateau.GetLength(0) - jeu.Joueurs.Count) / 2;
            ColoredString str;
            string nom;
            int score;
            for (int i = 0; i < jeu.Joueurs.Count; i++)
            {
                nom = jeu.Joueurs[i].Nom;
                score = jeu.Joueurs[i].Score;
                if (nom.Length > 15)
                    str = new ColoredString(nom.Substring(0, 12) + "...");
                else
                    str = new ColoredString(nom);

                str.ForegroundColor = ConsoleColor.Yellow;
                str.BackgroundColor = ConsoleColor.Black;
                Draw(str, startJoueurY + i, startJoueurX);

                str.Message = ": " + score;
                Draw(str, startJoueurY + i, startJoueurX + 16);
            }

            //On ajoute le nombre de jeton restant
            ColoredString restant = new ColoredString("Jetons restants : " + jeu.Sac_Jetons.NombreJetons(), ConsoleColor.Blue, ConsoleColor.Black);
            Draw(restant, startPlateauY, startJoueurX);

            //On ajoute la liste des jetons en mains
            int startPlayerY = startPlateauY + Plateau.TAILLE + 2;
            int startPlayerX = startPlateauX - 5;
            ColoredString cstr = new ColoredString("Au tour de " + jeu.GetPlayingPlayer().Nom + ".", ConsoleColor.Green, ConsoleColor.Black);
            ColoredString cstr2 = new ColoredString("Jetons en main : ", ConsoleColor.DarkGreen, ConsoleColor.Black);
            StringBuilder msg = new StringBuilder();
            foreach (char jeton in jeu.GetPlayingPlayer().Main)
                msg.Append(jeton + " ");
            ColoredString cstr3 = new ColoredString(msg.ToString(), ConsoleColor.Green, ConsoleColor.Black);

            int startJetonX = (Width - cstr3.Length) / 2;
            Draw(cstr, startPlayerY, startPlayerX);
            Draw(cstr2, startPlayerY + 1, startPlayerX);
            Draw(cstr3, startPlayerY + 3, startPlayerX + 5);

            int startChoixX = startJoueurX + 5;
            int startChoixY = startJoueurY + 15;
            ColoredString choix = null;

            if (step == 0)
                choix = new ColoredString("Votre mot :", ConsoleColor.Cyan, ConsoleColor.Black);
            else if (step == 1)
                choix = new ColoredString("Direction :", ConsoleColor.Cyan, ConsoleColor.Black);
            else if (step == 2)
            {
                choix = new ColoredString("Position  :", ConsoleColor.Cyan, ConsoleColor.Black);
                for (int i = 0; i < 3; i++)
                    PixelAt(startPlateauY + PosY, startPlateauX + PosX * 3 + i).BackgroundColor = ConsoleColor.Gray;
            }

            if (choix != null)
            {
                Draw(choix, startChoixY, startChoixX);
                this.cursorX = startChoixX + choix.Length + 1;
                this.cursorY = startChoixY;
            }

            if (step == 0)
            {
                ColoredString hint = new ColoredString("Entrer \"-\" pour passer votre tour.", ConsoleColor.DarkGray, ConsoleColor.Black);
                Draw(hint, Height - 2, 15);
            }
            if (step == 1)
            {
                ColoredString hint = new ColoredString("Appuyez sur ces touches pour choisir la direction :", ConsoleColor.DarkGray, ConsoleColor.Black);
                ColoredString hint2 = new ColoredString("h pour horizontal", ConsoleColor.DarkGray, ConsoleColor.Black);
                ColoredString hint3 = new ColoredString("v pour vertical", ConsoleColor.DarkGray, ConsoleColor.Black);
                Draw(hint, Height - 4, 15);
                Draw(hint2, Height - 3, 18);
                Draw(hint3, Height - 2, 18);
            }
            else if (step == 2)
            {
                ColoredString hint = new ColoredString("Utiliser les flèches pour choisir la position :", ConsoleColor.DarkGray, ConsoleColor.Black);
                Draw(hint, Height - 2, 15);
            }

            //On ajoute le message d'erreur
            if (this.Erreur != null)
            {
                ColoredString error = new ColoredString("Erreur : " + this.Erreur, ConsoleColor.Red, ConsoleColor.Black);
                int startErrorX = (Width - Erreur.Length) / 2 - 15;
                Draw(error, startPlateauY - 2, startErrorX);
                this.Erreur = null;
            }
        }
    }
}
