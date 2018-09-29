using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Input;
using MEFedMVVM.Common;
using MEFedMVVM.NavigationExtensions;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.ValidationsExtensions;
using MEFedMVVM.ViewModelLocator;
using MEFedMVVMDemo.Services.Models;

namespace MEFedMVVMDemo.ViewModels
{
    [ExportViewModel("AddNewUserViewModel")]
    public class AddNewUserViewModel : NotifyPropertyChangedBase, INavigationInfoSubscriber
    {
    	private readonly IMediator _mediator;
    	private User _newUser;
    	private INavigationManager _navigationManager;

    	public User NewUser
    	{
    		get
    		{
    			return _newUser;
    		}
    		private set
    		{
    			_newUser = value;
				OnPropertyChanged(() => NewUser);
    		}
    	}

    	public ICommand AddNewUserCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		[ImportingConstructor]
        public AddNewUserViewModel( IMediator mediator)
        {
			_mediator = mediator;
			NewUser = new User();
            
			AddNewUserCommand = new DataValidationDelegateCommand<User>(OnAddedHandler)
				.DependsOn( () => NewUser.Name)
				.AndOn( () => NewUser.Surname)
				.AndOn( () => NewUser.Age );

			CancelCommand = new DelegateCommand<object>(OnCancelHandler);
        }

    	private void OnCancelHandler(object obj)
    	{
			NewUser = new User();
    		_navigationManager.CloseNavigation();
    	}

    	private void OnAddedHandler(User obj)
    	{
			Debug.WriteLine("New user added");
			_mediator.NotifyColleagues(MediatorMessages.AddUser, NewUser);
			_navigationManager.CloseNavigation();
    	}

    	#region Implementation of INavigationInfoSubscriber

    	/// <summary>
    	/// Called when the navigation changes
    	/// </summary>
    	/// <param name="navigationManager">The navigation manager that is responsable for the navigation</param>
    	/// <param name="navigationParameter">The navigationParameter passed</param>
    	public void OnNavigationChanged(INavigationManager navigationManager, object navigationParameter)
    	{
    		_navigationManager = navigationManager;
    	}

    	#endregion
    }
}