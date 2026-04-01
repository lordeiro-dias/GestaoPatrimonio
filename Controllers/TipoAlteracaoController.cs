using GerenciamentoPatrimonio.Applications.Services;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.TipoAlteracaoDto;
using GerenciamentoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoAlteracaoController : ControllerBase
    {
        private readonly TipoAlteracaoService _service;

        public TipoAlteracaoController(TipoAlteracaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarTipoAlteracaoDto>> Listar()
        {
            List<ListarTipoAlteracaoDto> tiposDto = _service.Listar();

            return Ok(tiposDto);
        }

        [HttpGet("{id}")]
        public ActionResult<ListarTipoAlteracaoDto> BuscarPorId(Guid id)
        {
            try
            {
                ListarTipoAlteracaoDto tipo = _service.BuscarPorId(id);
                return Ok(tipo);
            }
            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(CriarTipoAlteracaoDto dto)
        {
            try
            {
                _service.Adicionar(dto);
                return Created();
            }
            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, CriarTipoAlteracaoDto dto)
        {
            try
            {
                _service.Atualizar(id,dto);
                return Created();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
