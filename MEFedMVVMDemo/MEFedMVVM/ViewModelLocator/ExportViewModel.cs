using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using MEFedMVVM.Common;


namespace MEFedMVVM.ViewModelLocator
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExportViewModel : ExportAttribute
    {
        internal const string NameProperty = "Name";
		internal const string IsViewModelFirstProperty = "IsViewModelFirst";
		internal const string ShouldReSatisfyImportsProperty = "ShouldReSatisfyImports";

        /// <summary>
        /// Gets the name of the View Model. This is used to import the ViewModel from the XAML
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Specifies if the ViewModel is DataContextAware
        /// </summary>
        public bool IsViewModelFirst { get; private set; }

		/// <summary>
		/// Set to true to re satisfy imports on a ViewModel which is marked as IsViewModelFirst whenever the data context of the view changes. 
		/// Please note this is only supported in WPF since Silverlight still does not expose a DataContextChanged event
		/// </summary>
    	public bool ShouldReSatisfyImports { get; set; }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="name">The name to assign to the ViewModel. This name will be used to assign a ViewModel to a View</param>
        public ExportViewModel(string name)
            : this(name, false)
        { }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="name">The name to assign to the ViewModel. This name will be used to assign a ViewModel to a View</param>
		/// <param name="isViewModelFirst">Pass true if you want to get injected with services even though the ViewModel is not created via MEF</param>
		public ExportViewModel(string name, bool isViewModelFirst)
            : base(name, typeof(object))
        {
            Name = name;
            IsViewModelFirst = isViewModelFirst;
        }
    }
}
