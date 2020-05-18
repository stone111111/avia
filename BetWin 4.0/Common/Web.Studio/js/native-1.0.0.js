//　基于原生的JS扩展工具类，避免和其他框架发生冲突


// 全局函数
!function () {
    // document.getElementById 的简写方法
    window["$id"] = function (id) {
        return document.getElementById(id);
    };


}();

// 字符串扩展方法
!function () {

    // 从url上获取内容
    String.prototype.queryString = function (name) {
        var search = location.search;
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var match = search.substr(1).match(reg);
        return match ? match[2] : null;
    };

    // 对字符串进行Base64编码
    String.prototype.base64 = function () {
        return window.btoa(this);
    };

    // Base64解码
    String.prototype.unbase64 = function () {
        return window.atob(this);
    };

}();

// 数字的处理
!function () {
    // 格式化数字 type:保持与C#的用法一致
    Number.prototype.ToString = function (type, options) {
        var value = this;
        if (!type) return String(value);
        if (!options) options = {};
        var result;
        switch (type) {
            case "n":
                result = (function () {
                    options = Utils.extend({
                        dec: 2  // 小数位数
                    }, options);
                    var arr = [];
                    var decimal = Math.abs(value) % 1;
                    var negative = "";
                    if (value < 0) {
                        negative = "-";
                    }
                    var numString = String(Math.floor(Math.abs(value)));
                    var start = numString.length % 3;
                    if (start) arr.push(numString.substr(0, start));
                    for (var i = 0; i < Math.floor(numString.length / 3); i++) {
                        arr.push(numString.substr(i * 3 + start, 3));
                    }
                    return negative + arr.join(",") + (options.dec ? decimal.toFixed(options.dec).substr(1) : "");
                })();
                break;
        }
        return result;
    };
}();

// 数组扩展方法
!function () {

    // 数组内是否存在元素
    Array.prototype.contains = function (value) {
        if (!value) return;
        return this.indexOf(value) !== -1;
    };

    // 移除元素
    Array.prototype.remove = function (value) {
        var index = this.indexOf(value);
        if (index > -1) {
            this.splice(index, 1);
        }
        return this;
    };

    // 获取数组的第一个内容（如果不存在则返回null）
    Array.prototype.first = function (callbackfn) {
        var arr = this;
        if (callbackfn) arr = arr.filter(callbackfn);
        if (arr.length === 0) return null;
        return arr[0];
    };
}();

