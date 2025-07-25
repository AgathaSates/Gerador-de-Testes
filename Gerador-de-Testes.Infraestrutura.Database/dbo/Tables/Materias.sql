CREATE TABLE [dbo].[Materias] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Nome]         NVARCHAR (MAX)   NOT NULL,
    [Serie]        INT              NOT NULL,
    [DisciplinaId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Materias] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Materias_Disciplinas_DisciplinaId] FOREIGN KEY ([DisciplinaId]) REFERENCES [dbo].[Disciplinas] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Materias_DisciplinaId]
    ON [dbo].[Materias]([DisciplinaId] ASC);

