using GerenciamentoPatrimonio.Domains;

namespace GerenciamentoPatrimonio.Interfaces
{
    public interface ISolicitacaoTransferenciaRepository
    {
        List<SolicitacaoTransferencia> Listar();
        SolicitacaoTransferencia BuscarPorId(Guid id);
        bool ExisteSolicitacaoPendende(Guid patrimonioId);
        bool UsuarioResponavelDaLocalizacao(Guid usuarioId, Guid LocalId);
        StatusTransferencia BuscarStatusTransferenciaPorNome(string nomeStatus);
        void Adicionar(SolicitacaoTransferencia solicitacaoTransferencia);
        void LocalExiste(Guid localId);
        Patrimonio BuscarPatrimonioPorId(Guid patrimonioId);
    }
}
