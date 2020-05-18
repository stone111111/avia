/// <reference path="iplayer.js" />
//彩票游戏
if (!window["Lottery"]) window["Lottery"] = new Object();

if (!Lottery["X5"]) Lottery["X5"] = new Object();

// 时时彩默认的投注号码
Lottery.X5.Number = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

Lottery.X5._direct = function (flag, tool, number) {
    if (tool == undefined) tool = true;
    if (number == undefined) number = Lottery.X5.Number;
    return new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            flag.each(function (label) {
                var item = { "label": label, "tool": tool, "number": number };
                t.data.ball.push(item);
            });

            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            return Lottery.Utils.GetDirect(t.data.submit.number);
        }
    });
};

// 五星
(function (ns) {

    // 五星复式
    ns["1"] = ns._direct(["万位", "千位", "百位", "十位", "个位"]);

    // 五星单式
    ns["2"] = Lottery.Utils.CreateSingle(5);

    // 组选_组选120
    ns["3"] = new Class({
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
            return Lottery.Utils.Combinations(5, t.data.submit.number.split(",").length);
        }
    });

    // 组选_组选60
    ns["4"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["二重号", "单号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            if (inputNumber.length != 2) return 0;
            var bet = 0;
            inputNumber[0].each(function (n1) {
                bet += Lottery.Utils.Combinations(3, inputNumber[1].filter(function (item) { return item != n1; }).length);
            });
            return bet;
        }
    });

    // 组选_组选30
    ns["5"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["二重号", "单号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            if (inputNumber.length != 2) return 0;
            var bet = 0;
            inputNumber[1].each(function (n1) {
                bet += Lottery.Utils.Combinations(2, inputNumber[0].filter(function (item) { return item != n1; }).length);
            });
            return bet;
        }
    });

    // 组选_组选20
    ns["6"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["三重号", "单号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            if (inputNumber.length != 2) return 0;
            var bet = 0;
            inputNumber[0].each(function (n1) {
                bet += Lottery.Utils.Combinations(2, inputNumber[1].filter(function (item) { return item != n1; }).length);
            });
            return bet;
        }
    });

    // 组选_组选10
    ns["7"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["三重号", "二重号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            if (inputNumber.length != 2) return 0;
            var bet = 0;
            inputNumber[0].each(function (n1) {
                bet += Lottery.Utils.Combinations(1, inputNumber[1].filter(function (item) { return item != n1; }).length);
            });
            return bet;
        }
    });


    // 组选_组选5
    ns["8"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["四重号", "单号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            if (inputNumber.length != 2) return 0;
            var bet = 0;
            inputNumber[0].each(function (n1) {
                bet += Lottery.Utils.Combinations(1, inputNumber[1].filter(function (item) { return item != n1; }).length);
            });
            return bet;
        }
    });

})(Lottery.X5);

// 四星
(function (ns) {
    // 四星复式
    ns["11"] =  ns._direct(["千位", "百位", "十位", "个位"]);
    ns["111"] = ns._direct(["万位", "千位", "百位", "十位"]);

    // 四星单式
    ns["12"] = ns["112"] = Lottery.Utils.CreateSingle(4);

    // 组选_组选24
    ns["13"] = ns["113"] = new Class({
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
            return Lottery.Utils.Combinations(4, t.data.submit.number.split(",").length);
        }
    });


    // 组选_组选12
    ns["14"] = ns["114"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["二重号", "单号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            if (inputNumber.length != 2) return 0;
            var bet = 0;
            inputNumber[0].each(function (n1) {
                bet += Lottery.Utils.Combinations(2, inputNumber[1].filter(function (item) { return item != n1; }).length);
            });
            return bet;
        }
    });

    // 组选_组选6
    ns["15"] = ns["115"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": "二重号", "tool": true, "number": ns.Number };
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

    // 组选_组选4
    ns["16"] = ns["116"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["三重号", "单号"].each(function (label) {
                var item = { "label": label, "tool": true, "number": ns.Number };
                t.data.ball.push(item);
            });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            if (inputNumber.length != 2) return 0;
            var bet = 0;
            inputNumber[0].each(function (n1) {
                bet += Lottery.Utils.Combinations(1, inputNumber[1].filter(function (item) { return item != n1; }).length);
            });
            return bet;
        }
    });

})(Lottery.X5);

