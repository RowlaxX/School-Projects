using System;
using System.Collections.Generic;
using System.Text;

namespace Scrable
{
    abstract class Joueur
    {
        public const int QUANTITE_MAIN = 7;

        public string Nom { get; set; }
        public int Score { get; protected set; } = 0;
        private List<string> motsTrouves = new List<string>();
        public List<char> Main { get; private set; } = new List<char>(QUANTITE_MAIN);

        public Joueur(string nom)
        {
            this.Nom = nom;
        }

        public abstract ChoixJoueur Choix(Jeu jeu);

        public void Add_Mot(string mot)
        {
            this.motsTrouves.Add(mot.ToLower());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Nom : " + Nom);
            sb.AppendLine("Score : " + Score);

            sb.Append("Mots trouvés : " + motsTrouves);
            foreach (string mot in motsTrouves)
                sb.Append(mot + ", ");
            sb.Remove(sb.Length - 2, sb.Length);
            sb.AppendLine();

            sb.Append("Main : ");
            foreach (char lettre in Main)
                sb.Append(lettre + ", ");
            sb.Remove(sb.Length - 2, sb.Length);
            sb.AppendLine();

            return sb.ToString();
        }

        public int Add_Score(int valeur)
        {
            if (valeur <= 0)
                throw new ArgumentOutOfRangeException("valeur must be greater than 0.");
            return Score += valeur;
        }

        public void Add_Main_Courante(Jeton jeton)
        {
            Add_Main_Courante(jeton.Lettre);
        }

        public void Add_Main_Courante(char lettre)
        {
            if (Nombre_Jeton_Main() >= QUANTITE_MAIN)
                throw new Exception("Il n'y a plus de place dans la main du joueur");
            this.Main.Add(lettre.ToString().ToUpper()[0]);
        }

        public bool Remove_Main_Courante(char lettre)
        {
            return this.Main.Remove(lettre);
        }

        public bool Contains_Main_Courante(char lettre)
        {
            return this.Main.Contains(lettre);
        }

        public bool Contains_Main_Courante(List<char> lettres)
        {
            List<char> copy = new List<char>(this.Main);
            foreach (char lettre in lettres)
                if (!copy.Remove(lettre.ToString().ToUpper()[0] ))
                    return false;
            return true;
        }

        public int Nombre_Jeton_Main()
        {
            return Main.Count;
        }
    }
}
