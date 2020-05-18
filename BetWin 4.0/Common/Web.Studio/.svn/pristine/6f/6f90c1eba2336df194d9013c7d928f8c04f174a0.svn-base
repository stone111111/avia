/// <reference path="iplayer.js" />
//彩票游戏
if (!window["Lottery"]) window["Lottery"] = new Object();

if (!Lottery["P10"]) Lottery["P10"] = new Object();

// PK10默认的投注号码
Lottery.P10.Number = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10"];

// 排名玩法
(function (ns) {

    // 排名 冠军
    ns["1"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);

            var item = { "label": "冠军", "tool": true, "number": ns.Number };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return Lottery.Utils.GetDirect(t.data.submit.number);
        }
    });

    //  排名 冠亚军
    ns["2"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["冠军", "亚军"].each(function (label) {
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

    //  排名 前三
    ns["3"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["冠军", "亚军", "季军"].each(function (label) {
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
            if (input.contains("")) return bet;
            input[0].split(",").each(function (n1) {
                input[1].split(",").filter(function (item) { return item != n1; }).each(function (n2) {
                    bet += input[2].split(',').filter(function (item) { return item != n1 && item != n2; }).length;
                });
            });
            return bet;
        }
    });

    // 定位胆 前五
    ns["4"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);

            ["第一名", "第二名", "第三名", "第四名", "第五名"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            var bet = 0;
            input.each(function (item) { bet += item.length; });
            return bet;
        }
    });

    // 定位胆 后五
    ns["5"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);

            ["第六名", "第七名", "第八名", "第九名", "第十名"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            var bet = 0;
            input.each(function (item) { bet += item.length; });
            return bet;
        }
    });

    // 名次 排名
    ns["101"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);

            ["1号", "2号", "3号", "4号", "5号", "6号", "7号", "8号", "9号", "10号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"] };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            var bet = 0;
            input.each(function (item) { bet += item.length; });
            return bet;
        }
    });

    // 前二 后二 单式
    ns["6"] = ns["112"] = Lottery.Utils.CreateSingle(2, {
        "repeat": false,
        "number": ns.Number
    });

    // 前三 后三 单式
    ns["7"] = ns["113"] = Lottery.Utils.CreateSingle(3, {
        "repeat": false,
        "number": ns.Number
    });

    // 前四 后四 单式
    ns["8"] = ns["114"] = Lottery.Utils.CreateSingle(4, {
        "repeat": false,
        "number": ns.Number
    });

    // 前五 后五 单式
    ns["9"] = ns["115"] = Lottery.Utils.CreateSingle(5, {
        "repeat": false,
        "number": ns.Number
    });

    // 前六 后六 单式
    ns["10"] = ns["116"] = Lottery.Utils.CreateSingle(6, {
        "repeat": false,
        "number": ns.Number
    });

    // 前七 后七 单式
    ns["111"] = ns["117"] = Lottery.Utils.CreateSingle(7, {
        "repeat": false,
        "number": ns.Number
    });

})(Lottery.P10);

// 趣味 大小单双
(function (ns) {

    // 大小
    ns["11"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);

            ["冠军", "亚军", "季军", "第四名", "第五名", "第六名", "第七名", "第八名", "第九名", "第十名"].each(function (label) {
                var item = { "label": label, "tool": false, "number": ["大", "小"], "class": "p33" };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            var bet = 0;
            input.each(function (item) { bet += item.length == 1 ? 1 : 0; });
            return bet;
        }
    });

    // 单双
    ns["12"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);

            ["冠军", "亚军", "季军", "第四名", "第五名", "第六名", "第七名", "第八名", "第九名", "第十名"].each(function (label) {
                var item = { "label": label, "tool": false, "number": ["单", "双"], "class": "p33" };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            var bet = 0;
            input.each(function (item) { bet += item.length == 1 ? 1 : 0; });
            return bet;
        }
    });

})(Lottery.P10);

// 趣味 两面
(function (ns) {

    ns["21"] = ns["22"] = ns["23"] = ns["24"] = ns["25"] = ns["26"] =
    ns["27"] = ns["28"] = ns["29"] = ns["30"] = ns["31"] =
        ns["32"] = new Class({
            "Implements": [Events, Options, Lottery.IPlayer],
            "initialize": function (options) {
                var t = this;
                t.init(options);
                t.data.ball.push({ "label": "大小", "tool": false, "number": ["大", "小"] });
                t.data.ball.push({ "label": "单双", "tool": false, "number": ["单", "双"] });
                t.draw();
            },
            // 获取当前投注的数量
            "getBet": function () {
                var t = this;
                t.getSubmit();
                var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
                var bet = 1;
                input.each(function (item) { bet *= item.length; });
                return bet;
            }
        });

})(Lottery.P10);

// 趣味 龙虎
(function (ns) {

    ns["41"] = ns["42"] = ns["43"] = ns["44"] = ns["45"] = ns["46"] =
    ns["47"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.data.ball.push({ "label": "龙虎", "tool": false, "number": ["龙", "虎"] });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return input[0].length == 1 ? 1 : 0;
        }
    });

})(Lottery.P10);

// 趣味 合值
(function (ns) {

    // 冠亚和
    ns["51"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.data.ball.push({ "label": "冠亚和", "tool": false, "number": ["3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19"] });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return input[0].length;
        }
    });

    // 单双
    ns["52"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.data.ball.push({ "label": "单双", "tool": false, "number": ["单", "双"] });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return input[0].length == 1 ? 1 : 0;
        }
    });

    // 大小
    ns["53"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.data.ball.push({ "label": "大小", "tool": false, "number": ["大", "小"] });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var input = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return input[0].length == 1 ? 1 : 0;
        }
    });

})(Lottery.P10);