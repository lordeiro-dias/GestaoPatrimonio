using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.SolicitacaoTransferenciaDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class SolicitacaoTransferenciaService
    {
        private readonly ISolicitacaoTransferenciaRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SolicitacaoTransferenciaService(ISolicitacaoTransferenciaRepository repository, IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public List<ListarSolicitacaoTransferenciaDto> Listar()
        {
            List<SolicitacaoTransferencia> solicitacoes = _repository.Listar();
            List<ListarSolicitacaoTransferenciaDto> solicitacoesDto = solicitacoes.Select(s => new ListarSolicitacaoTransferenciaDto
            {
                SolicitacaoTransferenciaId = s.SolicitacaoTransferenciaID,
                DataCriacaoSolicitante = s.DataCriacaoSolicitacao,
                DataResposta = s.DataResposta,
                Justificativa = s.Justificativa,
                StatusTransferenciaId = s.StatusTransferenciaID,
                UsuarioIdSolicitacao = s.UsuarioIDSolicitacao,
                UsuarioIdAprovacao = s.UsuarioIDAprovacao,
                PatrimonioId = s.PatrimonioID,
                LocalId = s.LocalID
            }).ToList();

            return solicitacoesDto;
        }

        public ListarSolicitacaoTransferenciaDto BuscarPorId(Guid id)
        {
            SolicitacaoTransferencia solicitacao = _repository.BuscarPorId(id);

            if(solicitacao == null)
            {
                throw new DomainException("Solicitação de Transferência não encontrada.");
            }

            return new ListarSolicitacaoTransferenciaDto
            {
                SolicitacaoTransferenciaId = solicitacao.SolicitacaoTransferenciaID,
                DataCriacaoSolicitante = solicitacao.DataCriacaoSolicitacao,
                DataResposta = solicitacao.DataResposta,
                Justificativa = solicitacao.Justificativa,
                StatusTransferenciaId = solicitacao.StatusTransferenciaID,
                UsuarioIdSolicitacao = solicitacao.UsuarioIDSolicitacao,
                UsuarioIdAprovacao = solicitacao.UsuarioIDAprovacao,
                PatrimonioId = solicitacao.PatrimonioID,
                LocalId = solicitacao.LocalID
            };
        }
    }
}
