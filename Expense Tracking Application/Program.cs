using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

Console.WriteLine("Welcome to TrackMoney");
Console.WriteLine("You have currently 100kr in your account.");
Console.WriteLine("Pick an option:");
Console.WriteLine("(1) Show items (All/Expense(s)/Income(s))");
Console.WriteLine("(2) Add New Expense/Income");
Console.WriteLine("(3) Edit item (Edit, Remove)");
Console.WriteLine("(4) Save and Quit");
int input = Console.ReadLine();
string fileName = @"C:\Temp\Expenses.txt";
List<Items> expensesList = new List<Items>();
Items.populateExpenseList();
switch (input)
{
    case 1:
        Items.showItems();
        break;
    case 2:
        Items.addExpense();
        break;
    case 3:
        Items.editRemoveItem();
        break;
    case 4:
        Items.saveToFile();
        break;
    default:
        break;
}


Console.ReadLine();

[Serializable]
class Items   // base-parent-super class
{
    
    public string Title { get; set; }
    public string Month { get; set; }
    public double Amount { get; set; }

    public static void showItems()
    {
        foreach (Expense expense in expensesList)
        {
            Console.WriteLine(expense.Title.PadRight(10) + expense.Month.PadRight(10)
                + expense.Amount.ToString().PadRight(10));
        }
    }

    public static void addExpense()
    {
        Console.WriteLine("What do you like to enter? Expense/Income?");
        String expenseType = Console.ReadLine();
        Console.WriteLine("Enter the Title of the expense");
        String expenseTitle = Console.ReadLine();
        Console.WriteLine("Enter the Month of the expense");
        String expenseMonth = Console.ReadLine();
        Console.WriteLine("Enter the Amount of the expense");
        String expenseAmount = Console.ReadLine();
        if (expenseType.ToLower() == "expense")
        {
            expensesList.Add(new Expense(expenseTitle, expenseMonth, Convert.ToDouble(expenseAmount)));
        }
        else
        {
            expensesList.Add(new Income(expenseTitle, expenseMonth, Convert.ToDouble(expenseAmount)));
        }

    }

    public static void editRemoveItem()
    {

    }

    public static void saveToFile()
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, expensesList);
            stream.Close();
        }
    }

    public static void populateExpenseList()
    {
        Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        IFormatter formatter = new BinaryFormatter();
        expensesList = (List<Items>)formatter.Deserialize(stream);
        stream.Close();
    }
}

class Expense : Items // inherits from Items
{
    public Expense(string title, string month, double amount)
    {
        Title = title;
        Month = month;
        Amount = amount;
    }
}

class Income : Items
{
    public Income(string title, string month, double amount)
    {
        Title = title;
        Month = month;
        Amount = amount;
    }
}

