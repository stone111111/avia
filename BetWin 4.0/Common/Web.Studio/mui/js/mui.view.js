/*
 * mui view   视图配置
 * @param {type} $
 * @returns {undefined}
 */


(function ($, window, doc) {

    var LAY_BODY = "LAY_app_body",
        setter = $.setter,
        // 来源页面（用于关闭当前页面的时候后退处理）
        origin = null,
        // 外部传入进来的参数值
        params = {},
        // 事件注册
        events = {};

    // 构造器
    var Class = function (id) {
        this.id = id || LAY_BODY;
        if (this.id instanceof Element) {
            this.container = this.id;
        } else if (typeof this.id === "string") {
            this.container = document.getElementById(this.id);
        }
    };

    var view = function (id) {
        return new Class(id);
    };

    // 请求
    view.req = function (options) {
        var that = this,
            success = options.success,
            error = options.error,
            request = setter.request,
            response = setter.response;

        options.data = options.data || {};
        options.headers = options.headers || {};

        if (request.tokenName && !options.headers[request.tokenName]) {
            options.headers[request.tokenName] = $.store(setter.tableName, request.tokenName);
        }

        delete options.success;
        delete options.error;

        if (options.url) {
            if (setter.apiurl && !/^http|^\/\//.test(options.url)) {
                if (setter.apipath && !setter.debug) {
                    options.headers[setter.apipath] = window.btoa ? window.btoa(options.url) : options.url;
                    options.url = setter.apiurl;
                } else {
                    options.url = setter.apiurl + options.url;
                }
            }
        }

        return $.ajax(options.url, $.extend({
            type: "post",
            success: function (res) {
                var statusCode = response.statusCode;

                if (res[response.statusName] === statusCode.ok) {
                    typeof options.done === 'function' && options.done(res);
                }
                //登录状态失效，清除本地 access_token，并强制跳转到登入页
                else if (statusCode.logout(res)) {
                    view.exit();
                }
                //只要 http 状态码正常，无论 response 的 code 是否正常都执行 success
                typeof success === 'function' && success(res);
            },
            error: function (xhr, type, errorThrown) {
                console.error(options.url, xhr, type, errorThrown);
                typeof error === 'function' && error(xhr, type, errorThrown);
            }
        }, options));
    };

    // 请求模板文件渲染
    Class.prototype.render = function (views, params) {
        var that = this,
            setter = $.setter;

        that.params = params;

        views = setter.base + setter.views + views + setter.engine;
        views = views.replace(/\/\.\//g, "/");

        $.ajax({
            url: views,
            dataType: "html",
            type: "GET",
            data: { v: setter.version },
            success: function (html) {
                that.container.classList.remove("loading");
                var res = {
                    body: html
                };
                if (that.then) {
                    that.then(res);
                    delete that.then;
                }
                that.parse(html);
                if (that.done) {
                    that.done(res);
                    delete that.done;
                }
            },
            error(xhr, type, errorThrown) {
                console.error(views, xhr, type, errorThrown);
            }
        });

        return that;
    };

    // 解析模板，并且加入到元素上
    Class.prototype.parse = function (html, refresh, callback) {
        var that = this,
            laytpl = $.laytpl,
            router = $.router(),
            fn = function (options) {
                var tpl = laytpl(options.dataElem.innerHTML),
                    res = $.extend({
                        params: router.params
                    }, options.res);

                options.dataElem.insertAdjacentHTML("afterend", tpl.render(res));

                typeof callback === "function" && callback();

                try {
                    options.done && new Function('d', options.done)(res);
                } catch (e) {
                    console.error(options.dataElem, '\n存在错误回调脚本\n\n', e);
                }
            };

        that.container.innerHTML = html;
        var elemTemp = that.container.querySelectorAll("[template]");
        // 解析脚本
        that.container.querySelectorAll("script").forEach(function (node) {
            if (node.tagName === "SCRIPT" && (!node.hasAttribute("type") || node.getAttribute("type") === "text/javascript")) {
                var script = node.innerHTML;
                that.container.removeChild(node);
                try {
                    new Function(script).apply(that);
                } catch (e) {
                    console.log(e, script);
                }
            }
        });

        router.params = that.params || {};

        // 遍历循环模板
        for (var i = elemTemp.length; i > 0; i--) {
            var dataElem = elemTemp[i - 1];
            var done = dataElem.getAttribute("lay-done"),
                type = dataElem.getAttribute("lay-type") || "get",
                url = laytpl(dataElem.getAttribute("lay-url") || "").render(router),
                data = laytpl(dataElem.getAttribute("lay-data") || "").render(router),
                headers = laytpl(dataElem.getAttribute("lay-headers") || "").render(router);

            try {
                data = new Function("return " + data + ";")();
            } catch (e) {
                console.error("lay-data:", data, e.message);
                data = {};
            }

            try {
                headers = new Function('return ' + headers + ';')();
            } catch (e) {
                console.error("lay-headers:", data, e.message);
                headers = headers || {};
            }

            if (url) {
                switch (type) {
                    case "control":
                        break;
                    case "get":
                    case "post":
                        view.req({
                            url: url,
                            type: type,
                            dataType: "json",
                            headers: headers,
                            data: data,
                            success: function (res) {
                                fn({
                                    dataElem: dataElem,
                                    res: res,
                                    done: done
                                });
                            }
                        });
                        break;
                }
            } else {
                fn({
                    dataElem: dataElem,
                    res: that,
                    done: done
                });
            }
        }
        return that;
    };

    //视图请求成功后的回调
    Class.prototype.then = function (callback) {
        this.then = callback;
        return this;
    };

    //视图渲染完毕后的回调
    Class.prototype.done = function (callback) {
        this.done = callback;
        return this;
    };

    // 对外接口
    $.view = view;

})(mui, window, document);