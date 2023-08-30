using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrable
{
    class Dictionnaire
    {

        private Dictionary<int, HashSet<string>> dico = new Dictionary<int, HashSet<string>>();
        public string Langue { get; private set; }

        //Constructeurs
        public Dictionnaire(string path, string langue)
        {
            if (path == null)
                throw new ArgumentNullException("path may not be null.");
            if (langue == null)
                throw new ArgumentNullException("langue may not be null.");
            this.Langue = langue;

            StreamReader sr = new StreamReader(path);
            string[] mots;
            int length;
            HashSet<string> smallDico;

            while (!sr.EndOfStream)
            {
                length = Convert.ToInt32(sr.ReadLine());
                mots = sr.ReadLine().Split(" ");

                smallDico = GetDictionnaire(length, mots.Length);
                foreach (string s in mots)
                    smallDico.Add(s.ToLower());
            }

        }

        //Utils
        private HashSet<string> GetDictionnaire(int tailleMot, int defSize = 0)
        {
            if (tailleMot <= 1 || tailleMot > 15)
                throw new ArgumentOutOfRangeException("tailleMot must be between 2 & 15.");

            if ( ! dico.ContainsKey(tailleMot))
            {
                HashSet<string> smallDico = new HashSet<string>(defSize);
                dico.Add(tailleMot, smallDico);
                return smallDico;
            }
            return dico[tailleMot];
        }

        //Methodes imposées
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Langue + "\nMots : Nombre\n");
            foreach (int s in dico.Keys)
                sb.AppendLine(s + " : " + Size(s));
            return sb.ToString();
        }

        public bool RechDichoRecursif(string mot)
        {
            //La recherche est techniquement récursive car l'objet HashSet effectue
            //une recherche dichotomique
            return Contains(mot);
        }

        //Methodes
        public bool Contains(string mot)
        {
            if (mot == null)
                return false;
            if (!this.dico.ContainsKey(mot.Length))
                return false;

            HashSet<string> dico = GetDictionnaire(mot.Length);
            return dico.Contains(mot.ToLower());
        }

        public bool AddWord(string mot)
        {
            if (mot == null)
                return false;
            return GetDictionnaire(mot.Length).Add(mot);
        }

        public bool RemoveWord(string mot)
        {
            if (mot == null)
                return false;

            HashSet<string> dico = GetDictionnaire(mot.Length);
            return dico.Remove(mot);
        }

        public int Size()
        {
            int count = 0;
            foreach (HashSet<string> dico in dico.Values)
                count += dico.Count;
            return count;
        }

        public int Size(int wordLength)
        {
            return GetDictionnaire(wordLength).Count;
        }
    }
}
