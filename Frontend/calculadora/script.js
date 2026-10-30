function somar() {
    const inputNumero1 = document.querySelector("#numero1");
    const inputNumero2 = document.querySelector("#numero2");
    const resultado = document.querySelector("#resultado");

    if (inputNumero1.value === "" || inputNumero2.value === "") {
        resultado.textContent = "Preencha os dois números.";
        return;
    }

    const numero1 = Number(inputNumero1.value);
    const numero2 = Number(inputNumero2.value);

    const soma = numero1 + numero2;

    resultado.textContent = `Resultado: ${soma}`;
}

function subtrair() {
    const inputNumero1 = document.querySelector("#numero1");
    const inputNumero2 = document.querySelector("#numero2");
    const resultado = document.querySelector("#resultado");

    if (inputNumero1.value === "" || inputNumero2.value === "") {
        resultado.textContent = "Preencha os dois números.";
        return;
    }

    const numero1 = Number(inputNumero1.value);
    const numero2 = Number(inputNumero2.value);

    const subtracao = numero1 - numero2;

    resultado.textContent = `Resultado: ${subtracao}`;
}

function multiplicar() {
    const inputNumero1 = document.querySelector("#numero1");
    const inputNumero2 = document.querySelector("#numero2");
    const resultado = document.querySelector("#resultado");

    if (inputNumero1.value === "" || inputNumero2.value === "") {
        resultado.textContent = "Preencha os dois números.";
        return;
    }

    const numero1 = Number(inputNumero1.value);
    const numero2 = Number(inputNumero2.value);

    const multiplicacao = numero1 * numero2;

    resultado.textContent = `Resultado: ${multiplicacao}`;
}

function dividir() {
    const inputNumero1 = document.querySelector("#numero1");
    const inputNumero2 = document.querySelector("#numero2");
    const resultado = document.querySelector("#resultado");

    if (inputNumero1.value === "" || inputNumero2.value === "") {
        resultado.textContent = "Preencha os dois números.";
        return;
    }

    const numero1 = Number(inputNumero1.value);
    const numero2 = Number(inputNumero2.value);

    if (numero2 === 0) {
        resultado.textContent = "Não é possível dividir por zero.";
        return;
    }

    const divisao = numero1 / numero2;

    resultado.textContent = `Resultado: ${divisao}`;
}

function limpar() {
    const inputNumero1 = document.querySelector("#numero1");
    const inputNumero2 = document.querySelector("#numero2");
    const resultado = document.querySelector("#resultado");

    inputNumero1.value = "";
    inputNumero2.value = "";
    resultado.textContent = "Resultado: ";
}