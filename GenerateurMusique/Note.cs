using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurMusique
{
    class Note
    {
        static Random rand = new Random();
        private int maxValue = 88; //88

        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        private int minValue = 39; //39

        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        private int valeur;

        private int duration;

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int Valeur
        {
            get { return valeur; }
            set { valeur = value; }
        }

        public Note()
        {
            valeur = rand.Next(minValue, maxValue);
            duration = rand.Next(14, 18);
        }
    }
}
