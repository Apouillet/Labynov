using System;
using System.Collections.Generic;
using System.Text;

namespace Labynov
{
    public class Dalton : Personnage
    {
        //Mais Ma pourquoi prendre un bain ? J'en ai pris un l'année dernière !
        public int odeurDePied { get; set; }
        public bool creuse = false;

        public Dalton(int x, int y) : base(x, y)
        {
        }

        public Dalton(int x, int y, string name) : base(x, y, name)
        {
            this.signe = "o";
        }
    }
}
