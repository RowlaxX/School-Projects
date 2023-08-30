using System;
using System.Collections.Generic;
using System.Text;

namespace Scrable
{
    class Jeton
    {

        //Variables d'instances
        public char Lettre { get; private set; }
        public int Point {
            get
            {
                return this.point;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value must be positiv.");
                this.point = value;
            }
        }
        public int Quantite {
            get {
                return this.quantite;
            } 
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value must be positiv.");
                this.quantite = value;
            }
        }

        private int quantite;
        private int point;

        //Constructeurs
        public Jeton(char lettre, int point, int quantite)
        {
            this.Lettre = lettre;
            this.Point = point;
            this.Quantite = quantite;
        }

        public Jeton(string ligne)
        {
            if (ligne == null)
                throw new ArgumentNullException("ligne est null.");

            string[] tab = ligne.Split(";");
            this.Lettre = tab[0][0];
            this.Point = Convert.ToInt32(tab[1]);
            this.Quantite = Convert.ToInt32(tab[2]);
        }

        //Methodes
        public int Prendre()
        {
            return --this.Quantite;
        }

        public bool HasMore()
        {
            return Quantite > 0;
        }

        public override string ToString()
        {
            return "lettre : " + Lettre + "   point : " + Point + "   quantité : " + Quantite;  
        }

    }
}
