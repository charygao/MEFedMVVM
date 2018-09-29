using System;
using System.Collections.Generic;
using MEFedMVVM.Services.Contracts;
using System.Reflection;

namespace MEFedMVVM.Services.CommonServices
{
    public abstract class MediatorBase : IMediatorBase
    {
        private readonly MessageToActionMap _invocationList = new MessageToActionMap();

        public IDisposable Register(object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            
            List<KeyValuePair<string, WeakAction>> registrations = new List<KeyValuePair<string,WeakAction>>();

            foreach (var methodInfo in target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (MediatorMessageSinkAttribute attribute in methodInfo.GetCustomAttributes(typeof(MediatorMessageSinkAttribute), true))
                {
                    if (methodInfo.GetParameters().Length != 1)
                        throw new InvalidOperationException("The registered method should only have 1 parameter since the Mediator has only 1 argument to pass");
                    
                    registrations.Add( new KeyValuePair<string, WeakAction> ( attribute.Message,
                    _invocationList.AddAction(attribute.Message, target, methodInfo, attribute.ParameterType)) );
                }
            }

            return new MediatorRegistration(_invocationList, registrations);
        }

        public void NotifyColleagues<T>(string message, T parameter)
        {
            var actions = _invocationList.GetActions(message);

            if (actions != null)
            {
                actions.ForEach(action => action.DynamicInvoke(parameter));
            }
        }

        public void NotifyColleagues(string message)
        {
            var actions = _invocationList.GetActions(message);

            if (actions != null)
            {
                actions.ForEach(action => action.DynamicInvoke());
            }
        }

        protected MessageToActionMap InvocationList
        {
            get
            {
                return _invocationList;
            }
        }
    }

    internal class MediatorRegistration : IDisposable
    {
        private MessageToActionMap _invocationList;
        private IEnumerable<KeyValuePair<string, WeakAction>> _actionRegistereds;

        public MediatorRegistration(MessageToActionMap invocationList, IEnumerable<KeyValuePair<string, WeakAction>> actionRegistereds)
        {
            _invocationList = invocationList;
            _actionRegistereds = actionRegistereds;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var actionRegistered in _actionRegistereds)
                _invocationList.RemoveAction(actionRegistered.Key, actionRegistered.Value);

            _actionRegistereds = null;
            _invocationList = null;
        }

        #endregion
    }
}
