using GerenciamentoPatrimonio.Domains;

namespace GerenciamentoPatrimonio.Interfaces
{
    public interface ISolicitacaoTransferenciaRepository
    {
        List<SolicitacaoTransferencia> Listar();
        SolicitacaoTransferencia BuscarPorId(Guid id);
        bool ExisteSolicitacaoPendente(Guid patrimonioId);
        bool UsuarioResponsavelDaLocalizacao(Guid usuarioId, Guid LocalId);
        StatusTransferencia BuscarStatusTransferenciaPorNome(string nomeStatus);
        void Adicionar(SolicitacaoTransferencia solicitacaoTransferencia);
        bool LocalExiste(Guid localId);
        Patrimonio BuscarPatrimonioPorId(Guid patrimonioId);
        StatusPatrimonio BuscarStatusPatrimonioPorNome(string nomeStatus);
        TipoAlteracao BuscarTipoAlteracaoPorNome(string nomeTipo);
        void Atualizar(SolicitacaoTransferencia solicitacaoTransferencia);
        void AtualizarPatrimonio(Patrimonio patrimonio);
        void AdicionarLog(LogPatrimonio logPatrimonio);
    }
}
