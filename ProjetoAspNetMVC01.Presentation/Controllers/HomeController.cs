using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoAspNetMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //método utilizado para criar a página
        public IActionResult Index(
            [FromServices] ITarefaRepository tarefaRepository,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            try
            {
                //obter o usuario autenticado no sistema
                var usuario = usuarioRepository.Obter(User.Identity.Name);

                //consultar todas as tarefas do usuario autenticado
                var tarefas = tarefaRepository.ConsultarPorUsuario(usuario.IdUsuario);

                //calcular a quantidade de cada prioridade
                TempData["PrioridadeAlta"] = tarefas.Count(t => t.Prioridade.Equals("ALTA"));
                TempData["PrioridadeMedia"] = tarefas.Count(t => t.Prioridade.Equals("MEDIA"));
                TempData["PrioridadeBaixa"] = tarefas.Count(t => t.Prioridade.Equals("BAIXA"));
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View();
        }
    }
}





