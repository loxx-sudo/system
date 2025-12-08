using System;
using System.IO;

class CafePOS
{
    // Cafe Information Constants
    const string CAFE_NAME = "Coffee Corner Cafe";
    const string CAFE_ADDRESS = "San Pablo, Laguna";
    const string CAFE_CONTACT = "Contact: (049) 555-1234";
    const string CAFE_TIN = "TIN: 123-456-789-000";

    // Stock Variables
    static int coffeeStock = 50;
    static int teaStock = 40;
    static int pastaStock = 30;
    static int cakeStock = 25;
    static int sandwichStock = 35;

    // Price Variables
    static double coffeePrice = 120.0;
    static double teaPrice = 100.0;
    static double pastaPrice = 180.0;
    static double cakePrice = 150.0;
    static double sandwichPrice = 140.0;

    // Order Storage Variables
    static string item1 = "", item2 = "", item3 = "", item4 = "", item5 = "";
    static int qty1 = 0, qty2 = 0, qty3 = 0, qty4 = 0, qty5 = 0;
    static double price1 = 0, price2 = 0, price3 = 0, price4 = 0, price5 = 0;
    static int orderCount = 0;

    // File Names
    static string RECEIPT_FILE = "receipt.txt";
    static string SALES_LOG_FILE = "sales_log.txt";
    static string STOCK_FILE = "stock.txt";

    // Main Function
    static void Main()
    {
        // Display welcome screen
        ShowWelcomeScreen();
        
        // Load stock from file
        LoadStock();

        // Main program loop
        bool running = true;
        
        while (running == true)
        {
            // Display main menu
            ShowMainMenu();
            string choice = GetUserChoice();

            // Process menu choice
            if (choice == "M")
            {
                ViewMenu();
            }
            else if (choice == "O")
            {
                AddOrder();
            }
            else if (choice == "V")
            {
                ViewCart();
            }
            else if (choice == "C")
            {
                ProcessCheckout();
            }
            else if (choice == "S")
            {
                SaveStock();
                Console.WriteLine("\nStock saved successfully!");
                PauseScreen();
            }
            else if (choice == "L")
            {
                ViewSalesLog();
            }
            else if (choice == "X")
            {
                ExitSystem();
                running = false;
            }
            else
            {
                Console.WriteLine("\nInvalid option!");
                PauseScreen();
            }
        }
    }

