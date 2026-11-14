using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefas.Models
{
    /// <summary>
    /// Pessoa que pode participar de tarefas (tabela <c>Participantes</c>).
    /// O e-mail é único.
    /// </summary>
    public class Participante
    {
        /// <summary>Chave primária, gerada pelo banco com <c>NEWID()</c>.</summary>
        public Guid IdParticipante { get; set; }

        /// <summary>Nome do participante (obrigatório).</summary>
        [Required(ErrorMessage = "Informe o nome do participante.")]
        [StringLength(150, ErrorMessage = "O nome pode ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>E-mail do participante (obrigatório e único).</summary>
        [Required(ErrorMessage = "Informe o e-mail.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(255, ErrorMessage = "O e-mail pode ter no máximo 255 caracteres.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Tarefas em que a pessoa participa (relação N:N).</summary>
        public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    }
}
