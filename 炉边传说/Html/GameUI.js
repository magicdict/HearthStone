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
    //动作对话框
    //被动作对象为随从
    var card = document.getElementById("BasicCard").cloneNode(true);
    card.setAttribute("id", "ActionMinion");
    card.setAttribute("x", "20");
    card.setAttribute("y", "20");
    document.getElementById("ActionPanel").appendChild(card);

    card = document.getElementById("BasicCard").cloneNode(true);
    card.setAttribute("id", "BeActionMinion");
    card.setAttribute("x", "200");
    card.setAttribute("y", "20");
    document.getElementById("ActionPanel").appendChild(card);


    card = document.getElementById("BasicPlayerInfo").getElementById("HeroInfo").cloneNode(true);
    card.setAttribute("id", "ActionHero");
    card.setAttribute("x", "20");
    card.setAttribute("y", "20");
    document.getElementById("ActionPanel").appendChild(card);

    card = document.getElementById("BasicPlayerInfo").getElementById("HeroInfo").cloneNode(true);
    card.setAttribute("id", "BeActionHero");
    card.setAttribute("x", "200");
    card.setAttribute("y", "20");
    document.getElementById("ActionPanel").appendChild(card);


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
        button.getElementById("txtButton").innerHTML = "放在这里";
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
                TargetDir = true;
                TargetPos = n + 1;
                TargetPosDialog.dialog("close");
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
                TargetDir = false;
                TargetPos = n + 1;
                TargetPosDialog.dialog("close");
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
    var Hero = document.getElementById("BasicPlayerInfo").getElementById("HeroInfo").cloneNode(true);
    Hero.setAttribute("id", "MyTargetPos0");
    Hero.setAttribute("display", "none");
    Hero.setAttribute("x", "0");
    Hero.setAttribute("y", "175");
    Hero.onclick = function () {
        TargetDir = true;
        TargetPos = 0;
        TargetPosDialog.dialog("close");
    }
    document.getElementById("TargetPanel").appendChild(Hero);

    var Hero = document.getElementById("BasicPlayerInfo").getElementById("HeroInfo").cloneNode(true);
    Hero.setAttribute("id", "YourTargetPos0");
    Hero.setAttribute("display", "none");
    Hero.setAttribute("x", "0");
    Hero.setAttribute("y", "0");
    Hero.onclick = function () {
        TargetDir = false;
        TargetPos = 0;
        TargetPosDialog.dialog("close");
    }
    document.getElementById("TargetPanel").appendChild(Hero);

    document.getElementById("btnCreateGame").style.display = "none";
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
                if (IsMyTurn) {
                    if (BattleInfo.MyInfo.可用水晶 >= BattleInfo.HandCard[n].使用成本) {
                        UserHandCard(BattleInfo.HandCard[n].序列号);
                    } else {
                        document.getElementById("txtMessage").innerHTML = "水晶不够";
                        MessageDialog.dialog("open");
                    }
                }
            }
        })(i);
        if (BattleInfo.MyInfo.可用水晶 >= BattleInfo.HandCard[i].使用成本 && IsMyTurn) {
            HandCard.getElementById("rctReadyToFight").setAttribute("fill", "lightgreen");
        } else {
            HandCard.getElementById("rctReadyToFight").setAttribute("fill", "pink");
        }
    }
    //本方随从
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
                if (BattleInfo.MyBattle[n].能否攻击) {
                    GetFightTargetList("ME#" + (n + 1));
                } else {
                    document.getElementById("txtMessage").innerHTML = "该随从需要休息或者已经攻击过了";
                    MessageDialog.dialog("open");
                }
            }
        })(i);
        if (BattleInfo.MyBattle[i].能否攻击) {
            MinionCard.getElementById("rctReadyToFight").setAttribute("fill", "lightgreen");
        } else {
            MinionCard.getElementById("rctReadyToFight").setAttribute("fill", "pink");
        }

        MinionCard = document.getElementById("MyExistMinion" + (i + 1));
        SetMinion(MinionCard, BattleInfo.MyBattle[i]);

        MinionCard = document.getElementById("MyTargetPos" + (i + 1));
        SetMinion(MinionCard, BattleInfo.MyBattle[i]);
    }
    //对方随从
    for (var i = 0; i < 7; i++) {
        var MinionCard = document.getElementById("YourMinion" + (i + 1));
        MinionCard.setAttribute("display", "none");
    }
    for (var i = 0; i < BattleInfo.YourBattle.length; i++) {
        var MinionCard = document.getElementById("YourMinion" + (i + 1));
        SetMinion(MinionCard, BattleInfo.YourBattle[i]);
        MinionCard = document.getElementById("YourTargetPos" + (i + 1));
        SetMinion(MinionCard, BattleInfo.YourBattle[i]);
        MinionCard.getElementById("rctReadyToFight").setAttribute("fill", "pink");
    }


    var HeroCard = document.getElementById("MyHero");
    SetHero(HeroCard, BattleInfo.MyInfo, true);
    HeroCard = document.getElementById("MyTargetPos0");
    SetHero(HeroCard, BattleInfo.MyInfo, true);

    HeroCard = document.getElementById("YourHero");
    SetHero(HeroCard, BattleInfo.YourInfo, false);
    HeroCard = document.getElementById("YourTargetPos0");
    SetHero(HeroCard, BattleInfo.YourInfo, false);

    HeroCard = document.getElementById("MyHero");
    SetCystal(HeroCard, BattleInfo.MyInfo);

    HeroCard = document.getElementById("YourHero");
    SetCystal(HeroCard, BattleInfo.YourInfo);

    //如果这次的刷新是 战吼位置选择的前期准备，则接下来执行战吼的位置选择
    if (Interrupt != undefined && Interrupt.ActionName == "BATTLECRYPOSITION") {
        InitTargetDialog(Interrupt.ExternalInfo);
        Currentrequest = RequestType.使用手牌;
        Step = "4";
        SessionData = Interrupt.SessionData + "BATTLECRYPOSITION:";
        TargetPosDialog.dialog("open");
    }

    //胜负判定
    if (BattleInfo.MyInfo.生命力 <= 0 && BattleInfo.YourInfo.生命力 <= 0) {
        document.getElementById("txtMessage").innerHTML = "双败";
        MessageDialog.dialog("open");
        document.getElementById("btnCreateGame").setAttribute("display", "");
        document.getElementById("btnEndTurn").setAttribute("display", "none");
        return;
    }
    if (BattleInfo.MyInfo.生命力 <= 0) {
        document.getElementById("txtMessage").innerHTML = "你输了";
        MessageDialog.dialog("open");
        document.getElementById("btnCreateGame").setAttribute("display", "");
        document.getElementById("btnEndTurn").setAttribute("display", "none");
        return;
    }
    if (BattleInfo.YourInfo.生命力 <= 0) {
        document.getElementById("txtMessage").innerHTML = "你赢了";
        MessageDialog.dialog("open");
        document.getElementById("btnCreateGame").setAttribute("display", "");
        document.getElementById("btnEndTurn").setAttribute("display", "none");
        return;
    }
}
//法力水晶
function SetCystal(HeroCard, Hero) {
    for (var i = 1; i < 10 + 1; i++) {
        HeroCard.getElementById("Cystal" + i).setAttribute("display", "none");
    }
    for (var i = 1; i < Hero.总体水晶 + 1; i++) {
        HeroCard.getElementById("Cystal" + i).setAttribute("display", "");
        HeroCard.getElementById("Cystal" + i).setAttribute("fill", "white");
    }
    for (var i = 1; i < Hero.可用水晶 + 1; i++) {
        HeroCard.getElementById("Cystal" + i).setAttribute("fill", "lightblue");
    }
    HeroCard.getElementById("txtCystal").innerHTML = Hero.可用水晶 + "/" + Hero.总体水晶;
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
//设置英雄外观
function SetHero(HeroCard, Hero, IsMyHero) {
    HeroCard.setAttribute("display", "");
    HeroCard.getElementById("txtHeroShieldPoint").innerHTML = Hero.护盾值;
    HeroCard.getElementById("txtHeroLifePoint").innerHTML = Hero.生命力;
    HeroCard.getElementById("txtHeroAttackPoint").innerHTML = Hero.攻击力;
    var isEnbale;
    if (IsMyTurn && IsMyHero && Hero.使用英雄技能) {
        isEnbale = true;
    }
    if ((!IsMyTurn) && (!IsMyHero) && Hero.使用英雄技能) {
        isEnbale = true;
    }

    if (HeroCard.getElementById("rctAbilityEnable") != null) {
        if (isEnbale) {
            HeroCard.getElementById("rctAbilityEnable").setAttribute("fill", "lightgreen");
            HeroCard.onclick = function () {
                UserHandCard(Hero.英雄技能);
            }
        } else {
            HeroCard.getElementById("rctAbilityEnable").setAttribute("fill", "pink");
            HeroCard.onclick = function () {
                document.getElementById("txtMessage").innerHTML = "您不能使用英雄技能";
                MessageDialog.dialog("open");
            }
        }
    }
}
//动作初始化
function InitActionDialog(YourPos, MyPos, ActionKbn) {
    if (ActionKbn == "Fight") {
        Message = "攻击方：";
        var Card;
        var Message;
        if (YourPos != 0) {
            card = document.getElementById("ActionHero");
            card.setAttribute("display", "none");

            card = document.getElementById("ActionMinion");
            SetMinion(card, BattleInfo.YourBattle[YourPos - 1]);
            Message = BattleInfo.YourBattle[YourPos - 1].名称;
        } else {
            card = document.getElementById("ActionMinion");
            card.setAttribute("display", "none");

            card = document.getElementById("ActionHero");
            SetHero(card, BattleInfo.YourInfo);
            Message = "[对方英雄]";
        }
        Message = "<br />被攻击方：";
        if (MyPos != 0) {
            card = document.getElementById("BeActionHero");
            card.setAttribute("display", "none");

            card = document.getElementById("BeActionMinion");
            SetMinion(card, BattleInfo.MyBattle[MyPos - 1]);
            Message = BattleInfo.MyBattle[MyPos - 1].名称;
        } else {
            card = document.getElementById("BeActionMinion");
            card.setAttribute("display", "none");

            card = document.getElementById("BeActionHero");
            SetHero(card, BattleInfo.MyInfo);
            Message = "[本方英雄]";
        }
        document.getElementById("txtActionMessage").innerHTML = Message;
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
//抉择的初始化
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

    TargetPos = -1;

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