using GerenciamentoPatrimonio.Applications.Services;
using GerenciamentoPatrimonio.DTOs.TipoPatrimonioDto;
using GerenciamentoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoPatrimonioController : ControllerBase
    {
        private readonly TipoPatrimonioService _service;

        public TipoPatrimonioController(TipoPatrimonioService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarTipoPatrimonioDto>> Listar()
        {
            List<ListarTipoPatrimonioDto> tipos = _service.Listar();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public ActionResult<ListarTipoPatrimonioDto> BuscarPorId(Guid id)
        {
            try
            {
                ListarTipoPatrimonioDto tipo = _service.BuscarPorId(id);
                return Ok(tipo);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(CriarTipoPatrimonioDto dto)
        {
            try
            {
                _service.Adicionar(dto);
                return Created();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, CriarTipoPatrimonioDto dto)
        {
            try
            {
                _service.Atualizar(id, dto);
                return Ok();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
