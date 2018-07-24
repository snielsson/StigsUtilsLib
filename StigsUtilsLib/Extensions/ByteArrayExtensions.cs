// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System.Text;

namespace StigsUtilsLib.Extensions {
	//EASY: unit test.
	/// <summary>
	///     Common extension methods for byte arrays.
	/// </summary>
	public static class ByteArrayExtensions {
		/// <summary>
		///     Converts a byte array to a string.
		/// </summary>
		/// <param name="this">The bytes to interpret as a string.</param>
		/// <param name="encoding">Encoding to use when interpreting bytes to string. Defaults to UTF8.</param>
		/// <returns>The bytes interpreted as a string for the given encoding.</returns>
		public static string AsString(this byte[] @this, Encoding encoding = null) {
			encoding = encoding ?? Encoding.UTF8;
			return encoding.GetString(@this);
		}
	}
}