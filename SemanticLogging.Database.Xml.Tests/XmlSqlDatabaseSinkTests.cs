using System;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SemanticLogging.Database.Xml.Tests.Util;

namespace SemanticLogging.Database.Xml.Tests
{
    [TestClass]
    public class XmlSqlDatabaseSinkTests
    {
        [TestMethod]
        public void ShouldFailForNullInstance()
        {
            AssertEx.Throws<ArgumentNullException>(() => new XmlSqlDatabaseSink(null, "connstr", "table", "proc", Buffering.DefaultBufferingInterval, Buffering.DefaultBufferingCount, Buffering.DefaultMaxBufferSize, TimeSpan.Zero));
        }

        [TestMethod]
        public void ShouldFailForNullConnectionString()
        {
            AssertEx.Throws<ArgumentNullException>(() => new XmlSqlDatabaseSink("instance", null, "table", "proc", Buffering.DefaultBufferingInterval, Buffering.DefaultBufferingCount, Buffering.DefaultMaxBufferSize, TimeSpan.Zero));
        }

        [TestMethod]
        public void ShouldFailForNullTableName()
        {
            AssertEx.Throws<ArgumentNullException>(() => new XmlSqlDatabaseSink("instance", "connstr", null, "proc", Buffering.DefaultBufferingInterval, Buffering.DefaultBufferingCount, Buffering.DefaultMaxBufferSize, TimeSpan.Zero));
        }

        [TestMethod]
        public void ShouldFailForNullStoredProcedure()
        {
            AssertEx.Throws<ArgumentNullException>(() => new XmlSqlDatabaseSink("instance", "connstr", "table", null, Buffering.DefaultBufferingInterval, Buffering.DefaultBufferingCount, Buffering.DefaultMaxBufferSize, TimeSpan.Zero));
        }

        [TestMethod]
        public void ShouldValidateSqlConnectionString()
        {
            AssertEx.Throws<ArgumentException>(() => new XmlSqlDatabaseSink("instance", "invalid_connstr", "table", "proc", Buffering.DefaultBufferingInterval, Buffering.DefaultBufferingCount, Buffering.DefaultMaxBufferSize, TimeSpan.Zero));
        }
    }
}
