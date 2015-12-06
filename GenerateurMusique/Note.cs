using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurMusique
{
    class Note
    {

        private int id;

        private int duration;

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Note(int number, int durat)
        {
            duration = durat;
            id = number;
        }
    }
}
