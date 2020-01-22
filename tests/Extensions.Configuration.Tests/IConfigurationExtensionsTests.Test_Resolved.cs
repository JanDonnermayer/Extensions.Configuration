using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Extensions.Configuration.Tests
{
    public partial class IConfigurationExtensionsTests
    {
        public class Test_Resolved
        {
            private IConfiguration configurationMock;

            [SetUp]
            public void Setup()
            {
                this.configurationMock = Mock.Of<IConfiguration>();
            }
       

        }
    }
}