/// <reference path="../mootools.js" />
/// <reference path="../mootools-more.js" />
// 玩法的基类
//彩票游戏
if (!window["Lottery"]) window["Lottery"] = new Object();
// 模式对应的金额
var MODE = {
    "元": 2,
    "二元": 2,
    "一元": 1,
    "五角": 0.5,
    "角": 0.2,
    "二角": 0.2,
    "一角": 0.1,
    "五分": 0.05,
    "分": 0.02,
    "二分": 0.02,
    "一分": 0.01,
    "厘": 0.002,
    "二厘": 0.002,
    "一厘": 0.001,
    "毫": 0.0002,
    "一毫": 0.0001
};

/// 彩种视频
(function (ns) {
    if (ns["Video"]) return;
    ns["Video"] = {
        "PK10": {
            "url": "/wechat/game.html?Type=PK10",
            "width": 450,
            "height": 367
        },
        "HKSM": {
            "url": "/wechat/game.html?Type=HKSM",
            "width": 450,
            "height": 367
        },
        "VRVenus": {
            "url": "/studio/charts/video.html?Type=VRVenus",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true
        },
        "VRMars": {
            "url": "/studio/charts/video.html?Type=VRMars",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true
        },
        "VR3": {
            "url": "/studio/charts/video.html?Type=VR3",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true

        },
        "VRRacing": {
            "url": "/studio/charts/video.html?Type=VRRacing",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true

        },
        "VRBoat": {
            "url": "/studio/charts/video.html?Type=VRBoat",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true
        },
        "VRBaccarat": {
            "url": "/studio/charts/video.html?Type=VRBaccarat",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true
        },
        "VRRace": {
            "url": "/studio/charts/video.html?Type=VRRace",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true
        },
        "VRSwim": {
            "url": "/studio/charts/video.html?Type=VRSwim",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true
        },
        "VRBike": {
            "url": "/studio/charts/video.html?Type=VRBike",
            "width": 1024,
            "height": 620,
            "autoplay": true,
            "resize": true
        }
    };
})(Lottery);

