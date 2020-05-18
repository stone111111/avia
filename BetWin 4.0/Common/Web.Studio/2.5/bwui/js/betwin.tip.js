// 弹出框  配合 css/betwin.tip.css 使用

!function (ns) {
    ns["Tip"] = new Class({
        "Implements": [Events, Options],
        "options": {
            // 弹窗类型 alert | confirm | tip
            "type": "alert",
            // 是否启用遮罩
            "mask": true,
            "target": $(document.body),
            "callback": function (e) {

            },
            // 自动关闭的时间
            "closetime": null
        },
        "dom": {
            "mask": null,
            "alert": null
        },
        "initialize": function (msg, options) {
            var t = this;
            t.setOptions(options);
            if (!t.options.target) t.options.target = $(document.body);
            t.dom.alert = new Element("div", {
                "class": "bw-tip hide bw-tip-" + t.options.type,
                "html": "<content>" + msg + "</content>"
            });
            t.dom.alert.inject(t.options.target);
            t.dom.alert.removeClass.delay(10, t.dom.alert, ["hide"]);

            var nav = new Element("nav", {
                "events": {
                    "click": function (e) {
                        var obj = $(e.target);
                        if (obj.get("tag") == "a") {
                            t.options.callback.delay(100, t, [{
                                "type": obj.get("class")
                            }]);
                            t.dispose();
                        }
                    }
                }
            });
            switch (t.options.type) {
                case "alert":
                    new Element("a", {
                        "class": "confirm",
                        "text": "确定"
                    }).inject(nav);
                    nav.inject(t.dom.alert);
                    break;
                case "tip":
                    if (!t.options.closetime) t.options.closetime = 3000;
                    t.dom.alert.addEvent("click", function () {
                        t.options.closetime = 0;
                        t.options.callback.apply(t);
                        t.dispose();
                    });
                    break;
                case "confirm":
                    new Element("a", {
                        "class": "confirm",
                        "text": "确定"
                    }).inject(nav);
                    new Element("a", {
                        "class": "cancel",
                        "text": "取消"
                    }).inject(nav);
                    nav.inject(t.dom.alert);
                    break;
            }

            if (t.options.mask) {
                t.dom.mask = new Element("div", {
                    "class": "tip-mask"
                });
                t.dom.mask.inject(t.dom.alert, "after");
            }

            if (t.options.closetime) {
                (function () {
                    if (!t.options.closetime) return;
                    t.options.callback.apply(t);
                    t.dispose.apply(t);
                }).delay(t.options.closetime);

            }
        },
        // 关闭弹窗
        "dispose": function () {
            var t = this;
            if (t.dom.mask) {
                t.dom.mask.addClass("hide");
                t.dom.mask.dispose.delay(250, t.dom.mask);
            }
            if (t.dom.alert) {
                t.dom.alert.addClass("hide");
                t.dom.alert.dispose.delay(250, t.dom.alert);
            }
        }
    });
}(BW);