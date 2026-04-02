using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.PatrimonioDto;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class PatrimonioService
    {
        private readonly IPatrimonioRepository _repository;

        public PatrimonioService(IPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarPatrimonioDto> Listar()
        {
            List<Patrimonio> patrimonios = _repository.Listar();
            List<ListarPatrimonioDto> patrimoniosDto = patrimonios.Select(p => new ListarPatrimonioDto
            {
                PatrimonioId = p.PatrimonioID,
                Denominacao = p.Denominacao,
                NumeroPatrimonio = p.NumeroPatrimonio,
                Valor = p.Valor,
                Imagem = p.Imagem,
                LocalID= p.LocalID,
                TipoPatrimonioID = p.TipoPatrimonioID,
                StatusPatrimonioID = p.StatusPatrimonioID
            }).ToList();

            return patrimoniosDto;
        }

        public ListarPatrimonioDto BuscarPorId(Guid id)
        {
            Patrimonio patrimonio = _repository.BuscarPorId(id);

            return new ListarPatrimonioDto
            {
                PatrimonioId = patrimonio.PatrimonioID,
                Denominacao = patrimonio.Denominacao,
                NumeroPatrimonio = patrimonio.NumeroPatrimonio,
                Valor = patrimonio.Valor,
                Imagem = patrimonio.Imagem,
                LocalID = patrimonio.LocalID,
                TipoPatrimonioID = patrimonio.TipoPatrimonioID,
                StatusPatrimonioID = patrimonio.StatusPatrimonioID
            };
        }


    }
}