// 三码（前三、中三、后三）
(function (ns) {

    // 复式
    var _direct = function (flag) {
        return new Class({
            "Implements": [Events, Options, Lottery.IPlayer],
            "initialize": function (options) {
                var t = this;
                t.init(options);
                flag.each(function (label) {
                    var item = { "label": label, "tool": true, "number": ns.Number };
                    t.data.ball.push(item);
                });

                t.draw();
            },
            // 获取当前投注的数量
            "getBet": function () {
                var t = this;
                t.getSubmit();
                return Lottery.Utils.GetDirect(t.data.submit.number);
            }
        });
    }
    ns["21"] = _direct(["万位", "千位", "百位"]);
    ns["31"] = _direct(["千位", "百位", "十位"]);
    ns["41"] = _direct(["百位", "十位", "个位"]);

    //三星单式
    ns["22"] = ns["32"] = ns["42"] = Lottery.Utils.CreateSingle(3);

    //三星和值
    ns["23"] = ns["33"] = ns["43"] = new Class({
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
    ns["24"] = ns["34"] = ns["44"] = new Class({
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
            return Lottery.Utils.Combinations(2, inputNumber[0].length) * 2;
        }
    });

    //组六
    ns["25"] = ns["35"] = ns["45"] = new Class({
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

    //混选
    ns["26"] = ns["36"] = ns["46"] = Lottery.Utils.CreateSingle(3, {
        "around": false
    });

})(Lottery.X5);

//二码
(function (ns) {
    // 复式
    ns["51"] = Lottery.Utils.CrateDicect(["万位", "千位"]);
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

})(Lottery.X5);

// 胆码
(function (ns) {

    // 定位胆
    ns["71"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["万位", "千位", "百位", "十位", "个位"].each(function (label) {
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

    //后三一码 前三一码 不定一码
    ns["72"] = ns["74"] = ns["76"] = new Class({
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


    //后三二码 前三二码 不定二码
    ns["73"] = ns["75"] = ns["77"] = new Class({
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

    // 不定三码
    ns["78"] = new Class({
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
            return Lottery.Utils.Combinations(3, inputLength);
        }
    });

})(Lottery.X5);

// 趣味
(function (ns) {
    // 大小单双 前二
    ns["81"] = ns._direct(["万位", "千位"], false, ["大", "小", "单", "双"]);
    // 大小单双 后二
    ns["82"] = ns._direct(["十位", "个位"], false, ["大", "小", "单", "双"]);
    //大小单双 合值
    ns["181"] = ns._direct(["合值"], false, ["大", "小", "单", "双"]);

    // 大小单双 定位胆
    ns["182"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            ["万位", "千位", "百位", "十位", "个位"].each(function (label) {
                var item = { "label": label, "tool": false, "number": ["大", "小", "单", "双"] };
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

    // 一帆风顺 好事成双 三星报喜 四季发财
    ns["83"] = ns["84"] = ns["85"] = ns["86"] = new Class({
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
            return inputNumber[0].length;
        }
    });

    ns["183"] = ns["189"] = ns["190"] = ns["191"] = ns["192"] = ns["193"] = ns["194"] =
        ns["195"] = ns["196"] = ns["197"] = ns._direct(["龙虎"], false, ["龙", "虎", "和"]);

    ns["184"] = ns._direct(["豹子"], false, ["前", "中", "后"]);
    ns["185"] = ns._direct(["顺子"], false, ["前", "中", "后"]);
    ns["186"] = ns._direct(["对子"], false, ["前", "中", "后"]);
    ns["187"] = ns._direct(["半顺"], false, ["前", "中", "后"]);
    ns["188"] = ns._direct(["杂六"], false, ["前", "中", "后"]);

})(Lottery.X5);

// 任选
(function (ns) {

    // 任二直选
    ns["91"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.options.flags = true;
            t.data.ball.push({ "label": "第一位", "tool": true, "number": ns.Number });
            t.data.ball.push({ "label": "第二位", "tool": true, "number": ns.Number });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return inputNumber[0].length * inputNumber[1].length * t.getFlags(2);
        }
    });

    // 任二组选
    ns["92"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.options.flags = true;
            t.data.ball.push({ "label": "组选", "tool": true, "number": ns.Number });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return Lottery.Utils.Combinations(2, inputNumber[0].length) * t.getFlags(2);
        }
    });

    // 任二单式
    ns["93"] = Lottery.Utils.CreateSingle(2, {
        "flags": true,
        "flagLength": 2
    });

    // 任三直选
    ns["94"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.options.flags = true;
            t.data.ball.push({ "label": "第一位", "tool": true, "number": ns.Number });
            t.data.ball.push({ "label": "第二位", "tool": true, "number": ns.Number });
            t.data.ball.push({ "label": "第三位", "tool": true, "number": ns.Number });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return inputNumber[0].length * inputNumber[1].length * inputNumber[2].length * t.getFlags(3);
        }
    });

    // 任三组三
    ns["95"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.options.flags = true;
            t.data.ball.push({ "label": "组选", "tool": true, "number": ns.Number });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return Lottery.Utils.Combinations(2, inputNumber[0].length) * t.getFlags(3) * 2;
        }
    });

    // 任三组六
    ns["96"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.options.flags = true;
            t.data.ball.push({ "label": "组选", "tool": true, "number": ns.Number });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return Lottery.Utils.Combinations(3, inputNumber[0].length) * t.getFlags(3);
        }
    });

    // 任三单式
    ns["97"] = Lottery.Utils.CreateSingle(3, {
        "flags": true,
        "flagLength": 3
    });


    // 任三直选
    ns["98"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            t.options.flags = true;
            t.data.ball.push({ "label": "第一位", "tool": true, "number": ns.Number });
            t.data.ball.push({ "label": "第二位", "tool": true, "number": ns.Number });
            t.data.ball.push({ "label": "第三位", "tool": true, "number": ns.Number });
            t.data.ball.push({ "label": "第四位", "tool": true, "number": ns.Number });
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            var t = this;
            t.getSubmit();
            var inputNumber = Lottery.Utils.GetInputNumber(t.data.submit.number);
            return inputNumber[0].length * inputNumber[1].length * inputNumber[2].length * inputNumber[3].length * t.getFlags(4);
        }
    });

    // 任三混选
    ns["100"] = Lottery.Utils.CreateSingle(3, {
        "flags": true,
        "flagLength": 3,
        "around": false
    });

    // 任四单式
    ns["99"] = Lottery.Utils.CreateSingle(4, {
        "flags": true,
        "flagLength": 4
    });

})(Lottery.X5);