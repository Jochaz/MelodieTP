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

        private int mutationRate = 15;

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

            // enregistrer l'average de la génération en cours dans l'array

            // elitisime -- garder le meilleur
            // boucle sur le nb d'individu (de 2 à 13)
            //  pour chaque boucle, on va déterminer si on créer le mec avec 1 parent ou 2
            // pour cela, on fait un rdm, si le rdm est inférieur au crossover rate, alors 2 parents, sinon 1 seul
            // si 2 parents, il faut les sélectionner (tournois), puis créer les 2 invidivus avec eux, et enfin muter l'enfant (attention, l'instrument peut aussi muter)
                    // crossover : faire un rdm sur le nb de gène total des individus. puis conserver la partie des notes qui se trouve avant la valeur du rdm, et garder l'autre partie du 2ème
                    // crossover instrument : rdm 1 ou 2, ppour savoir quel instrument on conserve

                    // mutation : parcourir tous les gènes de l'invidu, faire un rdm, si rdm inférieur au taux de mutation, alors muter la note d'une amplitude raisonnable (+10 ou -10)
                    // mutation instrument : idem pour le rdm, et changer l'instrument si besoin.....

            // si 1 seul parent, alors le copier et le muter

            // on se retrouve alors avec 1 seul survivant de la génération précédente + 12 nouveaux individus créés à partir des anciens, et différents de quelques gènes (mutation)




            Individu newIndividu;
            Individu parent;
            int nbParent;
            int i;
            List<Individu> nextGen = new List<Individu>();
            List<Individu> parents = new List<Individu>();
            nextGen.Clear();
            // on ajoute le meilleur dans la génération suivante
            nextGen.Add(meilleur());
            nextGen[0].SongNumber = 1;

            // il faut maintenant ajouter dans la nouvelle liste les 12 prochains individus qui feront partie de la prochaine génération

            for (i = 1; i <= 12; i++)
            {

                parents.Clear();
                // on détermine le nb de parent
                // si le nb tiré au sort est inférieur au taux de crossover, alors 2 parents, sinon 1 seul
                nbParent = (rand.Next(0, 100) <= crossoverRate ? 2 : 1);

                // on définit les parents
                // on récupère le meilleur parmis x individus tirés au sort parmi tous les individus
                parent = meilleur(tirage(2, null));
                for (int a = 1; a <= nbParent; a++)
                {

                    while (parents.Contains(parent))
                    {
                        parent = meilleur(tirage(2, null));
                    }

                    parents.Add(parent);
                }

                newIndividu = new Individu(parents);
                newIndividu.muter(mutationRate);
                nextGen.Add(newIndividu);
                nextGen[i].SongNumber = i + 1;
            }

            individus.Clear();
            individus = nextGen;

            nbGeneration++;
        }

        // tire au sort un certains nb d'individu (1 par défaut), parmis une liste d'individu (liste complète par défaut)
        public List<Individu> tirage(int nb = 1, List<Individu> lstIndividu = null)
        {

            lstIndividu = (lstIndividu == null ? this.individus : lstIndividu);
            List<Individu> lstRdm = new List<Individu>();


            int index = rand.Next(0, lstIndividu.Count - 1);

            for (int a = 1; a <= nb; a++)
            {
                // petit controle permettant d'éviter d'avoir 2 fois le même individu dans la liste
                while(lstRdm.Contains(lstIndividu[index])){
                    index = rand.Next(0, lstIndividu.Count - 1);
                }

                lstRdm.Add(lstIndividu[index]);
            }

            return lstRdm;
        }
        
        public void InitPopulation()
        {
            individus = new List<Individu>();
            averages = new List<double>();
            averages.Clear();

            // création de la population de base
            for (int i = 0; i < 13; i++)
            {

                Individu individu = new Individu();
                individu.SongNumber = i + 1;
                individus.Add(individu);
            }
        }

        public void saveGenAverage(){
            int total = 0;
            double average;
            foreach (Individu individu in individus)
            {
                total += individu.Note;
            }

            average = (double)total / individus.Count;
            Console.WriteLine(total / individus.Count);
            averages.Add(average);
        }
        // cette méthode va renvoyer l'individu ayant la meilleure note parmis une liste d'individus
        // si plusieurs individus sont les meilleurs (même note), alors nous allons en renvoyer un aléatoirement parmi eux
        public Individu meilleur(List<Individu> listIndividus = null)
        {

            //Console.WriteLine(listIndividus[0].Note);
            // si pas de paramètre, alors nous faisons un "élitisme" sur l'ensemble des individus, sinon sur les individus transmis dans la liste
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
                {
                    meilleurs.Add(listIndividus[i]);
                }
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
