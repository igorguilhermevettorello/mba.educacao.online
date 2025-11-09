using MBA.Educacao.Online.Alunos.Domain.Enums;

namespace MBA.Educacao.Online.Alunos.Domain.Entities
{
    public class HistoricoAprendizado
    {
        public Guid? AulaId { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public StatusAprendizadoEnum Status { get; private set; }

        protected HistoricoAprendizado() { }

        public HistoricoAprendizado(Guid? aulaId = null)
        {
            AulaId = aulaId;
            DataInicio = DateTime.Now;
            Status = StatusAprendizadoEnum.EmAndamento;
        }

        public static HistoricoAprendizado Criar(Guid? aulaId = null)
        {
            return new HistoricoAprendizado(aulaId);
        }

        public HistoricoAprendizado IniciarEstudo()
        {
            if (Status == StatusAprendizadoEnum.Concluido)
                throw new InvalidOperationException("Aula já foi concluída");

            return new HistoricoAprendizado
            {
                AulaId = AulaId,
                DataInicio = DataInicio,
                DataConclusao = DataConclusao,
                Status = StatusAprendizadoEnum.EmAndamento
            };
        }

        public HistoricoAprendizado FinalizarEstudo()
        {
            if (Status == StatusAprendizadoEnum.Concluido)
                throw new InvalidOperationException("Aula já foi concluída");

            if (Status != StatusAprendizadoEnum.EmAndamento)
                throw new InvalidOperationException("Aula precisa estar em andamento para ser finalizada");

            return new HistoricoAprendizado
            {
                AulaId = AulaId,
                DataInicio = DataInicio,
                DataConclusao = DateTime.Now,
                Status = StatusAprendizadoEnum.Concluido
            };
        }

        public bool EstaConcluido()
        {
            return Status == StatusAprendizadoEnum.Concluido;
        }

        public bool EstaEmAndamento()
        {
            return Status == StatusAprendizadoEnum.EmAndamento;
        }
    }
}

