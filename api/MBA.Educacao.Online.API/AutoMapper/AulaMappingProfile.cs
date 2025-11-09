using AutoMapper;
using MBA.Educacao.Online.API.DTOs.Alunos;
using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.API.AutoMapper
{
    public class AulaMappingProfile : Profile
    {
        public AulaMappingProfile()
        {
            // Mapeamento de Aula para AulaCursoDto
            CreateMap<Aula, AulaCursoDto>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativa));
        }
    }
}

