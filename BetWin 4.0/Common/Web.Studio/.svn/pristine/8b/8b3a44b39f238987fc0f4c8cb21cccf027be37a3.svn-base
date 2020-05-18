!function () {
    var plusObj = window.webkit || window.android;
    if (!plusObj) return;

    window["plus"] = {
        callback: {}
    };

    // 统一回调函数
    window["_pluscallback"] = function (data) {
        if (!data) return;
        if (typeof data === "string") data = JSON.parse(data);
        var msgId = data["msgId"];
        if (!msgId || !plus.callback[msgId]) return;
        plus.callback[msgId](data);
        delete plus.callback[msgId];
    };

    // 要发送的消息
    var getData = function (action, postData) {
        var data = {
            "action": action,
            "msgId": new Date().getTime(),
            "data": postData || null
        };
        return data;
    };

    var getUrl = function (url) {
        if (/^http/.test(url)) return url;
        var domain = location.protocol + "://" + location.host;
        if (/^\//.test(url)) return domain + url;
        var path = location.pathname;
        path = path.substr(0, path.lastIndexOf('/'));
        return domain + path + "/" + url;
    };

    // 设备管理
    plus["device"] = {
        // 获取设备信息
        get: function (callback) {
            var data = getData("device.get");
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        },
        // 震动
        vibrate: function () {
            var data = getData("device.vibrate");
            plusObj.messageHandlers.device.postMessage(data);
        },
        screen: {
            horizontal: function (callback) {
                var data = getData("device.screen.horizontal");
                if (callback) plus.callback[data.msgId] = callback;
                plusObj.messageHandlers.device.postMessage(data);
            },
            vertical: function (callback) {
                var data = getData("device.screen.vertical");
                if (callback) plus.callback[data.msgId] = callback;
                plusObj.messageHandlers.device.postMessage(data);
            }
        }
    };

    // webview窗体管理
    plus["webview"] = {
        // 获取当前窗体
        currentWebview: function (callback) {
            var data = getData("webview.currentWebview");
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        },
        // 关闭窗体
        close: function (id, aniShow) {
            var data = getData("webview.close", {
                id: id || null,
                aniShow: aniShow || "None"
            });
            plusObj.messageHandlers.device.postMessage(data);
        },
        // 创建一个窗体但是不显示，相当于预加载
        create: function (url, id, aniShow, options, callback) {
            var data = getData("webview.create", {
                url: getUrl(url),
                id: id || null,
                aniShow: aniShow || "None",
                options: options || {}
            });
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        },
        // 显示窗体
        show: function (id) {
            var data = getData("webview.create", {
                id: id
            });
            plusObj.messageHandlers.device.postMessage(data);
        },
        // 创建窗体并且显示
        open: function (url, id, aniShow, options, callback) {
            var data = getData("webview.open", {
                url: getUrl(url),
                id: id || null,
                aniShow: aniShow || "None",
                options: options || {}
            });
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };

    // 键值对存储管理
    plus["storage"] = {
        //获取
        getItem: function (key, callback) {
            var data = getData("storage.getItem", {
                key: key
            });
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        },
        // 写入
        setItem: function (key, value) {
            if (typeof value === "object") value = JSON.stringify(value);
            var data = getData("storage.setItem", {
                key: key,
                value: value
            });
            plusObj.messageHandlers.device.postMessage(data);
        },
        // 删除
        removeItem: function (key, callback) {
            var data = getData("storage.removeItem", {
                key: key
            });
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };

    plus["gallery"] = {
        //保存图片
        save: function (img, callback) {
            var canvas = document.createElement("canvas");
            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0, img.width, img.height);
            var dataURL = canvas.toDataURL("image/png");

            var data = getData("gallery.save", {
                img: dataURL
            });
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };

    // 分享
    plus["share"] = {
        send: function (data, callback) {
            data = getData("share.send", {
                title: data.title || "",
                content: data.content || "",
                href: data.href || ""
            });
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };

    // 二维码
    plus["qrcode"] = {
        scan: function (img, callback) {
            var canvas = document.createElement("canvas");
            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0, img.width, img.height);
            var dataURL = canvas.toDataURL("image/png");

            var data = getData("qrcode.scan", {
                img: dataURL
            });
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        },
        camera: function (callback) {
            var data = getData("qrcode.camera");
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };

    // 浏览器
    plus["navigator"] = {
        open: function (url) {
            var data = getData("navigator.open", {
                url: url
            });
            plusObj.messageHandlers.device.postMessage(data);
        },
        cache: function (callback) {
            var data = getData("navigator.cache");
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };

    // 指纹识别
    plus["touchid"] = {
        check: function (callback) {
            var data = getData("touchid.check");
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        },
        authenticate: function (callback) {
            var data = getData("touchid.authenticate");
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };

    // 消息推送
    plus["push"] = {
        get: function (callback) {
            var data = getData("push.get");
            if (callback) plus.callback[data.msgId] = callback;
            plusObj.messageHandlers.device.postMessage(data);
        }
    };
}();