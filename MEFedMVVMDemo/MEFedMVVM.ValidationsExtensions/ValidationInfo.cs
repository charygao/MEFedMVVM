using System;
using System.Windows.Controls;

namespace MEFedMVVM.ValidationsExtensions
{
	public class ValidationInfo
	{
		public object ErrorContent { get; private set; }

		public bool IsException { get; private set; }

		public Exception Exception { get; private set; }

		public ValidationInfo(ValidationError error)
		{
			ErrorContent = error.ErrorContent;
			Exception = error.Exception;
			IsException = error.Exception != null;
		}
	}

	public class ValidationInfoUpdate
	{
		public bool IsAdded { get; set; }
		public ValidationInfo ValidationInfo { get; set; }
	}
}