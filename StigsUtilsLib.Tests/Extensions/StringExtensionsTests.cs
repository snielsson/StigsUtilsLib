// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Shouldly;
using StigsUtils.Extensions;
using Xunit;

namespace StigsUtils.Tests.Extensions {

	public class StringExtensionsTests {
		[Fact]
		public void AssertExistingDirectoryWorks() {
			Should.Throw<ArgumentException>(() => ((string) null).AssertExistingDirectory());
			Should.Throw<ArgumentException>(() => string.Empty.AssertExistingDirectory());
			Should.Throw<ArgumentException>(() => "does not exist".AssertExistingDirectory());
			Should.Throw<ArgumentException>(() => "TestData/empty.txt"
				.AssertExistingFile()
				.AssertExistingDirectory());
			Should.NotThrow(() => "TestData".AssertExistingDirectory());
		}
		[Fact]
		public void AssertExistingFileWorks() {
			Should.Throw<ArgumentException>(() => ((string) null).AssertExistingFile());
			Should.Throw<ArgumentException>(() => string.Empty.AssertExistingFile());
			Should.Throw<ArgumentException>(() => "does not exist".AssertExistingFile());
			Should.NotThrow(() => "TestData/empty.txt".AssertExistingFile());
			Should.Throw<ArgumentException>(() => "TestData"
				.AssertExistingDirectory()
				.AssertExistingFile());
		}

		[Fact]
		public void FileNamePartsWorks() {
			Should.Throw<ArgumentNullException>(() => ((string) null).FileNameParts());
			"".FileNameParts().ShouldBeEmpty();
			".".FileNameParts().ShouldBe(new[] { "" });
			"a.".FileNameParts().ShouldBe(new[] { "a", "" });
			"a.b".FileNameParts().ShouldBe(new[] { "a", "b" });
			"a.b.c".FileNameParts().ShouldBe(new[] { "a", "b", "c"});
			"a.b.c.".FileNameParts().ShouldBe(new[] { "a", "b", "c", ""});
			"dir/a.b.c.".FileNameParts().ShouldBe(new[] { "a", "b", "c", ""});
			"dir\\a.b.c.".FileNameParts().ShouldBe(new[] { "a", "b", "c", ""});
			"dir.dir/a.b.c.".FileNameParts().ShouldBe(new[] { "a", "b", "c", ""});
			"dir.dir\\a.b.c.".FileNameParts().ShouldBe(new[] { "a", "b", "c", ""});
		}

		[Fact]
		public void MapFilePathWorks() {
			Should.Throw<ArgumentNullException>(() => ((string) null).MapFilePath(null));
			Should.Throw<ArgumentNullException>(() => ((string) null).MapFilePath("c:/"));
			Should.Throw<ArgumentNullException>(() => ".".MapFilePath(null));
			"c:/test.txt".MapFilePath("c:/target").ShouldBe("c:\\target\\test.txt");
			"c:/a/b/c/../d/test.txt".MapFilePath("c:/target").ShouldBe("c:\\target\\a\\b\\d\\test.txt");
			"c:/test.txt".MapFilePath("c:/target").ShouldBe("c:\\target\\test.txt");
			"c:/test/".MapFilePath("c:/target").ShouldBe("c:\\target\\test\\");
			"c:/.".MapFilePath("c:/target").ShouldBe("c:\\target");
			"c:/..".MapFilePath("c:/target").ShouldBe("c:\\target");
			var cwd = Environment.CurrentDirectory;
			"c:/a/b/c/../d/test.txt".MapFilePath(".").ShouldBe(cwd + "\\a\\b\\d\\test.txt");
		}

		[Fact]
		public void TailWorks() {
			Should.Throw<ArgumentNullException>(() => ((string) null).Tail(".").ShouldBe(null));
			string.Empty.Tail(".").ShouldBe(string.Empty);
			".".Tail(".").ShouldBe("");
			"a.".Tail(".").ShouldBe("");
			"a.b".Tail(".").ShouldBe("b");
			"a.b.c".Tail(".").ShouldBe("b.c");
			"a.b.c.".Tail(".").ShouldBe("b.c.");
			".a.b.c.".Tail(".").ShouldBe("a.b.c.");
		}

		[Fact]
		public void TrimEndWorks() {
			Should.Throw<ArgumentNullException>(() => ((string)null).TrimEnd("_").ShouldBe(null),"Cannot invoke on null");
			Should.Throw<ArgumentNullException>(() => "_".TrimEnd((string)null).ShouldBe(null));
			"".TrimEnd("xyz").ShouldBe("", "Should not trim anything on empty string.");
			"xyz".TrimEnd("").ShouldBe("xyz", "Should not trim anything because str is empty.");
			"1abcdefg".TrimEnd("xyz").ShouldBe("1abcdefg", "Should not trim anything");
			"2abcdefg".TrimEnd("efg").ShouldBe("2abcd");
			"3abcdefg".TrimEnd("exfg").ShouldBe("3abcdefg", "Should not trim partially.");
			"4abcdefg".TrimEnd("exfg",true).ShouldBe("4abcde", "Should trim partially");
		}
	}
}