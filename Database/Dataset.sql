INSERT INTO Clientes (Nome, Telefone, Observacoes, Status) VALUES
('Carlos Henrique', '11985472563', 'Cliente frequente, permitir parcelamento', 1),
('Joana Martins', '11999887766', 'Bloquear vendas acima de R$500', 2),
('Fernando Lima', '11933445566', 'Solicita nota fiscal sempre', 1),
('Patrícia Souza', '11922334455', 'Cliente bloqueado por inadimplência', 2),
('Marcelo Oliveira', '11911223344', 'Liberar apenas mediante aprovação do gerente', 1),
('Vanessa Almeida', '11977665544', 'Cliente VIP – atendimento prioritário', 1),
('Roberto Silva', '11966554433', 'Solicitou pausa no cadastro – manter bloqueado', 2),
('Luciana Ferreira', '11988887777', 'Prefere contato via WhatsApp', 1),
('André Couto', '11977776666', 'Cliente bloqueado por tentativa de fraude', 2),
('Beatriz Ramos', '11966665555', 'Faz pedidos grandes no fim do mês', 1),
('Diego Azevedo', '11955554444', 'Aguardando atualização cadastral', 2),
('Tatiane Lopes', '11944443333', 'Sempre retira produtos no balcão', 1),
('Eduardo Nascimento', '11933332222', 'Possui fiado ativo com vencimento próximo', 1);

INSERT INTO Produtos (Nome, Descricao, Imagem, Valor_Produto, Estoque, Status) VALUES 
('Cimento Votoran 50kg', 'Saco de cimento Votoran Todas as Obras 50kg', 'imagens/produtos/D25AC65F-19B4-4413-9D8B-11BA05EE3B4C.jpeg', 39.90, 120, 1),
('Areia Média Ensacada', 'Saco de areia média lavada 20kg', 'imagens/produtos/CB5CEE7C-D050-4485-B6FC-9021E9CD649E.png', 12.50, 200, 1),
('Brita 1 Ensacada', 'Brita número 1 lavada saco de 20kg', 'imagens/produtos/F5C8DA9B-BF35-42AC-8116-98C5E0A2EC36.jpeg', 15.90, 180, 1),
('Bloco de Concreto 39x19x14cm', 'Bloco estrutural cinza para alvenaria', 'imagens/produtos/3EAF7607-4B6B-4FBC-A73A-6EE237F6ADD0.png', 3.20, 1000, 1),
('Tijolo Cerâmico 9 Furos', 'Tijolo baiano cerâmico com 9 furos', 'imagens/produtos/D45EB777-A85C-42A2-9DA2-BF9CE33B2F7F.png', 1.10, 1500, 1),
('Telha de Barro Tipo Colonial', 'Telha cerâmica tipo colonial 40x24cm', 'imagens/produtos/66123D64-6F23-4016-AAC6-66DA0F0F3BF6.jpeg', 2.90, 800, 1),
('Saco de Cal Hidratada 20kg', 'Cal hidratada para construção civil', 'imagens/produtos/606D3ED3-BC98-47BA-BCEB-0B823ACB1072.jpeg', 14.50, 150, 1),
('Viga de Aço 4m', 'Viga de aço CA-50 4 metros para estrutura', 'imagens/produtos/A390FA80-22D4-42CD-921F-B065D4E7C14E.png', 38.00, 60, 1),
('Tubo PVC 100mm 3m', 'Tubo esgoto PVC 100mm 3 metros', 'imagens/produtos/4A2787DC-BDA1-4B22-ACB7-B30F430304DB.jpg', 28.70, 90, 1),
('Caixa de Massa Corrida 25kg', 'Massa corrida para acabamento interno', 'imagens/produtos/CDC33D88-0C50-49F7-8DD0-A495B4095657.png', 52.00, 70, 1);

INSERT INTO Produtos (Nome, Descricao, Imagem, Valor_Produto, Estoque, Status) VALUES
('Argamassa AC-I 20kg', 'Argamassa colante para cerâmicas internas', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 18.90, 100, 1),
('Rolo de Pintura 23cm', 'Rolo de lã para pintura de superfícies lisas', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 12.90, 50, 1),
('Massa Acrílica 25kg', 'Massa acrílica para áreas externas', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 58.00, 60, 1),
('Caixa d’Água 500L', 'Caixa d’água em polietileno com tampa', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 320.00, 10, 1),
('Carrinho de Mão Reforçado', 'Carrinho de mão com caçamba metálica reforçada', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 189.90, 25, 1),
('Martelo 27mm', 'Martelo com cabo de fibra e cabeça de aço forjado', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 24.50, 70, 1),
('Nível de Alumínio 60cm', 'Nível bolha de alumínio anodizado', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 34.90, 40, 1),
('Pá Quadrada de Aço', 'Pá de aço para construção com cabo de madeira', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 28.90, 55, 1),
('Saco de Gesso 40kg', 'Gesso em pó para revestimento interno', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 26.00, 80, 1),
('Chave de Fenda 1/4" x 6"', 'Chave de fenda com cabo emborrachado e ponta imantada', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 8.90, 120, 1);

INSERT INTO Produtos (Nome, Descricao, Imagem, Valor_Produto, Estoque, Status) VALUES
('Broxa de Pintura 4"', 'Broxa para aplicação de impermeabilizantes e cal', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 9.50, 100, 1),
('Pincel 2"', 'Pincel para pintura em áreas pequenas e acabamentos', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 6.90, 120, 1),
('Lona Plástica Preta 4x5m', 'Lona plástica para proteção de obras e materiais', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 27.90, 40, 1),
('Desempenadeira de Aço Lisa', 'Desempenadeira para acabamento de reboco e massa', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 19.00, 60, 1),
('Escada Alumínio 5 Degraus', 'Escada retrátil em alumínio leve e resistente', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 179.90, 15, 1);

INSERT INTO Produtos (Nome, Descricao, Imagem, Valor_Produto, Estoque, Status) VALUES
('Torneira Esfera 1/2"', 'Torneira de esfera em PVC para uso residencial', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 11.90, 100, 1),
('Joelho PVC 90º 50mm', 'Joelho para conexões hidráulicas em PVC', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 3.80, 200, 1),
('Interruptor Simples', 'Interruptor de luz padrão 4x2 branco', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 4.90, 150, 1),
('Tomada 2P+T 10A', 'Tomada de embutir padrão novo', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 5.50, 130, 1),
('Fita Isolante 19mmx10m', 'Fita isolante preta para instalações elétricas', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 2.90, 300, 1),
('Cimento Branco 1kg', 'Cimento branco para rejuntes e acabamento', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 12.00, 60, 1),
('Balde de Construção 12L', 'Balde plástico resistente com alça metálica', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 15.00, 80, 1),
('Luvas de PVC', 'Luvas de proteção impermeáveis para obra', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 4.20, 90, 1),
('Máscara PFF2 com válvula', 'Máscara de proteção contra poeira com válvula', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 3.50, 200, 1),
('Trena 5m', 'Trena retrátil com trava e cinta de aço', 'imagens/produtos/FEC9529E-C179-4BAB-9C31-D380F2EC1189.png', 16.90, 70, 1);

