namespace GerenciadorTarefas.Models
{
    /// <summary>
    /// Métodos auxiliares do enum <see cref="EStatus"/>.
    /// </summary>
    public static class EStatusExtensions
    {
        /// <summary>
        /// Devolve o nome do status para exibir na tela (por exemplo, "Em Andamento" em vez de "EmAndamento").
        /// </summary>
        /// <param name="status">Status da tarefa.</param>
        /// <returns>O texto do status, ou "Desconhecido" para um valor fora do enum.</returns>
        public static string String(this EStatus status)
        {
            return status switch
            {
                EStatus.Pendente => "Pendente",
                EStatus.EmAndamento => "Em Andamento",
                EStatus.Concluida => "Concluída",
                _ => "Desconhecido"
            };
        }
    }
}
