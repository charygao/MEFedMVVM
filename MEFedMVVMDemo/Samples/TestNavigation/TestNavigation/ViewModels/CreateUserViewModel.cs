using System.Collections.Generic;
using MEFedMVVM.NavigationExtensions;
using MEFedMVVM.ViewModelLocator;
using TestNavigation.Models;

namespace TestNavigation.ViewModels
{
	[ExportViewModel("CreateUserViewModel")]
	public class CreateUserViewModel : INavigationInfoSubscriber
	{
		public IEnumerable<string> Genders { get; set; }
		public UserProfile Profile { get; set; }

		public CreateUserViewModel()
		{
			Genders = new[] {"Male", "Female"};
			Profile = new UserProfile();
		}

		#region Implementation of INavigationInfoSubscriber

		/// <summary>
		/// Called when the navigation changes
		/// </summary>
		/// <param name="navigationManager">The navigation manager that can be user to control the navigation of the control</param>
		/// <param name="navigationParameter">The navigationParameter passed</param>
		public void OnNavigationChanged(INavigationManager navigationManager, object navigationParameter)
		{
			//when the next navigation subscriber tries to navigate pass along the user profile
			navigationManager.NavigatingAway += args =>
			{
				if(args.NewNavigationInfoSubscriber != this)
					args.NewNavigationInfoSubscriber.OnNavigationChanged(navigationManager, Profile);
			};
		}

		#endregion
	}
}