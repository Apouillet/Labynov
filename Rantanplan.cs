using System;

namespace Labynov
{
    public class Rantanplan : Personnage
    {
        public int obeissance { get; set; }
        public int vies { get; set; }

        public int faim { get; set; }

        public int viesMax { get; set; }

        public bool inteligent = false;

        public bool trouveAvrel = false;

        public int ptsRecordPied { get; set; }
        public string choixRecordPied { get; set; }
        public int ptsRecordNourriture { get; set; }
        public string choixRecordNourriture { get; set; }
        public Rantanplan(int x, int y, int vies, int faim, int obeissance) : base(x, y)
        {
            this.name = "Rantanplan";
            this.vies = vies;
            this.viesMax = vies;
            this.faim = faim;
            this.obeissance = obeissance;
            this.signe = "R";
        }

        public bool perdreUneVieEtMeurt()
        {
            Console.WriteLine("Kaï Kaï ! \n");
            this.vies -= 1;
            this.faim += (this.obeissance + this.faim) / (2 * this.viesMax);
            this.obeissance -= (this.obeissance + this.faim) / (2 * this.viesMax);
            if (inteligent)
            {
                this.obeissance = 0;
                this.faim = 100;
            }
            if (this.obeissance < 0)
            {
                this.obeissance = 0;
            }
            if (this.vies <= 0)
            {
                return true;
            }
            return false;
        }
    }
}