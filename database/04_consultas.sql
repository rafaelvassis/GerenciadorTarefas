USE GerenciadorTarefas;
GO

-- Todas as tarefas
SELECT * FROM Tarefas;

-- Tarefas pendentes (Status = 0)
SELECT Titulo, DataVencimento
FROM Tarefas
WHERE Status = 0;

-- Tarefas ordenadas pela data de vencimento
SELECT Titulo, DataVencimento, Status
FROM Tarefas
ORDER BY DataVencimento;

-- Tarefas com o nome da categoria
SELECT Tarefas.Titulo, Categorias.Nome AS Categoria
FROM Tarefas
INNER JOIN Categorias ON Tarefas.FK_IdCategoria = Categorias.IdCategoria;

-- Tarefas com seus participantes (LEFT JOIN para mostrar também as tarefas sem participantes)
SELECT Tarefas.Titulo, Participantes.Nome AS Participante
FROM Tarefas
LEFT JOIN TarefasParticipantes ON TarefasParticipantes.FK_IdTarefa = Tarefas.IdTarefa
LEFT JOIN Participantes ON Participantes.IdParticipante = TarefasParticipantes.FK_IdParticipante
ORDER BY Tarefas.Titulo;

-- Tarefas de um participante
SELECT Tarefas.Titulo, Tarefas.DataVencimento
FROM Tarefas
INNER JOIN TarefasParticipantes ON TarefasParticipantes.FK_IdTarefa = Tarefas.IdTarefa
INNER JOIN Participantes ON Participantes.IdParticipante = TarefasParticipantes.FK_IdParticipante
WHERE Participantes.Email = 'ana@exemplo.com';

-- Tarefas sem nenhum participante
SELECT Titulo
FROM Tarefas
WHERE IdTarefa NOT IN (SELECT FK_IdTarefa FROM TarefasParticipantes);

-- Quantidade de tarefas por status
SELECT Status, COUNT(*) AS Quantidade
FROM Tarefas
GROUP BY Status;

-- Quantidade de tarefas por categoria
SELECT Categorias.Nome AS Categoria, COUNT(*) AS Quantidade
FROM Tarefas
INNER JOIN Categorias ON Tarefas.FK_IdCategoria = Categorias.IdCategoria
GROUP BY Categorias.Nome;

-- Quantidade de tarefas de cada participante
SELECT Participantes.Nome, COUNT(TarefasParticipantes.FK_IdTarefa) AS Quantidade
FROM Participantes
LEFT JOIN TarefasParticipantes ON TarefasParticipantes.FK_IdParticipante = Participantes.IdParticipante
GROUP BY Participantes.Nome;

-- Tarefas atrasadas: venceram antes de hoje e não foram concluídas
SELECT Titulo, DataVencimento
FROM Tarefas
WHERE DataVencimento < CAST(GETDATE() AS DATE)
	AND Status <> 2;
GO
