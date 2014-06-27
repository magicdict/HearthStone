﻿using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Server
{
    /// <summary>
    /// 服务器 - 客户端 通信协议
    /// </summary>
    public static class ServerResponse
    {
        /// <summary>
        /// 处理Request
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public static string ProcessRequest(String Request, RequestType requestType)
        {
            String Response = String.Empty;
            switch (requestType)
            {
                case RequestType.新建游戏:
                    //返回GameId
                    Response = GameServer.CreateNewGame(Request.Substring(3)).ToString(GameServer.GameIdFormat);
                    break;
                case RequestType.传送套牌:
                    Stack<String> Deck = new Stack<string>();
                    foreach (var card in Request.Substring(9).Split(CardUtility.strSplitArrayMark.ToCharArray()))
                    {
                        Deck.Push(card);
                    }
                    GameServer.SetCardStack(int.Parse(Request.Substring(3, 5)), Request.Substring(8, 1) == CardUtility.strTrue, Deck);
                    Response = CardUtility.strTrue;
                    break;
                case RequestType.等待游戏列表:
                    Response = GameServer.GetWaitGameList();
                    break;
                case RequestType.加入游戏:
                    Response = GameServer.JoinGame(int.Parse(Request.Substring(3, 5)), Request.Substring(8)).ToString();
                    break;
                case RequestType.游戏启动状态:
                    Response = GameServer.IsGameStart(int.Parse(Request.Substring(3, 5))).ToString();
                    break;
                case RequestType.先后手状态:
                    Response = GameServer.IsFirst(int.Parse(Request.Substring(3, 5)), Request.Substring(8, 1) == CardUtility.strTrue) ? CardUtility.strTrue : CardUtility.strFalse;
                    break;
                case RequestType.抽牌:
                    var Cardlist = GameServer.DrawCard(int.Parse(Request.Substring(3, 5)), Request.Substring(8, 1) == CardUtility.strTrue, int.Parse(Request.Substring(9, 1)));
                    Response = String.Join(Engine.Utility.CardUtility.strSplitArrayMark, Cardlist.ToArray());
                    break;
                case RequestType.回合结束:
                case RequestType.写入行动:
                    GameServer.WriteAction(int.Parse(Request.Substring(3, 5)), Request.Substring(8));
                    break;
                case RequestType.读取行动:
                    Response = GameServer.ReadAction(int.Parse(Request.Substring(3, 5)));
                    break;
                case RequestType.奥秘判定:
                    Response = GameServer.SecretHit(int.Parse(Request.Substring(3, 5)), Request.Substring(8, 1) == CardUtility.strTrue, Request.Substring(9));
                    break;
                case RequestType.使用手牌:
                    Response = GameServer.UseHandCard(int.Parse(Request.Substring(3, 5)), Request.Substring(8, 1) == CardUtility.strTrue, Request.Substring(9));
                    break;
                default:
                    break;
            }
            return Response;
        }
        /// <summary>
        /// 消息类型(3位)
        /// </summary>
        public enum RequestType
        {
            /// <summary>
            /// 新建一个游戏
            /// </summary>
            新建游戏,
            /// <summary>
            /// 传送套牌
            /// </summary>
            传送套牌,
            /// <summary>
            /// 获得等待中游戏列表
            /// </summary>
            等待游戏列表,
            /// <summary>
            /// 加入一个游戏
            /// </summary>
            加入游戏,
            /// <summary>
            /// 主机询问是否游戏已经启动
            /// </summary>
            游戏启动状态,
            /// <summary>
            /// 确认先后手状态
            /// </summary>
            先后手状态,
            /// <summary>
            /// 认输，退出一个游戏
            /// </summary>
            认输,
            /// <summary>
            /// 抽牌
            /// </summary>
            抽牌,
            /// <summary>
            /// 回合结束
            /// </summary>
            回合结束,
            /// <summary>
            /// 写入行动
            /// </summary>
            写入行动,
            /// <summary>
            /// 读取行动
            /// </summary>
            读取行动,
            /// <summary>
            /// 奥秘判定
            /// </summary>
            奥秘判定,
            /// <summary>
            /// 使用手牌
            /// </summary>
            使用手牌
        }
    }
}
