using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls.Primitives;
using MEFedMVVM.NavigationExtensions.Core;

namespace MEFedMVVM.NavigationExtensions.NavigationHandlers
{
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[Export(typeof(ControlNavigationHandler))]
	public class ButtonNavigationHandler : ControlNavigationHandler
	{
		private RoutedEventHandler _buttonOnClick;

		#region Overrides of ControlNavigationHandler

		public override bool CanSupportControl(DependencyObject control)
		{
			return control is ButtonBase;
		}

		/// <summary>
		/// Method to handle the navigation
		/// Here is where you handle the event of the control to fire the navigation. To fire the navigation call the OnEventFired
		/// </summary>
		/// <param name="invoker"></param>
		/// <param name="viewName"></param>
		protected override void HandleNavigationInternal(DependencyObject invoker, string viewName)
		{
			var button = invoker as ButtonBase;
			if (button != null)
			{
				_buttonOnClick = (s, e) => OnEventFired(viewName);
				button.Click += _buttonOnClick;
			}
		}		

		/// <summary>
		/// Method to unregister navigation
		/// Here is where you unregister the event of the control
		/// </summary>
		/// <param name="invoker"></param>
		public override void UnregisterNavigation(DependencyObject invoker)
		{
			var button = invoker as ButtonBase;
			if (button != null && _buttonOnClick != null)
				button.Click -= _buttonOnClick;
		}

		#endregion
	}
}