using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Alunos.Domain.Entities
{
    public class Aluno : Entity, IAggregateRoot
    {
        private readonly List<Matricula> _matriculas = new();
        private readonly List<Certificado> _certificados = new();

        public string Nome { get; private set; }
        public string Email { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }

        // Coleções de leitura
        public IReadOnlyCollection<Matricula> Matriculas => _matriculas.AsReadOnly();
        public IReadOnlyCollection<Certificado> Certificados => _certificados.AsReadOnly();

        protected Aluno() 
        {
            Nome = string.Empty;
            Email = string.Empty;
        }

        public Aluno(Guid usuarioId, string nome, string email)
        {
            Id = usuarioId;
            Nome = nome;
            Email = email;
            DataCadastro = DateTime.Now;
            Ativo = true;
        }

        public void Inativar()
        {
            Ativo = false;
        }

        public void Ativar()
        {
            Ativo = true;
        }

        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }

        // Métodos para gerenciar matrículas
        public void AdicionarMatricula(Guid cursoId, DateTime dataValidade)
        {
            if (EstaMatriculadoNoCurso(cursoId))
                throw new InvalidOperationException("Aluno já está matriculado neste curso");

            var matricula = new Matricula(cursoId, dataValidade);
            _matriculas.Add(matricula);
        }

        public void CancelarMatricula(Guid cursoId)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId);
            if (matricula == null)
                throw new InvalidOperationException("Matrícula não encontrada");

            matricula.Cancelar();
        }

        public bool EstaMatriculadoNoCurso(Guid cursoId)
        {
            return _matriculas.Any(m => m.CursoId == cursoId && m.Ativo);
        }

        // Métodos para gerenciar certificados
        public void AdicionarCertificado(Guid cursoId, string codigo)
        {
            if (!EstaMatriculadoNoCurso(cursoId))
                throw new InvalidOperationException("Aluno deve estar matriculado no curso para receber certificado");

            var certificado = new Certificado(cursoId, codigo);
            _certificados.Add(certificado);
        }

        // Métodos para gerenciar aprendizado através das matrículas
        public void IniciarAprendizado(Guid cursoId, Guid? aulaId = null)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.Ativo);
            if (matricula == null)
                throw new InvalidOperationException("Aluno deve estar matriculado no curso para iniciar aprendizado");

            matricula.IniciarAprendizado(aulaId);
        }

        public void AtualizarProgressoAprendizado(Guid cursoId, decimal percentual, Guid? aulaId = null)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.Ativo);
            if (matricula == null)
                throw new InvalidOperationException("Matrícula não encontrada");

            matricula.AtualizarProgressoAprendizado(percentual, aulaId);
        }

        public void ConcluirAprendizado(Guid cursoId, Guid? aulaId = null)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.Ativo);
            if (matricula == null)
                throw new InvalidOperationException("Matrícula não encontrada");

            matricula.ConcluirAprendizado(aulaId);
        }

        public decimal ObterProgressoNoCurso(Guid cursoId)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.Ativo);
            if (matricula == null)
                return 0;

            return matricula.ObterProgressoGeral();
        }

        public bool CursoEstaConcluido(Guid cursoId)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.Ativo);
            if (matricula == null)
                return false;

            return matricula.EstaConcluida();
        }
    }
}

