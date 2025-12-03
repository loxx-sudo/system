using System;
using System.IO;

class CafePOS
{
    const string CAFE_NAME = "Coffee Corner Cafe";
    const string CAFE_ADDRESS = "San Pablo, Laguna";
    const string CAFE_CONTACT = "Contact: (049) 555-1234";
    const string CAFE_TIN = "TIN: 123-456-789-000";

    static int coffeeStock = 50;
    static int teaStock = 40;
    static int pastaStock = 30;
    static int cakeStock = 25;
    static int sandwichStock = 35;

    static double coffeePrice = 120.0;
    static double teaPrice = 100.0;
    static double pastaPrice = 180.0;
    static double cakePrice = 150.0;
    static double sandwichPrice = 140.0;

    // Order storage - fixed 5 items
    static string item1 = "", item2 = "", item3 = "", item4 = "", item5 = "";
    static int qty1 = 0, qty2 = 0, qty3 = 0, qty4 = 0, qty5 = 0;
    static double price1 = 0, price2 = 0, price3 = 0, price4 = 0, price5 = 0;
    static int orderCount = 0;

    static string RECEIPT_FILE = "receipt.txt";
    static string SALES_LOG_FILE = "sales_log.txt";
    static string STOCK_FILE = "stock.txt";

    static void Main()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("  Welcome to " + CAFE_NAME);
        Console.WriteLine("========================================");

        // Load stock from file if exists
        LoadStockFromFile();

        bool running = true;

