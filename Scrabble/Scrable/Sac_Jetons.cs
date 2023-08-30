using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrable
{
    class Sac_Jetons
    {
        private List<Jeton> jetons = new List<Jeton>(27);

        public Sac_Jetons(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path may not be null.");

            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
                jetons.Add(new Jeton(line));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Sac de jetons : \n");
            foreach (Jeton jeton in jetons)
                sb.Append(jeton.ToString());
            return sb.ToString();
        }

        public Jeton Retire_Jeton(Random r)
        {
            if (NombreJetons() <= 0)
                throw new ApplicationException("No more jeton in this bag");


            int num = r.Next(0, NombreJetons());
            int temp = 0;
            foreach (Jeton j in this.jetons)
            {
                temp += j.Quantite;
                if (temp >= num)
                {
                    j.Quantite--;
                    return j;
                }
            }

            return null;
        }

        public bool Contains(char lettre)
        {
            return Get(lettre) != null;
        }

        public bool Contains(Jeton jeton)
        {
            return jetons.Contains(jeton);
        }

        public int NombreJetons()
        {
            int count = 0;
            foreach (Jeton jeton in jetons)
                count += jeton.Quantite;
            return count;
        }

        public int NombreJetons(char lettre)
        {
            try
            {
                return Get(lettre).Quantite;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        public Jeton Get(char lettre)
        {
            foreach (Jeton jeton in jetons)
                if (jeton.Lettre == lettre)
                    return jeton;
            return null;
        }

    }
}
