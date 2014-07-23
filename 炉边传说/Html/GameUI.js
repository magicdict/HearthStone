//新建游戏
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
    for (var i = 0; i < 6; i++) {
        var card = document.getElementById("BasicCard").cloneNode(true);
        card.setAttribute("id", "MyExistMinion" + (i + 1));
        card.setAttribute("display", "none");
        card.setAttribute("x", 40 + i * 160);
        card.setAttribute("y", "20");
        document.getElementById("MinionsPanel").appendChild(card);
    }
    for (var i = 0; i < 7; i++) {
        var button = document.getElementById("BasicButton").cloneNode(true);
        button.setAttribute("id", "MinPos" + (i + 1));
        button.setAttribute("display", "none");
        button.setAttribute("x", i * 160);
        button.setAttribute("y", "175");
        (function (n) {
            button.onclick = function () {
                MinionPosDialog.dialog("close");
                AfterPutMinionPos(n + 1);
            }
        })(i);
        document.getElementById("MinionsPanel").appendChild(button);
    }
    //目标选择
    for (var i = 0; i < 7; i++) {
        var button = document.getElementById("BasicCard").cloneNode(true);
        button.setAttribute("id", "MyTargetPos" + (i + 1));
        button.setAttribute("display", "none");
        button.setAttribute("x", (i + 1) * 160);
        button.setAttribute("y", "175");
        (function (n) {
            button.onclick = function () {
                TargetPosDialog.dialog("close");
                AfterTargetPos(true, n + 1);
            }
        })(i);
        document.getElementById("TargetPanel").appendChild(button);

        var button = document.getElementById("BasicCard").cloneNode(true);
        button.setAttribute("id", "YourTargetPos" + (i + 1));
        button.setAttribute("display", "none");
        button.setAttribute("x", (i + 1) * 160);
        button.setAttribute("y", "0");
        (function (n) {
            button.onclick = function () {
                TargetPosDialog.dialog("close");
                AfterTargetPos(false, n + 1);
            }
        })(i);
        document.getElementById("TargetPanel").appendChild(button);
    }


    //英雄
    var Hero = document.getElementById("BasicPlayerInfo").cloneNode(true);
    Hero.setAttribute("id", "MyHero");
    Hero.setAttribute("display", "");
    Hero.setAttribute("x", "475");
    Hero.setAttribute("y", "575");
    document.getElementById("gamePanel").appendChild(Hero);

    var Hero = document.getElementById("BasicPlayerInfo").cloneNode(true);
    Hero.setAttribute("id", "YourHero");
    Hero.setAttribute("display", "");
    Hero.setAttribute("x", "475");
    Hero.setAttribute("y", "25");
    document.getElementById("gamePanel").appendChild(Hero);

    //英雄 目标选择

    var Hero = document.getElementById("BasicPlayerInfo").getElementById("imgHero").cloneNode(true);
    Hero.setAttribute("id", "MyTargetPos0");
    Hero.setAttribute("display", "none");
    Hero.setAttribute("x", "0");
    Hero.setAttribute("y", "175");
    Hero.onclick = function () {
        TargetPosDialog.dialog("close");
        AfterTargetPos(true, 0);
    }
    document.getElementById("TargetPanel").appendChild(Hero);

    var Hero = document.getElementById("BasicPlayerInfo").getElementById("imgHero").cloneNode(true);
    Hero.setAttribute("id", "YourTargetPos0");
    Hero.setAttribute("display", "none");
    Hero.setAttribute("x", "0");
    Hero.setAttribute("y", "0");
    Hero.onclick = function () {
        TargetPosDialog.dialog("close");
        AfterTargetPos(true, 0);
    }
    document.getElementById("TargetPanel").appendChild(Hero);



    document.getElementById("btnCreateGame").disabled = "disabled";
    socket.send(RequestType.开始游戏);
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
        SetMinion(HandCard, BattleInfo.HandCard[i]);
        HandCard.getElementById("txtStatus").innerHTML = "";
        //必须使用闭包！和Lambda一样
        (function (n) {
            HandCard.onclick = function () {
                UserHandCard(BattleInfo.HandCard[n].序列号);
            }
        })(i);
    }

    for (var i = 0; i < 7; i++) {
        var MinionCard = document.getElementById("MyMinion" + (i + 1));
        MinionCard.setAttribute("display", "none");
    }

    for (var i = 0; i < BattleInfo.MyBattle.length; i++) {
        var MinionCard = document.getElementById("MyMinion" + (i + 1));
        SetMinion(MinionCard, BattleInfo.MyBattle[i]);
        //必须使用闭包！和Lambda一样
        (function (n) {
            MinionCard.onclick = function () {
                Fight("ME#" + (n + 1));
            }
        })(i);

        MinionCard = document.getElementById("MyExistMinion" + (i + 1));
        SetMinion(MinionCard, BattleInfo.MyBattle[i]);

        MinionCard = document.getElementById("MyTargetPos" + (i + 1));
        SetMinion(MinionCard, BattleInfo.MyBattle[i]);
    }

    for (var i = 0; i < 7; i++) {
        var MinionCard = document.getElementById("YourMinion" + (i + 1));
        MinionCard.setAttribute("display", "none");
    }
    for (var i = 0; i < BattleInfo.YourBattle.length; i++) {
        var MinionCard = document.getElementById("YourMinion" + (i + 1));
        SetMinion(MinionCard, BattleInfo.YourBattle[i]);
        MinionCard = document.getElementById("YourTargetPos" + (i + 1));
        SetMinion(MinionCard, BattleInfo.YourBattle[i]);
    }
    //如果这次的刷新是 战吼位置选择的前期准备，则接下来执行战吼的位置选择
    if (Interrupt.ActionName == "BATTLECRYPOSITION") {
        InitTargetDialog(Interrupt.ExternalInfo);
        Currentrequest = RequestType.使用手牌;
        Step = "4";
        SessionData = Interrupt.SessionData + "BATTLECRYPOSITION:";
        TargetPosDialog.dialog("open");
    }
}
//设定手牌外观
function SetMinion(MinionCard, Minion) {
    MinionCard.setAttribute("display", "");
    MinionCard.getElementById("txtName").innerHTML = Minion.名称;
    MinionCard.getElementById("txtCost").innerHTML = Minion.使用成本;
    MinionCard.getElementById("txtAttackPoint").innerHTML = Minion.攻击力;
    MinionCard.getElementById("txtLifePoint").innerHTML = Minion.生命值;
    MinionCard.getElementById("txtStatus").innerHTML = Minion.状态列表;
    if (Minion.描述.length > 20) {
        MinionCard.getElementById("txtDescirption1").innerHTML = Minion.描述.toString().substr(0, 10);
        MinionCard.getElementById("txtDescirption2").innerHTML = Minion.描述.toString().substr(10, 10);
        MinionCard.getElementById("txtDescirption3").innerHTML = Minion.描述.toString().substr(20);
    } else {
        if (Minion.描述.length > 10) {
            MinionCard.getElementById("txtDescirption1").innerHTML = Minion.描述.toString().substr(0, 10);
            MinionCard.getElementById("txtDescirption2").innerHTML = Minion.描述.toString().substr(10, 10);
            MinionCard.getElementById("txtDescirption3").innerHTML = "";
        } else {
            MinionCard.getElementById("txtDescirption1").innerHTML = Minion.描述;
            MinionCard.getElementById("txtDescirption2").innerHTML = "";
            MinionCard.getElementById("txtDescirption3").innerHTML = "";
        }
    }
}

