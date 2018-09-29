using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;

using MEFedMVVM.Common;
using System.ComponentModel.Composition.Primitives;

namespace MEFedMVVM.ViewModelLocator
{
    /// <summary>
    /// This is the ViewModel initializer that ViewModel after it is set as datacontext
    /// </summary>
    public class DataContextAwareViewModelInitializer : BasicViewModelInitializer
    {
        public DataContextAwareViewModelInitializer(MEFedMVVMResolver resolver)
            : base (resolver )
        { }

		public override void CreateViewModel(Export viewModelContext,
			FrameworkElement containerElement)
		{
			CreateViewModel(viewModelContext, containerElement, false);
		}

    	public void CreateViewModel(Export viewModelContext, 
            FrameworkElement containerElement, bool shouldReSatisfyImports)
        {
            if (!Designer.IsInDesignMode) // if at runtime
            {
#if SILVERLIGHT
                RoutedEventHandler handler = null;
                handler = delegate
                {
                    if (containerElement.DataContext != null) // it means we have the VM instance now we should inject the services
                    {
                        resolver.SatisfyImports(containerElement.DataContext, containerElement);
                    }
					containerElement.Loaded -= handler;
                };
                if (containerElement.DataContext == null)
                    containerElement.Loaded += handler;
                else
                {
                    handler(null, default(RoutedEventArgs));
                }
#else
                DependencyPropertyChangedEventHandler handler = null;
                handler = delegate
                {
                    if (containerElement.DataContext != null) // it means we have the VM instance now we should inject the services
                    {
						var data = containerElement.DataContext.GetType().GetCustomAttributes(typeof(ExportViewModel), true).FirstOrDefault();

						if (data == null || ((ExportViewModel)data).Name != (string) viewModelContext.Metadata[ExportViewModel.NameProperty])
							return;

						if (!shouldReSatisfyImports)
						{
							containerElement.DataContextChanged -= handler;
						}

                    	resolver.SatisfyImports(containerElement.DataContext, containerElement);
                    }
                };

                if (containerElement.DataContext == null)
                    containerElement.DataContextChanged += handler; // we need to wait until the context is set
                else // DataContext is already set 
                {
                    handler(null, default(DependencyPropertyChangedEventArgs));
                }
#endif
            }


            if(Designer.IsInDesignMode)
            {
                base.CreateViewModel(viewModelContext, containerElement ); // this will create the VM and set it as DataContext
            }
        }
    }
}
