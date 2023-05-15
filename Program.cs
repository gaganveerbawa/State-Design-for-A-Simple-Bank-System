using System;

namespace BankSystem
{
    public class BankAccount
    {
        private AccountState state;
        public decimal Balance { get; private set; }

        public BankAccount()
        {
            state = new OpenState(this);
            Balance = 0;
        }

        public void SetState(AccountState state)
        {
            this.state = state;
        }

        public void Withdraw(decimal amount)
        {
            state.Withdraw(amount);
        }

        public void Deposit(decimal amount)
        {
            state.Deposit(amount);
        }

        public decimal GetBalance()
        {
            return Balance;
        }

        // Abstract state class
        public abstract class AccountState
        {
            protected BankAccount account;

            protected AccountState(BankAccount account)
            {
                this.account = account;
            }

            public abstract void Withdraw(decimal amount);
            public abstract void Deposit(decimal amount);
        }

        // Concrete state: Open state
        public class OpenState : AccountState
        {
            public OpenState(BankAccount account) : base(account)
            {
            }

            public override void Withdraw(decimal amount)
            {
                if (account.Balance >= amount && amount >= 0)
                {
                    account.Balance -= amount;
                    Console.WriteLine($"Withdrawn: ${amount}. New balance: ${account.Balance}");
                }                
                else if (amount < 0) 
                {
                    Console.WriteLine("Cannot withdraw negative amount");
                }
                else
                {
                    Console.WriteLine("Insufficient funds.");
                }
            }

            public override void Deposit(decimal amount)
            {
                if (amount < 0)
                {
                    Console.WriteLine("Cannot deposit negative amount");
                }
                else
                {
                    account.Balance += amount;
                    Console.WriteLine($"Deposited: ${amount}. New balance: ${account.Balance}");
                }
            }
        }

        // Concrete state: Closed state
        public class ClosedState : AccountState
        {
            public ClosedState(BankAccount account) : base(account)
            {
            }

            public override void Withdraw(decimal amount)
            {
                Console.WriteLine("Cannot withdraw from a closed account.");
            }

            public override void Deposit(decimal amount)
            {
                Console.WriteLine("Cannot deposit to a closed account.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BankAccount account = new BankAccount();
            account.Deposit(100);
            account.Deposit(-100);
            account.Withdraw(50);
            account.Withdraw(80);
            account.Withdraw(-40);

            // Change state to ClosedState
            account.SetState(new BankAccount.ClosedState(account));
            account.Withdraw(20);
            account.Deposit(10);

            Console.ReadLine();
        }
    }
}