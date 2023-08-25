using Repositories.Connections.Interfaces;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Repositories.Connections
{
    public class DbConnectionExtractBot : DbConnection, IDbConnectionExtractBot
    {
        protected IDbConnection DapperConnection { get; set; }
        public DbConnectionExtractBot(string stringConnection)
        {
            DapperConnection = new SqlConnection(stringConnection);
        }
        public override string ConnectionString { get; set; }

        public override string Database { get; }

        public override string DataSource { get; }

        public override string ServerVersion { get; }

        public override ConnectionState State => DapperConnection.State;


        public override void ChangeDatabase(string databaseName)
        {
            DapperConnection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            DapperConnection.Close();
        }

        public override void Open()
        {
            DapperConnection.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return (DbTransaction)DapperConnection.BeginTransaction();
        }

        protected override DbCommand CreateDbCommand()
        {
            return (DbCommand)DapperConnection.CreateCommand();
        }
    }
}

