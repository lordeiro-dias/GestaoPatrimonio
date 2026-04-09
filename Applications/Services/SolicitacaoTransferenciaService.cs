using GerenciamentoPatrimonio.Applications.Regras;
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

        public void Adicionar(Guid usuarioId, CriarSolicitacaoTransferenciaDto dto)
        {
            Validar.ValidarJustificativa(dto.Justificativa);

            Usuario usuario = _usuarioRepository.BuscarPorId(usuarioId);

            if(usuario == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            Patrimonio patrimonio = _repository.BuscarPatrimonioPorId(dto.PatrimonioId);

            if(patrimonio == null)
            {
                throw new DomainException("Patrimônio não encontrado.");
            }

            if(!_repository.LocalExiste(dto.LocalId))
            {
                throw new DomainException("Local não existe.");
            }

            if(patrimonio.LocalID == dto.LocalId)
            {
                throw new DomainException("O patrimnônio já está nessa localização.");
            }

            if(_repository.ExisteSolicitacaoPendente(dto.PatrimonioId))
            {
                throw new DomainException("Já existe uma solicitção pendente para esse patrimônio.");
            }

            if(usuario.TipoUsuario.NomeTipo == "Responsável")
            {
                bool usuarioResponsavel = _repository.UsuarioResponsavelDaLocalizacao(usuarioId, patrimonio.LocalID);

                if(!usuarioResponsavel) // se retornar falso
                {
                    throw new DomainException("O responsável só pode socilitar transferência de patrimônio do ambiente ao qual está vinculado.");
                }
            }

            StatusTransferencia statusPendente = _repository.BuscarStatusTransferenciaPorNome("Pendente de Aprovação");

            if(statusPendente == null)
            {
                throw new DomainException("Status de transferência pendente não encontrado.");
            }

            SolicitacaoTransferencia solicitacao = new SolicitacaoTransferencia
            {
                DataCriacaoSolicitacao = DateTime.Now,
                Justificativa = dto.Justificativa,
                StatusTransferenciaID = statusPendente.StatusTransferenciaID,
                UsuarioIDSolicitacao = usuarioId,
                UsuarioIDAprovacao = null,
                PatrimonioID = dto.PatrimonioId,
                LocalID = dto.LocalId
            };

            _repository.Adicionar(solicitacao);
        }

        public void Responder(Guid transferenciaId, Guid usuarioId, ResponderSolicitacaoTransferenciaDto dto)
        {
            Usuario usuario = _usuarioRepository.BuscarPorId(usuarioId);

            if(usuario == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            SolicitacaoTransferencia solicitacao = _repository.BuscarPorId(transferenciaId);

            if(solicitacao == null)
            {
                throw new DomainException("Solicitação de transferência não encontrada.");
            }

            Patrimonio patrimonio = _repository.BuscarPatrimonioPorId(solicitacao.PatrimonioID);

            if(patrimonio == null)
            {
                throw new DomainException("Patrimônio não encontrado.");
            }

            StatusTransferencia statusPendente = _repository.BuscarStatusTransferenciaPorNome("Pendente de aprovação");

            if(statusPendente == null)
            {
                throw new DomainException("Status pendente não encontrado.");
            }

            if(solicitacao.StatusTransferenciaID != statusPendente.StatusTransferenciaID)
            {
                throw new DomainException("Essa soliciação já foi respondida.");
            }

            if(usuario.TipoUsuario.NomeTipo == "Reponsável")
            {
                bool usuarioReponsavel = _repository.UsuarioResponsavelDaLocalizacao(usuarioId, patrimonio.LocalID);

                if (!usuarioReponsavel)
                {
                    throw new DomainException("Somente o responsável do ambiente de origem pode aprovar ou rejeitar essa solicitação.");
                }
            }

            StatusTransferencia statusReposta;

            if(dto.Aprovado)
            {
                statusReposta = _repository.BuscarStatusTransferenciaPorNome("Aprovado");
            }
            else
            {
                statusReposta = _repository.BuscarStatusTransferenciaPorNome("Recusado");
            }

            if(statusReposta == null)
            {
                throw new DomainException("Status de resposta da transferência não encontrado");
            }

            solicitacao.StatusTransferenciaID = statusReposta.StatusTransferenciaID;
            solicitacao.UsuarioIDAprovacao = usuarioId;
            solicitacao.DataResposta = DateTime.Now;

            _repository.Atualizar(solicitacao);

            if(dto.Aprovado)
            {
                StatusPatrimonio statusTransferido = _repository.BuscarStatusPatrimonioPorNome("Transferido");

                if(statusTransferido == null)
                {
                    throw new DomainException("Status de patrimônio 'Transferido' não encontrado.");
                }

                TipoAlteracao tipoAlteracao = _repository.BuscarTipoAlteracaoPorNome("Transferência");

                if(tipoAlteracao == null)
                {
                    throw new DomainException("Tipo de alteração 'Transferência' não encontrado.");
                }

                patrimonio.LocalID = solicitacao.LocalID;
                patrimonio.StatusPatrimonioID = statusTransferido.StatusPatrimonioID;

                _repository.AtualizarPatrimonio(patrimonio);

                LogPatrimonio log = new LogPatrimonio
                {
                    DataTransferencia = DateTime.Now,
                    TipoAlteracaoID = tipoAlteracao.TipoAlteracaoID,
                    StatusPatrimonioID = statusTransferido.StatusPatrimonioID,
                    PatrimonioID = patrimonio.PatrimonioID,
                    UsuarioID = usuarioId,
                    LocalID = patrimonio.LocalID
                };

                _repository.AdicionarLog(log);
            }
        }
    }
}
