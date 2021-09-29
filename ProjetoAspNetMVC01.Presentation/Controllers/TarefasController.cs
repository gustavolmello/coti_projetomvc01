using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoAspNetMVC01.Presentation.Models;
using ProjetoAspNetMVC01.Reports.Excel;
using ProjetoAspNetMVC01.Reports.Pdf;
using ProjetoAspNetMVC01.Repository.Entities;
using ProjetoAspNetMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Presentation.Controllers
{
    [Authorize]
    public class TarefasController : Controller
    {
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost] //Indica que o método é executado pelo botão SUBMIT do formulário
        public IActionResult Cadastro(TarefasCadastroModel model,
            [FromServices] ITarefaRepository tarefaRepository,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            //verificar se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //obter os dados do usuario autenticado no sistema
                    var usuario = usuarioRepository.Obter(User.Identity.Name);

                    //criando um objeto da classe Tarefa (entidade)
                    var tarefa = new Tarefa();

                    //transferir os dados da model para a entidade
                    tarefa.IdTarefa = Guid.NewGuid();
                    tarefa.Nome = model.Nome;
                    tarefa.Data = DateTime.Parse(model.Data);
                    tarefa.Hora = TimeSpan.Parse(model.Hora);
                    tarefa.Descricao = model.Descricao;
                    tarefa.Prioridade = model.Prioridade;
                    tarefa.IdUsuario = usuario.IdUsuario; //foreign key

                    //gravando no banco de dados
                    tarefaRepository.Inserir(tarefa);

                    //enviar mensagem para a página
                    TempData["MensagemSucesso"] = $"Tarefa {tarefa.Nome}, cadastrada com sucesso.";

                    //limpar os campos do formulário
                    ModelState.Clear();
                }
                catch (Exception e)
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
        public IActionResult Consulta(TarefasConsultaModel model,
            [FromServices] ITarefaRepository tarefaRepository,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            //verificar não há erros de validação no preenchimento dos campos
            if (ModelState.IsValid)
            {
                try
                {
                    //obter os dados do usuario autenticado no sistema
                    var usuario = usuarioRepository.Obter(User.Identity.Name);

                    //realizando a consulta de tarefas
                    model.Tarefas = tarefaRepository.ConsultarPorDatas
                        (DateTime.Parse(model.DataMin), DateTime.Parse(model.DataMax), usuario.IdUsuario);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro: {e.Message}";
                }
            }

            //enviando os dados para a página..
            return View(model);
        }

        public IActionResult Edicao(Guid id, [FromServices] ITarefaRepository tarefaRepository)
        {
            var model = new TarefasEdicaoModel();

            try
            {
                //consultar os dados da tarefa no banco atraves do ID..
                var tarefa = tarefaRepository.ObterPorId(id);

                //transferir os dados da tarefa para a classe model
                model.IdTarefa = tarefa.IdTarefa;
                model.Nome = tarefa.Nome;
                model.Data = tarefa.Data.ToString("yyyy-MM-dd");
                model.Hora = tarefa.Hora.ToString(@"hh\:mm");
                model.Descricao = tarefa.Descricao;
                model.Prioridade = tarefa.Prioridade;
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Erro: {e.Message}";
            }

            return View(model);
        }

        [HttpPost] //recebe os dados enviados pelo formulário (SUBMIT)
        public IActionResult Edicao(TarefasEdicaoModel model,
            [FromServices] ITarefaRepository tarefaRepository,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //obter os dados do usuario autenticado
                    var usuario = usuarioRepository.Obter(User.Identity.Name);

                    var tarefa = new Tarefa();

                    tarefa.IdTarefa = model.IdTarefa;
                    tarefa.Nome = model.Nome;
                    tarefa.Data = DateTime.Parse(model.Data);
                    tarefa.Hora = TimeSpan.Parse(model.Hora);
                    tarefa.Descricao = model.Descricao;
                    tarefa.Prioridade = model.Prioridade;
                    tarefa.IdUsuario = usuario.IdUsuario; //foreign key

                    tarefaRepository.Alterar(tarefa);

                    TempData["MensagemSucesso"] = $"Tarefa {tarefa.Nome} atualizada com sucesso.";
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro: {e.Message}";
                }
            }

            return View();
        }

        public IActionResult Exclusao(Guid id,
            [FromServices] ITarefaRepository tarefaRepository,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            try
            {
                //obter os dados do usuario autenticado
                var usuario = usuarioRepository.Obter(User.Identity.Name);

                //consultar a tarefa no banco de dados atraves do ID..
                var tarefa = tarefaRepository.ObterPorId(id);
                tarefa.IdUsuario = usuario.IdUsuario;

                //excluindo a tarefa no banco de dados..
                tarefaRepository.Excluir(tarefa);

                TempData["MensagemSucesso"] = $"Tarefa {tarefa.Nome}, excluída com sucesso.";
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Erro: {e.Message}";
            }

            //redirecionamento para a página de consulta
            return RedirectToAction("Consulta");
        }

        public IActionResult Relatorio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Relatorio(TarefasRelatorioModel model,
            [FromServices] ITarefaRepository tarefaRepository,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //obter os dados do usuario autenticado
                    var usuario = usuarioRepository.Obter(User.Identity.Name);

                    //consultar as tarefas pelo periodo de datas
                    var tarefas = tarefaRepository.ConsultarPorDatas
                        (DateTime.Parse(model.DataMin), DateTime.Parse(model.DataMax), usuario.IdUsuario);

                    //verificar o formato de relatorio selecionado..
                    switch (model.Formato)
                    {
                        case "EXCEL":

                            //gerar o aquivo do relatorio excel..
                            var excel = TarefasReportExcel.Create
                                (DateTime.Parse(model.DataMin), DateTime.Parse(model.DataMax), usuario, tarefas);

                            //DOWNLOAD DO ARQUIVO
                            Response.Clear();
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Headers.Add("content-disposition", "attachment; filename=tarefas.xlsx");
                            Response.Body.WriteAsync(excel, 0, excel.Length);
                            Response.Body.Flush();
                            Response.StatusCode = StatusCodes.Status200OK;

                            break;

                        case "PDF":

                            //gerar o aquivo do relatorio pdf..
                            var pdf = TarefasReportPdf.Create
                                (DateTime.Parse(model.DataMin), DateTime.Parse(model.DataMax), usuario, tarefas);

                            //DOWNLOAD DO ARQUIVO
                            Response.Clear();
                            Response.ContentType = "application/pdf";
                            Response.Headers.Add("content-disposition", "attachment; filename=tarefas.pdf");
                            Response.Body.WriteAsync(pdf, 0, pdf.Length);
                            Response.Body.Flush();
                            Response.StatusCode = StatusCodes.Status200OK;

                            break;
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }
    }
}





