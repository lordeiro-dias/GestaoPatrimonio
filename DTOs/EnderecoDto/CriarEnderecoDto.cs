namespace GerenciamentoPatrimonio.DTOs.EnderecoDto
{
    public class CriarEnderecoDto
    {
        public string Logradouro { get; set; } = string.Empty;
        public int? Numero { get; set; }
        public string Complemento { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public Guid BairroId { get; set; }
    }
}
