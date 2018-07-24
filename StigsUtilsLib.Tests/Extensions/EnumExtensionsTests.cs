// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using Shouldly;
using StigsUtilsLib.Extensions;
using Xunit;

namespace StigsUtilsLib.Tests.Extensions {
	public class EnumExtensionsTests {

		public enum TestEnum
		{
			None,
			SomeVal,
			SomeOtherVal = 117
		}

		public enum TestEnum2 : long
		{
			None,
			SomeVal,
			SomeOtherVal = 117
		}

		[Fact]
		public void GetEnumValueWorksWithIntBasedEnum() {

			int result = typeof(TestEnum).GetEnumValue<int>("None");
			result.ShouldBe(0);
			result = typeof(TestEnum).GetEnumValue<int>("SomeVal");
			result.ShouldBe(1);
			result = typeof(TestEnum).GetEnumValue<int>("SomeOtherVal");
			result.ShouldBe(117);
			Should.Throw<InvalidCastException>(() => typeof(TestEnum).GetEnumValue<long>("SomeOtherVal"));
			Should.Throw<KeyNotFoundException>(() => typeof(TestEnum).GetEnumValue<int>("NoneExisting"));

		}

		[Fact]
		public void GetEnumValueWorksWithLongBasedEnum() {

			long result = typeof(TestEnum2).GetEnumValue<long>("None");
			result.ShouldBe(0);
			result = typeof(TestEnum2).GetEnumValue<long>("SomeVal");
			result.ShouldBe(1);
			result = typeof(TestEnum2).GetEnumValue<long>("SomeOtherVal");
			result.ShouldBe(117);
			Should.Throw<InvalidCastException>(() => typeof(TestEnum2).GetEnumValue<int>("SomeOtherVal"));
			Should.Throw<KeyNotFoundException>(() => typeof(TestEnum2).GetEnumValue<long>("NoneExisting"));

		}


	}
}