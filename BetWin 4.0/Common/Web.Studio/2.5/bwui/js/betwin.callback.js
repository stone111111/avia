if (!BW.callback) BW.callback = new Object();
if (!BW.load) BW.load = new Object();

// 系统公用的初始化系统
!function (ns) {
    // 绑定标记了 data-dom 的元素（存在重复元素返回Array）
    ns["dom"] = function () {
        var t = this;
        if (!t.dom.elements) t.dom.elements = new Object();
        t.dom.element.getElements("[data-dom]").each(function (item) {
            var name = item.get("data-dom");
            if (!t.dom.elements[name]) {
                t.dom.elements[name] = item;
            } else if (t.dom.elements[name].length) {
                t.dom.elements[name].push(item);
            } else {
                t.dom.elements[name] = [t.dom.elements[name]];
                t.dom.elements[name].push(item);
            }
        });
    };

    // 绑定搜索控件
    ns["search"] = function () {
        var t = this;
        var searchbox = null;
        var parent = t.dom.element;
        while (parent) {
            if (parent.get("tag") == "form") {
                searchbox = parent;
                break;
            }
            var form = parent.getElement("form");
            if (form) {
                searchbox = form;
                break;
            }
            parent = parent.getParent();
        }
        if (!searchbox) return;
        t.dom.searchbox = searchbox;
        searchbox.addEvent("submit", function (e) {
            t.fire();
            e.stop();
        });
    };

}(BW.load);

// 全局设定
!function (ns) {
    // 设定站点信息
    ns["setting-site"] = function (result) {
        var t = this;
        if (!result.success) return;
        GolbalSetting.Site = result.info;
        console.log(BW.Utils.setStore);
        BW.Utils.setStore("GolbalSetting.Site", GolbalSetting.Site);
    };

}(BW.callback);
// 控件
!function (ns) {
    ns["control-toggle"] = function () {
        var t = this;
        var dom = new Object();
        t.dom.container.getElements("[data-toggle-id]").each(function (item) {
            dom[item.get("data-toggle-id")] = item;
        });
        t.dom.container.getElements("[data-toggle-target]").addEvent("click", function () {
            var target = this.get("data-toggle-target");
            Object.forEach(dom, function (item, key) {
                if (key == target) return;
                var className = item.get("data-toggle-name") || "show";
                if (item.hasClass(className)) item.removeClass(className);
            });
            if (!dom[target]) return;
            var className = dom[target].get("data-toggle-name") || "show";
            dom[target].toggleClass(className);
        });
    };

    // 绑定select控件
    ns["control-select"] = function (result) {
        var t = this;
        if (!result.success) return;
        switch (t.dom.element.get("tag")) {
            case "select":
                switch (typeof (result.info)) {
                    case "object":
                        Object.forEach(result.info, function (value, key) {
                            var element = null;
                            switch (typeof (value)) {
                                case "string":
                                    element = new Element("option", {
                                        "value": value,
                                        "text": key
                                    });
                                    break;
                                case "object":
                                    element = new Element("optgroup", {
                                        "label": key
                                    });
                                    Object.forEach(value, function (v, k) {
                                        new Element("option", {
                                            "value": k,
                                            "text": v
                                        }).inject(element);
                                    });
                                    break;
                            }
                            if (!element) return;

                            element.inject(t.dom.element);
                        });
                        break;
                }

                break;
        }
    };

    // UI控件
    ns["control-ui"] = function () {
        var t = this;

        t.dom.element.getElements("input[data-type]").each(function (item) {
            switch (item.get("data-type")) {
                case "date":
                    item.set("title", "日历控件");
                    break;
            }
        });
    };

    // 向上滚动控件（配合list回调使用）
    ns["control-list-up"] = function () {
        var t = this;
        var height = t.dom.element.getStyle("height").toInt();
        var obj = t.dom.element.getElement("[data-list-element]");

        if (!obj) return;
        var list = t.dom.element.getElements("[data-list-element] > *");
        if (list.length <= 1) return;
        var index = 0;
        var over = false;
        obj.addEvents({
            "mouseover": function () {
                over = true;
            },
            "mouseout": function () {
                over = false;
            }
        })
        var fx = function () {
            var top = (index % list.length) * height * -1;
            if (top == 0) {
                obj.addClass("bw-animate-stop");
            } else {
                obj.removeClass("bw-animate-stop");
            }
            (function () {
                if (!over) {
                    obj.setStyle("margin-top", top);
                    index++;
                }
                fx.delay(1000);
            }).delay(50);
        };
        console.log(obj);
        fx.apply();
    };

}(BW.callback);

