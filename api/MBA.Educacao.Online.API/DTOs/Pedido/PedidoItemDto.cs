namespace MBA.Educacao.Online.API.DTOs.Pedido
{
    public class PedidoItemDto
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public string CursoNome { get; set; }
        public decimal Valor { get; set; }
        public Guid? MatriculaId { get; set; }

        public PedidoItemDto()
        {
            CursoNome = string.Empty;
        }
    }
}

