using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Transactions;

namespace Bank_AMS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Check if files exist or create them if they don't
            fileCheck(@"firstnames.txt");
            fileCheck(@"lastnames.txt");
            fileCheck(@"passwords.txt");
            fileCheck(@"accountNumbers.txt");
            fileCheck(@"accountBalances.txt");
            fileCheck(@"passwordResets.txt");

            bool run = true;
            do
            {
                int trialCount = 3;
                int sign_in_trials = 3;
                int sign_in_count = 0;
                int which_user = 0;
                int menu;
                int same = 0;
                int foundpassword = 0;
                int found_account_no = 0;
                int page = 0;
                bool running = true;
                bool valid = true;
                do
                {
                    //Getting data from lists
                    List<string> firstname = new List<string>(File.ReadAllLines(@"firstnames.txt"));
                    List<string> lastname = new List<string>(File.ReadAllLines(@"lastnames.txt"));
                    List<string> password = new List<string>(File.ReadAllLines(@"passwords.txt"));
                    List<string> accountNumber = new List<string>(File.ReadAllLines(@"accountNumbers.txt"));
                    List<string> accountBalance = new List<string>(File.ReadAllLines(@"accountBalances.txt"));
                    List<string> passwordReset = new List<string>(File.ReadAllLines(@"passwordResets.txt"));
                    int who_is_signing_up = firstname.Count;

                    // Home page
                    if (page == 0)
                    {
                        page = Home();
                    }
                    //Registration page
                    if (page == 1)
                    {
                        Console.WriteLine("\n\t\t----------------");
                        Console.WriteLine("\t\tAccount Creation");
                        Console.WriteLine("\t\t----------------\n");
                        Console.WriteLine("\t\t(Required) Fill in the following form!");
                        string name1;
                        string name2;
                        do
                        {
                            Console.Write("\n\tInput your first name: ");
                            name1 = Console.ReadLine();
                            Console.Write("\tInput your last name: ");
                            name2 = Console.ReadLine();
                            if (name1.Length < 3 || name2.Length < 3)
                            {
                                Console.WriteLine("\n\tName should be at least 3 characters long!!\n");
                            }
                        } while (name1.Length < 3 || name2.Length < 3);

                        // add values to the lists
                        firstname.Add(name1.ToLower());
                        lastname.Add(name2.ToLower());

                        //Get password
                        string key;
                        string key2;
                        bool keyCheck = true;
                        int trial_count = 3;
                        do
                        {
                            if (trial_count > 0)
                            {
                                Console.WriteLine($"\nYou have {trial_count} trials left!\n");
                                key = GetPassword();
                                key2 = ConfirmPassword();
                                if (key.Length < 8 || key2.Length < 8)
                                {
                                    Console.WriteLine("\n\tpassword must be at least 8 characters long!\n");
                                    trial_count--;
                                }
                                else if (key != key2)
                                {
                                    Console.WriteLine("\n\tThe Passwords do not match!!\n");
                                    trial_count--;
                                }
                                else
                                {
                                    for (int i = 1; i < firstname.Count; i++)
                                    {
                                        if (firstname.Count > i)
                                        {
                                            if ($"{name1}{name2}" == $"{firstname[i - 1]}{lastname[i - 1]}" && $"{key2}" == $"{password[i - 1]}")
                                            {
                                                Console.WriteLine("\n\t\tThis account already exists!!");
                                                done();
                                                valid = false;
                                                break;
                                            }
                                        }
                                    }
                                    Console.Write("\n\ninput a password reset hint! (For when you forget your password!) \n\t>>");
                                    string reset = Console.ReadLine();
                                    passHint(passwordReset, reset);
                                    password.Add(key2);
                                    keyCheck = false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nYou have made three invalid attempts! Return to Home page!\n");
                                valid = false;
                                done();
                                keyCheck = false;
                            }
                        } while (keyCheck);

                        if (valid)
                        {
                            // Generating Account Number and Account Balance
                            bool accountNumberChecker = true;
                            int number;
                            do
                            {
                                int numcheck = 0;
                                Random random = new Random();
                                number = random.Next(135798642, 246897531) + random.Next(135798642, 246897531);

                                if (accountNumber.Count > 0)
                                {
                                    for (int i = 0; i < accountNumber.Count; i++)
                                    {
                                        try
                                        {
                                            if (Convert.ToInt64(accountNumber[i]) == number)
                                            {
                                                numcheck++;
                                            }
                                        }
                                        catch (FormatException ex)
                                        {
                                            accountNumber.Add(Convert.ToString(number));
                                            accountNumberChecker = false;
                                        }
                                    }
                                    if (numcheck == 0)
                                    {
                                        accountNumber.Add(Convert.ToString(number));
                                        accountNumberChecker = false;
                                    }
                                }
                                else
                                {
                                    accountNumber.Add(Convert.ToString(number));
                                    accountNumberChecker = false;
                                }
                            } while (accountNumberChecker);

                            // initiate account balance to 0
                            accountBalance.Add("0");

                            // Update files
                            updateData(firstname, lastname, password, accountBalance, accountNumber, passwordReset);
                            Console.Clear();
                            Console.WriteLine("\n\tAccount registration successful!\n\n");
                            Console.WriteLine("\n\tYour username is " + firstname[who_is_signing_up] + lastname[who_is_signing_up]);
                            Console.WriteLine($"\tYour Account number is {accountNumber[who_is_signing_up]}");
                            Console.WriteLine($"\tYour Account balance is {accountBalance[who_is_signing_up]}. Log in and deposit to change this!");
                            page++;
                        }
                        else
                        {
                            Console.Clear();
                            running = false;
                        }
                    }
                    // Login Page
                    if (page == 2)
                    {
                        which_user = 0;
                        Console.WriteLine("\n\t\t-------------");
                        Console.WriteLine("\t\tAccount Log in");
                        Console.WriteLine("\t\t---------------\n");

                        // user data
                        Console.WriteLine($"\t\tYou have {sign_in_trials} attempts left\n");
                        Console.Write("\t\tinput your username or account number: ");
                        string sign_in_account_no = Console.ReadLine();
                        string sign_in_password = GetPassword();

                        for (int i = 0; i < firstname.Count; i++)
                        {
                            if ((sign_in_account_no == accountNumber[i] || sign_in_account_no == $"{firstname[i]}{lastname[i]}"))
                            {
                                found_account_no = 1;
                                which_user = i;
                                same = 1;
                            }
                            if (sign_in_password == password[i] && same == 1)
                            {
                                foundpassword = 1;
                                which_user = i;
                                break;
                            }
                            if (sign_in_password != password[i] && same == 1)
                            {
                                same = 0;
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (foundpassword == 0 || found_account_no == 0)
                        {
                            Console.Clear();
                            Console.Write("\n\t\tinvalid account_number or password!!\n");
                            sign_in_trials--;
                            sign_in_count++;
                            trialCount--;

                            if (sign_in_count < 3 && sign_in_trials > 0)
                            {
                                string sign_in_fail = "";

                                while (sign_in_fail.ToLower() != "t" && sign_in_fail.ToLower() != "f")
                                {
                                    Console.WriteLine("\n\t\tEnter T to Try again!");
                                    Console.Write("\t\tEnter F for forgot password!   >");
                                    sign_in_fail = Console.ReadLine();

                                    if (sign_in_fail.ToLower() == "t")
                                    {
                                        Console.Clear();
                                        if (trialCount == 0)
                                        {
                                            trialCount = 3;
                                            sign_in_count = 0;
                                            sign_in_trials = 3;
                                        }
                                        page = 2;
                                        break;
                                    }
                                    else if (sign_in_fail.ToLower() == "f")
                                    {
                                        Console.Clear();
                                        if ((found_account_no == 1 && foundpassword == 0))
                                        {
                                            Console.WriteLine($"hint: {passwordReset[which_user]}");
                                            done();
                                        }
                                        else if ((found_account_no == 0 && foundpassword == 1) || (found_account_no == 0 && foundpassword == 0))
                                        {
                                            Console.Clear();
                                            Console.WriteLine($"\nThe account {sign_in_account_no} doesn't exist!!\n");
                                            done();
                                        }
                                        running = false;
                                        break;
                                    }
                                }
                            }
                            if (sign_in_count == 3 && sign_in_trials == 0)
                            {
                                Console.Clear();
                                Console.WriteLine("\t\tYou have made 3 incorrect inputs. Returning to main menu");
                                done();
                                running = false;
                            }
                        }
                        else if (foundpassword == 1 && found_account_no == 1)
                        {
                            Console.Clear();
                            page = 3;
                        }
                    }
                    // Menu page and after login pages
                    if (page == 3)
                    {
                        menu = Menu(firstname, lastname, accountNumber, which_user);

                        // User choices
                        switch (menu)
                        {
                            case 1:
                                Console.Clear();
                                Console.WriteLine("\n\t\t-----------------");
                                Console.WriteLine("\t\tMoney Transactions");
                                Console.WriteLine("\t\t------------------\n");

                                int choice = 0;

                                try
                                {
                                    Console.WriteLine("Transactions Options: ");
                                    Console.WriteLine("1. Deposit funds");
                                    Console.WriteLine("2. Withdraw funds");
                                    Console.WriteLine("3. Transfer funds to another account");
                                    Console.WriteLine("4. Menu Page");
                                    Console.Write("Choose 1,2,3 or 4: ");
                                    choice = Convert.ToInt32(Console.ReadLine());

                                    if (choice != 1 && choice != 2 && choice != 3 && choice != 4)
                                    {
                                        Console.WriteLine("\t\t\nWrong Input made!\n");
                                    }
                                }
                                catch (FormatException e)
                                {
                                    Console.WriteLine("Wrong format used!\n");
                                }

                                if (choice == 1)
                                {
                                    double amount = 0.0;
                                    bool deposited = true;

                                    do
                                    {
                                        bool wow = true;
                                        try
                                        {
                                            Console.Write("\n\t\tInput amount you wish to deposit_ ");
                                            amount = Convert.ToDouble(Console.ReadLine());
                                        }
                                        catch (FormatException ex)
                                        {
                                            Console.WriteLine("Wrong format used!\n");
                                            wow = false;
                                            done();
                                        }

                                        if (wow)
                                        {
                                            if (amount < 100)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Deposit is less than the minimum deposit amount of 100\n");
                                            }
                                            else if (amount > 100)
                                            {
                                                Console.Clear();
                                                double balance = Convert.ToDouble(accountBalance[which_user]);
                                                balance += amount;
                                                accountBalance.Remove(accountBalance[which_user]);
                                                accountBalance.Insert(which_user, Convert.ToString(balance));
                                                updateData(firstname, lastname, password, accountBalance, accountNumber, passwordReset);
                                                Console.WriteLine($"Deposit successful. New balance is {accountBalance[which_user]}\n");
                                                done();
                                                deposited = false;
                                            }
                                        }
                                    } while (deposited);
                                }
                                if (choice == 2)
                                {
                                    Console.WriteLine("\n\t\t---------");
                                    Console.WriteLine("\t\tWithdrawal");
                                    Console.WriteLine("\t\t----------\n");
                                    double amount = 0.0;
                                    bool withdrawal = true;

                                    do
                                    {
                                        try
                                        {
                                            Console.Write("\n\t\tInput amount you wish to withdraw_ ");
                                            amount = Convert.ToDouble(Console.ReadLine());
                                        }
                                        catch (FormatException e)
                                        {
                                            Console.WriteLine("Wrong format used!\n");
                                            done();
                                        }

                                        if (amount < 100)
                                        {
                                            Console.WriteLine("Withdrawal is less than the minimum withdrawal amount of 100\n");
                                        }
                                        else if (amount > Convert.ToDouble(accountBalance[which_user]))
                                        {
                                            Console.WriteLine($"\nAmount specified is greater than your account balance of {accountBalance[which_user]}\n");
                                            done();
                                            withdrawal = false;
                                        }
                                        else
                                        {
                                            double balance = Convert.ToDouble(accountBalance[which_user]);
                                            balance -= amount;
                                            accountBalance.Remove(accountBalance[which_user]);
                                            accountBalance.Insert(which_user, Convert.ToString(balance));
                                            updateData(firstname, lastname, password, accountBalance, accountNumber, passwordReset);
                                            Console.WriteLine($"withdrawal successful. New balance is {accountBalance[which_user]}\n");
                                            done();
                                            withdrawal = false;
                                        }
                                    } while (withdrawal);
                                }

                                if (choice == 3)
                                {
                                    Console.WriteLine("\n\t\t-------------");
                                    Console.WriteLine("\t\tMoney Tranfer");
                                    Console.WriteLine("\t\t-------------\n");
                                    int transfer;
                                    bool proceed = true;
                                    try
                                    {
                                        Console.Write("Input the account number to transfer funds to: ");
                                        transfer = Convert.ToInt32(Console.ReadLine());
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.WriteLine("Wrong format used!!");
                                        proceed = false;
                                        done();
                                        break;
                                    }

                                    int lookfor;
                                    if (accountNumber.Count > 0)
                                    {
                                        try
                                        {
                                            lookfor = accountNumber.IndexOf(transfer.ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("\n\t\tThe account number entered doesn't exist in this bank!!");
                                            proceed = false;
                                            done();
                                            break;
                                        }

                                        if (proceed)
                                        {
                                            int transferAmount = 0;
                                            try
                                            {
                                                Console.Write("\n\tEnter amount you wish to transfer: ");
                                                transferAmount = Convert.ToInt32(Console.ReadLine());
                                            }
                                            catch (FormatException ex)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("\n\t\tWrong format used!!");
                                                done();
                                                break;
                                            }

                                            if (transferAmount > Convert.ToInt32(accountBalance[which_user]))
                                            {
                                                Console.Write("\n\tError! Amount entered is greater than your Account Balance!!");
                                                done();
                                            }
                                            else
                                            {
                                                int recipient = Convert.ToInt32(accountBalance[lookfor]) + transferAmount;
                                                int senderOfFunds = Convert.ToInt32(accountBalance[which_user]) - transferAmount;
                                                accountBalance.Remove(accountBalance[lookfor]);
                                                accountBalance.Insert(lookfor, Convert.ToString(recipient));
                                                accountBalance.Remove(accountBalance[which_user]);
                                                accountBalance.Insert(which_user, Convert.ToString(senderOfFunds));
                                                updateData(firstname, lastname, password, accountBalance, accountNumber, passwordReset);
                                                Console.WriteLine($"\n\t\tTransfer successful. New account balance is {accountBalance[which_user]}");
                                                done();
                                                break;
                                            }
                                        }

                                    }

                                }

                                if (choice == 4)
                                {
                                    Console.Clear();
                                    break;
                                }
                                break;
                            case 2:
                                bool balanceInquiry = true;
                                Console.Clear();
                                Console.WriteLine("\n\t\t-----------------");
                                Console.WriteLine("\t\tBalance Inquiry");
                                Console.WriteLine("\t\t------------------\n");
                                do
                                {
                                    Console.WriteLine("\tCheck balance?");
                                    Console.Write("\tY(yes), M(menu)");
                                    string choice1 = Console.ReadLine();

                                    if (choice1.ToLower() == "y" || choice1.ToLower() == "yes")
                                    {
                                        checkBalance(which_user, password, accountBalance);
                                        balanceInquiry = false;
                                    }
                                    else if (choice1.ToLower() == "m" || choice1.ToLower() == "menu")
                                    {
                                        Console.Clear();
                                        balanceInquiry = false;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nWrong choice made!!\n");
                                    }
                                } while (balanceInquiry);
                                break;
                            case 3:
                                int choice2 = 0;
                                Console.Clear();
                                Console.WriteLine("\n\t\t-----------------");
                                Console.WriteLine("\t\tAccount Information");
                                Console.WriteLine("\t\t------------------\n");

                                while (choice2 != 1 && choice2 != 2 && choice2 != 3)
                                {
                                    try
                                    {
                                        Console.WriteLine("Options: ");
                                        Console.WriteLine("\t\t1.Delete Account");
                                        Console.WriteLine("\t\t2.Change Password");
                                        Console.Write("\t\t3.Menu   >");
                                        choice2 = Convert.ToInt32(Console.ReadLine());
                                    }
                                    catch (FormatException e)
                                    {
                                        Console.WriteLine("\t\tChoose option 1,2 or 3!\n");
                                    }
                                }

                                if (choice2 == 1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Are you sure you want to delete your Account?");
                                    Console.Write("Y/N (Y for YES and N for NO)   >");
                                    string sure = Console.ReadLine();

                                    switch (sure.ToLower())
                                    {
                                        case "y":
                                            Console.Clear();
                                            string del = GetPassword();

                                            if (del == password[which_user])
                                            {
                                                Console.WriteLine("\n\tAccount deletion successful!");
                                                Console.WriteLine("\n\tYou will be missed!");
                                                done();
                                                firstname.Remove(firstname[which_user]);
                                                lastname.Remove(lastname[which_user]);
                                                password.Remove(password[which_user]);
                                                accountNumber.Remove(accountNumber[which_user]);
                                                accountBalance.Remove(accountBalance[which_user]);

                                                updateData(firstname, lastname, password, accountBalance, accountNumber, passwordReset);
                                                running = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Wrong password!!\n");
                                                done();
                                            }
                                            break;
                                        case "n":
                                            Console.Clear();
                                            Console.WriteLine("Thank you for choosing to continue banking with us\n");
                                            break;
                                    }
                                }
                                else if (choice2 == 2)
                                {
                                    int num1 = 0;
                                    string pass = "";
                                    Console.Clear();

                                    try
                                    {
                                        Console.Write("\t\tinput your account number: ");
                                        num1 = Convert.ToInt32(Console.ReadLine());
                                        Console.Write("\n\t\tOld password: ");
                                        pass = GetPassword();

                                        if (pass != password[which_user] && num1 != Convert.ToInt32(accountNumber[which_user]))
                                        {
                                            Console.WriteLine("\nWrong account number or password!\n");
                                        }
                                    }
                                    catch (FormatException e)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nWrong format used!\n\n");
                                    }

                                    if (pass == password[which_user] && num1 == Convert.ToInt32(accountNumber[which_user]))
                                    {
                                        Console.Clear();
                                        Console.Write("\n\tNew password: ");
                                        string pass_new = GetPassword();
                                        string confirm_pass_new = ConfirmPassword();

                                        if (pass_new == confirm_pass_new)
                                        {
                                            password.Remove(password[which_user]);
                                            password.Insert(which_user, pass_new);
                                            Console.Clear();
                                            Console.WriteLine("password reset successfully!!\n");
                                            updateData(firstname, lastname, password, accountBalance, accountNumber, passwordReset);
                                            page = 2;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\n\t\t\tThe passwords do not match!!\n");
                                            done();
                                        }
                                    }

                                }
                                else if (choice2 == 3)
                                {
                                    Console.Clear();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("invalid choice made!!");
                                }
                                break;
                            case 4:
                                string logout = "";
                                Console.Clear();
                                Console.WriteLine("\t\t---------------");
                                Console.WriteLine("\t\t    Log out");
                                Console.WriteLine("\t\t---------------");

                                while (logout.ToLower() != "yes" && logout.ToLower() != "y" && logout != "no" && logout.ToLower() != "n")
                                {
                                    Console.WriteLine("You will be signed out, Proceed? (yes or no)");
                                    logout = Console.ReadLine();
                                }

                                if (logout.ToLower() == "yes" || logout.ToLower() == "y")
                                {
                                    running = false;
                                }
                                else if (logout.ToLower() == "no" || logout.ToLower() == "n")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid choice made!");
                                }
                                break;
                        }
                    }
                    // Quit bank application
                    if (page == 9)
                    {
                        string quit = "";
                        while (quit.ToLower() != "yes" && quit.ToLower() != "y" && quit.ToLower() != "no" && quit.ToLower() != "n")
                        {
                            Console.Clear();
                            Console.WriteLine("Input Yes or no");
                            Console.Write("Are you sure you want to exit? (yes / no)  >");
                            quit = Console.ReadLine();

                            if (quit.ToLower() == "yes" || quit.ToLower() == "y")
                            {
                                Console.WriteLine("Exit Successful!!");
                                done();
                                running = false;
                                run = false;
                            }
                            else if (quit.ToLower() == "no" || quit.ToLower() == "n")
                            {
                                running = false;
                                break;
                            }
                        }
                    }
                } while (running);
            } while (run);
        }
        public static int Home()
        {
            int page = 0;

            Console.Clear();
            // User interface
            Console.WriteLine("\t\t\t------------------------------------");
            Console.WriteLine("\t\t\tCodex Bank Account Management system");
            Console.WriteLine("\t\t\t------------------------------------");

            do
            {
                Console.WriteLine("Welcome!\n");
                Console.WriteLine("\t1. Create an Account");
                Console.WriteLine("\t2. Login into an existing account!");
                Console.WriteLine("\t9. Quit\n");
                Console.Write("\tChoose 1,2 or 9: ");

                try
                {
                    page = Convert.ToInt32(Console.ReadLine());
                    if (page != 1 && page != 2 && page != 9)
                    {
                        Console.WriteLine("Wrong input made!\n");
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Wrong input made!\n");
                }
            } while (page != 1 && page != 2 && page != 9);
            return page;
        }
        public static int Menu(List<string> firstname, List<string> lastname, List<string> accountNumber, int which_user)
        {
            int menu = 0;
            // implement menu
            Console.WriteLine("\n\n\t\tSIGNED IN\n");
            Console.WriteLine("\t\t----------------------------");
            Console.WriteLine($"\t\tAccount Name: {firstname[which_user] + " " + lastname[which_user]}");
            Console.WriteLine($"\t\tAccount Number: {accountNumber[which_user]}");
            Console.WriteLine("\t\t----------------------------\n");

            Console.WriteLine("\n\t\t\t\tMenu options");
            Console.WriteLine("\t\t1. Money Transactions");
            Console.WriteLine("\t\t2. Balance Inquiry");
            Console.WriteLine("\t\t3. Account Information");
            Console.Write("\t\t4. Log out\t\t>: ");
            try
            {
                menu = Convert.ToInt32(Console.ReadLine());

                if (menu != 1 && menu != 2 && menu != 3 && menu != 4)
                {
                    Console.WriteLine("\t\t\nWrong input made\n");
                }
            }
            catch (FormatException e)
            {
                Console.Clear();
                Console.WriteLine("\t\t\nChoose 1, 2, 3 or 4\n\n");
                menu = Menu(firstname, lastname, accountNumber, which_user);
            }
            return menu;
        }
        public static void updateData(List<string> firstname, List<string> lastname, List<string> password, List<string> accountBalance, List<string> accountNumber, List<string> passwordReset)
        {
            File.WriteAllLines(@"firstnames.txt", firstname);
            File.WriteAllLines(@"lastnames.txt", lastname);
            File.WriteAllLines(@"passwords.txt", password);
            File.WriteAllLines(@"accountBalances.txt", accountBalance);
            File.WriteAllLines(@"accountNumbers.txt", accountNumber);
            File.WriteAllLines(@"passwordResets.txt", passwordReset);
        }
        public static void checkBalance(int which_user, List<string> password, List<string> accountBalance)
        {
            Console.WriteLine("Check Balance!");
            string pass = GetPassword();

            if (pass == password[which_user])
            {
                Console.Clear();
                Console.WriteLine($"\n\tYour account balance is {accountBalance[which_user]}\n");
                Console.WriteLine("Press Enter to continue!");
                Console.ReadLine();
            }
            else
            {
                string try_again = "";

                Console.WriteLine("\n\tThat password is incorrect!!");
                Console.Write("\nTry again? (yes or No)  >");
                try_again = Console.ReadLine();

                if (try_again.ToLower() == "yes" || try_again.ToLower() == "y")
                {
                    checkBalance(which_user, password, accountBalance);
                }
                else if (try_again.ToLower() == "no" || try_again.ToLower() == "n")
                {
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("wrong input made");
                }
            }
        }
        public static void done()
        {
            try
            {
                Console.WriteLine("press enter to continue");
                Console.ReadLine();
                Console.Clear();
            }
            catch (FormatException e)
            {
                Console.Clear();
                done();
            }
        }
        public static void passHint(List<string> passwordReset, string reset)
        {
            passwordReset.Add(reset);
            Console.WriteLine("hint saved successfully!\n");
            done();
        }
        public static void fileCheck(string filename)
        {
            if (File.Exists(filename))
            {
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("\t\t\t\tstart of list");
                }
            }
        }
        public static string GetPassword()
        {
            Console.Write("\n\t\tInput password: ");
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
            return password.ToString();
        }
        public static string ConfirmPassword()
        {
            Console.Write("\n\t\tRe-enter password: ");
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
            return password.ToString();
        }
    }
}