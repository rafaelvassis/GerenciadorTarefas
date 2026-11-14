using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefas.Models
{
    /// <summary>
    /// Situação de uma tarefa. O valor numérico é o que fica gravado na coluna <c>Status</c>
    /// (a constraint <c>CK_Tarefas_Status</c> só aceita 0, 1 e 2).
    /// </summary>
    public enum EStatus
    {
        /// <summary>Tarefa ainda não iniciada.</summary>
        [Display(Name = "Pendente")]
        Pendente = 0,

        /// <summary>Tarefa em execução.</summary>
        [Display(Name = "Em Andamento")]
        EmAndamento = 1,

        /// <summary>Tarefa finalizada.</summary>
        [Display(Name = "Concluída")]
        Concluida = 2
    }
}
