/// <reference path="mootools.js" />
/// <reference path="mootools-more.js" />

if (!window["BW"]) window["BW"] = new Object();

(function (ns) {
    ns.diagIndex = 101;

    // 弹出窗口的状态缓存对象
    ns.diagObj = new Object();

    ns.Diag = new Class({
        Implements: [Events, Options],
        options: {
            // 弹出框的名字
            "name": null,
            // 自定义的样式
            "cssname": null,
            //加载类型  "control | frame | store
            "type": null,
            // 加载地址
            "src": null,
            // 能否关闭
            "close": true,
            // 标题栏
            "title": null,
            // 拖动
            "drag": false,
            // 宽度
            "width": 640,
            // 高度
            "height": 480,
            // 是否可以缩放
            "resize": false,
            //是否启用遮罩
            "mask": false,
            // 传递查询时候的数据
            "data": null,
            // 存入store的数据
            "store": null,
            // 加载完毕之后要执行的方法
            "callback": null,
            // 关闭窗口时执行的方法
            "closecallback": null
        },
        "dom": {
            // 当前的窗口对象
            "obj": null,
            // 遮罩层对象
            "mask": null,
            // 触发点击事件的元素，关闭之后将会产生一个缩小的动画
            "element": null
        },
        "initialize": function (el, options) {
            var t = this;
            if (el) t.dom.element = el = $(el);
            t.setOptions(options)
            if (t.options.name == null) {
                alert("未定义窗口名字");
                return;
            }
            if (ns.diagObj[t.options.name]) {
                ns.diagObj[t.options.name].target.shake();
                return;
            } else {
                ns.diagObj[t.options.name] = new Object();
            }
            ns.diagObj[t.options.name].target = t;

            t.dom.obj = new Element("div", {
                "class": "diag",
                "data-name": t.options.name,
                "styles": {
                    "margin-top": t.options.height / -2,
                    "margin-left": t.options.width / -2,
                    "left": ns.diagObj[t.options.name].left,
                    "top": ns.diagObj[t.options.name].top,
                    "width": t.options.width.toInt(),
                    "height": t.options.height.toInt(),
                    "zIndex": ns.diagIndex + 2
                }
            });
            t.dom.obj.inject(document.body);

            if (t.options.cssname) t.dom.obj.addClass(t.options.cssname);

            if (t.options.title) {
                new Element("div", {
                    "class": "diag-title",
                    "html": "<strong>" + t.options.title + "</strong>"
                }).inject(t.dom.obj);

                if (t.options.drag) {
                    new Drag(t.dom.obj, {
                        "handle": t.dom.obj.getElement(".diag-title")
                    });
                }
            }

            if (t.options.close) {
                new Element("a", {
                    "href": "javascript:",
                    "class": "diag-close",
                    "events": {
                        "click": function () {
                            t.Close();
                        }
                    }
                }).inject(t.dom.obj, "top");
            }

            if (t.options.resize) {
                new Element("a", {
                    "href": "javascript:",
                    "class": "diag-resize",
                    "events": {
                        "click": function () {
                            t.Resize(this);
                        }
                    }
                }).inject(t.dom.obj, "top");
            }

            var height = 0;
            if (t.options.title) {
                height += t.dom.obj.getElement(".diag-title").getSize().y;

            }

            var postData = null;
            if (t.options.data) {
                if (t.options.data == "parent" && el != null) {
                    var parent = el.getParent("[data-diag-data]");
                    if (parent != null) t.options.data = parent.get("data-diag-data");
                }
                postData = (typeof (t.options.data) == "string" ? t.options.data : Object.toQueryString(t.options.data));
            }
            var content = new Element("div", {
                "class": "diag-content",
                "data-bind-post": postData
            });
            content.inject(t.dom.obj);

            if (t.options.store) {
                content.store("data", t.options.store);
            }

            if (t.options.mask) {
                t.dom.mask = new Element("div", {
                    "class": "diag-mask",
                    "styles": {
                        "zIndex": ns.diagIndex + 1
                    }
                });
                if (t.options.cssname) t.dom.mask.addClass(t.options.cssname);
                t.dom.mask.inject(document.body);
            }

            if (t["bind-" + t.options.type]) {
                t["bind-" + t.options.type].apply(t, [content]);
            }

            ns.diagIndex += 2;
        },
        // 加载一个控件
        "bind-control": function (content) {
            var t = this;
            content.set({
                "data-bind-action": t.options.src,
                "data-bind-type": "control",
                "data-bind-callback": t.options.callback,
                "data-bind-load": t.options.load
            });
            new ns.BindEvent(content);
        },
        "bind-frame": function (content) {
            var t = this;
            var height = t.options.height;
            if (t.options.title) height -= t.dom.obj.getElement(".diag-title").getSize().y;
            new Element("iframe", {
                "frameborder": 0,
                "width": "100%",
                "height": height,
                "src": t.options.src
            }).inject(content);
        },
        "shake": function () {
            var t = this;
            var left = t.dom.obj.getStyle("left").toInt();
            var fx = new Fx.Tween(t.dom.obj, {
                "duration": 25,
                "link": "chain"
            });
            for (var i = 0; i < 10; i++) {
                fx.start("left", left + (i % 2 == 0 ? -1 : 1) * 10);
            }
        },
        // 改变窗口大小
        "Resize": function (obj) {
            var t = this;
            obj.toggleClass("diag-resize-min");
            var iframe = null;
            if (t.options.type == "frame") {
                iframe = t.dom.obj.getElement("iframe");
            }
            if (obj.hasClass("diag-resize-min")) {
                t.dom.obj.setStyles({
                    "width": t.options.width / 2,
                    "height": t.options.height / 2
                });
                if (iframe) iframe.set("height", t.options.height / 2 - 45);
            } else {
                t.dom.obj.setStyles({
                    "width": t.options.width,
                    "height": t.options.height
                });
                if (iframe) iframe.set("height", t.options.height - 45);
            }
        },
        // 关闭弹出的模式窗口
        "Close": function () {
            var t = this;
            if (t.dom.obj != null) {
                ns.diagObj[t.options.name].left = t.dom.obj.getStyle("left");
                ns.diagObj[t.options.name].top = t.dom.obj.getStyle("top");

                t.dom.obj.addClass("diag-dispose");
                if (t.dom.close) t.dom.close.dispose();
                if (t.dom.mask) t.dom.mask.dispose();

                if (t.options.closecallback) {
                    if (typeof (t.options.closecallback) == "function") {
                        t.options.closecallback.apply(t);
                    }
                }

                (function () {
                    t.dom.obj.dispose();
                    t.dom.obj = null;
                    delete ns.diagObj[t.options.name];
                    delete t;
                }).delay(300);
            }
        }
    });

})(BW);

!function () {
   // if (MooTools.mobile) return;
    window.addEvents({
        "click": function (e) {
            var obj = $(e.target);
            if (obj.get("data-diag-name")) {
                e.stop();
                new BW.Diag(obj, Element.GetAttribute(obj, "data-diag-"));
            }
            if (obj.hasClass("diag-user") && obj.get("data-userid") != null) {
                e.stop();

                var width = Math.min(900, UI.getSize().x * 0.8).toInt();
                var height = Math.min(600, UI.getSize().y * 0.8).toInt();

                new BW.Diag(obj, {
                    "name": "UserInfo",
                    "cssname": "diag-userinfo",
                    "type": "control",
                    "src": "/controls/userinfo.html",
                    "title": "查看用户资料",
                    "mask": true,
                    "width": width,
                    "height": height,
                    "drag": true,
                    "data": {
                        "UserID": obj.get("data-userid")
                    },
                    "callback": "tip,userinfo-callback"
                });
            }
        }
    });
}();