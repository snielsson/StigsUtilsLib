// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace StigsUtils.Extensions {
	public static class EnumExtensions {
		private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> EnumValues = new ConcurrentDictionary<Type, Dictionary<string, object>>();
		public static T GetEnumValue<T>(this Type @this, string name) {
			Dictionary<string, object> keyValueMap = EnumValues.GetOrAdd(@this, enumType => {
				string[] keys = Enum.GetNames(enumType);
				var values = Enum.GetValues(enumType);
				var map = new Dictionary<string, object>();
				for (var i = 0; i < keys.Length; i++) {
					map[keys[i]] = values.GetValue(i);
				}
				return map;
			});
			return (T) keyValueMap[name];
		}

		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> @this) {
			return @this.Where(x => x != null);
		}
	}
}