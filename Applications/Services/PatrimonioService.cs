using CsvHelper;
using CsvHelper.Configuration;
using GerenciamentoPatrimonio.Applications.Mapeamentos;
using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.PatrimonioDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;
using System.Globalization;

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
                StatusPatrimonioID = p.StatusPatrimonioID
            }).ToList();

            return patrimoniosDto;
        }

        public ListarPatrimonioDto BuscarPorId(Guid id)
        {
            Patrimonio patrimonio = _repository.BuscarPorId(id);

            if(patrimonio == null)
            {
                throw new DomainException("Patrimônio não encontrado");
            }

            return new ListarPatrimonioDto
            {
                PatrimonioId = patrimonio.PatrimonioID,
                Denominacao = patrimonio.Denominacao,
                NumeroPatrimonio = patrimonio.NumeroPatrimonio,
                Valor = patrimonio.Valor,
                Imagem = patrimonio.Imagem,
                LocalID = patrimonio.LocalID,
                StatusPatrimonioID = patrimonio.StatusPatrimonioID
            };
        }

        public void Adicionar(IFormFile arquivoCsv, Guid usuarioId)
        {
            if (arquivoCsv == null || arquivoCsv.Length == 0)
            {
                throw new DomainException("Arquivo CSV é obrigatório.");
            }

            Local localizacaoSemLocal = _repository.BuscarLocalPorNome("Sem local");

            if (localizacaoSemLocal == null)
            {
                throw new DomainException("Localização 'Sem local' não cadastrada.");
            }

            StatusPatrimonio statusAtivo = _repository.BuscarStatusPatrimonioPorNome("Ativo");

            if (statusAtivo == null)
            {
                throw new DomainException("Status 'Ativo' não cadastrado.");
            }

            TipoAlteracao tipoAlteracao = _repository.BuscarTipoAlteracaoPorNome("Atualização de dados");

            if (tipoAlteracao == null)
            {
                throw new DomainException("Tipo de alteração 'Atualização de dados' não cadastrado.");
            }

            List<ImportarPatrimonioCsvDto> registros;

            // Abre o arquivo enviado (IFormFile)
            using (var stream = arquivoCsv.OpenReadStream())
            // Lê o arquivo como texto
            using (var reader = new StreamReader(stream))

            // Cria leitor de CSV com configurações personalizadas
            // CultureInfo define como números, datas e textos são interpretados
            // InvaruantCulture -> padrão universal
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Define que o separador é ponto e vírgula
                Delimiter = ";",

                // Ignora erros caso o cabeçalho não bata 100%
                // Não trava a aplicação por conta de formatação, vamos tratar os erros depois.
                HeaderValidated = null,

                // Ignora o erro se faltar algum campo
                MissingFieldFound = null,

                //Ignora dados "quebrados" no CSV
                BadDataFound = null, // EX. abriu aspas e não fechou

                // Remove espaços extras automaticamente
                TrimOptions = TrimOptions.Trim
            }))
            {
                // Registra o mapa que criamos (Tradução CSV -> DTO)
                csv.Context.RegisterClassMap<ImportarPatrimonioCsvMap>();

                // Pega todas as linhas do CSV e converte para uma lista de DTO
                registros = csv.GetRecords<ImportarPatrimonioCsvDto>().ToList();
            }

            var erros = new List<string>();

            foreach (var item in registros)
            {
                //se não tem numero patrimonio, ignora registro
                if (string.IsNullOrWhiteSpace(item.NumeroPatrimonio))
                { 
                    continue; 
                }

                //remove espaços extras do numero
                string numeroPatrimonio = item.NumeroPatrimonio.Trim();

                if(string.IsNullOrWhiteSpace(item.Denominacao))
                {
                    erros.Add($"Patrimônio {numeroPatrimonio} sem denominação.");
                    continue; // não cadastra, mas segue no loop
                }

                string denominacao = item.Denominacao.Trim();

                DateTime? dataIncorporacao = null;

                // usa o formato brasileiro só pra ler
                // Depois pega o DateTime e formata
                if(!string.IsNullOrWhiteSpace(item.DataIncorporacao))
                {
                    if(DateTime.TryParse(item.DataIncorporacao, new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime dataConvertida))
                    {
                        dataIncorporacao = dataConvertida;
                    }
                }

                decimal? valorAquisicao = null;
                if(!string.IsNullOrWhiteSpace(item.ValorAquisicao))
                {
                    // Remove separador de milhar e 
                    string valorTexto = item.ValorAquisicao.Replace(".", "").Replace(",", ".");

                    // TryParse - converte string -> decimal
                    // NumberStyles.Any -> define quais formatos de número são permitidos - any aceita qualquer número, com espaço, etc.
                    // out decimal valorConvertido -> se der certo: cria a variavel com o valor já convertido

                    if(decimal.TryParse(valorTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorConvertido))
                    {
                        valorAquisicao = valorConvertido;
                    }

                    Validar.ValidarNumeroPatrimonio(numeroPatrimonio);
                    Validar.ValidarNome(denominacao);

                    bool patrimonioExistente = _repository.BuscarPorNumeroPatrimonio(numeroPatrimonio);

                    if (patrimonioExistente == true)
                    {
                        continue;
                    }

                    Patrimonio patrimonio = new Patrimonio
                    {
                        Denominacao = denominacao,
                        NumeroPatrimonio = numeroPatrimonio,
                        Valor = valorAquisicao,
                        Imagem = null,
                        LocalID = localizacaoSemLocal.LocalID,
                        StatusPatrimonioID = statusAtivo.StatusPatrimonioID
                    };

                    _repository.Adicionar(patrimonio);

                    LogPatrimonio log = new LogPatrimonio
                    {
                        DataTransferencia = dataIncorporacao ?? DateTime.Now,
                        TipoAlteracaoID = tipoAlteracao.TipoAlteracaoID,
                        StatusPatrimonioID = patrimonio.StatusPatrimonioID,
                        PatrimonioID = patrimonio.PatrimonioID,
                        UsuarioID = usuarioId,
                        LocalID = patrimonio.LocalID
                    };

                    _repository.AdicionarLog(log);
                }
            }
        }
        public void AtualizarStatus(Guid patrimonioId, AtualizarStatusPatrimonioDto dto)
        {
            Patrimonio patrimonioBanco = _repository.BuscarPorId(patrimonioId);

            if (patrimonioBanco == null)
            {
                throw new DomainException("Patrimônio não encontrado.");
            }

            if (!_repository.StatusPatrimonioExiste(dto.StatusPatrimonioID))
            {
                throw new DomainException("Status de patrimônio informado não existe.");
            }

            patrimonioBanco.StatusPatrimonioID = dto.StatusPatrimonioID;

            _repository.AtualizarStatus(patrimonioBanco);
        }
    }
}
