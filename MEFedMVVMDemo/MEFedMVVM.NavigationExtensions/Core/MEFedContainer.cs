using System.ComponentModel.Composition.Hosting;

namespace MEFedMVVM.NavigationExtensions.Core
{
	/// <summary>
	/// Workaround for the ViewModelResolver missing from the MEFedMVVM core (reason for it missing is to support WPF and SL)
	/// </summary>
	public class MEFedContainer
	{
		private static MEFedContainer _instance;
		public static MEFedContainer Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MEFedContainer();
					
#if MEFEDWPF
					_instance.Container = MEFedMVVM.ViewModelLocator.ViewModelRepository.Instance.Resolver.Container;
					_instance.Resolver = MEFedMVVM.ViewModelLocator.ViewModelRepository.Instance.Resolver;
#endif
#if MEFEDSL
					_instance.Container = MEFedMVVM.ViewModelLocator.ViewModelRepository.Instance.Resolver.Container;
					_instance.Resolver = MEFedMVVM.ViewModelLocator.ViewModelRepository.Instance.Resolver;
#endif
				}
				return _instance;
			}
		}

		public CompositionContainer Container { get; set; }

		public ViewModelLocator.MEFedMVVMResolver Resolver { get; set; }

		private MEFedContainer(){}
	}
}