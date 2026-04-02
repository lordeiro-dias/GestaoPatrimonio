using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.CargoDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class CargoService
    {
        private readonly ICargoRepository _repository;

        public CargoService(ICargoRepository repository)
        {
            _repository = repository;
        }

        public List<ListarCargoDto> Listar()
        {
            List<Cargo> cargos = _repository.Listar();

            List<ListarCargoDto> cargosDto = cargos.Select(c => new ListarCargoDto
            {
                cargoId = c.CargoID,
                nomeCargo = c.NomeCargo
            }).ToList();

            return cargosDto;
        }

        public ListarCargoDto BuscarPorId(Guid cargoId)
        {
            Cargo cargo = _repository.BuscarPorId(cargoId);

            return new ListarCargoDto
            {
                cargoId = cargo.CargoID,
                nomeCargo = cargo.NomeCargo
            };
        }

        public void Adicionar(CriarCargoDto dto)
        {
            Validar.ValidarNome(dto.nomeCargo);
            Cargo cargoExistente = _repository.BuscarPorNome(dto.nomeCargo);

            if(cargoExistente != null)
            {
                throw new DomainException("Já existe um cargo cadastrado com esse nome.");
            }

            Cargo cargo = new Cargo
            { 
                NomeCargo = dto.nomeCargo
            };

            _repository.Adicionar(cargo);
        }

        public void Atualizar(Guid cargoId, CriarCargoDto dto)
        {
            Validar.ValidarNome(dto.nomeCargo);
            Cargo cargoExistente = _repository.BuscarPorNome(dto.nomeCargo);

            if (cargoExistente != null)
            {
                throw new DomainException("Já existe um cargo cadastrado com esse nome.");
            }

            Cargo cargoBanco = _repository.BuscarPorId(cargoId);

            if(cargoBanco == null)
            {
                throw new DomainException("Cargo não encontrado.");
            }

            cargoBanco.NomeCargo = dto.nomeCargo;
            _repository.Atualizar(cargoBanco);
        }
    }
}
