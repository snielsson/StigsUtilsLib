// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System.Collections.Generic;

namespace StigsUtilsLib.Extensions {
	public static class ListExtensions {
		public static IList<T> ClearAndAddRange<T>(this IList<T> @this, IList<T> content) {
			@this.Clear();
			foreach (var x in content) @this.Add(x);
			return @this;
		}
	}
}