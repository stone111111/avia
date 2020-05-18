/*
name: mootools.mobile
description: mootools 移动端扩展
requires: [mootools, mootools-more]
provides: Touch
*/

// 标记这是移动端应用
if (window["MooTools"]) window["MooTools"].mobile = {
    "version": "1.0.0",
    "requires": "1.5.2"
};

// 判断当前环境
if (!window["Platform"]) window["Platform"] = new Object();
!function (ns) {
    var userAgent = window.navigator.userAgent;
    ns.ios = Browser.platform == "ios";
    ns.android = Browser.platform == "android";
    ns.mobile = ns.ios || ns.android;
    ns.app = /x5app|betwinapp/i.test(userAgent);
    ns.callback = new Object();
    // 是否支持触控事件
    ns.touch = ('ontouchstart' in window || window.DocumentTouch && document instanceof DocumentTouch);
    if (!ns.app) {
        return;
    }
    document.writeln("<script type=\"text/javascript\" src=\"/cordova.js\"></script>");
    document.addEventListener('deviceready', function () {

        if (ns.ios) {
            $(document.body).addClass("app-ios");
        }

        // 获取发布版本号
        ns.callback["appVersion"] = function (success, error) {
            if (!error) error = function () { alert("版本号获取失败"); }
            cordova.getAppVersion.getVersionNumber(success, error);
        }
        // 清除缓存
        ns.callback["cacheClear"] = function (success, faild) {
            if (!success) success = function (status) {
                alert('缓存清除成功');
                location.reload();
            };
            if (!faild) faild = function (status) {
                alert('缓存清理失败，' + status);
            };
            window.cache.clear(success, faild);
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
        // 分享
        ns.callback["share"] = function (name, url) {
            if (!name) name = "";
            if (!url) url = window.location.href;
            plugins.socialsharing.share(name, null, null, url);
        };
    });
}(Platform);

// 移动端事件
!function () {
    if (!Platform.mobile || !Platform.touch) return;

    // 清除掉click事件，使用touch事件代替
    !function () {
        var disabled;
        delete Element.Events["click"];
        Element.Events["touch"] = Element.Events["click"] = {
            base: "touchend",
            condition: function (event) {
                if (disabled || event.targetTouches.length !== 0) return false;
                var touch = event.changedTouches[0],
                    target = document.elementFromPoint(touch.clientX, touch.clientY);
                do {
                    if (target == this) return true;
                } while (target && (target = target.parentNode));
                return false;
            }
        };
    }();

    // 滑动事件（暂时只支持左右）
    !function () {
        var name = 'swipe',
            distanceKey = name + ':distance',
            cancelKey = name + ':cancelVertical',
            dflt = 50;
        var start = {}, disabled, active;
        var clean = function () {
            active = false;
        };
        var events = {
            touchstart: function (event) {
                if (event.touches.length > 1) return;
                var touch = event.touches[0];
                active = true;
                start = { x: touch.pageX, y: touch.pageY };
            },
            touchmove: function (event) {
                if (disabled || !active) return;
                var touch = event.changedTouches[0],
                    end = { x: touch.pageX, y: touch.pageY };
                if (this.retrieve(cancelKey) && Math.abs(start.y - end.y) > 10) {
                    active = false;
                    return;
                }
                var distance = this.retrieve(distanceKey, dflt),
                    delta = end.x - start.x,
                    isLeftSwipe = delta < -distance,
                    isRightSwipe = delta > distance;
                if (!isRightSwipe && !isLeftSwipe)
                    return;
                event.preventDefault();
                active = false;
                event.direction = (isLeftSwipe ? 'left' : 'right');
                event.start = start;
                event.end = end;
                this.fireEvent(name, event);
            },
            touchend: clean,
            touchcancel: clean
        };
        Element.Events[name] = {
            onAdd: function () {
                this.addEvents(events);
            },
            onRemove: function () {
                this.removeEvents(events);
            },
        };
    }();
}();

// 移动端的框架加载
if (!window["Frame"]) window["Frame"] = new Object();
!function (ns) {
    // 框架的静态存储对象
    ns["header"] = null,
        ns["title"] = null,
        ns["setting"] = null,
        ns["footer"] = null,
        ns["frames"] = null,
        ns["size"] = {
            "width": 0,
            "height": 0
        },
        // 已加载的页面
        ns["list"] = new Array();

    ns["init"] = function () {
        var t = this;
        t.header = $$("header").getLast();
        if (t.header) {
            t.title = t.header.getElement("h1");
            t.setting = t.header.getElement(".setting");
        }
        t.footer = $$("footer").getLast();
        t.frames = $("frames");
        t.size.width = document.body.getStyle("width").toInt() || document.body.getWidth();
    };
    ns["open"] = function (url, title, data, callback) {
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
    };
    ns["show"] = function (index, isBack) {
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
    };
    // 后退
    ns["back"] = function () {
        var t = this;
        if (t.list.length <= 1) return;
        t.show(t.list.length - 2, true);
    };
    // 打开一个新窗口
    ns["newopen"] = function (url, name) {
        if (Platform.app & !name) {
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
    };
}(Frame);

