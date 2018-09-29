using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVM.ViewModelLocator;
using System.Threading;
using MEFedMVVMSample.Models;

namespace MEFedMVVMSample.Services.DesignTimeData
{
    /// <summary>
    /// This is the service that returns design time data to blend
    /// </summary>
    [ExportService(ServiceType.DesignTime, typeof(IContactsDataService))]
    public class DesignTimeContactsDataService : IContactsDataService
    {
        public void GetContacts(Action<IEnumerable<Models.Contact>> contactsArrived)
        {
            contactsArrived(new List<Contact>
                {
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                    new Contact { Name = "Lorem", Surname = "ipsum", EMail="laremipsum@blend.com" },
                });

        }
        
        public void GetContactsByName(string name, Action<IEnumerable<Contact>> contactsArrived)
        {
            // at design time this will never be called thus do nothing 
        }
    }
}
