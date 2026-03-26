using GerenciamentoPatrimonio.Domains;

namespace GerenciamentoPatrimonio.Interfaces
{
    public interface ILocalRepository
    {
        List<Local> Listar();
        Local BuscarPorId(Guid localID);
        void Adicionar(Local local);
        bool AreaExiste(Guid areaId);
        void Atualizar(Local local);
    }
}
