using MEFedMVVM.Common;
using MEFedMVVM.NavigationExtensions;
using MEFedMVVM.ViewModelLocator;
using TestNavigation.Models;

namespace TestNavigation.ViewModels
{
	[ExportViewModel("RenderUserProfileViewModel")]
	public class RenderUserProfileViewModel : NotifyPropertyChangedBase, INavigationInfoSubscriber, IDesignTimeAware
	{
		private UserProfile _profile;
		public UserProfile Profile
		{
			get { return _profile; }
			set
			{
				_profile = value;
				OnPropertyChanged(() => Profile);
			}
		}

		#region Implementation of INavigationInfoSubscriber

		/// <summary>
		/// Called when the navigation changes
		/// </summary>
		/// <param name="navigationManager">The navigation manager that can be user to control the navigation of the control</param>
		/// <param name="navigationParameter">The navigationParameter passed</param>
		public void OnNavigationChanged(INavigationManager navigationManager, object navigationParameter)
		{
			Profile = (UserProfile) navigationParameter;
		}

		#endregion

		#region Implementation of IDesignTimeAware

		public void DesignTimeInitialization()
		{
			Profile = new UserProfile()
			          	{
			          		Name = "Marlon",
			          		Surname = "Grech",
			          		Gender = "Male"
			          	};
		}

		#endregion
	}
}