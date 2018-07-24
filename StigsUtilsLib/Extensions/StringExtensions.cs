// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StigsUtilsLib.Exceptions;

namespace StigsUtilsLib.Extensions {

	//EASY: document.
	public static class StringExtensions {
		private static readonly JsonSerializerSettings DefaultJsonDeserializerSettings = new JsonSerializerSettings {
			Formatting = Formatting.None
		};

		//EASY: document and unit test.
		public static T FromJson<T>(this string @this, JsonSerializerSettings settings) => JsonConvert.DeserializeObject<T>(@this, settings ?? DefaultJsonDeserializerSettings);

		//EASY: unit test.
		/// <summary>
		///     Fluent assertion that @this is an existing file.
		/// </summary>
		/// <param name="this">A path as string.</param>
		/// <exception cref="ArgumentException">Thrown if @this is not an existing file.</exception>
		/// <returns>Returns @this if @this is a path to an existing file and else a System.ArgumentException is thrown.</returns>
		public static string AssertExistingFile(this string @this) {
			if (!File.Exists(@this)) throw new AssertExistingFileException($"{@this} (fullpath = {Path.GetFullPath(@this)}) is not the path of an existing file.");
			return @this;
		}

		//EASY: unit test.
		public static string AssertRelativePath(this string @this) {
			if (Path.IsPathRooted(@this)) throw new AssertRelativePathException("Path must be relative. but was " + @this);
			return @this;
		}

		//EASY: unit test.
		public static string FixPathSeparators(this string @this) {
			if (Path.AltDirectorySeparatorChar == Path.DirectorySeparatorChar) return @this;
			return @this.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}

		//EASY: unit test.
		/// <summary>
		///     Fluent assertion that @this is an existing directory.
		/// </summary>
		/// <param name="this">A path as string.</param>
		/// <exception cref="ArgumentException">Thrown if @this is not an existing directory.</exception>
		/// <returns>Returns @this if @this is a path to an existing directory and else a System.ArgumentException is thrown.</returns>
		public static string AssertExistingDirectory(this string @this) {
			if (!Directory.Exists(@this)) throw new AssertExistingDirectoryException($"{@this} (fullpath = {Path.GetFullPath(@this)}) is not the path of an existing directory.");
			return @this;
		}

		//EASY: document and unit test.
		public static void WriteAllText(this string @this, string content) {
			Directory.CreateDirectory(Path.GetDirectoryName(@this));
			File.WriteAllText(@this, content);
		}

		//EASY: document and unit test.
		public static void CopyTo(this string @this, string to) {
			Directory.CreateDirectory(Path.GetDirectoryName(to));
			File.Copy(@this, to);
		}

		//EASY: document and unit test.
		public static string CreateDirectoryIfNotExists(this string @this) {
			var directoryPath = Path.GetDirectoryName(@this);
			if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
			return @this;
		}

		//EASY: document and unit test.
		public static string DeleteAndCreateDirectory(this string @this) {
			if (Directory.Exists(@this)) Directory.Delete(@this, true);
			Directory.CreateDirectory(@this);
			return @this;
		}

		//EASY: document and unit test.
		public static string GetExtension(this string @this) => Path.GetExtension(@this);
		public static string SetExtension(this string @this, string newExtension) => Path.ChangeExtension(@this, newExtension);

		/// <summary>
		///     Get array of the parts of a filename separated by '.'.
		///     See unit test for examples.
		/// </summary>
		/// <param name="this"></param>
		/// <returns>Array of extension string.</returns>
		public static string[] FileNameParts(this string @this) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			return Path.GetFileName(@this).Split('.');
		}

