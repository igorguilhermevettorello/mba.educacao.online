using MBA.Educacao.Online.Alunos.Data.Context;

namespace MBA.Educacao.Online.Alunos.Data.Seeds
{
    public static class SeederAplicacao
    {
        public static async Task EnsureSeedAlunos(AlunoContext context, string env)
        {
            // Verifica se já existem dados
            if (context.Alunos.Any())
            {
                return; // Já possui dados, não precisa popular
            }

            // TODO: Adicionar dados iniciais se necessário
            // Por exemplo, alunos de teste em ambiente de desenvolvimento
            
            if (env == "Development")
            {
                // Exemplo de seed para desenvolvimento
                // var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno.teste@email.com");
                // context.Alunos.Add(aluno);
                // await context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }
    }
}

