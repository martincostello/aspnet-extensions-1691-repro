// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetExtensions1691Repro
{
    [Collection(HttpServerFixtureCollection.Name)]
    public class Repro : IDisposable
    {
        private readonly HttpServerFixture _fixture;

        public Repro(HttpServerFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> GetTestCases()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new object[] { false, i.ToString(), string.Empty};
            }

            yield return new object[] { true, "TheDolphin", "TheDolphin" };
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public async Task Can_Reload_Configuration_And_Get_Value(bool reload, string value, string expected)
        {
            if (reload)
            {
                _fixture.OverrideConfiguration("Echo", value);
            }

            using (var httpClient = new HttpClient())
            {
                Assert.Equal(expected, await httpClient.GetStringAsync("http://localhost:5000/"));
            }
        }

        public void Dispose()
            => _fixture?.ClearConfigurationOverrides();
    }
}
