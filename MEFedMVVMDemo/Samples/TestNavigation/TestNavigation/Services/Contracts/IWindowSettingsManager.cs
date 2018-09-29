using MEFedMVVM.Services.Contracts;
using TestNavigation.Models;

namespace TestNavigation.Services.Contracts
{
	public interface IWindowSettingsManager : IContextAware
	{
		void ApplySettings(ApplicationSettings settings);
	}
}