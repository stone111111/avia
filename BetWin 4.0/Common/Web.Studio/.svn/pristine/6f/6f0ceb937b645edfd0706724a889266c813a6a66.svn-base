// 弹出框

!function (ns) {
    var DiagObj = new Object();

    ns.Diag = new Class({
        "Implements": [Events, Options],
        "options": {
            // 窗体的名字（防止重复打开）
            "name": null,
            // 是否有遮罩
            "mask": true,
            // 点击遮罩关闭弹出层
            "maskclose": false,
            // 宽度
            "width": 640,
            // 高度
            "height": 480,
            // 是否允许改变大小
            "resize": false,
            // 要加载的目标路径
            "src": null,
            // 要加载的目标类型 control | frame
            "type": "control",
            // 是否使用标题栏
            "title": null,
            "data": null,
            // 是否有关闭按钮
            "close": true,
            // 自定义的样式
            "cssname": null
        },
        "initialize": function (options) {
            var t = this;
            t.setOptions(options);
            if (!t.options.name) return;


            if (DiagObj[t.options.name]) {
                DiagObj[t.options.name].close();
                //DiagObj[t.options.name].element.diag.addClass("shake");
                //(function () {
                //    DiagObj[t.options.name].element.diag.removeClass("shake");
                //}).delay(200);
                //return;
            }

            if (t.options.mask) {
                t.element.mask = new Element("div", {
                    "class": "bw-mask hide " + (t.options.mask ? "" : "hide"),
                    "events": {
                        "click": function () {
                            if (t.options.maskclose) t.close();
                        }
                    }
                });
                t.element.mask.inject(document.body);
                t.element.mask.removeClass.delay(10, t.element.mask, ["hide"]);
            }

            t.element.diag = new Element("div", {
                "class": "bw-diag " + (t.options.cssname || ""),
                "events": {
                    "mousewheel": function (e) {
                        e.stopPropagation();
                    }
                },
                "styles": {
                    "width": t.options.width,
                    "height": t.options.height
                }
            });
            t.element.diag.inject(document.body);

            if (t.options.title) {
                t.element["diag-title"] = new Element("div", {
                    "class": "bw-diag-title",
                    "html": "<h3>" + t.options.title + "</h3>"
                });
                t.element["diag-title"].inject(t.element.diag);
            }

            if (t.options.close) {
                new Element("a", {
                    "href": "javascript:",
                    "class": "bw-diag-close",
                    "events": {
                        "click": function () {
                            t.close();
                        }
                    }
                }).inject(t.element.diag);
            }

            t.element["diag-content"] = new Element("div", {
                "class": "bw-diag-content"
            });
            t.element["diag-content"].inject(t.element.diag);


            switch (t.options.type) {
                case "control":
                    if (t.options.data) {
                        switch (typeof (t.options.data)) {
                            case "string":
                                t.element["diag-content"].set("data-bind-post", t.options.data);
                                break;
                            case "object":
                                t.element["diag-content"].set("data-bind-post", Object.toQueryString(t.options.data));
                                break;
                        }
                    }
                    new ns.Bind(t.element["diag-content"], {
                        "action": t.options.src,
                        "type": "control"
                    });
                    break;
            }

            DiagObj[t.options.name] = t;

            t.element.diag.store("diag", t);

        },
        // 弹出框的dom元素
        "element": {
            // 遮罩元素
            "mask": null,
            // 弹出框元素
            "diag": null,
            // 标题栏
            "diag-title": null,
            // 内容
            "diag-content": null
        },
        // 打开窗体
        "open": function () {
            var t = this;
        },
        // 关闭窗体
        "close": function () {
            var t = this;
            if (t.element.diag) {
                t.element.diag.addClass("hide");
                t.element.diag.dispose.delay(250, t.element.diag);
            }
            if (t.element.mask) {
                t.element.mask.addClass("hide");
                t.element.mask.dispose.delay(250, t.element.mask);
            }
            if (DiagObj[t.options.name]) delete DiagObj[t.options.name];
        }
    });

    // 对外公开的参数
    ns.DiagOpen = function (name, option) {
        option.name = name;
        new BW.Diag(option);
    }

    // 关闭窗口
    ns.DiagClose = function (name) {
        if (name) {
            if (DiagObj[name]) DiagObj[name].close();
        } else {
            Object.forEach(DiagObj, function (diag, key) {
                diag.close();
            });
        }       
    }
}(BW);