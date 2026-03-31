using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.EnderecoDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class EnderecoService
    {
        private readonly IEnderecoRepository _repository;

        public EnderecoService(IEnderecoRepository repository)
        {
            _repository = repository; 
        }

        public List<ListarEnderecoDto> Listar()
        {
            List<Endereco> enderecos = _repository.Listar();

            List<ListarEnderecoDto> enderecosDto = enderecos.Select(e => new ListarEnderecoDto
            {
                EnderecoId = e.EnderecoID,
                Logradouro = e.Logradouro,
                Complemento = e.Complemento,
                Numero = e.Numero,
                CEP = e.CEP,
                BairroId = e.BairroID,
            }).ToList();

            return enderecosDto;
        }

        public ListarEnderecoDto BuscarPorId(Guid enderecoId)
        {
            Endereco endereco = _repository.BuscarPorId(enderecoId);

            if(endereco == null)
            {
                throw new DomainException("Endereço não encontrado.");
            }

            return new ListarEnderecoDto
            {
                EnderecoId = endereco.EnderecoID,
                Logradouro = endereco.Logradouro,
                Complemento = endereco.Complemento,
                Numero = endereco.Numero,
                CEP = endereco.CEP,
                BairroId = endereco.BairroID
            };
        }

        public void Adicionar(CriarEnderecoDto enderecoDto)
        {
            Validar.ValidarNome(enderecoDto.Logradouro);

            Endereco enderecoExistente = _repository.BuscarPorLogradouroENumero(enderecoDto.Logradouro, enderecoDto.Numero, enderecoDto.BairroId);

            if(enderecoExistente != null)
            {
                throw new DomainException("Já existe uma Rua cadastrada neste Bairro e Número.");
            }

            if(!_repository.BairroExiste(enderecoDto.BairroId))
            {
                throw new DomainException("Bairro informado não existe.");
            }

            Endereco endereco = new Endereco
            {
                Logradouro = enderecoDto.Logradouro,
                Numero = enderecoDto.Numero,
                Complemento = enderecoDto.Complemento,
                CEP = enderecoDto.CEP,
                BairroID = enderecoDto.BairroId
            };

            _repository.Adicionar(endereco);
        }

        public void Atualizar(Guid enderecoId, CriarEnderecoDto enderecoDto)
        {
            Validar.ValidarNome(enderecoDto.Logradouro);

            Endereco enderecoExistente = _repository.BuscarPorLogradouroENumero(enderecoDto.Logradouro, enderecoDto.Numero, enderecoDto.BairroId);
            Endereco enderecoBanco = _repository.BuscarPorId(enderecoId);

            if (enderecoExistente != null)
            {
                throw new DomainException("Já existe uma Rua cadastrada neste Bairro e Número.");
            }

            if (enderecoBanco == null)
            {
                throw new DomainException("Endereço não encontrado.");
            }

            if (!_repository.BairroExiste(enderecoDto.BairroId))
            {
                throw new DomainException("Bairro informado não existe.");
            }

            enderecoBanco.Logradouro = enderecoDto.Logradouro;
            enderecoBanco.Numero = enderecoDto.Numero;
            enderecoBanco.Complemento = enderecoDto.Complemento;
            enderecoBanco.CEP = enderecoDto.CEP;
            enderecoBanco.BairroID = enderecoDto.BairroId;

            _repository.Atualizar(enderecoBanco);
        }
    }
}
