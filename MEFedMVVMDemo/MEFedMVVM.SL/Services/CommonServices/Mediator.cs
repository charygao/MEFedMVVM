using MEFedMVVM.ViewModelLocator;
using MEFedMVVM.Services.Contracts;
using System.ComponentModel.Composition;

namespace MEFedMVVM.Services.CommonServices
{
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    [ExportService(ServiceType.Both, typeof(IMediator))]
    public class Mediator : MediatorBase, IMediator
    {
    }
}
