using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventPlus.WebAPI.Services;

/// <summary>
/// Serviço responsável pelo gerenciamento de imagens em nuvem (ex: Azure Blob Storage, AWS S3 ou Google Cloud Storage).
/// 
/// CONCEITO PARA ALUNOS:
/// Em aplicações profissionais, as imagens NUNCA são salvas na própria pasta do servidor web ou no banco de dados.
/// Elas são enviadas para um serviço de armazenamento de arquivos em nuvem (Object Storage/Blob Storage).
/// O banco de dados guarda apenas o link/endereço (URL) de onde a imagem está hospedada.
/// 
/// NOTA DE IMPLEMENTAÇÃO:
/// Esta classe é uma estrutura preparada (Stub/Mock). Quando for definida a ferramenta de nuvem definitiva 
/// (ex: Azure.Storage.Blobs com BlobContainerClient), substitua o código deste método pela chamada SDK oficial.
/// </summary>
public class CloudStorageServiceStub : IImageStorageService
{
    /// <summary>
    /// Simula o envio de uma imagem para o provedor de nuvem e gera uma URL pública.
    /// </summary>
    /// <param name="arquivo">Arquivo recebido da requisição HTTP.</param>
    /// <returns>URL pública fictícia gerada para a imagem.</returns>
    public async Task<string> UploadImageAsync(IFormFile arquivo)
    {
        // 1. Validação básica se o arquivo foi enviado
        if (arquivo == null || arquivo.Length == 0)
        {
            throw new ArgumentException("Nenhum arquivo de imagem foi fornecido para upload.");
        }

        // 2. Validação da extensão da imagem para segurança da aplicação
        string extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        string[] extensoesPermitidas = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

        bool extensaoValida = false;
        foreach (var ext in extensoesPermitidas)
        {
            if (ext == extensao)
            {
                extensaoValida = true;
                break;
            }
        }

        if (!extensaoValida)
        {
            throw new InvalidOperationException("Formato de imagem inválido. Apenas JPG, JPEG, PNG, WEBP e GIF são permitidos.");
        }

        // 3. Geramos um nome único para o arquivo usando GUID para evitar conflitos de nomes iguais
        string nomeUnicoArquivo = $"{Guid.NewGuid()}{extensao}";

        // 4. SIMULAÇÃO DO UPLOAD NA NUVEM (Ex: Azure Blob Storage ou Google Cloud Storage)
        // Quando integrar com o SDK real do Azure Blob Storage:
        // var containerClient = new BlobContainerClient(connectionString, "container-eventos");
        // var blobClient = containerClient.GetBlobClient(nomeUnicoArquivo);
        // await blobClient.UploadAsync(arquivo.OpenReadStream());
        // return blobClient.Uri.ToString();

        await Task.Delay(100); // Simulação do tempo de resposta da rede

        // Retorna a URL simulated da nuvem que será gravada na propriedade ImagemUrl do Evento
        string urlNuvemSimulada = $"https://storage.seuprovedor.com/eventos/{nomeUnicoArquivo}";
        return urlNuvemSimulada;
    }

    /// <summary>
    /// Simula a remoção da imagem no provedor de nuvem.
    /// </summary>
    /// <param name="imageUrl">URL da imagem armazenada.</param>
    /// <returns>Verdadeiro se a remoção simulada foi concluída.</returns>
    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return false;
        }

        // SIMULAÇÃO DA EXCLUSÃO NA NUVEM:
        // No Azure Blob Storage real:
        // var blobClient = new BlobClient(new Uri(imageUrl));
        // return await blobClient.DeleteIfExistsAsync();

        await Task.Delay(50);
        return true;
    }
}
