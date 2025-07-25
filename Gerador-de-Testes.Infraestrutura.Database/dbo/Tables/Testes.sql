CREATE TABLE [dbo].[Testes] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [Titulo]             NVARCHAR (100)   NOT NULL,
    [QuantidadeQuestoes] INT              NOT NULL,
    [ProvaRecuperacao]   BIT              NOT NULL,
    [Serie]              INT              NOT NULL,
    [DisciplinaId]       UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Testes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Testes_Disciplinas_DisciplinaId] FOREIGN KEY ([DisciplinaId]) REFERENCES [dbo].[Disciplinas] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Testes_DisciplinaId]
    ON [dbo].[Testes]([DisciplinaId] ASC);

