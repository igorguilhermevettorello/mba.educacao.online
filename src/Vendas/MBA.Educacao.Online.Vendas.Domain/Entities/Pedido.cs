using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Vendas.Domain.Enum;

namespace MBA.Educacao.Online.Vendas.Domain.Entities
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid AlunoId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatusEnum PedidoStatus { get; private set; }
        
        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens;

        protected Pedido() 
        {
            _pedidoItens = new List<PedidoItem>();
        }

        public Pedido(Guid alunoId)
        {
            ValidarAlunoId(alunoId);
            AlunoId = alunoId;
            _pedidoItens = new List<PedidoItem>();
        }

        public void CalcularValorPedido()
        { 
            ValorTotal = PedidoItens.Sum(p => p.Valor);
        }

        public bool PedidoItemExistente(PedidoItem item)
        {
            return _pedidoItens.Any(p => p.CursoId == item.CursoId);
        }

        public void AdicionarItem(PedidoItem item)
        { 
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item do pedido não pode ser nulo");

            if (!item.IsValid())
                throw new ArgumentException("Item do pedido é inválido", nameof(item));

            if (PedidoStatus == PedidoStatusEnum.Pago)
                throw new InvalidOperationException("Não é possível adicionar itens a um pedido já pago");

            if (PedidoStatus == PedidoStatusEnum.Cancelado)
                throw new InvalidOperationException("Não é possível adicionar itens a um pedido cancelado");

            if (PedidoItemExistente(item))
                throw new InvalidOperationException($"Curso '{item.CursoNome}' já está no pedido");

            item.AssociarPedido(this.Id);
            _pedidoItens.Add(item);
            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem item)
        { 
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item do pedido não pode ser nulo");

            if (PedidoStatus == PedidoStatusEnum.Pago)
                throw new InvalidOperationException("Não é possível remover itens de um pedido já pago");

            if (PedidoStatus == PedidoStatusEnum.Cancelado)
                throw new InvalidOperationException("Não é possível remover itens de um pedido cancelado");

            if (!_pedidoItens.Contains(item))
                throw new InvalidOperationException("Item não encontrado no pedido");

            _pedidoItens.Remove(item);
            CalcularValorPedido();
        }

        public void RemoverItemPorCursoId(Guid cursoId)
        {
            var item = _pedidoItens.FirstOrDefault(i => i.CursoId == cursoId);
            if (item == null)
                throw new InvalidOperationException($"Item com curso ID {cursoId} não encontrado no pedido");

            RemoverItem(item);
        }

        public void AtualizarStatusRascunho()
        {
            if (PedidoStatus == PedidoStatusEnum.Pago)
                throw new InvalidOperationException("Não é possível alterar status de pedido pago para rascunho");

            PedidoStatus = PedidoStatusEnum.Rascunho;
        }

        public void AtualizarStatusIniciado()
        {
            if (PedidoStatus == PedidoStatusEnum.Pago)
                throw new InvalidOperationException("Não é possível alterar status de pedido pago para iniciado");

            if (PedidoStatus == PedidoStatusEnum.Cancelado)
                throw new InvalidOperationException("Não é possível iniciar pedido cancelado");

            if (!PedidoItens.Any())
                throw new InvalidOperationException("Não é possível iniciar pedido sem itens");

            PedidoStatus = PedidoStatusEnum.Iniciado;
        }

        public void AtualizarStatusPago()
        {
            if (PedidoStatus == PedidoStatusEnum.Cancelado)
                throw new InvalidOperationException("Não é possível pagar pedido cancelado");

            if (!PedidoItens.Any())
                throw new InvalidOperationException("Não é possível pagar pedido sem itens");

            PedidoStatus = PedidoStatusEnum.Pago;
        }

        public void AtualizarStatusCancelado()
        {
            if (PedidoStatus == PedidoStatusEnum.Pago)
                throw new InvalidOperationException("Não é possível cancelar pedido já pago");

            PedidoStatus = PedidoStatusEnum.Cancelado;
        }

        public bool EstaPago()
        {
            return PedidoStatus == PedidoStatusEnum.Pago;
        }

        public bool EstaCancelado()
        {
            return PedidoStatus == PedidoStatusEnum.Cancelado;
        }

        public int ObterQuantidadeItens()
        {
            return PedidoItens.Count;
        }

        public bool PossuiItens()
        {
            return PedidoItens.Any();
        }

        public void DefinirCodigo(int codigo)
        {
            if (codigo <= 0)
                throw new ArgumentException("Código do pedido deve ser maior que zero", nameof(codigo));

            if (Codigo > 0)
                throw new InvalidOperationException("Código do pedido já foi definido");

            Codigo = codigo;
        }

        public void DefinirDataCadastro(DateTime dataCadastro)
        {
            if (dataCadastro > DateTime.UtcNow)
                throw new ArgumentException("Data de cadastro não pode ser futura", nameof(dataCadastro));

            DataCadastro = dataCadastro;
        }

        public PedidoItem ObterItemPorCursoId(Guid cursoId)
        {
            var item = _pedidoItens.FirstOrDefault(i => i.CursoId == cursoId);
            if (item == null)
                throw new InvalidOperationException($"Item com curso ID {cursoId} não encontrado no pedido");

            return item;
        }

        public decimal ObterValorMedio()
        {
            if (!PedidoItens.Any())
                return 0;

            return ValorTotal / PedidoItens.Count;
        }

        // Métodos de validação privados
        private static void ValidarAlunoId(Guid alunoId)
        {
            if (alunoId == Guid.Empty)
                throw new ArgumentException("ID do aluno é inválido", nameof(alunoId));
        }

        public static class PedidoFactory
        {
            public static Pedido CriarPedido(Guid alunoId)
            {
                var pedido = new Pedido(alunoId);
                pedido.AtualizarStatusRascunho();
                return pedido;
            }
        }
    }
}