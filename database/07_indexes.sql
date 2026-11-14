USE GerenciadorTarefas;
GO

-- Não deixa cadastrar duas categorias com o mesmo nome
CREATE UNIQUE INDEX IX_Categorias_Nome ON Categorias (Nome);

-- Não deixa cadastrar dois participantes com o mesmo e-mail
CREATE UNIQUE INDEX IX_Participantes_Email ON Participantes (Email);

-- Deixam mais rápidas as buscas de tarefas por categoria e por participante
CREATE INDEX IX_Tarefas_FK_IdCategoria ON Tarefas (FK_IdCategoria);
CREATE INDEX IX_TarefasParticipantes_FK_IdParticipante ON TarefasParticipantes (FK_IdParticipante);
GO
