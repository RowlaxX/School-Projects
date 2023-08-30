using Scrable.ui;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scrable
{
    class Jeu
    {
        //Classe builder
        public class Builder
        {
            public const int MAX_JOUEURS = 4, MIN_JOUEURS = 1;

            public static Builder newBuilder()
            {
                return new Builder();
            }

            private string pathDictionnaire = null;
            private string pathSacJeton = null;
            private string pathPlateau = null;
            private string langue = null;
            private uint tempsJeu = 6;

            private List<string> nomJoueurs = new List<string>(1);
            private int nombreJoueur = 1;

            private Builder() { }

            public Builder PathDictionnaire(string path)
            {
                this.pathDictionnaire = path;
                return this;
            }

            public Builder PathSacJeton(string path)
            {
                this.pathSacJeton = path;
                return this;
            }

            public Builder PathPlateau(string path)
            {
                this.pathPlateau = path;
                return this;
            }

            public Builder AjouterJoueur(string nom)
            {
                if (nomJoueurs.Count >= nombreJoueur)
                    throw new ArgumentException("Il y a déjà le nombre maximum de joueurs.");
                this.nomJoueurs.Add(nom);
                return this;
            }

            public Builder NombreJoueur(int nombre)
            {
                if (nombre < MIN_JOUEURS || nombre > MAX_JOUEURS)
                    throw new ArgumentException("Il doit y avoir entre " + MIN_JOUEURS + " et " + MAX_JOUEURS + " joueurs.");
                if (nomJoueurs.Count != 0)
                    throw new ArgumentException("Veuillez appeller cette méthode avant d'ajouter des joueurs.");
                this.nombreJoueur = nombre;
                return this;
            }

            public Builder Langue(string langue)
            {
                this.langue = langue;
                return this;
            }

            public Builder TempsJeu(uint tempsJeu)
            {
                this.tempsJeu = tempsJeu;
                return this;
            }
            public Jeu BuildGame() 
            {
                Jeu jeu = new Jeu(tempsJeu);
                Console.WriteLine("Lecture du dictionnaire...");
                Dictionnaire dico = new Dictionnaire(pathDictionnaire, langue);
                Console.WriteLine("Lecture du sac de jetons...");
                Sac_Jetons sac = new Sac_Jetons(pathSacJeton);
                Console.WriteLine("Création d'un plateau...");
                Plateau plateau = pathPlateau == null ? new Plateau(jeu) : Plateau.FromFile(jeu, pathPlateau);
                Console.WriteLine("Création des joueurs...");
                List<Joueur> joueurs = new List<Joueur>();
                foreach (string nomJoueur in nomJoueurs)
                    joueurs.Add(new JoueurHumain(nomJoueur));
                for (int i = joueurs.Count; i < nombreJoueur; i++)
                    joueurs.Add(new JoueurIA());

                jeu.Dictionnaire = dico;
                jeu.Sac_Jetons = sac;
                jeu.Plateau = plateau;
                jeu.Joueurs = joueurs;
                Console.WriteLine("Fait.");
                return jeu;
            }
        }

        public Dictionnaire Dictionnaire { get; private set; }
        public Sac_Jetons Sac_Jetons { get; private set; }
        public bool Started { get; private set; } = false;
        public Plateau Plateau { get; private set; }
        public uint TempsJeu { get; private set; }
        public List<Joueur> Joueurs { get; private set; }
        public PlayingScreen PlayingScreen { get; private set; }
        private Screen MessageScreen { get; set; } = new Screen();

        private Random random = new Random();
        private int tour = 0;

        private Jeu(uint tempsJeu) 
        {
            this.PlayingScreen = new PlayingScreen(this);
            this.TempsJeu = tempsJeu;
        }

        public void Start()
        {
            if (Started)
                return;
            this.Started = true;
            Play();
        }

        private void Play()
        {
            //Les joueurs commencent à piocher
            MessageScreen.Update();
            ColoredString c = new ColoredString("Les joueurs piochent...", ConsoleColor.Magenta, ConsoleColor.Black);
            int y = Screen.Height / 2;
            int x = (Screen.Width - c.Length) / 2;
            MessageScreen.Draw(c, y, x);
            Joueurs.ForEach(Piocher);
            MessageScreen.Print();
            Thread.Sleep(1000);

            Joueur j;
            while (!FinJeu())
            {
                j = Joueurs[tour];
                Tour(j);
                tour = (tour + 1) % Joueurs.Count;
            }
        }

        private void Tour(Joueur joueur)
        {
            MessageScreen.Update();
            ColoredString c0 = new ColoredString("Au tour de " + joueur.Nom + " !", ConsoleColor.Magenta, ConsoleColor.Black);
            int y = Screen.Height / 2;
            int x0 = (Screen.Width - c0.Length) / 2;
            MessageScreen.Draw(c0, y, x0);

            if (joueur.Nombre_Jeton_Main() < Joueur.QUANTITE_MAIN)
            {
                ColoredString c1 = new ColoredString(joueur.Nom + " pioche...", ConsoleColor.Magenta, ConsoleColor.Black);
                int x1 = (Screen.Width - c1.Length) / 2;
                MessageScreen.Draw(c1, y + 2, x1);
            }

            MessageScreen.Print();
            Piocher(joueur);
            Thread.Sleep(1000);
            ChoixJoueur choix = null;
            DateTime limit = DateTime.Now.AddMinutes(this.TempsJeu);

            do
            {
                try
                {
                    choix = joueur.Choix(this);
                    if (choix == null)
                        return;
                    Plateau.Placer(choix);
                    return;
                }
                catch(ApplicationException e)
                {
                    PlayingScreen.Erreur = e.Message;
                }
                catch(ArgumentException e)
                {
                    PlayingScreen.Erreur = e.Message;
                }

            } while (DateTime.Now < limit);

            MessageScreen.Update();
            ColoredString c = new ColoredString("Temps expiré !", ConsoleColor.Magenta, ConsoleColor.Black);
            int x2 = (Screen.Width - c.Length) / 2;
            MessageScreen.Draw(c, y, x2);
            Joueurs.ForEach(Piocher);
            MessageScreen.Print();
            Thread.Sleep(1000);
            return;
        }

        public Joueur GetPlayingPlayer()
        {
            return this.Joueurs[tour];
        }

        private bool FinJeu()
        {
            return this.Sac_Jetons.NombreJetons() == 0;
        }

        private void Piocher(Joueur joueur)
        {
            while (joueur.Nombre_Jeton_Main() < Joueur.QUANTITE_MAIN)
                joueur.Add_Main_Courante(Sac_Jetons.Retire_Jeton(random));
        }
    }
}
