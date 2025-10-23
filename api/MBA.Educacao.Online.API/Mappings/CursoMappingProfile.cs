using AutoMapper;
using MBA.Educacao.Online.API.DTOs;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.ValueObjects;

namespace MBA.Educacao.Online.API.Mappings
{
    public class CursoMappingProfile : Profile
    {
        public CursoMappingProfile()
        {
            // Mapeamento de Curso para CursoDto
            CreateMap<Curso, CursoDto>();

            // Mapeamento de ConteudoProgramatico para ConteudoProgramaticoDto
            CreateMap<ConteudoProgramatico, ConteudoProgramaticoDto>();

            // Mapeamento de Aula para AulaDto
            CreateMap<Aula, AulaDto>();
        }
    }
}

