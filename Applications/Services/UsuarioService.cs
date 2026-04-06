using GerenciamentoPatrimonio.Applications.Autenticacao;
using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.UsuarioDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();

            List<ListarUsuarioDto> usuariosDto = usuarios.Select(u => new ListarUsuarioDto
            {
                UsuarioID = u.UsuarioID,
                NIF = u.NIF,
                Nome = u.Nome,
                RG = u.RG,
                CPF = u.CPF,
                CarteiraTrabalho = u.CarteiraTrabalho,
                Email = u.Email,
                Ativo = u.Ativo,
                PrimeiroAcesso = u.PrimeiroAcesso,
                EnderecoID = u.EnderecoID,
                CargoID = u.CargoID,
                TipoUsuarioID = u.TipoUsuarioID
            }).ToList();

            return usuariosDto;
        }
        public ListarUsuarioDto BuscarPorId(Guid id)
        {
            Usuario usuario = _repository.BuscarPorId(id);

            if(usuario == null)
            {
                throw new DomainException("Usuário não encontrado");
            }

            return new ListarUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                NIF = usuario.NIF,
                Nome = usuario.Nome,
                RG = usuario.RG,
                CPF = usuario.CPF,
                CarteiraTrabalho = usuario.CarteiraTrabalho,
                Email = usuario.Email,
                Ativo = usuario.Ativo,
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                EnderecoID = usuario.EnderecoID,
                CargoID = usuario.CargoID,
                TipoUsuarioID = usuario.TipoUsuarioID
            };
        }

        public void Adicionar(CriarUsuarioDto dto)
        {
            Validar.ValidarNome(dto.Nome);
            Validar.ValidarNIF(dto.NIF);
            Validar.ValidarCPF(dto.CPF);
            Validar.ValidarEmail(dto.Email);

            Usuario usuarioDuplicado = _repository.BuscarDuplicado(dto.NIF, dto.CPF, dto.Email);

            if(usuarioDuplicado != null)
            {
                if(usuarioDuplicado.NIF == dto.NIF)
                {
                    throw new DomainException("Já existe um usuário cadastrado com esse NIF.");
                }

                if(usuarioDuplicado.CPF == dto.CPF)
                {
                    throw new DomainException("Já eixste um usuário cadastrado com esse CPF.");
                }

                if(usuarioDuplicado.Email.ToLower() == dto.Email.ToLower())
                {
                    throw new DomainException("Já existe um usuário cadastrado com esse E-mail.");
                }
            }

            if(!_repository.EnderecoExiste(dto.EnderecoID))
            {
                throw new DomainException("Endereço informado não existe.");
            }

            if(!_repository.CargoExiste(dto.CargoID))
            {
                throw new DomainException("Cargo informado não existe.");
            }

            if(!_repository.TipoUsuarioExiste(dto.TipoUsuarioID))
            {
                throw new DomainException("Tipo de usuário informado não existe.");
            }

            Usuario usuario = new Usuario
            {
                NIF = dto.NIF,
                Nome = dto.Nome,
                CPF = dto.CPF,
                Email = dto.Email,
                RG = dto.RG,
                CarteiraTrabalho = dto.CarteiraTrabalho,
                Senha = CriptografiaUsuario.CriptografarSenha(dto.NIF),
                Ativo = true,
                PrimeiroAcesso = true,
                EnderecoID = dto.EnderecoID,
                CargoID = dto.CargoID,
                TipoUsuarioID = dto.TipoUsuarioID
            };

            _repository.Adicionar(usuario);
        }

        public void Atualizar(Guid id, CriarUsuarioDto dto)
        {
            Validar.ValidarNome(dto.Nome);
            Validar.ValidarNIF(dto.NIF);
            Validar.ValidarCPF(dto.CPF);
            Validar.ValidarEmail(dto.Email);

            Usuario usuarioBanco = _repository.BuscarPorId(id);

            if(usuarioBanco == null )
            {
                throw new DomainException("Usuário não encontrado.");
            }

            Usuario usuarioDuplicado = _repository.BuscarDuplicado(dto.NIF, dto.CPF, dto.Email, id);

            if(usuarioDuplicado != null)
            {
                if (usuarioDuplicado.NIF == dto.NIF)
                {
                    throw new DomainException("Já existe um usuário cadastrado com esse NIF.");
                }

                if (usuarioDuplicado.CPF == dto.CPF)
                {
                    throw new DomainException("Já eixste um usuário cadastrado com esse CPF.");
                }

                if (usuarioDuplicado.Email.ToLower() == dto.Email.ToLower())
                {
                    throw new DomainException("Já existe um usuário cadastrado com esse E-mail.");
                }
            }

            if (!_repository.EnderecoExiste(dto.EnderecoID))
            {
                throw new DomainException("Endereço informado não existe.");
            }

            if (!_repository.CargoExiste(dto.CargoID))
            {
                throw new DomainException("Cargo informado não existe.");
            }

            if (!_repository.TipoUsuarioExiste(dto.TipoUsuarioID))
            {
                throw new DomainException("Tipo de usuário informado não existe.");
            }

            usuarioBanco.NIF = dto.NIF;
            usuarioBanco.Nome = dto.Nome;
            usuarioBanco.RG = dto.RG;
            usuarioBanco.CPF = dto.CPF;
            usuarioBanco.CarteiraTrabalho = dto.CarteiraTrabalho;
            usuarioBanco.Email = dto.Email;
            usuarioBanco.EnderecoID = dto.EnderecoID;
            usuarioBanco.CargoID = dto.CargoID;
            usuarioBanco.TipoUsuarioID = dto.TipoUsuarioID;

            _repository.Atualizar(usuarioBanco);
        }

        public void AtualizarStatus(Guid id, AtualizarStatusUsuarioDto dto)
        {
            Usuario usuarioBanco = _repository.BuscarPorId(id);

            if(usuarioBanco == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            usuarioBanco.Ativo = dto.Ativo;
            _repository.AtualizarStatus(usuarioBanco);
        }
    }
}
