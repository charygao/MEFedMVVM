using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Diagnostics.CodeAnalysis;

namespace MEFedMVVM.Common
{
	/// <summary>
	/// The DelegateCommand implements the <see cref="ICommand"/> interface where delegates can be attached for the
	/// <see cref="Execute"/> and <see cref="CanExecute"/> methods.
	/// </summary>
	/// <typeparam name="T">The Command parameter type.</typeparam>
	public class DelegateCommand<T> : ICommand
	{
		private Action<T> _executeMethod;
		protected Func<T, bool> _canExecuteMethod;
		private List<WeakReference> _canExecuteChangedHandlers;

		/// <summary>
		/// Initialize a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="executeMethod">The delegate that is executed when <see cref="Execute"/> is called on the command.</param>
		/// <remarks><see cref="CanExecute"/> always returns true.</remarks>
		public DelegateCommand(Action<T> executeMethod) : this(executeMethod, (T meth) => { return true; }) { }

		/// <summary>
		/// Initialize a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="executeMethod">The delegate that is executed when <see cref="Execute"/> is called on the command.</param>
		/// <param name="canExecuteMethod">The delegate to be called when <see cref="CanExecute"/> is called on the command.</param>
		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
		{
			_executeMethod = executeMethod;
			_canExecuteMethod = canExecuteMethod;
		}

		#region CanExecute
		/// <summary>
		/// Method executed to determine whether or not the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Information used by the command.</param>
		public bool CanExecute()
		{
			return CanExecute(null);
		}

		/// <summary>
		/// Method executed to determine whether or not the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Information used by the command.</param>
		/// <returns>Returns true if this command can be executed, false otherwise.</returns>
		public bool CanExecute(T parameter)
		{
			if (_canExecuteMethod == null) return true;
			return _canExecuteMethod(parameter);
		}

		/// <summary>
		/// Method executed to determine whether or not the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Information used by the command.</param>
		/// <returns>Returns true if this command can be executed, false otherwise.</returns>
		public bool CanExecute(object parameter)
		{
			var p = parameter == null ? default(T) : (T)parameter;
			return CanExecute(p);
		}
		#endregion

		#region Execute
		/// <summary>
		/// The method to be executed when the command is invoked.
		/// </summary>
		public void Execute()
		{
			Execute(null);
		}

		/// <summary>
		/// The method to be executed when the command is invoked.
		/// </summary>
		/// <param name="parameter">Information used by the command.</param>
		public void Execute(T parameter)
		{
			if (_executeMethod != null)
			{
				_executeMethod(parameter);
			}
		}

		/// <summary>
		/// The method to be executed when the command is invoked.
		/// </summary>
		/// <param name="parameter">Information used by the command.</param>
		public void Execute(object parameter)
		{
			var p = parameter == null ? default(T) : (T)parameter;
			Execute(p);
		}
		#endregion

#if SILVERLIGHT
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute. You must keep a hard
        /// reference to the handler to avoid garbage collection and unexpected results. See remarks for more information.
        /// </summary>
        /// <remarks>
        /// When subscribing to the <see cref="ICommand.CanExecuteChanged"/> event using 
        /// code (not when binding using XAML) will need to keep a hard reference to the event handler. This is to prevent 
        /// garbage collection of the event handler because the command implements the Weak Event pattern so it does not have
        /// a hard reference to this handler. An example implementation can be seen in the CompositeCommand and CommandBehaviorBase
        /// classes. In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, but if you do, you
        /// are responsible for maintaining the reference.
        /// </remarks>
        /// <example>
        /// The following code holds a reference to the event handler. The myEventHandlerReference value should be stored
        /// in an instance member to avoid it from being garbage collected.
        /// <code>
        /// EventHandler myEventHandlerReference = new EventHandler(this.OnCanExecuteChanged);
        /// command.CanExecuteChanged += myEventHandlerReference;
        /// </code>
        /// </example>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }
#else
		/// <summary>
		/// Occurs when changes occur that affect whether the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (_canExecuteMethod != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}

			remove
			{
				if (_canExecuteMethod != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}
#endif

		/// <summary>
		/// Raises the <see cref="CanExecuteChanged" /> event.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "The this keyword is used in the Silverlight version")]
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
			Justification = "This cannot be an event")]
		public void RaiseCanExecuteChanged()
		{
#if SILVERLIGHT
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
#else
			CommandManager.InvalidateRequerySuggested();
#endif
		}
	}
}
