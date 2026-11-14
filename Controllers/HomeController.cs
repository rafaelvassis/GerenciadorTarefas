using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GerenciadorTarefas.Models;

namespace GerenciadorTarefas.Controllers;

/// <summary>
/// Páginas institucionais: início, sobre o projeto e erro.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// GET /: página inicial, com atalhos para o gerenciador de tarefas.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// GET /sobre: explicação do projeto, da arquitetura e do modelo relacional.
    /// </summary>
    [Route("sobre")]
    public IActionResult Sobre()
    {
        return View();
    }

    /// <summary>
    /// Página de erro exibida fora do ambiente de desenvolvimento (configurada no <c>Program.cs</c>).
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
