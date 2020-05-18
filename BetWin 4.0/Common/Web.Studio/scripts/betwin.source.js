/// <reference path="mootools.js" />
/// <reference path="mootools-more.js" />

//全局的站点参数设定
var GolbalSetting = {
    // 站点参数
    "Site": new Object(),
    // 全局枚举
    "Enum": new Object()
};

if (!window["BW"]) window["BW"] = new Object();

// 控件事件自动绑定
(function (ns) {

    // 绑定事件
    ns.BindEvent = new Class({
        Implements: [Events, Options],
        "options": {
            // 提交地址
            "action": null,
            // 渲染类型 control | ajax | list
            "type": null,
            // 触发数据加载的动作    load | click | submit | confirm
            "event": null,
            // 自定义要提交的数据
            "data": null,
            // 如果是列表需要用到搜索框的话，对应的搜索框ID
            "search": null,
            // 分页控件 ID
            "pagesplit": null,
            // 当前页码
            "pageindex": 1,
            // 分页大小
            "pagesize": 20,
            // 页面加载完成之后执行
            "load": null,
            // 自定义的回调函数
            "callback": null,
            // 目标对象，默认为自己
            "target": null,
            // 停止自动执行
            "stop": false,
            // 确认操作的提示文字
            "confirm-tip": "确认要进行该操作吗？",
            // 可以自定义查询数据的值
            "postdata": "betwin-postdata"
        },
        // 设置自定义查询数据
        "setData": function (data) {
            var t = this;
            t.dom.element.store(t.options.postdata, data);
        },
        // 加载时候的等待样式
        "loading": null,
        "dom": {
            // 原始对象
            "element": null,
            // 指定的页面对象
            "container": null,
            // 指定的搜索框对象
            "searchbox": null,
            // 分页控件
            "pagesplit": null,
            // 页面子元素
            "elements": new Object()
        },
        // 从元素上加载参数值
        "loadOptions": function (el) {
            var t = this;
            Object.forEach(Element.GetAttribute(t.dom.element, "data-bind-"), function (value, key) {
                if (!t.options[key]) t.options[key] = value;
            });
        },
        "initialize": function (el, options) {
            var t = this;
            t.setOptions(options);
            t.dom.element = el = $(el);
            t.loadOptions();
            switch (t.dom.element.get("tag")) {
                case "form":
                    t.options.type = "form";
                    t.dom.element.set("action", t.options.action);
                    break;
            }
            // 如果要加载上级元素设定的值
            var data = t.getPostData();
            if (data) t.options.data = data;
            if (t.options.target) {
                t.dom.container = $(t.options.target);
            } else {
                t.dom.container = t.dom.element;
            }

            t.load();
            t.loadOptions();

            if (t.options.search) t.dom.searchbox = $(t.options.search);
            if (t.options.search && t.dom.searchbox == null) {
                alert("指定的搜索框对象" + t.options.search + "不存在");
                return;
            }

            if (t.options.pagesplit) t.dom.pagesplit = $(t.options.pagesplit);
            if (t.options.pagesplit && t.dom.pagesplit == null) {
                alert("指定的分页控件" + t.options.pagesplit + "不存在");
                return;
            }
            if (t.dom.container == null) return;


            t.loading = t.options.loading || "loading-" + t.options.type;

            if (!t.options.stop) t.fire();

            switch (t.options.event) {
                case "click":
                case "submit":
                case "touchend":
                    t.dom.element.addEvent(t.options.event, function () {
                        t.fire.apply(t);
                    });
                    break;
                case "confirm":
                    t.dom.element.addEvent("click", function () {
                        new BW.Tip(t.options["confirm-tip"], {
                            "type": "confirm",
                            "callback": function () {
                                t.fire.apply(t);
                            }
                        });
                    });
                    break;
            }
            //if (t.options.event) {
            //    t.dom.element.addEvent(t.options.event, function () {
            //        t.fire.apply(t);
            //    });
            //}
        },
        // 加载的时候直接执行的事件
        "load": function () {
            var t = this;
            if (!t.options.load) return;
            t.options.load.split(",").each(function (name) {
                if (ns.load[name]) ns.load[name].apply(t);
            });
        },
        // 执行回调事件
        "run": function (result, callback) {
            var t = this;
            if (!callback) callback = t.options.callback;
            if (!callback) return;
            // 全局的错误处理
            if (typeof (result) == "object" && !result.success && BW.callback["golbal-error"]) {
                BW.callback["golbal-error"].apply(t, [result]);
            }
            callback.split(",").each(function (name) {
                if (ns.callback[name]) ns.callback[name].apply(t, [result]);
            });
        },
        // 重置需要延时加载的控件
        "updateDelay": function () {
            var t = this;
            t.dom.container.getElements("[data-bind-action-delay]").each(function (item) {
                var action = item.get("data-bind-action-delay");
                item.set("data-bind-action", action);
                item.set("data-bind-action-delay", null);
            });
        },
        // 在子控件上绑定事件
        "bindAction": function () {
            var t = this;
            t.updateDelay();
            t.dom.container.getElements("[data-bind-action]").each(function (item) {
                ns.Bind(item);
            });
        },
        // 获取控件上的要提交的值
        "getPostData": function () {
            var t = this;
            var data = new Object();
            if (t.dom.element.get("data-bind-data")) {
                t.dom.element.get("data-bind-data").split(",").each(function (item) {
                    switch (item) {
                        case "parent":
                            var parent = t.dom.element.getParent("[data-bind-post]");
                            if (parent != null) {
                                data = Object.merge(data, parent.get("data-bind-post").parseQueryString());
                            }
                            break;
                        case "url":
                            var url = location.search;
                            if (url && url.indexOf("?") == 0) {
                                url = url.substr(1);
                                data = Object.merge(data, url.parseQueryString());
                            }
                            break;
                        case "form":
                            var form = t.dom.element.getParent("form");
                            if (form) {
                                data = Element.GetData(form);
                            }
                            break;
                        default:
                            if (item.contains("=")) {
                                data = Object.merge(data, item.parseQueryString());
                            }
                            break;
                    }
                });
            }
            if (t.dom.searchbox) {
                var searchData = Element.toQueryString(t.dom.searchbox).parseQueryString();
                data = Object.merge(searchData, data);
            }
            if (t.dom.element.retrieve(t.options.postdata)) {
                data = Object.merge(data, t.dom.element.retrieve(t.options.postdata));
            }
            if (t.options.pageindex && t.options.pageindex != "1" && t.options.type == "list") {
                data["PageIndex"] = t.options.pageindex;
            }
            return data;
        },
        // 重新触发绑定的事件 callback:自定义的回调事件（如果指定则执行控件上设定的值）
        "fire": function (callback) {
            var t = this;
            if (t[t.options.type]) {
                t[t.options.type].apply(t, [callback]);
            }
        },
        // 加载一个html页面
        "control": function () {
            var t = this;
            new Request.HTML({
                "url": t.options.action,
                "onRequest": function () {
                    t.dom.container.addClass(t.loading);
                },
                "onComplete": function () {
                    t.dom.container.removeClass(t.loading);
                },
                "onSuccess": function (responseTree, responseElements, responseHTML, responseJavaScript) {
                    t.dom.container.set("html", responseHTML);
                    t.run(responseHTML);

                    t.bindAction();
                }
            }).get();
        },
        // 加载一个json对象
        "ajax": function (callback) {
            var t = this;
            var data = t.getPostData();
            new Request.JSON({
                "url": t.options.action,
                "onRequest": function () {
                    t.dom.container.addClass(t.loading);
                },
                "onComplete": function () {
                    t.dom.container.removeClass(t.loading);
                },
                "onFailure": function (xhr) {
                    var msg = null;
                    switch (xhr.status) {
                        case 503:
                            msg = null;
                            break;
                        default:
                            msg = xhr.response;
                            break;
                    }
                    if (msg) {
                        msg = msg.replace(/\<a .+?\<\/a\>/g, "BetWin 2.0.1");
                        new BW.Tip(msg);
                    }
                },
                "onSuccess": function (result) {
                    t.run(result, callback);
                    t.bindAction();
                }
            }).post(data);
        },
        // form表单对象
        "form": function () {
            var t = this;
            var form = t.dom.element;
            var formId = form.get("id");
            if (!formId) {
                formId = "form-" + new Date().getTime();
                form.set("id", formId);
            }
            var upload = false;
            // 如果有文件需要上传
            if (form.getElement("[type=file]")) {
                upload = true;
            }
            var send = {
                "onRequest": function () {
                    form.addClass(t.loading);
                },
                "onComplete": function () {
                    form.removeClass(t.loading);
                },
                "onFailure": function (xhr) {
                    new BW.Tip(xhr.response);
                },
                "onSuccess": function (result) {
                    result = JSON.decode(result);
                    t.run(result);
                }
            };
            form.set({
                "data-bind": true,
                "send": send,
                "events": {
                    "submit": function (e) {
                        if (e) e.stop();

                        var isCheck = true;
                        form.getElements("[data-regex]").each(function (item) {
                            if (!isCheck) return;
                            if (!Regex.test(item.get("data-regex"), item.get("value"))) {
                                new BW.Tip(item.get("placeholder"), {
                                    "callback": function () {
                                        item.focus();
                                    }
                                });
                                isCheck = false;
                            }
                        });
                        if (!isCheck) return;

                        if (upload) {
                            // 文件上传
                            var oReq = new XMLHttpRequest();
                            oReq.open("POST", form.get("action"), true);
                            oReq.onload = function (oEvent) {
                                send.onComplete();
                                if (oReq.status == 200) {
                                    send.onSuccess(oReq.response);
                                } else {
                                    send.onFailure(oReq);
                                }
                            };
                            send.onRequest();
                            oReq.send(new FormData(form));
                        } else {
                            // 普通上传
                            if (form.get("data-bind-type") == "search") {
                                var target = ns.Bind(t.dom.container);
                                target.fire();
                                return;
                            }
                            if (this.hasClass(t.loading)) return;

                            Object.forEach(t.getPostData(), function (value, key) {
                                var input = form.getElement("input[name=" + key + "]");
                                if (input == null) {
                                    input = new Element("input", {
                                        "type": "hidden",
                                        "name": key
                                    });
                                    input.inject(form);
                                }
                                input.set("value", value);
                            });

                            form.send();
                        }
                    }
                }
            });

        },
        // 加载绑定一个列表页面
        "list": function () {
            var t = this;
            var data = t.getPostData();
            new Request.JSON({
                "url": t.options.action,
                "onRequest": function () {
                    t.dom.container.addClass(t.loading);
                },
                "onComplete": function () {
                    t.dom.container.removeClass(t.loading);
                },
                "onSuccess": function (result) {
                    t.run(result);
                    t.bindAction();
                }
            }).post(data);
        }
    });

    ns.Bind = function (obj) {
        if (obj.retrieve("bind") == null) {
            obj.store("bind", new BW.BindEvent(obj));
        }
        return obj.retrieve("bind");
    };
})(BW);