// Element元素扩展方法
!function () {

    // 添加样式
    Element.prototype.addClass = function (className) {
        if (!className) return;
        this.classList.add(className);
    };

    // 移除样式
    Element.prototype.removeClass = function (className) {
        if (!className) return;
        this.classList.remove(className);
    };

    // 是否存在样式
    Element.prototype.hasClass = function (className) {
        if (!className) return false;
        return this.classList.contains(className);
    };

    Element.prototype.toggleClass = function (className) {
        if (!className) return;
        this.classList.toggle(className);
    };

    // 获取下级元素，返回Map
    Element.prototype.getElements = function (selectors, keySelector) {
        var elem = this;
        var list = elem.querySelectorAll(selectors);
        var data = {};
        if (typeof keySelector === "string") {
            var attribute = keySelector;
            keySelector = function (dom) {
                return dom.getAttribute(attribute);
            };
        }
        for (var i = 0; i < list.length; i++) {
            var key = keySelector(list[i]);
            data[key] = list[i];
        }
        return data;
    };

    // 设置元素属性（判断是否一致，如果一致则不发生变更）
    Element.prototype.set = function (name, value) {
        var elem = this;
        if (typeof value !== "string") value = value.toString();
        switch (name) {
            case "text":
                if (elem.innerText === value) return false;
                elem.innerText = value;
                break;
            case "html":
                if (elem.innerHTML === value) return false;
                elem.innerHTML = value;
                break;
            default:
                if (elem.getAttribute(name) === value) return false;
                elem.setAttribute(name, value);
                break;
        }
        return true;
    };

    // 绑定事件
    Element.prototype.addEvent = function (eventName, listener, childSelector) {
        var elem = this;
        elem.addEventListener(eventName, function (e) {
            var obj = this;
            if (!childSelector) {
                listener.apply(obj, [e]);
            } else {
                var path = e.path.slice(0, e.path.indexOf(this) + 1);
                var target = path.filter(childSelector).first();
                if (target) {
                    listener.apply(obj, [{
                        path: path,
                        target: target
                    }]);
                }
            }
        });
    };

    // 填充同名的数据到页面上显示
    Element.prototype.fill = function (data, options) {
        var obj = this;
        if (!options) options = {};
        options = Utils.extend({
            "field": "data-dom",
            "type": "data-fill"
        }, options);

        var elems = obj.getElements("[" + options.field + "]", options.field);
        for (var key in data) {
            if (elems[key]) {
                var type = elems[key].getAttribute(options.type) || "text";
                elems[key].set(type, data[key]);
            }
        }
        return elems;
    };

    // 获取表单内的全部元素（支持非表单元素）
    Element.prototype.getField = function (selectors, options) {
        var elem = this;
        if (!selectors) selectors = "[name]";
        options = Utils.extend({
            // 获取元素的值
            getValue: function (item) {
                var value = null;
                switch (item.tagName) {
                    case "SELECT":
                    case "TEXTAREA":
                        value = item.value;
                        break;
                    case "INPUT":
                        switch (item.getAttribute("type")) {
                            case "radio":
                                value = item.value;
                                break;
                            case "checkbox":
                                if (item.checked) {
                                    value = item.value;
                                }
                                break;
                            default:
                                value = item.value;
                                break;
                        }
                        break;
                    default:
                        value = item.innerText;
                        break;
                }
                return value;
            },
            getName: function (item) {
                return item.getAttribute("name");
            }
        }, options);
        var list = elem.querySelectorAll(selectors);
        var data = {};
        for (var i = 0; i < list.length; i++) {
            var elemObj = list[i];
            var name = options.getName(elemObj);
            if (!name) continue;
            var value = options.getValue(elemObj);
            if (!value) continue;

            if (!data[name]) {
                data[name] = value;
            } else if (Array.isArray(data[name])) {
                data[name].push(value);
            } else {
                data[name] = [data[name], value];
            }
        }
        for (var key in data) {
            if (Array.isArray(data[key])) {
                data[key] = data[key].join(",");
            }
        }
        return data;
    };

    // 注销自身
    Element.prototype.dispose = function () {
        var elem = this;
        var parent = elem.parentNode;
        parent.removeChild(elem);
    };

    NodeList.prototype.each = function (func) {
        var list = this;
        for (var i = 0; i < list.length; i++) {
            func(list[i], i);
        }
    };
}();

// 日期扩展
!function () {

    // 格式化日期类型
    Date.prototype.formatDate = function (t) {
        var e = { "M+": this.getMonth() + 1, "d+": this.getDate(), "h+": this.getHours(), "m+": this.getMinutes(), "s+": this.getSeconds(), "q+": Math.floor((this.getMonth() + 3) / 3), S: this.getMilliseconds() };
        /(y+)/.test(t) && (t = t.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length))); for (var n in e) e.hasOwnProperty(n) && new RegExp("(" + n + ")").test(t) && (t = t.replace(RegExp.$1, 1 === RegExp.$1.length ? e[n] : ("00" + e[n]).substr(("" + e[n]).length)));
        return t;
    };

}();

