// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Runtime.Serialization;

namespace StigsUtils.Exceptions {
	public class AssertException : Exception {
		public AssertException() { }
		public AssertException(string message) : base(message) { }
		public AssertException(string message, Exception innerException) : base(message, innerException) { }
		protected AssertException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}