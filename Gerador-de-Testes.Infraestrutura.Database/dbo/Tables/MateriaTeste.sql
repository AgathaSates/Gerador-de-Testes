CREATE TABLE [dbo].[MateriaTeste] (
    [MateriasId] UNIQUEIDENTIFIER NOT NULL,
    [TestesId]   UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_MateriaTeste] PRIMARY KEY CLUSTERED ([MateriasId] ASC, [TestesId] ASC),
    CONSTRAINT [FK_MateriaTeste_Materias_MateriasId] FOREIGN KEY ([MateriasId]) REFERENCES [dbo].[Materias] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MateriaTeste_Testes_TestesId] FOREIGN KEY ([TestesId]) REFERENCES [dbo].[Testes] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_MateriaTeste_TestesId]
    ON [dbo].[MateriaTeste]([TestesId] ASC);

