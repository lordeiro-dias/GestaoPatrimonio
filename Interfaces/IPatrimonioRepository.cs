using GerenciamentoPatrimonio.Domains;

namespace GerenciamentoPatrimonio.Interfaces
{
    public interface IPatrimonioRepository
    {
        List<Patrimonio> Listar();
        Patrimonio BuscarPorId(Guid patrimonioId);

        bool BuscarPorNumeroPatrimonio(string numeroPatrimonio);
        bool LocalizacaoExiste(Guid localizacaoId);
        bool StatusPatrimonioExiste(Guid statusPatrimonioId);

        void Adicionar(Patrimonio patrimonio);
        void AtualizarStatus(Patrimonio patrimonio);
        void AdicionarLog(LogPatrimonio logPatrimonio);

        Local BuscarLocalPorNome(string nomeLocal);
        StatusPatrimonio BuscarStatusPatrimonioPorNome(string nomeStatus);
        TipoAlteracao BuscarTipoAlteracaoPorNome(string nomeTipo);
    }
}
