using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EventPlus.WebAPI.Services;

/// <summary>
/// Interface que define o contrato para os serviços de armazenamento de imagens na nuvem.
/// 
/// O QUE É UMA INTERFACE?
/// Na orientação a objetos, uma interface funciona como um "contrato". Ela declara quais 
/// métodos um serviço deve ter, mas não como eles são implementados. Isso permite trocar 
/// a ferramenta de armazenamento (ex: Azure Blob Storage, Google Cloud Storage, Amazon S3) 
/// sem precisar alterar o restante do código da API.
/// </summary>
public interface IImageStorageService
{
    /// <summary>
    /// Envia uma imagem enviada pelo cliente para o serviço de armazenamento em nuvem e retorna a URL pública gerada.
    /// </summary>
    /// <param name="arquivo">Arquivo de imagem enviado através do formulário de requisição (IFormFile).</param>
    /// <returns>Retorna uma string contendo a URL pública da imagem salva na nuvem.</returns>
    Task<string> UploadImageAsync(IFormFile arquivo);

    /// <summary>
    /// Remove uma imagem da nuvem com base em sua URL ou identificador.
    /// </summary>
    /// <param name="imageUrl">URL completa ou caminho da imagem a ser removida.</param>
    /// <returns>Retorna true se a exclusão for realizada com sucesso; caso contrário, false.</returns>
    Task<bool> DeleteImageAsync(string imageUrl);
}
