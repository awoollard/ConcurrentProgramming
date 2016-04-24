namespace Bank
{
    /// <summary>
    /// The Bank program. Withdrawals and deposits are made into an account with/without locking.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for the Bank program.
        /// </summary>
        /// <param name="args">Arguments to the Bank program. Currently unused.</param>
        public static void Main(string[] args)
        {
            var accountTest = new AccountTest();
            accountTest.Run();
        }
    }
}
