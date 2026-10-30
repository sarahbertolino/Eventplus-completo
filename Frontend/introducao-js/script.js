const nome = "Carlos";
const idade = 20;
const estudante = true;

// console.log(nome)
// console.log(idade)
// console.log(estudante)

// console.log(`Meu nome é ${nome} e tenho ${idade} anos.`)

// if(idade >= 18){
//     console.log("Maior de idade")
// }
// else{
//     console.log("Menor Idade")
// }

const nomes = [
    "Fernanda",
    "Bruno",
    "Leonardo",
    "Eric",
    "Samuel"
]

for (let cont = 0; cont < nomes.length; cont++){
    console.log(nomes[cont]);
}

function calcularMedia(nota1, nota2){
    return (nota1 + nota2) / 2
}

const media = calcularMedia(6, 7);

console.log(media)

if (media >= 7) {
    console.log("Aprovado")
} else if(media >= 5) {
    console.log("Recuperação")
} else {
    console.log("Reprovado")
}
