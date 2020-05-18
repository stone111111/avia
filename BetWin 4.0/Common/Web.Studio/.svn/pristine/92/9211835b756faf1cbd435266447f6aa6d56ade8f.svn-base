if (!window["BW"]) window["BW"] = new Object();

// 框架
!function (ns) {

    var STORE_KEY = "FRAMES";

    ns.Frame = new Class({
        Implements: [Events, Options],
        "options": {
            // 外层元素
            "container": null,
            // 要显示的数量
            "count": 0,
            // 关闭页面的回调事件
            "unload": function () { }
        },
        "dom": {
            "container": null,
            "frames": function () {
                var t = this;
                var container = typeof (t.dom.container) == "function" ? t.dom.container() : t.dom.container;
                var frame = container.getElement(".bw-frames");
                if (!frame) {
                    frame = new Element("div", {
                        "class": "bw-frames"
                    });
                    frame.inject(container);
                }
                return frame;
            }
        },
        "data": {
            // 已经打开的窗口
            "frame": [],//[{name:obj}],
            // 根据名字查找窗体
            "getFrame": function (name) {
                var t = this;
                var obj = null;
                t.data.frame.each(function (item) {
                    if (obj) return;
                    if (item["name"] == name) obj = item.obj;
                });
                return obj;
            },
            // 打开一个窗口
            "add": function (name, obj) {
                var t = this;
                if (t.options.count == 1 && t.data.frame.length != 0) {
                    t.data.frame[0].obj.dispose();
                    t.data.frame = [{ "name": name, "obj": obj }];
                    return;
                }
                t.data.frame.push({ "name": name, "obj": obj });
                if (t.data.frame.length == 1) {
                    obj.addClass("active");
                } else {
                    (function () {
                        var pre = t.data.frame[t.data.frame.length - 2].obj;
                        if (pre && pre.hasClass("active")) pre.removeClass("active");
                        obj.addClass("active");
                    }).delay(10);
                }
            },
            // 移除数据（name以及name以后的）
            "remove": function (name) {
                var t = this;
                var frameIndex = name ? -1 : t.data.frame.length - 1;
                t.data.frame.each(function (item, index) {
                    if (frameIndex != -1) return;
                    if (item.name == name) frameIndex = index + 1;
                });
                if (frameIndex < 0) return;
                t.data.frame.each(function (item, index) {
                    if (index < frameIndex) return;
                    var obj = item.obj;
                    if (obj.hasClass("active")) {
                        obj.removeClass("active");
                        (function () {
                            obj.dispose();
                        }).delay(500);
                    } else {
                        obj.dispose();
                    }
                    t.data.frame[index] = null;
                });
                t.data.frame = t.data.frame.clean();
                t.data.frame.getLast().obj.addClass("active");
            }
        },
        "initialize": function (options) {
            var t = this;
            t.setOptions(options);
            t.dom.container = t.options.container;
        },
        // 新开一个窗口
        "open": function (name, action, callback, data) {
            var t = this;
            if (!name) name = action;
            callback = callback ? callback.split(',') : [];
            callback.push("control-frame");
            var frame = t.data.getFrame.apply(t, [name]);
            if (data && typeof (data) == "object") data = Object.toQueryString(data);

            if (!frame) {
                frame = new Element("div", {
                    "data-frame-name": name,
                    "class": "frame-item",
                    "data-bind-action": action,
                    "data-bind-type": "control",
                    "data-bind-stop": true,
                    "data-bind-callback": callback.length == 0 ? null : callback.join(","),
                    "data-bind-post": data || null
                });
                frame.store(STORE_KEY, t);
                new BW.Bind(frame);
                t.data.add.apply(t, [name, frame]);
            } else {
                t.close(name);
                return;
            }
            frame.inject(t.dom.frames.apply(t));
            frame.fire();
            //if (t.options.count == 1) {
            //    Object.forEach(t.data.frame, function (item, itemName) {
            //        if (itemName != name) item.dispose();
            //    });
            //}
        },
        // 未指定name的时候关闭最后一个，指定了name则关闭从包含name以及之后的所有
        "close": function (name) {
            var t = this;
            t.data.remove.apply(t, [name]);
        }
    });

    // 框架的回调代码
    ns.callback["control-frame"] = function () {
        var t = this;
        t.dom.element.getElements("[data-frame]").addEvent("tap", function () {
            var item = this;
            var name = item.get("data-frame");
            if (!name) return;
            var frame = t.dom.element.retrieve(STORE_KEY);
            switch (name) {
                case "back":
                    if (frame) frame.close.apply(frame);
                    break;
                case "link":
                    if (frame) frame.open.apply(frame, [item.get("data-frame-link-name"), item.get("data-frame-link")]);
                    break;
            }
        });
        if (ns.OffCanvasObj) ns.OffCanvasObj.close();
    };

}(BW);