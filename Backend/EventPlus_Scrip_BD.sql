-- ------------------------------------------------------------
-- 1) Criacao do banco
-- ------------------------------------------------------------
IF DB_ID('EventPlus') IS NULL
    CREATE DATABASE EventPlus;
GO

USE EventPlus;
GO

-- ------------------------------------------------------------
-- 2) Tabelas "pai" (sem dependencia de FK)
-- ------------------------------------------------------------

CREATE TABLE TipoUsuario (
    IdTipoUsuario     UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    TituloTipoUsuario VARCHAR(100)     NOT NULL,
    CONSTRAINT PK_TipoUsuario PRIMARY KEY (IdTipoUsuario)
);
GO

CREATE TABLE TipoEvento (
    IdTipoEvento     UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    TituloTipoEvento VARCHAR(100)     NOT NULL,
    CONSTRAINT PK_TipoEvento PRIMARY KEY (IdTipoEvento)
);
GO

CREATE TABLE Instituicao (
    IdInstituicao UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    CNPJ          VARCHAR(14)      NOT NULL,
    NomeFantasia  VARCHAR(100)     NOT NULL,
    Endereco      VARCHAR(100)     NOT NULL,
    CONSTRAINT PK_Instituicao PRIMARY KEY (IdInstituicao),
    CONSTRAINT UQ_Instituicao_CNPJ UNIQUE (CNPJ)
);
GO

-- ------------------------------------------------------------
-- 3) Usuario (associacao com TipoUsuario -> FK anulavel, SET NULL)
-- ------------------------------------------------------------

CREATE TABLE Usuario (
    IdUsuario     UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Nome          VARCHAR(100)     NOT NULL,
    Email         VARCHAR(256)     NOT NULL,
    Senha         VARCHAR(60)      NOT NULL,
    IdTipoUsuario UNIQUEIDENTIFIER NULL,
    CONSTRAINT PK_Usuario PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuario_Email UNIQUE (Email),
    CONSTRAINT FK_Usuario_TipoUsuario FOREIGN KEY (IdTipoUsuario)
        REFERENCES TipoUsuario (IdTipoUsuario)
        ON DELETE SET NULL
);
GO

-- ------------------------------------------------------------
-- 4) Evento (associacoes com TipoEvento e Instituicao -> FK anulaveis, SET NULL)
-- ------------------------------------------------------------

CREATE TABLE Evento (
    IdEvento      UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    NomeEvento    VARCHAR(100)     NOT NULL,
    DataEvento    DATETIME2        NOT NULL,
    Descricao     VARCHAR(MAX)     NOT NULL,
    ImagemUrl     NVARCHAR(2048)  NULL,
    IdTipoEvento  UNIQUEIDENTIFIER NULL,
    IdInstituicao UNIQUEIDENTIFIER NULL,
    CONSTRAINT PK_Evento PRIMARY KEY (IdEvento),
    CONSTRAINT FK_Evento_TipoEvento FOREIGN KEY (IdTipoEvento)
        REFERENCES TipoEvento (IdTipoEvento)
        ON DELETE SET NULL,
    CONSTRAINT FK_Evento_Instituicao FOREIGN KEY (IdInstituicao)
        REFERENCES Instituicao (IdInstituicao)
        ON DELETE SET NULL
);
GO

-- ------------------------------------------------------------
-- 5) Presenca (tabela associativa)
--    Evento  -> composicao: FK obrigatoria, ON DELETE CASCADE
--    Usuario -> associacao: FK anulavel,   ON DELETE SET NULL
-- ------------------------------------------------------------

CREATE TABLE Presenca (
    IdPresenca UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Situacao   BIT              NOT NULL,
    IdEvento   UNIQUEIDENTIFIER NOT NULL,
    IdUsuario  UNIQUEIDENTIFIER NULL,
    CONSTRAINT PK_Presenca PRIMARY KEY (IdPresenca),
    CONSTRAINT FK_Presenca_Evento FOREIGN KEY (IdEvento)
        REFERENCES Evento (IdEvento)
        ON DELETE CASCADE,
    CONSTRAINT FK_Presenca_Usuario FOREIGN KEY (IdUsuario)
        REFERENCES Usuario (IdUsuario)
        ON DELETE SET NULL
);
GO

-- ------------------------------------------------------------
-- 6) Comentario (tabela associativa)
--    Evento  -> composicao: FK obrigatoria, ON DELETE CASCADE
--    Usuario -> associacao: FK anulavel,   ON DELETE SET NULL
-- ------------------------------------------------------------

CREATE TABLE Comentario (
    IdComentario   UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    DataComentario DATETIME2        NOT NULL,
    Descricao      VARCHAR(200)     NOT NULL,
    Exibe          BIT              NOT NULL,
    IdEvento       UNIQUEIDENTIFIER NOT NULL,
    IdUsuario      UNIQUEIDENTIFIER NULL,
    CONSTRAINT PK_Comentario PRIMARY KEY (IdComentario),
    CONSTRAINT FK_Comentario_Evento FOREIGN KEY (IdEvento)
        REFERENCES Evento (IdEvento)
        ON DELETE CASCADE,
    CONSTRAINT FK_Comentario_Usuario FOREIGN KEY (IdUsuario)
        REFERENCES Usuario (IdUsuario)
        ON DELETE SET NULL
);
GO
