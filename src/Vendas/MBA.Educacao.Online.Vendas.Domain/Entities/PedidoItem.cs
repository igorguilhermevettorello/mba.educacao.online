using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Vendas.Domain.Entities
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid CursoId { get; private set; }
        public string CursoNome { get; private set; }
        public decimal Valor { get; private set; }
        public Guid? MatriculaId { get; private set; }
        public Pedido Pedido { get; private set; }

        public PedidoItem() 
        {
            CursoNome = string.Empty;
        }

        public PedidoItem(Guid cursoId, string cursoNome, decimal valor)
        {
            ValidarCursoId(cursoId);
            ValidarCursoNome(cursoNome);
            ValidarValor(valor);

            CursoId = cursoId;
            CursoNome = cursoNome;
            Valor = valor;
        }

        public void AtribuirMatricula(Guid matriculaId)
        {
            ValidarMatriculaId(matriculaId);

            if (MatriculaId.HasValue)
                throw new InvalidOperationException("Item já possui uma matrícula associada");

            MatriculaId = matriculaId;
        }

        public void AssociarPedido(Guid pedidoId)
        {
            ValidarPedidoId(pedidoId);
            PedidoId = pedidoId;
        }

        public bool PossuiMatricula()
        {
            return MatriculaId.HasValue;
        }

        public override bool IsValid()
        {
            return CursoId != Guid.Empty 
                && !string.IsNullOrWhiteSpace(CursoNome) 
                && Valor > 0;
        }

        // Métodos de validação privados
        private static void ValidarPedidoId(Guid pedidoId)
        {
            if (pedidoId == Guid.Empty)
                throw new ArgumentException("ID do pedido é inválido", nameof(pedidoId));
        }

        private static void ValidarCursoId(Guid cursoId)
        {
            if (cursoId == Guid.Empty)
                throw new ArgumentException("ID do curso é inválido", nameof(cursoId));
        }

        private static void ValidarCursoNome(string cursoNome)
        {
            if (string.IsNullOrWhiteSpace(cursoNome))
                throw new ArgumentException("Nome do curso é obrigatório", nameof(cursoNome));

            if (cursoNome.Length < 3)
                throw new ArgumentException("Nome do curso deve ter no mínimo 3 caracteres", nameof(cursoNome));

            if (cursoNome.Length > 200)
                throw new ArgumentException("Nome do curso deve ter no máximo 200 caracteres", nameof(cursoNome));
        }

        private static void ValidarValor(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor do item deve ser maior que zero", nameof(valor));

            if (valor > 999999.99m)
                throw new ArgumentException("Valor do item excede o limite máximo permitido", nameof(valor));
        }

        private static void ValidarMatriculaId(Guid matriculaId)
        {
            if (matriculaId == Guid.Empty)
                throw new ArgumentException("ID da matrícula é inválido", nameof(matriculaId));
        }
    }
}
