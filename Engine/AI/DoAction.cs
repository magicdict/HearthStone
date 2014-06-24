using Engine.Client;
using Engine.Server;
using System;
using System.Collections.Generic;

namespace Engine.AI
{
    public static class DoAction
    {
        public static List<String> Run()
        {
            List<String> Result = new List<string>();
            foreach (var item in GameManager.gameStatus.client.YourSelfInfo.handCards)
            {
                System.Diagnostics.Debug.WriteLine(item.名称);                
            }
            Result.Add(ActionCode.strEndTurn);
            return Result;
        }
    }
}
