using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVM.ViewModelLocator;
using System.Threading;
using MEFedMVVMSample.Models;

namespace MEFedMVVMSample.Services
{
    /// <summary>
    /// This is the runtime service. This should connect to a DB/Webservice or what have you... for demo I am returning some static data
    /// </summary>
    [ExportService(ServiceType.Runtime, typeof(IContactsDataService))]
    public class ContactsDataService : IContactsDataService
    {
        static IList<Contact> myContactsDemoData = new List<Contact>
                {
                    new Contact { Name = "Marlon", Surname = "Grech", EMail="marlongrech@mvvm.com" },
                    new Contact { Name = "Laurent", Surname = "Bugnion", EMail="laurent@mvvm.com" },
                    new Contact { Name = "Josh", Surname = "smith", EMail="jsmith@mvvm.com" },
                    new Contact { Name = "Glenn", Surname = "Block", EMail="gblock@mef.com" },
                    new Contact { Name = "Sacha", Surname = "Barber", EMail="sbarber@mvvm.com" },
                };

        public void GetContacts(Action<IEnumerable<Models.Contact>> contactsArrived)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                //simulate some work in the background
                Thread.Sleep(TimeSpan.FromSeconds(2));

                contactsArrived( myContactsDemoData );
            });
        }

        public void GetContactsByName(string name, Action<IEnumerable<Contact>> contactsArrived)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                //simulate some work in the background
                Thread.Sleep(TimeSpan.FromSeconds(2));

                contactsArrived(myContactsDemoData.Where(x=>x.Name.Contains(name)));
            });
        }
    }
}
