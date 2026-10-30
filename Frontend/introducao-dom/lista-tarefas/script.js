function adicionarTarefa() {
    const input = document.querySelector("#novaTarefa")
    const mensagem = document.querySelector("#mensagem")
    const listaTarefas = document.querySelector("#listaTarefas")

    if (input.value === "") {
        mensagem.textContent = "Digite uma tarefa.";
        return;
    } else {
        mensagem.textContent = "";
    }

    const tarefa = document.createElement("li");
    
    //Crio o botão para remover a tarefa
    const botaoRemover = document.createElement("button");

    //Adiciono o texto no botão
    botaoRemover.textContent = "Remover";
    //Adiciono a função de remover a tarefa dentro do botão
    botaoRemover.onclick = function () {
        tarefa.remove();
    };
    
    tarefa.textContent = input.value;
    //adiona o botão de remover dentro da tarefa
    tarefa.appendChild(botaoRemover);
    listaTarefas.appendChild(tarefa)
    input.value = ""
}