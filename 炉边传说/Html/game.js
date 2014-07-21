
var log = function (s) {
    if (document.readyState !== "complete") {
        log.buffer.push(s);
    } else {
        document.getElementById("output").innerHTML += (s + "\n");
    }
}

log.buffer = [];

var socket = null;
function init() {
    window.WebSocket = window.WebSocket || window.MozWebSocket;
    if (!window.WebSocket) {
        log("WebSocket not supported by this browser");
        return;
    }
    var webSocket = new WebSocket("ws://localhost:13001/");
    webSocket.onopen = onopen;
    webSocket.onclose = onclose;
    webSocket.onmessage = onmessage;

    socket = webSocket;
}

function onopen() {
    log("Open a web socket.");
}

function onclose() {
    log("Close a web socket.");
}

//全局变量
//游戏ID
var GameId = "00000";
//主机
var IsHost = false;
//先手
var IsFirst = false;
var data;
var ResponseCode;
var BattleInfo;
var IsMyTurn;
var Interrupt;
var ActiveCardSN;
var strHost;

function onmessage(evt) {
    data = evt.data;
    if (!data) return;
    ResponseCode = data.toString().substr(0, 3);
    data = data.toString().substr(3);
    switch (ResponseCode) {
        case RequestType.开始游戏:
            CreateGameResponse();
            break;
        case RequestType.回合结束:
            EndTrunResponse();
            break;
        case RequestType.传送套牌:
            SendDeckResponse();
            break;
        case RequestType.初始化状态:
            InitPlayInfoResponse();
            break;
        case RequestType.使用手牌:
            UseHandCardResponse();
            break;
        case RequestType.战场状态:
            BattleInfoResponse();
            break;
        case RequestType.中断续行:
            ResumeResponse();
            break;
    }
}

var strTrue = "1";
var strFalse = "0";
var RequestType = {
    新建游戏: "000",
    传送套牌: "001",
    等待游戏列表: "002",
    加入游戏: "003",
    游戏启动状态: "004",
    先后手状态: "005",
    认输: "006",
    抽牌: "007",
    回合结束: "008",
    写入行动: "009",
    读取行动: "010",
    奥秘判定: "011",
    使用手牌: "012",
    战场状态: "013",
    开始游戏: "014",
    初始化状态: "015",
    中断续行: "016"
};

function CreateGame() {
    //手牌区域
    for (var i = 0; i < 10; i++) {
        var card = document.getElementById("BasicCard").cloneNode(true);
        card.setAttribute("id", "HandCard" + (i + 1));
        card.setAttribute("display", "none");
        if (i % 2 == 0) {
            card.setAttribute("x", "1250");
            card.setAttribute("y", (i / 2) * 150);
        } else {
            card.setAttribute("x", "1400");
            card.setAttribute("y", ((i - 1) / 2) * 150);
        }
        document.getElementById("gamePanel").appendChild(card);
    }
    //本方对方随从区域
    for (var i = 0; i < 7; i++) {
        var card = document.getElementById("BasicCard").cloneNode(true);
        card.setAttribute("id", "MyMinion" + (i + 1));
        card.setAttribute("display", "");
        card.setAttribute("x", 50 + i * 160);
        card.setAttribute("y", "400");
        document.getElementById("gamePanel").appendChild(card);
    }
    for (var i = 0; i < 7; i++) {
        var card = document.getElementById("BasicCard").cloneNode(true);
        card.setAttribute("id", "YourMinion" + (i + 1));
        card.setAttribute("display", "");
        card.setAttribute("x", 50 + i * 160);
        card.setAttribute("y", "200");
        document.getElementById("gamePanel").appendChild(card);
    }
    //随从入场对话框
    //本方对方随从区域
    for (var i = 0; i < 6; i++) {
        var card = document.getElementById("BasicCard").cloneNode(true);
        card.setAttribute("id", "MyExistMinion" + (i + 1));
        card.setAttribute("display", "none");
        card.setAttribute("x", 40 + i * 160);
        card.setAttribute("y", "20");
        document.getElementById("MiniosPanel").appendChild(card);
    }
    for (var i = 0; i < 7; i++) {
        var button = document.getElementById("BasicButton").cloneNode(true);
        button.setAttribute("id", "MinPos" + (i + 1));
        button.setAttribute("display", "none");
        button.setAttribute("x", i * 160);
        button.setAttribute("y", "175");
        (function (n) {
            button.onclick = function () {
                dialog.dialog("close");
                AfterPutMinionPos(n + 1);
            }
        })(i);
        document.getElementById("MiniosPanel").appendChild(button);
    }

    document.getElementById("btnCreateGame").disabled = "disabled";
    socket.send(RequestType.开始游戏);
}

