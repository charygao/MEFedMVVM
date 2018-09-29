using System;

namespace MEFedMVVM.NavigationExtensions
{
	/// <summary>
	/// Interface implemented by ControlNavigationHandlers
	/// This allows you to get notified when navigation changes and also to close a Navigation 
	/// (close is handled by the Host, example for ContentControl it sets Content to null)
	/// </summary>
	public interface INavigationManager
	{
		/// <summary>
		/// Event raised when Navigation is changing from one view to another
		/// or when the CloseNavigation method is called
		/// </summary>
		event Action<NavigationEventArgs> NavigatingAway;

		/// <summary>
		/// Closes the current view
		/// </summary>
		void CloseNavigation();
	}

	/// <summary>
	/// Event args for NavigatingAway event of the INavigationManager
	/// </summary>
	public class NavigationEventArgs
	{
		/// <summary>
		/// Returns true if the user called the CloseNavigation
		/// </summary>
		public bool IsCloseRequest { get; set; }

		/// <summary>
		/// When user chooses to navigate to a new View 
		/// this will be set to the View unique identifier
		/// (If its for a CloseNavigation this will be null)
		/// </summary>
		public string NewNavigationView { get; set; }

		/// <summary>
		/// When user chooses to navigate to a new View 
		/// this will be set to the "ViewModel" of the View 
		/// if that ViewModel implement INavigationInfoSubsciber
		/// (If its for a CloseNavigation this will be null)
		/// </summary>
		public INavigationInfoSubscriber NewNavigationInfoSubscriber { get; set; }
	}
}