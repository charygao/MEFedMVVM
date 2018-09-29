using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition;

namespace MEFedMVVM.ViewModelLocator
{
    /// <summary>
    /// Import resolver for the MEFedMVVM lib
    /// </summary>
    public class MEFedMVVMResolver
    {
        private CompositionContainer _container;
        public MEFedMVVMResolver(CompositionContainer container)
        {
            this._container = container;
        }

        public CompositionContainer Container
        {
            get
            {
                return _container;
            }
        }

        public void SatisfyImports(object attributedPart, object contextToInject)
        {
            SetContextToExportProvider(contextToInject);
            Container.SatisfyImportsOnce(attributedPart);
            SetContextToExportProvider(null);
        }

        /// <summary>
        /// Gets teh ViewModel export 
        /// </summary>
        /// <param name="vmContractName">The contract for the view model to get</param>
        /// <param name="contextToInject">The context in which the View Model will be attached</param>
        /// <param name="policy">The policy that you want for the creation of this ViewModel</param>
        /// <returns>Returns the Export for this ViewModel</returns>
        public Export GetViewModelByContract(string vmContractName, object contextToInject, CreationPolicy policy)
        {
            if(Container == null)
                return null;

            var viewModelTypeIdentity = AttributedModelServices.GetTypeIdentity(typeof(object));
            var requiredMetadata = new Dictionary<string, Type>();
            requiredMetadata[ExportViewModel.NameProperty] = typeof(string);
            requiredMetadata[ExportViewModel.IsViewModelFirstProperty] = typeof(bool);


            var definition = new ContractBasedImportDefinition(vmContractName, viewModelTypeIdentity,
                                                               requiredMetadata, ImportCardinality.ExactlyOne, false,
                                                               false, policy);

            SetContextToExportProvider(contextToInject);
            var vmExports = Container.GetExports(definition);
            SetContextToExportProvider(null);

            var vmExport = vmExports.FirstOrDefault();
            if (vmExport != null)
                return vmExport;
            return null;
        }

		public Export GetValueByContract(string contractName, CreationPolicy policy)
    	{
			if (Container == null)
				return null;

			var viewModelTypeIdentity = AttributedModelServices.GetTypeIdentity(typeof(object));
			

			var definition = new ContractBasedImportDefinition(contractName, viewModelTypeIdentity,
															   null, ImportCardinality.ExactlyOne, false,
															   false, policy);

			var vmExports = Container.GetExports(definition);

			var vmExport = vmExports.FirstOrDefault();
			if (vmExport != null)
				return vmExport;
			return null;
    	}

        public object GetExportedValue(Export export)
        {
        	return export.Value;
        }

        internal void SetContextToExportProvider(object contextToInject)
        {
            if (Container.Providers != null && Container.Providers.Count >= 1)
            {
                //try to find the MEFedMVVMExportProvider
                foreach (var item in Container.Providers)
                {
                    var mefedProvider = item as IMEFedMVVMExportProvider;
                    if (mefedProvider != null)
                        mefedProvider.SetContextToInject(contextToInject);
                }
            }
        }

    	
    }
}
