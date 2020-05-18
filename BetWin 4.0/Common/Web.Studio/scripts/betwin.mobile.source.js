/// <reference path="mootools.mobile.js" />
// 平台
var Platform = {
    "Andoird": null,
    "IOS": null,
    // 是否是打包好的APP环境
    "APP": null,
    "callback": new Object()
};

!function (ns) {
    var userAgent = window.navigator.userAgent;
    ns.APP = /x5app|betwinapp/i.test(userAgent);
    ns.IOS = /iPhone|iPad/i.test(userAgent);
    ns.Andoird = /Android/i.test(userAgent);
    if (!ns.APP) return;

    document.writeln("<script type=\"text/javascript\" src=\"/cordova.js\"></script>");
    document.addEventListener('deviceready', function () {
        // 获取发布版本号
        ns.callback["appVersion"] = function (success, error) {
            if (!error) error = function () { alert("版本号获取失败"); }
            cordova.getAppVersion.getVersionNumber(success, error);
        }
        // 清除缓存
        ns.callback["cacheClear"] = function () {
            window.cache.clear(function (status) {
                alert('缓存清除成功');
            }, function (status) {
                alert('缓存清理失败，' + status);
            });
        };
        // 检查是否支持指纹识别
        ns.callback["touchCheck"] = function (success, error) {
            if (!error) error = function () { };
            navigator.touchid.checkSupport(success, error);
        };
        // 开启指纹识别
        ns.callback["touchidAuthenticate"] = function (success, error, message) {
            if (!error) error = function () { };
            if (!message) message = "请开始指纹识别：将手指放于home键，核对指纹。";
            navigator.touchid.authenticate(success, error, message);
        };
    });
}(Platform);

