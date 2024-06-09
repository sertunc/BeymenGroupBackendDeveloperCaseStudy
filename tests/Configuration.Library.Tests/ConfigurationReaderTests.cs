using Configuration.Data.Abstractions.Interfaces;
using Configuration.Data.Models.Entities;
using Configuration.Library.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Configuration.Library.Tests
{
    public class ConfigurationReaderTests
    {
        [Fact]
        public void Constructor_WithNullConfigurationStorage_ThrowsArgumentNullException()
        {
            // Arrange
            IConfigurationStorage configurationStorage = null;
            int refreshTimerIntervalInMs = 1000;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConfigurationReader(configurationStorage, refreshTimerIntervalInMs));
        }

        [Fact]
        public void Constructor_WithZeroRefreshTimerInterval_ThrowsArgumentException()
        {
            var configurationStorageMock = new Mock<IConfigurationStorage>();
            int refreshTimerIntervalInMs = 0;

            Assert.Throws<ArgumentException>(() => new ConfigurationReader(configurationStorageMock.Object, refreshTimerIntervalInMs));
        }

        [Fact]
        public void GetValue_WithEmptyKey_ThrowsArgumentNullException()
        {
            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>());

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<ArgumentNullException>(() => configurationReader.GetValue<string>(""));
        }

        [Fact]
        public void GetValue_WithNullConfigList_ThrowsArgumentNullException()
        {
            var webSiteNameKey = "WebSiteName";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>());

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<InvalidOperationException>(() => configurationReader.GetValue<string>(webSiteNameKey));
        }

        [Fact]
        public void GetValue_WithNonExistingKey_ThrowsConfigNotFoundException()
        {
            var nonExistingKey = "NonExistingKey";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>
                {
                    new ConfigModel { Name = "TimerInterval", Type = typeof(int).FullName, Value = "60000" }
                });

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<ConfigNotFoundException>(() => configurationReader.GetValue<string>(nonExistingKey));
        }

        [Fact]
        public void GetValue_WithMismatchedType_ThrowsTypeMismatchException()
        {
            var key = "SomeKey";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>
                {
                    new ConfigModel { Name = key, Type = typeof(int).FullName, Value = "42" }
                });

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<TypeMismatchException>(() => configurationReader.GetValue<string>(key));
        }

        [Fact]
        public void GetValue_WithInvalidIntegerType_ThrowsInvalidCastException()
        {
            var key = "SomeKey";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>
                {
                    new ConfigModel { Name = key, Type = typeof(int).FullName, Value = "42a" }
                });

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<FormatException>(() => configurationReader.GetValue<int>(key));
        }

        [Fact]
        public void GetValue_WithInvalidBooleanType_ThrowsInvalidCastException()
        {
            var key = "SomeKey";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>
                {
                    new ConfigModel { Name = key, Type = typeof(bool).FullName, Value = "truea" }
                });

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<FormatException>(() => configurationReader.GetValue<bool>(key));
        }

        [Fact]
        public void GetValue_WithInvalidDoubleType_ThrowsInvalidCastException()
        {
            var key = "SomeKey";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>
                {
                    new ConfigModel { Name = key, Type = typeof(double).FullName, Value = "12,56a" }
                });

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<FormatException>(() => configurationReader.GetValue<double>(key));
        }

        [Fact]
        public void GetValue_WithInvalidTypeCode_ThrowsInvalidCastException()
        {
            var key = "SomeKey";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>
                {
                    new ConfigModel { Name = key, Type = typeof(DateTime).FullName, Value = "42" }
                });

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 1000);

            Assert.Throws<InvalidCastException>(() => configurationReader.GetValue<DateTime>(key));
        }

        [Fact]
        public void GetValue_WithExistingKey_ReturnsCorrectValue()
        {
            var key = "SomeKey";
            var expectedValue = "SomeValue";

            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.GetAllByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ConfigModel>
                {
                    new ConfigModel { Name = key, Type = typeof(string).FullName, Value = expectedValue }
                });

            var configurationReader = new ConfigurationReader(configurationStorageMock.Object, 5_000);

            var actualValue = configurationReader.GetValue<string>(key);

            Assert.Equal(expectedValue, actualValue);
        }
    }
}