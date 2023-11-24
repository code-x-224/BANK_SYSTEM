using System;
using System.IO;
using System.Text;

public struct Person
{
    public string fname;
    public string phone;
    public string account_no;
    public string pass;
    public float balance;
}

class Program
{
    static void Main()
    {
        string filename;
        DateTime currenttime = DateTime.Now;
        string login;
        string pass2;
        float deposit;
        Person guy1,guy2;
        int choice = 0;
        int opt = 0;

        while (choice != 3)
        {
            menu();
            Console.Write("\t\tEnter choice:\t");
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateAccount(out guy1);
                    break;

                case 2:
                    if (LogIn(out guy1, out login, out pass2, out filename)){
                        Console.Clear();
                        Console.WriteLine($"\t\tWelcome back {guy1.fname}!!");
                         while (opt < 6){
                            menu2();
                            Console.Write("\t\tEnter option:");
                            opt = int.Parse(Console.ReadLine());
                            ProcessOption(opt, guy1, filename, out deposit, currenttime, out guy2);
                        }
                     }
                    else{
                        Console.WriteLine("\t\tWrong password or account does not exist.");
                        }
                    break;
                case 3:
                    Console.WriteLine("\t\tThank you for banking with us!");
                    break;

                default:
                    Console.WriteLine("\t\tInvalid option!!!");
                    break;
            }
        }
    }

    static void menu()
    {
        Console.WriteLine("\t\t################################");
        Console.WriteLine("\t\t##WELCOME TO CODEX BANK SYSTEM##");
        Console.WriteLine("\t\t################################");
        Console.WriteLine("\t\tEnter 1 to create account.");
        Console.WriteLine("\t\tEnter 2 to log in.");
        Console.WriteLine("\t\tEnter 3 to exit.");
    }

    static void menu2()
    {
        Console.WriteLine("\t\tEnter 1 to deposit.");
        Console.WriteLine("\t\tEnter 2 to withdraw.");
        Console.WriteLine("\t\tEnter 3 to send money.");
        Console.WriteLine("\t\tEnter 4 to check balance");
        Console.WriteLine("\t\tEnter 5 to change password");
        Console.WriteLine("\t\tEnter 6 to log out");
    }

    static string GetPassword()
    {
        Console.Write("\t\tEnter password:");
        StringBuilder password = new StringBuilder();

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b"); // Move the cursor back and erase the character
                }
            }
            else
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
        }

        Console.WriteLine(); // Move to the next line after password input
        return password.ToString();
    }

    static void CreateAccount(out Person guy1)
    {
        Console.Clear();
        Console.Write("\t\tEnter first name:");
        guy1.fname = Console.ReadLine();
        Console.Write("\t\tEnter phone number:");
        guy1.phone = Console.ReadLine();
        Console.Write("\t\tEnter account number:");
        guy1.account_no = Console.ReadLine();
        guy1.pass = GetPassword();
        Console.Write("\n\t\tEnter account balance:");
        guy1.balance = float.Parse(Console.ReadLine());
        string filename = guy1.phone + ".dat";

        if (File.Exists(filename))
        {
            Console.WriteLine("\n\t\tSorry !! Account already exists.");
        }
        else
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(guy1.fname);
                writer.WriteLine(guy1.phone);
                writer.WriteLine(guy1.account_no);
                writer.WriteLine(guy1.pass);
                writer.WriteLine(guy1.balance);
            }

            Console.WriteLine("\n\t\tAccount registration successful.");
        }
    }