function CreateGameResponse() {
    GameId = data.toString().substr(0, 5);
    IsHost = data.toString().substr(5, 1) == strTrue;
    IsFirst = data.toString().substr(6, 1) == strTrue;
    var gameInfo = GameId;
    if (IsHost) {
        gameInfo = gameInfo + "Host";
    } else {
        gameInfo = gameInfo + "Guest";
    }
    if (IsFirst) {
        gameInfo = gameInfo + "First";
    } else {
        gameInfo = gameInfo + "Second";
    }
    document.getElementById("GameId").innerHTML = gameInfo;
    SendDeck();
}

function EndTrun() {
    IsMyTurn = false;
    var message = RequestType.回合结束 + GameId + strHost;
    socket.send(message);
}

function EndTrunResponse() {
    var IsHostEnd = data.toString().substr(0, 1) == strTrue;
    var message = RequestType.战场状态 + GameId + strHost;
    socket.send(message);
    if (IsHostEnd != IsHost) {
        IsMyTurn = true;
    }
}

function SendDeck() {
    if (IsHost) {
        strHost = strTrue;
    } else {
        strHost = strFalse;
    }
    var message = RequestType.传送套牌 + GameId + strHost + "M000017|M000018|M000021|M000003|M000024|M000026|M000027|M000035|M000037|M000047|M000043|M000041|M000040|M000059|M000058|M000057|M000054|M000061|M000064|M000065|M000067|M000076|M000077|M000082|M000088|M000087|M000085|M000084|M000068|M000076";
    socket.send(message);
}

function UserHandCard(CardSN) {
    ActiveCardSN = CardSN;
    var message = RequestType.使用手牌 + GameId + strHost + CardSN;
    socket.send(message);
}

function SendDeckResponse() {
    if (!IsHost) {
        var message = RequestType.初始化状态 + GameId;
        socket.send(message);
    }
}
function InitPlayInfoResponse() {
    if (IsHost) {
        strHost = strTrue;
    } else {
        strHost = strFalse;
    }
    var message = RequestType.战场状态 + GameId + strHost;
    socket.send(message);
    if (IsFirst) IsMyTurn = true;
}

var SessionData;
var Step;
var MinionPos;
function UseHandCardResponse() {
    if (IsHost) {
        strHost = strTrue;
    } else {
        strHost = strFalse;
    }
    Interrupt = JSON.parse(data);

    switch (Interrupt.ActionName) {
        case "OK":
            var message = RequestType.战场状态 + GameId + strHost;
            socket.send(message);
            break;
        case "MINIONPOSITION":
            //随从入场对话框的UI初始化
            for (var i = 0; i < 6 ; i++) {
                var HandCard = document.getElementById("MyExistMinion" + (i + 1));
                HandCard.setAttribute("display", "none");
            }
            for (var i = 0; i < 7; i++) {
                var HandCard = document.getElementById("MinPos" + (i + 1));
                HandCard.setAttribute("display", "none");
            }
            for (var i = 0; i < BattleInfo.MyBattle.length; i++) {
                var HandCard = document.getElementById("MyExistMinion" + (i + 1));
                HandCard.setAttribute("display", "");
            }
            for (var i = 0; i < BattleInfo.MyBattle.length + 1; i++) {
                var HandCard = document.getElementById("MinPos" + (i + 1));
                HandCard.setAttribute("display", "");
            }
            dialog.dialog("open");
            break;
        case "BATTLECRYPOSITION":
            alert("战吼施放对象的选择:" + Interrupt.ExternalInfo)
            SessionData = Interrupt.SessionData + "BATTLECRYPOSITION:ME#1|";

            Step = "4";
            var message = RequestType.中断续行 + GameId + strHost + RequestType.使用手牌 + Step + ActiveCardSN + SessionData;
            socket.send(message);
            break;
        case "SPELLPOSITION":
            alert("法术施放对象的选择:" + Interrupt.ExternalInfo)
            SessionData = Interrupt.SessionData + "SPELLPOSITION:YOU#1|";

            Step = "2";
            var message = RequestType.中断续行 + GameId + strHost + RequestType.使用手牌 + Step + ActiveCardSN + SessionData;
            socket.send(message);
            break;
        case "SPELLDECIDE":
            alert("法术卡牌抉择:" + Interrupt.ExternalInfo)
            SessionData = Interrupt.SessionData + "SPELLDECIDE:1|";

            Step = "2";
            var message = RequestType.中断续行 + GameId + strHost + RequestType.使用手牌 + Step + ActiveCardSN + SessionData;
            socket.send(message);
            break;
    }
}

