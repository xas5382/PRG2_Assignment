using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Cup : IceCream
    {
        public Cup() { }
        public Cup(string option, int scoops, List<Flavour> flavours, List<Topping> toppings) : base(option, scoops, flavours, toppings)
        {
            Option = option;
            Scoops = scoops;
            Flavours = flavours;
            Toppings = toppings;
        }

        public override double CalculatePrice()
        {
            double price = 0;

            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2;
                }
            }

            if (Scoops == 1)
            {
                price = 4;
            }
            else if (Scoops == 2)
            {
                price = 5.5;
            }
            else if (Scoops == 3)
            {
                price = 6.5;
            }

            price += Toppings.Count() * 1;

            return price;
        }

        public override string ToString() 
        {
            return base.ToString() + "\n";
        }
    }
}
