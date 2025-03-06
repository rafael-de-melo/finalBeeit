﻿using finalBeeit.Models;

namespace finalBeeit.Services
{
    public interface ISteamApiService
    {
        public Task<List<Game>> SearchGameByName(string name);
    }
}
