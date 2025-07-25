CREATE TABLE [dbo].[Alternativas] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Descricao] NVARCHAR (200)   NOT NULL,
    [Correta]   BIT              NOT NULL,
    [QuestaoId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Alternativas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Alternativas_Questoes_QuestaoId] FOREIGN KEY ([QuestaoId]) REFERENCES [dbo].[Questoes] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Alternativas_QuestaoId]
    ON [dbo].[Alternativas]([QuestaoId] ASC);

