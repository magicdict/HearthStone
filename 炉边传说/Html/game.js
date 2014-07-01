
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

function onmessage(evt) {
    var data = evt.data;
    if (!data) return;
    if (LastRequest == RequestType.开始游戏) {
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
    }
}

var strTrue = "1";
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
    开始游戏: "014"
};

function CreateGame() {
    LastRequest = RequestType.开始游戏;
    socket.send(RequestType.开始游戏);
}