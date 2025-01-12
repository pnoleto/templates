﻿using Domain.Interfaces.Repositories.Base;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IArticleRepository : ISelectRepositoryBase<Article>, IRepositoryBase<Article>
    {
    }
}
