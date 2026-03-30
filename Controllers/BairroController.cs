using GerenciamentoPatrimonio.Applications.Services;
using GerenciamentoPatrimonio.DTOs.BairroDto;
using GerenciamentoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BairroController : ControllerBase
    {
        private readonly BairroService _service;

        public BairroController(BairroService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarBairroDto>> Listar()
        {
            List<ListarBairroDto> bairros = _service.Listar();
            return bairros;
        }

        [HttpGet("{id}")]
        public ActionResult<ListarBairroDto> ObterPorId(Guid id)
        {
            try
            {
                ListarBairroDto bairro = _service.ObterPorId(id);
                return bairro;
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(CriarBairroDto bairroDto)
        {
            try
            {
                _service.Adicionar(bairroDto);
                return Created();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, CriarBairroDto bairroDto)
        {
            try
            {
                _service.Atualizar(id, bairroDto);
                return NoContent();
            }
            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