//随从入场对话框的UI初始化
function InitPutMinionDialog() {

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
}

function InitSpellDecideDialog(DecideOpt) {
    var Options = DecideOpt.split("|");
    document.getElementById("txtAblitiy1").innerHTML = Options[0];
    document.getElementById("btnAblitiy1").onclick = function () {
        SpellDecideDialog.dialog("close");
        AfterSpellDecide(1);
    };
    document.getElementById("txtAblitiy2").innerHTML = Options[1];
    document.getElementById("btnAblitiy2").onclick = function () {
        SpellDecideDialog.dialog("close");
        AfterSpellDecide(2);
    };
}

//目标对话框的UI初始化
function InitTargetDialog(TargetList) {
    var targets = TargetList.split("|");
    var CurPos;

    var Hero = document.getElementById("MyTargetPos0");
    Hero.setAttribute("display", "none");
    for (var j = 0; j < targets.length; j++) {
        if (targets[j] == "ME#0") {
            Hero.setAttribute("display", "");
            break;
        }
    }

    Hero = document.getElementById("YourTargetPos0");
    Hero.setAttribute("display", "none");
    for (var j = 0; j < targets.length; j++) {
        if (targets[j] == "YOU#0") {
            Hero.setAttribute("display", "");
            break;
        }
    }

    for (var i = 0; i < 7; i++) {
        var HandCard = document.getElementById("MyTargetPos" + (i + 1));
        HandCard.setAttribute("display", "none");
    }
    for (var i = 0; i < BattleInfo.MyBattle.length; i++) {
        var HandCard = document.getElementById("MyTargetPos" + (i + 1));
        CurPos = "ME#" + (i + 1);
        for (var j = 0; j < targets.length; j++) {
            if (targets[j] == CurPos) {
                HandCard.setAttribute("display", "");
                break;
            }
        }
    }
    for (var i = 0; i < 7; i++) {
        var HandCard = document.getElementById("YourTargetPos" + (i + 1));
        HandCard.setAttribute("display", "none");
    }
    for (var i = 0; i < BattleInfo.YourBattle.length; i++) {
        var HandCard = document.getElementById("YourTargetPos" + (i + 1));
        CurPos = "YOU#" + (i + 1);
        for (var j = 0; j < targets.length; j++) {
            if (targets[j] == CurPos) {
                HandCard.setAttribute("display", "");
                break;
            }
        }
    }
}