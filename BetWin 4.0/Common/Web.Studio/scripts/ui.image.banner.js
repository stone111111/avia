/// <reference path="mootools.js" />
/*
仿FlashBanner的广告切换控件
el 控件的内容格式必须为 <a href=""><img src="" alg=""></a>
log : 20130126 允许图片不使用链接
*/
$import("UI.Image.Banner.css");

if (!window["UI"]) window["UI"] = new Object();

(function (ns) {
    if (ns.Image == undefined) ns.Image = new Object();

    ns.Image.Banner = new Class({
        Implements: [Events, Options],
        list: new Array(),
        index: 0,
        indexObj: null,
        tipObj: null,
        isOver: false,
        options: {
            width: 0,
            height: 0,
            timer: 3000,
            "duration": "long",
            "type": "number",
            "background": false
        },
        initialize: function (el, options) {
            var t = this;
            el = $(el);
            t.setOptions(options);
            el.setStyles({
                "width": t.options.width,
                "height": t.options.height
            });
            el.setStyles({
                "overflow": "hidden",
                "position": "relative"
            });

            var imgList = el.getElements("img");

            imgList.each(function (img) {
                var a = img.getParent("a");
                if (a == null) {
                    a = new Element("a", {
                        "href": "javascript:",
                        "title": img.get("alt")
                    });
                    img.inject(a);
                }

                a.setStyles({
                    "position": "absolute",
                    "opacity": 0,
                    "left": 0,
                    "height": 0,
                    "width": t.options.width,
                    "height": t.options.height
                });

                if (t.options.background) {
                    a.setStyle("background", "url('" + img.get("src") + "') no-repeat center center");
                    img.dispose();
                } else {
                    img.setStyles({
                        "width": t.options.width,
                        "height": t.options.height
                    });
                }

                a.set("tween", {
                    "duration": t.options.duration
                });
                t.list.push(a);
            });

            el.empty();

            t.list.each(function (item) {
                item.inject(el);
            });

            t.indexObj = new Element("div", {
                "class": "UI-Image-Banner-index UI-Image-Banner-" + t.options.type
            });
            t.indexObj.inject(el);

            for (var i = 0; i < t.list.length; i++) {
                new Element("a", {
                    "href": "javascript:",
                    "text": i + 1,
                    "events": {
                        "mouseover": function (e) {
                            t.Event(e, this);
                            t.isOver = true;
                        },
                        "mouseout": function () {
                            t.isOver = false;
                        }
                    }
                }).inject(t.indexObj);
            }

            t.tipObj = new Element("div", {
                "styles": {
                    "position": "absolute",
                    "width": "100%",
                    "left": 0,
                    "top": 0,
                    "padding": "2px",
                    "opacity": 0,
                    "color": "#FFFFFF",
                    "background-color": "#333",
                    "z-index": 2,
                    "text-align": "center"
                }
            });
            t.tipObj.inject(el, "top");
            switch (t.options.type) {
                case "line":
                    var left = ($$("body").getLast().getWidth() - 75 * t.indexObj.getElements("a").length) / 2;
                    t.indexObj.setStyles({
                        "right": "",
                        "left": left
                    });
                    t.indexObj.getElements("a").each(function (item) {
                        item.setStyles({
                            "line-height": "",
                            "padding": "",
                            "width": "70px",
                            "height": "4px",
                            "background-color": "#eeeeee",
                            "color": "transparent"
                        });
                    });
                    break;
            }

            t.Timer();


        },
        Event: function (e, obj) {
            if (!obj) return;
            var t = this;
            var index = obj.get("text").toInt() - 1;
            t.index = index;
            t.list.each(function (item) {
                item.setStyles({
                    //"opacity": 0,
                    "z-index": 0
                });
                t.list[t.index].tween("opacity", 0);
            });

            t.list[t.index].setStyle("z-index", 1);
            t.list[t.index].tween("opacity", 1);
            var img = t.list[t.index].getElement("img");
            if (img != null) {
                if (img.get("alt") == null || img.get("alt") == "") {
                    t.tipObj.fade("hide");
                } else {
                    t.tipObj.fade("0.6");
                    t.tipObj.set("text", img.get("alt"));
                }
            }
            switch (t.options.type) {
                case "number":
                case "line":
                    obj.getParent().getElements("a").each(function (item) {
                        if (item == obj) {
                            item.addClass("current");
                        } else {
                            item.removeClass("current");
                        }
                    });
                    break;
            }
        },
        Timer: function () {
            var t = this;
            if (t.options.timer == 0) return;
            var indexList = t.indexObj.getElements("a");
            var obj = indexList[t.index];
            if (!t.isOver) {
                t.Event(null, obj);
                t.index++;
            }
            (function () {
                if (t.index >= indexList.length) t.index = 0;
                t.Timer();
            }).delay(t.options.timer);
        }
    });
})(UI);