// 更新一个可编辑元素
(function (ns) {

    ns.Update = new Class({
        Implements: [Events, Options],
        "options": {
            "action": null,
            "name": null
        },
        "dom": {
            // 当前元素
            "element": null,
            "tag": null,
            "type": null
        },
        // 提示错误并且还原
        "restore": function (msg) {
            var t = this;
            var el = t.dom.element;
            new BW.Tip(msg, {
                "callback": function () {
                    switch (t.dom.type) {
                        case "checkbox":
                            el.set("checked", el.retrieve("value"));
                            break;
                        default:
                            el.set("value", el.retrieve("value"));
                            break;
                    }
                }
            });
        },
        "initialize": function (el, options) {
            var t = this;
            t.dom.element = el = $(el);
            t.setOptions(options);

            Object.forEach(Element.GetAttribute(el, "data-update-"), function (value, key) {
                t.options[key] = value;
            });

            var tag = el.get("tag");
            var type = null;
            switch (tag) {
                case "input":
                    switch (el.get("type")) {
                        case "checkbox":
                        case "radio":
                            type = "checkbox";
                            if (el.get("data-checked") == "true") {
                                el.set("checked", true);
                            }
                            break;
                        default:
                            type = "text";
                            break;
                    }
                    break;
                case "select":
                case "textarea":
                    type = "text";
                    break;
            }

            t.dom.tag = tag;
            t.dom.type = type;

            el.addEvents({
                "focus": function (e) {
                    switch (type) {
                        case "checkbox":
                            this.store("value", this.get("checked"));
                            break;
                        default:
                            this.store("value", this.get("value"));
                            break;
                    }
                },
                "change": function (e) {

                    var data = Object.clone(t.options);

                    switch (type) {
                        case "checkbox":
                            data["value"] = el.get("checked") ? 1 : 0;
                            break;
                        case "text":
                            data["value"] = el.get("value");
                            break;
                    }

                    new Request.JSON({
                        "url": t.options.action,
                        "onRequest": function () {
                            el.set("disabled", true);
                        },
                        "onComplete": function () {
                            el.set("disabled", false);
                        },
                        "onFailure": function (xhr) {
                            t.restore(xhr.statusText);
                        },
                        "onSuccess": function (result) {
                            if (!result.success) {
                                t.restore(result.msg);
                                return;
                            }
                            el.highlight();
                            new BW.Tip(result.msg, {
                                "delay": 3000,
                                "type": "tip",
                                "mask": false,
                                "drag": false
                            });
                        }
                    }).post(data);
                }
            });
        }
    });

})(BW);

