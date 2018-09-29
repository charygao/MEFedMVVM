using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MEFedMVVM.Common;

namespace MEFedMVVM.ValidationsExtensions
{
    /// <summary>
    /// Base class for data validation entities
    /// </summary>
    public abstract class DataValidationBase : NotifyPropertyChangedBase, IDataErrorInfo
    {
        private readonly Dictionary<string, string> _errors =
                new Dictionary<string, string>();

        /// <summary>
        /// Adds an error and notifies the UI that there was an issue with one of the inputs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property that has an error</param>
        /// <param name="error">The error message</param>
        protected void AddError<T>(Expression<Func<T>> property, string error)
        {
            var propertyName = GetPropertyName(property);
            if (_errors.ContainsKey(propertyName))
                _errors[propertyName] = error;
            else
                _errors.Add(propertyName, error);
        }

        /// <summary>
        /// Removes an error for a specific property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        protected void RemoveError<T>(Expression<Func<T>> property)
        {
            var propertyName = GetPropertyName(property);
            if (!_errors.ContainsKey(propertyName))
                return;
            _errors.Remove(propertyName);
        }

        /// <summary>
        /// Clears all errors
        /// </summary>
        protected void ClearErrors()
        {
            var keys = (from x in _errors.Keys select x).ToList();
            foreach (var propertyName in keys)
            {
                _errors.Remove(propertyName);
            }

            _errors.Clear();
        }

        protected void ValidateAndNotifyPropertyChanged<T>(Expression<Func<T>> property, params PropertyValidation[] validations)
        {
            bool foundError = false;
            foreach (var validation in validations)
            {
                if (validation.IsInvalid.Invoke())
                {
                    AddError(property, validation.ErrorMessage);
                    foundError = true;
                    break;
                }
            }

            if (!foundError)
                RemoveError(property);

            OnPropertyChanged(property);
        }

        /// <summary>
        /// Gets a value that indicates whether the object has validation errors. 
        /// </summary>
        /// <returns>
        /// true if the object currently has validation errors; otherwise, false.
        /// </returns>
        public bool HasErrors
        {
            get
            {
                return this._errors.Count > 0;
            }
        }


        private static string GetPropertyName<T>(Expression<Func<T>> property)
        {
            var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            return propertyInfo.Name;
        }

        #region Implementation of IDataErrorInfo

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                    return _errors[columnName];

                return String.Empty;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error
        {
            get
            {
				throw new NotSupportedException();
            }
        }

        #endregion
    }

    public struct PropertyValidation
    {
    	private const string NoMessageDefined = "no message defined";
    	public Func<bool> IsInvalid { get; private set; }
        public string ErrorMessage { get; private set; }

        private PropertyValidation(Func<bool> isPropertyInvalidFunc, string errorMessage)
            : this()
        {
            IsInvalid = isPropertyInvalidFunc;
            ErrorMessage = errorMessage;
        }

        public static PropertyValidation Create(Func<bool> isPropertyInvalidFunc, string errorMessage = NoMessageDefined)
        {
            return new PropertyValidation(isPropertyInvalidFunc, errorMessage);
        }
    }
}