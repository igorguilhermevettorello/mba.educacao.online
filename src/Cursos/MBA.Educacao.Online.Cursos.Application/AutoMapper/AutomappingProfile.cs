using AutoMapper;
using MBA.Educacao.Online.Cursos.Domain.DTOs.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Application.AutoMapper
{
    public class AutomappingProfile : Profile
    {
        public AutomappingProfile()
        {
            CreateMap<Curso, CursoCriarDto>();

        }
    }
}