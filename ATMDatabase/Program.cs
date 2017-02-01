using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMDatabase
{
    class Program
    {
        public static bool loggedIn = false;
        public static List<string> userlist = new List<string>();

        static void Main(string[] args)
        {
            using (var db = new ATMContext())
            {
                WelcomeScreen(db);
            }
        }

        private static void MakeWithdrawal(User userinstance, ATMContext db)
        {
       
            
                double amount = double.Parse(Read("How much would you like to withdraw?"));

                if (amount < userinstance.Balance)
                {
                    double newbalance = userinstance.Balance - amount;

                    userinstance.Balance = newbalance;
                    db.Entry(userinstance).State = System.Data.Entity.EntityState.Modified;

                    var newWithdrawal = new Withdrawal
                    {
                        Amount = amount,
                        Time = DateTime.Now
                    };

                    db.Withdrawals.Add(newWithdrawal);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Sorry, you do not have enough in your account!");
                    Environment.Exit(0);
                }
                Console.WriteLine("Would you like to make another withdrawal? [Y/N]");
                char selection = char.ToUpper(char.Parse(Read("> ")));
                if (selection == 'Y')
                {
                    MakeWithdrawal(userinstance, db);
                }
                else
                {
                    OptionScreen(userinstance, db);
                }
            
        }

        private static void WelcomeScreen(ATMContext db)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine(" 1. Create A User");
            Console.WriteLine(" 2. Sign In");
            Console.WriteLine(" 3. Exit");
            int choice = int.Parse(Read("> "));

            switch (choice)
            {
                case 1:
                    CreateUser(db);
                    break;
                case 2:
                    SignIn(db);
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private static void MakeDeposit(User userinstance, ATMContext db)
        {
            
            
                double amount = double.Parse(Read("How much would you like to deposit?"));
                double newbalance = amount + userinstance.Balance;

                userinstance.Balance = newbalance;
                db.Entry(userinstance).State = System.Data.Entity.EntityState.Modified;

                var newDeposit = new Deposit
                {
                    Amount = amount,
                    Time = DateTime.Now
                };

                db.Deposits.Add(newDeposit);
                db.SaveChanges();
               
                Console.WriteLine("Would you like to make another deposit? (Y/N)");
                char selection = char.ToUpper(char.Parse(Read("> ")));
                if(selection == 'Y')
                {
                    MakeDeposit(userinstance, db);
                }
                else
                {
                    OptionScreen(userinstance, db);
                }
                db.SaveChanges();
            

        }

        private static void GetBalance(User userinstance, ATMContext db)
        {
            
            
                Console.WriteLine($"{userinstance.Balance}");
                Console.WriteLine("Would you like to perform another action? [Y/N]");
                char selection = char.ToUpper(char.Parse(Read("> ")));
                if (selection == 'Y')
                {
                    Console.WriteLine("What would you like to do?");
                    Console.WriteLine(" 1. View Your Balance");
                    Console.WriteLine(" 2. Make a Deposit");
                    Console.WriteLine(" 3. Make a Withdrawal");
                    Console.WriteLine(" 4. Log Out");
                    int choice = int.Parse(Read("> "));

                    switch (choice)
                    {
                        case 1:
                            GetBalance(userinstance, db);
                            break;
                        case 2:
                            MakeDeposit(userinstance, db);
                            break;
                        case 3:
                            MakeWithdrawal(userinstance, db);
                            break;
                        case 4:
                            WelcomeScreen(db);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            
        }

        private static void SignIn(ATMContext db)
        {

            Console.WriteLine("Please enter your user name.");

            var idchoice = (Read("> "));
            foreach (User user in db.Users)
            {
                userlist.Add(user.Name);
            }
            if (userlist.Contains(idchoice))

            {
                var userinstance = db.Users.Where(u => u.Name == idchoice).First();
                Console.WriteLine("Please enter your password.");
                var userpass = Console.ReadLine();

                if (userpass == userinstance.Password)
                {
                    Console.WriteLine("Logged in!");
                    loggedIn = true;
                    Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("What would you like to do?");
                    Console.WriteLine(" 1. View Your Balance");
                    Console.WriteLine(" 2. Make a Deposit");
                    Console.WriteLine(" 3. Make a Withdrawal");
                    Console.WriteLine(" 4. Log Out");
                    int choice = int.Parse(Read("> "));

                    switch (choice)
                    {
                        case 1:
                            GetBalance(userinstance, db);
                            break;
                        case 2:
                            MakeDeposit(userinstance, db);
                            break;
                        case 3:
                            MakeWithdrawal(userinstance, db);
                            break;
                        case 4:
                            WelcomeScreen(db);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("That is incorrect.");
                }
            }
            else
            {
                Console.WriteLine("That user does not exist!");
                SignIn(db);
            }
        }

        private static void CreateUser(ATMContext db)
        {
            var name = Read("Name?");
            var address = Read("Address?");
            var password = Read("Password?");

            User newuser = new User
            {
                Name = name,
                Address = address,
                Password = password,
                Balance = 0
            };
            db.Users.Add(newuser);
            db.SaveChanges();
            Console.WriteLine("Would you like to create another user? [Y/N]");
            char selection = char.Parse(Read("> "));
            if (selection == 'Y')
            {
                CreateUser(db);
            }
            else
            {
                WelcomeScreen(db);
            }
            
            db.SaveChanges();
        }

        private static void OptionScreen(User userinstance, ATMContext db)
        {
            
            
                Console.WriteLine("What would you like to do?");
                Console.WriteLine(" 1. View Your Balance");
                Console.WriteLine(" 2. Make a Deposit");
                Console.WriteLine(" 3. Make a Withdrawal");
                Console.WriteLine(" 4. Log Out");
                int choice = int.Parse(Read("> "));

                switch (choice)
                {
                    case 1:
                        GetBalance(userinstance, db);
                        break;
                    case 2:
                        MakeDeposit(userinstance, db);
                        break;
                    case 3:
                        MakeWithdrawal(userinstance, db);
                        break;
                    case 4:
                        WelcomeScreen(db);
                        break;
                    default:
                        break;
                }
            
        }

        static string Read(string input)
        {
            Console.WriteLine(input);
            return Console.ReadLine();
        }
    }
}
