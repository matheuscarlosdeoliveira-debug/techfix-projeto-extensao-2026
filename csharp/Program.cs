// TechFix - Projeto de Extensão em Desenvolvimento de Sistemas - UNINOVE 2026
// Elen Carolina Souza de Aquino 2224200303; Evelyn de Carvalho Rocha 2226200016
// Leonardo Gomes de Sousa 2226106514; Matheus Carlos de Oliveira 2222200464
// Rodrigo França Alves Santana 2326202955; Rosilene do Espírito Santo 2226201088
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TechFix
{
    public class Cliente { public int Id; public string Nome, Telefone; }
    public class Equipamento { public int Id, ClienteId; public string Tipo, Modelo, NumeroSerie; }
    public class Tecnico { public int Id; public string Nome, Especialidade; }
    public class Servico { public int Id; public string Descricao; public decimal Valor; }
    public class ItemServico
    {
        public int ServicoId, Quantidade; public decimal ValorUnitario;
        public decimal Subtotal() { return Quantidade * ValorUnitario; }
    }
    public class OrdemServico
    {
        public int Id, EquipamentoId, TecnicoId;
        public DateTime DataAbertura = DateTime.Today;
        public string Problema, Diagnostico = "", Status = "Aberta", Aprovacao = "Pendente";
        public bool Urgente;
        public List<ItemServico> Itens = new List<ItemServico>();
        public const decimal TAXA_URGENCIA = 25.00m;
        public decimal Total() { return Itens.Sum(i => i.Subtotal()) + (Urgente ? TAXA_URGENCIA : 0m); }
        public void RegistrarDiagnostico(string texto)
        {
            if (Status != "Aberta" || String.IsNullOrWhiteSpace(texto))
                throw new InvalidOperationException("Diagnóstico exige OS aberta e texto preenchido.");
            Diagnostico = texto.Trim();
        }
        public void Decidir(bool aprovado)
        {
            if (Status != "Aberta" || Aprovacao != "Pendente" || String.IsNullOrWhiteSpace(Diagnostico))
                throw new InvalidOperationException("É necessário diagnosticar uma OS aberta e pendente.");
            Aprovacao = aprovado ? "Aprovado" : "Recusado";
            if (!aprovado) Status = "Cancelada";
        }
        public void Avancar()
        {
            if (Status == "Aberta" && Aprovacao == "Aprovado") Status = "Em andamento";
            else if (Status == "Em andamento") Status = "Concluída";
            else throw new InvalidOperationException("Transição não permitida. Verifique aprovação e status.");
        }
    }
    // Camada de operações: valida vínculos e mantém os cadastros em memória.
    public class Gestao
    {
        public List<Cliente> Clientes = new List<Cliente>();
        public List<Equipamento> Equipamentos = new List<Equipamento>();
        public List<Tecnico> Tecnicos = new List<Tecnico>();
        public List<Servico> Servicos = new List<Servico>();
        public List<OrdemServico> Ordens = new List<OrdemServico>();
        static string Obrigatorio(string texto)
        { if (String.IsNullOrWhiteSpace(texto)) throw new ArgumentException("Campo obrigatório."); return texto.Trim(); }
        public Cliente CadastrarCliente(string nome, string telefone)
        {
            nome = Obrigatorio(nome); telefone = Obrigatorio(telefone);
            if (Clientes.Any(item => item.Telefone == telefone)) throw new ArgumentException("Telefone já cadastrado.");
            Cliente c = new Cliente { Id = Clientes.Count + 1, Nome = nome, Telefone = telefone }; Clientes.Add(c); return c;
        }
        public Equipamento CadastrarEquipamento(int cliente, string tipo, string modelo, string serie)
        {
            if (!Clientes.Any(c => c.Id == cliente)) throw new ArgumentException("Cliente não encontrado.");
            serie = Obrigatorio(serie);
            if (Equipamentos.Any(item => String.Equals(item.NumeroSerie, serie, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Número de série já cadastrado.");
            Equipamento e = new Equipamento { Id = Equipamentos.Count + 1, ClienteId = cliente,
                Tipo = Obrigatorio(tipo), Modelo = Obrigatorio(modelo), NumeroSerie = serie }; Equipamentos.Add(e); return e;
        }
        public Tecnico CadastrarTecnico(string nome, string especialidade)
        {
            Tecnico t = new Tecnico { Id = Tecnicos.Count + 1, Nome = Obrigatorio(nome), Especialidade = Obrigatorio(especialidade) };
            Tecnicos.Add(t); return t;
        }
        public Servico CadastrarServico(string descricao, decimal valor)
        {
            if (valor < 0m || valor > 1000000m) throw new ArgumentException("Valor fora do limite.");
            Servico s = new Servico { Id = Servicos.Count + 1, Descricao = Obrigatorio(descricao), Valor = valor }; Servicos.Add(s); return s;
        }
        public OrdemServico Abrir(int equipamento, int tecnico, string problema, bool urgente, int servico, int quantidade)
        {
            if (!Equipamentos.Any(e => e.Id == equipamento) || !Tecnicos.Any(t => t.Id == tecnico))
                throw new ArgumentException("Equipamento ou técnico não encontrado.");
            Servico s = Servicos.Find(x => x.Id == servico);
            if (s == null || quantidade < 1 || quantidade > 1000) throw new ArgumentException("Serviço ou quantidade inválidos.");
            OrdemServico o = new OrdemServico { Id = Ordens.Count + 1, EquipamentoId = equipamento,
                TecnicoId = tecnico, Problema = Obrigatorio(problema), Urgente = urgente };
            o.Itens.Add(new ItemServico { ServicoId = s.Id, Quantidade = quantidade, ValorUnitario = s.Valor });
            Ordens.Add(o); return o;
        }
        public OrdemServico Buscar(int id)
        { OrdemServico o = Ordens.Find(x => x.Id == id); if (o == null) throw new ArgumentException("OS não encontrada."); return o; }
        public IEnumerable<OrdemServico> Historico(int cliente)
        {
            if (!Clientes.Any(c => c.Id == cliente)) throw new ArgumentException("Cliente não encontrado.");
            return Ordens.Where(o => Equipamentos.Any(e => e.Id == o.EquipamentoId && e.ClienteId == cliente));
        }
        public void CarregarDemonstracao()
        {
            CadastrarCliente("Maria Souza", "11988881234"); CadastrarCliente("João Pereira", "11977775678");
            CadastrarCliente("Ana Lima", "11966664321");
            CadastrarEquipamento(1, "Notebook", "Dell Inspiron 15", "SN001");
            CadastrarEquipamento(2, "Celular", "Samsung Galaxy S21", "SN002");
            CadastrarEquipamento(3, "Celular", "Motorola G", "SN003");
            CadastrarTecnico("Carlos Rocha", "Hardware"); CadastrarTecnico("Fernanda Alves", "Software");
            CadastrarServico("Formatação e reinstalação", 120m); CadastrarServico("Troca de tela", 350m);
            CadastrarServico("Troca de bateria", 180m);
        }
    }
    public class Program
    {
        static Gestao g = new Gestao();
        static readonly CultureInfo Br = CultureInfo.GetCultureInfo("pt-BR");
        static string Texto(string pergunta)
        {
            while (true) { Console.Write(pergunta); string s = Console.ReadLine();
                if (s == null) throw new OperationCanceledException();
                if (!String.IsNullOrWhiteSpace(s)) return s.Trim(); Console.WriteLine("Preencha o campo."); }
        }
        static int Inteiro(string pergunta, int min, int max)
        {
            int n; while (true) { string s = Texto(pergunta);
                if (Int32.TryParse(s, out n) && n >= min && n <= max) return n;
                Console.WriteLine("Informe um inteiro entre " + min + " e " + max + "."); }
        }
        static decimal Dinheiro(string pergunta)
        {
            decimal n; while (true) { string s = Texto(pergunta);
                if (Decimal.TryParse(s, NumberStyles.AllowDecimalPoint, Br, out n) && n >= 0m && n <= 1000000m
                    && Decimal.Round(n, 2) == n) return n;
                Console.WriteLine("Valor inválido. Use vírgula e até duas casas decimais, sem milhar."); }
        }
        static bool SimNao(string pergunta)
        {
            while (true) { string s = Texto(pergunta).ToUpperInvariant(); if (s == "S") return true;
                if (s == "N") return false; Console.WriteLine("Responda S ou N."); }
        }
        static void Listar(IEnumerable<OrdemServico> ordens)
        {
            int quantidade = 0;
            foreach (OrdemServico o in ordens) {
                Equipamento e = g.Equipamentos.Find(x => x.Id == o.EquipamentoId);
                Cliente c = g.Clientes.Find(x => x.Id == e.ClienteId);
                Console.WriteLine("OS #" + o.Id + " | " + c.Nome + " | " + e.Modelo + " | " + o.Status
                    + " | " + o.Aprovacao + " | " + o.Total().ToString("C2", Br)); quantidade++;
            }
            if (quantidade == 0) Console.WriteLine("Nenhuma OS encontrada.");
        }
        static void Cadastros()
        {
            foreach (Cliente c in g.Clientes) Console.WriteLine("Cliente #" + c.Id + " " + c.Nome);
            foreach (Equipamento e in g.Equipamentos) Console.WriteLine("Equipamento #" + e.Id + " " + e.Modelo + " / cliente #" + e.ClienteId);
            foreach (Tecnico t in g.Tecnicos) Console.WriteLine("Técnico #" + t.Id + " " + t.Nome);
            foreach (Servico s in g.Servicos) Console.WriteLine("Serviço #" + s.Id + " " + s.Descricao + " " + s.Valor.ToString("C2", Br));
        }
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("TECHFIX | UNINOVE 2026 | Protótipo acadêmico local");
            if (args.Contains("--demo")) g.CarregarDemonstracao();
            if (args.Contains("--roteiro")) { Demonstrar(); return; }
            int opcao = -1;
            try {
                do {
                    Console.WriteLine("\n1 Cliente | 2 Equipamento | 3 Técnico | 4 Serviço | 5 Abrir OS");
                    Console.WriteLine("6 Listar OS | 7 Diagnóstico | 8 Aprovar/recusar | 9 Avançar status");
                    Console.WriteLine("10 Histórico do cliente | 11 Relatório | 12 Ver cadastros | 0 Sair");
                    opcao = Inteiro("Opção: ", 0, 12);
                    try {
                        switch (opcao) {
                            case 1: g.CadastrarCliente(Texto("Nome: "), Texto("Telefone somente dígitos: ")); Console.WriteLine("Cliente cadastrado."); break;
                            case 2: Cadastros(); g.CadastrarEquipamento(Inteiro("ID cliente: ",1,Int32.MaxValue),Texto("Tipo: "),Texto("Modelo: "),Texto("Série: ")); Console.WriteLine("Equipamento cadastrado."); break;
                            case 3: g.CadastrarTecnico(Texto("Nome: "),Texto("Especialidade: ")); Console.WriteLine("Técnico cadastrado."); break;
                            case 4: g.CadastrarServico(Texto("Descrição: "),Dinheiro("Valor R$: ")); Console.WriteLine("Serviço cadastrado."); break;
                            case 5:
                                Cadastros(); int e=Inteiro("ID equipamento: ",1,Int32.MaxValue),t=Inteiro("ID técnico: ",1,Int32.MaxValue);
                                string p=Texto("Problema: "); bool u=SimNao("Urgente S/N? ");
                                int s=Inteiro("ID serviço: ",1,Int32.MaxValue),q=Inteiro("Quantidade: ",1,1000);
                                OrdemServico nova=g.Abrir(e,t,p,u,s,q); Console.WriteLine("OS #"+nova.Id+" aberta. Total "+nova.Total().ToString("C2",Br)); break;
                            case 6: Listar(g.Ordens); break;
                            case 7: g.Buscar(Inteiro("ID OS: ",1,Int32.MaxValue)).RegistrarDiagnostico(Texto("Diagnóstico: ")); Console.WriteLine("Diagnóstico registrado."); break;
                            case 8: g.Buscar(Inteiro("ID OS: ",1,Int32.MaxValue)).Decidir(SimNao("Cliente aprovou S/N? ")); Console.WriteLine("Decisão registrada pelo atendente."); break;
                            case 9: g.Buscar(Inteiro("ID OS: ",1,Int32.MaxValue)).Avancar(); Console.WriteLine("Status atualizado."); break;
                            case 10: Listar(g.Historico(Inteiro("ID cliente: ",1,Int32.MaxValue))); break;
                            case 11:
                                decimal total=0m; for(int i=0;i<g.Ordens.Count;i++) if(g.Ordens[i].Status!="Cancelada") total+=g.Ordens[i].Total();
                                Console.WriteLine("Total orçado não cancelado: "+total.ToString("C2",Br)+" (não representa recebimentos)"); break;
                            case 12: Cadastros(); break;
                        }
                    } catch (ArgumentException ex) { Console.WriteLine("Validação: "+ex.Message); }
                      catch (InvalidOperationException ex) { Console.WriteLine("Validação: "+ex.Message); }
                } while (opcao != 0);
            } catch(OperationCanceledException) { Console.WriteLine("Entrada encerrada."); }
            Console.WriteLine("Sessão encerrada. Dados em memória não são persistidos.");
        }
        static void Demonstrar()
        {
            g.CarregarDemonstracao();
            Console.WriteLine("DADOS FICTÍCIOS | Demonstração executada pelo código");
            OrdemServico a=g.Abrir(1,2,"Notebook lento",false,1,1);
            a.RegistrarDiagnostico("Sistema necessita reinstalação"); a.Decidir(true); a.Avancar(); a.Avancar();
            OrdemServico b=g.Abrir(2,1,"Tela quebrada",true,2,1);
            b.RegistrarDiagnostico("Substituir tela"); b.Decidir(true); b.Avancar();
            OrdemServico c=g.Abrir(3,1,"Bateria sem carga",false,3,1);
            c.RegistrarDiagnostico("Bateria desgastada"); c.Decidir(false);
            Listar(g.Ordens); Console.WriteLine("Total orçado não cancelado: "+g.Ordens.Where(o=>o.Status!="Cancelada").Sum(o=>o.Total()).ToString("C2",Br));
            Console.WriteLine("Histórico do cliente #1:"); Listar(g.Historico(1));
            try { c.Avancar(); } catch(InvalidOperationException ex) { Console.WriteLine("Bloqueio esperado: "+ex.Message); }
        }
    }
}

