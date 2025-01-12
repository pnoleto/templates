using Application.DTO;
using Domain.Models;
using AutoMapper;

namespace Application.Mapper.Profiles
{
    public class ArticleProfile: Profile
    {
        public ArticleProfile() 
        {
            CreateMap<Article, Article>().ReverseMap();
        }  
    }
}
