using System.ComponentModel.Composition;
using System.Windows.Input;
using MEFedMVVM.Common;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.ViewModelLocator;

namespace MEFedMVVMDemo.ViewModels
{
	[ExportViewModel("MainViewModel")]
	public class MainViewModel : NotifyPropertyChangedBase
	{
		private readonly IVisualStateManager _stateManager;

		[ImportingConstructor]
		public MainViewModel(IVisualStateManager stateManager)
		{
			_stateManager = stateManager;
		}
	}
}