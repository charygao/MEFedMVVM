using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVMSample.Models;

namespace MEFedMVVMSample.Services
{
    public interface IContactsDataService
    {
        void GetContacts(Action<IEnumerable<Contact>> contactsArrived);
        void GetContactsByName(string name, Action<IEnumerable<Contact>> contactsArrived);
    }
}
