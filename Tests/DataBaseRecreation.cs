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

                        // Criação da tabela DetalhesProdutos
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DetalhesProdutos' AND type = 'U')
                            BEGIN
                                CREATE TABLE [dbo].[DetalhesProdutos](
	                                [Id] [int] IDENTITY(1,1) NOT NULL,
	                                [Name] [varchar](50) NOT NULL,
	                                [Description] [varchar](50) NULL,
	                                [Price] [decimal](5, 2) NOT NULL,
	                                [ExpirationDate] [date] NULL,
	                                [BarCode] [bigint] NOT NULL,
	                                [StockQuantity] [int] NOT NULL,
	                                PRIMARY KEY CLUSTERED 
	                                (
		                                [Id] ASC
	                                )
                                ) ON [PRIMARY];
                            END";
                        command.ExecuteNonQuery();

                        // Criação da tabela Produtos
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Produtos' AND type = 'U')
                            BEGIN
                                CREATE TABLE [dbo].[Produtos](
	                                [Id] [int] IDENTITY(1,1) NOT NULL,
	                                [Name] [nvarchar](max) NULL,
	                                [StockQuantity] [int] NOT NULL,
	                                PRIMARY KEY CLUSTERED 
	                                (
		                                [Id] ASC
	                                )
                                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
                            END";
                        command.ExecuteNonQuery();

                        // Criação da tabela DetalhesProdutosVindosDaAPI
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DetalhesProdutosVindosDaAPI' AND type = 'U')
                            BEGIN
                                CREATE TABLE [dbo].[DetalhesProdutosVindosDaAPI](
                                    [Id] [int] IDENTITY(1,1) NOT NULL,
                                    [IdEndpointProduct] [int] NOT NULL,
                                    [Name] [varchar](50) NOT NULL,
                                    [Description] [varchar](50) NULL,
                                    [Price] [decimal](4, 2) NOT NULL,
                                    [ExpirationDate] [date] NULL,
                                    [BarCode] [bigint] NOT NULL,
                                    [StockQuantity] [int] NOT NULL,
                                    [CreationDate] [datetime] NOT NULL,
                                    [TypeOfExtraction] [varchar](50) NOT NULL,
                                    [RequestsQuantity] [int] NOT NULL,
                                    [TimesItRan] [int] NOT NULL DEFAULT(0),
                                    PRIMARY KEY CLUSTERED 
                                    (
                                        [Id] ASC
                                    )
                                ) ON [PRIMARY];
                            END";
                        command.ExecuteNonQuery();

                        // Criação da tabela ProdutosVindosDaAPI
                        command.CommandText = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProdutosVindosDaAPI' AND type = 'U')
                            BEGIN
                                CREATE TABLE [dbo].[ProdutosVindosDaAPI](
	                                [Id] [int] IDENTITY(1,1) NOT NULL,
	                                [IdEndpointProduct] [int] NOT NULL,
	                                [Name] [varchar](50) NULL,
	                                [StockQuantity] [int] NULL,
	                                PRIMARY KEY CLUSTERED 
	                                (
		                                [Id] ASC
	                                )
                                ) ON [PRIMARY];
                            END";
                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
            }
        }
        [Test]
        public void PopulatingTables_Test()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString + "Database=TCC;"))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    // Populando a tabela Produtos
                    command.CommandText = @"
                        DECLARE @BatchSize INT = 100; --Número de linhas por lote
                        DECLARE @TotalRows INT = 100; --Total de linhas a serem inseridas
                        DECLARE @CurrentRow INT = 1;

                        BEGIN TRANSACTION; -- Iniciar a primeira transação

                        WHILE @CurrentRow <= @TotalRows
                        BEGIN
                            INSERT INTO Produtos(Name, StockQuantity)
                            VALUES
                                ('Produto ' + CAST(@CurrentRow AS NVARCHAR(10)),
                                 ABS(CHECKSUM(NEWID()) % 100)); --Gere um valor de estoque aleatório

                            SET @CurrentRow = @CurrentRow + 1;

                            IF @CurrentRow % @BatchSize = 0
                            BEGIN
                                -- Commita o lote anterior
                                COMMIT;
                                -- Inicia uma nova transação
                                BEGIN TRANSACTION;
                            END
                        END

                        COMMIT; -- Commit final para garantir que todas as linhas sejam confirmadas
                    ";
                    command.ExecuteNonQuery();

                    // Populando a tabela DetalhesProdutos com os registros
                    command.CommandText = @"
                        -- Habilitar a inserção em massa
                        SET NOCOUNT ON;
                        -- Declarar variáveis
                        DECLARE @Counter INT = 1;
                        -- Limites das datas
                        DECLARE @MinDate DATE = '2023-01-01';
                        DECLARE @MaxDate DATE = '2100-12-31';
                        -- Loop para inserir as linhas
                        WHILE @Counter <= 100
                        BEGIN
                            -- Gerar dados aleatórios
                            DECLARE @RandomName VARCHAR(50) = 'Produto ' + CAST(@Counter AS VARCHAR(10));
                            DECLARE @RandomDescription VARCHAR(50) = 'Descrição ' + CAST(@Counter AS VARCHAR(10));
                            DECLARE @RandomPrice DECIMAL(5, 2) = CAST(RAND() * 0.99 AS DECIMAL(5, 2));
                            DECLARE @RandomExpirationDate DATE = DATEADD(DAY, CAST(RAND() * DATEDIFF(DAY, @MinDate, @MaxDate) AS INT), @MinDate);
                            DECLARE @RandomBarCode BIGINT = CAST(RAND() * (99999999999 - 11111111111) + 11111111111 AS BIGINT);
                            DECLARE @RandomStockQuantity INT = CAST(RAND() * 1000 AS INT);
                            -- Verificar se o preço não é nulo
                            IF @RandomPrice IS NULL
                            BEGIN
                                SET @RandomPrice = 0.00;
                            END
                            -- Inserir dados na tabela
                            INSERT INTO [DetalhesProdutos] ([Name], [Description], [Price], [ExpirationDate], [BarCode], [StockQuantity])
                            VALUES (@RandomName, @RandomDescription, @RandomPrice, @RandomExpirationDate, @RandomBarCode, @RandomStockQuantity);
                            -- Incrementar o contador
                            SET @Counter = @Counter + 1;
                        END
                        -- Desabilitar a inserção em massa
                        SET NOCOUNT OFF;
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
