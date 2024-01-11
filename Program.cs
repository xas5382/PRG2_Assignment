using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();

            Queue<Order> regularQueue = new Queue<Order>();
            Queue<Order> goldQueue = new Queue<Order>();

            List<string> iceCreamFlavourList = new List<string> { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea Salt" };

            bool correctFile = CreateCustomers(customerDict);

            while (correctFile)
            {
                int choice = DisplayMenu();

                if (choice == 1)
                {
                    DisplayCustomers(customerDict);
                }
                else if (choice == 2)
                {
                    ListCurrentOrders(regularQueue, goldQueue);
                }
                else if (choice == 4)
                {
                    DisplayCustomers(customerDict);
                    Customer customer = SelectCustomer(customerDict);
                    Order newOrder = new Order(regularQueue.Count() + 1, DateTime.Now);

                    if (customer != null)
                    {
                        IceCream iceCream = CreateCustomerOrder(customerDict, customer, iceCreamFlavourList);
                        newOrder.AddIceCream(iceCream);

                        while (true)
                        {
                            Console.Write("Would you like to add another ice cream to the order [Y/N]? ");
                            string addIceCream = Console.ReadLine();

                            if (addIceCream.ToLower() == "n")
                            {
                                break;
                            }
                            else if (addIceCream.ToLower() == "y")
                            {
                                Console.WriteLine();
                                IceCream additionalIceCream = CreateCustomerOrder(customerDict, customer, iceCreamFlavourList);
                                newOrder.AddIceCream(additionalIceCream);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                            }
                        }

                        customer.CurrentOrder = newOrder;

                        while (true)
                        {
                            Console.Write("Do you have a gold-tier Pointcard [Y/N]? ");

                            string goldPointCard = Console.ReadLine();

                            if (goldPointCard.ToLower() == "n")
                            {
                                regularQueue.Enqueue(newOrder);
                                Console.WriteLine("Order has been made successfully");
                                break;
                            }
                            else if (goldPointCard.ToLower() == "y")
                            {
                                newOrder.Id = goldQueue.Count() + 1;
                                goldQueue.Enqueue(newOrder);
                                Console.WriteLine("Order has been made successfully");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                            }
                        }
                    }
                }
                else if (choice == 6)
                {
                    DisplayCustomers(customerDict);
                    Customer customer = SelectCustomer(customerDict);

                    if (customer != null)
                    {
                        Order customerOrder = customer.CurrentOrder;
                        Console.WriteLine();
                        Console.WriteLine($"Order for {customer.Name}");
                        Console.WriteLine(customerOrder);

                        int optionChosen = OrderModificationMenu();
                        Console.WriteLine();

                        if (optionChosen == 1)
                        {
                            int modificationChoice = IceCreamModificationMenu();
                            if (modificationChoice == 1)
                            {
                                ModifyType(customerOrder);
                                Console.WriteLine(customerOrder);
                            }
                            else if (modificationChoice == 2)
                            {
                                ModifyNumOfScoops(customerOrder);
                            }
                            else if (modificationChoice == 3)
                            {
                                ModifyFlavours(customerOrder, iceCreamFlavourList);
                                Console.WriteLine(customerOrder);
                            }
                        }
                        else if (optionChosen == 2)
                        {
                            Order newOrder = new Order(regularQueue.Count() + 1, DateTime.Now);
                            IceCream iceCream = CreateCustomerOrder(customerDict, customer, iceCreamFlavourList);
                            newOrder.AddIceCream(iceCream);
                            customer.CurrentOrder = newOrder;

                            while (true)
                            {
                                Console.Write("Do you have a gold-tier Pointcard [Y/N]? ");

                                string goldPointCard = Console.ReadLine();

                                if (goldPointCard.ToLower() == "n")
                                {
                                    regularQueue.Enqueue(newOrder);
                                    Console.WriteLine("Order has been made successfully");
                                    break;
                                }
                                else if (goldPointCard.ToLower() == "y")
                                {
                                    newOrder.Id = goldQueue.Count() + 1;
                                    goldQueue.Enqueue(newOrder);
                                    Console.WriteLine("Order has been made successfully");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                                }
                            }
                        }
                        else
                        {
                            if (customerOrder.IceCreamList.Count() == 1)
                            {
                                Console.WriteLine("You cannot have zero ice creams order in your order");
                            }
                            else
                            {

                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Customer {customer.Name} with ID of {customer.MemberId} does not have an existing order.");
                        Console.WriteLine($"Enter the member ID of another customer or add an order for {customer.Name}.");
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Goodbye");
                    break;
                }

                Console.WriteLine();
            }
        }

        static bool CreateCustomers(Dictionary<int, Customer> customerDict)
        {
            string[] csvLines = File.ReadAllLines("customers.csv");
            string firstLine = csvLines[0];

            bool correctFile = false;

            if (firstLine.Contains("Name") && firstLine.Contains("MemberId") && firstLine.Contains("DOB"))
            {
                correctFile = true;
            }
            if (!correctFile)
            {
                Console.WriteLine("Please check the provided file to ensure that it contains the required information.");
            }

            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(',');

                int memberID = Convert.ToInt32(info[1]);
                DateTime dob = Convert.ToDateTime(info[2]);

                customerDict.Add(memberID, new Customer(info[0], memberID, dob));
            }

            return correctFile;
        }

        static int DisplayMenu()
        {
            Console.WriteLine("---------------- M E N U -----------------");
            Console.WriteLine("[1] List all customers");
            Console.WriteLine("[2] List all current orders");
            Console.WriteLine("[3] Register a new customer");
            Console.WriteLine("[4] Create a customer’s order");
            Console.WriteLine("[5] Display order details of a customer");
            Console.WriteLine("[6] Modify order details");
            Console.WriteLine("[0] Exit");
            Console.WriteLine("------------------------------------------");

            int choice;

            while (true)        // Loop to ensure user input is an integer between 0 and 5
            {
                Console.Write("Enter your option: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option. Please enter a number from the menu.");
                    continue;
                }

                if (choice >= 0 && choice <= 6)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number from the menu.");
                }
            }
        }

        static void DisplayCustomers(Dictionary<int, Customer> customerDict)
        {
            Console.WriteLine("{0,-11} {1,-12} {2}", "Name", "Member ID", "Date Of Birth");

            foreach (KeyValuePair<int, Customer> kvp in customerDict)
            {
                Console.WriteLine(kvp.Value);
            }
        }

        static void ListCurrentOrders(Queue<Order> regularQueue, Queue<Order> goldQueue)
        {
            if (regularQueue.Count == 0)
            {
                Console.WriteLine("There are no orders in the regular members queue.");
            }
            else
            {
                Console.WriteLine("Queue System for Sliver and Ordinary Member(s)");
                Console.WriteLine();

                foreach (Order order in regularQueue)
                {
                    Console.WriteLine(order);
                }
            }

            if (goldQueue.Count == 0)
            {
                Console.WriteLine("There are no orders in the gold members queue.");
            }
            else
            {
                Console.WriteLine("Queue System for Gold Member(s)");

                Console.WriteLine();

                foreach (Order order in regularQueue)
                {
                    Console.WriteLine(order);
                }
            }
        }

        static Customer SelectCustomer(Dictionary<int, Customer> customerDict)
        {
            Console.WriteLine();

            int memberID;

            while (true)
            {
                Console.Write("Enter the member ID: ");

                try
                {
                    memberID = Convert.ToInt32(Console.ReadLine());

                    if (customerDict.TryGetValue(memberID, out Customer customer))
                    {
                        return customer; // Customer found
                    }
                    else
                    {
                        Console.WriteLine($"Customer with member ID {memberID} does not exist. Please add the customer first.");
                        return null; // Customer not found
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid member ID.");
                }
            }
        }

        static IceCream CreateCustomerOrder(Dictionary<int, Customer> customerDict, Customer customer, List<string> iceCreamFlavourList)
        {
            string typeOfIceCream = GetIceCreamType();
            int scoopsOfIceCream = GetNumScoopsOfIceCream();

            bool dippedCone = false;
            string waffleFlavour = null;

            if (typeOfIceCream.ToLower() == "cone")
            {
                dippedCone = GetDippedCone();
            }
            else if (typeOfIceCream.ToLower() == "waffle")
            {
                waffleFlavour = GetWaffleFlavour();
            }

            Console.WriteLine();
            Console.WriteLine("Flavours of Ice Cream that are Available");
            Console.WriteLine("[1] Vanilla \n" + "[2] Chocolate \n" + "[3] Strawberry \n" + "[4] Durain \n" + "[5] Ube \n" +
                "[6] Sea Salt");
            Console.WriteLine("Note: The Durian, Ube and Sea Salt Ice Cream flavours costs an additional $2.");
            Console.WriteLine();

            List<Flavour> flavourList = GetIceCreamFlavours(iceCreamFlavourList, scoopsOfIceCream);

            Console.WriteLine();

            Console.WriteLine("Ice Cream Toppings that are Available");
            Console.WriteLine("[1] Sprinkles \n" + "[2] Mochi \n" + "[3] Sago \n" + "[4] Oreos");
            Console.WriteLine("Note: Each topping costs an additional $1. A maximum of 4 toppings can be ordered.");
            Console.WriteLine();

            List<Topping> toppingList = GetToppings();

            Console.WriteLine();

            if (typeOfIceCream.ToLower() == "cup")
            {
                IceCream iceCream = new Cup(typeOfIceCream, scoopsOfIceCream, flavourList, toppingList);
                return iceCream;
            }
            else if (typeOfIceCream.ToLower() == "cone")
            {
                IceCream iceCream = new Cone(typeOfIceCream, scoopsOfIceCream, flavourList, toppingList, dippedCone);
                return iceCream;
            }
            else
            {
                IceCream iceCream = new Waffle(typeOfIceCream, scoopsOfIceCream, flavourList, toppingList, waffleFlavour);
                return iceCream;
            }
        }

        static string GetIceCreamType()
        {
            Console.WriteLine("Types of Ice Cream Available");
            Console.WriteLine("[1] Cup \n" + "[2] Cone \n" + "[3] Waffle");

            string iceCreamOption;
            while (true)
            {
                Console.Write("Enter type of ice cream desired: ");
                iceCreamOption = Console.ReadLine();

                if (iceCreamOption == "1")
                {
                    return "Cup";
                }
                else if (iceCreamOption == "2")
                {
                    return "Cone";
                }
                else if (iceCreamOption == "3")
                {
                    return "Waffle";
                }
                else if (iceCreamOption.ToLower() != "cup" && iceCreamOption.ToLower() != "cone" && iceCreamOption.ToLower() != "waffle")
                {
                    Console.WriteLine("Please enter an available option that is shown above.");
                }
                else
                {
                    return (char.ToUpper(iceCreamOption[0]) + iceCreamOption.Substring(1));
                }
            }
        }

        static int GetNumScoopsOfIceCream()
        {
            int scoopsOfIceCream;

            while (true)
            {
                Console.Write("Enter number of scoops: ");
                try
                {
                    scoopsOfIceCream = Convert.ToInt32(Console.ReadLine());

                    if (scoopsOfIceCream <= 0)
                    {
                        Console.WriteLine("You need to order at least 1 scoop of ice cream.");
                    }
                    else if (scoopsOfIceCream > 3)
                    {
                        Console.WriteLine("You can only order at most 3 scoops of ice cream.");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid number of ice cream scoops ordered. Please enter a quantity from 1 to 3.");
                }
            }

            Console.WriteLine();
            return scoopsOfIceCream;
        }

        static bool GetDippedCone()
        {
            while (true)
            {
                Console.Write("Do you upgrade your cone to a chocolate-dipped cone for an additional $2 [Y/N]? ");
                string chocoCone = Console.ReadLine();

                if (chocoCone.ToLower() == "y")
                {
                    return true;
                }
                else if (chocoCone.ToLower() == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                }
            }
        }

        static string GetWaffleFlavour()
        {
            string waffleFlavour;

            Console.WriteLine("Types of Premium Waffle Flavours Available");
            Console.WriteLine("[1] Red Velvet \n" + "[2] Charcoal \n" + "[3] Pandan");

            while (true)
            {
                Console.Write("Would you like to upgrade your waffle to one of our premium flavours for an additional cost of $3 [Y/N]? ");
                string premiumWaffle = Console.ReadLine();

                if (premiumWaffle.ToLower() == "y")
                {
                    while (true)
                    {
                        Console.Write("Which flavour would you like? ");
                        waffleFlavour = Console.ReadLine();

                        if (waffleFlavour == "1")
                        {
                            waffleFlavour = "Red Velvet";
                            break;
                        }
                        else if (waffleFlavour == "2")
                        {
                            waffleFlavour = "Charcoal";
                            break;
                        }
                        else if (waffleFlavour == "3")
                        {
                            waffleFlavour = "Pandan";
                            break;
                        }
                        else if (waffleFlavour.ToLower() != "red velvet" && waffleFlavour.ToLower() != "redvelvet" && waffleFlavour.ToLower() != "charcoal" && waffleFlavour.ToLower() != "pandan")
                        {
                            Console.WriteLine("Please enter an available option that is shown above");
                        }
                        else
                        {
                            waffleFlavour = char.ToUpper(waffleFlavour[0]) + waffleFlavour.Substring(1);
                            break;
                        }
                    }

                    break;
                }
                else if (premiumWaffle.ToLower() == "n")
                {
                    waffleFlavour = "Original";
                    break;
                }
                else
                {
                    Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                }
            }

            return waffleFlavour;
        }

        static List<Flavour> GetIceCreamFlavours(List<string> iceCreamFlavourList, int scoopsOfIceCream)
        {
            List<Flavour> flavourList = new List<Flavour>();

            bool premiumIceCream = false;
            string chosenFlavour;
            int remainingScoops = scoopsOfIceCream;

            while (true)
            {
                Console.Write("Enter your desired flavour of ice cream: ");
                chosenFlavour = Console.ReadLine();
                int flavourIndex;

                try
                {
                    flavourIndex = int.Parse(chosenFlavour);

                    if (flavourIndex >= 1 && flavourIndex <= iceCreamFlavourList.Count())
                    {
                        chosenFlavour = iceCreamFlavourList[flavourIndex - 1];

                        if (chosenFlavour == "Durian" || chosenFlavour == "Ube" || chosenFlavour == "Sea Salt")
                        {
                            premiumIceCream = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours.");
                        continue;
                    }
                }
                catch (FormatException)
                {
                    if (iceCreamFlavourList.Contains(chosenFlavour, StringComparer.OrdinalIgnoreCase))
                    {
                        chosenFlavour = char.ToUpper(chosenFlavour[0]) + chosenFlavour.Substring(1);

                        if (chosenFlavour.ToLower() == "durain" || chosenFlavour.ToLower() == "ube" || chosenFlavour.ToLower() == "sea salt")
                        {
                            premiumIceCream = true;
                        }
                    }
                    else if (chosenFlavour.ToLower() == "seasalt")
                    {
                        chosenFlavour = char.ToUpper(chosenFlavour[0]) + chosenFlavour.Substring(1);
                        premiumIceCream = true;
                    }
                    else
                    {
                        Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours.");
                        continue;
                    }
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
                        Console.WriteLine($"Invalid quantity. Enter an integer from 1 to {scoopsOfIceCream}");
                        continue;
                    }

                    if (orderedQuantity > remainingScoops)
                    {
                        Console.WriteLine($"Invalid quantity. You cannot order more than {remainingScoops} scoops of {chosenFlavour} ice cream.");
                    }
                    else
                    {
                        remainingScoops -= orderedQuantity;
                        flavourList.Add(new Flavour(chosenFlavour, premiumIceCream, orderedQuantity));

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

            return flavourList;
        }

        static List<Topping> GetToppings()
        {
            List<Topping> toppingList = new List<Topping>();

            string orderedToppings;
            int maxToppings = 4;

            for (int count = 1; count <= maxToppings; count++)
            {
                Console.Write(count == 1 ? "Do you wish to add toppings to your ice cream [Y/N]? " : "Do you wish to add more toppings to your ice cream [Y/N]? ");
                string addToppings = Console.ReadLine();

                if (addToppings.ToLower() == "n")
                {
                    break;
                }
                else if (addToppings.ToLower() == "y")
                {
                    while (count <= 4)
                    {
                        Console.Write("Enter your desired topping: ");
                        orderedToppings = Console.ReadLine();

                        if (orderedToppings == "1")
                        {
                            toppingList.Add(new Topping("Sprinkles"));
                            break;
                        }
                        else if (orderedToppings == "2")
                        {
                            toppingList.Add(new Topping("Mochi"));
                            break;
                        }
                        else if (orderedToppings == "3")
                        {
                            toppingList.Add(new Topping("Sago"));
                            break;
                        }
                        else if (orderedToppings == "4")
                        {
                            toppingList.Add(new Topping("Oreos"));
                            break;
                        }
                        else if (orderedToppings.ToLower() == "sprinkles" || orderedToppings.ToLower() == "mochi" ||
                            orderedToppings.ToLower() == "sago" || orderedToppings.ToLower() == "oreos")
                        {
                            orderedToppings = char.ToUpper(orderedToppings[0]) + orderedToppings.Substring(1);
                            toppingList.Add(new Topping(orderedToppings));
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid topping entered. Please enter a valid topping from the list of available toppings found above.");
                            continue;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                    count--;
                }
            }

            return toppingList;
        }

        static int OrderModificationMenu()
        {
            Console.WriteLine("Options to Modify your Order");
            Console.WriteLine("[1] Choose an existing ice cream object to modify \n" +
                "[2] Add an entirely new ice cream object to the order \n" +
                "[3] Choose an existing ice cream object to delete from the order");

            int choice;

            while (true)        // Loop to ensure user input is an integer between 1 and 3
            {
                Console.Write("Enter your option: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option. Please enter an option from 1 to 3 as seen in above the menu.");
                    continue;
                }

                if (choice >= 1 && choice <= 3)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter an option from 1 to 3 as seen in above the menu.");
                }
            }
        }

        static int IceCreamModificationMenu()
        {
            Console.WriteLine("Parts of Ice Cream that can be changed");
            Console.WriteLine("[1] Type of Ice Cream \n" + "[2] Number of Scoops \n" + "[3] Flavour of Ice Cream \n" +
                "[4] Toppings chosen \n" + "[5] Change the cone flavour \n" + "[6] Change the waffle flavour");

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

        static void ModifyType(Order customerOrder)
        {
            List<string> iceCreamTypeList = new List<string> { "Cup", "Cone", "Waffle" };

            Console.WriteLine();
            Console.WriteLine($"You have {customerOrder.IceCreamList.Count()} ice creams in your order");

            IceCream originalIceCream = GetIceCreamToChange(customerOrder);

            int x = 1;
            List<string> availableOption = new List<string>();
            Console.WriteLine();

            foreach (IceCream iceCream in customerOrder.IceCreamList)
            {
                Console.WriteLine("Types of Ice Cream you can change to");

                foreach (string iceCreamOption in iceCreamTypeList)
                {
                    if (iceCreamOption == originalIceCream.Option)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"[{x}] {iceCreamOption}");
                        availableOption.Add(iceCreamOption);
                        x++;
                    }
                }
            }

            Console.WriteLine();

            while (true)
            {
                Console.Write("What do you want to change it to? ");
                string newIceCream = Console.ReadLine();

                if (newIceCream == "1")
                {
                    if (availableOption[0] == "Cup")
                    {
                        ReplaceWithCup(customerOrder, originalIceCream);
                    }
                    else if (availableOption[0] == "Cone")
                    {
                        bool dippedCone = GetDippedCone();
                        ReplaceWithCone(customerOrder, originalIceCream, dippedCone);
                    }
                    else if (availableOption[0] == "Waffle")
                    {
                        string waffleFlavour = GetWaffleFlavour();
                        ReplaceWithWaffle(customerOrder, originalIceCream, waffleFlavour);
                    }

                    break;
                }
                else if (newIceCream == "2")
                {
                    if (availableOption[1] == "Cup")
                    {
                        ReplaceWithCup(customerOrder, originalIceCream);
                    }
                    else if (availableOption[1] == "Cone")
                    {
                        bool dippedCone = GetDippedCone();
                        ReplaceWithCone(customerOrder, originalIceCream, dippedCone);
                    }
                    else if (availableOption[1] == "Waffle")
                    {
                        string waffleFlavour = GetWaffleFlavour();
                        ReplaceWithWaffle(customerOrder, originalIceCream, waffleFlavour);
                    }

                    break;
                }
                else if (newIceCream == "cup" || newIceCream == "Cup")
                {
                    ReplaceWithCup(customerOrder, originalIceCream);

                    break;
                }
                else if (newIceCream == "cone" || newIceCream == "Cone")
                {
                    bool dippedCone = GetDippedCone();
                    ReplaceWithCone(customerOrder, originalIceCream, dippedCone);
                    break;
                }
                else if (newIceCream == "waffle" || newIceCream == "Waffle")
                {
                    string waffleFlavour = GetWaffleFlavour();
                    ReplaceWithWaffle(customerOrder, originalIceCream, waffleFlavour);
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a choice from the list shown above.");
                }
            }
        }

        static IceCream GetIceCreamToChange(Order customerOrder)
        {
            foreach (IceCream iceCream in customerOrder.IceCreamList)
            {
                if (customerOrder.IceCreamList.Count() == 1)
                {
                    Console.WriteLine("You only have one ice cream in your order. Hence this ice cream will be modified.");
                    return iceCream;
                }

                while (true)
                {
                    Console.Write($"Do you wish to change yourZZZ order [Y/N]? ");
                    string changeOrder = Console.ReadLine();

                    if (changeOrder.ToLower() == "y")
                    {
                        return iceCream;
                    }
                    else if (changeOrder.ToLower() == "n" && customerOrder.IceCreamList.Last() == iceCream)
                    {
                        Console.WriteLine("This is the last ice cream in your order. Hence this ice cream will be modified.");
                        return iceCream;
                    }
                    else if (changeOrder.ToLower() == "n")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                    }
                }
            }

            return null;
        }

        static void ReplaceWithCup(Order customerOrder, IceCream originalIceCream)
        {
            IceCream replacementIceCream = new Cup("Cup", originalIceCream.Scoops, originalIceCream.Flavours, originalIceCream.Toppings);
            customerOrder.AddIceCream(replacementIceCream);
            customerOrder.DeleteIceCream(originalIceCream);
        }

        static void ReplaceWithCone(Order customerOrder, IceCream originalIceCream, bool dippedCone)
        {
            IceCream replacementIceCream = new Cone("Cone", originalIceCream.Scoops, originalIceCream.Flavours, originalIceCream.Toppings, dippedCone);
            customerOrder.AddIceCream(replacementIceCream);
            customerOrder.DeleteIceCream(originalIceCream);
        }

        static void ReplaceWithWaffle(Order customerOrder, IceCream originalIceCream, string waffleFlavour)
        {
            IceCream replacementIceCream = new Waffle("Waffle", originalIceCream.Scoops, originalIceCream.Flavours, originalIceCream.Toppings, waffleFlavour);
            customerOrder.AddIceCream(replacementIceCream);
            customerOrder.DeleteIceCream(originalIceCream);
        }

        static void ModifyNumOfScoops(Order customerOrder)
        {
            IceCream originalIceCream = GetIceCreamToChange(customerOrder);
        }

        static void ModifyFlavours(Order customerOrder, List<string>iceCreamFlavourList)
        {  
            IceCream originalIceCream = GetIceCreamToChange(customerOrder);

            int x = 1;
            List<string> availableOption = new List<string>();
            Console.WriteLine();

            Console.WriteLine("Ice Cream flavours that you can change to");
            /*
            foreach (Flavour flavour in originalIceCream.Flavours)
            {
                if (iceCreamFlavourList.Contains(flavour.Type))
                {
                    iceCreamFlavourList.Remove(flavour.Type);
                }
            }
            */

            foreach (string iceCreamFlavour in iceCreamFlavourList)
            {
                Console.WriteLine($"[{x}] {iceCreamFlavour}");
                availableOption.Add(iceCreamFlavour);
                x++;
            }

            Console.WriteLine();
            Console.WriteLine($"You have ordered {originalIceCream.Scoops} scoops of Ice Cream");
            
            List<Flavour> newFlavourList = GetIceCreamFlavours(iceCreamFlavourList, originalIceCream.Scoops);
            originalIceCream.Flavours = newFlavourList;

            Console.WriteLine();
        }
    }
}

/*             
 *              Console.Write("Which part of your ice cream order do you want to modify?");
 *              bool validFlavour = false;

                foreach (string flavour in iceCreamFlavourList)
                {
                    if (flavour.ToLower() == flavourType)
                    {
                        flavourType = char.ToUpper(flavourType[0]) + flavourType.Substring(1);
                        validFlavour = true;

                        if (flavourType.ToLower() == "durain" || flavourType.ToLower() == "ube" ||
                                flavourType.ToLower() == "sea salt")
                        {
                            premiumIceCream = true;
                            break;
                        }
                    }
                    else if (flavour.ToLower() == "seasalt")
                    {
                        flavourType = char.ToUpper(flavourType[0]) + flavourType.Substring(1);
                        premiumIceCream = true;
                        validFlavour = true;
                        break;
                    }
                }
                if (!validFlavour)
                {
                    Console.WriteLine("Please choose an ice cream flavour from the list of available ice cream flavours.");
                }


 *              foreach (Order order in regularQueue)
                {
                    List<IceCream> iceCreamList = order.IceCreamList;
                    int orderNumber = 1;
                    string orderID;

                    foreach (IceCream iceCream in iceCreamList)
                    {
                        if (iceCreamList.Count > 1) 
                        {
                            orderID = $"{order.Id:D2}/{orderNumber:D2}";
                            orderNumber++;
                        }
                        else
                        {
                            orderID = $"{order.Id:D2}";
                        }


            if (count > 4)
            {
                Console.WriteLine("You can only order a maximum of 4 toppings.");
            }

            for (int i = 0; i<=scoops; i+=quantity)
            {
                Console.Write("Enter your desired flavour of ice cream: ");

                Console.WriteLine("Flavours of Ice Cream that are Available");
                Console.WriteLine("[1] Vanilla \n" + "[2] Chocolate \n" + "[3] Strawberry" + "[4] Durain \n" + 
                    "[5] Ube \n" + "[6] Sea Salt");

                string flavourType = Console.ReadLine();

                if (flavourType.ToUpper() == "DURIAN" || flavourType.ToUpper() == "UBE" || flavourType.ToUpper() == "SEA SALT" || 
                    flavourType.ToUpper() == "SEASALT")
                {
                    premiumIceCream = true;
                }
                int orderedQuantity = 0;
                Console.Write($"Enter the number of {flavourType} scoops: ");
                try
                {
                    orderedQuantity = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException) 
                {
                    Console.WriteLine($"Invalid quantity. Enter an integer from 1 to {scoops}");
                }

                flavourList.Add(new Flavour(flavourType, premiumIceCream, quantity));
                
                if (orderedQuantity + i > scoops)
                {
                    break;
                }
            }
            */
