# Pitch TechFix

Meta de duração: 9 minutos, reservando até 1 minuto para transições. Limite da disciplina: 10 minutos. Este roteiro orienta a fala, mas cada integrante deve compreender e explicar sua parte.

## Leonardo de 00:00 a 01:00

**Mostrar:** capa e objetivos.

“Somos a equipe TechFix da UNINOVE. Criamos uma empresa fictícia de assistência técnica de computadores e celulares e um protótipo para organizar seus atendimentos. Nosso público proposto inclui usuários domésticos, estudantes e pequenos negócios. A situação que buscamos tratar é a dispersão de informações sobre aparelhos, orçamentos e reparos. Essa demanda ainda será validada com um representante do público; não estamos apresentando um estudo de campo concluído. O objetivo técnico é demonstrar o controle de ordens de serviço, com documentação, programação e banco coerentes.”

## Evelyn de 01:00 a 02:00

**Mostrar:** equipe, cronograma e quadro dos incrementos.

“Organizamos seis integrantes com responsabilidades de liderança, UML, programação, banco, documentação e testes. Escolhemos o modelo incremental: primeiro os cadastros e abertura; depois diagnóstico, decisão e status; por fim consultas e apresentação. Cada incremento passa por análise, implementação e verificação. Nosso plano DevOps usa um repositório único, revisão das alterações, testes e uma versão identificada para entrega. O console, o banco e a interface web são demonstrações independentes, sem integração nesta versão.”

## Rodrigo de 02:00 a 03:20

**Mostrar:** casos de uso, classes e atividades, em zoom legível.

“Definimos dez requisitos funcionais e quatro não funcionais. O ator do sistema local é o operador: ele registra também a decisão comunicada pelo cliente. No diagrama de classes, uma pessoa pode ter vários equipamentos, cada aparelho pode ter várias ordens e cada ordem guarda seus itens com o preço daquele atendimento. O fluxo de atividades rejeita vínculos inválidos, registra o diagnóstico e separa a aprovação da recusa. A recusa cancela a OS; o reparo depende de aprovação. Os nomes e regras são os mesmos no programa e no banco.”

## Matheus de 03:20 a 05:50

**Preparação:** abrir `TechFix.exe --demo`; os cadastros fictícios existem, mas ainda não há OS.

1. Menu 5: equipamento 1, técnico 2, problema “Notebook lento”, urgência N, serviço 1 e quantidade 1. Resultado: OS 1, R$ 120,00.
2. Menu 9, OS 1: mostrar que avançar sem aprovação é bloqueado.
3. Menu 7, OS 1: “Reinstalar o sistema”. Menu 8, OS 1, S. Menu 9, OS 1, duas vezes: Em andamento e Concluída.
4. Menu 5: equipamento 2, técnico 1, problema “Tela quebrada”, urgência S, serviço 2, quantidade 1. Resultado: R$ 375,00, incluindo a taxa de R$ 25,00.
5. Menu 7, OS 2: “Substituir tela”. Menu 8, OS 2, N: cancelamento por recusa.
6. Menu 6 lista as duas ordens. Menu 10, cliente 1, mostra o histórico. Menu 11 mostra R$ 120,00 porque a segunda foi cancelada.

**Fala de apoio:** “Usamos decimal para dinheiro, uma constante para a taxa, decisões para validar estados e repetições para manter o menu e percorrer listas. O preço do item é preservado. Aqui os dados ficam na memória e desaparecem ao encerrar.”

**Atenção:** essa demonstração interativa tem duas OS e total R$ 120,00; o modo `--roteiro` e o banco têm três OS e total R$ 495,00. Explique que são cenários diferentes. Se preferir demonstrar exatamente as figuras do relatório, execute `TechFix.exe --roteiro` e mostre a saída completa antes de voltar ao menu.

## Elen de 05:50 a 07:40

**Mostrar:** DER, seis tabelas e execução das quatro consultas em `techfix.sqlite`.

“Implementamos o banco em SQLite. Cliente, Equipamento, Tecnico, Servico e OrdemServico representam o negócio; OS_Servico resolve a relação muitos para muitos. As chaves estrangeiras verificam vínculos, e CHECK e UNIQUE rejeitam inconsistências. Dinheiro é armazenado em centavos inteiros. A consulta de ordens ativas retorna a OS 2 por R$ 375,00. O total por técnico exclui canceladas: Carlos tem R$ 375,00 e Fernanda R$ 120,00. Primeiro somamos itens por OS e depois acrescentamos a taxa, evitando duplicação. A integridade da base foi conferida.”

## Rosilene de 07:40 a 09:00

**Mostrar:** página web, filtro “joao”, combinação com Cancelada, limpeza dos filtros e registros de testes.

“A interface web é um protótipo de consulta com dados fictícios. A busca ignora acentos e pode ser combinada com a situação. Os testes do C# cobriram regras e entradas inválidas; os testes SQL verificaram vínculos, duplicidades, valores e cálculo. A solução ainda não possui autenticação, integração ou uso comercial. A validação com o público externo é uma próxima etapa. O projeto nos permitiu relacionar análise, orientação a objetos, lógica e banco no mesmo problema. Os links do código e do banco já estão no relatório; o link do vídeo será incluído após sua publicação.”

Depois da publicação, ajuste a última frase para indicar os links disponíveis, sem continuar dizendo que estão pendentes.

## Lista de preparação

Abrir antecipadamente os arquivos, ocultar informações pessoais da área de trabalho, testar áudio, usar zoom legível e ensaiar as transições. Não mostrar senhas nem dados de clientes reais. Publicar o vídeo Não listado e testar o acesso pelo link. Não afirmar pesquisa ou resultado externo que não ocorreu.