// 表单回调
!function (ns) {
    // 提示框（自动关闭）
    ns["form-tip"] = function (result) {
        var t = this;
        new BW.Tip(result.msg, {
            "type": "tip",
            "callback": function (e) {
                var tip = this;
                var callback = t.dom.element.get("data-bind-form-tip");
                if (callback) t.callback(result, callback);
            },
            "target": t.dom.element.get("data-bind-form-tip-target") == "body" ? null : t.dom.element,
            "closetime": t.dom.element.get("data-bind-form-tip-closetime")
        });
    };

    // 弹出确认框
    ns["form-alert"] = function (result) {
        var t = this;
        new BW.Tip(result.msg, {
            "type": "alert",
            "callback": function (e) {
                if (result.success) {
                    var callback = t.dom.element.get("data-bind-form-tip");
                    if (callback) t.callback(result, callback);
                }
            }
        });
    };

    // 填充内容
    ns["form-fill"] = function (result) {
        var t = this;
        if (!result.success) return;
        var data = result.info;
        t.dom.container.getElements("[data-name],[name]").each(function (item) {
            var name = item.get("data-name") || item.get("name");
            var format = item.get("data-format");
            var value = data[name];
            if (!value) return;
            if (format && htmlFunction[format]) value = htmlFunction[format](value);
            switch (item.get("tag")) {
                case "img":
                    item.set("src", value);
                    break;
                case "input":
                case "textarea":
                    item.set("value", value);
                    break;
                case "section":
                    item.set("html", value);
                    break;
                default:
                    item.set("text", value);
                    break;
            }
        });
    };

}(BW.callback);

// Diag回调
!function (ns) {
    ns["diag-close"] = function (result) {
        var t = this;
        var diag = t.dom.element.getParent(".bw-diag");
        if (!diag) return;

        var diagObj = diag.retrieve("diag");
        if (!diagObj) return;

        diagObj.close();
    };
}(BW.callback);

!function (ns) {
    var LIST_TEMPLATE = "LIST_TEMPLATE";
    var HTML_TEMPLATE = "HTML_TEMPLATE";

    ns["list"] = function (result) {
        var t = this;
        var element = t.dom.container.getElement("[data-list-element]");
        if (!element) {
            new BW.Tip("没有指定data-list-element元素");
            return;
        }
        var template = element.retrieve(LIST_TEMPLATE);
        if (!template) {
            template = element.get("html");
            element.store(LIST_TEMPLATE, template);
        }
        if (!result.success) return;
        var list = result.info.list;
        if (!list && result.info.length) {
            list = result.info;
        }
        var html = new Array();
        if (result.info.RecordCount == 0) {
            switch (element.get("tag")) {
                case "tbody":
                    var thead = element.getPrevious("thead");
                    if (thead) {
                        var thead_length = thead.getElements("th").length;
                        html.push("<tr><td colspan=\"" + thead_length + "\"><p class=\"empty\"></p></td></tr>");
                    }
                    break;
            }
        } else {
            list.each(function (item) {
                var content = template.toHtml(item);
                html.push(content);
            });
        }
        element.set("html", html.join(""));
    };

    // 分页控件
    ns["pagesplit"] = function (result) {
        var t = this;
        var pagesplit = t.dom.container.getNext(".bw-pagesplit");
        if (!pagesplit) {
            pagesplit = new Element("div", {
                "class": "bw-pagesplit",
                "events": {
                    "click": function (e) {
                        var target = $(e.target);
                        var page = target.get("data-page");
                        if (!page) return;
                        t.setData({
                            "PageIndex": page
                        });
                        t.fire();
                        t.setData();
                        var top = t.dom.container.getPosition().y;
                        window.scrollTo(0, top);
                    }
                }
            });
            pagesplit.inject(t.dom.container, "after");
        };
        pagesplit.empty();
        if (!result.info.RecordCount) return;

        var maxpage = result.info.RecordCount % result.info.PageSize == 0 ? result.info.RecordCount / result.info.PageSize : Math.floor(result.info.RecordCount / result.info.PageSize) + 1;

        var pageindex = parseInt(result.info.PageIndex);
        console.log(typeof pageindex);
        var list = new Array();
        list.push({ "name": "第一页", "page": 1 });
        if (pageindex != 1) list.push({ "name": "上一页", "page": pageindex - 1 });
        for (var i = Math.max(1, pageindex - 3); i <= Math.min(maxpage, Math.max(7, pageindex + 3)); i++) {
            list.push({ "name": i, "page": i, "active": i == pageindex })
        };
        if (pageindex != maxpage) list.push({ "name": "下一页", "page": pageindex + 1 });
        list.push({ "name": "最后页", "page": maxpage });

        list.each(function (item) {
            new Element("a", {
                "href": "javascript:",
                "class": item.active ? "active" : "",
                "text": item.name,
                "data-page": item.page
            }).inject(pagesplit);
        });
    };

    // html内容渲染
    ns["html"] = function (result) {
        var t = this;
        var html = t.dom.container.retrieve(HTML_TEMPLATE, t.dom.container.get("html"));
        t.dom.container.store(HTML_TEMPLATE, html);
        if (result.info) {
            t.dom.container = t.dom.container.set("html", html.toHtml(result.info));
        }
    }

    // 设定dom元素
    ns["dom"] = function (result) {
        var t = this;
        if (BW.load["dom"]) BW.load["dom"].apply(t);
    }

}(BW.callback);
