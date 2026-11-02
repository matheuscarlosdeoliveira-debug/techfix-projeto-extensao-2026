using System;
using System.Linq;
using TechFix;
class Testes
{
 static int passou=0;
 static void Verificar(bool condicao,string nome) { if(!condicao) throw new Exception("FALHOU: "+nome); Console.WriteLine("PASSOU | "+nome); passou++; }
 static void Bloqueia(Action acao,string nome) { bool bloqueou=false; try{acao();}catch(ArgumentException){bloqueou=true;}catch(InvalidOperationException){bloqueou=true;} Verificar(bloqueou,nome); }
 public static void Main() {
  Gestao g=new Gestao(); g.CarregarDemonstracao();
  Verificar(g.Clientes.Count==3 && g.Equipamentos.Count==3,"Cadastros relacionados");
  Bloqueia(()=>g.CadastrarCliente(" ","123"),"Nome vazio");
  Bloqueia(()=>g.CadastrarCliente("Outro","11988881234"),"Telefone duplicado");
  Bloqueia(()=>g.CadastrarEquipamento(99,"Celular","X","S99"),"Cliente inexistente");
  Bloqueia(()=>g.CadastrarEquipamento(1,"Celular","X","sn001"),"Série duplicada sem distinção de caixa");
  Bloqueia(()=>g.CadastrarServico("Teste",-1m),"Valor negativo");
  Bloqueia(()=>g.Abrir(99,1,"Falha",false,1,1),"Equipamento inexistente");
  Bloqueia(()=>g.Abrir(1,99,"Falha",false,1,1),"Técnico inexistente");
  Bloqueia(()=>g.Abrir(1,1,"Falha",false,99,1),"Serviço inexistente");
  Bloqueia(()=>g.Abrir(1,1,"Falha",false,1,0),"Quantidade inválida");
  OrdemServico o=g.Abrir(1,2,"Lento",true,1,2);
  Verificar(o.Total()==265m,"Quantidade e taxa fixa de R$ 25,00");
  g.Servicos[0].Valor=999m; Verificar(o.Total()==265m,"Preço histórico preservado");
  Bloqueia(()=>o.Avancar(),"Reparo antes da aprovação");
  Bloqueia(()=>o.Decidir(true),"Aprovação sem diagnóstico");
  o.RegistrarDiagnostico("Reinstalar"); o.Decidir(true); o.Avancar();
  Verificar(o.Status=="Em andamento","Aprovação e início do reparo");
  o.Avancar(); Verificar(o.Status=="Concluída","Conclusão do reparo");
  Bloqueia(()=>o.Avancar(),"Reabertura de OS concluída");
  OrdemServico recusada=g.Abrir(2,1,"Tela",false,2,1); recusada.RegistrarDiagnostico("Trocar"); recusada.Decidir(false);
  Verificar(recusada.Status=="Cancelada","Recusa cancela OS");
  Bloqueia(()=>recusada.Avancar(),"Reparo de OS cancelada");
  Verificar(g.Historico(1).Count()==1 && g.Historico(3).Count()==0,"Histórico filtrado por cliente");
  Bloqueia(()=>g.Buscar(999),"OS inexistente");
  Console.WriteLine("TOTAL: "+passou+" testes aprovados.");
 }
}
