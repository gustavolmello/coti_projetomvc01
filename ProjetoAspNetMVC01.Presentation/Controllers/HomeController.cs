using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }
    }
}


