using System;
using System.Collections.Generic;

namespace Cloud.Core.Tests
{
    using Testing;
    using Xunit;

    [IsUnit]
    public class SignedAccessConfigTest
    {
        /// <summary>Ensure the config object instantiating sets values in the expected way.</summary>
        [Fact]
        public void Test_SignedAccessConfig()
        {
            // Arrange
            var testPermissions = new List<AccessPermission> { AccessPermission.Write, AccessPermission.Read };
            var testExpiry = DateTimeOffset.UtcNow.AddDays(7);

            // Act
            var testAccessConfig = new SignedAccessConfig(testPermissions, testExpiry);

            // Assert
            Assert.NotNull(testAccessConfig);
            Assert.Equal(testPermissions, testAccessConfig.AccessPermissions);
            Assert.Equal(testExpiry, testAccessConfig.AccessExpiry);
        }
    }
}
