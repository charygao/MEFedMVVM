using System.ComponentModel.Composition.Hosting;

namespace MEFedMVVM.ViewModelLocator
{
    public interface IContainerProvider
    {
        CompositionContainer CreateContainer();
    }
}
