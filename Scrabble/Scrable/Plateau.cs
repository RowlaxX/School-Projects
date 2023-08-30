using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrable
{
    class Plateau
    {
        public const int TAILLE = 15;

        public static Plateau FromFile(Jeu jeu, string path)
        {
            Plateau instance = new Plateau(jeu);

            string[] lines = File.ReadAllLines(path);
            string[] temp;
            for (int i = 0; i < TAILLE; i++)
            {
                temp = lines[i].Split(";");
                for (int j = 0; j < temp.Length; j++)
                {
                    instance.plateau[i, j] = new CasePlateau(instance, i, j, temp[j][0]);
                    if (!instance.plateau[i, j].IsEmpty())
                        instance.placed++;
                }
            }

            return instance;
        }

        private Jeu jeu;
        private CasePlateau[,] plateau = new CasePlateau[TAILLE,TAILLE];

        private int placed = 0;

        public Plateau(Jeu jeu)
        {
            if (jeu == null)
                throw new NullReferenceException("jeu may not be null.");

            this.jeu = jeu;
            for (int i = 0; i < TAILLE; i++)
                for (int j = 0; j < TAILLE; j++)
                    plateau[i, j] = new CasePlateau(this, i, j);
        }

        public CasePlateau GetCase(int ligne, int colonne)
        {
            return plateau[ligne, colonne];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                for (int j = 0; j < plateau.GetLength(1); j++)
                    sb.Append(plateau[i, j] + ";");
                sb.Remove(sb.Length - 1, sb.Length);
                sb.Append("\n");
            }
            return sb.ToString();  
        }

        public void Test_Plateau(ChoixJoueur choix)
        {
            if (choix == null)
                throw new NullReferenceException("Choix est null.");

            List<char> lettresNecessaires = Lettres_Necessaires(choix);
            if (lettresNecessaires.Count == 0)
                throw new ApplicationException("Le choix du joueur ne requiert pas de lettre à poser.");
            if (placed > 0)
                if (lettresNecessaires.Count == choix.Mot.Length)
                    throw new ApplicationException("Il faut faire croiser au moins une lettre avec un mot présent.");

            if (!choix.Joueur.Contains_Main_Courante(lettresNecessaires))
                throw new ApplicationException("Le joueur n'a pas les lettres nécessaires dans sa main.");

            CheckMiddle(choix);

            char lettre;
            CasePlateau casePlateau = GetCase(choix.Ligne, choix.Colonne);
            ChoixJoueur.Directions directionOposee = choix.Direction == ChoixJoueur.Directions.Vertical ?
                                                            ChoixJoueur.Directions.Hozizontal :
                                                            ChoixJoueur.Directions.Vertical;

            if (choix.Direction == ChoixJoueur.Directions.Vertical)
                Test_Ligne(choix, choix.Colonne, choix.Direction);
            else
                Test_Ligne(choix, choix.Ligne, choix.Direction);

            for (int i = 0; i < choix.Mot.Length; i++)
            {
                lettre = choix.Mot[i];

                if (choix.Direction == ChoixJoueur.Directions.Hozizontal)
                {
                    casePlateau = GetCase(choix.Ligne, choix.Colonne + i);
                    Test_Ligne(choix, choix.Colonne + i, directionOposee);
                }
                else
                {
                    casePlateau = GetCase(choix.Ligne + i, choix.Colonne);
                    Test_Ligne(choix, choix.Ligne + i, directionOposee);
                }
            }
        }

        private void CheckMiddle(ChoixJoueur choix)
        {
            if (placed > 0)
                return;

            if (choix.Direction == ChoixJoueur.Directions.Vertical)
            {
                if (choix.Colonne != 7)
                    throw new ApplicationException("Le premier placement doit passer par la case du milieu.");
                if (choix.Mot.Length + choix.Ligne < 7)
                    throw new ApplicationException("Le premier placement doit passer par la case du milieu.");
            }
            else
            {
                if (choix.Ligne != 7)
                    throw new ApplicationException("Le premier placement doit passer par la case du milieu.");
                if (choix.Mot.Length + choix.Colonne < 7)
                    throw new ApplicationException("Le premier placement doit passer par la case du milieu.");
            }
        }

        private List<char> Lettres_Necessaires(ChoixJoueur choix)
        {
            List<char> necessaire = new List<char>();
            CasePlateau casePlateau;

            for (int i = 0; i < choix.Mot.Length; i++)
            {
                if (choix.Direction == ChoixJoueur.Directions.Hozizontal)
                    casePlateau = GetCase(choix.Ligne, choix.Colonne + i);
                else
                    casePlateau = GetCase(choix.Ligne + i, choix.Colonne);

                if (casePlateau.IsEmpty())
                    necessaire.Add(choix.Mot[i]);
            }

            return necessaire;
        }

        private void Test_Ligne(ChoixJoueur choix, int n, ChoixJoueur.Directions direction)
        {
            char[] tab = new char[TAILLE];
            
            for (int i = 0; i < TAILLE; i++)
                if (direction == ChoixJoueur.Directions.Hozizontal)
                    tab[i] = GetCase(choix.Ligne, i).Lettre;
                else
                    tab[i] = GetCase(i, choix.Colonne).Lettre;

            int padding = direction == ChoixJoueur.Directions.Hozizontal ? 
                                                choix.Colonne : 
                                                choix.Ligne;
            if (choix.Direction == direction)
            {
                for (int i = 0; i < choix.Mot.Length; i++)
                    tab[i + padding] = choix.Mot[i];
            }    
            else
            {
                int index = direction == ChoixJoueur.Directions.Vertical ? 
                                                n-choix.Colonne : 
                                                n-choix.Ligne;
                tab[padding] = choix.Mot[index];
            }

            string ligne = new string(tab);
            string[] mots = ligne.Split(CasePlateau.EMPTY);
            foreach (String s in mots)
                if (s.Length >= 2 && !jeu.Dictionnaire.Contains(s))
                    throw new ApplicationException("Le mot \"" + s + "\" n'est pas dans le dictionnaire.");
        }

        public void Placer(ChoixJoueur choix)
        {
            Test_Plateau(choix);

            int score = 0;
            int multiplier = 1;
            CasePlateau casePlateau;

            for (int i = 0; i < choix.Mot.Length; i++)
            {
                if (choix.Direction == ChoixJoueur.Directions.Hozizontal)
                    casePlateau = GetCase(choix.Ligne, choix.Colonne + i);
                else
                    casePlateau = GetCase(choix.Ligne + i, choix.Colonne);

                if (casePlateau.IsEmpty())
                {
                    casePlateau.Place(choix.Mot[i]);
                    placed++;
                    choix.Joueur.Remove_Main_Courante(choix.Mot[i]);
                }

                score += casePlateau.MultiplierLettre * jeu.Sac_Jetons.Get(choix.Mot[i]).Point;
                multiplier *= casePlateau.MultiplierMot;
                casePlateau.ClearMultiplier();
            }

            choix.Joueur.Add_Mot(choix.Mot);
            choix.Joueur.Add_Score(score * multiplier);
        }


        public Pixel[,] ToPixels()
        {
            Pixel[,] tab = new Pixel[TAILLE, TAILLE*3];
            Pixel[] temp;

            for (int i = 0; i < TAILLE; i++)
                for (int j = 0; j < TAILLE; j++)
                {
                    temp = GetCase(i, j).ToColoredString().ToPixelArray();
                    for (int k = 0; k < 3; k++)
                        tab[i, j * 3 + k] = temp[k];
                }

            return tab;
        }
    }
}
