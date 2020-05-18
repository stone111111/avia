/// <reference path="iplayer.js" />
//彩票游戏
if (!window["Lottery"]) window["Lottery"] = new Object();

if (!Lottery["X11"]) Lottery["X11"] = new Object();

Lottery.X11.Number = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11"];

//前码 + 不定位
(function (ns) {

    ns["1"] = new Class({
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
            return t.data.submit.number.split(",").length;
        }
    });

    // 前二码 复式
    ns["11"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["第一位", "第二位"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = t.data.submit.number.split("|");
            var bet = 0;
            if (input[0] == "" || input[1] == "") return bet;
            input[0].split(",").each(function (n1) {
                bet += input[1].split(",").filter(function (item) { return item != n1; }).length;
            });
            return bet;
        }
    });

    // 前二码 单式
    ns["12"] = Lottery.Utils.CreateSingle(2, {
        "repeat": false,
        "number": ns.Number
    });

    //前二码 组选
    ns["13"] = new Class({
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


    // 前三码 复式
    ns["31"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["第一位", "第二位", "第三位"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = t.data.submit.number.split("|");
            if (input.length != 3) return 0;
            var bet = 0;
            if (input[0] == "" || input[1] == "" || input[2] == "") return bet;
            input[0].split(",").each(function (n1) {
                input[1].split(",").each(function (n2) {
                    bet += input[2].split(",").filter(function (item) { return item != n1 && item != n2; }).length;
                });
            });
            return bet;
        }
    });

    // 前三码 单式
    ns["32"] = Lottery.Utils.CreateSingle(3, {
        "repeat": false,
        "number": ns.Number
    });

    //前三码 组选
    ns["33"] = new Class({
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

    // 前三码 组选单式
    ns["34"] = Lottery.Utils.CreateSingle(3, {
        "repeat": false,
        "number": ns.Number,
        "around": false
    });

    //不定位
    ns["41"] = new Class({
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
            return t.data.submit.number.split(",").length;
        }
    });

})(Lottery.X11);

// 任选
(function (ns) {
    // 任选的通用函数
    var _anySelected = function (length) {
        return new Class({
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
                return Lottery.Utils.Combinations(length, t.data.submit.number.split(",").length);
            }
        });
    }
    // 任选复式 一中一 ～ 八中五
    for (var i = 1; i < 9; i++) {
        ns["5" + i.toString()] = _anySelected(i);
    }

    // 任选单式 二中二～八中五
    for (var i = 2; i < 9; i++) {
        ns["6" + i.toString()] = Lottery.Utils.CreateSingle(i, {
            "repeat": false,
            "number": ns.Number,
            "around": false
        });
    }
})(Lottery.X11);

// 趣味
(function (ns) {

    // 定单双
    ns["71"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": true, "number": ["5单0双", "4单1双", "3单2双", "2单3双", "1单4双", "0单5双"] };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return t.data.submit.number.split(",").length;
        }
    });

    // 猜中位
    ns["72"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": true, "number": ["03", "04", "05", "06", "07", "08", "09"] };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return t.data.submit.number.split(",").length;
        }
    });

})(Lottery.X11);