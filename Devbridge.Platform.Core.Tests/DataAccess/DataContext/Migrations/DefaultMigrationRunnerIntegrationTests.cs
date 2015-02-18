using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

using Autofac;

using Devbridge.Platform.Core.Configuration;
using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Modules;
using Devbridge.Platform.Sample.Module;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.DataAccess.DataContext.Migrations
{
    [TestFixture]
    public class DefaultMigrationRunnerIntegrationTests : TestBase
    {
        [Test]
        public void Should_Run_Sample_Module_Migration_Successfully()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var configuration = container.Resolve<IConfiguration>();
                var connectionStringName = configuration.Database.ConnectionStringName;
                Assert.IsNotNull(connectionStringName);

                var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
                Assert.IsNotNull(connectionString);

                using (var connection = new SqlConnection(connectionString.ConnectionString))
                {
                    connection.Open();
                    DropTablesIfSuchExist(connection);

                    var versionUpdateCount = 0;
                    var assemblyLoader = container.Resolve<IAssemblyLoader>();
                    var versionChecker = new Mock<IVersionChecker>();
                    versionChecker
                        .Setup(vc => vc.VersionExists(It.IsAny<string>(), It.IsAny<long>()))
                        .Returns<string, long>((s, l) => false);
                    versionChecker
                        .Setup(vc => vc.AddVersion(It.IsAny<string>(), It.IsAny<long>()))
                        .Callback<string, long>((s, l) => versionUpdateCount++);

                    var migrationRunner = new DefaultMigrationRunner(assemblyLoader, configuration, versionChecker.Object);
                    var descriptors = new List<ModuleDescriptor> { new SampleModuleDescriptor() };

                    migrationRunner.MigrateStructure(descriptors);

                    // Should run 2 migration scripts
                    Assert.AreEqual(versionUpdateCount, 2);

                    // Check, if tables re-created
                    CheckIfTablesExist(connection);

                    // Shouldn't run the same migrations second time
                    versionUpdateCount = 0;
                    versionChecker
                        .Setup(vc => vc.VersionExists(It.IsAny<string>(), It.IsAny<long>()))
                        .Returns<string, long>((s, l) => true);

                    migrationRunner.MigrateStructure(descriptors);

                    // Should run 0 migration scripts
                    Assert.AreEqual(versionUpdateCount, 0);

                    connection.Close();
                }
            }
        }

        private void CheckIfTablesExist(SqlConnection connection)
        {
            var command = new SqlCommand("SELECT TABLE_NAME FROM information_schema.tables", connection);
            var tables = new List<string>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var table = reader.GetString(0);
                        tables.Add(table);
                    }
                    reader.Close();
                }
            }

            Assert.AreEqual(tables.Count, 3);
            Assert.IsTrue(tables.Contains("VersionInfo"));
            Assert.IsTrue(tables.Contains("TestTable"));
            Assert.IsTrue(tables.Contains("TestTable2"));
        }

        private void DropTablesIfSuchExist(SqlConnection connection)
        {
            var command = new SqlCommand("DROP TABLE [module_dbsample].[VersionInfo]", connection);
            command.ExecuteNonQuery();

            command = new SqlCommand("DROP TABLE [module_dbsample].[TestTable]", connection);
            command.ExecuteNonQuery();

            command = new SqlCommand("DROP TABLE [module_dbsample].[TestTable2]", connection);
            command.ExecuteNonQuery();
        }
    }
}
