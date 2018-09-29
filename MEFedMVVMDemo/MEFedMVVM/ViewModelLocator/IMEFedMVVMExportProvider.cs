using System.ComponentModel.Composition.Hosting;

namespace MEFedMVVM.ViewModelLocator
{
    public interface IMEFedMVVMExportProvider
    {
        void SetContextToInject(object context);
        ExportProvider SourceProvider { get; set; }
    }
}