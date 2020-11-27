using AutoMapper;
using BookApiService.Api.DTOs;
using BookApiService.Core.Enums;
using BookApiService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiService.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Book, ReadBookDto>()
                .ForMember(d => d.AgeCategorie, s => s.MapFrom(r => r.AgeCategorie.ToString()))
                .ForMember(d => d.Bookbinding, s => s.MapFrom(r => r.Bookbinding.ToString()))
                .ForMember(d => d.Genres, s => s.MapFrom(r => r.Genres.Select(g => g.ToString())));

            CreateMap<CreateBookDto, Book>()
                .ForMember(d => d.AgeCategorie, s => s.Ignore())
                .ForMember(d => d.Bookbinding, s => s.Ignore())
                .ForMember(d => d.Genres, s => s.Ignore())
                .ForMember(d => d.Id, s => s.Ignore())
                .AfterMap((x, y) =>
                {
                    if (Enum.TryParse(x.AgeCategorie, true, out AgeCategories newAgeCategorie))
                        y.AgeCategorie = newAgeCategorie;

                    if (Enum.TryParse(x.Bookbinding, true, out Bookbindings newBookbinding))
                        y.Bookbinding = newBookbinding;

                    var genres = new List<Genres>();
                    foreach (var genre in x.Genres)
                    {
                        if (Enum.TryParse(genre, true, out Genres newGenre))
                            genres.Add(newGenre);
                    }
                    y.Genres = genres.ToArray();
                });
        }
    }
}
