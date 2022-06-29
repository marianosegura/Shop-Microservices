﻿using Basket.API.Entities;
using System.Threading.Tasks;

  
namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);  // one basket per user, created is an update
        Task DeleteBasket(string userName);
    }
}
