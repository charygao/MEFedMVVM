using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MEFedMVVM.Common;

namespace MEFedMVVM.ValidationsExtensions
{
    public class DataValidationDelegateCommand<T> : DelegateCommand<T>
    {
    	private readonly DataValidationHandler<T> _validationHandler;

    	public DataValidationDelegateCommand(Action<T> executeMethod)
			: this(executeMethod, null)
		{ }

    	public DataValidationDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) 
            : base(executeMethod, null)
        {
			_validationHandler = new DataValidationHandler<T>();
			if (canExecuteMethod == null)
				_canExecuteMethod = OnCanExecute;
			else
				_canExecuteMethod = x => canExecuteMethod(x) && OnCanExecute(x);
        }

    	private bool OnCanExecute(T arg)
    	{
			return _validationHandler.ValidateAll(arg);
    	}

    	public DataValidationDelegateCommand<T> DependsOn<TProperty>(Expression<Func<TProperty>> property)
        {
			_validationHandler.DependsOn(property);
            return this;
        }

        public DataValidationDelegateCommand<T> AndOn<TProperty>(Expression<Func<TProperty>> property)
        {
            return DependsOn(property);
        }
    }
}
