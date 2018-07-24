// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 
using System.IO;
using Shouldly;
using StigsConfigLib;
using StigsUtils.Extensions;
using Xunit;

namespace StigsUtils.Tests {
	public class FileServiceTests {
		[Fact]
		public void FileServiceConstructionWorks() {
			var fileService = new FileService("TestData/FileServiceTestData");
			Path.IsPathRooted(fileService.RootPath).ShouldBeTrue();
			fileService.RootPath.EndsWith("TestData\\FileServiceTestData").ShouldBeTrue();
		}

		[Fact]
		public void FileServiceSearchUpRequiresExistingDirAsStartDir() {
			var fileService = new FileService("TestData/FileServiceTestData");
			Should.Throw<FileService.NotExistingDirectoryException>(() => fileService.SearchUp("TestDirA/NonExisting", "fileA-1.txt"), "Should throw FileService.NotExistingDirectoryException when using non existing directory as start dir.");
			var existingFile = "TestDirA/TestDirA-C/fileA-B-2.txt";
			fileService.FileExists(existingFile).ShouldBeTrue();
			Should.Throw<FileService.NotExistingDirectoryException>(() => fileService.SearchUp(existingFile, "fileA-1.txt"), "Should throw FileService.NotExistingDirectoryException when using existing file as start dir.");
		}

		[Fact]
		public void FileServiceSearchUpWorks() {
			var fileService = new FileService("TestData/FileServiceTestData");
			fileService.SearchUp("TestDirA/TestDirA-B", "fileA-B-2.txt").ShouldBe("TestDirA\\TestDirA-B\\fileA-B-2.txt");
			fileService.SearchUp("TestDirA/TestDirA-B", "fileA-1.txt").ShouldBe("TestDirA\\fileA-1.txt");
			fileService.SearchUp("TestDirA/TestDirA-B", "file1.txt").ShouldBe("file1.txt");
			fileService.SearchUp("TestDirA/TestDirA-B", "non-existing").ShouldBeNull();
			Should.Throw<StringExtensions.AssertRelativePathException>(() => fileService.SearchUp("c:/bla", "bla"));
		}
	}

}