DROP PROCEDURE IF EXISTS PagarConta;

DELIMITER //

CREATE PROCEDURE PagarConta(
    IN p_IdConta INT,
    IN p_IdCliente INT,
    IN p_ValorPagamento DECIMAL(10,2),
    IN p_DataPagamento DATETIME,
    IN p_ObservacaoPagamento TEXT
)
BEGIN
    DECLARE v_TotalProdutos DECIMAL(10,2) DEFAULT 0;
    DECLARE v_Diferenca DECIMAL(10,2) DEFAULT 0;
    DECLARE v_IdNovaConta INT DEFAULT NULL;
    DECLARE v_Situacao INT DEFAULT 1;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;

    SELECT COALESCE(SUM(CP.Quantidade * CP.Valor_Unitario), 0)
    INTO v_TotalProdutos
    FROM Contas_Produtos CP
    WHERE CP.ID_Conta = p_IdConta;

    IF v_TotalProdutos > p_ValorPagamento THEN
        SET v_Diferenca = v_TotalProdutos - p_ValorPagamento;
        SET v_Situacao = 2;

        INSERT INTO Contas (ID_Cliente, Situacao)
        VALUES (p_IdCliente, 0);

        SET v_IdNovaConta = LAST_INSERT_ID();

        INSERT INTO Contas_Produtos (ID_Conta, Quantidade, Valor_Unitario, Tipo_Item)
        VALUES (v_IdNovaConta, 1, v_Diferenca, 1);
    END IF;

    UPDATE Contas
    SET Situacao = v_Situacao,
        Valor_Pagamento = p_ValorPagamento,
        Data_Pagamento = p_DataPagamento,
        Observacao_Pagamento = p_ObservacaoPagamento
    WHERE ID_Conta = p_IdConta;

    INSERT INTO Vendas (ID_Item, Data_Venda, Quantidade, Valor_Venda, Tipo_Venda)
    VALUES (p_IdConta, p_DataPagamento, 1, p_ValorPagamento, 1);

    COMMIT;
END //

DELIMITER ;
