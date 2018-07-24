// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Xml.Serialization;

namespace StigsUtils {
	/// <summary>
	///     A caching factory to avoid memory leaks in the XmlSerializer class.
	///     See http://dotnetcodebox.blogspot.dk/2013/01/xmlserializer-class-may-result-in.html
	/// </summary>
	public static class CachingXmlSerializerFactory {
		private static readonly ConcurrentDictionary<string, XmlSerializer> Cache = new ConcurrentDictionary<string, XmlSerializer>();
		public static XmlSerializer Create(Type type, XmlRootAttribute root) {
			if (type == null) {
				throw new ArgumentNullException(nameof(type));
			}
			if (root == null) {
				throw new ArgumentNullException(nameof(root));
			}
			var key = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", type, root.ElementName);
			return Cache.GetOrAdd(key, _ => new XmlSerializer(type, root));
		}
		public static XmlSerializer Create<T>(XmlRootAttribute root) => Create(typeof(T), root);
		public static XmlSerializer Create<T>() => Create(typeof(T));
		public static XmlSerializer Create<T>(string defaultNamespace) => Create(typeof(T), defaultNamespace);
		public static XmlSerializer Create(Type type) => new XmlSerializer(type);
		public static XmlSerializer Create(Type type, string defaultNamespace) => new XmlSerializer(type, defaultNamespace);
	}
}