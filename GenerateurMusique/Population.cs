using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurMusique
{
    class Population
    {
        private int crossoverRate;

        public int CrossoverRate
        {
            get { return crossoverRate; }
            set { crossoverRate = value; }
        }

        private int mutationRate;

        public int MutationRate
        {
            get { return mutationRate; }
            set { mutationRate = value; }
        }

        private List<Individu> individus;

        internal List<Individu> Individus
        {
            get { return individus; }
            set { individus = value; }
        }

        private int nbGeneration;

        public Population()
        {
            InitPopulation();
            nbGeneration = 1;
        }

        public void NextGeneration()
        {
            // code...
            // selection des parents
            // reproduction
            // mutation des nouveaux enfants
            // survie des autres

            nbGeneration++;
        }

        public void InitPopulation()
        {
            individus = new List<Individu>();
            Random rand = new Random();
            crossoverRate = 50;
            mutationRate = 10;

            // création de la population de base
            for (int i = 0; i < 13; i++)
            {
                Console.WriteLine("---------------- " + (i+1) + " -----------------");
                int instrument = rand.Next(1, 129);
                Individu individu = new Individu(instrument, rand.Next(100,200));
                Console.WriteLine("Instrument : " + instrument);

                // 15 notes par individu
                for (int a = 0; a < 16; a++)
                {
                    Note note = new Note(rand.Next(40, 80), rand.Next(12,20));
                    Console.WriteLine(note.Id);
                    individu.Notes.Add(note);
                }

                Console.WriteLine("-----------------");
                // blabla
                individus.Add(individu);
            }
        }
    }
}
