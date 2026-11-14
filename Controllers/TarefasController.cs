using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorTarefas.Models;
using GerenciadorTarefas.Data;

namespace GerenciadorTarefas.Controllers;

/// <summary>
/// CRUD de tarefas: listagem com filtros, detalhes, cadastro, edição e exclusão.
/// </summary>
public class TarefasController : Controller
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Recebe o <see cref="AppDbContext"/> por injeção de dependência.
    /// </summary>
    public TarefasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /Tarefas: lista as tarefas ordenadas pelo vencimento, com filtros opcionais
    /// combináveis (ex.: <c>/Tarefas?participante={id}&amp;status=1</c>).
    /// </summary>
    /// <param name="participante">Mostra só as tarefas deste participante.</param>
    /// <param name="categoria">Mostra só as tarefas desta categoria.</param>
    /// <param name="status">Mostra só as tarefas com este status.</param>
    public async Task<IActionResult> Index(Guid? participante, Guid? categoria, EStatus? status)
    {
        var consulta = _context.Tarefas
            .Include(t => t.Categoria)
            .Include(t => t.Participantes)
            .AsQueryable();

        if (participante.HasValue)
        {
            consulta = consulta.Where(t => t.Participantes.Any(p => p.IdParticipante == participante));
        }
        if (categoria.HasValue)
        {
            consulta = consulta.Where(t => t.FK_IdCategoria == categoria);
        }
        if (status.HasValue)
        {
            consulta = consulta.Where(t => t.Status == status);
        }

        // Opções dos filtros, já com o valor escolhido selecionado
        ViewBag.FiltroParticipante = new SelectList(
            _context.Participantes.OrderBy(p => p.Nome), "IdParticipante", "Nome", participante);
        ViewBag.FiltroCategoria = new SelectList(
            _context.Categorias.OrderBy(c => c.Nome), "IdCategoria", "Nome", categoria);
        ViewBag.FiltroStatus = new SelectList(
            Enum.GetValues<EStatus>().Select(s => new { Valor = s, Nome = s.String() }), "Valor", "Nome", status);

        var tarefas = await consulta.OrderBy(t => t.DataVencimento).ToListAsync();
        return View(tarefas);
    }

    /// <summary>
    /// GET /Tarefas/Details/{id}: mostra uma tarefa com a categoria e os participantes.
    /// </summary>
    /// <param name="id">Id da tarefa.</param>
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tarefa = await _context.Tarefas
            .Include(t => t.Categoria)
            .Include(t => t.Participantes)
            .FirstOrDefaultAsync(m => m.IdTarefa == id);
        if (tarefa == null)
        {
            return NotFound();
        }

        return View(tarefa);
    }

    /// <summary>
    /// GET /Tarefas/Create: exibe o formulário de nova tarefa.
    /// </summary>
    public IActionResult Create()
    {
        CarregarListas();
        return View();
    }

    /// <summary>
    /// POST /Tarefas/Create: valida e grava a nova tarefa junto com os participantes marcados.
    /// </summary>
    /// <param name="tarefa">Dados do formulário (só os campos listados no <c>Bind</c>).</param>
    /// <param name="participantesIds">Ids dos participantes marcados (pode vir vazio).</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Titulo,Descricao,DataVencimento,Status,FK_IdCategoria")] Tarefa tarefa,
        Guid[] participantesIds)
    {
        if (ModelState.IsValid)
        {
            tarefa.Participantes = await _context.Participantes
                .Where(p => participantesIds.Contains(p.IdParticipante))
                .ToListAsync();

            _context.Add(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        CarregarListas(participantesIds);
        return View(tarefa);
    }

    /// <summary>
    /// GET /Tarefas/Edit/{id}: exibe o formulário de edição com os participantes atuais marcados.
    /// </summary>
    /// <param name="id">Id da tarefa.</param>
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tarefa = await _context.Tarefas
            .Include(t => t.Participantes)
            .FirstOrDefaultAsync(t => t.IdTarefa == id);
        if (tarefa == null)
        {
            return NotFound();
        }
        CarregarListas(tarefa.Participantes.Select(p => p.IdParticipante));
        return View(tarefa);
    }

    /// <summary>
    /// POST /Tarefas/Edit/{id}: atualiza os dados da tarefa e sincroniza os participantes,
    /// tirando quem foi desmarcado e incluindo quem foi marcado.
    /// </summary>
    /// <param name="id">Id da tarefa (vem da URL).</param>
    /// <param name="tarefa">Dados do formulário.</param>
    /// <param name="participantesIds">Ids dos participantes marcados (pode vir vazio).</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        [Bind("IdTarefa,Titulo,Descricao,DataVencimento,Status,FK_IdCategoria")] Tarefa tarefa,
        Guid[] participantesIds)
    {
        if (id != tarefa.IdTarefa)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existente = await _context.Tarefas
                .Include(t => t.Participantes)
                .FirstOrDefaultAsync(t => t.IdTarefa == id);
            if (existente == null)
            {
                return NotFound();
            }

            existente.Titulo = tarefa.Titulo;
            existente.Descricao = tarefa.Descricao;
            existente.DataVencimento = tarefa.DataVencimento;
            existente.Status = tarefa.Status;
            existente.FK_IdCategoria = tarefa.FK_IdCategoria;

            // Tira quem foi desmarcado e adiciona quem foi marcado
            foreach (var p in existente.Participantes.Where(p => !participantesIds.Contains(p.IdParticipante)).ToList())
            {
                existente.Participantes.Remove(p);
            }
            var atuais = existente.Participantes.Select(p => p.IdParticipante).ToList();
            var novos = await _context.Participantes
                .Where(p => participantesIds.Contains(p.IdParticipante) && !atuais.Contains(p.IdParticipante))
                .ToListAsync();
            foreach (var p in novos)
            {
                existente.Participantes.Add(p);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        CarregarListas(participantesIds);
        return View(tarefa);
    }

    /// <summary>
    /// GET /Tarefas/Delete/{id}: pede a confirmação da exclusão.
    /// </summary>
    /// <param name="id">Id da tarefa.</param>
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(m => m.IdTarefa == id);
        if (tarefa == null)
        {
            return NotFound();
        }

        return View(tarefa);
    }

    /// <summary>
    /// POST /Tarefas/Delete/{id}: exclui a tarefa. Os vínculos com participantes
    /// são removidos junto (exclusão em cascata).
    /// </summary>
    /// <param name="id">Id da tarefa.</param>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid? id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa != null)
        {
            _context.Tarefas.Remove(tarefa);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Preenche o select de categorias e a lista de participantes dos formulários de tarefa.
    /// </summary>
    /// <param name="participantesMarcados">Ids que devem aparecer marcados (ou nenhum).</param>
    private void CarregarListas(IEnumerable<Guid>? participantesMarcados = null)
    {
        ViewBag.Categorias = new SelectList(
            _context.Categorias.OrderBy(c => c.Nome), "IdCategoria", "Nome");
        ViewBag.Participantes = _context.Participantes.OrderBy(p => p.Nome).ToList();
        ViewBag.ParticipantesMarcados = (participantesMarcados ?? Enumerable.Empty<Guid>()).ToHashSet();
    }
}
