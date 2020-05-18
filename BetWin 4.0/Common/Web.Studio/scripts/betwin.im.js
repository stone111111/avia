/// <reference path="mootools.js" />
/// <reference path="../layui/layui.js" />

// 构建websocket模拟器
function CreateWebSocket(host) {
    if (window["WebSocket"]) return new ReconnectingWebSocket(host);

    //return CreateAjaxWebSocket(host);
    window["WebSocket"] = new Class({
        "id": null,
        "host": null,
        "flash": null,
        // 待处理的回调信息
        "handler": null,
        // 1 在线 
        "readyState": 0,
        "initialize": function (host) {
            var t = this;
            t.id = new Date().getTime();
            t.host = host;
            new Swiff("/studio/layui/websocket.swf", {
                "id": "flshwebsocket",
                "name": "flshwebsocket",
                "params": {
                    "allowScriptAccess": "always"
                }
            }).inject($(document.body));
            t.flash = $("flshwebsocket");

            var timer = setInterval(function () {
                if (t.flash.setCallerUrl) {
                    clearInterval(timer);
                    t.ready.apply(t);
                }
            }, 100);
        },
        "ready": function () {
            var t = this;

            try {
                t.flash.setCallerUrl(location.href);
                t.flash.setDebug(true);
                t.flash.create(t.id, t.host, [], null, 0, null);
            } catch (ex) {
                alert(ex.message);
            }

            if (t.onopen) {
                t.handler = setInterval(function () {
                    var events = t.flash.receiveEvents();
                    if (events.length != 0) {
                        events.each(function (item) {
                            switch (item.type) {
                                case "message":
                                    t.onmessage.apply(t, [{ "data": decodeURIComponent(item.message) }]);
                                    break;
                                case "open":
                                    t.readyState = 1;
                                    t.onopen.apply(t);
                                    break;
                                case "close":
                                    t.readyState = 0;
                                    break;
                            }
                        });
                    }
                }, 100);
            }
        },
        "onopen": null,
        "onmessage": null,
        "onclose": null,
        // 请求发送信息
        "send": function (data) {
            var t = this;
            var result = t.flash.send(t.id, data);
        }
    });

    return new window["WebSocket"](host);
}

// 构建ajax的
function CreateAjaxWebSocket(host) {
    window["WebSocket"] = new Class({
        "id": null,
        "host": null,
        "flash": null,
        // 待处理的回调信息
        "handler": null,
        // 1 在线 
        "readyState": 1,
        // ajax请求对象
        "request": null,
        "initialize": function (host) {
            var t = this;
            t.id = new Date().getTime();
            t.request = new Request.JSON({
                "url": host + "list",
                "onComplete": function () {
                    (function () {
                        t.request.post();
                    }).delay(3000);
                },
                "onSuccess": function (result) {
                    if (result.info.length == 0) return;

                    result.info.each(function (item) {
                        item.cid = item.logid;
                        item.mine = false;
                        item.formid = item.id;
                        item.id = item.key;
                        t.onmessage.apply(t, [{
                            "data": item
                        }]);
                    });
                }
            });
            t.request.post();
        },
        "ready": function () {
            var t = this;
        },
        "onopen": null,
        "onmessage": null,
        "onclose": null,
        // 请求发送信息
        "send": function (data) {
            var t = this;
            data = JSON.decode(data);
            new Request.JSON({
                "url": host + "send",
                "onSuccess": function (result) {
                    if (!result.success) {

                    }
                }
            }).post(data);
        }
    });

    return new window["WebSocket"](host);
}

