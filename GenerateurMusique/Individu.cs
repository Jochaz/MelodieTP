using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurMusique
{
    class Individu
    {

        private int instrument;

        private int tempo;

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
        private List<Note> notes;

        internal List<Note> Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public Individu(int instru, int temp)
        {
            notes = new List<Note>();
            instrument = instru;
            tempo = temp;
        }

        public void mutation()
        {

        }

        public void save()
        {

        }


    }
}
