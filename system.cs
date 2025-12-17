using System;
using System.IO;
using System.Collections.Generic;

class CafePOS
{
    const string CAFE_NAME = "Coffee Corner Cafe";
    const string CAFE_ADDRESS = "San Pablo, Laguna";
    const string CAFE_CONTACT = "Contact: (049) 555-1234";
    const string CAFE_TIN = "TIN: 123-456-789-000";
    const int LOW_STOCK_THRESHOLD = 10;

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

    static List<string> orderItems = new List<string>();
    static List<int> orderQuantities = new List<int>();
    static List<double> orderPrices = new List<double>();

    static string currentUser = "";
    static string currentUserRole = "";

    static void Main()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("  Welcome to " + CAFE_NAME);
        Console.WriteLine("========================================");
        
        LoadStock();
        CreateDefaultUsers();

        if (Login())
        {
            RunSystem();
        }
    }

    static bool Login()
    {
        Console.WriteLine("\n========== USER LOGIN ==========");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        
        Console.Write("Password: ");
        string password = Console.ReadLine();

        if (ValidateUser(username, password))
        {
            Console.WriteLine("\n✓ Login successful!");
            Console.WriteLine("Welcome, " + currentUser + " (" + currentUserRole + ")");
            LogAudit(currentUser + " logged in");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return true;
        }
        else
        {
            Console.WriteLine("\n✗ Invalid username or password!");
            return false;
        }
    }

    static bool ValidateUser(string username, string password)
    {
        try
        {
            if (File.Exists("users.txt"))
            {
                string[] users = File.ReadAllLines("users.txt");
                
                for (int i = 0; i < users.Length; i++)
                {
                    string[] userData = users[i].Split(',');
                    
                    if (userData.Length >= 3)
                    {
                        if (userData[0] == username && userData[1] == password)
                        {
                            currentUser = username;
                            currentUserRole = userData[2];
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Login error: " + ex.Message);
        }
        
        return false;
    }

    static void CreateDefaultUsers()
    {
        if (!File.Exists("users.txt"))
        {
            string defaultUsers = "admin,admin123,Admin\ncashier,cashier123,Cashier\n";
            File.WriteAllText("users.txt", defaultUsers);
        }
    }

    static void RunSystem()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            CheckLowStock();
            DisplayMainMenu();
            
            string choice = Console.ReadLine();
            
            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("\nInvalid input!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }
            
            choice = choice.ToUpper();

            switch (choice)
            {
                case "M":
                    ShowMenu();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "O":
                    AddOrder();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "V":
                    ViewCart();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "E":
                    EditCart();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "D":
                    RemoveFromCart();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "N":
                    CancelOrder();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "C":
                    ProcessCheckout();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "A":
                    if (currentUserRole == "Admin")
                    {
                        RestockInventory();
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Access Denied! Admin only.");
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "I":
                    if (currentUserRole == "Admin")
                    {
                        ViewInventoryLog();
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Access Denied! Admin only.");
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "S":
                    SaveStock();
                    Console.WriteLine("\nStock saved successfully!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;

                case "L":
                    ViewSalesLog();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "R":
                    if (currentUserRole == "Admin")
                    {
                        GenerateDailySummary();
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Access Denied! Admin only.");
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "U":
                    if (currentUserRole == "Admin")
                    {
                        ViewAuditLog();
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Access Denied! Admin only.");
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case "X":
                    running = ExitSystem();
                    break;

                default:
                    Console.WriteLine("\nInvalid option!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void DisplayMainMenu()
    {
        Console.WriteLine("\n========== MAIN MENU ==========");
        Console.WriteLine("User: " + currentUser + " (" + currentUserRole + ")");
        Console.WriteLine("===============================");
        Console.WriteLine("[M] View Menu");
        Console.WriteLine("[O] Add Order");
        Console.WriteLine("[V] View Cart");
        Console.WriteLine("[E] Edit Cart Item");
        Console.WriteLine("[D] Remove from Cart");
        Console.WriteLine("[N] Cancel Order");
        Console.WriteLine("[C] Checkout");
        Console.WriteLine("-------------------------------");
        
        if (currentUserRole == "Admin")
        {
            Console.WriteLine("[A] Restock Inventory (Admin)");
            Console.WriteLine("[I] Inventory Log (Admin)");
            Console.WriteLine("[R] Daily Report (Admin)");
            Console.WriteLine("[U] Audit Log (Admin)");
        }
        
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

    static void CheckLowStock()
    {
        bool hasLowStock = false;
        string lowStockItems = "";

        if (coffeeStock <= LOW_STOCK_THRESHOLD)
        {
            hasLowStock = true;
            lowStockItems += "  - Coffee: " + coffeeStock + " units\n";
        }
        if (teaStock <= LOW_STOCK_THRESHOLD)
        {
            hasLowStock = true;
            lowStockItems += "  - Tea: " + teaStock + " units\n";
        }
        if (pastaStock <= LOW_STOCK_THRESHOLD)
        {
            hasLowStock = true;
            lowStockItems += "  - Pasta: " + pastaStock + " units\n";
        }
        if (cakeStock <= LOW_STOCK_THRESHOLD)
        {
            hasLowStock = true;
            lowStockItems += "  - Cake: " + cakeStock + " units\n";
        }
        if (sandwichStock <= LOW_STOCK_THRESHOLD)
        {
            hasLowStock = true;
            lowStockItems += "  - Sandwich: " + sandwichStock + " units\n";
        }

        if (hasLowStock)
        {
            Console.WriteLine("\n⚠️  WARNING: Low Stock Alert!");
            Console.WriteLine("========================================");
            Console.WriteLine(lowStockItems);
            Console.WriteLine("========================================");
        }
    }

    static void AddOrder()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("          ADD ORDER");
        Console.WriteLine("========================================");
        Console.WriteLine("");
        Console.WriteLine("  1. Coffee      - P" + coffeePrice.ToString("0.00") + "  (Stock: " + coffeeStock + ")");
        Console.WriteLine("  2. Tea         - P" + teaPrice.ToString("0.00") + "  (Stock: " + teaStock + ")");
        Console.WriteLine("  3. Pasta       - P" + pastaPrice.ToString("0.00") + "  (Stock: " + pastaStock + ")");
        Console.WriteLine("  4. Cake        - P" + cakePrice.ToString("0.00") + "  (Stock: " + cakeStock + ")");
        Console.WriteLine("  5. Sandwich    - P" + sandwichPrice.ToString("0.00") + "  (Stock: " + sandwichStock + ")");
        Console.WriteLine("");
        Console.WriteLine("========================================");

        Console.Write("\nEnter item number (1-5): ");
        string itemInput = Console.ReadLine();
        
        int itemNum;
        if (!int.TryParse(itemInput, out itemNum) || itemNum < 1 || itemNum > 5)
        {
            Console.WriteLine("\nError: Invalid item number!");
            return;
        }

        Console.Write("Enter quantity: ");
        string qtyInput = Console.ReadLine();
        
        int quantity;
        if (!int.TryParse(qtyInput, out quantity) || quantity <= 0)
        {
            Console.WriteLine("\nError: Invalid quantity!");
            return;
        }

        if (quantity > 100)
        {
            Console.WriteLine("\nError: Quantity too large! Maximum 100 per order.");
            return;
        }

        if (ProcessItemOrder(itemNum, quantity))
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("  ORDER ADDED SUCCESSFULLY!");
            Console.WriteLine("========================================");
            LogAudit(currentUser + " added " + orderItems[orderItems.Count - 1] + " x" + quantity);
        }
    }

    static bool ProcessItemOrder(int itemNum, int quantity)
    {
        string itemName = "";
        double itemPrice = 0;
        bool hasStock = false;

        switch (itemNum)
        {
            case 1:
                itemName = "Coffee";
                itemPrice = coffeePrice;
                if (quantity <= coffeeStock)
                {
                    hasStock = true;
                    coffeeStock -= quantity;
                }
                break;

            case 2:
                itemName = "Tea";
                itemPrice = teaPrice;
                if (quantity <= teaStock)
                {
                    hasStock = true;
                    teaStock -= quantity;
                }
                break;

            case 3:
                itemName = "Pasta";
                itemPrice = pastaPrice;
                if (quantity <= pastaStock)
                {
                    hasStock = true;
                    pastaStock -= quantity;
                }
                break;

            case 4:
                itemName = "Cake";
                itemPrice = cakePrice;
                if (quantity <= cakeStock)
                {
                    hasStock = true;
                    cakeStock -= quantity;
                }
                break;

            case 5:
                itemName = "Sandwich";
                itemPrice = sandwichPrice;
                if (quantity <= sandwichStock)
                {
                    hasStock = true;
                    sandwichStock -= quantity;
                }
                break;
        }

        if (!hasStock)
        {
            Console.WriteLine("\nError: Insufficient stock!");
            Console.WriteLine("Available stock: " + GetAvailableStock(itemNum));
            return false;
        }

        orderItems.Add(itemName);
        orderQuantities.Add(quantity);
        orderPrices.Add(itemPrice);
        
        double lineTotal = itemPrice * quantity;
        Console.WriteLine("\n" + itemName + " x " + quantity + " = P" + lineTotal.ToString("0.00"));
        
        return true;
    }

    static int GetAvailableStock(int itemNum)
    {
        switch (itemNum)
        {
            case 1: return coffeeStock;
            case 2: return teaStock;
            case 3: return pastaStock;
            case 4: return cakeStock;
            case 5: return sandwichStock;
            default: return 0;
        }
    }

    static void ViewCart()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("          YOUR CART");
        Console.WriteLine("========================================");

        if (orderItems.Count == 0)
        {
            Console.WriteLine("\nYour cart is empty!");
        }
        else
        {
            Console.WriteLine("");
            double subtotal = DisplayCartItems();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Subtotal: P" + subtotal.ToString("0.00"));
            Console.WriteLine("");
        }
        Console.WriteLine("========================================");
    }

    static double DisplayCartItems()
    {
        double subtotal = 0;

        for (int i = 0; i < orderItems.Count; i++)
        {
            double lineTotal = orderPrices[i] * orderQuantities[i];
            Console.WriteLine("  " + (i + 1) + ". " + orderItems[i] + " x" + orderQuantities[i] + " = P" + lineTotal.ToString("0.00"));
            subtotal += lineTotal;
        }

        return subtotal;
    }

    static void EditCart()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("          EDIT CART ITEM");
        Console.WriteLine("========================================");

        if (orderItems.Count == 0)
        {
            Console.WriteLine("\nYour cart is empty!");
            return;
        }

        Console.WriteLine("");
        for (int i = 0; i < orderItems.Count; i++)
        {
            Console.WriteLine("  " + (i + 1) + ". " + orderItems[i] + " x" + orderQuantities[i]);
        }
        Console.WriteLine("");

        Console.Write("Enter item number to edit: ");
        string input = Console.ReadLine();
        
        int itemIndex;
        if (!int.TryParse(input, out itemIndex) || itemIndex < 1 || itemIndex > orderItems.Count)
        {
            Console.WriteLine("\nError: Invalid item number!");
            return;
        }

        itemIndex--;

        Console.Write("Enter new quantity (0 to remove): ");
        string qtyInput = Console.ReadLine();
        
        int newQty;
        if (!int.TryParse(qtyInput, out newQty) || newQty < 0)
        {
            Console.WriteLine("\nError: Invalid quantity!");
            return;
        }

        string itemName = orderItems[itemIndex];
        int oldQty = orderQuantities[itemIndex];
        int qtyDiff = newQty - oldQty;

        if (qtyDiff > 0)
        {
            if (!UpdateStock(itemName, qtyDiff, false))
            {
                Console.WriteLine("\nError: Insufficient stock!");
                return;
            }
        }
        else if (qtyDiff < 0)
        {
            UpdateStock(itemName, Math.Abs(qtyDiff), true);
        }

        if (newQty == 0)
        {
            LogAudit(currentUser + " removed " + itemName + " from cart");
            orderItems.RemoveAt(itemIndex);
            orderQuantities.RemoveAt(itemIndex);
            orderPrices.RemoveAt(itemIndex);
            Console.WriteLine("\n✓ Item removed from cart!");
        }
        else
        {
            LogAudit(currentUser + " edited " + itemName + " quantity from " + oldQty + " to " + newQty);
            orderQuantities[itemIndex] = newQty;
            Console.WriteLine("\n✓ Quantity updated successfully!");
        }
    }

    static bool UpdateStock(string itemName, int quantity, bool addBack)
    {
        switch (itemName)
        {
            case "Coffee":
                if (!addBack && quantity > coffeeStock) return false;
                coffeeStock = addBack ? coffeeStock + quantity : coffeeStock - quantity;
                break;

            case "Tea":
                if (!addBack && quantity > teaStock) return false;
                teaStock = addBack ? teaStock + quantity : teaStock - quantity;
                break;

            case "Pasta":
                if (!addBack && quantity > pastaStock) return false;
                pastaStock = addBack ? pastaStock + quantity : pastaStock - quantity;
                break;

            case "Cake":
                if (!addBack && quantity > cakeStock) return false;
                cakeStock = addBack ? cakeStock + quantity : cakeStock - quantity;
                break;

            case "Sandwich":
                if (!addBack && quantity > sandwichStock) return false;
                sandwichStock = addBack ? sandwichStock + quantity : sandwichStock - quantity;
                break;
        }

        return true;
    }

    static void RemoveFromCart()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("        REMOVE FROM CART");
        Console.WriteLine("========================================");

        if (orderItems.Count == 0)
        {
            Console.WriteLine("\nYour cart is empty!");
            return;
        }

        Console.WriteLine("");
        for (int i = 0; i < orderItems.Count; i++)
        {
            Console.WriteLine("  " + (i + 1) + ". " + orderItems[i] + " x" + orderQuantities[i]);
        }
        Console.WriteLine("");

        Console.Write("Enter item number to remove: ");
        string input = Console.ReadLine();
        
        int itemIndex;
        if (!int.TryParse(input, out itemIndex) || itemIndex < 1 || itemIndex > orderItems.Count)
        {
            Console.WriteLine("\nError: Invalid item number!");
            return;
        }

        itemIndex--;

        string itemName = orderItems[itemIndex];
        int qty = orderQuantities[itemIndex];

        UpdateStock(itemName, qty, true);

        LogAudit(currentUser + " removed " + itemName + " x" + qty + " from cart");

        orderItems.RemoveAt(itemIndex);
        orderQuantities.RemoveAt(itemIndex);
        orderPrices.RemoveAt(itemIndex);

        Console.WriteLine("\n✓ Item removed from cart!");
    }

    static void CancelOrder()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("          CANCEL ORDER");
        Console.WriteLine("========================================");

        if (orderItems.Count == 0)
        {
            Console.WriteLine("\nNo active order to cancel!");
            return;
        }

        Console.WriteLine("\nAre you sure you want to cancel this order?");
        Console.Write("Confirm (Y/N): ");
        
        string confirm = Console.ReadLine();
        
        if (confirm != null && confirm.ToUpper() == "Y")
        {
            for (int i = 0; i < orderItems.Count; i++)
            {
                UpdateStock(orderItems[i], orderQuantities[i], true);
            }

            LogAudit(currentUser + " cancelled order with " + orderItems.Count + " items");

            orderItems.Clear();
            orderQuantities.Clear();
            orderPrices.Clear();

            Console.WriteLine("\n✓ Order cancelled successfully!");
        }
    }

    static void ProcessCheckout()
    {
        Console.Clear();
        
        if (orderItems.Count == 0)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("          CHECKOUT");
            Console.WriteLine("========================================");
            Console.WriteLine("\nNo items to checkout!");
            return;
        }

        Console.WriteLine("========================================");
        Console.WriteLine("          CHECKOUT");
        Console.WriteLine("========================================");

        double subtotal = CalculateTotal();
        double vat = subtotal * 0.12;
        double grandTotal = subtotal + vat;

        Console.WriteLine("");
        Console.WriteLine("Subtotal:     P" + subtotal.ToString("0.00"));
        Console.WriteLine("VAT (12%):    P" + vat.ToString("0.00"));
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("GRAND TOTAL:  P" + grandTotal.ToString("0.00"));

        double payment = GetPayment(grandTotal);
        double change = payment - grandTotal;

        PrintReceipt(subtotal, vat, grandTotal, payment, change);
        SaveReceipt(subtotal, vat, grandTotal, payment, change);
        SaveSale(subtotal, vat, grandTotal);
        SaveStock();
        
        LogAudit(currentUser + " completed checkout - Total: P" + grandTotal.ToString("0.00"));

        Console.Write("\nStart new order? (Y/N): ");
        string newOrder = Console.ReadLine();
        
        if (newOrder != null && newOrder.ToUpper() == "Y")
        {
            orderItems.Clear();
            orderQuantities.Clear();
            orderPrices.Clear();
            Console.WriteLine("\n✓ New order started!");
        }
        else
        {
            orderItems.Clear();
            orderQuantities.Clear();
            orderPrices.Clear();
        }
    }

    static double CalculateTotal()
    {
        double total = 0;

        for (int i = 0; i < orderItems.Count; i++)
        {
            total += orderPrices[i] * orderQuantities[i];
        }

        return total;
    }

    static double GetPayment(double grandTotal)
    {
        while (true)
        {
            Console.Write("\nEnter payment amount: P");
            string paymentInput = Console.ReadLine();
            
            double payment;
            if (double.TryParse(paymentInput, out payment))
            {
                if (payment >= grandTotal)
                {
                    return payment;
                }
                else
                {
                    double shortage = grandTotal - payment;
                    Console.WriteLine("\nInsufficient payment!");
                    Console.WriteLine("Amount needed: P" + shortage.ToString("0.00") + " more");
                }
            }
            else
            {
                Console.WriteLine("\nError: Invalid payment amount!");
            }
        }
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
        Console.WriteLine("Cashier: " + currentUser);
        Console.WriteLine("========================================\n");

        for (int i = 0; i < orderItems.Count; i++)
        {
            Console.WriteLine(orderItems[i] + " x" + orderQuantities[i]);
            Console.WriteLine("  P" + (orderPrices[i] * orderQuantities[i]).ToString("0.00"));
        }

        Console.WriteLine("\n----------------------------------------");
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

    static void RestockInventory()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("        RESTOCK INVENTORY");
        Console.WriteLine("========================================");
        Console.WriteLine("\nCurrent Stock:");
        Console.WriteLine("  1. Coffee:   " + coffeeStock + " units");
        Console.WriteLine("  2. Tea:      " + teaStock + " units");
        Console.WriteLine("  3. Pasta:    " + pastaStock + " units");
        Console.WriteLine("  4. Cake:     " + cakeStock + " units");
        Console.WriteLine("  5. Sandwich: " + sandwichStock + " units");
        Console.WriteLine("========================================");

        Console.Write("\nEnter item number (1-5): ");
        string itemInput = Console.ReadLine();
        
        int itemNum;
        if (!int.TryParse(itemInput, out itemNum) || itemNum < 1 || itemNum > 5)
        {
            Console.WriteLine("\nError: Invalid item number!");
            return;
        }

        Console.Write("Enter quantity to add: ");
        string qtyInput = Console.ReadLine();
        
        int quantity;
        if (!int.TryParse(qtyInput, out quantity) || quantity <= 0)
        {
            Console.WriteLine("\nError: Invalid quantity!");
            return;
        }

        string itemName = "";
        int oldStock = 0;
        int newStock = 0;

        switch (itemNum)
        {
            case 1:
                itemName = "Coffee";
                oldStock = coffeeStock;
                coffeeStock += quantity;
                newStock = coffeeStock;
                break;

            case 2:
                itemName = "Tea";
                oldStock = teaStock;
                teaStock += quantity;
                newStock = teaStock;
                break;

            case 3:
                itemName = "Pasta";
                oldStock = pastaStock;
                pastaStock += quantity;
                newStock = pastaStock;
                break;

            case 4:
                itemName = "Cake";
                oldStock = cakeStock;
                cakeStock += quantity;
                newStock = cakeStock;
                break;

            case 5:
                itemName = "Sandwich";
                oldStock = sandwichStock;
                sandwichStock += quantity;
                newStock = sandwichStock;
                break;
        }

        Console.WriteLine("\n✓ Stock updated successfully!");
        Console.WriteLine(itemName + ": " + oldStock + " → " + newStock + " units");
        
        LogInventory(itemName, quantity, "Restocked");
        LogAudit(currentUser + " restocked " + itemName + " +" + quantity + " units");
        SaveStock();
    }

    static void GenerateDailySummary()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("       DAILY SALES SUMMARY");
        Console.WriteLine("========================================");
        Console.WriteLine("Date: " + DateTime.Now.ToString("MM/dd/yyyy"));
        Console.WriteLine("========================================\n");

        try
        {
            if (!File.Exists("sales.txt"))
            {
                Console.WriteLine("No sales data available yet!");
                return;
            }

            string[] lines = File.ReadAllLines("sales.txt");
            string today = DateTime.Now.ToString("MM/dd/yyyy");

            int transactionCount = 0;
            double totalRevenue = 0;
            double totalVAT = 0;
            int coffeeCount = 0, teaCount = 0, pastaCount = 0, cakeCount = 0, sandwichCount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Date: ") && lines[i].Contains(today))
                {
                    transactionCount++;

                    for (int j = i; j < lines.Length && j < i + 20; j++)
                    {
                        if (lines[j].Contains("Grand Total:"))
                        {
                            string amountStr = lines[j].Split('P')[1].Trim();
                            totalRevenue += Convert.ToDouble(amountStr);
                        }
                        if (lines[j].Contains("VAT (12%):"))
                        {
                            string vatStr = lines[j].Split('P')[1].Trim();
                            totalVAT += Convert.ToDouble(vatStr);
                        }
                        if (lines[j].Contains("- Coffee x "))
                        {
                            string qtyStr = lines[j].Split('x')[1].Trim();
                            coffeeCount += Convert.ToInt32(qtyStr);
                        }
                        if (lines[j].Contains("- Tea x "))
                        {
                            string qtyStr = lines[j].Split('x')[1].Trim();
                            teaCount += Convert.ToInt32(qtyStr);
                        }
                        if (lines[j].Contains("- Pasta x "))
                        {
                            string qtyStr = lines[j].Split('x')[1].Trim();
                            pastaCount += Convert.ToInt32(qtyStr);
                        }
                        if (lines[j].Contains("- Cake x "))
                        {
                            string qtyStr = lines[j].Split('x')[1].Trim();
                            cakeCount += Convert.ToInt32(qtyStr);
                        }
                        if (lines[j].Contains("- Sandwich x "))
                        {
                            string qtyStr = lines[j].Split('x')[1].Trim();
                            sandwichCount += Convert.ToInt32(qtyStr);
                        }
                    }
                }
            }

            Console.WriteLine("Total Transactions: " + transactionCount);
            Console.WriteLine("Total Revenue:      P" + totalRevenue.ToString("0.00"));
            Console.WriteLine("Total VAT:          P" + totalVAT.ToString("0.00"));
            
            if (transactionCount > 0)
            {
                double avgSale = totalRevenue / transactionCount;
                Console.WriteLine("Average Sale:       P" + avgSale.ToString("0.00"));
            }

            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("         ITEMS SOLD TODAY");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Coffee:             " + coffeeCount + " units");
            Console.WriteLine("Tea:                " + teaCount + " units");
            Console.WriteLine("Pasta:              " + pastaCount + " units");
            Console.WriteLine("Cake:               " + cakeCount + " units");
            Console.WriteLine("Sandwich:           " + sandwichCount + " units");
            Console.WriteLine("========================================");

            string bestSeller = "N/A";
            int maxSold = 0;
            
            if (coffeeCount > maxSold) { maxSold = coffeeCount; bestSeller = "Coffee"; }
            if (teaCount > maxSold) { maxSold = teaCount; bestSeller = "Tea"; }
            if (pastaCount > maxSold) { maxSold = pastaCount; bestSeller = "Pasta"; }
            if (cakeCount > maxSold) { maxSold = cakeCount; bestSeller = "Cake"; }
            if (sandwichCount > maxSold) { maxSold = sandwichCount; bestSeller = "Sandwich"; }

            Console.WriteLine("\nBEST SELLER: " + bestSeller + " (" + maxSold + " units)");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to generate report: " + ex.Message);
        }
    }

    static void ViewInventoryLog()
    {
        Console.Clear();
        
        try
        {
            if (File.Exists("inventory.txt"))
            {
                Console.WriteLine("========================================");
                Console.WriteLine("       INVENTORY MOVEMENT LOG");
                Console.WriteLine("========================================\n");
                string logContent = File.ReadAllText("inventory.txt");
                Console.WriteLine(logContent);
            }
            else
            {
                Console.WriteLine("========================================");
                Console.WriteLine("       INVENTORY MOVEMENT LOG");
                Console.WriteLine("========================================");
                Console.WriteLine("\nNo inventory records found yet!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to read inventory log: " + ex.Message);
        }
    }

    static void ViewSalesLog()
    {
        Console.Clear();
        
        try
        {
            if (File.Exists("sales.txt"))
            {
                Console.WriteLine("========================================");
                Console.WriteLine("          SALES LOG");
                Console.WriteLine("========================================");
                string logContent = File.ReadAllText("sales.txt");
                Console.WriteLine(logContent);
            }
            else
            {
                Console.WriteLine("========================================");
                Console.WriteLine("          SALES LOG");
                Console.WriteLine("========================================");
                Console.WriteLine("\nNo sales records found yet!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to read sales log: " + ex.Message);
        }
    }

    static void ViewAuditLog()
    {
        Console.Clear();
        
        try
        {
            if (File.Exists("audit.txt"))
            {
                Console.WriteLine("========================================");
                Console.WriteLine("           AUDIT LOG");
                Console.WriteLine("========================================\n");
                string logContent = File.ReadAllText("audit.txt");
                Console.WriteLine(logContent);
            }
            else
            {
                Console.WriteLine("========================================");
                Console.WriteLine("           AUDIT LOG");
                Console.WriteLine("========================================");
                Console.WriteLine("\nNo audit records found yet!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[Error] Failed to read audit log: " + ex.Message);
        }
    }

    static void SaveReceipt(double subtotal, double vat, double grandTotal, double payment, double change)
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
            receipt += "Cashier: " + currentUser + "\n";
            receipt += "========================================\n\n";

            for (int i = 0; i < orderItems.Count; i++)
            {
                receipt += orderItems[i] + " x" + orderQuantities[i] + "\n";
                receipt += "  P" + (orderPrices[i] * orderQuantities[i]).ToString("0.00") + "\n";
            }

            receipt += "\n----------------------------------------\n";
            receipt += "Subtotal:     P" + subtotal.ToString("0.00") + "\n";
            receipt += "VAT (12%):    P" + vat.ToString("0.00") + "\n";
            receipt += "----------------------------------------\n";
            receipt += "GRAND TOTAL:  P" + grandTotal.ToString("0.00") + "\n";
            receipt += "Payment:      P" + payment.ToString("0.00") + "\n";
            receipt += "Change:       P" + change.ToString("0.00") + "\n";
            receipt += "========================================\n";

            File.WriteAllText("receipt.txt", receipt);
        }
        catch { }
    }

    static void SaveSale(double subtotal, double vat, double grandTotal)
    {
        try
        {
            string logEntry = "\n========================================\n";
            logEntry += "          SALE RECORD\n";
            logEntry += "========================================\n";
            logEntry += "Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
            logEntry += "Cashier: " + currentUser + "\n";
            logEntry += "----------------------------------------\n";
            logEntry += "Subtotal:     P" + subtotal.ToString("0.00") + "\n";
            logEntry += "VAT (12%):    P" + vat.ToString("0.00") + "\n";
            logEntry += "Grand Total:  P" + grandTotal.ToString("0.00") + "\n";
            logEntry += "----------------------------------------\n";
            logEntry += "Items Sold:\n";

            for (int i = 0; i < orderItems.Count; i++)
            {
                logEntry += "  - " + orderItems[i] + " x " + orderQuantities[i] + "\n";
            }

            logEntry += "========================================\n";

            File.AppendAllText("sales.txt", logEntry);
        }
        catch { }
    }

    static void SaveStock()
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

            File.WriteAllText("stock.txt", stockData);
        }
        catch { }
    }

    static void LoadStock()
    {
        try
        {
            if (File.Exists("stock.txt"))
            {
                string[] lines = File.ReadAllLines("stock.txt");

                if (lines.Length >= 8)
                {
                    coffeeStock = Convert.ToInt32(lines[3].Split(':')[1].Trim().Replace(" units", ""));
                    teaStock = Convert.ToInt32(lines[4].Split(':')[1].Trim().Replace(" units", ""));
                    pastaStock = Convert.ToInt32(lines[5].Split(':')[1].Trim().Replace(" units", ""));
                    cakeStock = Convert.ToInt32(lines[6].Split(':')[1].Trim().Replace(" units", ""));
                    sandwichStock = Convert.ToInt32(lines[7].Split(':')[1].Trim().Replace(" units", ""));

                    Console.WriteLine("[Stock loaded from file]\n");
                }
            }
        }
        catch { }
    }

    static void LogInventory(string itemName, int quantity, string action)
    {
        try
        {
            string logEntry = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " | ";
            logEntry += action + " | " + itemName + " | +" + quantity + " | By: " + currentUser + "\n";

            File.AppendAllText("inventory.txt", logEntry);
        }
        catch { }
    }

    static void LogAudit(string action)
    {
        try
        {
            string logEntry = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + " | " + action + "\n";
            File.AppendAllText("audit.txt", logEntry);
        }
        catch { }
    }

    static bool ExitSystem()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("        CLOSING SYSTEM");
        Console.WriteLine("========================================");
        
        SaveStock();
        LogAudit(currentUser + " logged out");
        
        Console.WriteLine("\nThank you for using Cafe POS!");
        Console.WriteLine("Goodbye!\n");
        Console.WriteLine("========================================");
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
        return false;
    }
}
