PRAGMA foreign_keys = ON;
BEGIN;
CREATE TABLE Cliente (
 id_cliente INTEGER PRIMARY KEY,
 nome TEXT NOT NULL CHECK(length(trim(nome))>0),
 telefone TEXT NOT NULL UNIQUE CHECK(length(trim(telefone))>0)
);
CREATE TABLE Tecnico (
 id_tecnico INTEGER PRIMARY KEY,
 nome TEXT NOT NULL CHECK(length(trim(nome))>0),
 especialidade TEXT NOT NULL CHECK(length(trim(especialidade))>0)
);
CREATE TABLE Equipamento (
 id_equipamento INTEGER PRIMARY KEY,
 id_cliente INTEGER NOT NULL REFERENCES Cliente(id_cliente),
 tipo TEXT NOT NULL CHECK(length(trim(tipo))>0),
 modelo TEXT NOT NULL CHECK(length(trim(modelo))>0),
 numero_serie TEXT COLLATE NOCASE NOT NULL UNIQUE CHECK(length(trim(numero_serie))>0)
);
CREATE TABLE Servico (
 id_servico INTEGER PRIMARY KEY,
 descricao TEXT NOT NULL CHECK(length(trim(descricao))>0),
 valor_centavos INTEGER NOT NULL CHECK(typeof(valor_centavos)='integer' AND valor_centavos BETWEEN 0 AND 100000000)
);
CREATE TABLE OrdemServico (
 id_os INTEGER PRIMARY KEY,
 id_equipamento INTEGER NOT NULL REFERENCES Equipamento(id_equipamento),
 id_tecnico INTEGER NOT NULL REFERENCES Tecnico(id_tecnico),
 data_abertura TEXT NOT NULL CHECK(length(data_abertura)=10),
 problema TEXT NOT NULL CHECK(length(trim(problema))>0),
 diagnostico TEXT NOT NULL DEFAULT '',
 status TEXT NOT NULL DEFAULT 'Aberta' CHECK(status IN ('Aberta','Em andamento','Concluída','Cancelada')),
 aprovacao TEXT NOT NULL DEFAULT 'Pendente' CHECK(aprovacao IN ('Pendente','Aprovado','Recusado')),
 urgente INTEGER NOT NULL DEFAULT 0 CHECK(urgente IN (0,1)),
 taxa_urgencia_centavos INTEGER NOT NULL DEFAULT 0 CHECK(taxa_urgencia_centavos IN (0,2500)),
 CHECK((urgente=0 AND taxa_urgencia_centavos=0) OR (urgente=1 AND taxa_urgencia_centavos=2500)),
 CHECK((status IN ('Em andamento','Concluída') AND aprovacao='Aprovado') OR status IN ('Aberta','Cancelada')),
 CHECK((aprovacao='Recusado' AND status='Cancelada') OR aprovacao<>'Recusado'),
 CHECK(aprovacao='Pendente' OR length(trim(diagnostico))>0)
);
CREATE TABLE OS_Servico (
 id_os INTEGER NOT NULL REFERENCES OrdemServico(id_os),
 id_servico INTEGER NOT NULL REFERENCES Servico(id_servico),
 quantidade INTEGER NOT NULL CHECK(typeof(quantidade)='integer' AND quantidade BETWEEN 1 AND 1000),
 valor_unitario_centavos INTEGER NOT NULL CHECK(typeof(valor_unitario_centavos)='integer' AND valor_unitario_centavos BETWEEN 0 AND 100000000),
 PRIMARY KEY(id_os,id_servico)
);
CREATE INDEX idx_equipamento_cliente ON Equipamento(id_cliente);
CREATE INDEX idx_os_equipamento ON OrdemServico(id_equipamento);
CREATE INDEX idx_os_tecnico ON OrdemServico(id_tecnico);
CREATE INDEX idx_item_servico ON OS_Servico(id_servico);
COMMIT;
