using System;
using System.Collections.Generic;
using MEFedMVVM.Common;
using MEFedMVVMSample.Models;
using System.ComponentModel.Composition;
using MEFedMVVMSample.Services;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.ViewModelLocator;
using System.Windows.Input;

namespace MEFedMVVMSample.ViewModels
{
    [ExportViewModel("ContactListViewModel")]
    public class ContactListViewModel : NotifyPropertyChangedBase
    {
        IList<Contact> _contacts;
        private IContactsDataService _contactsDataService;
        private IVisualStateManager _stateManager;
        private IDispatcherService _dispatcherService;

        /// <summary>
        /// Command to apply a search
        /// </summary>
        public ICommand SearchCommand { get; private set; }

        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        public IList<Contact> Contacts
        {
            get { return _contacts; }
        }

        [ImportingConstructor]
        public ContactListViewModel(IContactsDataService contactsDataService, 
            IVisualStateManager stateManager, IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
            _stateManager = stateManager;
            _contactsDataService = contactsDataService;
            SearchCommand = new DelegateCommand<string>(Search, CanSearch);
            InitData();
        }

        private void InitData()
        {
            _contactsDataService.GetContacts(OnContactsArrived);
            _stateManager.GoToState("LoadingContacts");
        }

        private void OnContactsArrived(IEnumerable<Contact> contacts)
        {
            _contacts = new List<Contact>();
            foreach (var item in contacts)
            {
                _contacts.Add(item);
            }

            //raise the property changed on the UI thread
            _dispatcherService.BeginInvoke( (Action)delegate
                {
                    //notify ui that the contacts data arrived so binding updates
                    OnPropertyChanged(() => Contacts);
            
                    //update state
                    _stateManager.GoToState("ContactsLoaded");
                });
        }

        private bool CanSearch(string searchableName)
        {
            return !String.IsNullOrEmpty(searchableName);
        }

        private void Search(string searchableName)
        {
            _contactsDataService.GetContactsByName(searchableName, OnContactsArrived);
            _stateManager.GoToState("LoadingContacts");
        }
    }
}
