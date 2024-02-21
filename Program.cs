using System;
using System.IO;

class SalesReceiptCalculator
{
    const int numberOfProducts = 5;
    const int numberOfCustomers = 3;

    static string[,] customerInfo = new string[numberOfCustomers, 5];
    static double[,] products = new double[numberOfCustomers, numberOfProducts];

    static string[] productList = { "First Item", "Second Item", "Third Item", "Fourth Item", "Last Item" };
    static double[] productCosts = { 10, 20, 30, 40, 50 };

    static string fileName = "client_data.txt";

    static void Main()
    {
        try
        {
            Console.WriteLine("\n****** Customer Sales Receipt Calculator ******");

            LoadData();

            int choice;

            do
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Enter New Purchase");
                Console.WriteLine("2. View All Purchases");
                Console.WriteLine("3. Save and Exit");
                Console.Write("Enter your choice (1-3): ");

                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                    Console.Write("Enter your choice (1-3): ");
                }

                switch (choice)
                {
                    case 1:
                        EnterNewPurchase();
                        break;
                    case 2:
                        ViewAllPurchases();
                        break;
                    case 3:
                        SaveData();
                        Console.WriteLine("Data saved successfully. Exiting...");
                        break;
                }

            } while (choice != 3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void EnterNewPurchase()
    {
        for (int customerIndex = 0; customerIndex < numberOfCustomers; customerIndex++)
        {
            Console.WriteLine($"\nEnter the following information for Customer {customerIndex + 1}:");

            Console.Write("\nFirst Name: ");
            customerInfo[customerIndex, 0] = Console.ReadLine();

            Console.Write("\nLast Name: ");
            customerInfo[customerIndex, 1] = Console.ReadLine();

            Console.Write("\nPhone Number: ");
            customerInfo[customerIndex, 2] = ValidateInput(Console.ReadLine());

            Console.Write("\nE-mail Address: ");
            customerInfo[customerIndex, 3] = ValidateInput(Console.ReadLine());

            Console.Write("\nStreet Address: ");
            customerInfo[customerIndex, 4] = Console.ReadLine();

            Console.WriteLine("\nSelect items from the following list: ");

            for (int i = 0; i < numberOfProducts; i++)
            {
                Console.WriteLine($"{productList[i]}: ${productCosts[i]}");
            }

            for (int productIndex = 0; productIndex < numberOfProducts; productIndex++)
            {
                Console.Write($"\nSelect quantity for {productList[productIndex]}: ");
                int quantity = ValidateQuantity(Console.ReadLine());
                products[customerIndex, productIndex] = quantity * productCosts[productIndex];
            }
        }

        Console.WriteLine("Purchase information recorded successfully.");
    }

    static void ViewAllPurchases()
{
    for (int customerIndex = 0; customerIndex < numberOfCustomers; customerIndex++)
    {
        double total = 0;

        Console.WriteLine($"\n\n------ Customer {customerIndex + 1} Purchase Receipt ------");
        Console.WriteLine($"Customer Name: {customerInfo[customerIndex, 0]} {customerInfo[customerIndex, 1]}");
        Console.WriteLine($"Phone Number: {customerInfo[customerIndex, 2]}");
        Console.WriteLine($"E-mail Address: {customerInfo[customerIndex, 3]}");
        Console.WriteLine($"Street Address: {customerInfo[customerIndex, 4]}");

        Console.WriteLine("\n****** Items Purchased ******");

        for (int productIndex = 0; productIndex < productList.Length; productIndex++)
        {
            Console.WriteLine($"{productList[productIndex]}: ${products[customerIndex, productIndex]}");
            total += products[customerIndex, productIndex];
        }

        double taxes = 0.04 * total;

        Console.WriteLine($"\nTotal: ${total}");
        Console.WriteLine($"Tax Added: ${taxes}");
        Console.WriteLine($"Grand total including tax: ${total + taxes}");
    }
}



    static void LoadData()
{
    if (File.Exists(fileName))
    {
        string[] lines = File.ReadAllLines(fileName);

        for (int i = 0; i < Math.Min(lines.Length, numberOfCustomers); i++)
        {
            string[] data = lines[i].Split(';');
            for (int j = 0; j < Math.Min(data.Length, 5); j++)
            {
                customerInfo[i, j] = data[j];
            }

            for (int j = 5; j < data.Length; j++)
            {
                if (j - 5 < numberOfProducts)
                {
                    products[i, j - 5] = double.Parse(data[j]);
                }
            }
        }

        Console.WriteLine("Data loaded successfully.");
    }
}


    static void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            for (int i = 0; i < numberOfCustomers; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    writer.Write($"{customerInfo[i, j]};");
                }

                for (int j = 0; j < numberOfProducts; j++)
                {
                    writer.Write($"{products[i, j]}");
                    if (j < numberOfProducts - 1)
                        writer.Write(";");
                    else
                        writer.WriteLine();
                }
            }
        }
   }

    static string ValidateInput(string input)
    {
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Input cannot be empty. Please try again: ");
            input = Console.ReadLine();
        }
        return input;
    }

    static int ValidateQuantity(string input)
    {
        int quantity;
        while (!int.TryParse(input, out quantity) || quantity <= 0)
        {
            Console.WriteLine("\nInvalid quantity. Please enter an integer equal to or greater than 0: ");
            input = Console.ReadLine();
        }
        return quantity;
    }
}
