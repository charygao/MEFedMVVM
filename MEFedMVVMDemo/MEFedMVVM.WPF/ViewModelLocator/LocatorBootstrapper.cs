using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVM.ViewModelLocator;
using MEFedMVVM.Common;
using System.ComponentModel.Composition.Hosting;

namespace MEFedMVVM.ViewModelLocator
{
    /// <summary>
    /// This is the bootstrapper that inializes the MEF catalogs
    /// </summary>
    public class LocatorBootstrapper
    {
        /// <summary>
        /// Checks if the bootstapper is inialized if now it inializes it
        /// </summary>
        public static CompositionContainer EnsureLocatorBootstrapper()
        {
            IComposer composer = null;

            if (Designer.IsInDesignMode)
            {
                if (designTimeComposer == null)
                    designTimeComposer = new DefaultDesignTimeComposer();
                composer = designTimeComposer;
            }
            else
            {
                if (runtimeComposer == null) // if the composer is not set then we should use the default one
                    runtimeComposer = new DefaultRuntimeComposer();
                composer = runtimeComposer;
            }

            CompositionContainer container;
            if (composer is IContainerProvider)
            {
                container = ((IContainerProvider) composer).CreateContainer();
                
                var mefedProviderLocated = container.Providers.OfType<IMEFedMVVMExportProvider>().Any();
                if(!mefedProviderLocated)
                    throw new InvalidOperationException("ContainerProvider did not include MEFedMVVMExportProvider in the CompositionContainer");     
            }
            else
            {
                var catalog = composer.InitializeContainer();
                var mefedProvider = new MEFedMVVMExportProvider(MEFedMVVMCatalog.CreateCatalog(catalog));
                var providersList = new List<ExportProvider>();

                //check if there are any custom export providers
                var providers = composer.GetCustomExportProviders();
                if (providers != null && providers.Count() != 0)
                    providersList.AddRange(providers);

                //add mefedMVVM provider
                providersList.Add(mefedProvider);

                container = new CompositionContainer(providersList.ToArray());
                foreach (var imefedProvider in container.Providers.OfType<IMEFedMVVMExportProvider>())
                {
                    imefedProvider.SourceProvider = container;
                }
            }

            return container;

        }

        private static IComposer runtimeComposer { get; set; }
        private static IComposer designTimeComposer { get; set; }

        /// <summary>
        /// forces the Locator to use this composer
        /// </summary>
        ///<param name="designTimeComposer">The compose to create the CompositionContainer</param>
        ///<param name="runtimeComposer">The composer to use for runtime</param>
        public static void ApplyComposer(IComposer runtimeComposer)
        {
            LocatorBootstrapper.runtimeComposer = runtimeComposer;
        }
    }
}