		/// <summary>
		///     Get the rest of the string after the first occurence of the string in the split parameter.
		///     If the string does not contain the string in the split parameter the empty string is returned.
		/// </summary>
		/// <param name="this">The string to get a tail from.</param>
		/// <param name="split">The string to use for the split.</param>
		/// <param name="stringComparison">
		///     Determines how to compare string when searching for the split, defaults to
		///     StringComparison.CurrentCulture.
		/// </param>
		/// <exception cref="ArgumentNullException">Thrown if @this is null.</exception>
		/// <returns>The rest of the string afte the first occurrence of the split string.</returns>
		public static string Tail(this string @this, string split, StringComparison stringComparison = StringComparison.CurrentCulture) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			var index = @this.IndexOf(".", stringComparison);
			if (index == -1) return string.Empty;
			return @this.Substring(index + 1);
		}

		/// <summary>
		///     Maps the relative path to a path relative to the given targetRootDir.
		/// </summary>
		/// <param name="this"></param>
		/// <param name="targetRootDir">The new root directory to map the relative path to.</param>
		/// <exception cref="ArgumentException">thrown if @this or targetRootDir are null or if @this is an absolute path. </exception>
		/// <returns>The relative path mapped to the targetRootDir.</returns>
		public static string MapFilePath(this string @this, string targetRootDir) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			if (targetRootDir == null) throw new ArgumentNullException(nameof(targetRootDir));
			@this = Path.GetFullPath(@this);
			@this = @this.Substring(Path.GetPathRoot(@this).Length);
			targetRootDir = Path.GetFullPath(targetRootDir);
			var result = Path.GetFullPath(Path.Combine(targetRootDir, @this));
			return result;
		}

		public static void ShouldBeAFile(this string @this) {
			if (!File.Exists(@this)) throw new ArgumentException($"{@this} is not a file");
		}

		public static string TrimStart(this string @this, string s, bool throwOnNoMatch = false) {
			if (@this.StartsWith(s)) return @this.Substring(s.Length);
			if (throwOnNoMatch) throw new TrimStartException($"{@this} does not start with {s}");
			return @this;
		}

		/// <summary>
		///     Trims the given string from the end of this string, optionally allowing partial trimming.
		/// </summary>
		/// <param name="this">The string to trim from the end.</param>
		/// <param name="str">The string to trim from the end of this string.</param>
		/// <param name="allowPartialTrim">If false, only trim if this ends with the entire string give in the str parameter.</param>
		/// <returns>
		///     This string trimmed from the end if the the end of this matches str (wholly or partially depending on the
		///     allowPartialTrim parameter.
		/// </returns>
		public static string TrimEnd(this string @this, string str, bool allowPartialTrim = false) {
			if (@this == null) throw new ArgumentNullException(nameof(@this));
			if (str == null) throw new ArgumentNullException(nameof(str));
			if (@this.Length < str.Length) return @this;
			var j = @this.Length;
			for (var i = str.Length - 1; i >= 0; i--) {
				if (str[i] != @this[j - 1]) break;
				j--;
			}
			if (!allowPartialTrim && j != @this.Length - str.Length) return @this;
			return @this.Substring(0, j);
		}

		public static IEnumerable<string> AssertAllFilesExist(this IEnumerable<string> @this) {
			string[] files = @this as string[] ?? @this.ToArray();
			foreach (var file in files)
				if (!File.Exists(file))
					throw new AssertExistingFileException(Path.GetFullPath(file));
			return files;
		}
		public static IEnumerable<string> AssertAnyFileExists(this IEnumerable<string> @this) {
			string[] files = @this as string[] ?? @this.ToArray();
			foreach (var file in files)
				if (File.Exists(file))
					return files;
			throw new AssertExistingFileException($"None of the files exists: {string.Join(",", files)}");
		}

		public class AssertExistingDirectoryException : AssertException {
			public AssertExistingDirectoryException(string message) : base(message) { }
		}

		public class TrimStartException : Exception {
			public TrimStartException(string message) : base(message) { }
		}

		public class AssertExistingFileException : AssertException {
			public AssertExistingFileException(string message) : base(message) { }
		}

		public class AssertRelativePathException : AssertException {
			public AssertRelativePathException(string message) : base(message) { }
		}
	}

}