// 回调方法
(function (ns) {

    ns.callback = {
        // 默认的页面渲染方法
        "html": function (result) {
            var t = this;
            var html = t.dom.container.retrieve("html", t.dom.container.get("html"));
            t.dom.container.store("html", html);
            if (result.info) {
                t.dom.container = t.dom.container.set("html", html.toHtml(result.info));
            }
        },
        // html渲染之后查找图片控件
        "html-img": function (result) {
            var t = this;
            t.dom.container.getElements("img[data-src]").each(function (item) {
                item.set("src", item.get("data-src"));
                item.set("data-src", null);
            });
        },
        // 绑定页面的元素
        "html-field": function (result) {
            var t = this;
            if (!result.success) {
                new BW.Tip(result.msg);
                return;
            }
            t.dom.container.getElements("[data-bind-html]").each(function (item) {
                var html = item.retrieve("html", item.get("html"));
                item.store("html", item);
                item.set("html", html.toHtml(result.info));
            });
        },
        // 使用laytpl引擎
        "html-tpl": function (result) {
            var t = this;
            if (!result.success) {
                new BW.Tip(result.msg);
                return;
            }
            var body = t.dom.container.getElement("[data-list-element]");
            if (body == null) {
                alert("列表对象中没有指定data-list-element对象");
                return;
            }
            var template = $(t.dom.element.get("data-bind-tpl-template"));
            if (template == null) template = body;

            var html = template.retrieve("template");
            if (html == null) {
                html = template.get("value");
                template.store("template", html);
            }
            console.log(html);
            laytpl(html).render(result.info, function (html) {
                body.set("html", html);
            });

        },
        // 绑定列表
        "list": function (result) {
            var t = this;
            if (!result.success) {
                new BW.Tip(result.msg);
                return;
            }
            var body = t.dom.container.getElement("[data-list-element]");
            if (body == null) {
                alert("列表对象中没有指定data-list-element对象");
                return;
            }
            var template = body.retrieve("template");
            if (template == null) {
                template = body.get("html");
                body.store("template", template);
            }
            body.empty();
            if (!result.info) {
                result.info = {
                    "list": [],
                    "RecordCount": 0,
                    "PageIndex": 0,
                    "PageSize": 20
                };
            }
            var html = new Array();

            if (result.info.length) {
                result.info = {
                    "RecordCount": result.info.length,
                    "list": result.info
                };
            }
            if (result.info.RecordCount == 0) {
                var msg = body.get("data-none-tip") || "没有符合当前条件的记录";
                body.addClass("bw-list-none");
                switch (body.get("tag")) {
                    case "table":
                    case "tbody":
                        var thead = t.dom.container.getElement("thead");
                        if (thead) {
                            var length = thead.getElements("tr th").length;
                            html.push("<tr><td colspan=\"" + length + "\"><p class=\"am-alert am-text-center\">" + msg + "</p></td></tr>");
                        }
                        break;
                    case "ul":
                        html.push("<li class=\"bw-list-none\"><p>" + msg + "</p></li>")
                        break;
                    default:
                        html.push("");
                        break;
                }
                //if (t.dom.container.get("tag") == "table") {
                //    var length = t.dom.container.getElement("thead tr").getElements("th").length;
                //    html.push("<tr><td colspan=\"" + length + "\"><p class=\"am-alert am-text-center\">没有符合当前条件的记录</p></td></tr>");
                //} else {
                //    html.push("");
                //}
            } else {
                result.info.list.each(function (item) {
                    html.push(template.toHtml(item));
                });
            }
            body.set("html", html.join(""));

            if (t.dom.pagesplit != null) ns.callback.pagesplit.apply(t, [result]);
        },
        // 分页控件（从load["pagesplit"]中自动调用）
        "pagesplit": function (result) {
            var t = this;
            var page = t.dom.pagesplit;
            page.empty();
            var info = result.info;
            info.PageIndex = info.PageIndex.toInt();
            var maxPage = info.MaxPage = info.RecordCount % info.PageSize == 0 ? (info.RecordCount / info.PageSize) : Math.floor(info.RecordCount / info.PageSize) + 1;
            if (maxPage == 0) return;

            // 跳转到指定页码
            var jump = function (page) {
                t.options.pageindex = page;
                t.fire();
            }

            switch (t.dom.element.get("data-platform")) {
                case "mobile":
                    // 移动端的分页控件
                    !function () {
                        var ul = new Element("ul", {
                            "class": "pagesplit-mobile"
                        });
                        new Element("li", {
                            "text": "上一页",
                            "class": "nav " + (info.PageIndex == 1 ? "disabled" : ""),
                            "events": {
                                "click": function () {
                                    var page = Math.max(1, info.PageIndex - 1);
                                    jump.apply(t, [page]);
                                }
                            }
                        }).inject(ul);

                        var li = new Element("li", {
                            "class": "jump"
                        });
                        var select = new Element("select", {
                            "events": {
                                "change": function () {
                                    var page = this.get("value");
                                    jump.apply(t, [page]);
                                }
                            }
                        });
                        for (var i = 1; i <= maxPage; i++) {
                            new Element("option", {
                                "value": i,
                                "text": "第" + i + "页",
                                "selected": i == info.PageIndex
                            }).inject(select);
                        }
                        select.inject(li);
                        li.inject(ul);

                        new Element("li", {
                            "text": "下一页",
                            "class": "nav " + (info.PageIndex == maxPage ? "disabled" : ""),
                            "events": {
                                "click": function () {
                                    var page = Math.min(maxPage, info.PageIndex + 1);
                                    console.log(info);
                                    jump.apply(t, [page]);
                                }
                            }
                        }).inject(ul);

                        ul.inject(page);
                    }();
                    break;
                default:
                    // PC端的分页控件
                    !function () {
                        var ul = new Element("ul", {
                            "class": "pageSplit"
                        });
                        new Element("li", {
                            "class": "",
                            "html": "${RecordCount}条记录 ${PageIndex}/${MaxPage}页".toHtml(info)
                        }).inject(ul);

                        new Element("li", {
                            "class": info.PageIndex == 1 ? "am-disabled" : "",
                            "title": "首页",
                            "html": "<a href=\"javascript:\" data-page=\"1\" class=\"am-icon-backward am-text-xs\"></a>"
                        }).inject(ul);

                        for (var i = Math.max(1, info.PageIndex - 3); i <= Math.min(info.PageIndex + 3, maxPage); i++) {
                            new Element("li", {
                                "class": i == info.PageIndex ? "am-active" : "",
                                "html": "<a href=\"javascript:\" data-page=\"" + i + "\">" + i + "</a>"
                            }).inject(ul);
                        }

                        new Element("li", {
                            "class": "",
                            "html": "<input title=\"跳转\" type=\"number\" min=\"1\" max=\"" + maxPage + "\" value=\"" + info.PageIndex + "\" style=\"width:" + Math.max(60, (info.PageIndex.toString().length + 12)) + "px;\" />"
                        }).inject(ul);

                        new Element("li", {
                            "class": info.PageIndex == maxPage ? "am-disabled" : "",
                            "title": "末页",
                            "html": "<a href=\"javascript:\" data-page=\"" + (info.MaxPage) + "\" class=\"am-icon-forward am-text-xs\"></a>"
                        }).inject(ul);

                        ul.inject(page);

                        ul.getElements("a").addEvent("click", function (e) {
                            if (this.hasClass("am-disabled") || this.hasClass("am-active")) return;
                            jump.apply(t, [this.get("data-page")]);
                        });

                        ul.getElement("input[type=number]").addEvent("change", function () {
                            var page = this.get("value").toInt();
                            if (isNaN(page) || page < 1 || page > info.MaxPage) {
                                this.set("value", info.PageIndex);
                                return;
                            }
                            jump.apply(t, [page]);
                        });
                    }();
                    break;
            }
        },
        // 执行绑定在控件上的方法(与form-refser重复）
        "fire": function (result) {
            var t = this;
            var target = t.dom.element.get("data-bind-fire-target") == null ? t.dom.element : $(t.dom.element.get("data-bind-fire-target"));
            if (target == null) {
                alert("指定的执行对象为空 data-bind-fire-target");
                return;
            }

            t = ns.Bind(target);
            t.fire();
        },
        // 成功之后再次触发本 data-bind-fire-target 
        "success-fire": function (result) {
            if (!result.success) return;
            var t = this;
            ns.callback["fire"].apply(t);
        },
        // 错误触发
        "error-tip": function (result) {
            if (result.success) return;
            var t = this;
            new BW.Tip(result.msg, {
                "callback": function () {
                    var callback = t.dom.element.get("data-bind-error-tip");
                    if (!callback || !BW.callback[callback]) return;
                    t.run(result, callback);
                }
            });
        },
        // 绑定元素上的提示信息（包括日历控件）
        "tip": function () {
            ns.load.tip.apply(this);
        },
        // 填充表单（根据元素的name标记）
        "form-fill": function (result) {
            var t = this;
            if (!result.success) {
                new BW.Tip(result.msg, {
                    "callback": function () {
                        var callback = t.dom.element.get("data-bind-form-fill-faild");
                        if (!callback) return;
                        t.run(result, callback)
                    }
                });
                return;
            }
            var info = result.info;
            if (info == null) {
                new BW.Tip("绑定控件为空");
                return;
            }
            t.dom.container.getElements("[name],[data-name]").each(function (item) {
                var name = item.get("data-name") || item.get("name");
                var value = Object.getValue(info, name);
                if (!value) return;
                var format = item.get("data-fill-format");
                if (format && htmlFunction[format]) value = htmlFunction[format](value);
                switch (item.get("type")) {
                    case "checkbox":
                        value = value == 1 || value == "true";
                        item.set("checked", value);
                        break;
                    case "image":
                        item.set("src", value);
                        break;
                    case "datetime-local":
                        value = htmlFunction["datetime-local"](value);
                        item.set("value", value);
                        break;
                    case "file":
                        break;
                    default:
                        switch (item.get("tag")) {
                            case "select":
                                item.set("data-selected", value);
                                break;
                            case "img":
                                item.set("src", value);
                                break;
                            case "span":
                            case "div":
                            case "label":
                            case "p":
                                item.set("html", value);
                                break;
                        }
                        item.set("value", value);
                        break;
                }
                Object.each(Element.GetAttribute(item), function (value, key) {
                    if (/\$\{(.+?)\}/.test(value)) {
                        value = value.toHtml(result.info);
                        item.set("data-" + key, value);
                    }
                });
            });
        },
        // 表单提交之后提示信息
        "form-tip": function (result) {
            var t = this;
            new BW.Tip(result.msg, {
                "callback": function () {
                    if (!result.success) return;
                    if (t.dom.element.get("tag") == "form") {
                        t.dom.element.reset();
                    }
                    var callback = t.dom.element.get("data-bind-form-tip");
                    if (callback) {
                        t.run(result, callback);
                    }
                }
            });
        },
        // 表单保存成功之后重新触发目标控件上的事件
        "form-refser": function (result) {
            var t = this;
            if (!result.success) return;
            var target = t.dom.element.get("data-bind-refser");
            if (target == null) return null;
            target = $(target);
            if (target == null) return null;

            ns.Bind(target).fire();
        },
        // 删除一条记录
        "delete": function (result) {
            var t = this;
            new BW.Tip(result.msg, {
                "callback": function () {
                    if (!result.success) return;
                    var parent = t.dom.element.getParent("[data-bind-delete]");
                    if (parent != null) parent.dispose();
                }
            });
        },
        // 关闭弹出的模式窗口，不判断状态
        "diag-close": function (result) {
            var t = this;
            var diag = t.dom.element.getParent(".diag[data-name]");
            if (diag == null) return;

            var name = diag.get("data-name");
            if (!BW.diagObj[name] || !BW.diagObj[name].target) return null;

            BW.diagObj[name].target.Close();
        },
        // 绑定select控件上的数据
        "select": function (result) {
            var t = this;
            if (!result.info) return;
            var selected = t.dom.element.get("data-selected");
            if (typeOf(result.info) == "array") {
                result.info = {
                    "list": result.info,
                    "recordCount": result.info.length
                };
            }
            if (!result.info.list) {
                Object.forEach(result.info, function (value, key) {
                    var group = new Element("optgroup", {
                        "label": key
                    });
                    Object.forEach(value, function (v, k) {
                        new Element("option", {
                            "text": v,
                            "value": k,
                            "selected": selected == k ? true : null,
                            "data-option": 1
                        }).inject(group);
                    });
                    group.inject(t.dom.element);
                });
                return;
            }
            if (t.dom.element.get("tag") != "select") return;
            var value = t.dom.element.get("data-select-value") || "value";
            var text = t.dom.element.get("data-select-text") || "text";

            t.dom.element.getElements("[data-option]").dispose();
            result.info.list.each(function (item) {
                new Element("option", {
                    "text": item[text],
                    "value": item[value],
                    "selected": selected == item[value] ? true : null,
                    "data-option": 1
                }).inject(t.dom.element);
            });
        },
        // 数据统计
        "count": function (result) {
            if (!result.success) return;
            var t = this;
            var tbody = t.dom.container.getElement("[data-list-element]");
            if (tbody == null) return;
            var tfoot = tbody.getNext("tfoot,.list-tfoot");
            if (tfoot == null) return;

            var html = tfoot.retrieve("html", tfoot.get("html"));
            tfoot.store("html", html);
            tfoot.set("html", html.toHtml(result.info));
        },
        // tab 标签切换效果
        "tab": function (result) {
            var t = this;
            var tab = new Object();
            t.dom.container.getElements("[data-tab-link],[data-tab-target]").each(function (item) {
                var link = item.get("data-tab-link");
                var target = item.get("data-tab-target");

                if (link) {
                    if (!tab[link]) {
                        tab[link] = {
                            "link": new Array(),
                            "target": new Array()
                        };
                    }
                    if (item.get("data-tab-current")) tab[link]["current"] = item.get("data-tab-current");
                    tab[link]["link"].push(item);
                }
                if (target) {
                    if (!tab[target]) {
                        tab[target] = {
                            "link": new Array(),
                            "target": new Array()
                        };
                    }
                    tab[target]["target"].push(item);
                }
            });

            Object.forEach(tab, function (obj, key) {
                if (obj.link.length != obj.target.length) return;
                var current = obj["current"] || "current";
                obj.link.each(function (item, index) {
                    item.addEvent("click", function () {
                        if (item.hasClass(current)) return;
                        obj.link.removeClass(current);
                        obj.target.setStyle("display", "none");
                        this.addClass(current);
                        obj.target[index].setStyle("display", "block");
                    });
                });
                obj.link[0].fireEvent("click");
            });
        },
        // 点击图片上传
        "image-upload": function (result) {
            var t = this;
            t.dom.element.getElements("img[data-upload]").each(function (item) {
                new Element("input", {
                    "type": "file"
                }).inject(item);
            });
        },
        // 给加载之后的元素绑定update事件
        "update": function () {
            var t = this;
            var name = "data-bind-update-action";
            t.dom.element.getElements("[data-update-name]").each(function (item) {
                var action = item.get(name) || t.dom.element.get(name);
                if (action == null) {
                    var parent = item.getParent("[" + name + "]");
                    if (parent != null) action = parent.get(name);
                }
                new BW.Update(item, {
                    "action": action
                });
            });
        },
        "checked": function () {
            var t = this;
            t.dom.element.getElements("[type=checkbox]").each(function (item) {
                if (item.get("data-checked") == "1") {
                    item.set("checked", true);
                }
            })
        }
    }

    // 加载后执行
    ns.load = {
        // 元素的提示框
        "tip": function () {
            var t = this;
            t.dom.element.getElements("[placeholder]").addEvents({
                "focus": function () {
                    var obj = this;
                    var placeholder = obj.get("placeholder");
                    if (placeholder == null || placeholder == "") return;
                    var tip = obj.getNext();
                    if (tip == null || !tip.hasClass("input-tip")) {
                        var diag = obj.getParent(".diag");
                        var header = obj.getParent(".page-header");
                        var position = diag == null ? obj.getPosition(header) : obj.getPosition(diag);
                        tip = new Element("span", {
                            "class": "input-tip",
                            "styles": {
                                "left": position.x,
                                "top": position.y
                            },
                            "text": placeholder
                        });
                        tip.inject(obj, "after");
                    }
                    tip.addClass.delay(100, tip, ["focus"]);
                },
                "blur": function () {
                    var obj = this;
                    var tip = obj.getNext();
                    if (tip == null || !tip.hasClass("input-tip")) return;
                    tip.removeClass("focus");
                }
            });
            t.dom.element.getElements("[data-type=datetime],[data-type=date]").each(function (item) {
                var id = item.get("id");
                if (id == null) {
                    id = "datetime_" + Math.round(Math.random() * 100000);
                    item.set("id", id);
                }
                var type = item.get("data-type");
                var istime = type == "datetime";
                var format = type == "datetime" ? "YYYY-MM-DD hh:mm:ss" : "YYYY-MM-DD";
            });
        },
        // 自动设置控件上的搜索对象
        "search": function () {
            var t = this;
            var search = null;
            var frame = t.dom.container;
            while (frame.get("tag") != "body") {
                search = frame.getElement("form");
                if (search) break;
                frame = frame.getParent();
            }
            if (search == null) return;
            var id = search.get("id");
            if (id == null) {
                id = "search-" + new Date().getTime();
                search.set("id", id);
            }
            t.dom.element.set("data-bind-search", id);

            search.set("onsubmit", "return false;");

            var elementId = t.dom.element.get("id");
            if (elementId == null) {
                elementId = "list-" + new Date().getTime();
                t.dom.element.set("id", elementId);
            }

            var submit = search.getElement("input[type=submit]");
            if (submit != null) {
                submit.set("data-event-fire", elementId);
            }
        },
        // 自动创建分页对象
        "pagesplit": function () {
            var t = this;
            var pageSplit = t.dom.container.getNext(".pageSplit");

            if (pageSplit == null) {
                pageSplit = new Element("div", {
                    "class": "pageSplit",
                    "id": "pagesplit-" + new Date().getTime()
                });
                pageSplit.inject(t.dom.container, "after");
            }

            var id = pageSplit.get("id");
            if (id == null) {
                id = "pagesplit-" + new Date().getTime();
            }

            t.dom.element.set("data-bind-pagesplit", id);
        },
        //从父级元素中获取值，自动生成input元素到form控件里面
        "form-parent": function () {
            var t = this;
            var parent = t.dom.element.getParent("[data-bind-post]");
            if (parent == null) return;
            var data = parent.get("data-bind-post").parseQueryString();
            Object.forEach(data, function (value, key) {
                var input = t.dom.element.getElement("[name=" + key + "]");
                if (input == null) {
                    input = new Element("input", {
                        "name": key,
                        "type": "hidden"
                    });
                    input.inject(t.dom.element);
                }
                input.set("value", value);
            });
        },
        // 页面排序
        "list-sort": function () {
            var t = this;
            if (t.dom.element.get("tag") != "table") return;

            var clearSort = function () {
                var obj = t.dom.element.retrieve("sort");
                if (!obj) return;
                obj.removeClass("asc");
                obj.removeClass("desc");
            };

            var elementSort = function (action) {
                var obj = t.dom.element.retrieve("sort");
                if (!obj) return;
                var index = obj.get("data-list-sort-index").toInt();
                var listElement = t.dom.element.getElement("[data-list-element]");
                var list = listElement.getElements("tr").map(function (item) {
                    if (item.getElement("[colspan]") != null) return null;
                    var td = item.getElements("td")[index];
                    var value = td.get("text");
                    value = value.replace(/[￥,%]/g, "").toFloat();

                    return {
                        "value": value,
                        "item": item
                    };
                });

                list.sort(function (a, b) {
                    if (!a || !b) return 0;
                    if (isNaN(a.value) || isNaN(b.value)) return 0;
                    switch (action) {
                        case "asc":
                            return a.value > b.value ? 1 : -1;
                            break;
                        case "desc":
                            return a.value > b.value ? -1 : 1;
                            break;
                    }
                    return 0;
                });

                list.each(function (t) {
                    if (!t) return;
                    t.item.inject(listElement, "bottom");
                });
            };

            t.dom.element.getElements("thead th").each(function (item, index) {
                if (!item.get("data-list-sort")) return;
                var label = new Element("label", {
                    "html": item.get("html"),
                    "data-list-sort-index": index,
                    "data-list-sort-action": null,
                    "class": "sort",
                    "events": {
                        "click": function () {
                            var obj = this;
                            clearSort();
                            var action = obj.get("data-list-sort-action");
                            switch (action) {
                                case "asc":
                                case null:
                                    action = "desc";
                                    break;
                                case "desc":
                                    action = "asc";
                                    break;
                            }
                            obj.set("data-list-sort-action", action);
                            obj.addClass(action);
                            t.dom.element.store("sort", obj);
                            elementSort(action);
                        }
                    }
                });
                item.empty();
                label.inject(item);
            });
        },
        "dom": function () {
            var t = this;
            t.dom.container.getElements("[data-dom]").each(function (item) {
                t.dom.elements[item.get("data-dom")] = item;
            });
        }
    };


    ns._post = {
        // 当前任务执行的状态
        "status": false,
        // 任务列队
        "list": new Array(),
        // 运行任务
        "run": function () {
            if (this.status) return;
            if (this.list.length == 0) {
                this.status = false;
                return;
            }
            this.list[0].post();
        }
    };

    // 一个排队提交json请求的系统
    ns.POST = function (action, data, options) {
        if (!options) options = new Object();

        var request = new Request.JSON({
            "url": action,
            "data": data,
            "onRequest": function () {
                ns._post.status = true;
            },
            "onComplete": function () {
                ns._post.list.shift();
                ns._post.status = false;
                ns._post.run.delay(100, ns._post);
            },
            "onSuccess": function (result) {
                if (options["callback"]) options["callback"].apply(this, [result]);
            }
        });
        ns._post.list.push(request);
        ns._post.run();
    }

})(BW);

