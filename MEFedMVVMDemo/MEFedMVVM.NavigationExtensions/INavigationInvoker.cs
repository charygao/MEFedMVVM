namespace MEFedMVVM.NavigationExtensions
{
	public interface INavigationInvoker
	{
		/// <summary>
		/// navigates to the specified view
		/// </summary>
		/// <param name="viewName">THe view name specified when view decorated with [NavigationView]</param>
		/// <param name="parameter">Parameter to pass</param>
		INavigationManager Navigate(string viewName, object parameter);
	}
}