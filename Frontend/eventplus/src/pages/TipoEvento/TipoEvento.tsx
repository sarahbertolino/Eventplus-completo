import { useCallback, useEffect, useState, type FormEvent } from "react";
import Header from "../../components/header/Header";
import Footer from "../../components/footer/Footer";
import Campo from "../../components/campo/Campo";
import Botao from "../../components/botao/Botao";
import ModalConfirmacao from "../../components/modalConfirmacao/ModalConfirmacao";
import { tipoEventoService } from "../../services/eventPlusservice";
import { getApiErrorMessage } from "../../services/api";
import type { TipoEvento } from "../../types/api";
import editarIcon from "../../assets/icons/lapis.svg";
import excluirIcon from "../../assets/icons/lixeira.svg";
import visao from "../../assets/visao-img.png";
import "./TipoEvento.css";

export default function TipoEventoPage() {
  const [tipos, setTipos] = useState<TipoEvento[]>([]);
  const [titulo, setTitulo] = useState("");
  const [emEdicao, setEmEdicao] = useState<string | null>(null);
  const [selecionado, setSelecionado] = useState<TipoEvento | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [enviando, setEnviando] = useState(false);
  const [mensagem, setMensagem] = useState("");

  const carregar = useCallback(async () => {
    setCarregando(true);
    try { setTipos(await tipoEventoService.listar()); }
    catch (error) { setMensagem(getApiErrorMessage(error)); }
    finally { setCarregando(false); }
  }, []);
  useEffect(() => { void carregar(); }, [carregar]);

  async function salvar(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setEnviando(true); setMensagem("");
    try {
      if (emEdicao) await tipoEventoService.atualizar(emEdicao, titulo.trim());
      else await tipoEventoService.cadastrar(titulo.trim());
      setTitulo(""); setEmEdicao(null);
      await carregar(); setMensagem("Tipo de evento salvo com sucesso.");
    } catch (error) { setMensagem(getApiErrorMessage(error)); }
    finally { setEnviando(false); }
  }
  async function excluir() {
    if (!selecionado) return;
    setEnviando(true);
    try { await tipoEventoService.excluir(selecionado.idTipoEvento); if (emEdicao === selecionado.idTipoEvento) { setEmEdicao(null); setTitulo(""); } setSelecionado(null); await carregar(); setMensagem("Tipo de evento excluído."); }
    catch (error) { setMensagem(getApiErrorMessage(error)); setSelecionado(null); }
    finally { setEnviando(false); }
  }

  return <><Header /><main className="tipo-evento-page">
    <section className="tipo-evento-cadastro">
      <h1 className="tipo-evento-titulo">Administração tipo de eventos</h1>
      <div className="tipo-evento-form-area"><img src={visao} alt="Equipe organizando um evento" />
        <form onSubmit={salvar}><label className="visually-hidden" htmlFor="tipo-evento-titulo">Título</label>
          <Campo id="tipo-evento-titulo" value={titulo} onChange={event => setTitulo(event.target.value)} placeholder="Título" maxLength={80} required />
          <Botao type="submit" texto={enviando ? "Salvando..." : emEdicao ? "Salvar alterações" : "Cadastrar"} disabled={enviando} />
          {emEdicao && <button type="button" className="tipo-evento-cancelar" onClick={() => { setEmEdicao(null); setTitulo(""); }}>Cancelar edição</button>}
          {mensagem && <p role="status">{mensagem}</p>}
        </form>
      </div>
    </section>
    <section className="tipo-evento-lista"><div className="tipo-evento-lista-inner"><h2>Lista tipo de eventos</h2>
      <div className="tipo-evento-tabela-wrap"><table><thead><tr><th>Título</th><th>Editar</th><th>Deletar</th></tr></thead><tbody>
        {carregando && <tr><td colSpan={3}>Carregando...</td></tr>}
        {!carregando && tipos.length === 0 && <tr><td colSpan={3}>Nenhum tipo cadastrado.</td></tr>}
        {!carregando && tipos.map(tipo => <tr key={tipo.idTipoEvento}><td>{tipo.titulo}</td><td><button type="button" aria-label={`Editar ${tipo.titulo}`} onClick={() => { setEmEdicao(tipo.idTipoEvento); setTitulo(tipo.titulo); window.scrollTo({ top: 0, behavior: "smooth" }); }}><img src={editarIcon} alt="" /></button></td><td><button type="button" aria-label={`Excluir ${tipo.titulo}`} onClick={() => setSelecionado(tipo)}><img src={excluirIcon} alt="" /></button></td></tr>)}
      </tbody></table></div></div></section>
    <ModalConfirmacao show={Boolean(selecionado)} titulo="Excluir tipo de evento" mensagem={selecionado ? `Deseja excluir o tipo “${selecionado.titulo}”?` : ""} textoConfirmar={enviando ? "Excluindo..." : "Excluir"} textoCancelar="Cancelar" onClose={() => setSelecionado(null)} onConfirm={() => void excluir()} />
  </main><Footer /></>;
}
