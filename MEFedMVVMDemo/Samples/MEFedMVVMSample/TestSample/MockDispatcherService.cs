using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEFedMVVM.Services.Contracts;

namespace TestSample
{
    public class MockDispatcherService : IDispatcherService
    {
        public void BeginInvoke(Delegate method, params object[] parameters)
        {
            method.DynamicInvoke(parameters);
        }

        public void InjectContext(object context)
        {
            // do nothing
        }
    }
}
