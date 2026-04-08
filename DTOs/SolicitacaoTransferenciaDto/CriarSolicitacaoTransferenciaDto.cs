namespace GerenciamentoPatrimonio.DTOs.SolicitacaoTransferenciaDto
{
    public class CriarSolicitacaoTransferenciaDto
    {
        public string Justificativa { get; set; }
        public Guid PatrimonioId { get; set; }
        public Guid LocalId { get; set; }
    }
}
