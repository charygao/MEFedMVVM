namespace MEFedMVVM.NavigationExtensions
{

	/// <summary>
	/// If a NavigationParameter object implements this class it will 
	/// automatically get injected with the NavigationManager responsable for the navigation
	/// </summary>
	public interface INavigationManagerProvider
	{
		/// <summary>
		/// Gets or sets the Navigation manager responsable for the navigation 
		/// </summary>
		INavigationManager NavigationManager { get; set; }
	}
}