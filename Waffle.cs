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

            if (Scoops == 1)
            {
                price += 7;
            }
            else if (Scoops == 2)
            {
                price += 8.5;
            }
            else if (Scoops == 3)
            {
                price += 9.5;
            }

            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2; ;
                }
            }

            if (WaffleFlavour.ToLower() != "original")
            {
                price += 3;
            }

            price += Toppings.Count() * 1;

            return price;
        }

        public void ModifyWaffleFlavour()   // method to modify waffle flavour, not found in the class diagram
        {
            List<string> waffleFlavourList = new List<string> { "Original , 0", "Red Velvet, 3", "Charcoal, 3", "Pandan, 3" };
            
            if (waffleFlavourList.Contains(WaffleFlavour))
            {
                waffleFlavourList.Remove(WaffleFlavour);
            }

            Console.WriteLine("\n" + "Waffle Flavours to choose from and their additional costs");
            Console.WriteLine("{0,-17} {1}", "Flavour", "Add on Cost"); ;
            for (int i = 0; i< waffleFlavourList.Count; i++)
            {
                string[] info = waffleFlavourList[i].Split(",");
                Console.WriteLine($"[{i+1}] {info[0], -12} {info[1]}");
            }

            string newWaffleFlavour;
            while (true)
            {
                Console.Write("\n" + "Which waffle flavour would you like? ");
                newWaffleFlavour = Console.ReadLine();

                if (newWaffleFlavour.ToLower() == "original")
                {
                    WaffleFlavour = "Original";
                }
                else if (newWaffleFlavour.ToLower() == "red velvet")
                {
                    WaffleFlavour = "Red Velvet";
                }
                else if (newWaffleFlavour.ToLower() == "charcoal")
                {
                    WaffleFlavour = "Charcoal";
                }
                else if (newWaffleFlavour.ToLower() == "pandan")
                {
                    WaffleFlavour = "Pandan";
                }
                else if (newWaffleFlavour == "1" || newWaffleFlavour == "2" || newWaffleFlavour == "3" || newWaffleFlavour == "4")
                {
                    int index = Convert.ToInt32(newWaffleFlavour);
                    WaffleFlavour = waffleFlavourList[index - 1].Split(",")[0];
                }
                else
                {
                    Console.WriteLine("Please reply with an option from the above menu");
                    continue;
                }

                break;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "Waffle Flavour \n" +  $"- {waffleFlavour} \n"; 
        }
    }
}
