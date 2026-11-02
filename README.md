# TechFix

Projeto de Extensão em Desenvolvimento de Sistemas — UNINOVE — 2026.

Protótipo acadêmico de gestão de assistência técnica. Dados de demonstração fictícios.

## Abrir o projeto

1. Leia `documentacao/TechFix_Trabalho_Consolidado.pdf` e o guia de finalização.
2. No Windows, abra `csharp/INICIAR.cmd`. Ele compila `Program.cs` com o compilador do .NET Framework e inicia com cadastros fictícios. Alternativa: abra `TechFix.exe --demo` em um terminal.
3. Com SDK .NET 8 instalado, execute `dotnet run -- --demo` na pasta `csharp`. Essa alternativa não foi executada no ambiente de preparação.
4. Para reproduzir as saídas registradas, execute `TechFix.exe --roteiro`.
5. Abra `banco/techfix.sqlite` em um gerenciador compatível com SQLite. Ative as chaves estrangeiras: `PRAGMA foreign_keys = ON;` em cada conexão.
6. Abra `web/ABRIR.cmd` para consultar a interface web local.

## Pastas

| Pasta | Conteúdo |
|---|---|
| documentacao | Relatório, texto editável e materiais de finalização. |
| csharp | Fonte, projeto, executáveis de demonstração e testes. |
| banco | Criação, dados, consultas e base SQLite fictícia. |
| diagramas | Organograma, UML, DER e fontes editáveis. |
| evidencias | Saídas reais de execução e verificações. |
| web | Protótipo de consulta, independente do C# e do banco. |

## O que foi verificado

- Compilação e execução com o compilador .NET Framework 4 disponível no Windows.
- 21 verificações automatizadas do domínio e um cenário de entradas inválidas do menu.
- SQL executado sem adaptações, integridade `ok`, nenhuma violação de FK e dez verificações adicionais.
- Busca da web sem acento, combinação de filtros, ausência de resultados e limpeza dos filtros.

O console guarda os registros somente em memória. O SQLite e a web são demonstrações independentes, conforme o escopo do relatório. O total de R$ 495,00 no cenário fictício é orçamento de OS não canceladas, não receita.

## Recriar o banco

Em uma base vazia, execute nesta ordem: `01_estrutura.sql`, `02_dados.sql`, `03_consultas.sql`. Para usar `executar.py`, coloque esse arquivo e os três scripts em uma pasta sem `techfix.sqlite`, então execute `python executar.py`. O script recusa sobrescrever uma base existente.

## Executar testes do domínio

Na pasta `csharp`, compile `Program.cs` e `Testes.cs` juntos, definindo `Testes` como ponto de entrada. Exemplo no terminal do desenvolvedor: `csc /r:System.Core.dll /main:Testes /out:Testes.exe Program.cs Testes.cs`. Execute `Testes.exe`. O projeto SDK exclui `Testes.cs` para manter apenas o ponto de entrada da aplicação.

## Situação da entrega

A parcial de 28/09/2026 foi entregue. Faltam a publicação do GitHub e do vídeo, inserção dos links reais, confirmação do prazo final e validação externa da demanda. O relatório explicita esses limites; nenhum estudo de campo ou publicação foi inventado.

Os nomes e RAs estão no relatório e no fonte conforme os documentos fornecidos. Antes de publicar um repositório público, o grupo deve decidir quais dados acadêmicos manter acessíveis; o acesso da professora às entregas deve estar garantido.
