// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using Bynder.Sdk.Api.Converters;
using Xunit;

namespace Bynder.Test.Api.Converters
{
    public class DateTimeOffsetConverterTest
    {
        [Fact]
        public void CanConvertOnlyWhenTypeIsDateTimeOffset()
        {
            DateTimeOffsetConverter converter = new DateTimeOffsetConverter();
            Assert.False(converter.CanConvert(typeof(int)));
            Assert.False(converter.CanConvert(typeof(string)));
            Assert.False(converter.CanConvert(typeof(bool)));
            Assert.True(converter.CanConvert(typeof(DateTimeOffset)));
        }

        [Theory]
        [InlineData("1000-01-01", "1000-01-01T00:00:00Z")]
        [InlineData("2002-02-02", "2002-02-02T00:00:00Z")]
        [InlineData(null, "")]
        public void ConvertReturnsStringWithDate(string input, string expected)
        {
            DateTimeOffsetConverter converter = new DateTimeOffsetConverter();
            var date = converter.Convert(string.IsNullOrEmpty(input) ? null : DateTimeOffset.Parse(input));
            Assert.Equal(expected, date);
        }
    }
}
