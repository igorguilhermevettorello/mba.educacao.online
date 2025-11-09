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
            ValidarUsuarioId(usuarioId);
            ValidarNome(nome);
            ValidarEmail(email);

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
            ValidarNome(nome);
            Nome = nome;
        }

        public void AtualizarEmail(string email)
        {
            ValidarEmail(email);
            Email = email;
        }

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

        public void AdicionarCertificado(Guid cursoId, string codigo)
        {
            if (!EstaMatriculadoNoCurso(cursoId))
                throw new InvalidOperationException("Aluno deve estar matriculado no curso para receber certificado");

            var certificado = new Certificado(cursoId, codigo);
            _certificados.Add(certificado);
        }

        public void IniciarAprendizado(Guid cursoId, Guid? aulaId = null)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.Ativo);
            if (matricula == null)
                throw new InvalidOperationException("Aluno deve estar matriculado no curso para iniciar aprendizado");

            matricula.IniciarAprendizado(aulaId);
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

        public Matricula ObterMatriculaPorCurso(Guid cursoId)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.Ativo);
            if (matricula == null)
                throw new InvalidOperationException($"Matrícula ativa não encontrada para o curso {cursoId}");

            return matricula;
        }

        public int ObterTotalMatriculasAtivas()
        {
            return _matriculas.Count(m => m.Ativo);
        }

        public int ObterTotalCertificados()
        {
            return _certificados.Count(c => c.Ativo);
        }

        private static void ValidarUsuarioId(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty)
                throw new ArgumentException("ID do usuário é inválido", nameof(usuarioId));
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do aluno é obrigatório", nameof(nome));

            if (nome.Length < 3)
                throw new ArgumentException("Nome do aluno deve ter no mínimo 3 caracteres", nameof(nome));

            if (nome.Length > 100)
                throw new ArgumentException("Nome do aluno deve ter no máximo 100 caracteres", nameof(nome));
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email do aluno é obrigatório", nameof(email));

            if (email.Length > 200)
                throw new ArgumentException("Email do aluno deve ter no máximo 200 caracteres", nameof(email));

            if (!email.Contains("@") || !email.Contains("."))
                throw new ArgumentException("Email inválido", nameof(email));
        }
    }
}

