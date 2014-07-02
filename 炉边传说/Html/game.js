
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

var LastRequest;
var data;
var BattleInfo;

function onmessage(evt) {
    data = evt.data;
    if (!data) return;
    switch (LastRequest) {
        case RequestType.开始游戏:
            CreateGameResponse();
            break;
        case RequestType.传送套牌:
            SendDeckResponse();
            break;
        case RequestType.初始化状态:
            InitPlayInfoResponse();
            break;
        case RequestType.战场状态:
            BattleInfoResponse();
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
    初始化状态: "015"
};

function CreateGame() {
    LastRequest = RequestType.开始游戏;
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

function SendDeck() {
    LastRequest = RequestType.传送套牌;
    var strHost;
    if (IsHost) {
        strHost = strTrue;
    } else {
        strHost = strFalse;
    }
    var message = RequestType.传送套牌 + GameId + strHost + "M000017|M000018|M000021|M000003|M000024|M000026|M000027|M000035|M000037|M000047|M000043|M000041|M000040|M000059|M000058|M000057|M000054|M000061|M000064|M000065|M000067|M000076|M000077|M000082|M000088|M000087|M000085|M000084|M000068|M000076";
    socket.send(message);
}

function UserHandCard(CardSN) {
    LastRequest = RequestType.使用手牌;
    var message = RequestType.使用手牌 + GameId + strHost + CardSN;
    socket.send(message);
}

function SendDeckResponse() {
    //alert("传送套牌:[" + data.toString() + "]");
    LastRequest = RequestType.初始化状态;
    if (!IsHost) {
        var message = RequestType.初始化状态 + GameId;
        socket.send(message);
    }
}

function InitPlayInfoResponse() {
    //alert("初始化状态:[" + data.toString() + "]");
    LastRequest = RequestType.战场状态;
    if (IsHost) {
        strHost = strTrue;
    } else {
        strHost = strFalse;
    }
    var message = RequestType.战场状态 + GameId + strHost;
    socket.send(message);
}
//暂时不考虑验证
function BattleInfoResponse() {
    BattleInfo = JSON.parse(data);
    //for (var i = 0; i < BattleInfo.MyInfo.手牌.length; i++) {
    //    alert(BattleInfo.MyInfo.手牌[i]);
    //}
    var divHtml = "BattleInfo<br>";
    for (var i = 0; i < BattleInfo.HandCard.length; i++) {
        divHtml += "手牌:" + BattleInfo.HandCard[i].名称 + "<input type=\"button\" onclick=\"UserHandCard(\'" + BattleInfo.HandCard[i].序列号 + "\')\" value=\"使用\" />" + "<br>";
    }
    document.getElementById("BattleInfo").innerHTML = divHtml;
}