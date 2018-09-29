using MEFedMVVM.Services.Contracts;

namespace MEFedMVVM.NavigationExtensions
{
	public interface INavigationInvokerFactory : IContextAware
	{
		/// <summary>
		/// Creates a navigation invoker that can render views in the specified host name
		/// </summary>
		/// <param name="hostName">The name of the host to use</param>
		/// <returns></returns>
		INavigationInvoker CreateNavigationInvoker(string hostName);
	}

	
}