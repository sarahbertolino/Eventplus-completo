import type { PropsWithChildren } from "react";
import { perfilEhAdministrador, perfilEhOrganizador, useAuth } from "../context/auth";
import { Navigate, useLocation } from "react-router-dom";

interface ProtectedRouteProps extends PropsWithChildren{
    role?: "administrador" | "organizador";
}

function ProtectedRoute({children, role}: ProtectedRouteProps){
    const { autenticado, carregandoSessao, perfil} = useAuth();
    const location = useLocation();

    if(carregandoSessao) {
        return <main>Carregando sessão...</main>;
    }

    if(!autenticado){
        return <Navigate to="/login" state={{from: location.pathname}} replace />
    }

    const autorizado = !role || (role === "administrador" && perfilEhAdministrador(perfil)) || (role === "organizador" && perfilEhOrganizador(perfil));

    if(!autorizado) return <Navigate to="/home" replace />

    return children;
}

export default ProtectedRoute;
