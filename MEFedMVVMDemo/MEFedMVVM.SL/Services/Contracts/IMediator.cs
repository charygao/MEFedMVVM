using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MEFedMVVM.Services.Contracts;

namespace MEFedMVVM.Services.Contracts
{
    /// <summary>
    /// A message mediator allowing disconnected ViewModels to send and
    /// receive messages.
    /// </summary>
    public interface IMediator : IMediatorBase
    {

    }
}
