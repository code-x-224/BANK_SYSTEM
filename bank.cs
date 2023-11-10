using System;
using System.IO;


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
        DateTime currenttime=DateTime.Now;
        string login;
        string pass2;
        float deposit;
        Person guy1, guy2;
        int choice = 0;
        int opt = 0;

        while (choice != 3)
        {
            menu();
            Console.Write("Enter choice:\t");
            choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                Console.Clear();
                Console.Write("Enter first name:");
                guy1.fname = Console.ReadLine();
                Console.Write("Enter phone number:");
                guy1.phone = Console.ReadLine();
                Console.Write("Enter account number:");
                guy1.account_no = Console.ReadLine();
                Console.Write("Enter password:");
                guy1.pass = Console.ReadLine();
                guy1.balance = 0;

                filename = guy1.phone + ".dat";
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine(guy1.fname);
                    writer.WriteLine(guy1.phone);
                    writer.WriteLine(guy1.account_no);
                    writer.WriteLine(guy1.pass);
                    writer.WriteLine(guy1.balance);
                }

                Console.WriteLine("Account registration successful.");
            }

            if (choice == 2)
            {
                Console.Clear();
                Console.Write("Enter phone number:");
                login = Console.ReadLine();
                Console.Write("Enter password:");
                pass2 = Console.ReadLine();

                filename = login + ".dat";

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

                    if (pass2 == guy1.pass)
                    {
                        Console.Clear();

                        while (opt < 6)
                        {
                            menu2();
                            Console.Write("Enter option:");
                            opt = int.Parse(Console.ReadLine());

                            switch (opt)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.Write("Enter amount to deposit:");
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
                                    Console.WriteLine($"Deposit successful. Your account balance is {guy1.balance:C2}");
                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("Enter amount to withdraw");
                                    deposit=float.Parse(Console.ReadLine());
                                    if(deposit>guy1.balance){
                                        Console.WriteLine($"Cannot withdraw that amount your current balance is {guy1.balance:C2}");
                                    }
                                    else{
                                      using (StreamWriter writer = new StreamWriter(filename))
                                    {
                                        guy1.balance -= deposit;
                                        writer.WriteLine(guy1.fname);
                                        writer.WriteLine(guy1.phone);
                                        writer.WriteLine(guy1.account_no);
                                        writer.WriteLine(guy1.pass);
                                        writer.WriteLine(guy1.balance);
                                    }
                                    Console.WriteLine($"Withdrawal successful.Your account balance is{guy1.balance:C2}"); 
                                    }
                                    break;
                                case 3:
                                    Console.Clear();
                                    Console.Write("Enter account to send money:");
                                    login = Console.ReadLine();
                                    string filename2 = login + ".dat";
                                    using (FileStream fs = new FileStream(filename2, FileMode.Open, FileAccess.Read))
                                    using (BinaryReader reader = new BinaryReader(fs)){
                                            guy2 = new Person();
                                            if (reader.PeekChar() != -1)
                                             {
                                                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                                                guy2.fname = reader.ReadString();
                                                guy2.phone = reader.ReadString();
                                                guy2.account_no = reader.ReadString();
                                                guy2.pass = reader.ReadString();
                                                guy2.balance = reader.ReadSingle();
                                                Console.WriteLine("Enter amount to send:");
                                                deposit = float.Parse(Console.ReadLine());
                                                using (FileStream fsSender = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                                                using (BinaryReader senderReader = new BinaryReader(fsSender))
                                                using (BinaryWriter senderWriter = new BinaryWriter(fsSender))
                                                {
                                                    guy1 = new Person();
                                                    senderReader.BaseStream.Seek(0, SeekOrigin.Begin);
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

                                                            Console.WriteLine($"Transfer to {guy2.fname} successful.");

                                                            fsSender.SetLength(0);
                                                            fsSender.Seek(0, SeekOrigin.Begin);
                                                            guy1.balance -= deposit;
                                                            senderWriter.Write(guy1.fname);
                                                            senderWriter.Write(guy1.phone);
                                                            senderWriter.Write(guy1.account_no);
                                                            senderWriter.Write(guy1.pass);
                                                            senderWriter.Write(guy1.balance);

                                                            Console.WriteLine($"Your current balance is {guy1.balance:C2}");
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine($"Insufficient balance to send {deposit:C2}! Your balance is {guy1.balance:C2}");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Account does not exist.");
                                                }
                                            }
                                            break;
                                case 4:
                                    Console.WriteLine($"Your current account balance is {guy1.balance:C2} as at {currenttime}");
                                    break;
                                case 5:
                                    Console.Clear();
                                    Console.Write("Enter current password:");
                                    pass2 = Console.ReadLine();

                                    if (pass2 == guy1.pass)
                                    {
                                        Console.Write("Enter new password:");
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

                                            Console.WriteLine($"Password reset successful. New password is {guy1.pass}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong password");
                                    }
                                    break;
                                case 6:
                                    Console.WriteLine("Logging out.....");
                                    break;
                                default:
                                    Console.WriteLine("Invalid option!!!");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong password.");
                    }
                }
                else
                {
                    Console.WriteLine("Account does not exist.");
                }
            }

            if (choice == 3)
            {
                Console.WriteLine("Thank you for banking with us!");
            }
        }
    }

    static void menu()
    {
        Console.WriteLine("Enter 1 to create account.");
        Console.WriteLine("Enter 2 to log in.");
        Console.WriteLine("Enter 3 to exit.");
    }

    static void menu2()
    {
        Console.WriteLine("Enter 1 to deposit.");
        Console.WriteLine("Enter 2 to withdraw.");
        Console.WriteLine("Enter 3 to send money.");
        Console.WriteLine("Enter 4 to check balance");
        Console.WriteLine("Enter 5 to change password");
        Console.WriteLine("Enter 6 to log out");
    }
}
