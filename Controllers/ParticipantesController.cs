using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorTarefas.Data;
using GerenciadorTarefas.Models;

namespace GerenciadorTarefas.Controllers;

/// <summary>
/// Cadastro de participantes (na tela, "Pessoas"). Regra: o e-mail é único.
/// Excluir um participante só o tira das tarefas; as tarefas continuam existindo.
/// </summary>
public class ParticipantesController : Controller
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Recebe o <see cref="AppDbContext"/> por injeção de dependência.
    /// </summary>
    public ParticipantesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /Participantes: lista os participantes com a quantidade de tarefas de cada um.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var participantes = await _context.Participantes
            .Include(p => p.Tarefas)
            .OrderBy(p => p.Nome)
            .ToListAsync();
        return View(participantes);
    }

    /// <summary>
    /// GET /Participantes/Create: exibe o formulário de novo participante.
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// POST /Participantes/Create: grava o participante, se os dados forem válidos e o e-mail ainda não existir.
    /// </summary>
    /// <param name="participante">Dados do formulário.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Email")] Participante participante)
    {
        // Regra: o e-mail é único
        if (await _context.Participantes.AnyAsync(p => p.Email == participante.Email))
        {
            ModelState.AddModelError("Email", "Já existe um participante com esse e-mail.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(participante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(participante);
    }

    /// <summary>
    /// POST /Participantes/CriarRapido: cadastra um participante a partir do formulário de tarefa,
    /// sem sair da página (chamado via JavaScript). Aplica as mesmas regras do cadastro normal.
    /// </summary>
    /// <param name="participante">Nome e e-mail informados.</param>
    /// <returns>
    /// JSON <c>{ id, nome }</c> com o participante criado, ou status 400 com <c>{ erro }</c>.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CriarRapido([Bind("Nome,Email")] Participante participante)
    {
        // Regra: o e-mail é único
        if (await _context.Participantes.AnyAsync(p => p.Email == participante.Email))
        {
            ModelState.AddModelError("Email", "Já existe um participante com esse e-mail.");
        }

        if (!ModelState.IsValid)
        {
            var erro = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
            return BadRequest(new { erro });
        }

        _context.Add(participante);
        await _context.SaveChangesAsync();
        return Json(new { id = participante.IdParticipante, nome = participante.Nome });
    }

    /// <summary>
    /// GET /Participantes/Edit/{id}: exibe o formulário de edição.
    /// </summary>
    /// <param name="id">Id do participante.</param>
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var participante = await _context.Participantes.FindAsync(id);
        if (participante == null)
        {
            return NotFound();
        }
        return View(participante);
    }

    /// <summary>
    /// POST /Participantes/Edit/{id}: salva as alterações, se o e-mail não pertencer a outro participante.
    /// </summary>
    /// <param name="id">Id do participante (vem da URL).</param>
    /// <param name="participante">Dados do formulário.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("IdParticipante,Nome,Email")] Participante participante)
    {
        if (id != participante.IdParticipante)
        {
            return NotFound();
        }

        // Regra: o e-mail é único (ignorando o próprio participante)
        if (await _context.Participantes.AnyAsync(p => p.Email == participante.Email && p.IdParticipante != participante.IdParticipante))
        {
            ModelState.AddModelError("Email", "Já existe um participante com esse e-mail.");
        }

        if (ModelState.IsValid)
        {
            _context.Update(participante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(participante);
    }

    /// <summary>
    /// GET /Participantes/Delete/{id}: pede a confirmação e avisa se o participante está em tarefas.
    /// </summary>
    /// <param name="id">Id do participante.</param>
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var participante = await _context.Participantes
            .Include(p => p.Tarefas)
            .FirstOrDefaultAsync(p => p.IdParticipante == id);
        if (participante == null)
        {
            return NotFound();
        }

        return View(participante);
    }

    /// <summary>
    /// POST /Participantes/Delete/{id}: exclui o participante. Ele sai das tarefas em que estava;
    /// as tarefas continuam existindo.
    /// </summary>
    /// <param name="id">Id do participante.</param>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var participante = await _context.Participantes
            .Include(p => p.Tarefas)
            .FirstOrDefaultAsync(p => p.IdParticipante == id);
        if (participante != null)
        {
            _context.Participantes.Remove(participante);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
