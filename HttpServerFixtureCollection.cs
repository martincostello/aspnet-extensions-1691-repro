// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Xunit;

namespace AspNetExtensions1691Repro
{
    [CollectionDefinition(Name)]
    public sealed class HttpServerFixtureCollection : ICollectionFixture<HttpServerFixture>
    {
        public const string Name = "http";
    }
}
