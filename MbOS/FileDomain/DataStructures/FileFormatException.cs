using System;
using System.Runtime.Serialization;

namespace MbOS.FileDomain.DataStructures {
	[Serializable]
	internal class FileFormatException : Exception {
		public FileFormatException() {
		}

		public FileFormatException(string message) : base(message) {
		}

		public FileFormatException(string message, Exception innerException) : base(message, innerException) {
		}

		protected FileFormatException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}