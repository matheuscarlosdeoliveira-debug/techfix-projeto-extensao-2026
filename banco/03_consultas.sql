-- Q1 Clientes e equipamentos
SELECT c.nome AS cliente,e.tipo,e.modelo,e.numero_serie
FROM Cliente c JOIN Equipamento e ON e.id_cliente=c.id_cliente
ORDER BY c.nome,e.id_equipamento;

-- Q2 Ordens abertas ou em andamento e orçamento calculado
SELECT o.id_os,c.nome AS cliente,t.nome AS tecnico,o.status,
       printf('%.2f',(SUM(i.quantidade*i.valor_unitario_centavos)+o.taxa_urgencia_centavos)/100.0) AS total_reais
FROM OrdemServico o
JOIN Equipamento e ON e.id_equipamento=o.id_equipamento
JOIN Cliente c ON c.id_cliente=e.id_cliente
JOIN Tecnico t ON t.id_tecnico=o.id_tecnico
JOIN OS_Servico i ON i.id_os=o.id_os
WHERE o.status IN ('Aberta','Em andamento')
GROUP BY o.id_os,c.nome,t.nome,o.status,o.taxa_urgencia_centavos
ORDER BY o.id_os;

-- Q3 Total orçado não cancelado por técnico
WITH Totais AS (
 SELECT o.id_os,o.id_tecnico,SUM(i.quantidade*i.valor_unitario_centavos)+o.taxa_urgencia_centavos AS centavos
 FROM OrdemServico o JOIN OS_Servico i ON i.id_os=o.id_os
 WHERE o.status<>'Cancelada'
 GROUP BY o.id_os,o.id_tecnico,o.taxa_urgencia_centavos
)
SELECT t.id_tecnico,t.nome,printf('%.2f',COALESCE(SUM(x.centavos),0)/100.0) AS total_reais
FROM Tecnico t LEFT JOIN Totais x ON x.id_tecnico=t.id_tecnico
GROUP BY t.id_tecnico,t.nome ORDER BY t.id_tecnico;

-- Q4 Histórico do cliente 1 com serviços
SELECT o.id_os,e.modelo,o.data_abertura,o.status,s.descricao,i.quantidade,
       printf('%.2f',i.valor_unitario_centavos/100.0) AS valor_unitario_reais
FROM OrdemServico o JOIN Equipamento e ON e.id_equipamento=o.id_equipamento
JOIN OS_Servico i ON i.id_os=o.id_os JOIN Servico s ON s.id_servico=i.id_servico
WHERE e.id_cliente=1 ORDER BY o.data_abertura,o.id_os;
