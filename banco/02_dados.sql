PRAGMA foreign_keys = ON;
BEGIN;
-- Dados inteiramente fictícios para demonstração acadêmica.
INSERT INTO Cliente VALUES (1,'Maria Souza','11988881234'),(2,'João Pereira','11977775678'),(3,'Ana Lima','11966664321');
INSERT INTO Tecnico VALUES (1,'Carlos Rocha','Hardware'),(2,'Fernanda Alves','Software');
INSERT INTO Equipamento VALUES
 (1,1,'Notebook','Dell Inspiron 15','SN001'),
 (2,2,'Celular','Samsung Galaxy S21','SN002'),
 (3,3,'Celular','Motorola G','SN003');
INSERT INTO Servico VALUES (1,'Formatação e reinstalação',12000),(2,'Troca de tela',35000),(3,'Troca de bateria',18000);
INSERT INTO OrdemServico VALUES
 (1,1,2,'2026-10-24','Notebook lento','Sistema necessita reinstalação','Concluída','Aprovado',0,0),
 (2,2,1,'2026-10-24','Tela quebrada','Substituir tela','Em andamento','Aprovado',1,2500),
 (3,3,1,'2026-10-24','Bateria sem carga','Bateria desgastada','Cancelada','Recusado',0,0);
INSERT INTO OS_Servico VALUES (1,1,1,12000),(2,2,1,35000),(3,3,1,18000);
COMMIT;
