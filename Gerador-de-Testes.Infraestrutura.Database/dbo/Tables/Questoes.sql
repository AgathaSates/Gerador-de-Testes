CREATE TABLE [dbo].[Questoes] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Enunciado] NVARCHAR (200)   NOT NULL,
    [MateriaId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Questoes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Questoes_Materias_MateriaId] FOREIGN KEY ([MateriaId]) REFERENCES [dbo].[Materias] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Questoes_MateriaId]
    ON [dbo].[Questoes]([MateriaId] ASC);

