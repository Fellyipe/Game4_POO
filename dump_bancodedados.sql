CREATE DATABASE poo_game3;

USE nome_do_banco_de_dados;

CREATE TABLE tb_produto (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Descricao VARCHAR(255) NOT NULL,
    Preco DECIMAL(10,2) NOT NULL,
    QuantidadeEmEstoque INT NOT NULL
);

CREATE TABLE tb_pedido (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Data DATE NOT NULL,
    Cliente VARCHAR(100) NOT NULL,
    Status VARCHAR(20) NOT NULL
);

CREATE TABLE tb_itempedido (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    tb_produtoId INT,
    Quantidade INT,
    PrecoUnitario DECIMAL(10, 2),
    tb_pedidoId INT,
    FOREIGN KEY (tb_produtoId) REFERENCES tb_produto (Id) ON DELETE CASCADE,
    FOREIGN KEY (tb_pedidoId) REFERENCES tb_pedido (Id) ON DELETE CASCADE
);
