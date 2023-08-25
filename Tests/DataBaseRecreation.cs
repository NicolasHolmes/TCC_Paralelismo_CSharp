using NUnit.Framework;
using System.Data.SqlClient;

namespace Tests
{
    [TestFixture]
    public class DataBaseRecreation
    {
        private string _connectionString = "Server=(localdb)\\SQLTCC;Integrated Security=true;";
        [Test]
        public void RecreateTables_Test()
        {
            using (SqlConnection masterConnection = new SqlConnection(_connectionString))
            {
                masterConnection.Open();

                // Criação do banco de dados TCC
                using (SqlCommand createDatabaseCommand = masterConnection.CreateCommand())
                {
                    createDatabaseCommand.CommandText = @"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TCC')
                BEGIN
                    CREATE DATABASE TCC;
                END";
                    createDatabaseCommand.ExecuteNonQuery();
                }
            }

            // Usar o banco de dados TCC a partir de agora
            using (SqlConnection connection = new SqlConnection(_connectionString + "Database=TCC;"))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        // Criação do banco de dados TCC
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TCC')
                            BEGIN
                                CREATE DATABASE TCC;
                            END";
                        command.ExecuteNonQuery();

                        // Usar o banco de dados TCC
                        command.CommandText = "USE [TCC];";
                        command.ExecuteNonQuery();

                        // Criação da tabela Legislaturas
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Legislaturas' AND type = 'U')
                            BEGIN
                                CREATE TABLE [dbo].[Legislaturas](
                                    [Id] [int] IDENTITY(1,1) NOT NULL,
                                    [IdEndpointLegislatura] [int] NOT NULL,
                                    [DataInicio] [varchar](50) NULL,
                                    [DataFim] [varchar](50) NULL,
                                    CONSTRAINT [PK_Legislaturas] PRIMARY KEY CLUSTERED 
                                    (
                                        [id] ASC
                                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                                ) ON [PRIMARY];
                            END";
                        command.ExecuteNonQuery();

                        // Criação da tabela Deputados
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Deputados' AND type = 'U')
                            BEGIN
                                CREATE TABLE [dbo].[Deputados](
                                    [Id] [int] IDENTITY(1,1) NOT NULL,
                                    [IdEndpointDeputado] [int] NOT NULL,
                                    [Nome] [varchar](100) NULL,
                                    [SiglaPartido] [varchar](15) NULL,
                                    [SiglaUf] [char](2) NULL,
                                    [LegislaturaId] [int] NULL,
                                    [Email] [varchar](100) NULL,
                                    [DataCriacao] [datetime] NULL,
                                    [DataAtualizacao] [datetime] NULL,
                                    [Deletado] [bit] NULL,
                                    CONSTRAINT [PK_Deputados] PRIMARY KEY CLUSTERED 
                                    (
                                        [Id] ASC
                                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                                ) ON [PRIMARY];
                            END";
                        command.ExecuteNonQuery();

                        // Criação da tabela DeputadosDetalhes
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DeputadosDetalhes' AND type = 'U')
                            BEGIN
                                CREATE TABLE [dbo].[DeputadosDetalhes](
	                                [Id] [int] IDENTITY(1,1) NOT NULL,
	                                [IdEndpointDeputado] [int] NOT NULL,
	                                [NomeCivil] [varchar](100) NULL,
	                                [Cpf] [varchar](15) NULL,
	                                [Sexo] [char](1) NULL,
	                                [DataNascimento] [date] NULL,
	                                [UfNascimento] [varchar](100) NULL,
	                                [MunicipioNascimento] [varchar](100) NULL,
	                                [Escolaridade] [varchar](100) NULL,
	                                [DataCriacao] [datetime] NULL,
	                                [DataAtualizacao] [datetime] NULL,
	                                [Deletado] [bit] NULL,
                                 CONSTRAINT [PK_DeputadosDetalhes] PRIMARY KEY CLUSTERED 
                                (
	                                [Id] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                                ) ON [PRIMARY];
                            END";
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
