CREATE TABLE [dbo].[QuestaoTeste] (
    [QuestoesId] UNIQUEIDENTIFIER NOT NULL,
    [TestesId]   UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_QuestaoTeste] PRIMARY KEY CLUSTERED ([QuestoesId] ASC, [TestesId] ASC),
    CONSTRAINT [FK_QuestaoTeste_Questoes_QuestoesId] FOREIGN KEY ([QuestoesId]) REFERENCES [dbo].[Questoes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_QuestaoTeste_Testes_TestesId] FOREIGN KEY ([TestesId]) REFERENCES [dbo].[Testes] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_QuestaoTeste_TestesId]
    ON [dbo].[QuestaoTeste]([TestesId] ASC);

