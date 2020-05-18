using Bank.Repository.Abstract;
using System;

namespace Bank.Model
{
    public class Client : IIdentifiable<long>
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Account Account { get; set; }

        public Client(long id)
        {
            Id = id;
        }

        public Client(long id, string firstName, string lastName, DateTime dateOfBirth, Account account)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Account = account;
        }

        public long GetId() => Id;

        public void SetId(long id) => Id = id;
    }
}
