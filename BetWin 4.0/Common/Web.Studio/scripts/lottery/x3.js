/// <reference path="iplayer.js" />

//彩票游戏
if (!window["Lottery"]) window["Lottery"] = new Object();

if (!Lottery["X3"]) Lottery["X3"] = new Object();

// 3D默认的投注号码
Lottery.X3.Number = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

// 三码
(function (ns) {

    // 复式
    ns["21"] = Lottery.Utils.CrateDicect(["千位", "百位", "个位"]);

    //三星单式
    ns["22"] = Lottery.Utils.CreateSingle(3);

    //三星和值
    ns["23"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": Lottery.Utils.Array(0, 27) };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var num = [0, 0, 0];
            var bet = 0;
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number)[0].map(function (item) { return item.toInt(); });

            for (num[0] = 0; num[0] < 10; num[0]++) {
                for (num[1] = 0; num[1] < 10; num[1]++) {
                    for (num[2] = 0; num[2] < 10; num[2]++) {
                        if (inputNumber.contains(num[0] + num[1] + num[2])) bet++;
                    }
                }
            }
            return bet;
        }
    });

    //组三
    ns["24"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": true, "number": ns.Number };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            console.log(inputNumber);
            return Lottery.Utils.Combinations(2, inputNumber[0].length) * 2;
        }
    });

    //组六
    ns["25"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": true, "number": ns.Number };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return Lottery.Utils.Combinations(3, inputNumber[0].length);
        }
    });

})(Lottery.X3);

//二码
(function (ns) {
    // 复式
    ns["51"] = Lottery.Utils.CrateDicect(["百位", "十位"]);
    ns["61"] = Lottery.Utils.CrateDicect(["十位", "个位"]);

    // 单式
    ns["52"] = ns["62"] = Lottery.Utils.CreateSingle(2);

    // 和值
    ns["53"] = ns["63"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": Lottery.Utils.Array(0, 18) };
            t.data.ball.push(item);
            t.draw();
        },
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var num = [0, 0];
            var bet = 0;
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number)[0].map(function (item) { console.log(item); return item.toInt(); });

            for (num[0] = 0; num[0] < 10; num[0]++) {
                for (num[1] = 0; num[1] < 10; num[1]++) {
                    if (inputNumber.contains(num[0] + num[1])) bet++;
                }
            }
            return bet;
        }
    });

    // 组选 复式
    ns["54"] = ns["64"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": true, "number": ns.Number };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);

            return Lottery.Utils.Combinations(2, inputNumber[0].length);
        }
    });

    //组选 单式
    ns["55"] = ns["65"] = Lottery.Utils.CreateSingle(2, true);

    //组选 和值
    ns["56"] = ns["66"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": Lottery.Utils.Array(1, 17) };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var num = [0, 0];
            var bet = 0;
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number)[0].map(function (item) { return item.toInt(); });
            for (num[0] = 0; num[0] < 10; num[0]++) {
                for (num[1] = num[0] + 1; num[1] < 10; num[1]++) {
                    if (inputNumber.contains(num[0] + num[1])) bet++;
                }
            }
            return bet;
        }
    });

})(Lottery.X3);



// 胆码
(function (ns) {

    // 定位胆
    ns["71"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["百位", "十位", "个位"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            })
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            var bet = 0;
            inputNumber.each(function (item) {
                if (item.join("") == "") return;
                bet += item.length;
            })
            return bet;
        }
    });

    //一码
    ns["72"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": "不定胆", "tool": true, "number": ns.Number };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return Lottery.Utils.GetInputNumber(t.data.submit.number)[0].length;
        }
    });


    //二码
    ns["73"] =  new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": "不定胆", "tool": true, "number": ns.Number };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputLength = Lottery.Utils.GetInputNumber(t.data.submit.number)[0].length;
            return Lottery.Utils.Combinations(2, inputLength);
        }
    });

})(Lottery.X3);
