using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefas.Models
{
    /// <summary>
    /// Tarefa cadastrada no sistema (tabela <c>Tarefas</c>).
    /// Pertence a uma categoria e pode ter vários participantes, ou nenhum.
    /// </summary>
    public class Tarefa
    {
        /// <summary>Chave primária, gerada pelo banco com <c>NEWID()</c>.</summary>
        public Guid IdTarefa { get; set; }

        /// <summary>Título da tarefa (obrigatório, até 200 caracteres).</summary>
        [Display(Name = "Título")]
        [Required(ErrorMessage = "Informe o título da tarefa.")]
        [StringLength(200, ErrorMessage = "O título pode ter no máximo 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>Descrição opcional (até 1000 caracteres).</summary>
        [Display(Name = "Descrição")]
        [StringLength(1000, ErrorMessage = "A descrição pode ter no máximo 1000 caracteres.")]
        public string? Descricao { get; set; }

        /// <summary>Data limite para concluir a tarefa.</summary>
        [Display(Name = "Vencimento")]
        [Required(ErrorMessage = "Informe a data de vencimento.")]
        [DataType(DataType.Date)]
        public DateTime DataVencimento { get; set; }

        /// <summary>Situação atual da tarefa.</summary>
        public EStatus Status { get; set; }

        /// <summary>Data e hora do cadastro, preenchida pelo banco com <c>SYSDATETIME()</c>.</summary>
        [Display(Name = "Criada em")]
        public DateTime DataCriacao { get; set; }

        /// <summary>Chave estrangeira da categoria (obrigatória).</summary>
        [Display(Name = "Categoria")]
        public Guid FK_IdCategoria { get; set; }

        /// <summary>Categoria da tarefa.</summary>
        public Categoria? Categoria { get; set; }

        /// <summary>
        /// Participantes da tarefa. A relação N:N fica na tabela <c>TarefasParticipantes</c>.
        /// </summary>
        public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
    }
}
