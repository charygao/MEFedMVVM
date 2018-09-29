using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MEFedMVVM.ValidationsExtensions
{
	public class DataValidationHandler<T>
	{
		private readonly IList<string> _dependantProperties = new List<string>();

		public bool ValidateAll(T objectToValidate)
		{
			if (_dependantProperties.Count == 0)
				return true;

			var context = objectToValidate as IDataErrorInfo;
			if (context == null)
			{
				Debug.WriteLine("DataValidationHandler: current context is not an IDataErrorInfo");
				return true;
			}

			return _dependantProperties.All(property => 
			{ 
				var isNullOrEmpty = String.IsNullOrEmpty(context[property]);
				if(!isNullOrEmpty)
					Debug.WriteLine(String.Format("Validation Error at: {0}, Error: {1}", property, context[property]));
				return isNullOrEmpty;
			});
		}

		public bool ValidateProperty<TProperty>(Expression<Func<TProperty>> property, T objectToValidate)
		{
			var context = objectToValidate as IDataErrorInfo;
			if (context == null)
			{
				Debug.WriteLine("DataValidationHandler: current context is not an IDataErrorInfo");
				return true;
			}
			var propertyName = GetPropertyName(property);
			var isNullOrEmpty = String.IsNullOrEmpty( context[propertyName] );
			if(!isNullOrEmpty)
				Debug.WriteLine(String.Format("Validation Error at: {0}, Error: {1}", propertyName, context[propertyName]));
			return isNullOrEmpty;
		}

		public DataValidationHandler<T> DependsOn<TProperty>(Expression<Func<TProperty>> property)
		{
			var propertyName = GetPropertyName(property);
			_dependantProperties.Add(propertyName);
			return this;
		}

		public DataValidationHandler<T> AndOn<TProperty>(Expression<Func<TProperty>> property)
		{
			return DependsOn(property);
		}

		public void ForcePropertyValidation<TProperty>(Expression<Func<TProperty>> property, T objectToValidate)
		{
			var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
			ApplyPropertyValue(propertyInfo, objectToValidate);
		}

		public void ForceAllValidations(T objectToValidate)
		{
			foreach (var propertyInfo in _dependantProperties.Select(property => typeof (T).GetProperty(property)))
				ApplyPropertyValue(propertyInfo, objectToValidate);
		}

		#region Helpers

		private static string GetPropertyName<TProperty>(Expression<Func<TProperty>> property)
		{
			var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
			}

			return propertyInfo.Name;
		}

		private static void ApplyPropertyValue(PropertyInfo propertyInfo, T objectToValidate)
		{
			if (propertyInfo != null)
				propertyInfo.SetValue(objectToValidate, propertyInfo.GetValue(objectToValidate, null), null);
		}

		#endregion
	}
}