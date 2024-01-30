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

        public void ModifyIceCreamScoops()
        {
            List<int> availableOption = new List<int> { 1, 2, 3 };
            availableOption.Remove(Scoops);
            Console.WriteLine();
            while (true)
            {
                if (availableOption.Count() == 3)
                {
                    Console.Write("How many scoops of Ice Cream do you want? ");
                }
                else
                {
                    Console.Write("Enter the updated number of Ice Cream scoops: ");
                }

                string numIceCreamScoops = Console.ReadLine();
                try
                {
                    int intNumIceCreamScoops = Convert.ToInt32(numIceCreamScoops);

                    if (availableOption.Contains(intNumIceCreamScoops))
                    {
                        Scoops = intNumIceCreamScoops;
                        break;
                    }
                    else if (intNumIceCreamScoops == Scoops)
                    {
                        Console.WriteLine($"Please do not enter the previously entered quantity of {Scoops} scoops of Ice Cream");
                    }
                    else
                    {
                        if (availableOption.Count() == 2)
                        {
                            Console.WriteLine($"Please enter a quantity that is either {availableOption[0]} scoop(s) or {availableOption[1]} scoop(s)");
                        }
                        else
                        {
                            Console.WriteLine("Invalid number of ice cream scoops ordered. Please enter a quantity from 1 to 3. \n");
                        }
                    }
                }
                catch (FormatException)
                {
                    if (numIceCreamScoops.ToLower() == "one" && availableOption.Contains(1))
                    {
                        Scoops = 1;
                        break;
                    }
                    else if (numIceCreamScoops.ToLower() == "two" && availableOption.Contains(2))
                    {
                        Scoops = 2;
                        break;
                    }
                    else if (numIceCreamScoops.ToLower() == "three" && availableOption.Contains(3))
                    {
                        Scoops = 3;
                        break;
                    }
                    else
                    {
                        if (availableOption.Count() == 2)
                        {
                            Console.WriteLine($"Please enter a quantity that is either {availableOption[0]} scoop(s) or {availableOption[1]} scoop(s) \n");
                        }
                        else
                        {
                            Console.WriteLine("Invalid number of ice cream scoops ordered. Please enter a quantity from 1 to 3. \n");
                        }
                    }
                }
            }
        }

        public void ModifyIceCreamFlavours()
        {
            List<string> iceCreamFlavourList = new List<string>();
            string[] csvLines = File.ReadAllLines("flavours.csv");

            Console.WriteLine("\n" + "Flavours of Ice Cream that are Available and their Cost" + "\n" + "{0,-17} {1}", "Flavour", "Add On Cost");
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(",");
                iceCreamFlavourList.Add(info[0]);
                Console.WriteLine($"[{i}] {info[0],-13} {info[1]}");
            }
            Console.WriteLine("[0] {0,-13} {1}", "No Flavour", "0" + "\n");

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
                    
                    if (flavourIndex == 0)
                    {
                        chosenFlavour = $"No Flavour";
                        scoopsOfIceCream--;
                    }
                    else if (flavourIndex >= 1 && flavourIndex <= iceCreamFlavourList.Count())
                    {
                        string[] info = iceCreamFlavourList[flavourIndex - 1].Split(",");
                        chosenFlavour = info[0];
                        scoopsOfIceCream--;
                    }
                    else
                    {
                        Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours. \n");
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
                    if (chosenFlavour.ToLower() == "noflavour")
                    {
                        chosenFlavour = $"No Flavour";
                        scoopsOfIceCream--;
                        correctFlavour = true;
                    }

                    if (!correctFlavour)
                    {
                        Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours. \n");
                        continue;
                    }
                }

                if (chosenFlavour.ToLower() == "durian" || chosenFlavour.ToLower() == "ube" || chosenFlavour.ToLower() == "sea salt")
                {
                    premiumIceCream = true;
                }

                customerflavourList.Add(new Flavour(chosenFlavour, premiumIceCream));
            }

            Flavours = customerflavourList;
        }

        public void ModifyIceCreamToppings()
        {
            List<Topping> customerToppingList = new List<Topping>();
            string[] csvLines = File.ReadAllLines("toppings.csv");

            Console.WriteLine("\n" + "Ice Cream Toppings that are Available and their Cost" + "\n" + "{0,-17} {1}", "Toppings", "Add On Cost");
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(",");
                Console.WriteLine($"[{i}] {info[0],-13} {info[1]}");
            }
            Console.WriteLine("[0] {0,-13} {1}", "No Toppings", "0.00" + "\n");

            string[] numberSuffix = { "2nd", "3rd", "4th" };

            for (int count = 1; count <= 4; count++)
            {
                if (count == 1)
                {
                    Console.Write("Which topping option do you want? ");
                }
                else
                {
                    Console.Write($"What will your {numberSuffix[count - 2]} topping option be? ");
                }

                string chosenTopping = Console.ReadLine();
                chosenTopping = chosenTopping.Replace(" ", "");
                if (chosenTopping == "1" || chosenTopping.ToLower() == "sprinkles")
                {
                    customerToppingList.Add(new Topping("Sprinkles"));
                }
                else if (chosenTopping == "2" || chosenTopping.ToLower() == "mochi")
                {
                    customerToppingList.Add(new Topping("Mochi"));
                }
                else if (chosenTopping == "3" || chosenTopping.ToLower() == "sago")
                {
                    customerToppingList.Add(new Topping("Sago"));
                }
                else if (chosenTopping == "4" || chosenTopping.ToLower() == "oreos")
                {
                    customerToppingList.Add(new Topping("Oreos"));
                }
                else if (chosenTopping.ToLower() == "notoppings" || chosenTopping == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid topping entered. Please enter a valid topping from the list of available toppings found above. \n");
                    count--;
                }
            }

            Toppings = customerToppingList;
        }

        public override string ToString()
        {
            string iceCreamInfo = "";

            iceCreamInfo += ($"{Option} Ice Cream \n");
            iceCreamInfo += ($"{Scoops} Scoop(s) of Ice Cream \n");

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

            foreach (KeyValuePair <string, int> kvp in scoopesPerFlavour)
            {
                iceCreamInfo += ($"- {kvp.Value} {kvp.Key} \n");
            }

            iceCreamInfo += ("Topping(s) \n");

            if (Toppings.Count > 0)
            {
                Dictionary<string, int> quantityPerTopping = new Dictionary<string, int>();
                
                for (int i = 0; i < Toppings.Count(); i++)
                {
                    if (quantityPerTopping.ContainsKey(Toppings[i].Type))
                    {
                        quantityPerTopping[Toppings[i].Type] += 1;
                    }
                    else
                    {
                        quantityPerTopping[Toppings[i].Type] = 1;
                    }
                }

                foreach (KeyValuePair<string, int> kvp in quantityPerTopping)
                {
                    iceCreamInfo += ($"- {kvp.Value} {kvp.Key} \n");
                }
            }
            else
            {
                iceCreamInfo += ($"- None \n");
            }

            return iceCreamInfo.ToString();
        }
    }
}

/*
 
            if (Toppings.SequenceEqual(customerToppingList))
            {
                
            }
            else
            {
                Console.WriteLine("\n" + "The modified list of Ice Cream Topping(s) is the same as the original list of Ice Cream Topping(s)");
            }
            Console.WriteLine("\n" + "The modified list of Ice Cream Flavours is the same as the original list of Ice Cream Flavours");
 */
