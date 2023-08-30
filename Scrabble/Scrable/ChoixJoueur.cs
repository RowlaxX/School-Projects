using System;
using System.Collections.Generic;
using System.Text;

namespace Scrable
{
    class ChoixJoueur
    {
        public enum Directions
        {
            Vertical,
            Hozizontal
        }

        public string Mot { get; private set; }
        public int Ligne { get; private set; }
        public int Colonne { get; private set; }
        public Directions Direction { get; private set; }
        public Joueur Joueur { get; private set; }

        public ChoixJoueur(Joueur joueur, string mot, int ligne, int colonne, Directions direction)
        {
            if (joueur == null)
                throw new ArgumentNullException("Le joueur est null.");

            if (mot == null)
                throw new ArgumentNullException("Le mot est null.");
            if (mot.Length < 2)
                throw new ArgumentException("Le mot est trop petit.");
            if (mot.Length > 15)
                throw new ArgumentException("Le mot est trop grand.");

            if (ligne < 0 || ligne >= Plateau.TAILLE)
                throw new ArgumentOutOfRangeException("La ligne doit être comprise entre 0 et 15 exclus.");
            if (colonne < 0 || colonne >= Plateau.TAILLE)
                throw new ArgumentOutOfRangeException("La colonne doit être comprise entre 0 et 15 exclus.");

            if (direction == Directions.Vertical && ligne + mot.Length >= Plateau.TAILLE)
                throw new ArgumentException("Le mot dépasse du plateau.");
            if (direction == Directions.Hozizontal && colonne + mot.Length >= Plateau.TAILLE)
                throw new ArgumentException("Le mot dépasse du plateau.");

            this.Joueur = joueur;
            this.Mot = mot.ToUpper();
            this.Ligne = ligne;
            this.Colonne = colonne;
            this.Direction = direction;
        }
    }
}
