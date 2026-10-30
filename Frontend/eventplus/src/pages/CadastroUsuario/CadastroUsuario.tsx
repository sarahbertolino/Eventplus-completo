import { useState, type FormEvent } from "react";
import { Link, useNavigate } from "react-router-dom";
import Header from "../../components/header/Header";
import Footer from "../../components/footer/Footer";
import Campo from "../../components/campo/Campo";
import Botao from "../../components/botao/Botao";
import { usuarioService } from "../../services/eventPlusservice";
import { getApiErrorMessage } from "../../services/api";
import "./CadastroUsuario.css";

export default function CadastroUsuario() {
  const navigate = useNavigate();
  const [nome, setNome] = useState("");
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [confirmarSenha, setConfirmarSenha] = useState("");
  const [mensagem, setMensagem] = useState("");
  const [salvando, setSalvando] = useState(false);

  async function cadastrar(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setMensagem("");
    if (senha !== confirmarSenha) {
      setMensagem("As senhas não coincidem.");
      return;
    }
    setSalvando(true);
    try {
      await usuarioService.cadastrar({ nome: nome.trim(), email: email.trim(), senha, idTipoUsuario: null });
      navigate("/login", { replace: true, state: { cadastroConcluido: true } });
    } catch (error) {
      setMensagem(getApiErrorMessage(error));
    } finally {
      setSalvando(false);
    }
  }

  return <><Header /><main className="cadastro-publico-page">
    <section className="cadastro-publico-card">
      <h1>Criar conta</h1>
      <p>Cadastre-se para participar dos eventos e comentar.</p>
      <form onSubmit={cadastrar}>
        <label className="visually-hidden" htmlFor="cadastro-nome">Nome</label>
        <Campo id="cadastro-nome" value={nome} onChange={event => setNome(event.target.value)} placeholder="Nome" autoComplete="name" maxLength={100} required />
        <label className="visually-hidden" htmlFor="cadastro-email">E-mail</label>
        <Campo id="cadastro-email" type="email" value={email} onChange={event => setEmail(event.target.value)} placeholder="E-mail" autoComplete="email" required />
        <label className="visually-hidden" htmlFor="cadastro-senha">Senha</label>
        <Campo id="cadastro-senha" type="password" value={senha} onChange={event => setSenha(event.target.value)} placeholder="Senha (mínimo 8 caracteres)" autoComplete="new-password" minLength={8} maxLength={60} required />
        <label className="visually-hidden" htmlFor="cadastro-confirmar-senha">Confirmar senha</label>
        <Campo id="cadastro-confirmar-senha" type="password" value={confirmarSenha} onChange={event => setConfirmarSenha(event.target.value)} placeholder="Confirmar senha" autoComplete="new-password" minLength={8} maxLength={60} required />
        <Botao type="submit" texto={salvando ? "Cadastrando..." : "Cadastrar"} disabled={salvando} />
        {mensagem && <p className="cadastro-publico-mensagem" role="alert">{mensagem}</p>}
      </form>
      <p className="cadastro-publico-login">Já tem uma conta? <Link to="/login">Entrar</Link></p>
    </section>
  </main><Footer /></>;
}
