using System;
using MEFedMVVM.ViewModelLocator;
using MEFedMVVM.Services.Contracts;
using Microsoft.Expression.Interactivity.Core;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.ComponentModel.Composition;

namespace MEFedMVVM.Services.CommonServices
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	[PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportService(ServiceType.Both, typeof(IVisualStateManager))]
    public class DefaultVisualStateManagerInvoker : GoToStateAction, IVisualStateManager
	{
		private readonly IList<string> _preInitStateHistory;

		public DefaultVisualStateManagerInvoker()
		{
			_preInitStateHistory = new List<string>();
		}

		#region IVisualStateManager Members

        public void GoToState(string stateName)
        {
            if (!IsInitialized)
            {
				_preInitStateHistory.Add(stateName);
                Debug.WriteLine("Could not attach to the Visual State Manager. Make sure you have the correct visual states in your XAML. This can also be the case where the view is not loaded yet. In that case ignore this message since the State will be applied as soon as the View is loaded");
            }
            else
            {
                InvokeState(stateName);
            }
        }

        #endregion

        public bool IsInitialized { get; set; }

        #region IContextAware Members
        public void InjectContext(object context)
        {
            var element = context as FrameworkElement;
            if (element != null)
            {
                RoutedEventHandler handler = null;
                handler = (sender, e) =>
                {
                    var root = (FrameworkElement)sender;
                    TryAttach(root);
                    element.Loaded -= handler;
                };
                element.Loaded += handler;
                TryAttach(element);
            }
        }
        private void TryAttach(FrameworkElement root)
        {
            if (VisualStateManager.GetVisualStateGroups(root).Count > 0) // check if the Visual States are defined in the root element
                AttachAndExecuteStateHistory(root);
            else
            {
                // if not then check if they are in the content (This is what Blend does, the Visual States are defined in the first element)
#if SILVERLIGHT
                var contentControlRoot = root as UserControl;
#else
                var contentControlRoot = root as ContentControl;
#endif

                if (contentControlRoot != null)
                {
                    var child = (FrameworkElement)contentControlRoot.Content;
                    if (child != null)
                    {
                        if (VisualStateManager.GetVisualStateGroups(child).Count > 0)
                            AttachAndExecuteStateHistory(child);
                    }
                }
            }
        }

        private void AttachAndExecuteStateHistory(FrameworkElement root)
        {
            Detach();
            Attach(root);
            IsInitialized = true;
            if (_preInitStateHistory.Count > 0)
            {
            	var stateGroups = VisualStateManager.GetVisualStateGroups(root);
            	var visited = new List<string>();
            	var statesInvoked = 0;
            	foreach (var state in _preInitStateHistory.Reverse())
            	{
            		var owningGroup = GetOwningStateGroup(state,stateGroups);
					if (null == owningGroup || !visited.Contains(owningGroup))
					{
						InvokeState(state);
						visited.Add(owningGroup);
						statesInvoked++;
					}
            	}
				_preInitStateHistory.Clear();
            	Debug.WriteLine
            		(string.Format
            		 	("Successfully attached to the Visual State Manager of {0} and executed the {1} last invoked state{2} in {3} group{4}",
            		 	 root.GetType().Name,
            		 	 statesInvoked,
            		 	 statesInvoked == 1 ? "" : "s",
            		 	 visited.Count,
            		 	 visited.Count == 1 ? "" : "s"));
            }
        }

		private string GetOwningStateGroup(string visualState, IEnumerable stateGroups)
		{
			foreach (var stateGroup in stateGroups)
			{
				var group = stateGroup as VisualStateGroup;
				if (null != group)
				{
					foreach (var state in group.States)
					{
						var childState = state as VisualState;
						if (null != childState && childState.Name.Equals(visualState))
						{
							return group.Name;
						}
					}
				}
			}
			return null;
		}

		private void InvokeState(string stateName)
		{
			try
			{
				UseTransitions = true;
				StateName = stateName;
				if (AssociatedObject == null)
				{
					Debug.WriteLine("Cannot invoke state since AssociatedObject is null");
				}
				else
				{
					LastStateExecuted = stateName;
				}
				Invoke(null);
			}
			catch (Exception e)
			{
				Debug.WriteLine("Could not invoke State " + stateName + " exception " + e);
			}
		}

		public string LastStateExecuted { get; private set; }

		#endregion
    }
}
