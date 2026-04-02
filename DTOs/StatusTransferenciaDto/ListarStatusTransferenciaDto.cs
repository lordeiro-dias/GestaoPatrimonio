namespace GerenciamentoPatrimonio.DTOs.StatusTransferenciaDto
{
    public class ListarStatusTransferenciaDto
    {
        public Guid StatusTransferenciaId { get; set; }
        public string NomeStatus { get; set; } = string.Empty;
    }
}
