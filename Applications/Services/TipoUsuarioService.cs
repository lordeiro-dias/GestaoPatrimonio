using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.TipoUsuarioDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class TipoUsuarioService
    {
        private readonly ITipoUsuarioRepository _repository;

        public TipoUsuarioService(ITipoUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarTipoUsuarioDto> Listar()
        {
            List<TipoUsuario> tipos = _repository.Listar();

            List<ListarTipoUsuarioDto> tiposDto = tipos.Select(t => new ListarTipoUsuarioDto
            {
                TipoUsuarioId = t.TipoUsuarioID,
                NomeTipo = t.NomeTipo
            }).ToList();

            return tiposDto;
        }

        public ListarTipoUsuarioDto BuscarPorId(Guid id)
        {
            TipoUsuario tipo = _repository.BuscarPorId(id);

            if (tipo == null)
            {
                throw new DomainException("Tipo de Usuário não encontrado.");
            }

            return new ListarTipoUsuarioDto
            {
                TipoUsuarioId = tipo.TipoUsuarioID,
                NomeTipo = tipo.NomeTipo
            };
        }

        public void Adicionar(CriarTipoUsuarioDto tipoDto)
        {
            Validar.ValidarNome(tipoDto.NomeTipo);

            TipoUsuario tipoExistente = _repository.BuscarPorNome(tipoDto.NomeTipo);

            if(tipoExistente != null)
            {
                throw new DomainException("Já existe um tipo de usuário com este nome.");
            }

            TipoUsuario tipo = new TipoUsuario
            {
                NomeTipo = tipoDto.NomeTipo
            };

            _repository.Adicionar(tipo);
        }

        public void Atualizar(Guid id, CriarTipoUsuarioDto tipoDto)
        {
            Validar.ValidarNome(tipoDto.NomeTipo);

            TipoUsuario tipoExistente = _repository.BuscarPorNome(tipoDto.NomeTipo);

            TipoUsuario tipoBanco = _repository.BuscarPorId(id);

            if(tipoBanco == null)
            {
                throw new DomainException("Tipo de usuário não encontrado.");
            }

            if (tipoExistente != null)
            {
                throw new DomainException("Já existe um tipo de usuário com este nome.");
            }

            tipoBanco.NomeTipo = tipoDto.NomeTipo;

            _repository.Atualizar(tipoBanco);
        }
    }
}
