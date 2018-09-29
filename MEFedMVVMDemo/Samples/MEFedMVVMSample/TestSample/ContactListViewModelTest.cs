using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using MEFedMVVMSample.Services;
using MEFedMVVM.Services.Contracts;
using MEFedMVVMSample.ViewModels;
using Rhino.Mocks.Constraints;
using MEFedMVVMSample.Models;

namespace TestSample
{
    [TestClass]
    public class ContactListViewModelTest
    {
        [TestMethod]
        public void Should_Load_Contacts_InitData()
        {
            //ARRANGE
            var contactsDataService = MockRepository.GenerateStub<IContactsDataService>();
            var stateManager = MockRepository.GenerateStub<IVisualStateManager>();
            var dispatcher = new MockDispatcherService();
            var mockContactData = new List<Contact>
                {
                    new Contact()
                };
            
            //ACT.. by creating the VM we are also loading the data
            var viewModel = new ContactListViewModel(contactsDataService, stateManager, dispatcher);

            //we have to make our mock service actually pass back the mock data
            EnsureCallbackForIContactsDataServiceIsCalled(contactsDataService, s => s.GetContacts(null), mockContactData, args => args.First());

            //ASSERT
            Assert.AreEqual(mockContactData.Count, viewModel.Contacts.Count, "The contacts were not populated");
            contactsDataService.AssertWasCalled(cds => cds.GetContacts(null), call => call.Constraints(new Anything()));
            stateManager.AssertWasCalled(sm => sm.GoToState("LoadingContacts"));
            stateManager.AssertWasCalled(sm => sm.GoToState("ContactsLoaded"));
        }

        [TestMethod]
        public void Should_Search_Contacts_Execute()
        {
            //ARRANGE
            var contactsDataService = MockRepository.GenerateStub<IContactsDataService>();
            var stateManager = MockRepository.GenerateStub<IVisualStateManager>();
            var dispatcher = new MockDispatcherService();
            var mockContactData = new List<Contact>
                {
                    new Contact()
                };

            //ACT.. 
            var viewModel = new ContactListViewModel(contactsDataService, stateManager, dispatcher);
            viewModel.SearchCommand.Execute("some string");

            //we have to make our mock service actually pass back the mock data
            EnsureCallbackForIContactsDataServiceIsCalled(contactsDataService, s => s.GetContactsByName(null, null), mockContactData, args => args[1]);

            //ASSERT
            Assert.AreEqual(mockContactData.Count, viewModel.Contacts.Count, "The contacts were not populated");
            contactsDataService.AssertWasCalled(cds => cds.GetContacts(null), call => call.Constraints(new Anything()));
            stateManager.AssertWasCalled(sm => sm.GoToState("LoadingContacts"));
            stateManager.AssertWasCalled(sm => sm.GoToState("ContactsLoaded"));
        }

        [TestMethod]
        public void Should_Search_Contacts_CanExecute()
        {
            //ARRANGE
            var contactsDataService = MockRepository.GenerateStub<IContactsDataService>();
            var stateManager = MockRepository.GenerateStub<IVisualStateManager>();
            var dispatcher = new MockDispatcherService();
           
            //ACT.. 
            var viewModel = new ContactListViewModel(contactsDataService, stateManager, dispatcher);

            //ASSERT
            Assert.IsTrue(viewModel.SearchCommand.CanExecute("some string"));
            Assert.IsFalse(viewModel.SearchCommand.CanExecute(null));
            Assert.IsFalse(viewModel.SearchCommand.CanExecute(""));
        }

        #region Helpers

        //this makes sure that 
        private static void EnsureCallbackForIContactsDataServiceIsCalled(IContactsDataService contactsDataService,
            Action<IContactsDataService> methodToReturnTheData, List<Contact> mockContactData, Func<object[], object> getCallbackFromArguments)
        {
            var actionOnGetContacts = getCallbackFromArguments( contactsDataService.GetArgumentsForCallsMadeOn(methodToReturnTheData).First() );
            ((Action<IEnumerable<Contact>>)actionOnGetContacts).Invoke(mockContactData);
        }
        #endregion
    }
}
