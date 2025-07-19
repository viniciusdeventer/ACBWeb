DROP DATABASE IF EXISTS ACBarros;

CREATE DATABASE ACBarros;

USE ACBarros;

-- Tabela de Usuários
CREATE TABLE Usuarios (
    ID_Usuario INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Usuario VARCHAR(50) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL,
    Status INT DEFAULT 1,       -- 0 = Excluído, 1 = Ativo
    Data_Cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO Usuarios (Nome, Usuario, Senha, Status) VALUES ('Administrador', 'admin', '8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918', 1);

-- Tabela de Clientes
CREATE TABLE Clientes (
    ID_Cliente INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Telefone VARCHAR(15),
    Observacoes TEXT,
    Status INT DEFAULT 1,       -- 0 = Excluído, 1 = Ativo, 2 = Bloqueado
    Data_Cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Produtos
CREATE TABLE Produtos (
    ID_Produto INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Descricao TEXT,
    Imagem VARCHAR(255),
    Valor_Produto DECIMAL(10,2) DEFAULT 0.00 NOT NULL,
    Estoque INT,
    Status INT DEFAULT 1,       -- 0 = Excluído, 1 = Ativo
    Data_Cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Contas Fiado
CREATE TABLE Contas (
    ID_Conta INT PRIMARY KEY AUTO_INCREMENT,
    ID_Cliente INT NOT NULL,
    Situacao INT DEFAULT 0, -- 0 = Aberto, 1 = Pago, 2 = Pago Parcial
    Valor_Pagamento DECIMAL(10,2) DEFAULT 0.00,
    Observacao_Pagamento TEXT,
	Data_Pagamento DATETIME,
    Data_Cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ID_Cliente) REFERENCES Clientes(ID_Cliente)
);

CREATE TABLE Contas_Produtos (
    ID_Item INT PRIMARY KEY AUTO_INCREMENT,
    ID_Conta INT NOT NULL,
    ID_Produto INT, -- NULL se for ajuste/saldo
    Quantidade INT DEFAULT 0,
    Valor_Unitario DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    Tipo_Item INT DEFAULT 0, -- Produto = 0, Ajuste = 1
    Data_Cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ID_Conta) REFERENCES Contas(ID_Conta),
    FOREIGN KEY (ID_Produto) REFERENCES Produtos(ID_Produto)
);

-- Tabela de Vendas
CREATE TABLE Vendas (
    ID_Venda INT PRIMARY KEY AUTO_INCREMENT,
    ID_Item INT NOT NULL,
    Data_Venda DATETIME NOT NULL,
    Quantidade INT NOT NULL,
    Valor_Venda DECIMAL(10,2) DEFAULT 0.00,
    Tipo_Venda INT NOT NULL, -- 1 = Venda, 2 = Pagamento de Conta
    Data_Cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);