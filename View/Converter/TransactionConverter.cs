﻿using Bank.Model;
using Bank.View.Model;
using System.Collections.Generic;

namespace Bank.View.Converter
{
    class TransactionConverter : AbstractConverter
    {
        public static TransactionView ConvertTransactionToTransactionView(Transaction transaction)
           => new TransactionView
           {
               Date = transaction.Date,
               Purpose = transaction.Purpose,
               Payer = transaction.Payer.FirstName + " " + transaction.Payer.LastName,
               PayerAccount = transaction.Payer.Account.Number.Value,
               Receiver = transaction.Receiver.FirstName + " " + transaction.Receiver.LastName,
               ReceiverAccount = transaction.Receiver.Account.Number.Value,
               Amount = transaction.Amount.Value,
           };


        public static IList<TransactionView> ConvertTransactionListToTransactionViewList(IList<Transaction> transactions)
            => ConvertEntityListToViewList(transactions, ConvertTransactionToTransactionView);
    }
}
