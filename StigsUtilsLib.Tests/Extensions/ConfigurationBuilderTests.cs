// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using Microsoft.Extensions.Configuration;
using StigsConfigLib;
using Xunit;

namespace StigsUtils.Tests.Extensions {
	public sealed class ConfigurationBuilderTests {
		[Fact]
		public void Works() {
			ConfigurationService.Load<SomeConfiguration>();

		}

		public class SomeConfiguration : ConfigurationService<SomeConfiguration>
		{
		}


	}
}