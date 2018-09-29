using System.ComponentModel.Composition;

namespace MEFedMVVM.NavigationExtensions
{
	/// <summary>
	/// Custom export for the Navigation views
	/// </summary>
	public class NavigationViewAttribute : ExportAttribute
	{
		public NavigationViewAttribute(string viewIdentifier)
			:base (viewIdentifier, typeof(object))
		{ }
	}
}