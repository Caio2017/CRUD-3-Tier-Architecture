USE master
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = 'db_CRUD3Camadas')
	BEGIN
		ALTER DATABASE db_CRUD3Camadas SET SINGLE_USER WITH ROLLBACK IMMEDIATE
		DROP DATABASE db_CRUD3Camadas
	END
GO
CREATE DATABASE db_CRUD3Camadas
GO
USE db_CRUD3Camadas
GO
CREATE TABLE Cliente (
	Id	 INT PRIMARY KEY IDENTITY,
	Nome VARCHAR(100),
	Sexo CHAR(1),
	Telefone CHAR(11),
	Email VARCHAR(100),
	DataNascimento	DATE,
	LimiteCompra SMALLMONEY
)

GO
--Procedure Inserir
CREATE PROCEDURE spInserirCliente 
	@nome	VARCHAR(100),
	@sexo	CHAR(1),
	@tel	CHAR(11),
	@email	VARCHAR(100),
	@dtnasc	DATE,
	@limite	SMALLMONEY
AS
	BEGIN
		INSERT INTO Cliente (Nome, Sexo, Telefone, Email, DataNascimento, LimiteCompra)
			VALUES (@nome, @sexo, @tel, @email, @dtnasc, @limite)
			
		--SELECT @@IDENTITY AS RetornoID
	END
	
GO
--Procedure Alterar
CREATE PROCEDURE spAlterarCliente 
	@idCliente	INT,
	@nome	VARCHAR(100),
	@sexo	CHAR(1),
	@tel	CHAR(11),
	@email	VARCHAR(100),
	@dtnasc	DATE,
	@limite	SMALLMONEY
AS
	BEGIN
		UPDATE 
			Cliente 
		SET 
			Nome = @nome, Sexo = @sexo, Telefone = @tel, Email = @email, DataNascimento = @dtnasc, LimiteCompra = @limite
		WHERE Id = @idCliente
		
		--SELECT @idCliente AS RetornoId
	END
	
GO
--Procedure Deletar
CREATE PROCEDURE spExcluirCliente 
	@idCliente	INT
AS
	BEGIN
		DELETE FROM
			Cliente
		WHERE
			Id = @idCliente
	END
	
GO	
--Procedure Consultar por ID
CREATE PROCEDURE spConsultarClientePorID
	@idCliente INT
AS
	BEGIN
		SELECT * FROM 
			Cliente
		WHERE Id = @idCliente
		
		--SELECT @idCliente AS RetornoID
	END
	
GO
--Procedure Consultar por Nome
CREATE PROCEDURE spConsultarClientePorNome
	@nome VARCHAR(50)
AS
	BEGIN
		SELECT * FROM 
			Cliente
		WHERE Nome LIKE '%' + @nome + '%'
	END
	

--INSERTS
GO
EXEC spInserirCliente 'Joaquina', 'F', '11965748123', 'joaquina23@gmail.com', '24-06-1996', 1200.00
GO
EXEC spInserirCliente 'Bemaldo', 'M', '11988451420', 'bemao@gmail.com', '26-02-1995', 1250.00
