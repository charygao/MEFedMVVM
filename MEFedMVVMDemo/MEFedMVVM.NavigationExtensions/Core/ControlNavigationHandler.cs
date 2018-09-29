using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using MEFedMVVM.ViewModelLocator;

namespace MEFedMVVM.NavigationExtensions.Core
{
	/// <summary>
	/// Base abstract class to be implemented for different control invoker 
	/// </summary>
	public abstract class ControlNavigationHandler : INavigationManager
	{
		private ControlNavigationHost _host;
		private object _navigationParameter;

		/// <summary>
		/// Does the hook to the control's event so that to fire the navigation
		/// </summary>
		/// <param name="invoker">The control invoker</param>
		/// <param name="viewName">The view export name</param>
		public void HandleNavigation(DependencyObject invoker, string viewName)
		{
			HandleNavigationInternal(invoker, viewName);
		}

		/// <summary>
		/// Prepares the host
		/// </summary>
		/// <param name="host"></param>
		public void PrepareHost(DependencyObject host)
		{
			var hosts = MEFedContainer.Instance.Container.GetExportedValues<ControlNavigationHost>();
			_host = hosts.FirstOrDefault(x => x.CanSupportControl(host));
			if (_host == null)
				throw new InvalidOperationException(host.GetType().FullName + " is not supported for the NavigationHost. Note: you can create your own by implementing a ControlNavigationHost and exporting it as type ControlNavigationHost");

			_host.PrepareHost(host);

		}

		/// <summary>
		/// Prepares the parameter to be passed
		/// </summary>
		/// <param name="parameter"></param>
		public void PrepareNavigationParameter(object parameter)
		{
			var navigationProvider = parameter as INavigationManagerProvider;
			if (navigationProvider != null)
				navigationProvider.NavigationManager = this;

			_navigationParameter = parameter;
		}

		public abstract bool CanSupportControl(DependencyObject control);

		/// <summary>
		/// Method to handle the navigation
		/// Here is where you handle the event of the control to fire the navigation. To fire the navigation call the OnEventFired
		/// </summary>
		/// <param name="invoker"></param>
		/// <param name="viewName"></param>
		protected abstract void HandleNavigationInternal(DependencyObject invoker, string viewName);

		/// <summary>
		/// Method to unregister navigation
		/// Here is where you unregister the event of the control
		/// </summary>
		/// <param name="invoker"></param>
		public abstract void UnregisterNavigation(DependencyObject invoker);

		/// <summary>
		/// Method to be called from the event handler handled in the HandleNavigationInternal
		/// </summary>
		/// <param name="viewName">The view name that is passed to you from HandleNavigationInternal</param>
		public void OnEventFired(string viewName)
		{
			if (_host == null)
				throw new InvalidOperationException("NavigationHost was not specified");

			var export = MEFedContainer.Instance.Resolver.GetValueByContract(viewName, CreationPolicy.NonShared);
			if (export == null)
				throw new InvalidOperationException("View " + viewName + " was not found. Make sure view is exported");

			var view = export.Value as FrameworkElement;
			if (view == null)
				throw new InvalidOperationException("View must be a framework element");
			var navigationInfoSubscriber = view.DataContext as INavigationInfoSubscriber;
			if (navigationInfoSubscriber != null)
				navigationInfoSubscriber.OnNavigationChanged(this, _navigationParameter);

			OnNavigationChanged(new NavigationEventArgs { IsCloseRequest = false, NewNavigationView = viewName, NewNavigationInfoSubscriber = navigationInfoSubscriber});

			NavigationExtensions.SetNavigationHandler(view, this);

			_host.RenderControl(view);

		}

		#region Implementation of INavigationManager

		public event Action<NavigationEventArgs> NavigatingAway;

		public void CloseNavigation()
		{
			if (_host == null)
				throw new InvalidOperationException("NavigationHost was not specified");

			OnNavigationChanged(new NavigationEventArgs {IsCloseRequest = true});
			_host.RemoveControl();
		}

		#endregion

		private void OnNavigationChanged(NavigationEventArgs args)
		{
			if (NavigatingAway != null)
				NavigatingAway(args);
		}
	}
}