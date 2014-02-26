using System;
using FluentMigrator;

namespace NDepth.Database.Migrations
{
    public static class MigrationExtensions
    {
        public static bool IsProduction(this Migration migration)
        {
            return IsEnvironmentVariableSet("ProductionProfile");
        }

        public static bool IsStaging(this Migration migration)
        {
            return IsEnvironmentVariableSet("StagingProfile");
        }

        public static bool IsDevelopment(this Migration migration)
        {
            return IsEnvironmentVariableSet("DevelopmentProfile");
        }

        public static bool IsTesting(this Migration migration)
        {
            return IsEnvironmentVariableSet("TestingProfile");
        }

        public static bool IsSchemaOnly(this Migration migration)
        {
            return (!IsProduction(migration) && !IsStaging(migration) && !IsDevelopment(migration) && !IsTesting(migration));
        }

        #region Utility methods

        private static bool IsEnvironmentVariableSet(string name)
        {
            var variable = Environment.GetEnvironmentVariable(name);
            return ((string.Compare(variable, "1", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(variable, "true", StringComparison.OrdinalIgnoreCase) == 0));
        }

        #endregion
    }
}
