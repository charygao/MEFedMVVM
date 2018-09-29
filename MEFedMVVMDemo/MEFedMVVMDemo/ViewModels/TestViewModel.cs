using System.Collections.Generic;
using System.Threading;
using MEFedMVVM.Common;
using MEFedMVVM.ViewModelLocator;
using MEFedMVVMDemo.Services.Contracts;
using MEFedMVVMDemo.Services.Models;
using System.Collections.ObjectModel;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.Services.CommonServices;
using System.ComponentModel.Composition;

namespace MEFedMVVMDemo.ViewModels
{
    [ExportViewModel("VM1")]
    public class TestViewModel : NotifyPropertyChangedBase
    {
        private readonly ObservableCollection<User> _users = new ObservableCollection<User>();
        private readonly IMediator _mediator;

        private User _selectedUser;

        /// <summary>
        /// Gets or sets the selected user
        /// </summary>
        public User SelectedUser
        {
            get { return _selectedUser; }
            set 
            { 
                _selectedUser = value;
                _mediator.NotifyColleagues(MediatorMessages.SelectedUser, value);
                OnPropertyChanged(() => SelectedUser);
            }
        }
        
        /// <summary>
        /// Gets the list of users
        /// </summary>
        public IList<User> Users { get { return _users; } }

        readonly IUsersService userService;
        readonly IVisualStateManager stateManager;

        [ImportingConstructor]
        public TestViewModel(IUsersService userService, IMediator mediator, 
            IVisualStateManager stateManager)
        {
            stateManager.GoToState("Initial");// go to this state.
            this.userService = userService;
            this._mediator = mediator;
            this.stateManager = stateManager;
            var syncContext = System.Threading.SynchronizationContext.Current;
            ThreadPool.QueueUserWorkItem(delegate
                                              {
                                                  Thread.Sleep(3000);
                                                  syncContext.Send(delegate
                                                                       {
                                                                           LoadUsers();
                                                                       }, null);
                                              });
        }

        void LoadUsers()
        {
            foreach (var item in userService.GetAllUsers())
            {
                if (SelectedUser == null)
                    SelectedUser = item;
                _users.Add(item);
            }
            stateManager.GoToState("Welcome");// go to this state.
        }
    }
}
