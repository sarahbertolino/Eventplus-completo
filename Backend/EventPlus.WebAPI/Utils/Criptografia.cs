namespace EventPlus.WebAPI.Utils;

/// <summary>
/// Utilitário estático responsável pelas operações de Criptografia e Hashing de senhas na API.
/// 
/// BOAS PRÁTICAS DE SEGURANÇA (.NET):
/// Nunca salve senhas em texto puro no banco de dados! 
/// Utilizamos o algoritmo BCrypt, que gera um hash seguro acompanhado de um "Salt" (sal) aleatório,
/// tornando impossível a recuperação da senha original mesmo se o banco for comprometido.
/// </summary>
public static class Criptografia
{
    /// <summary>
    /// Recebe a senha digitada pelo usuário e gera um Hash seguro criptografado com BCrypt.
    /// </summary>
    /// <param name="senha">Senha em texto puro (plain text).</param>
    /// <returns>Hash seguro de 60 caracteres contendo o salt e o hash gerado.</returns>
    public static string GerarHash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    /// <summary>
    /// Compara a senha em texto puro digitada no login com o Hash salvo no banco de dados.
    /// </summary>
    /// <param name="senhaInformada">Senha informada pelo usuário no formulário de login.</param>
    /// <param name="senhaBanco">Hash de senha salvo na tabela Usuario do banco de dados.</param>
    /// <returns>Retorna true se a senha for válida; caso contrário, false.</returns>
    public static bool CompararHash(string senhaInformada, string senhaBanco)
    {
        if (string.IsNullOrEmpty(senhaInformada) || string.IsNullOrEmpty(senhaBanco))
        {
            return false;
        }

        return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);
    }
}
