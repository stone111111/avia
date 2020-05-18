window["GolbalSetting"] = {
    "Site": null,
    "User": null,
    // ajax请求自定义的头部信息
    "Header": null
};

// betwin 基础框架
if (!window["BW"]) window["BW"] = new Object();

!function (ns) {
    var EVENT_KEY = "BINDEVENT";

    var userAgent = window.navigator.userAgent;

    ns["platform"] = {
        "x5": /x5/.test(userAgent),
        "ios": /iPhone/i.test(userAgent),
        "android": /Android/i.test(userAgent),
        "windows": /Windows/i.test(userAgent),
        "wechat": /MicroMessenger/i.test(userAgent)
    };

    // 执行渲染
    ns["run"] = {
        // 加载一个html页面
        "control": function () {
            var t = this;
            var success = function (html) {
                t.dom.container.set("html", html);
                t.callback(html);
                ns.Utils.bindAction.apply(t);
            };
            var html = null;
            var cacheKey = "control:" + t.options.action;
            if (t.options.cache) {
                html = ns.Utils.getStore(cacheKey);
            }
            if (html) {
                success.apply(t, [html]);
            } else {
                new Request.HTML({
                    "url": t.options.action,
                    "noCache": !t.options.cache,
                    "onRequest": function () {
                        if (ns.loading["control"]) ns.loading["control"].apply(t, [true]);
                    },
                    "onComplete": function () {
                        if (ns.loading["control"]) ns.loading["control"].apply(t, [false]);
                    },
                    "onSuccess": function (responseTree, responseElements, responseHTML, responseJavaScript) {
                        if (t.options.cache) ns.Utils.setStore(cacheKey, responseHTML);
                        success.apply(t, [responseHTML])
                    }
                }).get();
            }
        },
        // AJAX请求
        "ajax": function (callback) {
            var t = this;
            var data = t.getPostData();

            var success = function (result) {
                t.callback(result, callback);
                // ns.Utils.bindAction.apply(t);
            };

            var cacheKey = "ajax:" + t.options.action + "?" + Object.toQueryString(data);
            var result = null;
            if (t.options.cache) {
                result = ns.Utils.getStore(cacheKey);
            }
            if (result) {
                success.apply(t, [result]);
            } else {
                new Request.JSON({
                    "url": t.options.action,
                    "headers": GolbalSetting.Header || {},
                    "onRequest": function () {
                        if (ns.loading["ajax"]) ns.loading["ajax"].apply(t, [true]);
                    },
                    "onComplete": function () {
                        if (ns.loading["ajax"]) ns.loading["ajax"].apply(t, [false]);
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
                            if (ns.Tip) new BW.Tip(msg);
                        }
                    },
                    "onSuccess": function (result) {
                        success.apply(t, [result]);
                        if (t.options.cache) ns.Utils.setStore(cacheKey, result);
                    }
                }).post(data);
            }
        },
        // 表单提交
        "form": function () {
            var t = this;
            if (t.dom.element.get("tag") == "form") {
                t.dom.element.set({
                    "events": {
                        "submit": function (e) {
                            e.stop();
                            var request = this.get("send");
                            Object.forEach(GolbalSetting.Header || {}, function (value, name) {
                                request.setHeader(name, value);
                            });
                            request.send({ "url": t.options.action });
                        }
                    },
                    "send": {
                        "headers": GolbalSetting.Header || {},
                        "onRequest": function () {
                            if (ns.loading["form"]) ns.loading["form"].apply(t, [true]);
                        },
                        "onComplete": function () {
                            if (ns.loading["form"]) ns.loading["form"].apply(t, [false]);
                        },
                        "onSuccess": function (result) {
                            var result = JSON.decode(result);
                            if (result && result.success) {
                                t.dom.element.reset();
                            }
                            t.callback(result);
                        },
                        "onFailure": function (xhr) {
                            var url = xhr.responseURL;
                            var msg = null;
                            switch (xhr.status) {
                                case 404:
                                    msg = "没有找到路径<br />" + url;
                                    break;
                                case 500:
                                    msg = "接口发生内部错误<br />" + url;
                                    break;
                                default:
                                    msg = xhr.response;
                                    break;
                            }
                            new BW.Tip(msg);
                        }
                    }
                });
            }
        },
        // 列表
        "list": function () {

        }
    };

    // BetWin 的工具类
    ns["Utils"] = {
        // 获取本地存储值
        "getStore": function (key) {
            if (!localStorage) return;
            var result = localStorage.getItem(key);
            if (!result) return null;
            if (key.indexOf("ajax:") == 0 || key.indexOf("json:") == 0) return JSON.decode(result);
            return result;
        },
        // 设定本地存储值
        "setStore": function (key, value) {
            if (!localStorage) return;
            if (typeof (value) == "object") value = JSON.encode(value);
            localStorage.setItem(key, value);
        },
        // 清理缓存（自动判断移动端）
        "clearcache": function (callback) {
            // 清除本地存储
            !function () {
                if (!window["localStorage"]) return;
                for (var i = localStorage.length - 1; i >= 0; i--) {
                    var key = localStorage.key(i);
                    ["control", "ajax"].each(function (item) {
                        if (key.indexOf(item + ":") == 0) localStorage.removeItem(key);
                    });
                }
            }();

            // 执行WeX5的清除事件
            if (ns.platform.x5) {

            }
            if (callback) callback.apply(this);
        },
        // 自动绑定下级的bind事件
        "bindAction": function () {
            var t = this;
            var el = document.body;
            if (t && t.dom && t.dom.container) el = t.dom.container;
            el.getElements("[data-bind-action]").each(function (item) {
                new ns.Bind(item);
            });
        }
    };

    // 绑定元素上的数据
    ns["Bind"] = new Class({
        Implements: [Events, Options],
        "options": {
            // 提交地址
            "action": null,
            // 渲染类型 control | ajax | list | form
            "type": null,
            // 触发数据加载的动作    load | click | submit | confirm
            "event": null,
            // 页面加载完成之后执行
            "load": null,
            // 自定义的回调函数
            "callback": null,
            // 目标对象，默认为自己
            "target": null,
            // 自定义的等待方法
            "loading": null,
            // dom元素上设定的要传输的值
            "data": null,
            // 是否启用本地缓存（仅限于control类型）
            "cache": null,
            // 不自动执行
            "stop": false,
            "confirm": "您确认要进行该操作吗？"
        },
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
        "data": {
            // 要上传的数据
            "post": null
        },
        // 从元素上加载参数值
        "loadOptions": function (el) {
            var t = this;
            if (!el) el = t.dom.element;
            if (!el) return;
            var prefix = "data-bind-";
            var regex = new RegExp("^" + prefix);
            for (var i = 0; i < el.attributes.length; i++) {
                var att = el.attributes[i];
                if (regex.test(att.name)) {
                    var name = att.name.substr(prefix.length);
                    if (!t.options[name]) t.options[name] = att.value;
                }
            }
        },
        // 设定数据
        "setData": function (data) {
            var t = this;
            t.data.post = data;
        },
        // 获取要提交的数据
        "getPostData": function () {
            var t = this;
            var data = new Object();
            if (t.options.data) {
                t.options.data.split(',').each(function (item) {
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
                            var form = t.dom.element.getParent("form,[data-form]");
                            if (!form) {
                                var parent = t.dom.element;
                                while (parent != null) {
                                    form = parent.getElement("form,[data-form]");
                                    if (!form) break;
                                    parent = parent.getParent();
                                }
                            }
                            if (form) {
                                data = Object.merge(data, Element.GetData(form));
                            } else {

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
            if (t.dom.searchbox) data = Object.merge(data, Element.GetData(t.dom.searchbox));
            if (t.data.post) data = Object.merge(data, t.data.post);
            return data;
        },
        // 构造函数
        "initialize": function (el, options) {
            var t = this;
            t.dom.element = $(el);
            t.loadOptions();
            t.setOptions(options);

            if (t.options.target) {
                t.dom.container = $(t.options.target);
            } else {
                t.dom.container = t.dom.element;
            }

            if (t.options.load) {
                t.options.load.split(',').each(function (name) {
                    if (ns.load[name]) ns.load[name].apply(t);
                });
            }
            if (!t.options.stop) t.fire();

            switch (t.options.event) {
                case "click":
                case "tap":
                    t.dom.element.addEvent(t.options.event, function () {
                        t.fire();
                    });
                    break;
                case "confirm":
                    t.dom.element.addEvent("click", function () {
                        new BW.Tip(t.options.confirm, {
                            "callback": function (e) {
                                if (e.type == "confirm") t.fire();
                            }
                        });
                    });
                    break;
            }

            t.dom.element.store(EVENT_KEY, t);
        },
        "fire": function () {
            var t = this;
            if (ns.run[t.options.type]) {
                ns.run[t.options.type].apply(t);
            }
        },
        // 执行回调事件
        "callback": function (result, callback) {
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
        }
    });

    // 获取绑定的事件
    Element.prototype.getBindEvent = function () {
        return this.retrieve(EVENT_KEY);
    };

    // 执行绑定的事件
    Element.prototype.fire = function () {
        var t = this.getBindEvent();
        if (t) t.fire();
    };

}(BW);


// 公共的等待效果
if (!BW.loading) BW.loading = new Object();
!function (ns) {

    var loading = function (type, show) {
        var t = this;
        var cssName = "loading-" + type;
        if (show) {
            t.dom.container.addClass(cssName);
        } else {
            t.dom.container.removeClass(cssName);
        }
    };

    ns["ajax"] = function (show) {
        var t = this;
        loading.apply(t, ["ajax", show]);
    };

    ns["form"] = function (show) {
        var t = this;
        loading.apply(t, ["form", show]);
    };

    ns["control"] = function (show) {
        var t = this;
        loading.apply(t, ["control", show]);
    };

}(BW.loading)


window.addEvent("domready", function () {
    $$("[data-bind-action]").each(function (item) {
        new BW.Bind(item);
    });

    // 给body添加当前平台的标识
    !function () {
        Object.forEach(BW.platform, function (value, key) {
            if (value) document.body.addClass("bw-platform-" + key);
        });
    }();

});