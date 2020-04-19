using System;
using System.Collections.Generic;

namespace Cloud.Core.Tests
{
    using Testing;
    using Xunit;

    [IsUnit]
    public class SignedAccessConfigTest
    {
        [Fact]
        public void Test_SignedAccessConfig()
        {
            var testPermissions = new List<AccessPermission> { AccessPermission.Write, AccessPermission.Read };
            var testExpiry = DateTimeOffset.UtcNow.AddDays(7);

            var testAccessConfig = new SignedAccessConfig(testPermissions, testExpiry);

            Assert.NotNull(testAccessConfig);
            Assert.Equal(testPermissions, testAccessConfig.AccessPermissions);
            Assert.Equal(testExpiry, testAccessConfig.AccessExpiry);
        }
    }
}
