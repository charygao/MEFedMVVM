using System;
using MEFedMVVM.Common;

namespace MEFedMVVM.NavigationExtensions
{
	public class NavigationCommand<T> : DelegateCommand<T>, INavigationManagerProvider
	{
		/// <summary>
        /// Initialize a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">The delegate that is executed when <see cref="Execute"/> is called on the command.</param>
        /// <remarks><see cref="CanExecute"/> always returns true.</remarks>
        public NavigationCommand(Action<T> executeMethod) : base(executeMethod) { }

        /// <summary>
        /// Initialize a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">The delegate that is executed when <see cref="Execute"/> is called on the command.</param>
        /// <param name="canExecuteMethod">The delegate to be called when <see cref="CanExecute"/> is called on the command.</param>
		public NavigationCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
			: base(executeMethod, canExecuteMethod)
        { }

		/// <summary>
		/// Gets or sets the navigation manager used for the specific navigation
		/// </summary>
		public INavigationManager NavigationManager { get; set; }
	}
}