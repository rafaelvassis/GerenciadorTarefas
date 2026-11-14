namespace GerenciadorTarefas.Models;

/// <summary>
/// Dados exibidos na página de erro (<c>Views/Shared/Error.cshtml</c>).
/// </summary>
public class ErrorViewModel
{
    /// <summary>Identificador da requisição que falhou, útil para procurar nos logs.</summary>
    public string? RequestId { get; set; }

    /// <summary>Indica se há um <see cref="RequestId"/> para mostrar.</summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
