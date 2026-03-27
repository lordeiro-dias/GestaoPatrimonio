using GerenciamentoPatrimonio.Domains;

namespace GerenciamentoPatrimonio.Interfaces
{
    public interface ILocalRepository
    {
        List<Local> Listar();
        Local BuscarPorId(Guid localID);
        Local BuscarPorNome(string nomeLocal, Guid areaId);
        void Adicionar(Local local);
        bool AreaExiste(Guid areaId);
        void Atualizar(Local local);
    }
}
