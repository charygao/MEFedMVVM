using System;
using System.IO;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Windows.Resources;
using System.ComponentModel.Composition.Primitives;

namespace MEFedMVVM.ViewModelLocator
{
    /// <summary>
    /// Default composer for Design time. This will load all assemblies that have the DesignTimeCatalog attibute
    /// </summary>
    public class DefaultDesignTimeComposer : IComposer
    {
        #region IComposer Members

        public ComposablePartCatalog InitializeContainer()
        {
            return GetCatalog();
        }

        /// <summary>
        /// Use this to return the list of Custom Export providers you want to be used in the main composition
        /// return null if you do not want to retun any custom export providers
        /// </summary>
        /// <returns>Return a list of Export Providers to be used</returns>
        public IEnumerable<ExportProvider> GetCustomExportProviders()
        {
            return null;
        }

        #endregion 

        private const string MefedmvvmSL = "MEFedMVVM.SL";
        private const string MefedmvvmSLDll = "MEFedMVVM.SL.dll";

        private static AggregateCatalog GetCatalog()
        {
            Dictionary<string, AssemblyCatalog> assemDict = new Dictionary<string, AssemblyCatalog>();
            IList<AssemblyCatalog> assembliesLoadedCatalogs = (from assembly in
                                                                   (IEnumerable<Assembly>)typeof(AppDomain)
                                                                  .GetMethod("GetAssemblies")
                                                                  .Invoke(AppDomain.CurrentDomain, null)
                                                               let references = ((IEnumerable<AssemblyName>)typeof(Assembly).GetMethod("GetReferencedAssemblies").Invoke(assembly, null))

                                                               where (
                                                                       !ShouldIgnoreAtDesignTime(assembly) 
                                                                       && references.Any(x => x.Name.Contains(MefedmvvmSL))
                                                                     )
                                                                     || assembly.ManifestModule.Name == MefedmvvmSLDll
                                                               
                                                               select new AssemblyCatalog(assembly)).ToList();

            if (assembliesLoadedCatalogs.Where(x => x.Assembly.ManifestModule.Name != MefedmvvmSLDll).Count() == 0)
            {
                Debug.WriteLine("No assemblies found for Design time. ");
                return null;
            }

            var catalog = new AggregateCatalog();

            foreach (var item in assembliesLoadedCatalogs)
            {
                AssemblyCatalog ass;
                if (assemDict.TryGetValue(item.Assembly.FullName, out ass))
                {
                    DateTime oldAssDT = File.GetLastAccessTime(ass.Assembly.Location);
                    DateTime newAssDT = File.GetLastAccessTime(item.Assembly.Location);
                    if (newAssDT > oldAssDT)
                    {
                        assemDict[item.Assembly.FullName] = item;
                    }
                }
                else
                {
                    assemDict[item.Assembly.FullName] = item;
                }
            }

            foreach (var item in assemDict.Values)
                catalog.Catalogs.Add(item);
            return catalog;
        }

        private static bool ShouldIgnoreAtDesignTime(Assembly assembly)
        {
            object[] customAttributes = assembly.GetCustomAttributes(typeof(IgnoreAtDesignTimeAttribute), true);
            return customAttributes != null && customAttributes.Length != 0;
        }
    }

    /// <summary>
    /// Implemenation for the default runtime composer. This composer doesn't do anything since it relies on the CompositionInializer.SatisyImports default implemenation
    /// </summary>
    public class DefaultRuntimeComposer : IComposer
    {
        #region IComposer Members

        public ComposablePartCatalog InitializeContainer()
        {
            return new AggregateCatalog((from assembly in GetAssemblyList() select new AssemblyCatalog(assembly)).Cast<ComposablePartCatalog>());
        }

        /// <summary>
        /// Use this to return the list of Custom Export providers you want to be used in the main composition
        /// return null if you do not want to retun any custom export providers
        /// </summary>
        /// <returns>Return a list of Export Providers to be used</returns>
        public IEnumerable<ExportProvider> GetCustomExportProviders()
        {
            return null;
        }

        #endregion

        private static List<Assembly> GetAssemblyList()
        {
            List<Assembly> list = new List<Assembly>();
            foreach (AssemblyPart part in Deployment.Current.Parts)
            {
                StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(part.Source, UriKind.Relative));
                if (resourceStream != null)
                {
                    Assembly item = part.Load(resourceStream.Stream);
                    list.Add(item);
                }
            }
            return list;
        }

    }

}
