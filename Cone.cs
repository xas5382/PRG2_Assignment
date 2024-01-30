//==========================================================
// Student Number : S10257400
// Student Name : See Wai Kee, Audrey
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Cone : IceCream
    {
        private bool dipped;

        public bool Dipped
        {
            get { return dipped; }
            set { dipped = value; }
        }

        public Cone() { }

        public Cone(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, bool dipped) : base(option, scoops, flavours, toppings)
        {
            Dipped = dipped;
        }

        public override double CalculatePrice()
        {
            double price = 0;

            if (Scoops == 1)
            {
                price += 4;
            }
            else if (Scoops == 2)
            {
                price += 5.5;
            }
            else if (Scoops == 3)
            {
                price += 6.5;
            }

            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2; ;
                }
            }
            
            if (dipped) 
            {
                price += 2;
            }

            price += Toppings.Count() * 1;

            return price;
        }

        public void ModifyConeFlavour()   // method to modify cone flavour, not found in the class diagram
        {
            Console.WriteLine();

            while (true)
            {
                Console.Write(Dipped ? "Do you want to change Ice Cream cone to an orginal cone [Y/N]? " : "Do you want to upgrade Ice Cream cone to a chocolate cone [Y/N]? ");
                string changeCone = Console.ReadLine();

                if (changeCone.ToLower() == "y" && Dipped == false)
                {
                    Dipped = true;
                    break;
                }
                else if (changeCone.ToLower() == "y" && Dipped == true)
                {
                    Dipped = false;
                    break;
                }
                else if (changeCone.ToLower() == "n")
                {
                    break;
                }
                else if (changeCone.ToLower() != "y" && changeCone.ToLower() != "n")
                {
                    Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                }
            }
        }

        public override string ToString()
        {
            if (!Dipped)
            {
                return base.ToString() + "Chocolate Cone? \n" + "- No \n";
            }
            else
            {
                return base.ToString() + "Chocolate Cone? \n" + "- Yes \n";
            }
        }
    }
}