        if (running)
        {
            DisplayMainMenu();
            string choice = Console.ReadLine();
            
            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("\nInvalid input! Please enter a valid option.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Main();
                return;
            }
            
            choice = choice.ToUpper();

            if (choice == "M")
            {
                ShowMenu();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Main();
            }
            else if (choice == "O")
            {
                AddOrder();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Main();
            }
            else if (choice == "V")
            {
                ViewOrders();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Main();
            }
            else if (choice == "C")
            {
                running = ProcessCheckout();
                if (running)
                {
                    Main();
                }
            }
            else if (choice == "S")
            {
                SaveStockToFile();
                Console.WriteLine("\nStock saved successfully!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Main();
            }
            else if (choice == "L")
            {
                ViewSalesLog();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Main();
            }
            else if (choice == "X")
            {
                ExitSystem();
            }
            else
            {
                Console.WriteLine("\nInvalid option! Please select from the menu.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Main();
            }
        }
    }

    static void DisplayMainMenu()
    {
        Console.WriteLine("\n========== MAIN MENU ==========");
        Console.WriteLine("[M] View Menu");
        Console.WriteLine("[O] Add Order");
        Console.WriteLine("[V] View Cart");
        Console.WriteLine("[C] Checkout");
        Console.WriteLine("[S] Save Stock");
        Console.WriteLine("[L] View Sales Log");
        Console.WriteLine("[X] Exit System");
        Console.WriteLine("===============================");
        Console.Write("\nSelect Option: ");
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("            CAFE MENU");
        Console.WriteLine("========================================");
        Console.WriteLine("");
        Console.WriteLine("  BEVERAGES:");
        Console.WriteLine("  1. Coffee      - P" + coffeePrice.ToString("0.00") + "  (Stock: " + coffeeStock + ")");
        Console.WriteLine("  2. Tea         - P" + teaPrice.ToString("0.00") + "  (Stock: " + teaStock + ")");
        Console.WriteLine("");
        Console.WriteLine("  FOOD:");
        Console.WriteLine("  3. Pasta       - P" + pastaPrice.ToString("0.00") + "  (Stock: " + pastaStock + ")");
        Console.WriteLine("  4. Cake        - P" + cakePrice.ToString("0.00") + "  (Stock: " + cakeStock + ")");
        Console.WriteLine("  5. Sandwich    - P" + sandwichPrice.ToString("0.00") + "  (Stock: " + sandwichStock + ")");
        Console.WriteLine("");
        Console.WriteLine("========================================");
    }

    static void AddOrder()
    {
        Console.Clear();
        
        if (orderCount >= 5)
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("  CART FULL!");
            Console.WriteLine("========================================");
            Console.WriteLine("Maximum 5 items allowed.");
            Console.WriteLine("Please checkout current orders first.");
            return;
        }

        Console.WriteLine("========================================");
        Console.WriteLine("          ADD ORDER");
        Console.WriteLine("========================================");
        Console.WriteLine("");
        Console.WriteLine("  1. Coffee      - P" + coffeePrice.ToString("0.00"));
        Console.WriteLine("  2. Tea         - P" + teaPrice.ToString("0.00"));
        Console.WriteLine("  3. Pasta       - P" + pastaPrice.ToString("0.00"));
        Console.WriteLine("  4. Cake        - P" + cakePrice.ToString("0.00"));
        Console.WriteLine("  5. Sandwich    - P" + sandwichPrice.ToString("0.00"));
        Console.WriteLine("");
        Console.WriteLine("========================================");

        Console.Write("\nEnter item number (1-5): ");
        string itemInput = Console.ReadLine();
        
        int itemNum;
        bool validItem = int.TryParse(itemInput, out itemNum);

        if (!validItem)
        {
            Console.WriteLine("\nError: Please enter a valid number!");
            return;
        }

        if (itemNum < 1 || itemNum > 5)
        {
            Console.WriteLine("\nError: Item number must be between 1 and 5!");
            return;
        }

        Console.Write("Enter quantity: ");
        string qtyInput = Console.ReadLine();
        
        int quantity;
        bool validQty = int.TryParse(qtyInput, out quantity);

        if (!validQty)
        {
            Console.WriteLine("\nError: Please enter a valid quantity!");
            return;
        }

        if (quantity <= 0)
        {
            Console.WriteLine("\nError: Quantity must be greater than 0!");
            return;
        }

        if (quantity > 100)
        {
            Console.WriteLine("\nError: Quantity too large! Maximum 100 per item.");
            return;
        }

        if (ProcessItemOrder(itemNum, quantity))
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("  ORDER ADDED SUCCESSFULLY!");
            Console.WriteLine("========================================");
        }
    }

    static bool ProcessItemOrder(int itemNum, int quantity)
    {
        string itemName = "";
        double itemPrice = 0;
        bool hasStock = false;

        if (itemNum == 1)
        {
            itemName = "Coffee";
            itemPrice = coffeePrice;
            if (quantity <= coffeeStock)
            {
                hasStock = true;
                coffeeStock -= quantity;
            }
        }
        else if (itemNum == 2)
        {
            itemName = "Tea";
            itemPrice = teaPrice;
            if (quantity <= teaStock)
            {
                hasStock = true;
                teaStock -= quantity;
            }
        }
        else if (itemNum == 3)
        {
            itemName = "Pasta";
            itemPrice = pastaPrice;
            if (quantity <= pastaStock)
            {
                hasStock = true;
                pastaStock -= quantity;
            }
        }
        else if (itemNum == 4)
        {
            itemName = "Cake";
            itemPrice = cakePrice;
            if (quantity <= cakeStock)
            {
                hasStock = true;
                cakeStock -= quantity;
            }
        }
        else if (itemNum == 5)
        {
            itemName = "Sandwich";
            itemPrice = sandwichPrice;
            if (quantity <= sandwichStock)
            {
                hasStock = true;
                sandwichStock -= quantity;
            }
        }

        if (!hasStock)
        {
            Console.WriteLine("\nError: Insufficient stock!");
            Console.WriteLine("Available stock: " + GetAvailableStock(itemNum));
            return false;
        }

        // Store order in the next available slot
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

        orderCount++;
        
        double lineTotal = itemPrice * quantity;
        Console.WriteLine("\n" + itemName + " x " + quantity + " = P" + lineTotal.ToString("0.00"));
        
        return true;
    }

    static int GetAvailableStock(int itemNum)
    {
        if (itemNum == 1) return coffeeStock;
        if (itemNum == 2) return teaStock;
        if (itemNum == 3) return pastaStock;
        if (itemNum == 4) return cakeStock;
        if (itemNum == 5) return sandwichStock;
        return 0;
    }

    static void ViewOrders()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("          YOUR CART");
        Console.WriteLine("========================================");

        if (orderCount == 0)
        {
            Console.WriteLine("\nYour cart is empty!");
            Console.WriteLine("Please add items to your order.");
        }
        else
        {
            Console.WriteLine("");
            double subtotal = DisplayOrderList();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Subtotal: P" + subtotal.ToString("0.00"));
            Console.WriteLine("");
        }
        Console.WriteLine("========================================");
    }

    static double DisplayOrderList()
    {
        double subtotal = 0;

        if (orderCount >= 1)
        {
            double total1 = price1 * qty1;
            Console.WriteLine("  " + item1 + " x" + qty1 + " = P" + total1.ToString("0.00"));
            subtotal += total1;
        }

        if (orderCount >= 2)
        {
            double total2 = price2 * qty2;
            Console.WriteLine("  " + item2 + " x" + qty2 + " = P" + total2.ToString("0.00"));
            subtotal += total2;
        }

        if (orderCount >= 3)
        {
            double total3 = price3 * qty3;
            Console.WriteLine("  " + item3 + " x" + qty3 + " = P" + total3.ToString("0.00"));
            subtotal += total3;
        }

        if (orderCount >= 4)
        {
            double total4 = price4 * qty4;
            Console.WriteLine("  " + item4 + " x" + qty4 + " = P" + total4.ToString("0.00"));
            subtotal += total4;
        }

        if (orderCount >= 5)
        {
            double total5 = price5 * qty5;
            Console.WriteLine("  " + item5 + " x" + qty5 + " = P" + total5.ToString("0.00"));
            subtotal += total5;
        }

        return subtotal;
    }

    static bool ProcessCheckout()
    {
        Console.Clear();
        
        if (orderCount == 0)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("          CHECKOUT");
            Console.WriteLine("========================================");
            Console.WriteLine("\nNo orders to checkout!");
            Console.WriteLine("Please add items first.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return true;
        }

        Console.WriteLine("========================================");
        Console.WriteLine("          CHECKOUT");
        Console.WriteLine("========================================");

        double totalAmount = CalculateTotal();
        double vat = totalAmount * 0.12;
        double grandTotal = totalAmount + vat;

        Console.WriteLine("");
        DisplayCheckoutSummary(totalAmount, vat, grandTotal);

        double payment = GetPayment(grandTotal);
        double change = payment - grandTotal;

        PrintReceipt(totalAmount, vat, grandTotal, payment, change);

        // Save receipt and sales log to files
        SaveReceiptToFile(totalAmount, vat, grandTotal, payment, change);
        SaveSalesLog(totalAmount, vat, grandTotal);
        SaveStockToFile();

        return AskForNewOrder();
    }

    static double CalculateTotal()
    {
        double total = 0;

        if (orderCount >= 1)
        {
            total += price1 * qty1;
        }
        if (orderCount >= 2)
        {
            total += price2 * qty2;
        }
        if (orderCount >= 3)
        {
            total += price3 * qty3;
        }
        if (orderCount >= 4)
        {
            total += price4 * qty4;
        }
        if (orderCount >= 5)
        {
            total += price5 * qty5;
        }

        return total;
    }

    static void DisplayCheckoutSummary(double subtotal, double vat, double grandTotal)
    {
        Console.WriteLine("Subtotal:     P" + subtotal.ToString("0.00"));
        Console.WriteLine("VAT (12%):    P" + vat.ToString("0.00"));
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("GRAND TOTAL:  P" + grandTotal.ToString("0.00"));
    }

    static double GetPayment(double grandTotal)
    {
        double payment = 0;
        bool validPayment = false;

        Console.Write("\nEnter payment amount: P");
        string paymentInput = Console.ReadLine();
        
        validPayment = double.TryParse(paymentInput, out payment);

        if (!validPayment)
        {
            Console.WriteLine("\nError: Invalid payment amount!");
            return GetPayment(grandTotal);
        }

        if (payment < 0)
        {
            Console.WriteLine("\nError: Payment cannot be negative!");
            return GetPayment(grandTotal);
        }

        if (payment < grandTotal)
        {
            double shortage = grandTotal - payment;
            Console.WriteLine("\nInsufficient payment!");
            Console.WriteLine("Amount needed: P" + shortage.ToString("0.00") + " more");
            return GetPayment(grandTotal);
        }

        return payment;
    }

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
        Console.WriteLine("");

        if (orderCount >= 1)
        {
            Console.WriteLine(item1 + " x" + qty1);
            Console.WriteLine("  P" + (price1 * qty1).ToString("0.00"));
        }
        if (orderCount >= 2)
        {
            Console.WriteLine(item2 + " x" + qty2);
            Console.WriteLine("  P" + (price2 * qty2).ToString("0.00"));
        }
        if (orderCount >= 3)
        {
            Console.WriteLine(item3 + " x" + qty3);
            Console.WriteLine("  P" + (price3 * qty3).ToString("0.00"));
        }
        if (orderCount >= 4)
        {
            Console.WriteLine(item4 + " x" + qty4);
            Console.WriteLine("  P" + (price4 * qty4).ToString("0.00"));
        }
        if (orderCount >= 5)
        {
            Console.WriteLine(item5 + " x" + qty5);
            Console.WriteLine("  P" + (price5 * qty5).ToString("0.00"));
        }

        Console.WriteLine("");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Subtotal:     P" + subtotal.ToString("0.00"));
        Console.WriteLine("VAT (12%):    P" + vat.ToString("0.00"));
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("GRAND TOTAL:  P" + grandTotal.ToString("0.00"));
        Console.WriteLine("Payment:      P" + payment.ToString("0.00"));
        Console.WriteLine("Change:       P" + change.ToString("0.00"));
        Console.WriteLine("========================================");
        Console.WriteLine("    Thank you for your purchase!");
        Console.WriteLine("       Please come again!");
        Console.WriteLine("========================================\n");
    }

    // ========== FILE I/O FUNCTIONS ==========

    static void SaveReceiptToFile(double subtotal, double vat, double grandTotal, double payment, double change)
    {
        try
        {
            string receipt = "========================================\n";
            receipt += "        OFFICIAL RECEIPT\n";
            receipt += "========================================\n";
            receipt += CAFE_NAME + "\n";
            receipt += CAFE_ADDRESS + "\n";
            receipt += CAFE_CONTACT + "\n";
            receipt += CAFE_TIN + "\n";
            receipt += "========================================\n";
            receipt += "Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
            receipt += "========================================\n\n";

            if (orderCount >= 1)
            {
                receipt += item1 + " x" + qty1 + "\n";
                receipt += "  P" + (price1 * qty1).ToString("0.00") + "\n";
            }
            if (orderCount >= 2)
            {
                receipt += item2 + " x" + qty2 + "\n";
                receipt += "  P" + (price2 * qty2).ToString("0.00") + "\n";
            }
            if (orderCount >= 3)
            {
                receipt += item3 + " x" + qty3 + "\n";
                receipt += "  P" + (price3 * qty3).ToString("0.00") + "\n";
            }
            if (orderCount >= 4)
            {
                receipt += item4 + " x" + qty4 + "\n";
                receipt += "  P" + (price4 * qty4).ToString("0.00") + "\n";
            }
            if (orderCount >= 5)
            {
                receipt += item5 + " x" + qty5 + "\n";
                receipt += "  P" + (price5 * qty5).ToString("0.00") + "\n";
            }

            receipt += "\n----------------------------------------\n";
            receipt += "Subtotal:     P" + subtotal.ToString("0.00") + "\n";
            receipt += "VAT (12%):    P" + vat.ToString("0.00") + "\n";
            receipt += "----------------------------------------\n";
            receipt += "GRAND TOTAL:  P" + grandTotal.ToString("0.00") + "\n";
            receipt += "Payment:      P" + payment.ToString("0.00") + "\n";
            receipt += "Change:       P" + change.ToString("0.00") + "\n";
            receipt += "========================================\n";
            receipt += "    Thank you for your purchase!\n";
            receipt += "       Please come again!\n";
            receipt += "========================================\n";

            File.WriteAllText(RECEIPT_FILE, receipt);
            Console.WriteLine("[Receipt saved: " + Path.GetFullPath(RECEIPT_FILE) + "]");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to save receipt: " + ex.Message);
        }
    }

    static void SaveSalesLog(double subtotal, double vat, double grandTotal)
    {
        try
        {
            string logEntry = "\n========================================\n";
            logEntry += "          SALE RECORD\n";
            logEntry += "========================================\n";
            logEntry += "Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
            logEntry += "----------------------------------------\n";
            logEntry += "Subtotal:     P" + subtotal.ToString("0.00") + "\n";
            logEntry += "VAT (12%):    P" + vat.ToString("0.00") + "\n";
            logEntry += "Grand Total:  P" + grandTotal.ToString("0.00") + "\n";
            logEntry += "----------------------------------------\n";
            logEntry += "Items Sold:\n";

            if (orderCount >= 1)
            {
                logEntry += "  - " + item1 + " x " + qty1 + "\n";
            }
            if (orderCount >= 2)
            {
                logEntry += "  - " + item2 + " x " + qty2 + "\n";
            }
            if (orderCount >= 3)
            {
                logEntry += "  - " + item3 + " x " + qty3 + "\n";
            }
            if (orderCount >= 4)
            {
                logEntry += "  - " + item4 + " x " + qty4 + "\n";
            }
            if (orderCount >= 5)
            {
                logEntry += "  - " + item5 + " x " + qty5 + "\n";
            }

            logEntry += "========================================\n";

            File.AppendAllText(SALES_LOG_FILE, logEntry);
            Console.WriteLine("[Sale logged: " + Path.GetFullPath(SALES_LOG_FILE) + "]");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to save sales log: " + ex.Message);
        }
    }

    static void SaveStockToFile()
    {
        try
        {
            string stockData = "========================================\n";
            stockData += "        CURRENT INVENTORY\n";
            stockData += "========================================\n";
            stockData += "Coffee:       " + coffeeStock + " units\n";
            stockData += "Tea:          " + teaStock + " units\n";
            stockData += "Pasta:        " + pastaStock + " units\n";
            stockData += "Cake:         " + cakeStock + " units\n";
            stockData += "Sandwich:     " + sandwichStock + " units\n";
            stockData += "========================================\n";
            stockData += "Last Updated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
            stockData += "========================================\n";

            File.WriteAllText(STOCK_FILE, stockData);
            Console.WriteLine("[Stock saved: " + Path.GetFullPath(STOCK_FILE) + "]");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to save stock: " + ex.Message);
        }
    }

    static void LoadStockFromFile()
    {
        try
        {
            if (File.Exists(STOCK_FILE))
            {
                string[] lines = File.ReadAllLines(STOCK_FILE);

                if (lines.Length >= 6)
                {
                    // Parse stock values from file
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

    static void ViewSalesLog()
    {
        Console.Clear();
        
        try
        {
            if (File.Exists(SALES_LOG_FILE))
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
    }

    static bool AskForNewOrder()
    {
        Console.Write("\nStart new order? (Y/N): ");
        string newOrder = Console.ReadLine();
        
        if (string.IsNullOrEmpty(newOrder))
        {
            return false;
        }
        
        newOrder = newOrder.ToUpper();

        if (newOrder == "Y")
        {
            ResetOrders();
            Console.WriteLine("\nNew order started!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return true;
        }
        
        return false;
    }

    static void ResetOrders()
    {
        orderCount = 0;
        item1 = ""; item2 = ""; item3 = ""; item4 = ""; item5 = "";
        qty1 = 0; qty2 = 0; qty3 = 0; qty4 = 0; qty5 = 0;
        price1 = 0; price2 = 0; price3 = 0; price4 = 0; price5 = 0;
    }

    static void ExitSystem()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("        CLOSING SYSTEM");
        Console.WriteLine("========================================");
        
        SaveStockToFile();
        
        Console.WriteLine("\n[Stock automatically saved]");
        Console.WriteLine("\nThank you for using Cafe POS!");
        Console.WriteLine("Goodbye!\n");
        Console.WriteLine("========================================");
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
