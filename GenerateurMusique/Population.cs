using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurMusique
{
    class Population
    {
        static Random rand = new Random();
        private int crossoverRate = 70;

        public int CrossoverRate
        {
            get { return crossoverRate; }
            set { crossoverRate = value; }
        }

        private int mutationRate = 8;

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

        public int NbGeneration
        {
            get { return nbGeneration; }
            set { nbGeneration = value; }
        }

        private List<double> averages;

        public List<double> Averages
        {
            get { return averages; }
            set { averages = value; }
        }

        public Population()
        {
            InitPopulation();
            nbGeneration = 1;
        }

        public void NextGeneration()
        {
            Individu newIndividu;
            Individu parent;
            int nbParent;
            int i;
            List<Individu> nextGen = new List<Individu>();
            List<Individu> parents = new List<Individu>();
            nextGen.Clear();

            // 1) SURVIE
                // élitisme, nous conservons que le meilleur
            nextGen.Add(meilleur());
            nextGen[0].SongNumber = 1;

            for (i = 2; i <= 12; i++)
            {
                parents.Clear();

                // 2) SELECTION des parents
                nbParent = (rand.Next(0, 100) <= crossoverRate ? 2 : 1);

                parent = meilleur(tirage(2));
                for (int a = 1; a <= nbParent; a++)
                {
                    while (parents.Contains(parent))
                        parent = meilleur(tirage(2));

                    parents.Add(parent);
                }

                // 3) CROSSOVER
                newIndividu = new Individu(parents);

                // 4) MUTATION
                newIndividu.muter(mutationRate);

                newIndividu.SongNumber = i;
                nextGen.Add(newIndividu);

            }
            // on vide la liste des individus de la génération actuelle
            individus.Clear();
            // on met les individus de la génération suivante dans la liste des individus de la génération actuelle
            individus = nextGen;

            nbGeneration++;
        }

        // tire au sort un certain nb d'individu (1 par défaut), parmis une liste d'individu (liste complète par défaut)
        public List<Individu> tirage(int nb = 1, List<Individu> lstIndividu = null)
        {
            lstIndividu = (lstIndividu == null ? this.individus : lstIndividu);
            List<Individu> lstRdm = new List<Individu>();

            int index = rand.Next(0, lstIndividu.Count - 1);

            for (int a = 1; a <= nb; a++)
            {
                // petit controle permettant d'éviter d'avoir 2 fois le même individu dans la liste
                while(lstRdm.Contains(lstIndividu[index]))
                    index = rand.Next(0, lstIndividu.Count - 1);
                lstRdm.Add(lstIndividu[index]);
            }

            return lstRdm;
        }
        
        public void InitPopulation()
        {
            individus = new List<Individu>();
            averages = new List<double>();

            // création de la population de base
            for (int i = 1; i <= 12; i++)
            {
                Individu individu = new Individu();
                individu.SongNumber = i;
                individus.Add(individu);
            }
        }

        public void saveGenAverage(){
            int total = 0;
            double average;
            foreach (Individu individu in individus)
                total += individu.Note;

            average = (double)total / individus.Count;
            averages.Add(average);
        }
        // cette méthode va renvoyer l'individu ayant la meilleure note parmis une liste d'individus
        // si plusieurs individus sont les meilleurs (même note), alors nous allons en renvoyer un aléatoirement parmi eux
        public Individu meilleur(List<Individu> listIndividus = null)
        {
            // si pas de paramètre saisi, alors nous faisons un "élitisme" sur l'ensemble des individus, sinon sur les individus transmis dans la liste
            listIndividus = (listIndividus == null ? this.individus : listIndividus);

            Individu meilleur = listIndividus[0];
            List<Individu> meilleurs = new List<Individu>();
            meilleurs.Add(meilleur);

            int i;

            for (i = 1; i <= listIndividus.Count - 1; i++)
            {

                if (listIndividus[i].Note > meilleur.Note)
                {
                    meilleurs.Clear();
                    meilleurs.Add(listIndividus[i]);
                    meilleur = listIndividus[i];
                }
                else if (listIndividus[i].Note == meilleur.Note)
                    meilleurs.Add(listIndividus[i]);
            }

            // si 1 seul individu a la meilleure note, on le retourne
            if (meilleurs.Count == 1)
                return meilleur;

            // si on a plusieurs individus avec la même note, on a va devoir faire un rdm


            int index;
            index = rand.Next(0, meilleurs.Count);
            // return du 1er élément de la liste récupérée
            return tirage(1,meilleurs)[0];
        }
    }
}
