from pathlib import Path
import sqlite3,subprocess,json
root=Path(__file__).resolve().parent
# Não substitui uma base existente; cada execução gera um novo arquivo nomeado.
dest=root/'techfix.sqlite'
if dest.exists(): raise SystemExit('A base techfix.sqlite já existe. Use uma pasta nova para recriar a demonstração.')
c=sqlite3.connect(dest)
for f in ['01_estrutura.sql','02_dados.sql']: c.executescript((root/f).read_text(encoding='utf-8-sig'))
print('Banco criado:',dest.name)
for s in (root/'03_consultas.sql').read_text(encoding='utf-8-sig').split(';'):
 if s.strip():
  cur=c.execute(s); print('\n'+' | '.join(x[0] for x in cur.description))
  for row in cur.fetchall(): print(' | '.join(map(str,row)))
print('\nIntegridade:',c.execute('PRAGMA integrity_check').fetchone()[0])
print('Violações FK:',c.execute('PRAGMA foreign_key_check').fetchall())
c.close()
