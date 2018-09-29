namespace MEFedMVVM.NavigationExtensions
{
	/// <summary>
	/// Contract for navigation subscribers
	/// This would be implemented by a ViewModel to be passed the naviagtion info when the navigation changes
	/// </summary>
	public interface INavigationInfoSubscriber
	{
		/// <summary>
		/// Called when the navigation changes
		/// </summary>
		/// <param name="navigationManager">The navigation manager that is responsable for the navigation</param>
		/// <param name="navigationParameter">The navigationParameter passed</param>
		void OnNavigationChanged(INavigationManager navigationManager, object navigationParameter);
	}
}