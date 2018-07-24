// Copyright 2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt file in root dir or https://opensource.org/licenses/MIT. 

using Newtonsoft.Json;

namespace StigsUtils.Extensions {
	public static class ObjectExtensions {
		private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings {
			Formatting = Formatting.None
		};
		private static readonly JsonSerializerSettings DefaultPrettyJsonSerializerSettings = new JsonSerializerSettings {
			Formatting = Formatting.Indented
		};
		public static string ToJson(this object @this, JsonSerializerSettings settings = null) => JsonConvert.SerializeObject(@this, settings ?? DefaultJsonSerializerSettings);
		public static string ToPrettyJson(this object @this, JsonSerializerSettings settings = null) => JsonConvert.SerializeObject(@this, Formatting.Indented, settings ?? DefaultPrettyJsonSerializerSettings);
	}
}