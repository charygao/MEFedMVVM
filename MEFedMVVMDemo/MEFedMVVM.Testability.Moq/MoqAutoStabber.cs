using System;
using Moq;

namespace MEFedMVVM.Testability.Moq
{
	public class MoqAutoStabber : AutoStabberBase
	{
		public Mock<T> GetMock<T>() where T : class
		{
			return Get<T>().ToMock();
		}

		#region Overrides of AutoStabberBase

		protected override object CreateStub(Type type)
		{
			return ((Mock) Activator.CreateInstance(typeof(Mock<>).MakeGenericType(type), MockBehavior.Loose )).Object ; 
		}

		#endregion
	}

	public static class MoqExtensions
	{
		public static Mock<T> ToMock<T>(this T mock)
			where T : class
		{
			return Mock.Get(mock);
		}
	}
}