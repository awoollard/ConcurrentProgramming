using System;
using System.Threading;

namespace Bank
{
    /// <summary>
    /// Tests for the Account class.
    /// </summary>
    public class AccountTest
    {
        private readonly Account _account;
        private readonly Thread _depositThread;
        private readonly Thread _withdrawThread;

        /// <summary>
        /// Initialises the account and threads used for the test.
        /// </summary>
        public AccountTest()
        {
            _account = new Account();
            _depositThread = new Thread(DepositOneMillion);
            _withdrawThread = new Thread(WithdrawOneMillion);
        }

        /// <summary>
        /// Runs the tests.
        /// </summary>
        public void Run()
        {
            _depositThread.Start();
            _withdrawThread.Start();

            _depositThread.Join();
            _withdrawThread.Join();

            Console.WriteLine("Account balance: " + _account.Balance);
        }

        /// <summary>
        /// Deposits one million into the Account in individual transactions of one unit.
        /// </summary>
        private void DepositOneMillion()
        {
            for (var i = 0; i < 1000000; i++)
            {
                _account.Deposit(1);
            }
        }

        /// <summary>
        /// Withdraws one million from the Account in individual transactions of one unit.
        /// </summary>
        private void WithdrawOneMillion()
        {
            for (var i = 0; i < 1000000; i++)
            {
                _account.Withdraw(1);
            }
        }
    }
}
