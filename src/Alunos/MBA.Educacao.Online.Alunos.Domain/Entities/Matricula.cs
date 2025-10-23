using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Alunos.Domain.Entities
{
    public class Matricula : Entity
    {
        private readonly List<HistoricoAprendizado> _historicosAprendizado = new();

        public Guid CursoId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }

        // Coleção de leitura para históricos
        public IReadOnlyCollection<HistoricoAprendizado> HistoricosAprendizado => _historicosAprendizado.AsReadOnly();

        protected Matricula() { }

        public Matricula(Guid cursoId, DateTime dataValidade)
        {
            CursoId = cursoId;
            DataMatricula = DateTime.Now;
            DataValidade = dataValidade;
            Ativo = true;
        }

        public void Cancelar()
        {
            Ativo = false;
        }

        public void Reativar()
        {
            Ativo = true;
        }

        public void EstenderValidade(DateTime novaDataValidade)
        {
            if (novaDataValidade <= DataValidade)
                throw new ArgumentException("Nova data de validade deve ser posterior à data atual");

            DataValidade = novaDataValidade;
        }

        public bool EstaVencida()
        {
            return DateTime.Now > DataValidade;
        }

        // Métodos para gerenciar histórico de aprendizado
        public void IniciarAprendizado(Guid? aulaId = null)
        {
            if (!Ativo)
                throw new InvalidOperationException("Matrícula deve estar ativa para iniciar aprendizado");

            var historico = HistoricoAprendizado.Criar(aulaId);
            _historicosAprendizado.Add(historico);
        }

        public void AtualizarProgressoAprendizado(decimal percentual, Guid? aulaId = null)
        {
            var historico = _historicosAprendizado
                .FirstOrDefault(h => h.AulaId == aulaId);

            if (historico == null)
                throw new InvalidOperationException("Histórico de aprendizado não encontrado");

            var novoHistorico = historico.AtualizarProgresso(percentual);
            
            // Remove o histórico antigo e adiciona o novo
            _historicosAprendizado.Remove(historico);
            _historicosAprendizado.Add(novoHistorico);
        }

        public void ConcluirAprendizado(Guid? aulaId = null)
        {
            var historico = _historicosAprendizado
                .FirstOrDefault(h => h.AulaId == aulaId);

            if (historico == null)
                throw new InvalidOperationException("Histórico de aprendizado não encontrado");

            var historicoConcluido = historico.Concluir();
            
            // Remove o histórico antigo e adiciona o concluído
            _historicosAprendizado.Remove(historico);
            _historicosAprendizado.Add(historicoConcluido);
        }

        public decimal ObterProgressoGeral()
        {
            if (!_historicosAprendizado.Any())
                return 0;

            return _historicosAprendizado.Average(h => h.ProgressoPercentual);
        }

        public bool EstaConcluida()
        {
            return _historicosAprendizado.Any() && 
                   _historicosAprendizado.All(h => h.EstaConcluido());
        }
    }
}

