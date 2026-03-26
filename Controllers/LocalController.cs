using GerenciamentoPatrimonio.Applications.Services;
using GerenciamentoPatrimonio.DTOs.LocalDto;
using GerenciamentoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalController : ControllerBase
    {
        private readonly LocalService _service;

        public LocalController(LocalService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarLocalDto>> Listar()
        {
            List<ListarLocalDto> localizacoes = _service.Listar();
            return Ok(localizacoes);
        }

        [HttpGet("{id}")]
        public ActionResult<ListarLocalDto> BuscarPorId(Guid id)
        {
            try
            {
                ListarLocalDto localizacao = _service.BuscarPorId(id);
                return Ok(localizacao);
            }
            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(CriarLocalDto localDto)
        {
            try
            {
                _service.Adicionar(localDto);
                return Created();
            }
            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, CriarLocalDto localDto)
        {
            try
            {
                _service.Atualizar(id, localDto);
                return Ok();
            }
            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