static bool LogIn(out Person guy1, out string login, out string pass2, out string filename){
        Console.Clear();
        Console.Write("\t\tEnter phone number:");
        login = Console.ReadLine();
        pass2 = GetPassword();
        filename = login + ".dat";

        guy1 = new Person(); // Initialize guy1

        if (File.Exists(filename))
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                guy1.fname = reader.ReadLine();
                guy1.phone = reader.ReadLine();
                guy1.account_no = reader.ReadLine();
                guy1.pass = reader.ReadLine();
                guy1.balance = float.Parse(reader.ReadLine());
            }

            // Check if the entered password matches the stored password
            return guy1.pass == pass2;
        }
        else
        {
            Console.WriteLine("\t\tAccount does not exist.");
            return false;
        }
}

    static void ProcessOption(int opt, Person guy1, string filename, out float deposit, DateTime currenttime, out Person guy2)
    {
        deposit = 0;
        guy2 = new Person();
        guy1 = new Person();

        switch (opt)
        {
            case 1:
                Console.Clear();
                Console.Write("\t\tEnter amount to deposit:");
                deposit = float.Parse(Console.ReadLine());
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    guy1.balance += deposit;
                    writer.WriteLine(guy1.fname);
                    writer.WriteLine(guy1.phone);
                    writer.WriteLine(guy1.account_no);
                    writer.WriteLine(guy1.pass);
                    writer.WriteLine(guy1.balance);
                }
                Console.WriteLine($"\t\tDeposit of Ksh.{deposit:C2} on {currenttime} was successful. Your account balance is {guy1.balance:C2}.Transaction reference:");
                break;

            case 2:
                Console.Clear();
                Console.Write("\t\tEnter amount to withdraw:");
                deposit = float.Parse(Console.ReadLine());
                if (deposit > guy1.balance)
                {
                    Console.WriteLine($"\t\tCannot withdraw that amount. Your current balance is {guy1.balance:C2}");
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(filename))
                    {
                        guy1.balance -= deposit;
                        writer.WriteLine(guy1.fname);
                        writer.WriteLine(guy1.phone);
                        writer.WriteLine(guy1.account_no);
                        writer.WriteLine(guy1.pass);
                        writer.WriteLine(guy1.balance);
                    }
                    Console.WriteLine($"\t\tWithdrawal of Ksh.{deposit:C2} on {currenttime} successful. Your account balance is{guy1.balance:C2}.Transaction reference:");
                }
                break;

            case 3:
                Console.Clear();
                string login;
                Console.Write("\t\tEnter account to send money:");
                login = Console.ReadLine();
                string filename2 = login + ".dat";
                using (FileStream fs = new FileStream(filename2, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    if (reader.PeekChar() != -1)
                    {
                        fs.Seek(0, SeekOrigin.Begin);
                        guy2.fname = reader.ReadString();
                        guy2.phone = reader.ReadString();
                        guy2.account_no = reader.ReadString();
                        guy2.pass = reader.ReadString();
                        guy2.balance = reader.ReadSingle();
                        Console.WriteLine("\t\tEnter amount to send:");
                        deposit = float.Parse(Console.ReadLine());
                        using (FileStream fsSender = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                        using (BinaryReader senderReader = new BinaryReader(fsSender))
                        using (BinaryWriter senderWriter = new BinaryWriter(fsSender))
                        {
                            fsSender.Seek(0, SeekOrigin.Begin);
                            guy1.fname = senderReader.ReadString();
                            guy1.phone = senderReader.ReadString();
                            guy1.account_no = senderReader.ReadString();
                            guy1.pass = senderReader.ReadString();
                            guy1.balance = senderReader.ReadSingle();
                            if (deposit <= guy1.balance)
                            {
                                fsSender.SetLength(0);
                                fsSender.Seek(0, SeekOrigin.Begin);
                                guy2.balance += deposit;
                                senderWriter.Write(guy2.fname);
                                senderWriter.Write(guy2.phone);
                                senderWriter.Write(guy2.account_no);
                                senderWriter.Write(guy2.pass);
                                senderWriter.Write(guy2.balance);

                                Console.WriteLine($"\t\tTransfer to {guy2.fname} on {currenttime} was successful.Transaction reference:");

                                fsSender.SetLength(0);
                                fsSender.Seek(0, SeekOrigin.Begin);
                                guy1.balance -= deposit;
                                senderWriter.Write(guy1.fname);
                                senderWriter.Write(guy1.phone);
                                senderWriter.Write(guy1.account_no);
                                senderWriter.Write(guy1.pass);
                                senderWriter.Write(guy1.balance);

                                Console.WriteLine($"\t\tYour current balance is {guy1.balance:C2}");
                            }
                            else
                            {
                                Console.WriteLine($"\t\tInsufficient balance to send {deposit:C2}! Your current balance is {guy1.balance:C2}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\t\tAccount does not exist.");
                    }
                }
                break;

            case 4:
                Console.WriteLine($"\t\tYour current account balance is {guy1.balance:C2}.Transaction reference:");
                break;

            case 5:
                Console.Clear();
                string pass2;
                Console.Write("\t\tEnter current password:");
                pass2 = Console.ReadLine();

                if (pass2 == guy1.pass)
                {
                    Console.Write("\t\tEnter new password:");
                    string pass3 = Console.ReadLine();

                    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Write))
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        char[] empty = new char[30];
                        Array.Clear(empty, 0, empty.Length);
                        guy1.pass = new string(empty);
                        guy1.pass += pass3;

                        fs.SetLength(0);
                        fs.Seek(0, SeekOrigin.Begin);
                        writer.Write(guy1.fname);
                        writer.Write(guy1.phone);
                        writer.Write(guy1.account_no);
                        writer.Write(guy1.pass);
                        writer.Write(guy1.balance);

                        Console.WriteLine($"\t\tPassword reset successful. New password is {guy1.pass}");
                    }
                }
                else
                {
                    Console.WriteLine("\t\tWrong password");
                }
                break;

            case 6:
                Console.WriteLine("\t\tLogging out.....");
                break;

            default:
                Console.WriteLine("\t\tInvalid option!!!");
                guy1 = new Person(); // Assign a default value
                deposit = 0;
                guy2 = new Person();
                break;
        }
    }
}
