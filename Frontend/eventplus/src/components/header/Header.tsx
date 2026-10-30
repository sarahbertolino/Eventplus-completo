import { Link, NavLink, useNavigate } from "react-router-dom"

import logoEvent from "../../assets/logo-event.svg"
import "./Header.css"
import { perfilEhAdministrador, perfilEhOrganizador, useAuth } from "../../context/auth"

function Header() {
    const { autenticado, perfil, logout } = useAuth();

    const navigate = useNavigate();
    const administrador = perfilEhAdministrador(perfil);
    const organizador = perfilEhOrganizador(perfil);

    function sair() {
        logout();
        navigate("/Login");
    }

    return (
        <header className="header">
            <div className="header-conteudo">
                <Link to="/home">
                    <img src={logoEvent} alt="Logo Event+" />
                </Link>

                <nav>
                    <NavLink to="/home">Home</NavLink>
                    <NavLink to="/eventos">Eventos</NavLink>
                    {administrador ?
                        <NavLink to="/usuarios">Usuários</NavLink>
                        : null}
                    {!autenticado ? <NavLink to="/cadastro">Cadastre-se</NavLink> : null}
                    {organizador ?
                        <NavLink to="/cadastroEvento">Eventos</NavLink>
                        : null
                    }
                    {administrador ? <NavLink to="/tipoEvento">Tipos de eventos</NavLink> : null}
                    <Link to="/home#contato">Contatos</Link>
                </nav>

                {autenticado ?
                    <>
                        <span className="header-usuario">{perfil || "Usuário"}</span>
                        <button className="header-sair" onClick={sair} type="button">Sair</button>
                    </>
                    :
                    <Link to="/login">Entrar</Link>
                }
            </div>
        </header>
    )
}

export default Header;
