using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefas.Models
{
    /// <summary>
    /// Categoria usada para classificar as tarefas (tabela <c>Categorias</c>).
    /// O nome é único, e uma categoria com tarefas não pode ser excluída.
    /// </summary>
    public class Categoria
    {
        /// <summary>Chave primária, gerada pelo banco com <c>NEWID()</c>.</summary>
        public Guid IdCategoria { get; set; }

        /// <summary>Nome da categoria (obrigatório e único).</summary>
        [Required(ErrorMessage = "Informe o nome da categoria.")]
        [StringLength(150, ErrorMessage = "O nome pode ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>Tarefas desta categoria.</summary>
        public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    }
}