// 移动端的回调方法
(function (ns) {
    ns.callback["mobile-check"] = function () {
        var t = this;
        var obj = null;
        if (t.dom) {
            obj = t.dom.element.getElement("div");
        } else {
            obj = t.getElement("div");
        }
        if (!obj) return;
        var hide = obj.get("data-hide");
        // 头部底部的隐藏显示
        !function () {
            if (hide) hide = hide.split(',');
            ["header", "footer"].each(function (item) {
                if (!Frame[item]) return;
                if (hide && hide.contains(item)) {
                    Frame[item].addClass("hide");
                } else {
                    Frame[item].removeClass("hide");
                }
            });
        }();

        // 底部当前位置选中
        !function () {
            if (hide == "footer" || !Frame.footer) return;
            var home = obj.get("data-home");
            Frame.footer.getElements("[data-home]").each(function (item) {
                if (item.get("data-home") == home) {
                    item.addClass("current");
                } else {
                    item.removeClass("current");
                }
            });
        }();

        // 头部标题栏
        !function () {
            if (!Frame.title) return;
            var title = obj.get("data-title");
            if (!title) title = Frame.list.getLast();
            Frame.title.set("html", title);
        }();

        // 设置栏
        !function () {
            if (!Frame.setting) return;
            var setting = obj.get("data-setting-name");
            if (!setting) { Frame.setting.removeClass("show"); return; }
            Frame.setting.addClass("show");
            Frame.setting.set({
                "html": setting,
                "href": "javascript:" + obj.get("data-setting-href")
            });
        }();

        // 回调方法
        !function () {
            var callback = obj.get("data-callback");
            if (!callback) return;
            callback.split(',').each(function (name) {
                if (ns.callback[name]) ns.callback[name].apply(t);
            });
        }();

        // 移动端扩展
        !function () {
            //1、如果不是APP则隐藏相关按钮
            if (!Platform.app) {
                obj.getElements("[data-app=x5]").each(function (item) {
                    item.dispose();
                });
            }
        }();
    };

    // tab切换
    ns.callback["mobile-tab"] = function () {
        var t = this;
        var tab = t.dom.element.getElements("[data-tab-index]");
        if (tab.length == 0) return;
        var content = new Object();
        t.dom.element.getElements("[data-tab-content]").each(function (item) {
            content[item.get("data-tab-content")] = item;
        });
        var tabCurrent = tab.filter(function (item) { return item.hasClass("current"); }).getLast();
        var contentCurrent = null;
        if (tabCurrent) contentCurrent = content[tabCurrent.get("data-tab-index")];
        tab.addEvent("touchend", function (e) {
            var obj = this;
            if (tabCurrent == obj) return;
            if (tabCurrent) tabCurrent.removeClass("current");
            var name = obj.get("data-tab-index");
            obj.addClass("current");
            tabCurrent = obj;
            if (contentCurrent) contentCurrent.removeClass("show");
            if (content[name]) {
                content[name].addClass("show");
                contentCurrent = content[name];
            }
        });
    };

    // 返回
    ns.callback["mobile-back"] = function () {
        var t = this;
        Frame.back();
    };

    // 关闭一个页面的时候触发的方法
    ns.callback["mobile-dispose"] = function () {
        var t = this;
        var obj = null;
        var frame = null;
        if (t.dom) {
            frame = t.dom.element;
            obj = t.dom.element.getElement("div");
        } else {
            frame = obj;
            obj = t.getElement("div");
        }

        if (obj && obj.get("data-dispose")) {
            obj.get("data-dispose").split(',').each(function (name) {
                if (ns.callback[name]) ns.callback[name].apply(t);
            });
        }
        frame.dispose();
    };
})(BW);

