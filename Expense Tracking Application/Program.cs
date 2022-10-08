using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

Items.populateExpenseList();
Items.showOptions();

[Serializable]
class Items   // base-parent-super class
{
    public static int counter = 0;
    public int Id { get; set; }
    public string Title { get; set; }
    public string Month { get; set; }
    public double Amount { get; set; }

    static List<Items> expensesList = new List<Items>();
    static string fileName = @"C:\Temp\Expenses.txt";
    public static void showItems()
    {
        Console.WriteLine("Id".PadRight(10) + "Title".PadRight(10) + "Month".PadRight(10)
                + "Amount".PadRight(10));
        foreach (Items items in expensesList)
        {
            Console.WriteLine(items.Id.ToString().PadRight(10) + items.Title.PadRight(10) + items.Month.PadRight(10)
                + items.Amount.ToString().PadRight(10));
        }
        Items.showOptions();
    }

    public static void addExpense()
    {
        Console.WriteLine("What do you like to enter? Expense/Income?");
        String expenseType = Console.ReadLine();
        Console.WriteLine("Enter the Title");
        String expenseTitle = Console.ReadLine();
        Console.WriteLine("Enter the Month");
        String expenseMonth = Console.ReadLine();
        Console.WriteLine("Enter the Amount");
        String expenseAmount = Console.ReadLine();
        if (expenseType.ToLower() == "expense")
        {
            expensesList.Add(new Expense(expenseTitle, expenseMonth, -Convert.ToDouble(expenseAmount)));
        }
        else
        {
            expensesList.Add(new Income(expenseTitle, expenseMonth, Convert.ToDouble(expenseAmount)));
        }
		Console.WriteLine("Expense/Income added successfully");
		Items.showOptions();
    }

    public static void editRemoveItem()
    {
        Console.WriteLine("What do you want to do? (Edit/Remove)");
        string input = Console.ReadLine();
        if(input.ToLower().Trim() == "remove")
        {
            Items itemToDelete = null;
            Console.WriteLine("Enter Id to remove");
            string id = Console.ReadLine();
            foreach (Items items in expensesList)
            {
                if(items.Id == int.Parse(id))
                {
                    itemToDelete = items;
                }
            }
            expensesList.Remove(itemToDelete);
        }
        else if(input.ToLower().Trim() == "edit")
        {
            Console.WriteLine("Enter Id to edit");
            string id = Console.ReadLine();

            Console.WriteLine("Enter the new Title");
            String expenseTitle = Console.ReadLine();
            Console.WriteLine("Enter the new Month");
            String expenseMonth = Console.ReadLine();
            Console.WriteLine("Enter the new Amount");
            String expenseAmount = Console.ReadLine();
            foreach (Items items in expensesList)
            {
                if(items.Id == int.Parse(id))
                {
                    items.Title = expenseTitle;
                    items.Month = expenseMonth;
                    if(items.GetType() == typeof(Income))
                    {
                        items.Amount = Convert.ToDouble(expenseAmount);
                    }
                    else if(items.GetType() == typeof(Expense))
                    {
                        items.Amount = -Convert.ToDouble(expenseAmount);
                    }
                }
            }
        }
        Console.WriteLine("Expense/Income edited/deleted successfully");
        Items.showOptions();
    }

    public static void saveToFile()
    {
        
            File.Delete(fileName);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, expensesList);
            stream.Close();
		    Console.WriteLine("Data saved successfully to file");
    }

    public static void populateExpenseList()
    {
        if (File.Exists(fileName))
        {
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            IFormatter formatter = new BinaryFormatter();
            expensesList = (List<Items>)formatter.Deserialize(stream);
            stream.Close();
            counter = expensesList.ElementAt(expensesList.Count - 1).Id;
        }
        Items.showOptions();
    }

    public static void showOptions()
    {
		Console.WriteLine("Welcome to TrackMoney");
		Console.WriteLine("You have currently " +Items.calcFinalBalance()+ "Kr in your account.");
		Console.WriteLine("Pick an option:");
		Console.WriteLine("(1) Show items (All/Expense(s)/Income(s))");
		Console.WriteLine("(2) Add New Expense/Income");
		Console.WriteLine("(3) Edit item (Edit, Remove)");
		Console.WriteLine("(4) Save and Quit");
		string input = Console.ReadLine();
		switch (input)
		{
			case "1":
				Items.showItems();
				break;
			case "2":
				Items.addExpense();
				break;
			case "3":
				Items.editRemoveItem();
				break;
			case "4":
				Items.saveToFile();
				break;
			default:
				break;
		}
    }

    public static double calcFinalBalance()
    {
        double balance = 0;
        foreach (Items items in expensesList)
        {
            balance = balance + items.Amount;
        }
        return balance;
    }
}

[Serializable]
class Expense : Items // inherits from Items
{
    public Expense(string title, string month, double amount)
    {
        counter++;
        Id = counter;
        Title = title;
        Month = month;
        Amount = amount;
    }
}

[Serializable]
class Income : Items
{
    public Income(string title, string month, double amount)
    {
        counter++;
        Id = counter;
        Title = title;
        Month = month;
        Amount = amount;
    }
}

