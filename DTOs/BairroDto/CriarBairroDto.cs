namespace GerenciamentoPatrimonio.DTOs.BairroDto
{
    public class CriarBairroDto
    {
        public string NomeBairro { get; set; } = string.Empty;
        public Guid CidadeId { get; set; }
    }
}
