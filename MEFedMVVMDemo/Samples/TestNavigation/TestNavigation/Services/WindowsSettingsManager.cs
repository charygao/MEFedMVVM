using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MEFedMVVM.ViewModelLocator;
using TestNavigation.Models;
using TestNavigation.Services.Contracts;

namespace TestNavigation.Services
{
	[ExportService(ServiceType.Both, typeof(IWindowSettingsManager))]
	public class WindowsSettingsManager : IWindowSettingsManager
	{
		private ContentControl _view;

		#region Implementation of IContextAware

		public void InjectContext(object context)
		{
			_view = context as ContentControl;
		}

		public void ApplySettings(ApplicationSettings settings)
		{
			if(_view == null)
				return;
#if SILVERLIGHT
			//SILVELIGHT DOESN'T LIKE CHANGING OF COLORS SO I AM NOT PUTTING THIS IN THE DEMO
#else
			_view.Background = (Brush) new BrushConverter().ConvertFromString(settings.Color);
#endif


		}

		#endregion
	}
}