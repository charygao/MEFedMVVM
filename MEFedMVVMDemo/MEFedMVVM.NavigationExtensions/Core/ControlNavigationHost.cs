using System;
using System.Windows;

namespace MEFedMVVM.NavigationExtensions.Core
{
	/// <summary>
	/// abstract class for navigation hosts
	/// A navigation host is a class that tells a host element(example grid) how to render a view
	/// </summary>
	public abstract class ControlNavigationHost
	{
		private object _hostElement;

		/// <summary>
		/// Prepares the host
		/// </summary>
		/// <param name="host">the host element that will render the view</param>
		public void PrepareHost(object host)
		{
			_hostElement = host;
			OnHostElementSpecified(host);
		}

		/// <summary>
		/// Renders the specified object
		/// </summary>
		/// <param name="elementToRender">The element to render</param>
		public void RenderControl(object elementToRender)
		{
			if (_hostElement == null)
				throw new InvalidOperationException("NavgationHost not specified");

			var frameworkElement = elementToRender as FrameworkElement;
			if (frameworkElement == null)
				throw new InvalidOperationException("Element to render has to be a FrameworkElement");

			RenderControlInternal(frameworkElement);
		}

		/// <summary>
		/// Returns true if the specified control is supported by this host
		/// </summary>
		/// <param name="host">The element to act as host for views</param>
		/// <returns></returns>
		public abstract bool CanSupportControl(DependencyObject host);

		/// <summary>
		/// Method called when the host is set
		/// </summary>
		/// <param name="host"></param>
		protected abstract void OnHostElementSpecified(object host);

		/// <summary>
		/// When implemented specifies how the host should render the element
		/// </summary>
		/// <param name="control">The control to render</param>
		protected abstract void RenderControlInternal(FrameworkElement control);

		/// <summary>
		/// When implemented clear the content that was rendered by the RenderControl method
		/// </summary>
		public abstract void RemoveControl();
	}
}