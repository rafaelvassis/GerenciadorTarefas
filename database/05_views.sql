USE GerenciadorTarefas;
GO

-- Tarefas com o nome da categoria
CREATE VIEW vw_Tarefas AS
SELECT
	Tarefas.IdTarefa,
	Tarefas.Titulo,
	Tarefas.Descricao,
	Tarefas.DataVencimento,
	Tarefas.Status,
	Categorias.Nome AS Categoria
FROM Tarefas
INNER JOIN Categorias ON Tarefas.FK_IdCategoria = Categorias.IdCategoria;
GO

-- Cada participante com as tarefas em que participa
CREATE VIEW vw_TarefasParticipantes AS
SELECT
	Participantes.IdParticipante,
	Participantes.Nome AS Participante,
	vw_Tarefas.Titulo,
	vw_Tarefas.DataVencimento,
	vw_Tarefas.Status,
	vw_Tarefas.Categoria
FROM Participantes
INNER JOIN TarefasParticipantes ON TarefasParticipantes.FK_IdParticipante = Participantes.IdParticipante
INNER JOIN vw_Tarefas ON vw_Tarefas.IdTarefa = TarefasParticipantes.FK_IdTarefa;
GO

-- Tarefas atrasadas
CREATE VIEW vw_TarefasAtrasadas AS
SELECT Titulo, DataVencimento, Categoria
FROM vw_Tarefas
WHERE DataVencimento < CAST(GETDATE() AS DATE)
	AND Status <> 2;
GO

-- Testando as views
SELECT * FROM vw_Tarefas;
SELECT * FROM vw_TarefasParticipantes;
SELECT * FROM vw_TarefasAtrasadas;
GO