// 移动端的框架页面
(function (ns) {
    // 当前使用的对象
    ns.FrameObj = null;

    ns.Frame = new Class({
        Implements: [Events, Options],
        "options": {
            "action": null,
            "name": null
        },
        // 需要缓存的数据
        "data": {
            "container": null,
            // 缓存
            "cache": {}
        },
        "initialize": function (container, options) {
            var t = ns.FrameObj = this;
            t.setOptions(options);
            t.data.container = $(container);
        },
        // 打开一个窗体
        "open": function (url, options) {
            var t = this;
            if (!options) options = {};
            if (!options.id) options.id = url;
            // 进入页面的方式 默认从右到左
            if (!options.in) options.in = "right";

            Object.forEach(t.data.cache, function (item, key) {
                item.getElements(".bw-off-canvas-wrap.active").each(function (item) {
                    item.removeClass("active");
                });
            });
            var item = t.data.cache[options.id];
            if (!item) {
                t.data.cache[options.id] = new Element("div", {
                    "class": "bw-frame-item " + "in-" + options.in,
                    "id": options.id,
                    "data-bind-action": url,
                    "data-bind-type": "control",
                    "data-bind-load": "frame-item",
                    "data-bind-callback": "frame-item"
                });
            }
            if (options["data"]) t.data.cache[options.id].set("data-bind-post", Object.toQueryString(options["data"]));
            if (options["active"]) t.data.cache[options.id].addClass("active");
            t.data.cache[options.id].inject(t.data.container);
            new BW.Bind(t.data.cache[options.id]);
        },
        // 返回上一页
        "back": function (item, options) {
            var t = this;
            if (!options) options = {};
            var pre = item.getPrevious(".bw-frame-item");
            if (!pre) {
                history.back();
            } else {
                pre.addClass("active");
                if (options.reload) pre.getBindEvent().fire();
                (function () {
                    item.dispose();
                    if (t.data.cache[item.get("id")]) delete t.data.cache[item.get("id")];
                }).delay(300);
            }
        }
    });

    BW.load["frame-item"] = function () {
        var t = this;
        t.dom.element.addEvent("tap", function (e) {
            var obj = $(e.target);
            if (obj.get("data-off-canvas")) {
                var menu = $(obj.get("data-off-canvas"));
                if (menu) menu.toggleClass("active");
            }
            if (obj.get("data-frame")) {
                switch (obj.get("data-frame")) {
                    case "back":
                        BW.FrameObj.back(t.dom.element);
                        break;
                }
            }
            // 关闭遮罩
            if (obj.hasClass("bw-off-canvas-wrap")) {
                obj.removeClass("active");
            }
        });
    };

    BW.callback["frame-item"] = function () {
        var t = this;
        var pre = t.dom.element.getPrevious(".bw-frame-item");
        if (pre) {
            pre.removeClass("active");
            pre.scrollTo.delay(500, pre, [0, 0]);
        }
        t.dom.element.addClass("active");
        ["right", "left", "top", "bottom"].each(function (name) {
            name = "in-" + name;
            t.dom.element.removeClass(name);
        });
    };
})(BW);

