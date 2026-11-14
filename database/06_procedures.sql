USE GerenciadorTarefas;
GO

-- Cadastra uma nova tarefa
CREATE PROCEDURE sp_InserirTarefa
	@Titulo NVARCHAR(200),
	@Descricao NVARCHAR(1000),
	@DataVencimento DATETIME2,
	@IdCategoria UNIQUEIDENTIFIER
AS
BEGIN
	-- O título é obrigatório
	IF @Titulo IS NULL OR LTRIM(@Titulo) = ''
	BEGIN
		RAISERROR(N'O título da tarefa é obrigatório.', 16, 1);
		RETURN;
	END

	-- Toda tarefa começa como Pendente (Status = 0)
	INSERT INTO Tarefas (Titulo, Descricao, DataVencimento, Status, FK_IdCategoria)
	VALUES (@Titulo, @Descricao, @DataVencimento, 0, @IdCategoria);
END;
GO

-- Adiciona um participante em uma tarefa
CREATE PROCEDURE sp_AdicionarParticipante
	@IdTarefa UNIQUEIDENTIFIER,
	@IdParticipante UNIQUEIDENTIFIER
AS
BEGIN
	-- Não adiciona o mesmo participante duas vezes
	IF NOT EXISTS (SELECT 1 FROM TarefasParticipantes
		WHERE FK_IdTarefa = @IdTarefa AND FK_IdParticipante = @IdParticipante)
	BEGIN
		INSERT INTO TarefasParticipantes (FK_IdTarefa, FK_IdParticipante)
		VALUES (@IdTarefa, @IdParticipante);
	END
END;
GO

-- Muda o status de uma tarefa (0 = Pendente, 1 = Em andamento, 2 = Concluída)
CREATE PROCEDURE sp_AtualizarStatus
	@IdTarefa UNIQUEIDENTIFIER,
	@Status INT
AS
BEGIN
	UPDATE Tarefas
	SET Status = @Status
	WHERE IdTarefa = @IdTarefa;
END;
GO

-- Lista as tarefas de um participante
CREATE PROCEDURE sp_ListarTarefasPorParticipante
	@IdParticipante UNIQUEIDENTIFIER
AS
BEGIN
	SELECT Titulo, DataVencimento, Status, Categoria
	FROM vw_TarefasParticipantes
	WHERE IdParticipante = @IdParticipante
	ORDER BY DataVencimento;
END;
GO

-- Testando as procedures
DECLARE @IdCategoria UNIQUEIDENTIFIER = (SELECT IdCategoria FROM Categorias WHERE Nome = 'Geral');
DECLARE @IdParticipante UNIQUEIDENTIFIER = (SELECT IdParticipante FROM Participantes WHERE Email = 'ana@exemplo.com');

EXEC sp_InserirTarefa N'Organizar a mesa', NULL, '2026-12-01', @IdCategoria;

DECLARE @IdTarefa UNIQUEIDENTIFIER = (SELECT IdTarefa FROM Tarefas WHERE Titulo = 'Organizar a mesa');
EXEC sp_AdicionarParticipante @IdTarefa, @IdParticipante;
EXEC sp_AtualizarStatus @IdTarefa, 1;

EXEC sp_ListarTarefasPorParticipante @IdParticipante;
GO
