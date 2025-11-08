namespace MBA.Educacao.Online.Pagamentos.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}