using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pela autenticação de usuários via JWT (JSON Web Token).
/// 
/// COMO FUNCIONA O JWT?
/// 1. O usuário envia e-mail e senha via POST /api/Login.
/// 2. A API valida as credenciais no banco (e-mail e hash BCrypt).
/// 3. Se válido, a API gera um Token JWT assinado com uma chave secreta.
/// 4. O cliente usa esse token no cabeçalho "Authorization: Bearer {token}" 
///    em todas as requisições seguintes que exigem autenticação ([Authorize]).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Autentica um usuário e retorna o Token JWT de acesso.
    /// POST: /api/Login
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        // 1. Busca o usuário pelo e-mail e valida a senha com BCrypt
        var usuarioEncontrado = await _usuarioRepository.BuscarPorEmailESenhaAsync(dto.Email, dto.Senha);

        // 2. Se as credenciais forem inválidas, retorna 401 Unauthorized
        if (usuarioEncontrado == null)
        {
            return Unauthorized("E-mail ou senha inválidos.");
        }

        // 3. Criamos a lista de Claims (informações que ficam dentro do token)
        //    Claims são como "afirmações" sobre o usuário que ficam codificadas no token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioEncontrado.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuarioEncontrado.Email),
            new Claim("nome", usuarioEncontrado.Nome),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 4. Criamos a chave de segurança com base na chave secreta definida no appsettings.json
        var chaveSecreta = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("eventos-chave-autenticacao-webapi-dev")
        );

        // 5. Definimos o algoritmo de assinatura (HmacSha256 é padrão para JWT)
        var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

        // 6. Montamos o token JWT com todas as informações
        var token = new JwtSecurityToken(
            issuer: "webapi.event+",
            audience: "webapi.event+",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), // Token válido por 8 horas
            signingCredentials: credenciais
        );

        // 7. Convertemos o token para string e retornamos ao cliente
        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            Token = tokenString,
            Expiracao = token.ValidTo,
            Usuario = new
            {
                usuarioEncontrado.IdUsuario,
                usuarioEncontrado.Nome,
                usuarioEncontrado.Email
            }
        });
    }
}
