using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace MEFedMVVM.ViewModelLocator
{
    /// <summary>
    /// Interface for the entity responsable to creates the Composition Container that MEFedMVVM will use to resolve the ViewModels and services
    /// </summary>
    public interface IComposer
    {
        /// <summary>
        /// Implement this to return the Catalogs you want in the main composition
        /// Make sure you include the MEFedMVVM Assembly once in the catalog
        /// </summary>
        /// <returns>Return the catalog to go in the main compoistion</returns>
        ComposablePartCatalog InitializeContainer();

        /// <summary>
        /// Use this to return the list of Custom Export providers you want to be used in the main composition
        /// return null if you do not want to retun any custom export providers
        /// </summary>
        /// <returns>Return a list of Export Providers to be used</returns>
        IEnumerable<ExportProvider> GetCustomExportProviders();
    }
}