// 玩法的基类
(function (ns) {

    if (ns["IPlayer"]) return;

    ns.IPlayer = new Class({
        "Implements": [Events, Options],
        "options": {
            // 绑定对象
            "bind": null,
            // 玩法编号
            "id": 0,
            // 玩法的名字
            "name": null,
            // 奖金
            "reward": 0,
            // 玩法的代码
            "code": null,
            // 单式的参数设定，为null表示不是单式
            "single": null,
            // 单式的分隔符
            "singleSplit": /\s+|\|+|\n+/,
            // 是否是任选
            "flags": false,
            // 帮助提示信息
            "tip": null,
            // 小于多少注为单挑
            "singlebet": 0,
            // 单挑奖金封顶
            "singlereward": 0
        },
        "dom": {
            // 选号区域
            "container": null,
            // 元角分厘模式选择
            "mode": null,
            // 投注数量
            "bet": null,
            // 投注总金额
            "money": null,
            // 倍数
            "times": null,
            // 快速投注按钮
            "quick": null,
            // 选号按钮
            "select": null,
            // 追号按钮
            "chase": null,
            // 提交投注按钮
            "submit": null,
            // 选中号码清单
            "selected": null,
            // 投注总金额
            "total": null,
            // 单式输入框
            "textarea": null,
            // 返点模式选择
            "rebate": null,
            // 返点区域范围
            "rebate-range": null,
            // 奖金提示
            "reward": null,
            // 合买按钮
            "united": null,
            // 任选区域
            "flags": null
        },
        "init": function (options) {
            var t = this;
            t.setOptions(options);
            if (t.options.singlebet) t.options.singlebet = t.options.singlebet.toInt();
            if (t.options.singlereward) t.options.singlereward = t.options.singlereward.toInt();
            var bind = t.options.bind;
            for (var key in t.dom) {
                var dom = bind.dom.container.getElement("[data-dom=" + key + "]");
                if (dom != null) t.dom[key] = dom;
            }
            t.showButton();
        },
        // 数据
        "data": {
            // 投注号码球 格式：{ "label" : "万位", "tool" : true, \"number\" : [0,1,2,3,4,5] }
            "ball": [],
            // 当前要提交的数据
            "submit": {
                // 玩法编号
                "id": 0,
                // 选中的号码
                "number": null,
                // 资金模式
                "mode": null,
                // 倍数
                "times": null,
                // 当前投注金额
                "money": 0,
                // 当前选中的注数
                "bet": 0,
                // 选中的返点模式
                "rebate": 0,
                // 当前玩法的中奖奖金
                "reward": 0,
                // 任选的位数
                "flags": null
            }
        },
        // 生成当前要提交的数据
        "getSubmit": function () {
            var t = this;
            t.data.submit = new Object();
            t.data.submit.id = t.options.id;
            if (t.options.single) {
                t.data.submit.number = t.getSubmitBySingle();
            } else {
                t.data.submit.number = t.dom.container.getElements(".item").map(function (item) {
                    return item.getElements(".ball > .current").map(function (num) { return num.get("data-value"); }).join(",");
                }).join("|");
            }

            if (t.options.flags) {
                t.data.submit.number = t.dom.flags.getElements(".current[data-flag]").map(function (item) { return item.get("data-flag"); }).join("") + "*" + t.data.submit.number;
            }
            t.data.submit.mode = t.dom.mode.getElement(".current").get("text");
            t.data.submit.times = t.dom.times.get("value").toInt();
            if (t.dom.rebate) {
                if (t.dom.rebate.hasClass("selected-half") && t.dom["rebate-range"]) {
                    t.data.submit.rebate = t.dom["rebate-range"].get("value");
                } else {
                    t.data.submit.rebate = t.dom.rebate.hasClass("selected") ? 1 : 0;
                }
            }
            t.data.submit.reward = t.options.reward * MODE[t.data.submit.mode] * t.data.submit.times / 2;
        },
        // 获取单式的内容
        "getSubmitBySingle": function () {
            var t = this;
            var value = t.dom.textarea.get("value");
            var data = new Object();
            var list = new Array();
            //if (value.contains("|")) {
            //    value = value.replace(/(\d{2,5})(,)/gi, "$1|");
            //}
            var regex = new RegExp(t.options.single.number.join("|"), "g");
            var result = value.match(regex);
            if (!result) return "";
            var item = new Array();
            for (var i = 0; i < result.length; i++) {
                item.push(result[i]);
                if (item.length == t.options.single.length) {
                    var itemValue = ns.Utils.GetSingleValue(item.join(","), t.options.single);
                    if (!data[itemValue]) {
                        data[itemValue] = true;
                        list.push(itemValue);
                    }
                    item = new Array();
                }
            }
            //value.split(t.options.singleSplit).each(function (item, index) {
            //    item = ns.Utils.GetSingleValue(item, t.options.single);
            //    if (item == null) return;
            //    if (data[item]) return;
            //    data[item] = true;
            //    list.push(item);
            //});
            if (value.length < 102400) {
                t.dom.textarea.set("value", list.join("|"));
            }
            return list.join("|");
        },
        // 根据当前条件显示按钮
        "showButton": function () {
            var t = this;
            if (!t.dom.selected) return;
            if (t.dom.selected.getElement("li")) {
                [t.dom.submit, t.dom.chase, t.dom.united].each(function (item) {
                    if (item) item.setStyle("display", "inline-block");
                });
                if (t.dom.quick)
                    t.dom.quick.setStyle("display", "none");
            } else {
                [t.dom.submit, t.dom.chase, t.dom.united].each(function (item) {
                    if (item) item.setStyle("display", "none");
                });
                if (t.dom.quick)
                    t.dom.quick.setStyle("display", "inline-block");
            }
        },
        // 绘制投注区域
        "draw": function () {
            var t = this;
            t.dom.container.empty();
            // 如果是任选
            if (t.options.flags) {
                t.dom.flags = new Element("div", {
                    "class": "lottery-player-selector-flags"
                });
                ["万", "千", "百", "十", "个"].each(function (item) {
                    new Element("a", {
                        "href": "javascript:",
                        "data-flag": item,
                        "text": item + "位",
                        "events": {
                            "click": function () {
                                this.toggleClass("current");
                                t.getMoney();
                            }
                        }
                    }).inject(t.dom.flags);
                });

                t.dom.flags.inject(t.dom.container);
            }
            if (t.options.single) {
                t.dom.textarea = new Element("textarea", {
                    "class": "single",
                    "placeholder": t.options.tip,
                    "events": {
                        "change": function () {
                            t.getMoney();
                        }
                    }
                }).inject(t.dom.container);
                if (t.options.flags) t.dom.textarea.addClass("single-flags");
            } else {
                t.data.ball.each(function (item) {
                    var obj = new Element("div", { "class": "item clear " + item["class"] });

                    if (!item.label && item.tool) {
                        var name = t.options.name;
                        if (name && name.contains(" ")) name = name.substr(name.lastIndexOf(" "));
                        item.label = name;
                    }
                    if (item.label) {
                        new Element("label", { "text": item.label }).inject(obj);
                    }
                    var code = t.options.code;
                    if (code != null && code.contains("_")) code = "code" + code.substr(code.indexOf("_"));
                    var ball = new Element("div", { "class": "ball " + code });
                    item.number.each(function (num) {
                        new Element("a", {
                            "href": "javascript:",
                            "data-value": num,
                            "text": num
                        }).inject(ball);
                    });
                    ball.inject(obj);

                    if (item.tool) {
                        var tool = new Element("div", { "class": "tool" });
                        ["全", "大", "小", "奇", "偶", "清"].each(function (t) {
                            new Element("a", {
                                "href": "javascript:",
                                "data-tool": t,
                                "text": t
                            }).inject(tool);
                        });
                        tool.inject(obj);
                    }

                    obj.inject(t.dom.container);
                });
            }
        },
        // 获取当前投注的金额
        "getMoney": function () {
            var t = this;
            var bet = t.getBet();
            var money = 0;
            if (bet != 0) {
                //bet *= t.data.submit.times;
                money = bet * MODE[t.data.submit.mode] * t.data.submit.times;
            }
            t.data.submit.bet = bet;
            t.data.submit.money = money;
            t.dom.bet.set("text", bet);
            t.dom.money.set("text", money.ToString("c"));
            if (t.dom.reward && t.data.submit.reward) {
                if (money == 0) {
                    t.dom.reward.empty();
                } else {
                    t.dom.reward.set("html", "最高返奖<label style=\"color:red;\">" + (t.data.submit.reward).ToString("c") + "</label>元");
                }
            }
        },
        // 选号
        "selected": function (submit) {
            var t = this;
            var submitdata = t.data.submit;

            var selectedNumber = function () {
                if (!t.dom.selected) return;
                var item = new Element("li");
                new Element("span", {
                    "class": "type",
                    "text": "[" + t.options.name + "]"
                }).inject(item);

                new Element("span", {
                    "class": "bet red",
                    "html": "[${mode}] ${bet}注 ${times}倍 ${money}元".toHtml(t.data.submit)
                }).inject(item);

                new Element("a", {
                    "href": "javascript:",
                    "class": "icon icon16 del",
                    "data-action": "delete"
                }).inject(item);
                var number = t.data.submit.number;
                if (number.length > 50) {
                    number = number.substring(0, 45) + "...";
                }
                new Element("div", {
                    "class": "number",
                    "html": "投注内容：" + number
                }).inject(item);

                item.inject(t.dom.selected);
                item.store("selected", t.data.submit);

                t.reset();
                t.getTotal();

                if (submit) submit();
            };

            console.log(t.options.singlebet + "," + t.options.singlereward + "," + t.options.singlebet + "," + submitdata.bet);
            if (t.options.singlebet && t.options.singlereward && t.options.singlebet >= submitdata.bet) {
                new BW.Tip("您当前投注小于" + t.options.singlebet + "注，为单挑模式，奖金封顶为" + t.options.singlereward + "元，确认提交吗？", {
                    "type": "confirm",
                    "callback": function () {
                        selectedNumber();
                    }
                });
            } else {
                selectedNumber();
            }
            return submitdata;
        },
        // 获取总金额
        "getTotal": function () {
            var t = this;
            var money = 0;
            if (!t.dom.selected) return money;
            t.dom.selected.getElements("li").each(function (item) {
                var data = item.retrieve("selected");
                money += data.money;
            });
            t.dom.total.set("text", money.ToString("c"));

            t.showButton();

            return money;
        },
        // 获取任选的组合数量 length：任选的长度
        "getFlags": function (length) {
            var t = this;
            var flags = t.dom.container.getElements(".lottery-player-selector-flags .current").length;
            console.log(flags);
            return Lottery.Utils.Combinations(length, flags);
        },
        // 重置选中的号码
        "reset": function () {
            var t = this;
            t.dom.container.getElements(".current[data-value]").each(function (item) {
                item.removeClass("current");
            });
            if (t.dom.textarea != null) t.dom.textarea.set("value", "");
            t.getMoney();
        },
        // 清空选中的号码
        "clear": function () {
            var t = this;
            t.dom.selected.getElements("li").dispose();
            t.getTotal();
        }
    });
})(Lottery);


