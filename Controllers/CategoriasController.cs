using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorTarefas.Data;
using GerenciadorTarefas.Models;

namespace GerenciadorTarefas.Controllers;

/// <summary>
/// Cadastro de categorias. Regras: o nome é único, e uma categoria com tarefas não pode ser excluída.
/// </summary>
public class CategoriasController : Controller
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Recebe o <see cref="AppDbContext"/> por injeção de dependência.
    /// </summary>
    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /Categorias: lista as categorias com a quantidade de tarefas de cada uma.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var categorias = await _context.Categorias
            .Include(c => c.Tarefas)
            .OrderBy(c => c.Nome)
            .ToListAsync();
        return View(categorias);
    }

    /// <summary>
    /// GET /Categorias/Create: exibe o formulário de nova categoria.
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// POST /Categorias/Create: grava a categoria, se o nome for válido e ainda não existir.
    /// </summary>
    /// <param name="categoria">Dados do formulário.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome")] Categoria categoria)
    {
        // Regra: o nome da categoria é único
        if (await _context.Categorias.AnyAsync(c => c.Nome == categoria.Nome))
        {
            ModelState.AddModelError("Nome", "Já existe uma categoria com esse nome.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    /// <summary>
    /// GET /Categorias/Edit/{id}: exibe o formulário de edição.
    /// </summary>
    /// <param name="id">Id da categoria.</param>
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return View(categoria);
    }

    /// <summary>
    /// POST /Categorias/Edit/{id}: salva o novo nome, se não for igual ao de outra categoria.
    /// </summary>
    /// <param name="id">Id da categoria (vem da URL).</param>
    /// <param name="categoria">Dados do formulário.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("IdCategoria,Nome")] Categoria categoria)
    {
        if (id != categoria.IdCategoria)
        {
            return NotFound();
        }

        // Regra: o nome da categoria é único (ignorando a própria categoria)
        if (await _context.Categorias.AnyAsync(c => c.Nome == categoria.Nome && c.IdCategoria != categoria.IdCategoria))
        {
            ModelState.AddModelError("Nome", "Já existe uma categoria com esse nome.");
        }

        if (ModelState.IsValid)
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    /// <summary>
    /// GET /Categorias/Delete/{id}: pede a confirmação, ou avisa que a exclusão não é
    /// possível porque a categoria tem tarefas.
    /// </summary>
    /// <param name="id">Id da categoria.</param>
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias
            .Include(c => c.Tarefas)
            .FirstOrDefaultAsync(c => c.IdCategoria == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    /// <summary>
    /// POST /Categorias/Delete/{id}: exclui a categoria, desde que ela não tenha tarefas.
    /// </summary>
    /// <param name="id">Id da categoria.</param>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        // Regra: não é possível excluir uma categoria que tenha tarefas
        if (await _context.Tarefas.AnyAsync(t => t.FK_IdCategoria == id))
        {
            return RedirectToAction(nameof(Delete), new { id });
        }

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
