using Microsoft.AspNetCore.Mvc;
using ProjetoAspNetMVC01.Presentation.Models;
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

        [HttpPost] //Indica que o método é executado peo botão SUBMIT do formulário
        public IActionResult Cadastro(TarefasCadastroModel model)
        {
            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        public IActionResult Relatorio()
        {
            return View();
        }
    }
}
