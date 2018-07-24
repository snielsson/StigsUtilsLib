// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using StigsUtilsLib.Extensions;

namespace StigsUtilsLib {

	/// <summary>
	///     Service calls wrapping file system opertations and providing own root of file system.
	/// </summary>
	public class FileService {
		private string _rootPath;
		public FileService(string rootPath) => RootPath = rootPath;
		public string RootPath {
			get => _rootPath;
			set => _rootPath = Path.GetFullPath(value);
		}

		public string ToFullPath(string path) {
			var result = Path.Combine(RootPath, path.FixPathSeparators().AssertRelativePath());
			Debug.Assert(Path.IsPathRooted(result), "Result must be a rooted path.");
			return result;
		}

		public IEnumerable<string> EnumerateFileSystemEntries(string path) {
			path = Path.Combine(RootPath, path.FixPathSeparators().AssertRelativePath());
			return Directory.EnumerateFileSystemEntries(path).Select(x => x.Substring(RootPath.Length+1));
		}

		public IEnumerable<string> EnumerateFiles(string path = "", string searchPattern = null, EnumerationOptions options = null) {
			path = Path.Combine(RootPath, path.FixPathSeparators().AssertRelativePath());
			options = options ?? new EnumerationOptions {
				RecurseSubdirectories = true,
				IgnoreInaccessible = false,
				MatchCasing = MatchCasing.PlatformDefault,
				ReturnSpecialDirectories = false,
			};
			return Directory.EnumerateFiles(path, searchPattern ?? "*", options).Select(x => x.Substring(RootPath.Length+1));
		}

		public bool DirectoryExists(string path) => Directory.Exists(Path.Combine(RootPath, path.AssertRelativePath()));
		public bool FileExists(params string[] paths) => paths.All(x=>File.Exists(Path.Combine(RootPath, x.AssertRelativePath())));

		public string SearchUp(string startDir, string filename) {
			var dir = Path.Combine(RootPath, startDir.AssertRelativePath());
			if (!Directory.Exists(dir)) throw new NotExistingDirectoryException($"{dir} is not an existing directory.");
			while (dir.Length >= RootPath.Length) {
				var path = Path.Combine(dir, filename);
				if (File.Exists(path)) return path.Substring(RootPath.Length + 1).FixPathSeparators();
				dir = Directory.GetParent(dir).FullName;
			}
			return null;
		}

		public string ReadAllText(string path, Encoding encoding = null) {
			path = Path.Combine(RootPath, path.AssertRelativePath());
			if (encoding == null) return File.ReadAllText(path);
			return File.ReadAllText(path, encoding);
		}

		public bool TryDeleteRootDir() {
			if (!Directory.Exists(RootPath)) return false;
			Directory.Delete(RootPath, true);
			return true;
		}

		public class NotExistingDirectoryException : Exception {
			public NotExistingDirectoryException(string message) : base(message) { }
		}
		public string ChangeExtension(string @from, string to) {
			return Path.ChangeExtension(@from, to);
		}

		public void CopyFile(string path, FileService target, bool overwrite = false) {
			File.Copy(Path.Combine(RootPath, path.AssertRelativePath()),Path.Combine(target.RootPath,path).CreateDirectoryIfNotExists(),overwrite);
		}
		public void WriteAllText(string path, string content) {
			path = Path.Combine(RootPath, path.AssertRelativePath()).CreateDirectoryIfNotExists();
			File.WriteAllText(path, content);
		}



		//EASY implement Glob
		public IEnumerable<FileInfo> Glob(string globPattern) {
			//use Microsoft.Extensions.FileSystemGlobbing
			throw new NotImplementedException();
		}

		//EASY implement Glob
		public IEnumerable<FileInfo> Glob(string startingDirectory, string globPattern) {
			//use Microsoft.Extensions.FileSystemGlobbing
			throw new NotImplementedException();
		}
		
	}

}