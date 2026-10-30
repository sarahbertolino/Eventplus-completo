using System.Threading.Tasks;

namespace EventPlus.WebAPI.Services;

/// <summary>
/// Interface que define o contrato para moderação de conteúdo (análise de texto e termos impróprios).
/// 
/// CONCEITO PARA ALUNOS:
/// A moderação de conteúdo é utilizada para verificar se o texto de um comentário ou publicação 
/// contém palavras ofensivas, discursos de ódio ou conteúdo impróprio antes de ser exibido na API.
/// Esta interface permite plugar soluções como Azure AI Content Safety, Google Perspective API ou moderadores locais.
/// </summary>
public interface IContentModerationService
{
    /// <summary>
    /// Analisa um texto e determina se ele contém conteúdo impróprio.
    /// </summary>
    /// <param name="texto">Texto que será analisado (ex: comentário do usuário).</param>
    /// <returns>
    /// Retorna um objeto do tipo <see cref="ResultadoModeracao"/> contendo se o texto é válido 
    /// e a justificativa caso seja bloqueado.
    /// </returns>
    Task<ResultadoModeracao> ModerarTextoAsync(string texto);
}

/// <summary>
/// Estrutura que representa o resultado da análise de moderação de conteúdo.
/// </summary>
public class ResultadoModeracao
{
    /// <summary>
    /// Indica se o conteúdo é apropriado (true) ou impróprio (false).
    /// </summary>
    public bool EhApropriado { get; set; }

    /// <summary>
    /// Motivo ou mensagem informativa sobre a moderação realizada.
    /// </summary>
    public string Motivo { get; set; } = string.Empty;
}
