USE GerenciadorTarefas;
GO

CREATE TABLE Categorias (
	IdCategoria UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	Nome NVARCHAR(150) NOT NULL,

	CONSTRAINT PK_Categorias PRIMARY KEY (IdCategoria)
);

CREATE TABLE Participantes (
	IdParticipante UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	Nome NVARCHAR(150) NOT NULL,
	Email VARCHAR(255) NOT NULL,

	CONSTRAINT PK_Participantes PRIMARY KEY (IdParticipante)
);

-- Toda tarefa precisa ter uma categoria
CREATE TABLE Tarefas (
	IdTarefa UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	Titulo NVARCHAR(200) NOT NULL,
	Descricao NVARCHAR(1000) NULL,
	DataVencimento DATETIME2 NOT NULL,
	Status INT NOT NULL DEFAULT 0, -- 0 = Pendente, 1 = Em andamento, 2 = Concluída
	DataCriacao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
	FK_IdCategoria UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT PK_Tarefas PRIMARY KEY (IdTarefa),

	CONSTRAINT FK_Tarefas_Categorias_FK_IdCategoria
		FOREIGN KEY (FK_IdCategoria) REFERENCES Categorias(IdCategoria),

	-- O status só pode ser 0, 1 ou 2
	CONSTRAINT CK_Tarefas_Status CHECK (Status IN (0, 1, 2))
);

-- Liga tarefas e participantes (uma tarefa pode ter vários participantes, ou nenhum)
-- Se a tarefa ou o participante for excluído, o vínculo é apagado junto (CASCADE)
CREATE TABLE TarefasParticipantes (
	FK_IdTarefa UNIQUEIDENTIFIER NOT NULL,
	FK_IdParticipante UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT PK_TarefasParticipantes PRIMARY KEY (FK_IdTarefa, FK_IdParticipante),

	CONSTRAINT FK_TarefasParticipantes_Tarefas_FK_IdTarefa
		FOREIGN KEY (FK_IdTarefa) REFERENCES Tarefas(IdTarefa) ON DELETE CASCADE,

	CONSTRAINT FK_TarefasParticipantes_Participantes_FK_IdParticipante
		FOREIGN KEY (FK_IdParticipante) REFERENCES Participantes(IdParticipante) ON DELETE CASCADE
);
GO