// 玩法的工具类
(function (ns) {

    ns.Utils = {
        // 获取直选的数量
        "GetDirect": function (number) {
            var bet = 1;
            number.split("|").each(function (item) {
                if (item == "") bet = 0;
                if (bet == 0) return;
                bet *= item.split(",").distinct().length;
            });
            return bet;
        },
        // 创建一个单式对象
        "CreateSingle": function (length, singleOptions) {
            singleOptions = Object.merge({
                // 是否允许号码重复
                "repeat": true,
                // 号码范围
                "number": ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"],
                // 存在前后关系  即 01 和 10 属于不同的号码
                "around": true,
                // 单式的长度（必填）
                "length": length,
                // 是否是任选
                "flags": false,
                // 任选的长度
                "flagLength": 0
            }, singleOptions);


            //isGroup : 是否组选
            return new Class({
                "Implements": [Events, Options, Lottery.IPlayer],
                "initialize": function (options) {
                    var t = this;
                    options.single = singleOptions;
                    if (singleOptions.flags) options.flags = true;
                    t.init(options);
                    t.draw();
                },
                // 获取当前投注的数量
                "getBet": function () {
                    var t = this;
                    t.getSubmit();
                    if (t.options.flags) {
                        var number = t.data.submit.number.split("*")[1];
                        if (number == "") return 0;
                        var bet = number.split("|").length;
                        bet *= t.getFlags(singleOptions.flagLength);
                        return bet;
                    } else {
                        if (t.data.submit.number == "") return 0;
                        return t.data.submit.number.split("|").length;
                    }
                }
            });
        },
        // 创建一个复式的选择号码
        "CrateDicect": function (flag, tool, number) {
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
        },
        // 获取一个单式号码是否符合规则，不符合规则返回 null
        // number： 要检查的号码  options：检查参数
        "GetSingleValue": function (number, options) {
            var numbers = new RegExp(options.number.join("|"), "g");
            var num = new Array();
            var matchs = number.match(numbers);
            if (matchs == null) return null;
            if (matchs.length != options.length) return null;
            for (var i = 0; i < matchs.length; i++) {
                if (!options.repeat && num.contains(matchs[i])) {
                    return null;
                }
                num.push(matchs[i]);
            }
            if (!options.around) num = num.sort();

            return num.join(",");
        },
        // 阶乘
        "Factorial": function (num, start) {
            var value = 1;
            if (!start) start = 2;
            for (var i = start; i <= num; i++) value *= i;
            return value;
        },
        // 获取选中的号码
        "GetInputNumber": function (input) {
            return input.split("|").map(function (item) { return item == "" ? [] : item.split(","); });
        },
        // 排列组合数量 length : 已选择的长度 count：总长度
        "Combinations": function (length, count) {
            if (length > count) return 0;
            if (length == count) return 1;

            var t = ns.Utils;
            //C(n,m) = n * (n-1) * ... *(n-m+1) / m!
            return t.Factorial(count, count - length + 1) / t.Factorial(length);
        },
        // 生成区间数组
        "Array": function (start, end) {
            var list = new Array();
            for (var i = start; i <= end; i++) {
                list.push(i);
            }
            return list;
        }
    }

})(Lottery);

