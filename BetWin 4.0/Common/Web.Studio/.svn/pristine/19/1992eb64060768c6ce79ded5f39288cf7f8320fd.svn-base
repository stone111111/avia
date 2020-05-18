// 移动端使用 侧滑菜单

!function (ns) {
    // 当前打开的侧滑窗体
    ns.OffCanvasObj = null;

    ns.OffCanvas = new Class({
        "Implements": [Events, Options],
        "options": {
            // 可自定义的遮罩样式
            "mask": null,
            // 菜单大小
            "size": "auto",
            // 附带的数据
            "data": null,
            // 自定义样式
            "cssname": null,
            // 动作类型(bottom|top|left|right)
            "action": "bottom",
            // 要加载的路径地址
            "src": null
        },
        "dom": {
            "mask": null,
            "canvas": null
        },
        "initialize": function (options) {
            if (ns.OffCanvasObj) ns.OffCanvasObj.close();
            var t = ns.OffCanvasObj = this;
            t.setOptions(options);

            t.dom.mask = new Element("div", {
                "class": "bw-mask " + (t.options.mask || ""),
                "events": {
                    "click": function () {
                        console.log("关闭");
                        t.close();
                    }
                }
            });
            var data = t.options.data;
            if (data && typeof (data) == "object") {
                data = Object.toQueryString(t.options.data);
            }
            t.dom.canvas = new Element("div", {
                "class": "bw-offcanvas " + t.options.action + " " + (t.options.cssname || ""),
                "data-bind-action": t.options.src,
                "data-bind-type": "control",
                "data-bind-post": data || null,
                "events": {
                    "click": function (e) {
                        e.stopPropagation();
                    }
                }
            });
            if (t.options.size) {
                switch (t.options.action) {
                    case "top":
                    case "bottom":
                        t.dom.canvas.setStyle("height", t.options.size);
                        break;
                    case "left":
                    case "right":
                        t.dom.canvas.setStyle("width", t.options.size);
                        break;
                }
            }
            t.dom.canvas.inject(t.dom.mask);
            t.dom.mask.inject(document.body);
            new BW.Bind(t.dom.canvas);
            t.dom.mask.addClass.delay(50, t.dom.mask, ["active"]);
        },
        "close": function () {
            var t = this;
            t.dom.mask.removeClass("active");
            t.dom.mask.dispose.delay(250, t.dom.mask);
            ns.OffCanvasObj = null;
        }
    });

}(BW);