using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.ComponentModel.Composition.Primitives;
using MEFedMVVM.Common;

namespace MEFedMVVM.ViewModelLocator
{

     /// <summary>
    /// Custom MEF Catalog to return services that are marked as Runtime when you are at runtime and design time services when you are at design time
    /// </summary>
    public class MEFedMVVMCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        private readonly ComposablePartCatalog _inner;
        private readonly IQueryable<ComposablePartDefinition> _partsQuery;

        public MEFedMVVMCatalog(ComposablePartCatalog inner, bool designTime)
        {            
            _inner = inner;
            _partsQuery = designTime ?
                _inner.Parts.Where(p => p.ExportDefinitions.Any(ed => 
                    !ed.Metadata.ContainsKey(ExportService.IsDesignTimeServiceProperty) || 
                    ed.Metadata.ContainsKey(ExportService.IsDesignTimeServiceProperty) 
                    && 
                    (ed.Metadata[ExportService.IsDesignTimeServiceProperty].Equals(ServiceType.DesignTime) || 
                    ed.Metadata[ExportService.IsDesignTimeServiceProperty].Equals(ServiceType.Both))
                    ))
                : _inner.Parts.Where(p => p.ExportDefinitions.Any(ed => !ed.Metadata.ContainsKey(ExportService.IsDesignTimeServiceProperty) || 
                    ed.Metadata.ContainsKey(ExportService.IsDesignTimeServiceProperty) 
                    && 
                    (ed.Metadata[ExportService.IsDesignTimeServiceProperty].Equals(ServiceType.Runtime) || 
                    ed.Metadata[ExportService.IsDesignTimeServiceProperty].Equals(ServiceType.Both))));


            //Try to hookup Changed event
            var innerCatalogChangeInterface = inner as INotifyComposablePartCatalogChanged;
            if (innerCatalogChangeInterface != null)
            {
                innerCatalogChangeInterface.Changed += (s, e) => OnChanged(e);
                innerCatalogChangeInterface.Changing += (s, e) => OnChanging(e);
            }
        }

        #region INotifyComposablePartCatalogChanged implementation
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

        public void OnChanged(ComposablePartCatalogChangeEventArgs e)
        {
            if (this.Changed != null)
                this.Changed(this, e);
        }

        public void OnChanging(ComposablePartCatalogChangeEventArgs e)
        {
            if (this.Changing != null)
                this.Changing(this, e);
        }
        #endregion


        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return _partsQuery;
            }
        }

        public static MEFedMVVMCatalog CreateCatalog(ComposablePartCatalog inner)
        {
            return new MEFedMVVMCatalog(inner, Designer.IsInDesignMode);
        }
    }
    
}
