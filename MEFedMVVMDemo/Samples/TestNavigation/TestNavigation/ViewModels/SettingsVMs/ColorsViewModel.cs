using System.Collections.Generic;
using MEFedMVVM.Common;
using MEFedMVVM.NavigationExtensions;
using MEFedMVVM.ViewModelLocator;
using TestNavigation.Models;

namespace TestNavigation.ViewModels.SettingsVMs
{
	[ExportViewModel("ColorsViewModel")]
	public class ColorsViewModel : NotifyPropertyChangedBase, INavigationInfoSubscriber
	{
		public IEnumerable<string> Colors { get; private set; }
		private ApplicationSettings _applicationSettings;
		public ApplicationSettings ApplicationSettings
		{
			get { return _applicationSettings; }
			set
			{
				_applicationSettings = value;
				OnPropertyChanged(() => ApplicationSettings);
			}
		}

		public ColorsViewModel()
		{
			Colors = new[] { "White", "Pink", "Blue", "Green" };			
		}

		#region Implementation of INavigationInfoSubscriber

		/// <summary>
		/// Called when the navigation changes
		/// </summary>
		/// <param name="navigationManager">The navigation manager that is responsable for the navigation</param>
		/// <param name="navigationParameter">The navigationParameter passed</param>
		public void OnNavigationChanged(INavigationManager navigationManager, object navigationParameter)
		{
			ApplicationSettings = (ApplicationSettings) navigationParameter;
		}

		#endregion
	}
}