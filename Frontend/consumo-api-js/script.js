async function carregarUsuarios() {
    const mensagem = document.querySelector("#mensagem")

    mensagem.textContent = "Iniciando a busca..."

    const resposta = await fetch("https://jsonplaceholder.typicode.com/users");

    const usuarios = await resposta.json();

    const lista = document.querySelector("#listaUsuarios")

    lista.textContent = "";

    for (let cont = 0; cont < usuarios.length; cont++) {

        const usuario = usuarios[cont]

        const cartao = document.createElement("article")
        const titulo = document.createElement("h2")

        titulo.textContent = usuario.name;

        const usuarioTexto = document.createElement("p")
        usuarioTexto.textContent = `Usuário: ${usuario.username}`

        const email = document.createElement("p")
        email.textContent = `Email: ${usuario.email}`

        const cidade = document.createElement("p")
        cidade.textContent = `Cidade: ${usuario.address.city}`

        const empresa = document.createElement("p")
        empresa.textContent = `Empresa: ${usuario.company.name}`

        cartao.appendChild(titulo);
        cartao.appendChild(usuarioTexto)
        cartao.appendChild(email)
        cartao.appendChild(cidade)
        cartao.appendChild(empresa)

        lista.appendChild(cartao)
    }

    mensagem.textContent = "Usuários Buscados!"
}