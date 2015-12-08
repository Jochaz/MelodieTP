using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurMusique
{
    class Note
    {

        private int valeur;

        private int duration = 16;

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

        public Note(int number)
        {
            valeur = number;
        }
    }
}
