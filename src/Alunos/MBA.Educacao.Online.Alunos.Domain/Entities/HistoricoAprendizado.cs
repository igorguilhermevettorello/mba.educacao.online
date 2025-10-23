using MBA.Educacao.Online.Alunos.Domain.Enums;

namespace MBA.Educacao.Online.Alunos.Domain.Entities
{
    public class HistoricoAprendizado
    {
        public Guid? AulaId { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public decimal ProgressoPercentual { get; private set; }
        public StatusAprendizadoEnum Status { get; private set; }

        protected HistoricoAprendizado() { }

        public HistoricoAprendizado(Guid? aulaId = null)
        {
            AulaId = aulaId;
            DataInicio = DateTime.Now;
            ProgressoPercentual = 0;
            Status = StatusAprendizadoEnum.Pendente;
        }

        public static HistoricoAprendizado Criar(Guid? aulaId = null)
        {
            return new HistoricoAprendizado(aulaId);
        }

        public HistoricoAprendizado AtualizarProgresso(decimal percentual)
        {
            if (percentual < 0 || percentual > 100)
                throw new ArgumentException("Percentual deve estar entre 0 e 100");

            var novoHistorico = new HistoricoAprendizado
            {
                AulaId = AulaId,
                DataInicio = DataInicio,
                DataConclusao = DataConclusao,
                ProgressoPercentual = percentual,
                Status = Status
            };

            if (percentual == 100)
            {
                return novoHistorico.Concluir();
            }
            else if (percentual > 0)
            {
                novoHistorico.Status = StatusAprendizadoEnum.EmAndamento;
            }

            return novoHistorico;
        }

        public HistoricoAprendizado Concluir()
        {
            return new HistoricoAprendizado
            {
                AulaId = AulaId,
                DataInicio = DataInicio,
                DataConclusao = DateTime.Now,
                ProgressoPercentual = 100,
                Status = StatusAprendizadoEnum.Concluido
            };
        }

        public HistoricoAprendizado Iniciar()
        {
            return new HistoricoAprendizado
            {
                AulaId = AulaId,
                DataInicio = DataInicio,
                DataConclusao = DataConclusao,
                ProgressoPercentual = ProgressoPercentual,
                Status = StatusAprendizadoEnum.EmAndamento
            };
        }

        public bool EstaConcluido()
        {
            return Status == StatusAprendizadoEnum.Concluido && ProgressoPercentual == 100;
        }

        public bool EstaEmAndamento()
        {
            return Status == StatusAprendizadoEnum.EmAndamento;
        }
    }
}

