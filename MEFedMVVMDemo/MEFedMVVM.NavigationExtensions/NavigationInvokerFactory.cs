using System;
using System.ComponentModel.Composition;
using System.Windows;
using MEFedMVVM.NavigationExtensions.Core;
using MEFedMVVM.ViewModelLocator;

namespace MEFedMVVM.NavigationExtensions
{
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[ExportService(ServiceType.Both, typeof(INavigationInvokerFactory))]
	public class NavigationInvokerFactory : INavigationInvokerFactory
	{
		private FrameworkElement _view;

		#region Implementation of IContextAware

		public void InjectContext(object context)
		{
			_view = (FrameworkElement) context;
		}

		/// <summary>
		/// Creates a navigation invoker that can render views in the specified host name
		/// </summary>
		/// <param name="hostName">The name of the host to use</param>
		/// <returns></returns>
		public INavigationInvoker CreateNavigationInvoker(string hostName)
		{
			var element = _view.FindName(hostName) as FrameworkElement;
			if (element == null)
				throw new InvalidOperationException("Cannot find host with name " + hostName);
			return new NavigationInvoker((FrameworkElement) element);
		}

		#endregion
	}

	public class NavigationInvoker : ControlNavigationHandler, INavigationInvoker
	{
		public NavigationInvoker(FrameworkElement host)
		{
			PrepareHost(host);
		}

		#region Implementation of INavigationInvoker

		/// <summary>
		/// navigates to the specified view
		/// </summary>
		/// <param name="viewName">THe view name specified when view decorated with [NavigationView]</param>
		/// <param name="parameter"></param>
		public INavigationManager Navigate(string viewName, object parameter)
		{
			PrepareNavigationParameter(parameter);
			OnEventFired(viewName);
			return this;
		}

		#endregion

		#region Overrides of ControlNavigationHandler

		/// <summary>
		/// This method should never be called for this implementation of the control handler
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		public override bool CanSupportControl(DependencyObject control)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Method to handle the navigation
		/// Here is where you handle the event of the control to fire the navigation. To fire the navigation call the OnEventFired
		/// </summary>
		/// <param name="invoker"></param>
		/// <param name="viewName"></param>
		protected override void HandleNavigationInternal(DependencyObject invoker, string viewName)
		{
			
		}

		/// <summary>
		/// Method to unregister navigation
		/// Here is where you unregister the event of the control
		/// </summary>
		/// <param name="invoker"></param>
		public override void UnregisterNavigation(DependencyObject invoker)
		{
			
		}

		#endregion
	}
}