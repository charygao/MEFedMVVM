using System;
using MEFedMVVM.Services.Contracts;
using System.Windows.Threading;
using System.Windows;
using MEFedMVVM.ViewModelLocator;
using System.ComponentModel.Composition;

namespace MEFedMVVM.Services.CommonServices
{
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    [ExportService(ServiceType.Both, typeof(IDispatcherService))]
    public class DefaultDispatcherService : IDispatcherService
    {
        #region IDispatcherService Members

        public void BeginInvoke(Delegate method, params object[] parameters)
        {
            if (_currentDispatcher != null)
                _currentDispatcher.BeginInvoke(method, parameters);
        }

        #endregion
           
        public DefaultDispatcherService()
        {
            // this is the fallback in case this service is imported from a non ViewModel (i.e there will be no context to inject)
#if SILVERLIGHT
            _currentDispatcher = Deployment.Current.Dispatcher;
#else
            _currentDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
#endif
        }

        #region IContextAware Members
        Dispatcher _currentDispatcher;
        public void InjectContext(object context)
        {
            var dependencyObject = context as DependencyObject;
            if (dependencyObject != null)
                _currentDispatcher = dependencyObject.Dispatcher;
        }

        #endregion
    }
}