    // Function: Display Welcome Screen
    static void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("  Welcome to " + CAFE_NAME);
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    // Function: Display Main Menu
    static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("\n========== MAIN MENU ==========");
        Console.WriteLine("[M] View Menu");
        Console.WriteLine("[O] Add Order");
        Console.WriteLine("[V] View Cart");
        Console.WriteLine("[C] Checkout");
        Console.WriteLine("[S] Save Stock");
        Console.WriteLine("[L] View Sales Log");
        Console.WriteLine("[X] Exit");
        Console.WriteLine("===============================");
        Console.Write("\nSelect Option: ");
    }

    // Function: Get User Choice
    static string GetUserChoice()
    {
        string choice = Console.ReadLine();
        
        if (choice != null)
        {
            choice = choice.ToUpper();
        }
        else
        {
            choice = "";
        }
        
        return choice;
    }

    // Function: View Menu
    static void ViewMenu()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("            CAFE MENU");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("  BEVERAGES:");
        Console.WriteLine("  1. Coffee      - P" + coffeePrice + "  (Stock: " + coffeeStock + ")");
        Console.WriteLine("  2. Tea         - P" + teaPrice + "  (Stock: " + teaStock + ")");
        Console.WriteLine();
        Console.WriteLine("  FOOD:");
        Console.WriteLine("  3. Pasta       - P" + pastaPrice + "  (Stock: " + pastaStock + ")");
        Console.WriteLine("  4. Cake        - P" + cakePrice + "  (Stock: " + cakeStock + ")");
        Console.WriteLine("  5. Sandwich    - P" + sandwichPrice + "  (Stock: " + sandwichStock + ")");
        Console.WriteLine();
        Console.WriteLine("========================================");
        PauseScreen();
    }

    // Function: Add Order
    static void AddOrder()
    {
        Console.Clear();
        
        // Check if cart is full
        if (orderCount >= 5)
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("  CART FULL!");
            Console.WriteLine("========================================");
            Console.WriteLine("Maximum 5 items only.");
            Console.WriteLine("Please checkout first.");
            PauseScreen();
            return;
        }

        Console.WriteLine("========================================");
        Console.WriteLine("          ADD ORDER");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("  1. Coffee      - P" + coffeePrice);
        Console.WriteLine("  2. Tea         - P" + teaPrice);
        Console.WriteLine("  3. Pasta       - P" + pastaPrice);
        Console.WriteLine("  4. Cake        - P" + cakePrice);
        Console.WriteLine("  5. Sandwich    - P" + sandwichPrice);
        Console.WriteLine();
        Console.WriteLine("========================================");

        // Get item number
        int itemNum = GetItemNumber();
        
        if (itemNum == 0)
        {
            return;
        }

        // Get quantity
        int quantity = GetQuantity();
        
        if (quantity == 0)
        {
            return;
        }

        // Process the order
        bool success = ProcessOrder(itemNum, quantity);
        
        if (success == true)
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("  ORDER ADDED!");
            Console.WriteLine("========================================");
        }
        
        PauseScreen();
    }

    // Function: Get Item Number
    static int GetItemNumber()
    {
        Console.Write("\nEnter item number (1-5): ");
        string itemInput = Console.ReadLine();
        int itemNum = 0;
        
        try
        {
            itemNum = Convert.ToInt32(itemInput);
        }
        catch
        {
            Console.WriteLine("\nError: Invalid input!");
            PauseScreen();
            return 0;
        }

        if (itemNum < 1 || itemNum > 5)
        {
            Console.WriteLine("\nError: Please enter 1-5 only!");
            PauseScreen();
            return 0;
        }

        return itemNum;
    }

    // Function: Get Quantity
    static int GetQuantity()
    {
        Console.Write("Enter quantity: ");
        string qtyInput = Console.ReadLine();
        int quantity = 0;
        
        try
        {
            quantity = Convert.ToInt32(qtyInput);
        }
        catch
        {
            Console.WriteLine("\nError: Invalid quantity!");
            PauseScreen();
            return 0;
        }

        if (quantity <= 0)
        {
            Console.WriteLine("\nError: Quantity must be more than 0!");
            PauseScreen();
            return 0;
        }

        if (quantity > 100)
        {
            Console.WriteLine("\nError: Maximum 100 per item only!");
            PauseScreen();
            return 0;
        }

        return quantity;
    }

    // Function: Process Order
    static bool ProcessOrder(int itemNum, int quantity)
    {
        string itemName = "";
        double itemPrice = 0;
        bool hasStock = false;

        // Check item and update stock
        if (itemNum == 1)
        {
            itemName = "Coffee";
            itemPrice = coffeePrice;
            if (quantity <= coffeeStock)
            {
                hasStock = true;
                coffeeStock = coffeeStock - quantity;
            }
        }
        else if (itemNum == 2)
        {
            itemName = "Tea";
            itemPrice = teaPrice;
            if (quantity <= teaStock)
            {
                hasStock = true;
                teaStock = teaStock - quantity;
            }
        }
        else if (itemNum == 3)
        {
            itemName = "Pasta";
            itemPrice = pastaPrice;
            if (quantity <= pastaStock)
            {
                hasStock = true;
                pastaStock = pastaStock - quantity;
            }
        }
        else if (itemNum == 4)
        {
            itemName = "Cake";
            itemPrice = cakePrice;
            if (quantity <= cakeStock)
            {
                hasStock = true;
                cakeStock = cakeStock - quantity;
            }
        }
        else if (itemNum == 5)
        {
            itemName = "Sandwich";
            itemPrice = sandwichPrice;
            if (quantity <= sandwichStock)
            {
                hasStock = true;
                sandwichStock = sandwichStock - quantity;
            }
        }

        // Check if have enough stock
        if (hasStock == false)
        {
            Console.WriteLine("\nError: Not enough stock!");
            PauseScreen();
            return false;
        }

        // Add to cart
        AddToCart(itemName, quantity, itemPrice);
        
        double lineTotal = itemPrice * quantity;
        Console.WriteLine("\n" + itemName + " x " + quantity + " = P" + lineTotal);
        
        return true;
    }

    // Function: Add to Cart
    static void AddToCart(string itemName, int quantity, double itemPrice)
    {
        if (orderCount == 0)
        {
            item1 = itemName;
            qty1 = quantity;
            price1 = itemPrice;
        }
        else if (orderCount == 1)
        {
            item2 = itemName;
            qty2 = quantity;
            price2 = itemPrice;
        }
        else if (orderCount == 2)
        {
            item3 = itemName;
            qty3 = quantity;
            price3 = itemPrice;
        }
        else if (orderCount == 3)
        {
            item4 = itemName;
            qty4 = quantity;
            price4 = itemPrice;
        }
        else if (orderCount == 4)
        {
            item5 = itemName;
            qty5 = quantity;
            price5 = itemPrice;
        }

        orderCount = orderCount + 1;
    }

    // Function: View Cart
    static void ViewCart()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("          YOUR CART");
        Console.WriteLine("========================================");

        if (orderCount == 0)
        {
            Console.WriteLine("\nYour cart is empty!");
            Console.WriteLine("Please add items first.");
        }
        else
        {
            Console.WriteLine();
            double subtotal = DisplayCartItems();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Subtotal: P" + subtotal);
            Console.WriteLine();
        }
        
        Console.WriteLine("========================================");
        PauseScreen();
    }

    // Function: Display Cart Items
    static double DisplayCartItems()
    {
        double subtotal = 0;

        if (orderCount >= 1)
        {
            double total1 = price1 * qty1;
            Console.WriteLine("  " + item1 + " x" + qty1 + " = P" + total1);
            subtotal = subtotal + total1;
        }

        if (orderCount >= 2)
        {
            double total2 = price2 * qty2;
            Console.WriteLine("  " + item2 + " x" + qty2 + " = P" + total2);
            subtotal = subtotal + total2;
        }

        if (orderCount >= 3)
        {
            double total3 = price3 * qty3;
            Console.WriteLine("  " + item3 + " x" + qty3 + " = P" + total3);
            subtotal = subtotal + total3;
        }

        if (orderCount >= 4)
        {
            double total4 = price4 * qty4;
            Console.WriteLine("  " + item4 + " x" + qty4 + " = P" + total4);
            subtotal = subtotal + total4;
        }

        if (orderCount >= 5)
        {
            double total5 = price5 * qty5;
            Console.WriteLine("  " + item5 + " x" + qty5 + " = P" + total5);
            subtotal = subtotal + total5;
        }

        return subtotal;
    }

    // Function: Process Checkout
    static void ProcessCheckout()
    {
        Console.Clear();
        
        // Check if cart is empty
        if (orderCount == 0)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("          CHECKOUT");
            Console.WriteLine("========================================");
            Console.WriteLine("\nNo orders yet!");
            Console.WriteLine("Please add items first.");
            PauseScreen();
            return;
        }

        Console.WriteLine("========================================");
        Console.WriteLine("          CHECKOUT");
        Console.WriteLine("========================================");

        // Calculate totals
        double totalAmount = CalculateTotal();
        double vat = CalculateVAT(totalAmount);
        double grandTotal = totalAmount + vat;

        // Display summary
        DisplayCheckoutSummary(totalAmount, vat, grandTotal);

        // Get payment
        double payment = GetPayment(grandTotal);
        double change = payment - grandTotal;

        // Print receipt
        PrintReceipt(totalAmount, vat, grandTotal, payment, change);

        // Save to files
        SaveReceipt(totalAmount, vat, grandTotal, payment, change);
        SaveSalesLog(totalAmount, vat, grandTotal);
        SaveStock();

        // Ask for new order
        AskNewOrder();
    }

    // Function: Calculate Total
    static double CalculateTotal()
    {
        double total = 0;

        if (orderCount >= 1)
        {
            total = total + (price1 * qty1);
        }
        if (orderCount >= 2)
        {
            total = total + (price2 * qty2);
        }
        if (orderCount >= 3)
        {
            total = total + (price3 * qty3);
        }
        if (orderCount >= 4)
        {
            total = total + (price4 * qty4);
        }
        if (orderCount >= 5)
        {
            total = total + (price5 * qty5);
        }

        return total;
    }

    // Function: Calculate VAT
    static double CalculateVAT(double amount)
    {
        double vat = amount * 0.12;
        return vat;
    }

    // Function: Display Checkout Summary
    static void DisplayCheckoutSummary(double subtotal, double vat, double grandTotal)
    {
        Console.WriteLine();
        Console.WriteLine("Subtotal:     P" + subtotal);
        Console.WriteLine("VAT (12%):    P" + vat);
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("GRAND TOTAL:  P" + grandTotal);
    }

    // Function: Get Payment
    static double GetPayment(double grandTotal)
    {
        double payment = 0;
        bool validPayment = false;
        
        while (validPayment == false)
        {
            Console.Write("\nEnter payment: P");
            string paymentInput = Console.ReadLine();
            
            try
            {
                payment = Convert.ToDouble(paymentInput);
                
                if (payment < 0)
                {
                    Console.WriteLine("\nError: Payment cannot be negative!");
                }
                else if (payment < grandTotal)
                {
                    double shortage = grandTotal - payment;
                    Console.WriteLine("\nNot enough payment!");
                    Console.WriteLine("Need P" + shortage + " more");
                }
                else
                {
                    validPayment = true;
                }
            }
            catch
            {
                Console.WriteLine("\nError: Invalid amount!");
            }
        }

        return payment;
    }

    // Function: Print Receipt
    static void PrintReceipt(double subtotal, double vat, double grandTotal, double payment, double change)
    {
        Console.Clear();
        Console.WriteLine("\n========================================");
        Console.WriteLine("        OFFICIAL RECEIPT");
        Console.WriteLine("========================================");
        Console.WriteLine(CAFE_NAME);
        Console.WriteLine(CAFE_ADDRESS);
        Console.WriteLine(CAFE_CONTACT);
        Console.WriteLine(CAFE_TIN);
        Console.WriteLine("========================================");
        Console.WriteLine("Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
        Console.WriteLine("========================================");
        Console.WriteLine();

        // Display all items
        DisplayReceiptItems();

        Console.WriteLine();
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Subtotal:     P" + subtotal);
        Console.WriteLine("VAT (12%):    P" + vat);
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("GRAND TOTAL:  P" + grandTotal);
        Console.WriteLine("Payment:      P" + payment);
        Console.WriteLine("Change:       P" + change);
        Console.WriteLine("========================================");
        Console.WriteLine("    Thank you for your purchase!");
        Console.WriteLine("       Please come again!");
        Console.WriteLine("========================================\n");
    }

    // Function: Display Receipt Items
    static void DisplayReceiptItems()
    {
        if (orderCount >= 1)
        {
            Console.WriteLine(item1 + " x" + qty1);
            Console.WriteLine("  P" + (price1 * qty1));
        }
        if (orderCount >= 2)
        {
            Console.WriteLine(item2 + " x" + qty2);
            Console.WriteLine("  P" + (price2 * qty2));
        }
        if (orderCount >= 3)
        {
            Console.WriteLine(item3 + " x" + qty3);
            Console.WriteLine("  P" + (price3 * qty3));
        }
        if (orderCount >= 4)
        {
            Console.WriteLine(item4 + " x" + qty4);
            Console.WriteLine("  P" + (price4 * qty4));
        }
        if (orderCount >= 5)
        {
            Console.WriteLine(item5 + " x" + qty5);
            Console.WriteLine("  P" + (price5 * qty5));
        }
    }

    // Function: Save Receipt to File
    static void SaveReceipt(double subtotal, double vat, double grandTotal, double payment, double change)
    {
        try
        {
            // Create receipt content
            string receipt = "========================================\n";
            receipt = receipt + "        OFFICIAL RECEIPT\n";
            receipt = receipt + "========================================\n";
            receipt = receipt + CAFE_NAME + "\n";
            receipt = receipt + CAFE_ADDRESS + "\n";
            receipt = receipt + CAFE_CONTACT + "\n";
            receipt = receipt + CAFE_TIN + "\n";
            receipt = receipt + "========================================\n";
            receipt = receipt + "Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
            receipt = receipt + "========================================\n\n";

            // Add items to receipt
            if (orderCount >= 1)
            {
                receipt = receipt + item1 + " x" + qty1 + "\n";
                receipt = receipt + "  P" + (price1 * qty1) + "\n";
            }
            if (orderCount >= 2)
            {
                receipt = receipt + item2 + " x" + qty2 + "\n";
                receipt = receipt + "  P" + (price2 * qty2) + "\n";
            }
            if (orderCount >= 3)
            {
                receipt = receipt + item3 + " x" + qty3 + "\n";
                receipt = receipt + "  P" + (price3 * qty3) + "\n";
            }
            if (orderCount >= 4)
            {
                receipt = receipt + item4 + " x" + qty4 + "\n";
                receipt = receipt + "  P" + (price4 * qty4) + "\n";
            }
            if (orderCount >= 5)
            {
                receipt = receipt + item5 + " x" + qty5 + "\n";
                receipt = receipt + "  P" + (price5 * qty5) + "\n";
            }

            receipt = receipt + "\n----------------------------------------\n";
            receipt = receipt + "Subtotal:     P" + subtotal + "\n";
            receipt = receipt + "VAT (12%):    P" + vat + "\n";
            receipt = receipt + "----------------------------------------\n";
            receipt = receipt + "GRAND TOTAL:  P" + grandTotal + "\n";
            receipt = receipt + "Payment:      P" + payment + "\n";
            receipt = receipt + "Change:       P" + change + "\n";
            receipt = receipt + "========================================\n";
            receipt = receipt + "    Thank you for your purchase!\n";
            receipt = receipt + "       Please come again!\n";
            receipt = receipt + "========================================\n";

            // Write to file
            File.WriteAllText(RECEIPT_FILE, receipt);
            Console.WriteLine("[Receipt saved: " + RECEIPT_FILE + "]");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to save receipt: " + ex.Message);
        }
    }

    // Function: Save Sales Log to File
    static void SaveSalesLog(double subtotal, double vat, double grandTotal)
    {
        try
        {
            // Create log entry
            string logEntry = "\n========================================\n";
            logEntry = logEntry + "          SALE RECORD\n";
            logEntry = logEntry + "========================================\n";
            logEntry = logEntry + "Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
            logEntry = logEntry + "----------------------------------------\n";
            logEntry = logEntry + "Subtotal:     P" + subtotal + "\n";
            logEntry = logEntry + "VAT (12%):    P" + vat + "\n";
            logEntry = logEntry + "Grand Total:  P" + grandTotal + "\n";
            logEntry = logEntry + "----------------------------------------\n";
            logEntry = logEntry + "Items Sold:\n";

            // Add items to log
            if (orderCount >= 1)
            {
                logEntry = logEntry + "  - " + item1 + " x " + qty1 + "\n";
            }
            if (orderCount >= 2)
            {
                logEntry = logEntry + "  - " + item2 + " x " + qty2 + "\n";
            }
            if (orderCount >= 3)
            {
                logEntry = logEntry + "  - " + item3 + " x " + qty3 + "\n";
            }
            if (orderCount >= 4)
            {
                logEntry = logEntry + "  - " + item4 + " x " + qty4 + "\n";
            }
            if (orderCount >= 5)
            {
                logEntry = logEntry + "  - " + item5 + " x " + qty5 + "\n";
            }

            logEntry = logEntry + "========================================\n";

            // Append to file
            File.AppendAllText(SALES_LOG_FILE, logEntry);
            Console.WriteLine("[Sale logged: " + SALES_LOG_FILE + "]");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to save sales log: " + ex.Message);
        }
    }

    // Function: Save Stock to File
    static void SaveStock()
    {
        try
        {
            // Create stock data
            string stockData = "========================================\n";
            stockData = stockData + "        CURRENT INVENTORY\n";
            stockData = stockData + "========================================\n";
            stockData = stockData + "Coffee:       " + coffeeStock + " units\n";
            stockData = stockData + "Tea:          " + teaStock + " units\n";
            stockData = stockData + "Pasta:        " + pastaStock + " units\n";
            stockData = stockData + "Cake:         " + cakeStock + " units\n";
            stockData = stockData + "Sandwich:     " + sandwichStock + " units\n";
            stockData = stockData + "========================================\n";
            stockData = stockData + "Last Updated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
            stockData = stockData + "========================================\n";

            // Write to file
            File.WriteAllText(STOCK_FILE, stockData);
            Console.WriteLine("[Stock saved: " + STOCK_FILE + "]");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to save stock: " + ex.Message);
        }
    }

    // Function: Load Stock from File
    static void LoadStock()
    {
        try
        {
            if (File.Exists(STOCK_FILE) == true)
            {
                string[] lines = File.ReadAllLines(STOCK_FILE);

                if (lines.Length >= 8)
                {
                    // Parse stock values
                    string coffeeLine = lines[3];
                    string teaLine = lines[4];
                    string pastaLine = lines[5];
                    string cakeLine = lines[6];
                    string sandwichLine = lines[7];

                    coffeeStock = Convert.ToInt32(coffeeLine.Split(':')[1].Trim().Replace(" units", ""));
                    teaStock = Convert.ToInt32(teaLine.Split(':')[1].Trim().Replace(" units", ""));
                    pastaStock = Convert.ToInt32(pastaLine.Split(':')[1].Trim().Replace(" units", ""));
                    cakeStock = Convert.ToInt32(cakeLine.Split(':')[1].Trim().Replace(" units", ""));
                    sandwichStock = Convert.ToInt32(sandwichLine.Split(':')[1].Trim().Replace(" units", ""));

                    Console.WriteLine("[Stock loaded from file]\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Warning] Could not load stock file: " + ex.Message);
            Console.WriteLine("Using default stock values.\n");
        }
    }

    // Function: View Sales Log
    static void ViewSalesLog()
    {
        Console.Clear();
        
        try
        {
            if (File.Exists(SALES_LOG_FILE) == true)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("          SALES LOG");
                Console.WriteLine("========================================");
                string logContent = File.ReadAllText(SALES_LOG_FILE);
                Console.WriteLine(logContent);
            }
            else
            {
                Console.WriteLine("========================================");
                Console.WriteLine("          SALES LOG");
                Console.WriteLine("========================================");
                Console.WriteLine("\nNo sales records found yet!");
                Console.WriteLine("Sales will be logged after checkout.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to read sales log: " + ex.Message);
        }
        
        PauseScreen();
    }

    // Function: Ask for New Order
    static void AskNewOrder()
    {
        bool validAnswer = false;
        
        while (validAnswer == false)
        {
            Console.Write("\nNew order? (Y/N): ");
            string newOrder = Console.ReadLine();
            
            if (newOrder != null)
            {
                newOrder = newOrder.ToUpper();
            }

            if (newOrder == "Y")
            {
                ResetCart();
                Console.WriteLine("\nNew order started!");
                PauseScreen();
                validAnswer = true;
            }
            else if (newOrder == "N")
            {
                Console.WriteLine("\nReturning to main menu...");
                PauseScreen();
                validAnswer = true;
            }
            else
            {
                Console.WriteLine("\nInvalid input! Please enter Y or N only.");
            }
        }
    }

    // Function: Reset Cart
    static void ResetCart()
    {
        orderCount = 0;
        item1 = "";
        item2 = "";
        item3 = "";
        item4 = "";
        item5 = "";
        qty1 = 0;
        qty2 = 0;
        qty3 = 0;
        qty4 = 0;
        qty5 = 0;
        price1 = 0;
        price2 = 0;
        price3 = 0;
        price4 = 0;
        price5 = 0;
    }

    // Function: Exit System
    static void ExitSystem()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("        CLOSING SYSTEM");
        Console.WriteLine("========================================");
        
        SaveStock();
        
        Console.WriteLine("\n[Stock automatically saved]");
        Console.WriteLine("\nThank you for using Cafe POS!");
        Console.WriteLine("Goodbye!\n");
        Console.WriteLine("========================================");
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    // Function: Pause Screen
    static void PauseScreen()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
