using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace MEFedMVVM.Common
{
    public static class ObservablePropertyChanged
    {
        /// <summary>
        /// returns a PropertyChangedSubscriber so taht you can hook to PropertyChanged
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source">The source that is the INotifyPropertyChanged</param>
        /// <param name="property">THe property to attach to</param>
        /// <returns>Returns the subscriber</returns>
        public static PropertyChangedSubscriber<TSource, TProperty>
            OnChanged<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> property)
            where TSource : class, INotifyPropertyChanged
        {
            return new PropertyChangedSubscriber<TSource, TProperty>(source, property);
        }

    	/// <summary>
    	/// returns a PropertyChangedSubscriber so taht you can hook to PropertyChanged
    	/// </summary>
    	/// <typeparam name="TSource"></typeparam>
    	/// <typeparam name="TProperty"></typeparam>
    	/// <param name="source">The source that is the INotifyPropertyChanged</param>
    	/// <param name="property">THe property to attach to</param>
    	/// <param name="updateOnlyWhenDirty">Specify false so that you get notified also when the property did not change the value but the Setter was called</param>
    	/// <returns>Returns the subscriber</returns>
    	public static PropertyChangedSubscriber<TSource, TProperty>
			OnChanged<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> property, bool updateOnlyWhenDirty)
			where TSource : class, INotifyPropertyChanged
		{
			return new PropertyChangedSubscriber<TSource, TProperty>(source, property, updateOnlyWhenDirty);
		}
    }

    /// <summary>
    /// Shortcut to subscribe to PropertyChanged on an INotfiyPropertyChanged and executes an action when that happens
    /// </summary>
    /// <typeparam name="TSource">Must implement INotifyPropertyChanged</typeparam>
    /// <typeparam name="TProperty">Can be any type</typeparam>
    public class PropertyChangedSubscriber<TSource, TProperty>
        : IDisposable where TSource : class, INotifyPropertyChanged
    {
        private readonly Expression<Func<TSource, TProperty>> _propertyValidation;
    	private readonly bool _updateOnlyWhenDirty;
    	private readonly TSource _source;
        private Action<TSource> _onChange;
    	private readonly PropertyInfo _propertyInfo;
    	private object _lastValue;

    	public PropertyChangedSubscriber(TSource source, Expression<Func<TSource, TProperty>> property, bool updateOnlyWhenDirty = true)
        {
			_propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
    		if (_propertyInfo == null)
    			throw new ArgumentException("The lambda expression 'property' should point to a valid Property");

    		_propertyValidation = property;
    		_updateOnlyWhenDirty = updateOnlyWhenDirty;
    		_source = source;
            source.PropertyChanged += SourcePropertyChanged;
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        	if (!IsPropertyValid(e.PropertyName)) 
				return;

			if (_updateOnlyWhenDirty)
			{
				var currentValue = GetCurrentValue(sender);
				if (Equals(_lastValue, currentValue))
					return;
				_lastValue = currentValue;
			}

        	_onChange(sender as TSource);
        }

    	private object GetCurrentValue(object source)
    	{
    		return source.GetType().GetProperty(_propertyInfo.Name).GetValue(source, null);
    	}

    	/// <summary>
        /// Executes the action and returns an IDisposable so that you can unregister 
        /// </summary>
        /// <param name="onChanged">The action to execute</param>
        /// <returns>The IDisposable so that you can unregister</returns>
        public IDisposable Do(Action<TSource> onChanged)
        {
            _onChange = onChanged;
            return this;
        }

        /// <summary>
        /// Executes the action only once and automatically unregisters
        /// </summary>
        /// <param name="onChanged">The action to be executed</param>
        public void DoOnce(Action<TSource> onChanged)
        {
            Action<TSource> dispose = x => Dispose();
            _onChange = (Action<TSource>)Delegate.Combine(onChanged, dispose);
        }

        private bool IsPropertyValid(string propertyName)
        {
			return _propertyInfo.Name == propertyName;
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Unregisters the property
        /// </summary>
        public void Dispose()
        {
            _source.PropertyChanged -= SourcePropertyChanged;
        }

        #endregion
    }
}
