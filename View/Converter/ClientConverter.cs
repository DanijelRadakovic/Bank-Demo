using Bank.Model;
using System.Collections.Generic;

namespace Bank.View.Converter
{
    class ClientConverter : AbstractConverter
    {
        public static string ConvertClientToString(Client client)
            => string.Join(" ", client.FirstName, client.LastName);

        public static IList<string> ConvertClientListToStringList(IList<Client> clients)
            => ConvertEntityListToViewList(clients, ConvertClientToString);
    }
}
