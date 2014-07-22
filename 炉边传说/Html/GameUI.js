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
        button.setAttribute("x", i * 160);
        button.setAttribute("y", "0");
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
        button.setAttribute("x", i * 160);
        button.setAttribute("y", "175");
        (function (n) {
            button.onclick = function () {
                TargetPosDialog.dialog("close");
                AfterTargetPos(false, n + 1);
            }
        })(i);
        document.getElementById("TargetPanel").appendChild(button);
    }

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

        HandCard = document.getElementById("MyTargetPos" + (i + 1));
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

        HandCard = document.getElementById("YourTargetPos" + (i + 1));
        HandCard.getElementById("txtName").innerHTML = BattleInfo.YourBattle[i].名称;
        HandCard.getElementById("txtCost").innerHTML = BattleInfo.YourBattle[i].使用成本;
        HandCard.getElementById("txtAttackPoint").innerHTML = BattleInfo.YourBattle[i].攻击力;
        HandCard.getElementById("txtLifePoint").innerHTML = BattleInfo.YourBattle[i].生命值;
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
//目标对话框的UI初始化
function InitTargetDialog(TargetList) {
    var targets = TargetList.split("|");
    var CurPos;
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