namespace Bank
{
    /// <summary>
    /// Account class representing an account in a bank. Does not use locks.
    /// </summary>
    public class BrokenAccount
    {
        /// <summary>
        /// The balance of the account
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Initialises the account with a balance of zero.
        /// </summary>
        public BrokenAccount()
        {
            Balance = 0;
        }

        /// <summary>
        /// Deposits an amount of money into the account
        /// </summary>
        /// <param name="amount">Amount of money to deposit</param>
        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        /// <summary>
        /// Withdraws an amount of money from the account
        /// </summary>
        /// <param name="amount">Amount of money to withdraw</param>
        /// <returns></returns>
        public decimal Withdraw(decimal amount)
        {
            Balance -= amount;
            return amount;
        }
    }
}
