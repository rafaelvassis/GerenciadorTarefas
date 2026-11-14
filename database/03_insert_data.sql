USE GerenciadorTarefas;
GO

-- Categorias
INSERT INTO Categorias (Nome) VALUES ('Geral');
INSERT INTO Categorias (Nome) VALUES ('Trabalho');
INSERT INTO Categorias (Nome) VALUES ('Estudos');
INSERT INTO Categorias (Nome) VALUES ('Pessoal');

-- Participantes
INSERT INTO Participantes (Nome, Email) VALUES ('Ana Souza', 'ana@exemplo.com');
INSERT INTO Participantes (Nome, Email) VALUES ('Bruno Lima', 'bruno@exemplo.com');

-- Tarefas (o IdTarefa e a DataCriacao são preenchidos pelo banco)
-- Status: 0 = Pendente, 1 = Em andamento, 2 = Concluída
INSERT INTO Tarefas (Titulo, Descricao, DataVencimento, Status, FK_IdCategoria)
VALUES (N'Enviar relatório mensal', N'Enviar o relatório para o gestor', '2026-11-10', 0,
	(SELECT IdCategoria FROM Categorias WHERE Nome = 'Trabalho'));

INSERT INTO Tarefas (Titulo, Descricao, DataVencimento, Status, FK_IdCategoria)
VALUES (N'Reunião de planejamento', NULL, '2026-11-20', 1,
	(SELECT IdCategoria FROM Categorias WHERE Nome = 'Trabalho'));

INSERT INTO Tarefas (Titulo, Descricao, DataVencimento, Status, FK_IdCategoria)
VALUES ('Estudar SQL', 'Praticar SELECT, JOIN e GROUP BY', '2026-11-05', 2,
	(SELECT IdCategoria FROM Categorias WHERE Nome = 'Estudos'));

INSERT INTO Tarefas (Titulo, Descricao, DataVencimento, Status, FK_IdCategoria)
VALUES ('Curso de Entity Framework', NULL, '2026-11-25', 0,
	(SELECT IdCategoria FROM Categorias WHERE Nome = 'Estudos'));

INSERT INTO Tarefas (Titulo, Descricao, DataVencimento, Status, FK_IdCategoria)
VALUES ('Pagar conta de luz', NULL, '2026-11-08', 2,
	(SELECT IdCategoria FROM Categorias WHERE Nome = 'Pessoal'));

-- Participantes de cada tarefa
-- A reunião tem dois participantes; "Pagar conta de luz" fica sem nenhum
INSERT INTO TarefasParticipantes (FK_IdTarefa, FK_IdParticipante)
VALUES ((SELECT IdTarefa FROM Tarefas WHERE Titulo = N'Enviar relatório mensal'),
	(SELECT IdParticipante FROM Participantes WHERE Email = 'ana@exemplo.com'));

INSERT INTO TarefasParticipantes (FK_IdTarefa, FK_IdParticipante)
VALUES ((SELECT IdTarefa FROM Tarefas WHERE Titulo = N'Reunião de planejamento'),
	(SELECT IdParticipante FROM Participantes WHERE Email = 'ana@exemplo.com'));

INSERT INTO TarefasParticipantes (FK_IdTarefa, FK_IdParticipante)
VALUES ((SELECT IdTarefa FROM Tarefas WHERE Titulo = N'Reunião de planejamento'),
	(SELECT IdParticipante FROM Participantes WHERE Email = 'bruno@exemplo.com'));

INSERT INTO TarefasParticipantes (FK_IdTarefa, FK_IdParticipante)
VALUES ((SELECT IdTarefa FROM Tarefas WHERE Titulo = 'Estudar SQL'),
	(SELECT IdParticipante FROM Participantes WHERE Email = 'bruno@exemplo.com'));

INSERT INTO TarefasParticipantes (FK_IdTarefa, FK_IdParticipante)
VALUES ((SELECT IdTarefa FROM Tarefas WHERE Titulo = 'Curso de Entity Framework'),
	(SELECT IdParticipante FROM Participantes WHERE Email = 'bruno@exemplo.com'));
GO
