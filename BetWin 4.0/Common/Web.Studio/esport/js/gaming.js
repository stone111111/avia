// 电竞的竞猜基础类库（适应页面的变化风格）

if (!window["ES"]) window["ES"] = new Object();

!function (ns) {
    // 遊戲對象
    ns["Obj"] = null;

    // 创建
    ns["Build"] = {
        "Match": function (item) {

        }
    };

    // 列表方式展示游戏列表
    ns["Game"] = new Class({
        Implements: [Events, Options],
        "options": {
            // 比赛列表的接口
            "matchlist": "/es/user/game/match",
            // 创建一个比赛的dom結構
            "buildMatch": function (item) {

            },
            // 指定的倒計時對象
            "countdown": null,
            // 自動更新數據的倒計時
            "matchTimer": 30
        },
        "dom": {
            // 賽事列表的外框對象
            "element": null,
            // 倒計時對象
            "countdown": null
        },
        "data": {
            "match": new Object(),
            // 當前的倒計時數據
            "timer": 30
        },
        // 加載比賽數據
        "loadMatch": function (data) {
            var t = this;
            if (!data) data = {};
            new Request.JSON({
                "url": t.options.matchlist,
                "onSuccess": function (result) {
                    if (!result.success) return;
                    result.info.each(function (item) {
                        var obj = t.options.buildMatch(item);
                        obj.inject(t.dom.element);
                    });
                }
            }).post(data);
        },
        // 倒計時
        "CountDown": function () {
            var t = this;
            if (t.dom.countdown) {
                t.dom.countdown.set("text", t.data.timer);
            } else {

            }
            t.data.timer--;
            if (t.data.timer < 0) t.data.timer = t.options.matchTimer;
            t.CountDown.delay(1000, t);
        },
        // 構造函數 el：外層dom框架 
        "initialize": function (el, options) {
            var t = this;
            t.dom.element = el = $(el);
            t.setOptions(options);
            t.dom.countdown = t.options.countdown;
            t.loadMatch();
        }
    });

}(ES);