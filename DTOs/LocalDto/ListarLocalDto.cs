namespace GerenciamentoPatrimonio.DTOs.LocalDto
{
    public class ListarLocalDto
    {
        public Guid LocalID { get; set; }
        public string NomeLocal { get; set; } = string.Empty;
        public int? LocalSAP { get; set; }
        public string DescricaoSAP { get; set; }
        public Guid AreaID { get; set; }
    }
}
