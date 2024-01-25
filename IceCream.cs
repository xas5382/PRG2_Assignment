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

        public void ModifyIceCreamFlavours()
        {
            List<string> iceCreamFlavourList = new List<string>();
            string[] csvLines = File.ReadAllLines("flavours.csv");

            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(",");
                iceCreamFlavourList.Add(info[0]);
                Console.WriteLine($"[{i}] {info[0],-13} {info[1]}");
            }

            Console.WriteLine("\n" + "Flavours of Ice Cream that are Available and their Cost" + "\n" + "{0,-17} {1}", "Flavour", "Add On Cost");

            List<Flavour> customerflavourList = new List<Flavour>();
            string chosenFlavour;
            int scoopsOfIceCream = Scoops;

            while (scoopsOfIceCream > 0)
            {
                bool premiumIceCream = false;
                Console.Write("Enter your desired flavour of ice cream: ");
                chosenFlavour = Console.ReadLine();
                int flavourIndex;

                try
                {
                    flavourIndex = Convert.ToInt32(chosenFlavour);

                    if (flavourIndex >= 1 && flavourIndex <= iceCreamFlavourList.Count())
                    {
                        string[] info = iceCreamFlavourList[flavourIndex - 1].Split(",");
                        chosenFlavour = info[0];
                        scoopsOfIceCream--;
                    }
                    else
                    {
                        Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours.");
                        continue;
                    }
                }
                catch (FormatException)
                {
                    chosenFlavour = chosenFlavour.Replace(" ", "");
                    bool correctFlavour = false;

                    if (chosenFlavour.ToLower() == "seasalt")
                    {
                        chosenFlavour = "Sea salt";
                        scoopsOfIceCream--;
                        correctFlavour = true;
                    }

                    for (int i = 0; i < iceCreamFlavourList.Count; i++)
                    {
                        string[] iceCreamInfo = iceCreamFlavourList[i].Split(",");

                        if (iceCreamInfo[0].ToLower() == chosenFlavour)
                        {
                            chosenFlavour = iceCreamInfo[0];
                            scoopsOfIceCream--;
                            correctFlavour = true;
                            break;
                        }
                    }

                    if (!correctFlavour)
                    {
                        Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours.");
                        continue;
                    }
                }

                if (chosenFlavour.ToLower() == "durian" || chosenFlavour.ToLower() == "ube" || chosenFlavour.ToLower() == "sea salt")
                {
                    premiumIceCream = true;
                }

                customerflavourList.Add(new Flavour(chosenFlavour, premiumIceCream));
            }

            if (!Flavours.SequenceEqual(customerflavourList))
            {
                Flavours = customerflavourList;
            }
            else
            {
                Console.WriteLine("\n" + "The modified list of Ice Cream Flavours is the same as the original list of Ice Cream Flavours");
            }
        }

        public void ModifyIceCreamToppings()
        {
            List<Topping> customerToppingList = new List<Topping>();
            string[] csvLines = File.ReadAllLines("toppings.csv");
            
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(",");
                Console.WriteLine($"[{i}] {info[0],-13} {info[1]}");
            }

            Console.WriteLine("\n" + "Ice Cream Toppings that are Available and their Cost" + "\n" + "{0,-17} {1}", "Toppings", "Add On Cost" + "\n");
            string[] numberSuffix = { "2nd", "3rd", "4th" };

            for (int count = 1; count <= 4; count++)
            {
                Console.Write(count == 1 ? "Which topping do you want? " : $"Do you wish to add a {numberSuffix[count - 2]} topping to your ice cream [Y/N]? ");
                string addToppings = Console.ReadLine();

                if (addToppings.ToLower() == "n")
                {
                    break;
                }
                else if (addToppings.ToLower() == "y" || count == 1)
                {
                    if (count != 1)
                    {
                        Console.Write("Enter your desired topping: ");
                        addToppings = Console.ReadLine();
                    }

                    if (addToppings == "1")
                    {
                        customerToppingList.Add(new Topping("Sprinkles"));
                    }
                    else if (addToppings == "2")
                    {
                        customerToppingList.Add(new Topping("Mochi"));
                    }
                    else if (addToppings == "3")
                    {
                        customerToppingList.Add(new Topping("Sago"));
                    }
                    else if (addToppings == "4")
                    {
                        customerToppingList.Add(new Topping("Oreos"));
                    }
                    else if (addToppings.ToLower() == "sprinkles" || addToppings.ToLower() == "mochi" ||
                        addToppings.ToLower() == "sago" || addToppings.ToLower() == "oreos")
                    {
                        addToppings = char.ToUpper(addToppings[0]) + addToppings.Substring(1);
                        customerToppingList.Add(new Topping(addToppings));
                    }
                    else
                    {
                        Console.WriteLine("Invalid topping entered. Please enter a valid topping from the list of available toppings found above.");
                        count--;
                    }
                }
                else
                {
                    Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                    count--;
                }
            }

            if (Toppings.SequenceEqual(customerToppingList))
            {
                Toppings = customerToppingList;
            }
            else
            {
                Console.WriteLine("\n" + "The modified list of Ice Cream Topping(s) is the same as the original list of Ice Cream Topping(s)");
            }
        }

        public override string ToString()
        {
            Dictionary<string, int> scoopesPerFlavour = new Dictionary<string, int>();
            for (int i = 0; i < Flavours.Count(); i++)
            {
                if (scoopesPerFlavour.ContainsKey(Flavours[i].Type))
                {
                    scoopesPerFlavour[Flavours[i].Type] += 1;
                }
                else
                {
                    scoopesPerFlavour[Flavours[i].Type] = 1;
                }
            }

            string iceCreamInfo = "";
            
            iceCreamInfo += ($"{Option} Ice Cream \n");
            iceCreamInfo += ($"{Scoops} scoops of Ice Cream \n");
            
            foreach (KeyValuePair <string, int> kvp in scoopesPerFlavour)
            {
                iceCreamInfo += ($"- {kvp.Value} {kvp.Key} \n");
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
