//==========================================================
// Student Number : S10257400
// Student Name : See Wai Kee, Audrey
//==========================================================

using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Order
    {
        private int id;
        private DateTime timeReceived;
        private DateTime? timeFulfilled;
        private List<IceCream> iceCreamList;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime TimeReceived
        {
            get { return timeReceived; }
            set { timeReceived = value; }
        }

        public DateTime? TimeFulfilled
        {
            get { return timeFulfilled; }
            set { timeFulfilled = value; }
        }

        public List<IceCream> IceCreamList
        {
            get { return iceCreamList; }
            set { iceCreamList = value; }
        }

        public Order() { }

        public Order(int id, DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
            IceCreamList = new List<IceCream>();
        }

        public void ModifyIceCream(int index)   // method to determine how to modify ice cream object, not found in the class diagram
        {
            IceCream iceCreamToChange = iceCreamList[index - 1];
            int modificationChoice = IceCreamModificationMenu();
            
            if (modificationChoice == 1)
            {
                IceCream newIceCream = ModifyIceCreamType(iceCreamToChange);
                DeleteIceCream(index - 1);
                AddIceCream(newIceCream);
            }
            else if (modificationChoice == 2)
            {
                ModifyIceCreamScoops(iceCreamToChange);
                ModifyIceCreamFlavours(iceCreamToChange);
            }
            else if (modificationChoice == 3)
            {
                ModifyIceCreamFlavours(iceCreamToChange);
            }
            else if (modificationChoice == 4)
            {
                ModifyIceCreamToppings(iceCreamToChange);
            }
            else if (modificationChoice == 5)
            {
                if (iceCreamToChange is Cone)
                {
                    Cone cone = (Cone)iceCreamToChange;
                    cone.ModifyConeFlavour();
                }
                else
                {
                    Console.WriteLine("\n" + "Ice Cream chosen in not a Cone Ice Cream" + "\n" + "Please choose a Cone Ice Cream");
                }
            }
            else 
            {
                if (iceCreamToChange is Waffle)
                {
                    Waffle waffle = (Waffle)iceCreamToChange;
                    waffle.ModifyWaffleFlavour();
                }
                else
                {
                    Console.WriteLine("\n" + "Ice Cream chosen in not a Waffle Ice Cream" + "\n" + "Please choose a Waffle Ice Cream");
                }
            }
        }

        public int IceCreamModificationMenu()   // method to display types of modifications user can make to their ice cream object, not found in the class diagram
        {
            Console.WriteLine("Parts of Ice Cream that can be changed \n" + "[1] Type of Ice Cream \n" + "[2] Number of Scoops \n" + 
                "[3] Flavour of Ice Cream \n" + "[4] Toppings chosen \n" + "[5] Change the cone flavour \n" + "[6] Change the waffle flavour");

            int choice;

            while (true)        // Loop to ensure user input is an integer between 1 and 6
            {
                Console.Write("Enter your option: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option. Please enter an option from 1 to 6 as seen in above the menu.");
                    continue;
                }

                if (choice >= 1 && choice <= 6)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter an option from 1 to 6 as seen in above the menu.");
                }
            }
        }

        public IceCream ModifyIceCreamType(IceCream iceCreamToChange)
        {
            List<string> iceCreamTypeList = new List<string> { "Cup", "Cone", "Waffle" };

            int x = 1;
            iceCreamTypeList.Remove(iceCreamToChange.Option);
            Console.WriteLine("\n" + "Types of Ice Cream you can change to");
            foreach (string iceCreamOption in iceCreamTypeList)
            {
                Console.WriteLine($"[{x}] {iceCreamOption}");
                x++;
            }

            Console.WriteLine();

            while (true)
            {
                Console.Write("What do you want to change it to? ");
                string newIceCream = Console.ReadLine();

                if ((newIceCream == "1" && iceCreamTypeList[0] == "Cup") || (newIceCream == "2" && iceCreamTypeList[1] == "Cup") || newIceCream == "cup" || newIceCream == "Cup")
                {
                    IceCream iceCream = new Cup("Cup", iceCreamToChange.Scoops, iceCreamToChange.Flavours, iceCreamToChange.Toppings);
                    return iceCream;
                }
                else if ((newIceCream == "1" && iceCreamTypeList[0] == "Cone") || (newIceCream == "2" && iceCreamTypeList[1] == "Cone") || newIceCream == "cone" || newIceCream == "Cone")
                {
                    IceCream iceCream = new Cone("Cone", iceCreamToChange.Scoops, iceCreamToChange.Flavours, iceCreamToChange.Toppings, false);
                    Cone cone = (Cone)iceCream;
                    cone.ModifyConeFlavour();
                    return iceCream;
                }
                else if ((newIceCream == "1" && iceCreamTypeList[0] == "Waffle") || (newIceCream == "2" && iceCreamTypeList[1] == "Waffle") || newIceCream == "waffle" || newIceCream == "Waffle")
                {
                    IceCream iceCream = new Waffle("Waffle", iceCreamToChange.Scoops, iceCreamToChange.Flavours, iceCreamToChange.Toppings, "null");
                    Waffle waffle = (Waffle)iceCream;
                    waffle.ModifyWaffleFlavour();
                    return iceCream;
                }
                else
                {
                    Console.WriteLine("Please enter a choice from the list shown above.");
                    continue;
                }
            }
        }

        public void ModifyIceCreamScoops(IceCream iceCreamToChange)
        {
            List<int> availableOption = new List<int> { 1, 2, 3 };
            availableOption.Remove(iceCreamToChange.Scoops);

            int numIceCreamScoops;
            while (true)
            {
                Console.Write("\n" + "Enter the updated number of Ice Cream scoops: ");

                try
                {
                    numIceCreamScoops = Convert.ToInt32(Console.ReadLine());
                    
                    if (availableOption.Contains(numIceCreamScoops))
                    {
                        break;
                    }
                    else if (numIceCreamScoops == iceCreamToChange.Scoops)
                    {
                        Console.WriteLine($"Please do not enter the previously entered quantity of {iceCreamToChange.Scoops} sccops of Ice Cream");
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a quantity that is either {availableOption[0]} scoop(s) or {availableOption[1]} scoop(s)");
                    }
                }
                catch
                {
                    Console.WriteLine($"Please enter a quantity that is either {availableOption[0]} scoop(s) or {availableOption[1]} scoop(s)");
                }
            }

            iceCreamToChange.Scoops = numIceCreamScoops;
        }

        public void ModifyIceCreamFlavours(IceCream iceCreamToChange)
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

            List<Flavour> customerflavourList = new List<Flavour>();
            bool premiumIceCream = false;
            string chosenFlavour;
            int remainingScoops = iceCreamToChange.Scoops;
            while (true)
            {
                Console.Write("\n" + "Which Ice Cream Flavour do you want? ");
                chosenFlavour = Console.ReadLine();
                int flavourIndex;
                bool correctFlavour = false;

                try
                {
                    flavourIndex = Convert.ToInt32(chosenFlavour);

                    if (flavourIndex >= 1 && flavourIndex <= iceCreamFlavourList.Count())
                    {
                        string[] info = iceCreamFlavourList[flavourIndex - 1].Split(",");
                        chosenFlavour = info[0];

                        if (chosenFlavour == "Durian" || chosenFlavour == "Ube" || chosenFlavour == "Sea salt")
                        {
                            premiumIceCream = true;
                        }

                        correctFlavour = true;
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

                    for (int i = 0; i < iceCreamFlavourList.Count; i++)
                    {
                        string[] iceCreamInfo = iceCreamFlavourList[i].Split(",");

                        if (iceCreamInfo[0].ToLower() == chosenFlavour)
                        {
                            chosenFlavour = iceCreamInfo[0];

                            if (chosenFlavour.ToLower() == "durian" || chosenFlavour.ToLower() == "ube" || chosenFlavour.ToLower() == "sea salt")
                            {
                                premiumIceCream = true;
                            }

                            correctFlavour = true;
                            break;
                        }
                        else if (chosenFlavour.ToLower() == "seasalt")
                        {
                            chosenFlavour = "Sea salt";
                            premiumIceCream = true;
                        }
                        else
                        {
                            Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours.");
                            break;
                        }
                    }
                }

                if (!correctFlavour)
                {
                    continue;
                }

                bool enteredFlavoursForAllIceCream = false;
                int orderedQuantity;

                while (true)
                {
                    Console.Write($"How many scoops of {chosenFlavour} ice cream do you wish to order? ");

                    try
                    {
                        orderedQuantity = Convert.ToInt32(Console.ReadLine());

                        if (orderedQuantity < 1)
                        {
                            Console.WriteLine($"Invalid quantity. You cannot order 0 or less scoops of {chosenFlavour} ice cream.");
                            continue;
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Invalid quantity. Enter an integer from 1 to {iceCreamToChange.Scoops}");
                        continue;
                    }

                    if (orderedQuantity > remainingScoops)
                    {
                        Console.WriteLine($"Invalid quantity. You cannot order more than {remainingScoops} scoops of {chosenFlavour} ice cream.");
                    }
                    else
                    {
                        bool addFlavour = false;
                        foreach (Flavour flavour in customerflavourList)
                        {
                            if (flavour.Type == chosenFlavour)
                            {
                                flavour.Quantity += orderedQuantity;
                                addFlavour = true;
                            }
                        }
                        
                        if (!addFlavour)
                        {
                            customerflavourList.Add(new Flavour(chosenFlavour, premiumIceCream, orderedQuantity));
                        }
                        
                        remainingScoops -= orderedQuantity;

                        if (remainingScoops == 0)
                        {
                            enteredFlavoursForAllIceCream = true;
                        }

                        break;
                    }
                }

                if (enteredFlavoursForAllIceCream)
                {
                    break;
                }
            }

            if (iceCreamToChange.Flavours.SequenceEqual(customerflavourList))
            {
                iceCreamToChange.Flavours = customerflavourList;
            }
            else
            {
                Console.WriteLine("\n" + "The modified list of Ice Cream Flavours is the same as the original list of Ice Cream Flavours");
            }
        }

        public void ModifyIceCreamToppings(IceCream iceCreamToChange)
        {
            List<Topping> customerToppingList = new List<Topping>();

            string[] csvLines = File.ReadAllLines("toppings.csv");
            Console.WriteLine("\n" + "Ice Cream Toppings that are Available and their Cost" + "\n" + "{0,-17} {1}", "Toppings", "Add On Cost");
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(",");
                Console.WriteLine($"[{i}] {info[0],-13} {info[1]}");
            }
            Console.WriteLine();
            string[] numberSuffix = { "2nd", "3rd", "4th" };

            for (int count = 1; count <= 4; count++)
            {
                Console.Write(count == 1 ? "Which topping do you want? " : $"Do you wish to add a {numberSuffix[count-2]} topping to your ice cream [Y/N]? ");
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

            if (iceCreamToChange.Toppings.SequenceEqual(customerToppingList))
            {
                iceCreamToChange.Toppings = customerToppingList;
            }
            else
            {
                Console.WriteLine("\n" + "The modified list of Ice Cream Topping(s) is the same as the original list of Ice Cream Topping(s)");
            }
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int index)
        {
            IceCreamList.RemoveAt(index);
        }

        public double CalculateTotal()
        {
            double total = 0;

            foreach (IceCream iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice();
            }

            return total;
        }

        public override string ToString()
        {
            StringBuilder orderInfo = new StringBuilder();

            orderInfo.Append($"Order ID: {Id:D2} \n");
            orderInfo.Append($"Timestamp: {TimeReceived.ToString("G")} \n");


            foreach (IceCream iceCream in IceCreamList)
            {
                orderInfo.Append($"{iceCream}");
            }

            return orderInfo.ToString();
        }
    }
}
