import { useEffect, useMemo, useState } from "react";
import { eventoService, tipoEventoService } from "../../services/eventPlusservice";
import { getApiErrorMessage } from "../../services/api";
import type { Evento, TipoEvento } from "../../types/api";
import Header from "../../components/header/Header";
import Footer from "../../components/footer/Footer";
import CardEvento from "../../components/cardEvento/CardEvento";
import "./Eventos.css";

export default function Eventos() {
  const [eventos, setEventos] = useState<Evento[]>([]);
  const [tipos, setTipos] = useState<TipoEvento[]>([]);
  const [filtro, setFiltro] = useState("Todos");
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");
  useEffect(() => {
    let ativo = true;
    Promise.allSettled([eventoService.listar(), tipoEventoService.listar()]).then(([eventResult, tipoResult]) => {
      if (!ativo) return;
      if (eventResult.status === "fulfilled") setEventos(eventResult.value);
      else setErro(getApiErrorMessage(eventResult.reason));
      if (tipoResult.status === "fulfilled") setTipos(tipoResult.value);
    }).finally(() => { if (ativo) setCarregando(false); });
    return () => { ativo = false; };
  }, []);
  const filtrados = useMemo(() => filtro === "Todos" ? eventos : eventos.filter(evento => evento.idTipoEventoNavigation?.titulo === filtro), [eventos, filtro]);
  return <><Header /><main className="eventos-page">
    <div className="eventos-container">
      <nav className="eventos-filtros" aria-label="Filtrar eventos">
        {["Todos", ...tipos.map(tipo => tipo.titulo)].map(tipo => <button type="button" key={tipo} className={filtro === tipo ? "ativo" : ""} aria-pressed={filtro === tipo} onClick={() => setFiltro(tipo)}>{tipo}</button>)}
      </nav>
      {carregando && <p>Carregando eventos...</p>}{erro && <p role="alert">{erro}</p>}
      {!carregando && !erro && filtrados.length === 0 && <p>Nenhum evento encontrado.</p>}
      <div className="eventos-grid">{filtrados.map(evento => <CardEvento key={evento.idEvento} id={evento.idEvento} imagem={evento.imagemUrl} categoria={evento.idTipoEventoNavigation?.titulo} titulo={evento.nome} descricao={evento.descricao} />)}</div>
    </div>
  </main><Footer /></>;
}
