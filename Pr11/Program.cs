using System;
using Pr11.Services;

namespace Pr11
{
    class Program
    {
        static void Main()
        {
            var gameService = new GameService();
            gameService.Start();
        }
    }
}   