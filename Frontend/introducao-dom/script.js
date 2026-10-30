const titulo = document.getElementById("titulo");
console.log(titulo.textContent);
titulo.textContent = "JavaScript alterou este título"

const descricao = document.getElementById("descricao")
console.log(descricao.textContent)
descricao.textContent = "O conteúdo foi alterado pelo DOM."

const titulo1 = document.querySelector("#titulo")
console.log(titulo1)

const mensagem = document.querySelector(".mensagem")
console.log(mensagem)

const botao = document.querySelector("button")

const mensagens = document.querySelectorAll(".mensagem")

for (let cont = 0; cont < mensagens.length; cont++) {
    console.log(mensagens[cont].textContent)
}

// titulo.style.color = "blue";
// titulo.style.backgroundColor = "lightgray";
// titulo.style.padding = "20px"

// descricao.style.fontSize = "20px"

titulo.classList.add("destaque")

//titulo.classList.remove("destaque")

titulo.classList.toggle("destaque")

console.log(titulo.classList.contains("destaque"))

function mostrarMensagem() {
    titulo.textContent = "Botão cliclado!"
}

function alternarDestaque() {
    titulo.classList.toggle("destaque");
}

function mostrarNome() {
    const inputNome = document.querySelector("#nome")
    const resultado = document.querySelector("#resultado")

    if (inputNome.value === "") {
        resultado.textContent = "Digite um nome."
    } else {
        resultado.textContent = `Olá, ${inputNome.value}!`
    }
}

function trocarImagem() {
    const imagem = document.querySelector("#imagem")

    imagem.src = "https://super.abril.com.br/wp-content/uploads/2018/07/566ee0ae82bee174ca0300dahomer-simpson.jpeg?crop=1&resize=1212,909"
    imagem.alt = "homer simpson"
}

const link = document.querySelector("#link")

link.href = "https://developer.mozilla.org";
link.textContent = "Abrir documentação"

function adicionarParagrafo(){
    const novoParagrafo = document.createElement("p");
    novoParagrafo.textContent = "Este elemento foi criado pelo JavaScript.";

    const conteudo = document.querySelector("#conteudo")

    conteudo.appendChild(novoParagrafo)
}

const frutas = [
    "Maçã",
    "Banana",
    "Laranja",
    "Uva"
]

const lista = document.querySelector("#lista")

for(let cont = 0; cont < frutas.length; cont++){
    const fruta = document.createElement("li");

    fruta.textContent = frutas[cont]

    lista.appendChild(fruta)
}

function removerAviso(){
    const aviso = document.querySelector("#aviso")

    aviso.remove()
}