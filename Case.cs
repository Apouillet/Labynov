using System;
using System.Collections.Generic;
using System.Text;

namespace Labynov
{
    public class Case
    {
        public int x { get; set; }
        public int y { get; set; }
        public string occupePar { get; set; }
        // X = terre , o = Dalton mechant , O = Avrel , R = Rantanplan ,   = libre;

        private bool libre = false;



        public int odeurDePieds { get; set; }

        public int odeurDeCuisine { get; set; }
        public Case(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.odeurDeCuisine = 0;
            this.odeurDePieds = 0;
            this.occupePar = "X";
        }
        public void soccupe(string obstacle)
        {
            //S'occupe par le signe destiner à représenter le personnage
            this.libre = false;
            this.occupePar = obstacle;
        }
        public void soccupe()
        {
            //S'occupe par de la terre à son initialisation
            this.libre = false;
            this.occupePar = "X";
        }
        public void seLibere()
        {
            this.libre = true;
            this.occupePar = " ";
        }
        public bool estLibre()
        {
            return this.libre;
        }
    }
}
