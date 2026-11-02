"""Reexecuta dez verificações SQL em memória, sem alterar a base entregue."""
from pathlib import Path
import sqlite3

root = Path(__file__).resolve().parent
con = sqlite3.connect(':memory:')
for filename in ('01_estrutura.sql', '02_dados.sql'):
    con.executescript((root / filename).read_text(encoding='utf-8-sig'))

cases = [
    ('Chave estrangeira inexistente', "INSERT INTO Equipamento VALUES(9,99,'Celular','X','S9')"),
    ('Telefone duplicado', "INSERT INTO Cliente VALUES(9,'Outro','11988881234')"),
    ('Série duplicada', "INSERT INTO Equipamento VALUES(9,1,'Celular','X','sn001')"),
    ('Valor negativo', "INSERT INTO Servico VALUES(9,'Teste',-1)"),
    ('Quantidade zero', 'UPDATE OS_Servico SET quantidade=0 WHERE id_os=1'),
    ('Estado incompatível com aprovação', "UPDATE OrdemServico SET status='Em andamento' WHERE id_os=3"),
    ('Taxa incompatível', 'UPDATE OrdemServico SET taxa_urgencia_centavos=0 WHERE id_os=2'),
    ('Status desconhecido', "UPDATE OrdemServico SET status='Inexistente' WHERE id_os=1"),
]
for name, sql in cases:
    try:
        con.execute(sql)
    except sqlite3.IntegrityError:
        print('PASSOU | ' + name)
    else:
        raise AssertionError('Restrição não aplicada: ' + name)
    finally:
        con.rollback()

con.execute('SAVEPOINT teste')
con.execute('INSERT INTO OS_Servico VALUES(2,3,2,18000)')
total = con.execute('''SELECT SUM(quantidade*valor_unitario_centavos) +
    (SELECT taxa_urgencia_centavos FROM OrdemServico WHERE id_os=2)
    FROM OS_Servico WHERE id_os=2''').fetchone()[0]
assert total == 73500
print('PASSOU | Vários serviços sem duplicar taxa de urgência')
con.execute('ROLLBACK TO teste')
con.execute('RELEASE teste')
assert con.execute('PRAGMA integrity_check').fetchone()[0] == 'ok'
assert not con.execute('PRAGMA foreign_key_check').fetchall()
print('PASSOU | Integridade e vínculos da base')
con.close()
