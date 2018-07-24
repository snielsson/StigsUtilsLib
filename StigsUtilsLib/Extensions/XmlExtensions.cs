// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace StigsUtilsLib.Extensions {

/*

Add these extension methods:

public static class XmlExtensions { ///

/// Deserialize XML stream, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter. /// public static T DeserializeXml(this Stream @this, string innerStartTag = null) { if (innerStartTag == null) { var xmlSerializer = CachingXmlSerializerFactory.Create(typeof (T)); using (var reader = XmlReader.Create(@this)) { return (T) xmlSerializer.Deserialize(reader); } } else { var xmlSerializer = CachingXmlSerializerFactory.Create(typeof (T), new XmlRootAttribute(innerStartTag)); using (var reader = XmlReader.Create(@this)) { reader.ReadToDescendant(innerStartTag); return (T) xmlSerializer.Deserialize(reader.ReadSubtree()); } } }
    /// <summary>
    ///     Deserialize XML string, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter.
    /// </summary>

    public static T DeserializeXml<T>(this string @this, string innerStartTag = null) {
        using (var stringReader = new StringReader(@this)) {
            using (var xmlReader = XmlReader.Create(stringReader)) {
                if (innerStartTag != null) {
                    xmlReader.ReadToDescendant(innerStartTag);
                    var xmlSerializer = CachingXmlSerializerFactory.Create(typeof (T), new XmlRootAttribute(innerStartTag));
                    return (T) xmlSerializer.Deserialize(xmlReader.ReadSubtree());
                }
                return (T) CachingXmlSerializerFactory.Create(typeof (T), new XmlRootAttribute("AutochartistAPI")).Deserialize(xmlReader);
            }
        }
    }
}

/// <summary>
///     A caching factory to avoid memory leaks in the XmlSerializer class.
/// See http://dotnetcodebox.blogspot.dk/2013/01/xmlserializer-class-may-result-in.html
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
    public static XmlSerializer Create<T>(XmlRootAttribute root) {
        return Create(typeof (T), root);
    }
    public static XmlSerializer Create<T>() {
        return Create(typeof (T));
    }
    public static XmlSerializer Create<T>(string defaultNamespace) {
        return Create(typeof (T), defaultNamespace);
    }
    public static XmlSerializer Create(Type type) {
        return new XmlSerializer(type);
    }
    public static XmlSerializer Create(Type type, string defaultNamespace) {
        return new XmlSerializer(type, defaultNamespace);
    }
}


Add these extension methods:

public static class XmlExtensions { ///

/// Deserialize XML stream, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter. /// public static T DeserializeXml(this Stream @this, string innerStartTag = null) { if (innerStartTag == null) { var xmlSerializer = CachingXmlSerializerFactory.Create(typeof (T)); using (var reader = XmlReader.Create(@this)) { return (T) xmlSerializer.Deserialize(reader); } } else { var xmlSerializer = CachingXmlSerializerFactory.Create(typeof (T), new XmlRootAttribute(innerStartTag)); using (var reader = XmlReader.Create(@this)) { reader.ReadToDescendant(innerStartTag); return (T) xmlSerializer.Deserialize(reader.ReadSubtree()); } } }
    /// <summary>
    ///     Deserialize XML string, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter.
    /// </summary>
    public static T DeserializeXml<T>(this string @this, string innerStartTag = null) {
        using (var stringReader = new StringReader(@this)) {
            using (var xmlReader = XmlReader.Create(stringReader)) {
                if (innerStartTag != null) {
                    xmlReader.ReadToDescendant(innerStartTag);
                    var xmlSerializer = CachingXmlSerializerFactory.Create(typeof (T), new XmlRootAttribute(innerStartTag));
                    return (T) xmlSerializer.Deserialize(xmlReader.ReadSubtree());
                }
                return (T) CachingXmlSerializerFactory.Create(typeof (T), new XmlRootAttribute("AutochartistAPI")).Deserialize(xmlReader);
            }
        }
    }
}

/// <summary>
///     A caching factory to avoid memory leaks in the XmlSerializer class.
/// See http://dotnetcodebox.blogspot.dk/2013/01/xmlserializer-class-may-result-in.html
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
    public static XmlSerializer Create<T>(XmlRootAttribute root) {
        return Create(typeof (T), root);
    }
    public static XmlSerializer Create<T>() {
        return Create(typeof (T));
    }
    public static XmlSerializer Create<T>(string defaultNamespace) {
        return Create(typeof (T), defaultNamespace);
    }
    public static XmlSerializer Create(Type type) {
        return new XmlSerializer(type);
    }
    public static XmlSerializer Create(Type type, string defaultNamespace) {
        return new XmlSerializer(type, defaultNamespace);
    }
}

*/


	public static class XmlExtensions {
		public static string ToXml<T>(this T @this, string rootTag, string defaultNamespace, IEnumerable<(string prefix, string ns)> namespaces = null) {
			using (var stream = new MemoryStream()) {
				var root = new XmlRootAttribute(rootTag) {
					Namespace = defaultNamespace
				};
				var xmlSerializer = CachingXmlSerializerFactory.Create(typeof(T), root);
				if (namespaces != null) {
					var ns = new XmlSerializerNamespaces();
					foreach (var tuple in namespaces) ns.Add(tuple.prefix, tuple.ns);
					xmlSerializer.Serialize(stream, @this, ns);
				} else xmlSerializer.Serialize(stream, @this);
				stream.Position = 0;
				using (var reader = new StreamReader(stream)) return reader.ReadToEnd();
			}
		}

		//SAMPLECLASS
		//public class ApplicationManifestTag {
		//	public ApplicationTag Application { get; set; }
		//	public InstrumentationTag Instrumentation { get; set; }
		//	public class ApplicationTag {
		//		[XmlAttribute("ciName")]
		//		public string CiName { get; set; }
		//		public WindowsServiceTag WindowsService { get; set; }
		//		public class WindowsServiceTag {
		//			public string ServiceName { get; set; }
		//		}
		//	}
		//	public class InstrumentationTag {
		//		public EventTargetTag EventTarget { get; set; }
		//		[XmlElement("Event")]
		//		public EventTag[] Events { get; set; }

		//		public class EventTargetTag {
		//			[XmlAttribute("eventLog")]
		//			public string EventLog { get; set; }
		//			public string EventSource { get; set; }
		//		}

		//		public class EventTag {
		//			[XmlAttribute("id")]
		//			public int Id { get; set; }
		//			[XmlAttribute("level")]
		//			public string Level { get; set; }
		//			public KnowledgebaseTag Knowledgebase { get; set; }
		//			public class KnowledgebaseTag {
		//				public string Summary { get; set; }
		//				public string Cause { get; set; }
		//				public string Resolutions { get; set; }
		//			}
		//		}
		//	}
		//}


		/// <summary>
		///     Deserialize XML string, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter.
		/// </summary>
		public static T FromXml<T>(this XmlReader @this, string rootTag, string defaultNamespace = "") {
			var root = new XmlRootAttribute(rootTag) {
				Namespace = defaultNamespace
			};
			var xmlSerializer = CachingXmlSerializerFactory.Create(typeof(T), root);
			return (T) xmlSerializer.Deserialize(@this);
		}
		/// <summary>
		///     Deserialize XML string, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter.
		/// </summary>
		public static T FromXml<T>(this string @this, string rootTag, string defaultNamespace = "") {
			using (var stringReader = new StringReader(@this)) return XmlReader.Create(stringReader).FromXml<T>(rootTag, defaultNamespace);
		}
		public static T FromXml<T>(this Stream @this, string rootTag, string defaultNameSpace = "") => XmlReader.Create(@this).FromXml<T>(rootTag, defaultNameSpace);
	}
}