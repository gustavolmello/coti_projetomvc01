using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjetoAspNetMVC01.Messages.Models;
using ProjetoAspNetMVC01.Messages.Services;
using ProjetoAspNetMVC01.Presentation.Models;
using ProjetoAspNetMVC01.Repository.Entities;
using ProjetoAspNetMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Presentation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginModel model, [FromServices] IUsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //buscar o usuario no banco de dados atraves do email e da senha
                    var usuario = usuarioRepository.Obter(model.Email, model.Senha);

                    //verificar se o usuario foi encontrado
                    if (usuario != null)
                    {
                        //criando a identificação do usuario para o projeto AspNet
                        var identificacao = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario.Email) },
                        CookieAuthenticationDefaults.AuthenticationScheme);

                        //gravar um COOKIE com a permissão de acesso para o usuario
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identificacao));

                        //redirecionamento..
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception("Acesso negado, usuário inválido.");
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegisterModel model, [FromServices] IUsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //verificar se o email informado já esta cadastrado no banco de dados
                    if (usuarioRepository.Obter(model.Email) != null)
                    {
                        throw new Exception($"O email {model.Email} já está cadastrado no sistema, tente outro.");
                    }
                    else
                    {
                        var usuario = new Usuario();

                        usuario.IdUsuario = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = model.Senha;
                        usuario.DataCadastro = DateTime.Now;

                        usuarioRepository.Inserir(usuario);

                        TempData["MensagemSucesso"] = $"Parabéns {usuario.Nome}, sua conta de usuário foi criada com sucesso.";
                        ModelState.Clear();
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult PasswordRecover()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordRecover(AccountPasswordRecoverModel model,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //buscar o usuario no banco de dados atraves do email..
                    var usuario = usuarioRepository.Obter(model.Email);

                    //verificando se o usuario foi encontrado
                    if (usuario != null)
                    {
                        //gerar uma nova senha para o usuario e alterar no banco de dados
                        usuario.Senha = new Random().Next(99999999, 999999999).ToString();
                        usuarioRepository.Alterar(usuario);

                        //Criando o conteudo do email
                        var emailModel = new EmailModel
                        {
                            EmailDestinatario = usuario.Email,
                            Assunto = "Nova senha gerada com sucesso - Sistema de Tarefas COTI Informática.",
                            Mensagem = $@"
                            <div style='text-align: center; margin: 40px; padding: 60px; border: 2px solid #ccc; font-size: 16pt;'>
                            <img src='https://www.cotiinformatica.com.br/imagens/logo-coti-informatica.png' />
                            <br/><br/>
                            Olá <strong>{usuario.Nome}</strong>,
                            <br/><br/>    
                            O sistema gerou uma nova senha para que você possa acessar sua conta.<br/>
                            Por favor utilize a senha: <strong>{usuario.Senha}</strong>
                            <br/><br/>
                            Não esqueça de, ao acessar o sistema, atualizar esta senha para outra
                            de sua preferência.
                            <br/><br/>              
                            Att<br/>   
                            Equipe COTI Informatica
                            </div>
                        "
                        };

                        //enviando um email para o usuario com a nova senha..
                        var emailService = new EmailService();
                        emailService.EnviarMensagem(emailModel);

                        TempData["MensagemSucesso"] = $"Uma nova senha foi gerada com sucesso e enviada para o email {usuario.Email}.";
                        ModelState.Clear();
                    }
                    else
                    {
                        TempData["MensagemErro"] = "O email informado não está cadastrado.";
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult Logout()
        {
            //Apagar o Cookie de autenticação criado para o usuario
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionar o usuario para a página de login do sistema
            return RedirectToAction("Login", "Account");
        }
    }
}