/* betwin.js 的移动端优化版本 */
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
            // 不调用全局错误处理的标记
            "callback-error": null,
            // 目标对象，默认为自己
            "target": null,
            // 停止自动执行
            "stop": false,
            // 确认操作的提示文字
            "confirm-tip": "确认要进行该操作吗？"
        },
        // 加载时候的等待样式
        "loading": "bw-loading",
        "dom": {
            // 原始对象
            "element": null,
            // 指定的页面对象
            "container": null,
            // 指定的搜索框对象
            "searchbox": null,
            // 分页控件
            "pagesplit": null
        },
        // 从元素上加载参数值
        "loadOptions": function (el) {
            var t = this;
            Object.forEach(Element.GetAttribute(t.dom.element, "data-bind-"), function (value, key) {
                if (!t.options[key]) t.options[key] = value;
            });
            //if (t.options.data != null && typeOf(t.options.data) == "string") {
            //    t.options.data = t.options.data.parseQueryString();
            //}
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

            //t.loading = "loading-" + t.options.type;

            if (!t.options.stop) t.fire();

            switch (t.options.event) {
                case "click":
                case "submit":
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
            if (typeof (result) == "object" && !result.success && !t.options["callback-error"] && BW.callback["golbal-error"]) {
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
            if (!t.dom.element.get("data-bind-data")) return null;
            var data = new Object();
            t.dom.element.get("data-bind-data").split(",").each(function (item) {

                switch (item) {
                    case "parent":
                        var parent = t.dom.element.getParent("[data-bind-post]");
                        if (parent != null) {
                            t.options.data = Object.merge(data, parent.get("data-bind-post").parseQueryString());
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
            return data;
        },
        // 重新触发绑定的事件 callback:自定义的回调事件（如果指定则执行控件上设定的值）
        "fire": function (callback) {
            var t = this;
            var data = t.getPostData();
            if (data) {
                t.options.data = data;
            }
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
                    document.body.addClass(t.loading);
                },
                "onComplete": function () {
                    document.body.removeClass(t.loading);
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
            var data = t.options.data;
            if (t.dom.searchbox) {
                var searchData = Element.toQueryString(t.dom.searchbox).parseQueryString();
                data = Object.merge(searchData, data);
            }
            new Request.JSON({
                "url": t.options.action,
                "onRequest": function () {
                    document.body.addClass(t.loading);
                },
                "onComplete": function () {
                    document.body.removeClass(t.loading);
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
                    document.body.addClass(t.loading);
                },
                "onComplete": function () {
                    document.body.removeClass(t.loading);
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

                            if (t.options.data != null) {
                                Object.forEach(t.options.data, function (value, key) {
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
                            }
                            form.send();
                        }
                    }
                }
            });

        },
        // 加载绑定一个列表页面
        "list": function () {
            var t = this;
            var data = new Object();
            if (t.dom.searchbox != null) data = Element.toQueryString(t.dom.searchbox).parseQueryString();
            data["pageindex"] = t.options.pageindex;
            if (t.options.data != null) data = Object.merge(data, t.options.data);

            new Request.JSON({
                "url": t.options.action,
                "onRequest": function () {
                    document.body.addClass(t.loading);
                },
                "onComplete": function () {
                    document.body.removeClass(t.loading);
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
            var html = new Array();

            if (result.info.length) {
                result.info = {
                    "RecordCount": result.info.length,
                    "list": result.info
                };
            }
            if (result.info.RecordCount == 0) {
                if (t.dom.container.get("tag") == "table") {
                    var length = t.dom.container.getElement("thead tr").getElements("th").length;
                    html.push("<tr><td colspan=\"" + length + "\"><p class=\"am-alert am-text-center\">没有符合当前条件的记录</p></td></tr>");
                } else {
                    html.push("");
                }
            } else {
                result.info.list.each(function (item) {
                    html.push(template.toHtml(item));
                });
            }
            body.set("html", html.join(""));

            if (t.dom.pagesplit != null) ns.callback.pagesplit.apply(t, [result]);
        },
        // 分页控件
        "pagesplit": function (result) {
            var t = this;
            var page = t.dom.pagesplit;
            page.empty();
            var info = result.info;
            info.PageIndex = info.PageIndex.toInt();
            var maxPage = info.MaxPage = info.RecordCount % info.PageSize == 0 ? (info.RecordCount / info.PageSize) : Math.floor(info.RecordCount / info.PageSize) + 1;
            if (maxPage == 0) return;
            page.addClass("mui-content-padded");


            var ul = new Element("ul", {
                "class": "mui-pagination"
            });

            new Element("li", {
                "class": "mui-previous " + (info.PageIndex == 1 ? "mui-disabled" : ""),
                "html": "<a href=\"javascript:\" data-page=\"1\">«</a>"
            }).inject(ul);

            for (var i = Math.max(1, info.PageIndex - 2); i <= Math.min(info.PageIndex + 2, maxPage); i++) {
                new Element("li", {
                    "class": i == info.PageIndex ? "mui-active" : "",
                    "html": "<a href=\"javascript:\" data-page=\"" + i + "\">" + i + "</a>"
                }).inject(ul);
            }

            new Element("li", {
                "class": "mui-next " + (info.PageIndex == maxPage ? "mui-disabled" : ""),
                "html": "<a href=\"javascript:\" data-page=\"" + (info.MaxPage) + "\">»</a>"
            }).inject(ul);

            ul.inject(page);

            ul.getElements("a").addEvent("click", function (e) {
                if (this.hasClass("am-disabled") || this.hasClass("am-active")) return;
                t.options.pageindex = this.get("data-page");
                t.fire();
            });
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
                var name = item.get("name") || item.get("data-name");
                var value = Object.getValue(info, name);
                switch (item.get("type")) {
                    case "checkbox":
                        value = value == 1 || value == "true";
                        item.set("checked", value);
                        break;
                    case "image":
                        item.set("src", value);
                        break;
                    default:
                        switch (item.get("tag")) {
                            case "select":
                                item.set("data-selected", value);
                                break;
                            case "img":
                                item.set("src", value);
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
            var selected = t.dom.element.get("data-selected");
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
            var frame = t.dom.container.getParent(".frame-item") || (t.dom.container.getParent("[data-bind-search]") || t.dom.container.getParent());
            if (frame == null) return null;
            var search = frame.getElement("form");
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

            var submit = search.getElement("[type=submit]");
            if (submit != null) {
                submit.addEvent("click", function () {
                    t.fire();
                });
            }
            var reset = search.getElement("[type=reset]");
            if (reset) {
                reset.addEvent("click", function () {
                    search.removeClass("show");
                });
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

// 弹出框
(function (ns) {

    // 提示信息
    ns.Tip = new Class({
        Implements: [Events, Options],
        "options": {
            // 点击确定的回调方法
            "callback": null,
            "mask": true,
            "drag": true,
            // 自定义的样式名字
            "cssname": null,
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
            if (t.options.width) {
                t.dom.alert.setStyle("width", t.options.width);
            }

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
                    if (t.options.delay == 0) t.options.delay = 2000;
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
            if (t.options.delay > 0) t.autoclose(t.options.delay);
        },
        // 自动关闭
        "autoclose": function (delay) {
            var t = this;
            if (delay <= 0) {
                t.close();
                if (t.options.type == "tip" && t.options.callback) {
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
            console.log("执行关闭");
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

// 框架的静态存储对象
var Frame = {
    "header": null,
    "title": null,
    "setting": null,
    "footer": null,
    "frames": null,
    "size": {
        "width": 0,
        "height": 0
    },
    // 已加载的页面
    "list": new Array(),
    "init": function () {
        var t = this;
        t.header = $$("header").getLast();
        if (t.header) {
            t.title = t.header.getElement("h1");
            t.setting = t.header.getElement(".setting");
        }
        t.footer = $$("footer").getLast();
        t.frames = $("frames");
        t.size.width = document.body.getStyle("width").toInt() || document.body.getWidth();
    },
    "open": function (url, title, data, callback) {
        var t = Frame;
        var index = t.list.indexOf(title);
        if (index != -1) {
            t.show.apply(t, [index, true]);
            return;
        }
        var defaultCallback = "mobile-check,mobile-tab";
        callback = (callback ? callback + "," : "") + defaultCallback;
        var frame = new Element("div", {
            "class": "frame-item",
            "data-bind-action": url,
            "data-bind-type": "control",
            "data-bind-callback": callback,
            "data-bind-post": data,
            "styles": {
                "width": t.size.width
            }
        });
        frame.inject(t.frames);
        BW.Bind(frame);

        t.list.push(title);
        t.show.apply(t);
        location.href = "#" + url;
    },
    "show": function (index, isBack) {
        var t = this;
        if (index == undefined) index = t.list.length - 1;
        t.frames.getElements(".frame-item").each(function (item, itemIndex) {
            if (itemIndex > index) {
                BW.callback["mobile-dispose"].apply(item.getBindEvent());
            }
        });

        t.frames.setStyles({
            "width": t.size.width * (index + 1),
            "margin-left": t.size.width * index * -1
        });
        if (isBack) {
            t.list = t.list.filter(function (item, itemIndex) { return itemIndex <= index; });
            BW.callback["mobile-check"].apply(t.frames.getElements(".frame-item").getLast().getBindEvent());
        }
    },
    // 后退
    "back": function () {
        var t = this;
        if (t.list.length <= 1) return;
        t.show(t.list.length - 2, true);
    },
    // 打开一个新窗口
    "newopen": function (url, name) {
        if (Platform.APP) {
            window.open(url, "_blank");
            return;
        }
        if (!name) name = "";
        var open = new Element("div", {
            "class": "newopen",
            "html": "<div class=\"title\">" + name + "</div>"
        });
        open.inject(document.body);
        var height = open.getHeight();
        new Element("iframe", {
            "src": url,
            "height": height - 32,
            "scrolling": "yes"
        }).inject(open);

        new Element("a", {
            "href": "javascript:",
            "class": "close",
            "events": {
                "touchend": function () {
                    open.addClass("dispose");
                    open.dispose.delay(500, open);
                }
            }
        }).inject(open);
    }
};

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
            if (!Platform.APP) {
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


window.addEvents({
    "domready": function () {
        $$("[data-bind-action]").each(function (item) {
            BW.Bind(item);
        });
    }
});