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

        private int max = 95;

        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        private int min = 25;

        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        private int songNumber;

        public int SongNumber
        {
            get { return songNumber; }
            set { songNumber = value; }
        }

        private int tempo = 150;

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

                // création des notes (16)
                for (int a = 0; a < 16; a++)
                {
                    Note note = new Note(rand.Next(min, max));
                    Genes.Add(note);
                }
            }
        }

        public void crossover(List<Individu> parents)
        {
            if (parents.Count == 1)
            {
                instrument = parents[0].instrument;
                genes = parents[0].genes;
            }
            else
            {

                int pointCoupure;
                // on détermine l'instrument
                instrument = parents[rand.Next(0, 1)].instrument;

                // on détermine le point de coupure des gènes (notes)
                pointCoupure = rand.Next(0, 15);

                // on affecte les nouvelles notes en fonction du point de coupure
                for (int i = 0; i < 16; i++)
                {
                    if (i < pointCoupure)
                        genes.Add(parents[0].Genes[i]);
                    else
                        genes.Add(parents[1].Genes[i]);
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
            }
            

            // ----- mutation des gènes (notes) ----- //
            Note note;
            for (int i = 0; i < 16; i++)
            {
                value = rand.Next(0, 100);
                if (value < tauxMutation)
                {
                    note = genes[i];
                    // on fait muter la valeur du gène d'une "amplitude" raisonnable comprise entre -10 et 10, pour éviter de trop grosses différences
                    note.Valeur += rand.Next(-10, 10);
                    // on vérifie que notre nouvelle valeur ne dépasse pas les min et max définit, si c'est le cas on applique le min et le max définit
                    note.Valeur = (note.Valeur > max ? max :
                                                                (note.Valeur < min ? min : note.Valeur));
                }
            }
        }

        public void save()
        {

        }


    }
}
