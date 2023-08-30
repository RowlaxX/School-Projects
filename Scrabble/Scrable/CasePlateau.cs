using System;
using System.Collections.Generic;
using System.Text;

namespace Scrable
{
    class CasePlateau
    {
        public enum Types
        {
            MotTriple,
            MotDouble,
            LettreTriple,
            LettreDouble,
            Normal
        }

        public const char EMPTY = '_';

        public Types Type { get; private set; }
        public int MultiplierMot { get; private set; } = 1;
        public int MultiplierLettre { get; private set; } = 1;
        public char Lettre { get; private set; }
        public int Ligne { get; private set; }
        public int Colonne { get; private set; }
        public Plateau Plateau { get; private set; }

        public CasePlateau(Plateau plateau, int ligne, int colonne) : this(plateau, ligne, colonne, EMPTY) { }

        public CasePlateau(Plateau plateau, int ligne, int colonne, char lettre)
        {
            if (plateau == null)
                throw new ArgumentNullException("plateau est null.");
            if (ligne < 0 || ligne >= Plateau.TAILLE)
                throw new ArgumentOutOfRangeException("Ligne doit être comprise entre 0 et " + Plateau.TAILLE);
            if (colonne < 0 || colonne >= Plateau.TAILLE)
                throw new ArgumentOutOfRangeException("Colonne doit être comprise entre 0 et " + Plateau.TAILLE);

            this.Plateau = plateau;
            this.Colonne = colonne;
            this.Ligne = ligne;
            this.Lettre = lettre;
            this.Type = FindType(ligne, colonne);

            if (Type == Types.MotTriple)
                this.MultiplierMot = 3;
            else if (Type == Types.MotDouble)
                this.MultiplierMot = 2;

            if (Type == Types.LettreTriple)
                this.MultiplierLettre = 3;
            else if (Type == Types.LettreDouble)
                this.MultiplierLettre = 2;
        }

        public void ClearMultiplier()
        {
            this.MultiplierLettre = 1;
            this.MultiplierMot = 1;
        }

        public bool IsEmpty()
        {
            return this.Lettre == EMPTY;
        }

        public void Place(char c)
        {
            if (!IsEmpty())
                throw new ApplicationException("La case est déjà remplie.");
            this.Lettre = c;
        }

        public ColoredString ToColoredString()
        {
            string msg = " " + (IsEmpty() ? " " : Lettre.ToString()) + " ";
            ConsoleColor foreground = ConsoleColor.White;
            ConsoleColor background;

            if (Type == Types.Normal)
                background = ConsoleColor.DarkGreen;
            else if (Type == Types.MotDouble)
                background = ConsoleColor.Magenta;
            else if (Type == Types.MotTriple)
                background = ConsoleColor.Red;
            else if (Type == Types.LettreDouble)
                background = ConsoleColor.Cyan;
            else
                background = ConsoleColor.DarkBlue;

            return new ColoredString(msg, foreground, background);   
        }

        private static Types FindType(int ligne, int colonne)
        {
            //Mot triple
            if (ligne == 0 || ligne == 14)
                if (colonne == 0 || colonne == 14 || colonne == 7)
                    return Types.MotTriple;

            if (ligne == 7)
                if (colonne == 0 || colonne == 14)
                    return Types.MotTriple;

            //Mot double
            if (ligne == colonne || ligne == 14 - colonne)
                if ((ligne >= 1 && ligne <= 4) || ligne == 7 || (ligne >= 10 && ligne <= 13))
                    return Types.MotDouble;

            //Lettre double
            if (ligne == 0 || ligne == 14)
                if (colonne == 3 || colonne == 11)
                    return Types.LettreDouble;

            if (ligne == 2 || ligne == 12)
                if (colonne == 6 || colonne == 8)
                    return Types.LettreDouble;

            if (ligne == 3 || ligne == 11)
                if (colonne == 0 || colonne == 7 || colonne == 14)
                    return Types.LettreDouble;

            if (ligne == 6 || ligne == 8)
                if (colonne == 2 || colonne == 12 || colonne == 6 || colonne == 8)
                    return Types.LettreDouble;

            if (ligne == 7)
                if (colonne == 3 || colonne == 11)
                    return Types.LettreDouble;

            //Lettre triple
            if (ligne == 1 || ligne == 13)
                if (colonne == 5 || colonne == 9)
                    return Types.LettreTriple;

            if (ligne == 5 || ligne == 9)
                if (colonne == 1 || colonne == 13 || colonne == 5 || colonne == 9)
                    return Types.LettreTriple;

            return Types.Normal;
        }
    }
}
