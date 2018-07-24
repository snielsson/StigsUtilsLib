// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.Linq;
using StigsUtilsLib.Extensions;

namespace StigsUtilsLib {
	public static class Assert {
		public static IEnumerable<string> AllFilesExist(params string[] files) => files.AssertAllFilesExist();
		public static IEnumerable<string> AnyFileExists(params string[] files) => files.AssertAnyFileExists();
		public static IEnumerable<bool> Any(string exceptionMessage, params bool[] expressions) {
			if (!expressions.Any(x => x)) throw new AssertException(exceptionMessage ?? "All expressions are false.");
			return expressions;
		}
		public static IEnumerable<Func<bool>> Any(params Func<bool>[] expressions) {
			if (!expressions.Any(x => x())) throw new AssertException("All expressions are false.");
			return expressions;
		}
		public static IEnumerable<Func<T, bool>> Any<T>(T arg, params Func<T, bool>[] expressions) {
			if (!expressions.Any(x => x(arg))) throw new AssertException("All expressions are false.");
			return expressions;
		}
		public static IEnumerable<bool> All(params bool[] expressions) {
			for (var i = 0; i < expressions.Length; i++) {
				if (!expressions[i]) throw new AssertException($"Expression at index {i} is false.");
			}
			return expressions;
		}
		public static IEnumerable<Func<bool>> All(params Func<bool>[] expressions) {
			for (var i = 0; i < expressions.Length; i++) {
				if (!expressions[i]()) throw new AssertException($"Expression at index {i} is false.");
			}
			return expressions;
		}
		public static IEnumerable<Func<T, bool>> All<T>(T arg, params Func<T, bool>[] expressions) {
			for (var i = 0; i < expressions.Length; i++) {
				if (!expressions[i](arg)) throw new AssertException($"Expression at index {i} is false.");
			}
			return expressions;
		}
		public class AssertException : Exception {
			public AssertException() { }
			public AssertException(string message) : base(message) { }
		}
	}
}