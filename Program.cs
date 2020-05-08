using System;
using System.Runtime.CompilerServices;

namespace Labynov
{
    public class Program
    {
        static void Main(string[] args)
        {
            Initializer init = new Initializer();
            Map plateauJeu = init.startMenu();
            plateauJeu.toString();
            plateauJeu.creuserGalleries();
            plateauJeu.odeurInit();
            Console.Write("\n");
            Console.Write("\n");
            plateauJeu.toString();
            Console.Write("\n");
            string str = plateauJeu.startRantanplan();
            Console.Write(str);
            plateauJeu.toString();

        }
    }
}
