using Microsoft.AspNetCore.Mvc;
using ProjetoAspNetMVC01.Presentation.Models;
using ProjetoAspNetMVC01.Repository.Entities;
using ProjetoAspNetMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Presentation.Controllers
{
    public class TarefasController : Controller
    {
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost] //Indica que o método é executado pelo botão SUBMIT do formulário
        public IActionResult Cadastro(TarefasCadastroModel model, [FromServices] ITarefaRepository tarefaRepository)
        {
            //verificar se todos os campos passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    //criando um objeto da classe Tarefa (entidade)
                    var tarefa = new Tarefa();

                    //transferir os dados da model para a entidade
                    tarefa.IdTarefa = Guid.NewGuid();
                    tarefa.Nome = model.Nome;
                    tarefa.Data = DateTime.Parse(model.Data);
                    tarefa.Hora = TimeSpan.Parse(model.Hora);
                    tarefa.Descricao = model.Descricao;
                    tarefa.Prioridade = model.Prioridade;

                    //gravando no banco de dados
                    tarefaRepository.Inserir(tarefa);

                    //enviar mensagem para a página
                    TempData["MensagemSucesso"] = $"Tarefa {tarefa.Nome}, cadastrada com sucesso.";

                    //limpar os campos do formulário
                    ModelState.Clear();
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = $"Erro {e.Message}";
                }
            }

            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost] //Indica que o método é executado pelo botão SUBMIT do formulário
        public IActionResult Consulta(TarefasConsultaModel model, [FromServices] ITarefaRepository tarefaRepository)
        {
            //verificar não há erros de validação no preenchimento dos campos
            if(ModelState.IsValid)
            {
                try
                {
                    //realizando a consulta de tarefas
                    model.Tarefas = tarefaRepository.ConsultarPorDatas
                        (DateTime.Parse(model.DataMin), DateTime.Parse(model.DataMax));
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = $"Erro: {e.Message}";
                }
            }

            //enviando os dados para a página..
            return View(model);
        }

        public IActionResult Relatorio()
        {
            return View();
        }
    }
}
