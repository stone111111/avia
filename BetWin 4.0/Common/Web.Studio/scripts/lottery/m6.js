/// <reference path="iplayer.js" />
//  彩票游戏
if (!window["Lottery"]) window["Lottery"] = new Object();

if (!Lottery["M6"]) Lottery["M6"] = new Object();

// 六合彩的投注号码
Lottery.M6.Number = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49"];

// 六合彩生肖
Lottery.M6.Lunar = ["鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪"];

// 六合彩色波
Lottery.M6.Color = ["红", "蓝", "绿"];

Lottery.M6.Color_Name = ["red", "blue", "green"];

// 红波
Lottery.M6.Color_Red = ["01", "02", "07", "08", "12", "13", "18", "19", "23", "24", "29", "30", "34", "35", "40", "45", "46"];
// 绿波
Lottery.M6.Color_Blue = ["03", "04", "09", "10", "14", "15", "20", "25", "26", "31", "36", "37", "41", "42", "47", "48"];
// 蓝波
Lottery.M6.Color_Green = ["05", "06", "11", "16", "17", "21", "22", "27", "28", "32", "33", "38", "39", "43", "44", "49"];

// 获取数值对应的颜色
Lottery.M6.GetColor = function (num) {
    var color = null;
    [Lottery.M6.Color_Red, Lottery.M6.Color_Blue, Lottery.M6.Color_Green].each(function (item, index) {
        if (item.contains(num)) {
            color = Lottery.M6.Color_Name[index];
        }
    });
    return color;
};

// 确认金额的按钮
Lottery.M6.Button = function (input, item, ns) {
    var t = this;

    var value = input.get("value").toFloat();
    if (isNaN(value)) value = 0;

    if (value < 1 || Math.floor(value) != value) {
        input.set("value", Math.max(2, Math.round(value / 2) * 2));
        input.highlight();
        return;
    }

    var times = value / 2;
    var mode = "元";
    if (value % 2 != 0) {
        times = value * 10 / 2;
        mode = "角";
    }

    t.data.submit = new Object();
    t.data.submit.id = t.options.id;
    t.data.submit.number = item;
    t.data.submit.mode = mode;
    t.data.submit.times = times;
    t.data.submit.bet = 1;
    t.data.submit.money = value;
    t.selected();

    input.set("value", "");

    ns.Submit.apply(t);
};

// 特码
(function (ns) {

    ns.Submit = function () {
        var t = this;
        // 如果投注按钮不存在则直接投注
        if (!t.dom.submit && t.dom.quick.get("data-event")) {
            t.dom.quick.fireEvent(t.dom.quick.get("data-event"));
        }
    };
    // 特码
    ns["1"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": ns.Number };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            return 1;
        },
        // 页面渲染
        "draw": function () {
            var t = this;
            t.dom.container.empty();
            var ul = new Element("ul", {
                "class": "lottery-player-selector-ball-m6-player1"
            });
            ns.Number.each(function (item) {
                var li = new Element("li");
                new Element("span", {
                    "class": "ball num" + item + " color-" + ns.GetColor(item),
                    "text": item
                }).inject(li);

                var input = new Element("input", {
                    "type": "text",
                    "placeholder": "投注金额"
                });
                input.inject(li);

                new Element("a", {
                    "href": "javascript:",
                    "class": "btn btn-blue ft12",
                    "text": "确定",
                    "events": {
                        "click": function (e) {
                            ns.Button.apply(t, [input, item, ns]);

                        }
                    }
                }).inject(li);

                li.inject(ul);
            });

            ul.inject(t.dom.container);
        }
    });

})(Lottery.M6);

// 趣味
(function (ns) {

    // 生肖
    ns["2"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": ns.Lunar };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            return 1;
        },
        // 页面渲染
        "draw": function () {
            var t = this;
            t.dom.container.empty();
            var ul = new Element("ul", {
                "class": "lottery-player-selector-ball-m6-player1"
            });
            ns.Lunar.each(function (item) {
                var li = new Element("li");
                new Element("span", {
                    "class": "ball lunar",
                    "text": item
                }).inject(li);

                var input = new Element("input", {
                    "type": "text",
                    "placeholder": "投注金额"
                });
                input.inject(li);

                new Element("a", {
                    "href": "javascript:",
                    "class": "btn btn-blue ft12",
                    "text": "确定",
                    "events": {
                        "click": function (e) {
                            ns.Button.apply(t, [input, item, ns]);
                        }
                    }
                }).inject(li);

                li.inject(ul);
            });

            ul.inject(t.dom.container);
        }
    });

    // 两面
    ns["3"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": ["大", "小", "和大", "和小", "单", "双", "和单", "和双", "大肖", "小肖", "尾大", "尾小"] };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            return 1;
        },
        // 页面渲染
        "draw": function () {
            var t = this;
            t.dom.container.empty();
            var ul = new Element("ul", {
                "class": "lottery-player-selector-ball-m6-player1"
            });
            ["大", "小", "和大", "和小", "单", "双", "和单", "和双", "大肖", "小肖", "尾大", "尾小"].each(function (item) {
                var li = new Element("li");
                new Element("span", {
                    "class": "ball lunar",
                    "text": item
                }).inject(li);

                var input = new Element("input", {
                    "type": "text",
                    "placeholder": "投注金额"
                });
                input.inject(li);

                new Element("a", {
                    "href": "javascript:",
                    "class": "btn btn-blue ft12",
                    "text": "确定",
                    "events": {
                        "click": function (e) {
                            ns.Button.apply(t, [input, item, ns]);
                        }
                    }
                }).inject(li);

                li.inject(ul);
            });

            ul.inject(t.dom.container);
        }
    });

    // 色波
    ns["4"] = new Class({
        "Implements": [Events, Options, Lottery.IPlayer],
        "initialize": function (options) {
            var t = this;
            t.init(options);
            var item = { "label": null, "tool": false, "number": ns.Color };
            t.data.ball.push(item);
            t.draw();
        },
        // 获取当前投注的数量
        "getBet": function () {
            return 1;
        },
        // 页面渲染
        "draw": function () {
            var t = this;
            t.dom.container.empty();
            var ul = new Element("ul", {
                "class": "lottery-player-selector-ball-m6-player1"
            });
            ns.Color.each(function (item, index) {
                var li = new Element("li");
                new Element("span", {
                    "class": "ball color-" + Lottery.M6.Color_Name[index],
                    "text": item
                }).inject(li);

                var input = new Element("input", {
                    "type": "text",
                    "placeholder": "投注金额"
                });
                input.inject(li);

                new Element("a", {
                    "href": "javascript:",
                    "class": "btn btn-blue ft12",
                    "text": "确定",
                    "events": {
                        "click": function (e) {
                            ns.Button.apply(t, [input, item, ns]);
                        }
                    }
                }).inject(li);

                li.inject(ul);
            });

            ul.inject(t.dom.container);
        }
    });

})(Lottery.M6);