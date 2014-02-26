using NDepth.Configuration.ConfigurationManager;
using NUnit.Framework;

namespace NDepth.Tests.Configuration.ConfigurationManagerTests
{
    [TestFixture]
    [Category("Configuration manager tests")]
    public class ConfigurationManagerTests
    {
        public class ConfigElement1
        {
            public string Name;
            public string Value;
        }

        public class ConfigElement2
        {
            public long Id;
            public string Value;
        }

        public class ConfigElement3
        {
            public long Id;
            public string Name;
            public string Value;
        }

        [Test]
        public void TestMetadata()
        {
            var config = new ConfigurationManager();
            Assert.IsNotNull(config);

            var node1 = config.Metadata.MetaDataConfigure<ConfigElement1>("Single");
            Assert.IsNotNull(node1);
            Assert.AreEqual(node1.Parent, config.Metadata);

            var node11 = node1.MetaDataConfigure<ConfigElement1>("Nested");
            Assert.IsNotNull(node11);
            Assert.AreEqual(node11.Parent, node1);

            var node12 = node1.MetaDataConfigure<ConfigElement2, long>(e => e.Id);
            Assert.IsNotNull(node12);
            Assert.AreEqual(node12.Parent, node1);

            var node13 = node1.MetaDataConfigure<ConfigElement3, long, string>(e => e.Id, e => e.Name);
            Assert.IsNotNull(node13);
            Assert.AreEqual(node13.Parent, node1);

            var node2 = config.Metadata.MetaDataConfigure<ConfigElement2, long>(e => e.Id);
            Assert.IsNotNull(node2);
            Assert.AreEqual(node2.Parent, config.Metadata);

            var node21 = node2.MetaDataConfigure<ConfigElement1>("Nested");
            Assert.IsNotNull(node21);
            Assert.AreEqual(node21.Parent, node2);

            var node22 = node2.MetaDataConfigure<ConfigElement2, long>(e => e.Id);
            Assert.IsNotNull(node22);
            Assert.AreEqual(node22.Parent, node2);

            var node23 = node2.MetaDataConfigure<ConfigElement3, long, string>(e => e.Id, e => e.Name);
            Assert.IsNotNull(node23);
            Assert.AreEqual(node23.Parent, node2);

            var node3 = config.Metadata.MetaDataConfigure<ConfigElement3, long, string>(e => e.Id, e => e.Name);
            Assert.IsNotNull(node3);
            Assert.AreEqual(node3.Parent, config.Metadata);

            var node31 = node3.MetaDataConfigure<ConfigElement1>("Nested");
            Assert.IsNotNull(node31);
            Assert.AreEqual(node31.Parent, node3);

            var node32 = node3.MetaDataConfigure<ConfigElement2, long>(e => e.Id);
            Assert.IsNotNull(node32);
            Assert.AreEqual(node32.Parent, node3);

            var node33 = node3.MetaDataConfigure<ConfigElement3, long, string>(e => e.Id, e => e.Name);
            Assert.IsNotNull(node33);
            Assert.AreEqual(node33.Parent, node3);
        }
    }
}