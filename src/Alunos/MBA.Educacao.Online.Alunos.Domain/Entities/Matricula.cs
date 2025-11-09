using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Alunos.Domain.Entities
{
    public class Matricula : Entity
    {
        private readonly List<HistoricoAprendizado> _historicosAprendizado = new();

        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public int ProgressoPercentual { get; private set; }

        // Coleção de leitura para históricos
        public IReadOnlyCollection<HistoricoAprendizado> HistoricosAprendizado => _historicosAprendizado.AsReadOnly();

        protected Matricula() { }

        public Matricula(Guid cursoId, DateTime dataValidade)
        {
            ValidarCursoId(cursoId);
            ValidarDataValidade(dataValidade);

            CursoId = cursoId;
            DataMatricula = DateTime.Now;
            DataValidade = dataValidade;
            Ativo = true;
            ProgressoPercentual = 0;
        }

        public Matricula(Guid alunoId, Guid cursoId, DateTime dataValidade) : this(cursoId, dataValidade)
        {
            ValidarAlunoId(alunoId);
            AlunoId = alunoId;
        }

        public void Cancelar()
        {
            if (!Ativo)
                throw new InvalidOperationException("Matrícula já está cancelada");

            Ativo = false;
        }

        public void Reativar()
        {
            if (Ativo)
                throw new InvalidOperationException("Matrícula já está ativa");

            if (EstaVencida())
                throw new InvalidOperationException("Não é possível reativar matrícula vencida. Estenda a validade primeiro");

            Ativo = true;
        }

        public void EstenderValidade(DateTime novaDataValidade)
        {
            ValidarExtensaoValidade(novaDataValidade);
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

        public void IniciarEstudoAula(Guid aulaId)
        {
            var historico = _historicosAprendizado
                .FirstOrDefault(h => h.AulaId == aulaId);

            if (historico == null)
            {
                // Se não existe histórico, inicia o aprendizado
                IniciarAprendizado(aulaId);
                return;
            }

            // Se já existe, atualiza para em andamento
            var historicoAtualizado = historico.IniciarEstudo();
            _historicosAprendizado.Remove(historico);
            _historicosAprendizado.Add(historicoAtualizado);
        }

        public void FinalizarEstudoAula(Guid aulaId, int totalAulasCurso)
        {
            var historico = _historicosAprendizado
                .FirstOrDefault(h => h.AulaId == aulaId);

            if (historico == null)
                throw new InvalidOperationException("Histórico de aprendizado não encontrado. Inicie o estudo da aula primeiro.");

            var historicoFinalizado = historico.FinalizarEstudo();
            _historicosAprendizado.Remove(historico);
            _historicosAprendizado.Add(historicoFinalizado);

            // Atualiza o progresso geral da matrícula
            CalcularProgressoGeral(totalAulasCurso);
        }

        public void CalcularProgressoGeral(int totalAulasCurso)
        {
            if (totalAulasCurso <= 0)
                throw new ArgumentException("Total de aulas do curso deve ser maior que zero", nameof(totalAulasCurso));

            var aulasConcluidas = _historicosAprendizado.Count(h => h.EstaConcluido());

            if (aulasConcluidas == 0)
            {
                ProgressoPercentual = 0;
                return;
            }

            ProgressoPercentual = (int)((decimal)aulasConcluidas / totalAulasCurso * 100);
        }

        public int ObterProgressoGeral()
        {
            return ProgressoPercentual;
        }

        public bool EstaConcluida()
        {
            return _historicosAprendizado.Any() &&
                   _historicosAprendizado.All(h => h.EstaConcluido());
        }

        public int ObterTotalHistoricos()
        {
            return _historicosAprendizado.Count;
        }

        public TimeSpan ObterTempoDecorrido()
        {
            return DateTime.Now - DataMatricula;
        }

        public int ObterDiasRestantes()
        {
            var dias = (DataValidade - DateTime.Now).Days;
            return dias > 0 ? dias : 0;
        }

        // Métodos de validação privados
        private static void ValidarAlunoId(Guid alunoId)
        {
            if (alunoId == Guid.Empty)
                throw new ArgumentException("ID do aluno é inválido", nameof(alunoId));
        }

        private static void ValidarCursoId(Guid cursoId)
        {
            if (cursoId == Guid.Empty)
                throw new ArgumentException("ID do curso é inválido", nameof(cursoId));
        }

        private static void ValidarDataValidade(DateTime dataValidade)
        {
            if (dataValidade <= DateTime.Now)
                throw new ArgumentException("Data de validade deve ser futura", nameof(dataValidade));

            var diferencaDias = (dataValidade - DateTime.Now).Days;
            if (diferencaDias > 3650) // 10 anos
                throw new ArgumentException("Data de validade não pode exceder 10 anos", nameof(dataValidade));
        }

        private void ValidarExtensaoValidade(DateTime novaDataValidade)
        {
            if (novaDataValidade <= DataValidade)
                throw new ArgumentException("Nova data de validade deve ser posterior à data atual", nameof(novaDataValidade));

            var diferencaDias = (novaDataValidade - DateTime.Now).Days;
            if (diferencaDias > 3650) // 10 anos
                throw new ArgumentException("Data de validade não pode exceder 10 anos", nameof(novaDataValidade));
        }
    }
}