var IM = new Class({
    "Implements": [Events, Options],
    "options": {
        // 当前站点编号
        "siteid": null,
        // 用户类型 User | Admin | Guest
        "type": null,
        // 初始化接口地址
        "init": "/handler/user/im/init",
        // 图片上传地址
        "upload": "/handler/user/im/uploadimage",
        // 获取所有好友的在线状态
        "online": "/handler/user/im/online",
        // 查看用户信息
        "userinfo": null,
        // 获取组成员
        "group": null,
        //主面板最小化后显示的名称
        "title": "在线客服",
        // 是否开启好友
        "isfriend": true,
        //是否开启群组
        "isgroup": false,
        //websocket服务器地址
        "host": null,
        // 简约模式
        "brief": false,
        // 默认客服是机器人
        "admin": "rebot",
        // 移動端
        "mobile": false
    },
    // 需要缓存的dom对象
    "dom": {
        // 整个对话框
        "chat": null,
        // 当前的对话框（聊天记录、输入框）
        "chatobj": null,
        // 快捷回复
        "reply": null
    },
    // websocket链接对象
    "socket": null,
    // im对象
    "layim": null,
    // 当前的连接状态
    "layimstatus": null,
    // 根据用户类型从cookie中获取session值
    "getSession": function () {
        var t = this;
        var regex = { "ADMIN": /ADMIN=([0-9a-f]{32})/i, "USER": /USER=([0-9a-f]{32})/i, "GUEST": /GHOST=([0-9a-f]{32})/i };
        var cookie = document.cookie;
        var session = null;
        if (t.options.type != null) {
            var value = regex[t.options.type];
            if (!value) return session;
            if (value.test(cookie)) {
                session = value.exec(cookie)[1];
            }
        }
        return session;
    },
    // 转换主机格式
    "getHost": function () {
        var t = this;
        var host = t.options.host;
        var siteId = t.options.siteid;
        if (!host) {
            host = "ws://" + location.host + ":" + siteId + "/service";
        } else {
            var local = location.host;
            if (local.contains(":")) {
                local = local.substr(0, local.indexOf(":"));
            }
            host = host.replace("localhost", local);
        }
        console.log(host);
        return host;
    },
    // 获取websocket连接状态
    "getState": function () {
        var t = this;
        if (t.socket == null) {
            t.setLoading(true);
            return;
        }
        switch (t.socket.readyState) {
            case 1:
                t.setLoading(false);
                break;
            default:
                t.setLoading(true);
                break;
        }
    },
    // 获取状态的定时器
    "timer": null,
    // 设置等待状态
    "setLoading": function (status) {
        var t = this;
        if (t.options.mobile) return;
        if (t.layimstatus === status) return;
        var loading = $("layui-layim-loading");
        if (loading == null) {
            loading = new Element("div", {
                "class": "layui-layer layui-layer-page layui-box layui-layim-min layui-layim-close layer-anim-02",
                "id": "layui-layim-loading",
                "styles": {
                    "bottom": 0,
                    "right": 0,
                    "left": "inherit",
                    "top": "inherit",
                    "width": 140,
                    "height": 50
                },
                "data-dot": 1
            });
            loading.inject(document.body);
        }

        if (status) {
            var loadingMsg = "";
            var loadingDot = loading.get("data-dot").toInt();
            for (var i = 0; i <= loadingDot % 6; i++) {
                loadingMsg += ".";
            }
            loading.set("text", loadingMsg);
            loading.set("data-dot", loadingDot + 1);
        }

        $$(".layui-layer").each(function (item) {
            if (status) {
                item.addClass("layui-layim-loading");
            } else {
                item.removeClass("layui-layim-loading");
            }
        });
        if (status) {
            loading.removeClass("layui-layim-loading");
        } else {
            if (loading) loading.addClass("layui-layim-loading");
        }

        t.layimstatus = status;

    },
    // 初始化
    "initialize": function (options) {
        var t = this;
        t.setOptions(options);
        var config;

        layui.use('layim', function (layim) {
            t.layim = layim;
            //基础配置
            config = layim.config({
                //获取主面板列表信息
                init: {
                    url: t.options.init
                  , type: 'post' //默认get，一般可不填
                  , data: {} //额外参数
                }

                //上传图片接口（返回的数据格式见下文）
              , uploadImage: {
                  url: t.options.upload
                , type: 'post' //默认post
              }
                , members: {
                    url: t.options.group
                , type: 'post'
                }
              , brief: t.options.brief //是否简约模式（默认false，如果只用到在线客服，且不想显示主面板，可以设置 true）
              , title: t.options.title
              , maxLength: 512 //最长发送的字符长度，默认3000
              , isfriend: t.options.isfriend
              , isgroup: t.options.isgroup
              , right: '0px' //默认0px，用于设定主面板右偏移量。该参数可避免遮盖你页面右下角已经的bar。
              , copyright: true //是否授权，如果通过官网捐赠获得LayIM，此处可填true
            });


            layim.on("ready", function (option) {
                if (!option.mine.id) {
                    t.imclose();
                } else {
                    t.options.type = layim.cache().mine.type;
                    t.options.admin = layim.cache().mine.rebot == "0" ? "admin" : "rebot";
                    t.ready(option);

                    t.timer = setInterval(function () {
                        t.getState();
                    }, 1000);
                }
            });

            layim.on('sendMessage', function (res) {
                var data = {
                    "to": res.to.id,
                    "content": res.mine.content
                };
                if (res.to.sign == "客服") {
                    data["rebot"] = t.options.admin == "rebot" ? 1 : 0;
                };
                if (!data.msgid) data.msgid = new Date().getTime();

                console.log(data);
                t.send("Message", {
                    "Content": data.content,
                    "Key": data.to
                });
            });
        });

    },
    //初始化数据已经完成
    "ready": function (options) {
        var t = this;
        t.socket = CreateWebSocket(t.getHost(t.options.host));

        //连接成功时触发
        t.socket.onopen = function (e) {
            t.open();
        };

        // 关闭（服务端主动关闭、发生错误均会触发此事件）
        t.socket.onclose = function (e) {
            e["_status"] = "主动关闭";
            console.debug(e);
        };

        // 错误 （网络原因或者服务端关闭触发）
        t.socket.onerror = function (e) {
            t.setLoading(true);
        };

        t.socket.onmessage = function (msg) {
            t.message(msg.data);
        };

        t.getOnline();

        t.layim.on('chatChange', function (e) {
            if (t.options.mobile) return;
            var data = e.data;
            var element = t.dom.chatobj = $(e.elem[0]);
            t.dom.chat = element.getParent(".layui-layim-chat");
            var tool = element.getElement(".layim-chat-tool");
            var textarea = element.getElement("textarea");

            // 转人工服务的按钮
            !function () {
                if (data.sign == "客服" && t.options.admin == "rebot") {
                    new Element("a", {
                        "href": "javascript:",
                        "text": "转人工服务",
                        "class": "layim-admin-staff",
                        "styles": {
                            "color": "rgb(9, 174, 176)",
                            "position": "absolute",
                            "right": "10px",
                            "font-size": "12px"
                        },
                        "events": {
                            "click": function () {
                                this.fade("out");
                                t.send("Staff");
                                t.options.admin = "staff";
                            }
                        }
                    }).inject(tool);
                }
            }();

            // 管理员端的快捷回复
            !function () {
                if (t.options.type != "ADMIN") return;

                new Element("a", {
                    "href": "javascript:",
                    "text": "快捷回复",
                    "class": "layim-chat-tool-reply",
                    "styles": {
                        "color": "rgb(9, 174, 176)",
                        "position": "absolute",
                        "right": "10px",
                        "font-size": "12px"
                    },
                    "events": {
                        "click": function () {
                            var reply = t.dom.chat.getElement(".layim-chat-reply");
                            if (reply == null) {
                                reply = new Element("div", {
                                    "class": "layim-chat-reply",
                                    "data-bind-action": "services/reply-chat.html",
                                    "data-bind-type": "control",
                                    "data-bind-callback": "service-reply"
                                });
                                reply.inject(t.dom.chat);
                                reply.store("im", t);
                                new BW.Bind(reply);
                            } else {
                                reply.setStyle("display", reply.getStyle("display") == "none" ? "block" : "none");
                            }
                        }
                    }
                }).inject(tool);

            }();

            // 头部的签名栏
            !function () {
                var chat = element.getElement(".layim-chat-title .layim-chat-other span[layim-event]");
                var cite = chat.getElement("cite");
                if (!cite) {
                    cite = new Element("cite");
                    cite.inject(chat);
                }
                if (cite && data["sign"] && data["sign"] != data["username"]) {
                    cite.set({
                        "html": data["sign"],
                        "title": data["sign"],
                        "styles": {
                            "font-size": "12px"
                        }
                    });
                }
            }();

            // 粘贴图片的扩展方法
            !function () {
                var textarea = element.getElement("textarea");
                if (textarea == null) {
                    return;
                }
                if (textarea.get("data-paste")) return;
                textarea.set("data-paste", 1);
                textarea.addEvent("paste", function (e) {
                    if (e.event.clipboardData.items) {
                        var ele = e.event.clipboardData.items;
                        for (var i = 0; i < ele.length; ++i) {
                            var item = ele[i];
                            if (item.kind == "file" && /image\/.*/.test(item.type)) {
                                var blob = item.getAsFile();
                                window.URL = window.URL || window.webkitURL;
                                var blobUrl = window.URL.createObjectURL(blob);

                                var html = "<div class=\"layim-chat-paste-image\" style=\"width:300px; height:300px; background:url('" + blobUrl + "') no-repeat center center; background-size:cover;\"></div>";

                                layer.confirm(html, {
                                    btn: ['确认', '取消'],
                                    scrollbar: false,
                                    closeBtn: 0,
                                    "title": "上传图片"
                                }, function (index, layero) {
                                    var fd = new FormData();
                                    fd.append("file", blob, "image.png");
                                    fd.append("id", Math.floor(Math.random() * 1000000));
                                    var xhr = new XMLHttpRequest();
                                    xhr.onload = function (ex) {
                                        textarea.set({
                                            "value": "",
                                            "disabled": false
                                        })
                                        if (xhr.status == 200) {
                                            var json = ex.target.response;
                                            var result = JSON.decode(json);
                                            if (result.code == 0) {
                                                textarea.set("value", "img[" + result.data.src + "]");
                                            } else {
                                                layer.msg(result.msg || '上传失败');
                                            }
                                        } else {
                                            layer.msg('图片上传错误');
                                        }
                                    }
                                    xhr.open('POST', t.options.upload, true);
                                    textarea.set({
                                        "value": "正在上传...",
                                        "disabled": true
                                    })
                                    xhr.send(fd);
                                    layer.close(index);
                                }, function (index) {
                                });
                            }
                        }
                    }
                });
            }();

            // 判断当前socket的状态
            !function () {
                switch (t.socket.readyState) {
                    case 1:
                        textarea.set({
                            "disabled": false,
                            "placeholder": ""
                        });
                        tool.fade("show");
                        break;
                    default:
                        textarea.set({
                            "disabled": true,
                            "placeholder": "服务器连接错误."
                        });
                        tool.fade("hide");
                        break;
                }
            }();

            // 生成@符号
            !function () {
                if (element.get("data-at")) return;
                element.addEvent("click", function (e) {
                    var obj = $(e.target);
                    if (obj.get("tag") == "img" && obj.getParent().hasClass("layim-chat-user")) {
                        var cite = obj.getNext("cite");
                        if (cite == null) return;
                        var value = cite.get("text");
                        var regex = /^(.+?)201[6|7|8]/;
                        if (!regex.test(value)) return;
                        var username = regex.exec(value)[1];

                        textarea.set("value", "@" + username + " " + textarea.get("value"));
                        textarea.focus();
                    }
                });
                element.set("data-at", "1");
            }();
        });

    },
    // 关闭聊天软件
    "imclose": function () {
        var t = this;
        clearInterval(t.timer);
        $$(".layui-layer").dispose();
        if (t.socket != null) {
            t.socket.close();
        }
    },
    // 获取所有用户的在线状态
    "getOnline": function () {
        var t = this;
    },
    // 设置单个用户的在线或者上线状态
    "setOnline": function (id, online) {
        var t = this;
        var item = $("layim-friend" + id);
        if (item == null) return;
        if (online) {
            t.Receive.Online.apply(t, [{
                "Key": id
            }]);
        } else {
            t.Receive.Offline.apply(t, [{
                "Key": id
            }]);
        }
    },
    // 发送数据
    "send": function (action, data) {
        var t = this;
        if (t.socket == null) return;
        if (!data) data = new Object();
        data["Action"] = action;
        data["ID"] = t.layim.cache().mine.id;
        data["Sign"] = t.layim.cache().mine.sign;
        data["Name"] = t.layim.cache().mine.username;
        data["Avatar"] = t.layim.cache().mine.avatar;
        try {
            t.socket.send(JSON.encode(data));
        } catch (e) {
            layer.msg("信息发送错误");
            console.log(e);
        }
    },
    // socket链接成功
    "open": function () {
        var t = this;
        var session = t.getSession();
        if (session == null) {
            t.imclose();
            return;
        }
        t.send("Online");
    },
    // 收到信息
    "message": function (result) {
        var t = this;
        console.log(result);
        var info = typeof (result) == "string" ? JSON.decode(result) : result;
        if (info == null || !info["Action"]) {
            console.log("收到错误的数据：");
            console.log(result);
            return;
        }
        var action = info["Action"];
        if (t.Receive[action]) {
            console.log(info);
            t.Receive[action].apply(t, [info]);
            return;
        }
    },
    // 收到消息的处理函数
    "Receive": {
        // 聊天信息
        "Message": function (info) {
            var t = this;

            var type = info["Type"] || "friend";

            console.log(type);
            t.layim.getMessage({
                username: info.Name //消息来源用户名
              , avatar: info.Avatar //消息来源用户头像
              , id: info.Key //聊天窗口来源ID（如果是私聊，则是用户id，如果是群聊，则是群组id）
              , type: type //聊天窗口来源类型，从发送消息传递的to里面获取
              , content: info.Content //消息内容
              , cid: info.ID //消息id，可不传。除非你要对消息进行一些操作（如撤回）
              , mine: false //是否我发送的消息，如果为true，则会显示在右方
              , fromid: info.SendID //消息来源者的id，可用于自动解决浏览器多窗口时的一些问题
              , timestamp: info.Time.toInt() //服务端动态时间戳
            });

            // 标记信息已读
            t.send("Read", {
                "LogID": info["ID"]
            });
        },
        // 系统提示（在对话框显示）
        "Tip": function (info) {
            var t = this;
            //Object {Content: "对方不在线，将会在上线后收到您的信息", Key: "119EBB6A92788282A7245BD923950B9A", Action: "Tip"}
            var type = info["Type"] || "friend";
            t.layim.getMessage({
                system: true //系统消息
              , id: info.Key //聊天窗口ID
              , type: type //聊天窗口类型
              , content: info.Content
            });
        },
        // 收到用户的在线通知
        "Online": function (info) {
            var t = this;
            t.layim.setFriendStatus(info.Key, 'online');
        },
        // 收到用户的下线通知
        "Offline": function (info) {
            var t = this;
            t.layim.setFriendStatus(info.Key, 'offline');
        }
    }
});


