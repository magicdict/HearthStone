
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
            UserHandCardResponse();
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

function UserHandCardResponse() {
    if (IsHost) {
        strHost = strTrue;
    } else {
        strHost = strFalse;
    }
    Interrupt = JSON.parse(data);
    var Step;
    var SessionData;
    var SpellDecide;
    switch (Interrupt.ActionName) {
        case "OK":
            var message = RequestType.战场状态 + GameId + strHost;
            socket.send(message);
            break;
        case "MINIONPOSITION":
            alert("随从位置的选择:" + Interrupt.ExternalInfo)
            SessionData = Interrupt.SessionData + "MINIONPOSITION:2|";
            Step = "2";
            var message = RequestType.中断续行 + GameId + strHost + RequestType.使用手牌 + Step + ActiveCardSN + SessionData
            socket.send(message);
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
    var divHtml = "战场信息<br>";
    if (IsMyTurn) {
        divHtml += "本方回合<br>"
        document.getElementById("btnEndTurn").disabled = "";

    } else {
        divHtml += "对方回合<br>"
        document.getElementById("btnEndTurn").disabled = "disabled";
    }
    divHtml += "生命力：" + BattleInfo.MyInfo.生命力 + "护盾值：" + BattleInfo.MyInfo.护盾值;
    divHtml += "可用水晶：" + BattleInfo.MyInfo.可用水晶 + "总体水晶：" + BattleInfo.MyInfo.总体水晶 + "<br>";
    for (var i = 0; i < BattleInfo.HandCard.length; i++) {
        divHtml += "手牌:" + BattleInfo.HandCard[i].名称 + "<input type=\"button\" onclick=\"UserHandCard(\'" + BattleInfo.HandCard[i].序列号 + "\')\" value=\"使用\" />" + "<br>";
    }
    divHtml += "本方随从<br>"
    for (var i = 0; i < BattleInfo.MyBattle.length; i++) {
        divHtml += "随从:" + BattleInfo.MyBattle[i].状态列表 + "<br>";
    }

    divHtml += "生命力：" + BattleInfo.YourInfo.生命力 + "护盾值：" + BattleInfo.YourInfo.护盾值;
    divHtml += "可用水晶：" + BattleInfo.YourInfo.可用水晶 + "总体水晶：" + BattleInfo.YourInfo.总体水晶 + "<br>";
    divHtml += "对方随从<br>"
    for (var i = 0; i < BattleInfo.YourBattle.length; i++) {
        divHtml += "随从:" + BattleInfo.YourBattle[i].状态列表 + "<br>";
    }
    document.getElementById("BattleInfo").innerHTML = divHtml;
}