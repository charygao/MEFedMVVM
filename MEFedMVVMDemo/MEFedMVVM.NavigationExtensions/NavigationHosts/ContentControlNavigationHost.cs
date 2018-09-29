using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using MEFedMVVM.NavigationExtensions.Core;

namespace MEFedMVVM.NavigationExtensions.NavigationHosts
{
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[Export(typeof(ControlNavigationHost))]
	public class ContentControlNavigationHost : ControlNavigationHost
	{
		private FrameworkElement _hostContentControl;

		#region Overrides of ControlNavigationHost

		/// <summary>
		/// Returns true if the specified control is supported by this host
		/// </summary>
		/// <param name="host">The element to act as host for views</param>
		/// <returns></returns>
		public override bool CanSupportControl(DependencyObject host)
		{
			return host is ContentControl;
		}

		/// <summary>
		/// Method called when the host is set
		/// </summary>
		/// <param name="host"></param>
		protected override void OnHostElementSpecified(object host)
		{
			_hostContentControl = host as FrameworkElement;
		}

		/// <summary>
		/// When implemented specifies how the host should render the element
		/// </summary>
		/// <param name="control">The control to render</param>
		protected override void RenderControlInternal(FrameworkElement control)
		{
			var contentControl = _hostContentControl as ContentControl;
			if (contentControl != null)
				contentControl.Content = control;
		}

		/// <summary>
		/// When implemented clear the content that was rendered by the RenderControl method
		/// </summary>
		public override void RemoveControl()
		{
			var contentControl = _hostContentControl as ContentControl;
			if (contentControl != null)
				contentControl.Content = null;
		}

		#endregion
	}
}