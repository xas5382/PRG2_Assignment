using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Waffle : IceCream
    {
        private string waffleFlavour;

        public string WaffleFlavour
        { 
            get { return waffleFlavour;} 
            set { waffleFlavour = value; }
        }

        public Waffle() { }

        public Waffle(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour) : base(option, scoops, flavours, toppings) 
        {
            WaffleFlavour = waffleFlavour;
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

            if (WaffleFlavour == "Red velvet" || WaffleFlavour == "Charcoal" || WaffleFlavour == "Pandan")
            {
                price += 3;
            }

            if (Scoops == 1)
            {
                price = 7;
            }
            else if (Scoops == 2)
            {
                price = 8.5;
            }
            else if (Scoops == 3)
            {
                price = 9.5;
            }

            price += Toppings.Count() * 1;

            return price;
        }

        public override string ToString()
        {
            return base.ToString() + "Waffle Flavour \n" +  $"- {waffleFlavour} \n"; 
        }
    }
}
