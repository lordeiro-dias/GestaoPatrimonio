using GerenciamentoPatrimonio.Domains;

namespace GerenciamentoPatrimonio.Interfaces
{
    public interface IBairroRepository
    {
        List<Bairro> Listar();
        Bairro ObterPorId(Guid BairroId);
        Bairro BuscarPorNome(string nomeBairro, Guid cidadeId);
        bool CidadeExiste(Guid cidadeId);
        void Adicionar(Bairro bairro);
        void Atualizar(Bairro bairro);
    }
}
