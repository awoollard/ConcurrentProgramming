namespace Bank
{
    /// <summary>
    /// Account class representing an account in a bank. Uses locks.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The balance of the account
        /// </summary>
        public decimal Balance { get; private set; }
        private readonly object _accountLock;

        /// <summary>
        /// Initialises the account with a balance of zero.
        /// </summary>
        public Account()
        {
            _accountLock = new object();
            Balance = 0;
        }

        /// <summary>
        /// Deposits an amount of money into the account
        /// </summary>
        /// <param name="amount">Amount of money to deposit</param>
        public void Deposit(decimal amount)
        {
            lock (_accountLock)
            {
                Balance += amount;
            }
            
        }

        /// <summary>
        /// Withdraws an amount of money from the account
        /// </summary>
        /// <param name="amount">Amount of money to withdraw</param>
        public decimal Withdraw(decimal amount)
        {
            lock (_accountLock)
            {
                Balance -= amount;
                return amount;
            }
        }
    }
}
