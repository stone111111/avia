    var instr = prompt("竞猜项数量?");
    var bet = new Array();
    var a = new Array(); //胜率


    for (var i = 0; i < instr; i++) {
        betstr = prompt("第" + (i + 1) + "个赔率？");
        bet[i] = betstr;
    }
    console.log("初期赔率:" + bet);

    var getpercent = function (x) {
        var value = 0;
        for (var i = 0; i < x.length; i++) {
            value += 1 / x[i];
        }
        return value;
    };

    var sum = function (list) {
        var value = 0;
        for (var i = 0; i < list.length; i++) {
            value += list[i];
        }
        return value;
    };

    var percent = (1 / getpercent(bet)).toFixed(2); //返奖率
    console.log("返奖率:" + percent);

    for (var i = 0; i < bet.length; i++) { //换算每项胜率
        a[i] = percent / bet[i];
    }

    var rand = Math.round(Math.random() * (bet.length - 1)); //随机投注项
    var newbet = Math.round(Math.random() * 4 + 1); //赔率变动值
    console.log("第" + (rand + 1) + "项,变化为:" + newbet);

    var begin = 1 - a[rand]; //初期未变更项占比
    var arr = new Array();
    
    for (var i = 0; i < bet.length; i++) {
        if (i != rand) {
            arr[i] = a[i] / begin;
        }
    }

    bet.splice(rand, 1, newbet);  //更新赔率

    var end = 1 - percent / bet[rand]; //变更后各项占比

    for (var i = 0; i < bet.length; i++) {
        if (i != rand) {
            bet.splice(i, 1, (percent / (end * arr[i])).toFixed(2)); //更新其它项赔率
        }
    }
    var newpercent = (1 / getpercent(bet)).toFixed(2);
    console.log("新返奖:" + newpercent);
    console.log(bet);