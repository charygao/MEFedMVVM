using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using MEFedMVVM.Common;
using MEFedMVVM.NavigationExtensions;
using MEFedMVVM.ViewModelLocator;
using TestNavigation.Models;

namespace TestNavigation.ViewModels
{
	[ExportViewModel("SettingsViewModel")]
	public class SettingsViewModel : INavigationInfoSubscriber
	{
		public void OnNavigationChanged(INavigationManager navigationManager, object navigationParameter)
		{
			_navigationManager = navigationManager;
			_onSettingsChangedCommand = (NavigationCommand<ApplicationSettings>)navigationParameter;
		}

		private DelegateCommand<ApplicationSettings> _onSettingsChangedCommand;
		private INavigationManager _navigationManager;
		public ApplicationSettings ApplicationSettings { get; set; }

		public ICommand SaveSettingsCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		/// <summary>
		/// This is here just to show how you can navigate to a view from a code not with the attached properties
		/// </summary>
		public ICommand ShowColors { get; private set; }

		[ImportingConstructor]
		public SettingsViewModel(INavigationInvokerFactory invokerFactory)
		{
			ApplicationSettings = new ApplicationSettings() {Color = "White"};
			SaveSettingsCommand = new DelegateCommand<object>(OnSaveExecuted, x => _onSettingsChangedCommand != null);
			CancelCommand = new DelegateCommand<object>(x => _navigationManager.CloseNavigation(),
			                                            x => _navigationManager != null);

			ShowColors = new DelegateCommand<object>(x =>
			                                         	{
			                                         		var invoker = invokerFactory.CreateNavigationInvoker("settingsContent");
			                                         		invoker.Navigate("http://myApp/settingsColors", ApplicationSettings);
			                                         	});
		}

		private void OnSaveExecuted(object x)
		{
			_onSettingsChangedCommand.Execute(ApplicationSettings);
		}

		
	}
}