// 弹出框
(function (ns) {
    // 提示信息
    ns.Tip = new Class({
        Implements: [Events, Options],
        "options": {
            // 点击确定的回调方法
            "callback": null,
            // 遮罩层
            "mask": true,
            // 是否允许拖动
            "drag": true,
            // 自定义的样式名字
            "cssname": null,
            // 标题栏
            "title": "系统提示",
            // 按钮类型 alert | confirm | tip
            "type": "alert",
            // 弹出窗口自动关闭的时间,为0表示不启用
            "delay": 0,
            // 固定的宽度
            "width": null
        },
        "dom": {
            // 弹出框对象
            "alert": null,
            // 遮罩对象
            "mask": null
        },
        "message": null,
        "initialize": function (msg, options) {
            var t = this;
            t.setOptions(options);
            var html = new Array();
            if (t.options.title) html.push("<div class=\"title\">" + t.options.title + "</div>");
            html.push("<div class=\"content\"><div class=\"content-msg\">" + msg + "</div><p class=\"button\"></p></div>");

            t.dom.alert = new Element("div", {
                "id": "bw-tip",
                "class": "bw-tip bw-tip-hide bw-tip-" + t.options.type,
                "html": html.join("")
            });
            if (t.options.cssname) t.dom.alert.addClass(t.options.cssname);
            if (t.options.width) t.dom.alert.setStyle("width", t.options.width);


            var button = t.dom.alert.getElement(".button");
            switch (t.options.type) {
                case "alert":
                    t.dispose();
                    t.dom.alert.inject(document.body);
                    var submit = new Element("a", {
                        "href": "javascript:",
                        "text": "确定",
                        "class": "alert-btn-submit",
                        "events": {
                            "click": function () {
                                if (t.options.callback) {
                                    t.options.callback.apply(t);
                                }
                                t.close();
                            }
                        }
                    });
                    submit.inject(button);
                    // 设置元素居中
                    !function () {
                        var size = t.dom.alert.getSize();
                        t.dom.alert.setStyles({
                            "margin-left": size.x / -2,
                            "margin-top": size.y / -2
                        });
                    }();
                    break;
                case "confirm":
                    t.dispose();
                    t.dom.alert.inject(document.body);
                    new Element("a", {
                        "href": "javascript:",
                        "text": "确定",
                        "class": "alert-btn-submit",
                        "events": {
                            "click": function () {
                                if (t.options.callback) {
                                    t.options.callback.apply(t);
                                }
                                t.close();
                            }
                        }
                    }).inject(button);
                    new Element("a", {
                        "href": "javascript:",
                        "text": "取消",
                        "class": "alert-btn-submit alert-btn-cancel",
                        "events": {
                            "click": function () {
                                t.close();
                            }
                        }
                    }).inject(button);
                    break;
                case "tip":
                    if (t.options.delay == 0) t.options.delay = 5000;
                    t.dom.alert.inject(document.body);
                    break;
            }

            t.dom.alert.removeClass.delay(100, t.dom.alert, ["bw-tip-hide"]);

            if (t.options.mask) {
                t.dom.mask = new Element("div", {
                    "id": "bw-tip-mask",
                    "class": "tip-mask tip-mask-hide"
                });
                t.dom.mask.inject(document.body);
                t.dom.mask.removeClass.delay(50, t.dom.mask, ["tip-mask-hide"]);
            }
            t.autoclose(t.options.delay);
        },
        // 自动关闭
        "autoclose": function (delay) {
            var t = this;
            if (t.options.delay == 0) return;

            if (delay <= 0) {
                t.close();
                if (t.options.callback) {
                    t.options.callback.apply(t);
                }
                return;
            }
            delay -= 1000;
            t.autoclose.delay(1000, t, [delay]);
        },
        //关闭弹出窗
        "close": function () {
            var t = this;
            if (t.dom.alert != null) { t.dom.alert.addClass("bw-tip-hide"); t.dom.alert.dispose.delay(500, t.dom.alert); }
            if (t.dom.mask != null) { t.dom.mask.addClass("tip-mask-hide"); t.dom.mask.dispose.delay(500, t.dom.mask); }
        },
        // 直接注销元素
        "dispose": function () {
            if ($("bw-tip") != null) $("bw-tip").dispose();
            if ($("bw-tip-mask") != null) $("bw-tip-mask").dispose();
        }
    });
})(BW);



