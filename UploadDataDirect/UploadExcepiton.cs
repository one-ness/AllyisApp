using System;
using System.Runtime.Serialization;

namespace UploadDataDirect
{
	[Serializable]
	internal class UploadExcepiton : Exception
	{
		public UploadExcepiton()
		{
		}

		public UploadExcepiton(string message) : base(message)
		{
		}

		public UploadExcepiton(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UploadExcepiton(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}