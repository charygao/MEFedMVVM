using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MEFedMVVM.Testability.Moq
{
	public abstract class AutoStabberBase : IEnumerable
	{
		readonly Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();

		public T Create<T>() where T : class
		{
			ConstructorInfo constructorInfo = typeof(T).GetConstructors().OrderByDescending(x => x.GetParameters().Length).First();
			object[] parameters = constructorInfo.GetParameters().Select(x => Get(x.ParameterType)).ToArray();
			return (T)constructorInfo.Invoke(parameters);
		}

		public void Add<TService, TInstance>(TInstance instance) where TService : TInstance
		{
			Add(typeof(TService), instance);
		}

		public void Add(Type t, object instance)
		{
			if (instance != null && !t.IsInstanceOfType(instance))
			{
				throw new ArgumentException("instance must be an instance of t: " + t.Name);
			}
			_dependencies[t] = instance;
		}

		public T Get<T>() where T : class
		{
			return (T)Get(typeof(T));
		}

		object Get(Type type)
		{
			if (!_dependencies.ContainsKey(type))
			{
				_dependencies[type] = CreateStub(type);
			}
			return _dependencies[type];
		}

		protected abstract object CreateStub(Type type);
		
		public IEnumerator GetEnumerator()
		{
			return _dependencies.Values.GetEnumerator();
		}
	}
}
