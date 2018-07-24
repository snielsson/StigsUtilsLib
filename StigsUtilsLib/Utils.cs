// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.IO;

namespace StigsUtilsLib {
	public static class Utils {
		//EASY: unit test.
		public static bool TryDeleteDirectory(string path, bool recursive = true, Action<Exception> exceptionCallback = null) {
			if (!Directory.Exists(path)) return false;
			try {
				Directory.Delete(path, recursive);
				return true;
			}
			catch (Exception ex) {
				exceptionCallback?.Invoke(ex);
				return false;
			}
		}
	}

}