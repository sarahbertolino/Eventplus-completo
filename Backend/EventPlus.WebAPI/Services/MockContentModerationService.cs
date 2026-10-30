using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventPlus.WebAPI.Services;

/// <summary>
/// Serviço de moderação de conteúdo simulado (Mock/Stub).
/// 
/// NOTA PARA PROFESSOR E ALUNOS:
/// Esta classe é uma implementação inicial simples preparada para a aula.
/// Caso decidam utilizar o Azure AI Content Safety ou o Google Content Moderation (Perspective API):
/// 1. Crie uma nova classe implementando <see cref="IContentModerationService"/> (ex: AzureContentModerationService).
/// 2. Injete a chave e endpoint da API do fornecedor escolhido.
/// 3. Altere o registro na Injeção de Dependência no Program.cs.
/// </summary>
public class MockContentModerationService : IContentModerationService
{
    // Lista didática simples de palavras bloqueadas para teste sem necessidade de chave de API externa
    private readonly List<string> _palavrasBloqueadas = new List<string>
    {
        "palavrao",
        "ofensa",
        "inapropriado",
        "spam",
        "hack"
    };

    /// <summary>
    /// Analisa se o texto fornecido contém palavras banidas ou termos ofensivos.
    /// </summary>
    /// <param name="texto">Texto do comentário ou descrição enviado.</param>
    /// <returns>Resultado com indicação de aprovação ou reprovação.</returns>
    public async Task<ResultadoModeracao> ModerarTextoAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return new ResultadoModeracao
            {
                EhApropriado = true,
                Motivo = "Texto vazio ou nulo."
            };
        }

        await Task.Delay(50); // Simulação da chamada assíncrona de API externa

        string textoMinusculo = texto.ToLowerInvariant();

        // Verifica se alguma palavra da lista proibida está presente no texto
        foreach (var palavra in _palavrasBloqueadas)
        {
            if (textoMinusculo.Contains(palavra))
            {
                return new ResultadoModeracao
                {
                    EhApropriado = false,
                    Motivo = $"O conteúdo contém termos impróprios ('{palavra}')."
                };
            }
        }

        return new ResultadoModeracao
        {
            EhApropriado = true,
            Motivo = "Conteúdo analisado e aprovado."
        };
    }
}