//随从入场位置选择后
function AfterPutMinionPos(MinionPos) {
    SessionData = Interrupt.SessionData + "MINIONPOSITION:" + MinionPos + "|";
    Step = "2";
    var message = RequestType.中断续行 + GameId + strHost + RequestType.使用手牌 + Step + ActiveCardSN + SessionData
    socket.send(message);
}

function ResumeResponse() {
    Interrupt = JSON.parse(data);
    if (Interrupt.ActionName == "OK") {
        var message = RequestType.战场状态 + GameId + strHost;
        socket.send(message);
    }
}
//暂时不考虑验证
function BattleInfoResponse() {
    BattleInfo = JSON.parse(data);

    for (var i = 0; i < 10; i++) {
        var HandCard = document.getElementById("HandCard" + (i + 1));
        HandCard.setAttribute("display", "none");
    }
    for (var i = 0; i < BattleInfo.HandCard.length; i++) {
        var HandCard = document.getElementById("HandCard" + (i + 1));
        HandCard.setAttribute("display", "");
        HandCard.getElementById("txtName").innerHTML = BattleInfo.HandCard[i].名称;
        HandCard.getElementById("txtCost").innerHTML = BattleInfo.HandCard[i].使用成本;
        HandCard.getElementById("txtAttackPoint").innerHTML = BattleInfo.HandCard[i].攻击力;
        HandCard.getElementById("txtLifePoint").innerHTML = BattleInfo.HandCard[i].生命值;
        //必须使用闭包！和Lambda一样
        (function (n) {
            HandCard.onclick = function () {
                UserHandCard(BattleInfo.HandCard[n].序列号);
            }
        })(i);
    }

    for (var i = 0; i < 7; i++) {
        var HandCard = document.getElementById("MyMinion" + (i + 1));
        HandCard.setAttribute("display", "none");
    }
    for (var i = 0; i < BattleInfo.MyBattle.length; i++) {
        var HandCard = document.getElementById("MyMinion" + (i + 1));
        HandCard.setAttribute("display", "");
        HandCard.getElementById("txtName").innerHTML = BattleInfo.MyBattle[i].名称;
        HandCard.getElementById("txtCost").innerHTML = BattleInfo.MyBattle[i].使用成本;
        HandCard.getElementById("txtAttackPoint").innerHTML = BattleInfo.MyBattle[i].攻击力;
        HandCard.getElementById("txtLifePoint").innerHTML = BattleInfo.MyBattle[i].生命值;

        HandCard = document.getElementById("MyExistMinion" + (i + 1));
        HandCard.getElementById("txtName").innerHTML = BattleInfo.MyBattle[i].名称;
        HandCard.getElementById("txtCost").innerHTML = BattleInfo.MyBattle[i].使用成本;
        HandCard.getElementById("txtAttackPoint").innerHTML = BattleInfo.MyBattle[i].攻击力;
        HandCard.getElementById("txtLifePoint").innerHTML = BattleInfo.MyBattle[i].生命值;

    }

    for (var i = 0; i < 7; i++) {
        var HandCard = document.getElementById("YourMinion" + (i + 1));
        HandCard.setAttribute("display", "none");
    }
    for (var i = 0; i < BattleInfo.YourBattle.length; i++) {
        var HandCard = document.getElementById("YourMinion" + (i + 1));
        HandCard.setAttribute("display", "");
        HandCard.getElementById("txtName").innerHTML = BattleInfo.YourBattle[i].名称;
        HandCard.getElementById("txtCost").innerHTML = BattleInfo.YourBattle[i].使用成本;
        HandCard.getElementById("txtAttackPoint").innerHTML = BattleInfo.YourBattle[i].攻击力;
        HandCard.getElementById("txtLifePoint").innerHTML = BattleInfo.YourBattle[i].生命值;
    }
}