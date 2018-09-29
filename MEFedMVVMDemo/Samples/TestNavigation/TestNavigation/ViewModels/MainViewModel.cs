using System.ComponentModel.Composition;
using System.Windows.Input;
using MEFedMVVM.Common;
using MEFedMVVM.NavigationExtensions;
using MEFedMVVM.ViewModelLocator;
using TestNavigation.Models;
using TestNavigation.Services.Contracts;

namespace TestNavigation.ViewModels
{
	[ExportViewModel("MainVM")]
	public class MainViewModel
	{
		private readonly IWindowSettingsManager _windowSettingsManager;
		public ICommand OnUserDetailsEntered { get; private set; }
		public UserProfile User { get; private set; }

		public NavigationCommand<ApplicationSettings> SettingsChangedCommand { get; private set; }

		[ImportingConstructor]
		public MainViewModel(IWindowSettingsManager windowSettingsManager)
		{
			_windowSettingsManager = windowSettingsManager;
			SettingsChangedCommand = new NavigationCommand<ApplicationSettings>(OnSettingsChangedExecuted);
		}

		private void OnSettingsChangedExecuted(ApplicationSettings settings)
		{
			_windowSettingsManager.ApplySettings(settings);
			SettingsChangedCommand.NavigationManager.CloseNavigation();
		}
	}
}