if (!window["Utils"]) window["Utils"] = {};
// 工具扩展类
!function (ns) {

    // 全局的参数配置
    ns.config = {
        // 默认的头部信息
        headers: function () {
            return new Object();
        }
    };

    //Auth 认证
    ns["Auth"] = {
        // Basic 方式
        "Basic": function (uid, pwd) {
            return "Basic " + (uid + ":" + pwd).base64();
        }
    };

    // 合并多个对象值
    ns["extend"] = function () {
        if (arguments.length === 0) return null;
        var data = arguments[0] || {};
        for (var i = 1; i < arguments.length; i++) {
            var obj = arguments[i];
            if (!obj) continue;
            for (var key in obj) {
                data[key] = obj[key];
            }
        }
        return data;
    };

    // ajax 异步请求
    ns["ajax"] = function (options) {
        options = ns.extend({
            headers: ns.config.headers() || {},
            method: "POST"
        }, options);
        if (!options.url) return;

        switch (options.type) {
            case "form":
                if (!options.headers["Content-Type"]) options.headers["Content-Type"] = "application/x-www-form-urlencoded";
                if (options.data) {
                    options.body = ns.toQueryString(options.data);
                    delete options["data"];
                }
                break;
            case "json":
                if (!options.headers["Content-Type"]) options.headers["Content-Type"] = "application/json";
                if (options.data) {
                    options.body = JSON.stringify(options.data);
                    delete options["data"];
                }
                break;
        }
        delete options["type"];

        if (options.before) options.before();
        return fetch(options.url, options)
            .then(response => {
                if (options.complete) options.complete(response);
                var type = response.headers.get("Content-Type");
                var result = null;
                switch (type) {
                    case "application/json":
                    case "text/json":
                        result = response.json();
                        break;
                    default:
                        result = response.text();
                        break;
                }
                return result;
            }).then(response => {
                if (options.success) options.success(response);
            }).catch(ex => {
                if (options.catch) options.catch(ex);
            });
    };

    // 发送post请求
    ns["post"] = function (options) {
        options = ns.extend({
            type: "form"
        }, options);
        return ns.ajax(options);
    };

    // 发送ajax请求
    ns["json"] = function (options) {
        options = ns.extend({
            type: "json"
        }, options);
        return ns.ajax(options);
    };

    // 加载样式
    ns["link"] = function (src) {
        if (!src) return;
        var id = src.replace(/[^\w]/g, "-");
        if ($id(id)) return;
        var elem = document.createElement("link");
        elem.setAttribute("href", src);
        elem.setAttribute("rel", "stylesheet");
        elem.setAttribute("id", id);
        document.head.appendChild(elem);
    };

    // 模板渲染
    ns["laytpl"] = function () {
        "use strict";

        var config = {
            open: '{{',
            close: '}}'
        };

        var tool = {
            exp: function (str) {
                return new RegExp(str, 'g');
            },
            //匹配满足规则内容
            query: function (type, _, __) {
                var types = [
                    '#([\\s\\S])+?',   //js语句
                    '([^{#}])*?' //普通字段
                ][type || 0];
                return exp((_ || '') + config.open + types + config.close + (__ || ''));
            },
            escape: function (html) {
                return String(html || '').replace(/&(?!#?[a-zA-Z0-9]+;)/g, '&amp;')
                    .replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/'/g, '&#39;').replace(/"/g, '&quot;');
            },
            error: function (e, tplog) {
                var error = 'Laytpl Error：';
                typeof console === 'object' && console.error(error + e + '\n' + (tplog || ''));
                return error + e;
            }
        };

        var exp = tool.exp, Tpl = function (tpl) {
            this.tpl = tpl;
        };

        Tpl.pt = Tpl.prototype;

        window.errors = 0;

        //编译模版
        Tpl.pt.parse = function (tpl, data) {
            var that = this, tplog = tpl;
            var jss = exp('^' + config.open + '#', ''), jsse = exp(config.close + '$', '');

            tpl = tpl.replace(/\s+|\r|\t|\n/g, ' ')
                .replace(exp(config.open + '#'), config.open + '# ')
                .replace(exp(config.close + '}'), '} ' + config.close).replace(/\\/g, '\\\\')

                //不匹配指定区域的内容
                .replace(exp(config.open + '!(.+?)!' + config.close), function (str) {
                    str = str.replace(exp('^' + config.open + '!'), '')
                        .replace(exp('!' + config.close), '')
                        .replace(exp(config.open + '|' + config.close), function (tag) {
                            return tag.replace(/(.)/g, '\\$1')
                        });
                    return str
                })

                //匹配JS规则内容
                .replace(/(?="|')/g, '\\').replace(tool.query(), function (str) {
                    str = str.replace(jss, '').replace(jsse, '');
                    return '";' + str.replace(/\\/g, '') + ';view+="';
                })

                //匹配普通字段
                .replace(tool.query(1), function (str) {
                    var start = '"+(';
                    if (str.replace(/\s/g, '') === config.open + config.close) {
                        return '';
                    }
                    str = str.replace(exp(config.open + '|' + config.close), '');
                    if (/^=/.test(str)) {
                        str = str.replace(/^=/, '');
                        start = '"+_escape_(';
                    }
                    return start + str.replace(/\\/g, '') + ')+"';
                });

            tpl = '"use strict";var view = "' + tpl + '";return view;';

            try {
                that.cache = tpl = new Function('d, _escape_', tpl);
                return tpl(data, tool.escape);
            } catch (e) {
                delete that.cache;
                return tool.error(e, tplog);
            }
        };

        Tpl.pt.render = function (data, callback) {
            var that = this, tpl;
            if (!data) return tool.error('no data');
            tpl = that.cache ? that.cache(data, tool.escape) : that.parse(that.tpl, data);
            if (!callback) return tpl;
            callback(tpl);
        };

        var laytpl = function (tpl) {
            if (typeof tpl !== 'string') return tool.error('Template not found');
            return new Tpl(tpl);
        };

        laytpl.config = function (options) {
            options = options || {};
            for (var i in options) {
                config[i] = options[i];
            }
        };

        laytpl.v = '1.2.0';

        return laytpl;
    };

    // Map转化成为QueryString字符串
    ns["toQueryString"] = function (data) {
        if (!data) return null;
        var arr = [];
        for (var key in data) {
            var value = encodeURIComponent(data[key]);
            arr.push(key + "=" + value);
        }
        return arr.join("&");
    };
}(Utils);


// 弹出框
if (!window["Popup"]) window["Popup"] = {};
!function (ns) {

    //{
    //    anim: 1,
    //    type: "alert",
    //    skin: "ios",
    //    content: res.msg,
    //    yes: function (index) {
    //        if (res.success) {
    //            betting.removeClass("show");
    //            loadBalance();
    //        }
    //        Popup.close(index);
    //    }
    //}
    var zIndex = 20100101;

    ns.index = 0;

    ns.elem = {};

    // 发起弹出框
    ns.open = function (options) {
        var mask = document.createElement("div");
        mask.addClass("popup-mask");
        mask.style.zIndex = zIndex += 1;
        document.body.appendChild(mask);

        if (!options) options = {};
        ns.index++;
        var box = document.createElement("div");
        box.addClass("popup-box");
        box.addClass("popup-box-" + options.type);
        box.set("data-index", ns.index);
        box.style.zIndex = zIndex += 1;

        var html = [];
        if (options.content) html.push(["<div class='popup-box-content'>", options.content, "</div>"].join(""));

        var buttons = [];
        if (options.btns) {
            buttons.push("<div class='popup-box-btns'>");
            options.btns.forEach(function (item, index) {
                buttons.push("<label data-event='" + index + "'>" + item.name + "</a>");
            });
            buttons.push("</div>");
            html.push(buttons.join(""));
        }

        box.addEvent("click", function (e) {
            var eventIndex = Number(e.target.getAttribute("data-event"));
            var event = options.btns[eventIndex].event;
            if (event) {
                event.apply(box, [box.getAttribute("data-index")]);
            }
        }, function (elem) { return elem.getAttribute("data-event"); });

        box.innerHTML = html.join("");
        document.body.appendChild(box);
        ns.elem[ns.index] = box;


    };

    Popup.close = function (index) {
        if (!ns.elem[index]) return;
        var elem = ns.elem[index];
        var mask = elem.previousElementSibling;
        if (mask && mask.hasClass("popup-mask")) mask.dispose();
        elem.dispose();
        delete ns.elem[index];
    };

    setTimeout(function () {
        var styleId = "native-css";
        if (!document.getElementById(styleId)) {
            var nativeNode = document.head.querySelector("script[src*='js/native-']");
            var src = nativeNode.getAttribute("src");
            src = src.replace(".js", ".css");
            var style = document.createElement("link");
            style.setAttribute("id", styleId);
            style.setAttribute("href", src);
            style.setAttribute("rel", "stylesheet");
            document.head.appendChild(style);
        }
    }, 1000);

}(Popup);
