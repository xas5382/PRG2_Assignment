using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    abstract class IceCream
    {
        private string option;
        private int scoops;
        private List<Flavour> flavours;
        private List<Topping> toppings;

        public string Option
        {
            get { return option; }
            set { option = value; }
        }

        public int Scoops
        {
            get { return scoops; }
            set { scoops = value; }
        }

        public List<Flavour> Flavours
        {
            get { return flavours; }
            set { flavours = value; }
        }

        public List<Topping> Toppings
        { 
            get { return toppings; } 
            set { toppings = value; }
        }

        public IceCream() { }
        
        public IceCream(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
        {
            Option = option;
            Scoops = scoops;
            Flavours = flavours;
            Toppings = toppings;
        }

        public abstract double CalculatePrice();

        public override string ToString()
        {
            string iceCreamInfo = "";

            iceCreamInfo += ($"{Option} Ice Cream \n");
            iceCreamInfo += ($"{Scoops} scoops of Ice Cream \n");
            
            for (int i = 0; i < Flavours.Count(); i++)
            {
                iceCreamInfo += ($"- {Flavours[i].Quantity} {Flavours[i].Type} \n");
            }

            for (int i = 0; i < Toppings.Count(); i++)
            {
                if (i == 0)
                {
                    iceCreamInfo += ("Topping(s) \n");
                    iceCreamInfo += ($"- {Toppings[i].Type} \n");
                }
                else
                {
                    iceCreamInfo += ($"- {Toppings[i].Type} \n");
                }
            }

            return iceCreamInfo.ToString();
        }
    }
}
