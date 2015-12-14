using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GenerateurMusique
{
    class Individu
    {
        static Random rand = new Random();
        private int instrument;

        private int songNumber;

        public int SongNumber
        {
            get { return songNumber; }
            set { songNumber = value; }
        }

        private int tempo;

        private int note = 0;

        public int Note
        {
            get { return note; }
            set { note = value; }
        }

        public int Tempo
        {
            get { return tempo; }
            set { tempo = value; }
        }

        public int Instrument
        {
            get { return instrument; }
            set { instrument = value; }
        }
        private List<Note> genes = new List<Note>();

        internal List<Note> Genes
        {
            get { return genes; }
            set { genes = value; }
        }

        public Individu(List<Individu> parents = null)
        {

            if (parents != null)
            {
                //----------- création AVEC parent  ----------------
                crossover(parents);
            }
            else
            {
                //----------- création SANS parent  ----------------
                instrument = rand.Next(1, 128);
                tempo = rand.Next(130, 170);
                // création des gènes : notes (16)
                for (int a = 0; a <= 15; a++)
                {
                    Note note = new Note();
                    Genes.Add(note);
                }
            }
        }

        public void crossover(List<Individu> parents)
        {
            if (parents.Count == 1)
            {
                instrument = parents[0].instrument;
                tempo = parents[0].tempo;
                genes = parents[0].genes;
                Console.WriteLine("Crossover 1 seul parent");
            }
            else
            {
                Console.WriteLine("Crossover 2 parents");
                instrument = parents[rand.Next(0, 1)].instrument;
                tempo = parents[rand.Next(0, 1)].tempo;

                int pointCoupure;
                pointCoupure = rand.Next(1, 14);

                // on affecte les nouvelles notes en fonction du point de coupure
                for (int i = 0; i <= 15; i++)
                {
                    if (i < pointCoupure)
                        genes.Add(parents[0].Genes[i]);
                    else
                        genes.Add(parents[1].Genes[i]);

                    // on modifie la durée du dernier gène ajouté
                    genes[genes.Count - 1].Duration = parents[rand.Next(0,1)].Genes[i].Duration;
                }
            }
        }

        public void muter(int tauxMutation)
        {
            int value;
            
            
            // ----- Mutation de l'instrument -------- //
            value = rand.Next(0, 100);

            if (value < tauxMutation)
            {
                instrument = rand.Next(1, 128);
                Console.WriteLine("mutation");
            }
            

            // ----- mutation des gènes (notes) ----- //
            Note note;
            for (int i = 0; i <= 15; i++)
            {
                note = genes[i];

                // --- mutation de la valeur de la note --- //
                value = rand.Next(0, 100);
                if (value < tauxMutation)
                {

                    note.Valeur += rand.Next(-10, 10);
                    note.Valeur = (note.Valeur > note.MaxValue ? note.MaxValue :
                                                                (note.Valeur < note.MinValue ? note.MinValue : note.Valeur));
                    Console.WriteLine("mutation");
                }

                // --- mutation de la durée d'une note (longueur) --- //
                value = rand.Next(0, 100);
                if (value < tauxMutation)
                {
                    note.Duration = rand.Next(13, 19);
                    Console.WriteLine("mutation");
                }
            }

            // ----- Mutation du tempo ----- //
            
            value = rand.Next(0,100);
            if (value < tauxMutation)
            {
                tempo = rand.Next(130, 170);
                Console.WriteLine("mutation");
            }
        }

        public void save()
        {

        }

    }
}
