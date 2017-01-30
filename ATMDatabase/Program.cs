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

        static void Main(string[] args)
        {
            using (var db = new ATMContext())
            {
                WelcomeScreen(db);
            }
        }

        private static void MakeWithdrawal(User userinstance)
        {
            using (var db = new ATMContext())
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

        private static void MakeDeposit(User userinstance)
        {
            using (var db = new ATMContext())
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
            };
           
        }

        private static void GetBalance(User userinstance)
        {
            Console.WriteLine($"{userinstance.Balance}");
        }

        private static void SignIn(ATMContext db)
        {
            Console.WriteLine("Please enter your user name.");

            var idchoice = (Read("> "));
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
                        GetBalance(userinstance);
                        break;
                    case 2:
                        MakeDeposit(userinstance);
                        break;
                    case 3:
                        MakeWithdrawal(userinstance);
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

        private static void CreateUser(ATMContext db)
        {
            var allusers = db.Users.Count();
            Console.WriteLine($"{allusers} users in database.");

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
        }

        static string Read(string input)
        {
            Console.WriteLine(input);
            return Console.ReadLine();
        }
    }
}
