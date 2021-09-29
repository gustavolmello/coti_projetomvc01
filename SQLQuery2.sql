/*
	Excluir todas as tarefas cadastradas
*/
DELETE FROM TAREFA;
GO

/*
	Adicionando um campo na tabela de tarefa
*/
ALTER TABLE TAREFA
ADD IDUSUARIO UNIQUEIDENTIFIER NOT NULL
GO

/*
	Definindo o campo como foreign key
*/
ALTER TABLE TAREFA ADD CONSTRAINT FK_USUARIO
FOREIGN KEY(IDUSUARIO)
REFERENCES USUARIO(IDUSUARIO)
GO