// 得到元素上的绑定事件
Element.prototype.getBindEvent = function () {
    var t = this.retrieve("bind");
    return t;
}


window.addEvent("domready", function () {
    // 加载自定义资源
    !function () {
        var theme = $(document.body).get("data-theme-resource");
        if (!theme) return;
        theme.split(',').each(function (item) {
            if (item == "") return;
            new Request({
                "url": item,
                "async": false,
                "onSuccess": function (result, xhr) {
                    var type = item.substring(item.lastIndexOf('.') + 1);
                    var element = null;
                    switch (type) {
                        case "css":
                            element = new Element("link", {
                                "href": item,
                                "rel": "stylesheet"
                            });
                            break;
                    }
                    element.inject(document.body);
                }
            }).get();
        });
    }();
    // 判断当前平台
    !function () {
        var platform = new Array();
        if (MooTools.mobile) platform.push("bw-mobile");
        platform.push("bw-platform-" + Browser.platform);
        platform.each(function (name) {
            $(document.body).addClass(name);
        });
    }();

    $$("[data-bind-action]").each(function (item) {
        BW.Bind(item);
    });
});

// 全局点击事件(移动端不参与)
!function () {
    if (MooTools.mobile) return;

    window.addEvent("click", function (e) {
        var obj = $(e.target);
        if (obj.get("data-event-target")) {
            var target = $(obj.get("data-event-target"));
            if (target == null) return;

            target.fireEvent("click");
        }
        if (obj.get("data-event-fire")) {
            var target = $(obj.get("data-event-fire"));
            if (target == null) return;

            BW.Bind(target).fire();
        }
        if (obj.get("data-type") == "switch") {
            var hidden = null;
            if (obj.get("name")) {
                var name = obj.get("name");
                hidden = obj.getNext("input[name=" + name + "]");
                if (hidden == null) {
                    hidden = new Element("input", {
                        "type": "hidden",
                        "name": name
                    });
                    hidden.inject(obj, "after");
                    obj.set({
                        "name": null,
                        "data-hidden-name": name
                    });
                }
            }
            if (hidden == null) hidden = obj.getNext("input[name=" + obj.get("data-hidden-name") + "]");
            if (hidden != null) hidden.set("value", obj.get("checked") ? 1 : 0);
        }
    });
}();


