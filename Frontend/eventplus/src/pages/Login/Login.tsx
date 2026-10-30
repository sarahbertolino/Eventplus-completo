import { Link, useLocation, useNavigate } from "react-router-dom";
import logoEvent from "../../assets/logo-event.svg"

import "./Login.css"
import { useAuth } from "../../context/auth";
import { useEffect, useState, type FormEvent } from "react";
import Botao from "../../components/botao/Botao";
import Campo from "../../components/campo/Campo";
import { getApiErrorMessage } from "../../services/api";

function Login() {

    const navigate = useNavigate();
    const location = useLocation();
    const cadastroConcluido = Boolean((location.state as { cadastroConcluido?: boolean } | null)?.cadastroConcluido);
    const { login, autenticado } = useAuth();
    const [email, setEmail] = useState("");
    const [senha, setSenha] = useState("");
    const [erro, setErro] = useState("");
    const [enviando, setEnviando] = useState(false);

    useEffect(() => {
        if (autenticado) navigate("/home", { replace: true });
    }, [autenticado, navigate])

    async function entrar(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        setErro("");
        setEnviando(true);

        try {
            await login({ email: email.trim(), senha });
            const state = location.state as { from?: string } | null
            navigate(state?.from || "/home", { replace: true })
        } catch (error) {
            setErro(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    return (
        <main className="login-page">
          <div className="login-container">
            <img className="login-logo" src={logoEvent} alt="EventPlus logo" />
            <form className="login-form" onSubmit={entrar} noValidate>

                <div>
                    <label className="visually-hidden" htmlFor="login-email">E-mail</label>
                    <Campo
                        id="login-email"
                        type="email"
                        value={email}
                        onChange={(event) => setEmail(event.target.value)}
                        placeholder="Login"
                        autoComplete="email"
                        required
                    />
                </div>
                <div>
                    <label className="visually-hidden" htmlFor="login-senha">Senha</label>
                    <Campo
                        id='login-senha'
                        type="password"
                        value={senha}
                        onChange={(event) => setSenha(event.target.value)}
                        placeholder="Senha"
                        autoComplete="current-password"
                        minLength={8}
                        required
                    />
                </div>
                <Botao
                    texto={enviando ? "Entrando..." : "Entrar"}
                    type="submit"
                    disabled={enviando}
                />
                <p className="forgot-password">Esqueceu a senha?</p>
                {cadastroConcluido ? <p className="login-cadastro-ok" role="status">Conta criada. Entre com seu e-mail e senha.</p> : null}
                {erro ? <p className="login-erro" role="alert">{erro}</p> : null}

            </form>
            <p className="login-cadastro-link">Não tem uma conta? <Link to="/cadastro">Cadastre-se</Link></p>
          </div>
        </main>
    )
}

export default Login;
