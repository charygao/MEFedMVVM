using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVM.Common;
using MEFedMVVM.ViewModelLocator;
using MEFedMVVM.Services.Contracts;
using MEFedMVVMDemo.Services.Models;
using MEFedMVVM.Services.CommonServices;
using System.ComponentModel.Composition;

namespace MEFedMVVMDemo.ViewModels
{
    [ExportViewModel("VM2")]
    public class SelectedUserViewModel : NotifyPropertyChangedBase, IDesignTimeAware
    {
        private User _selectedUser;
        private readonly IMediator _mediator;
      
        /// <summary>
        /// Gets or sets the selected user
        /// </summary>
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(() => SelectedUser);
            }
        }

        [ImportingConstructor]
        public SelectedUserViewModel(IMediator mediator)
        {
            this._mediator = mediator;
            this._mediator.Register(this);
        }

        [MediatorMessageSink(MediatorMessages.SelectedUser, ParameterType=typeof(User))]
        public void OnSelectedUserChanged(User selectedUser)
        {
            SelectedUser = selectedUser;
        }


        #region IDataContextAware Members

        public void DesignTimeInitialization()
        {
            SelectedUser = new User
            {
                Name = "Marlon", Surname = "Grech", Age = 24
            };
        }

        #endregion
    }
}
