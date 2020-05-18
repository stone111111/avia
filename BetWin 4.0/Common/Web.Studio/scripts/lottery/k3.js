/// <reference path="iplayer.js" />
//彩票游戏
if (!window["Lottery"]) window["Lottery"] = new Object();

if (!Lottery["K3"]) Lottery["K3"] = new Object();

// 快三默认的投注号码
Lottery.K3.Number = ["1", "2", "3", "4", "5", "6"];

//同号
(function (ns) {

    // 三同号
    ns["1"] = Lottery.Utils.CrateDicect(["三同号"], true, ns.Number);

    // 三同号 通选
    ns["2"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": ["通选"] };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return t.data.submit.number == "通选" ? 1 : 0;
        }
    });

    // 二同号 单选
    ns["3"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["二同号", "不同号"].each(function (item) {
                var item = { "label": item, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            var bet = 0;
            inputNumber[0].each(function (n1) {
                bet += inputNumber[1].filter(function (item) { return item != n1; }).length;
            });
            return bet;
        }
    });

    // 二同号 复选
    ns["4"] = Lottery.Utils.CrateDicect(["二同号"], true, ns.Number);

    // 二同号 通选
    ns["5"] = ns["2"];

})(Lottery.K3);

//不同号/连号
(function (ns) {

    // 三不同号
    ns["11"] = new Class({
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
            return Lottery.Utils.Combinations(3, t.data.submit.number.split(",").length);
        }
    });

    // 二不同号
    ns["12"] = new Class({
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
            return Lottery.Utils.Combinations(2, t.data.submit.number.split(",").length);
        }
    });

    // 三连号
    ns["13"] = ns["2"];

    // 三不同 通选
    ns["14"] = ns["2"];

})(Lottery.K3);

//趣味
(function (ns) {

    // 大小
    ns["21"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": ["大", "小"] };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return t.data.submit.number.split(",").length == 1 ? 1 : 0;
        }
    });

    // 单双
    ns["22"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": ["单", "双"] };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return t.data.submit.number.split(",").length == 1 ? 1 : 0;
        }
    });

    // 胆码
    ns["23"] = Lottery.Utils.CrateDicect(["胆码"], true, ns.Number);

    // 和值
    ns["24"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var num = [];
            for (var i = 3; i <= 18; i++) num.push(i);
            var item = { "label": null, "tool": false, "number": num };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var num = t.data.submit.number.split(",");
            return num.length;
        }
    });

    // 半顺
    ns["25"] = ns["2"];

    // 顺子
    ns["26"] = ns["2"];

    // 杂三
    ns["27"] = ns["2"];

})(Lottery.K3);