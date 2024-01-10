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
            StringBuilder iceCreamInfo = new StringBuilder();

            iceCreamInfo.Append($"{Option,-19} {Scoops,-3}");
            
            string x = "";
            for (int i = 0; i < Flavours.Count(); i++)
            {
                if (i < Flavours.Count() - 1)
                {
                    x += $" {Flavours[i].Type}({Flavours[i].Quantity}),";
                }
                else
                {
                    x += $" {Flavours[i].Type}({Flavours[i].Quantity})";
                }
            }
         
            iceCreamInfo.Append($"{x,-43}");

            string y = "";
            if (Toppings.Count == 0)
            {
                y += "None";
            }
            else
            {
                for (int i = 0; i < Toppings.Count(); i++)
                {
                    if (i < Toppings.Count() - 1)
                    {
                        y += $" {Toppings[i].Type},";
                    }
                    else
                    {
                        y += $" {Toppings[i].Type}";
                    }
                }
            }

            iceCreamInfo.Append($"{y,-33}");

            return iceCreamInfo.ToString();
        }
    }
}
