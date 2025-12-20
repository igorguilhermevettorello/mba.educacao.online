------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Query para obter pagamentos
select a.Nome,
       c.Descricao,
       c.Nivel,
       p.ValorTotal,
       p.DataCadastro,
       p.PedidoStatus,
       pg.Status,
       pg.Valor,
       pg.NomeCartao,
       pg.ExpiracaoCartao,
       pg.CvvCartao
  from Pedidos p
 inner join PedidoItens pi
         on pi.PedidoId = p.Id
 inner join Cursos c
         on c.Id = pi.CursoId
 inner join Alunos a
         on lower(a.Id) = lower(p.AlunoId)
  left join Pagamentos pg
         on pg.PedidoId = p.Id;
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Query para obter cursos de um pedido de matricula
select a.Nome,
       c.Id as CursoId,
       c.Descricao,
       c.Nivel,
       c.Valor,
       p.ValorTotal,
       p.DataCadastro,
       p.PedidoStatus
  from Pedidos p
 inner join PedidoItens pi
         on pi.PedidoId = p.Id
 inner join Cursos c
         on c.Id = pi.CursoId
 inner join Alunos a
         on lower(a.Id) = lower(p.AlunoId);
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Query para obter cursos de um pedido de matricula
select a.Id as AlunoId, a.Nome as aluno, c.Id as CursoId, c.Descricao as curso, al.Descricao as aula, h.*, m.ProgressoPercentual
  from Matriculas m
 inner join Alunos a
         on lower(a.Id) = lower(m.AlunoId)
 inner join Cursos c
         on c.Id = m.CursoId
 inner join Aulas al
         on al.CursoId = c.Id
  left join HistoricosAprendizado h
         on h.AulaId = al.Id
        and h.MatriculaId = m.Id
 where c.Id = :cursoId
   and a.Id = :alunoId;
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
select au.UserName as usuario, ar.Name as regra
  from AspNetUserRoles aur
  left join AspNetRoles ar
         on ar.Id = aur.RoleId
  left join AspNetUsers au
         on au.Id = aur.UserId;
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
