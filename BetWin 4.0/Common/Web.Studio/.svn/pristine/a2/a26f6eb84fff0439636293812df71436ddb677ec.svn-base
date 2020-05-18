/// <reference path="layui.js" />
document.writeln("<script src=\"/studio/layui/layui.betwin.config.js\"></script>");
/// layim 的封装层
var IM_REPLY_INDEX = 0;
var IM_REPLY_INSERT = null;
var IM_REPLY_SEND = null;
// 当前在线的人数
var IM_ONLINE = 0;
// 快捷回复回调方法
var IM_REPLY = function (obj) {
    IM_REPLY_INSERT(obj.innerText);
    //IM_REPLY_SEND();
    layer.close(IM_REPLY_INDEX);
};

//设置禁言的回调方法
var IM_BLOCK = function (obj) {
    obj = $(obj);
    IM_REPLY_INSERT("@BLOCK:" + (obj.get("data-time") || 0));
    IM_REPLY_SEND();
    layer.close(IM_REPLY_INDEX);
};

var LAYIM = null;

var IM = function (options) {
    // 如果有自定义的客服
    if (options.server) {
        new Element("a", {
            "href": options.server,
            "target": "_blank",
            "class": "layim-customerserver"
        }).inject(document.body);
        return;
    }
    var siteid = options["siteid"];
    var mobile = options["mobile"];
    var prot = location.protocol;
    var notice = options["notice"];
    var callback = options["callback"] || {};
    // 收到信息之后的自定义回调
    var messageCallback = options["onmessage"];
    if (window["GET_LAYIM_HOST"]) window["GET_LAYIM_HOST"](options["host"]);

    var protocol = { "http:": "ws", "https:": "wss" };

    var host = protocol[prot] + "://" + LAYIM_HOST + "/service/" + siteid;

    // 通过cookie获取验证信息
    var getAuth = function () {
        var regex = [/(ADMIN)=(\w{32})/, /(USER)=(\w{32})/, /(GHOST)=(\w{32})/];
        for (var i = 0; i < regex.length; i++) {
            if (!regex[i].test(document.cookie)) continue;
            var result = regex[i].exec(document.cookie);
            var auth = new Object();
            auth[result[1]] = result[2];
            return auth;
        }
        return null;
    };

    var authData = new Object();
    for (var item in getAuth()) {
        authData["_auth_" + item.toLowerCase()] = getAuth()[item];
    }

    var initUrl = "//" + LAYIM_HOST + "/service/layim/init";

    var uploadimage = options["uploadimage"] || "/handler/user/im/uploadimage";

    // 装载layim
    var layimInstall = function (layim) {
        // 与服务端内容的转换
        var config;
        // 头像的缓存
        var avatar = new Object();
        // 从缓存中获取头像
        var getAvatar = function () {
            var cache = layim.cache();
            cache.friend.each(function (friend) {
                friend.list.each(function (item) {
                    avatar[item.id] = item.avatar;
                });
            });
        };
        getAvatar();

        // 参数配置
        !function () {
            config = {
                // 发送信息出去
                "Send": {
                    // 发送上线通知
                    "online": function (group) {
                        var data = {
                            "Action": "Online"
                        };
                        if (group) data["Group"] = group;
                        socket.send(JSON.stringify(data));
                    },
                    // 发送下线通知
                    "offline": function () {

                    },
                    // 发送信息
                    "message": function (res) {
                        var action = "Message";
                        switch (res.to.type) {
                            case "group":
                                action = "Group";
                                break;
                        }
                        socket.send(JSON.stringify({
                            "Action": action,
                            "Content": res.mine.content,
                            "TalkKey": res.to.id,
                            "Time": res.mine.timestamp
                        }));
                    },
                    // 发送一条已读信息
                    "read": function (msgId, notifyId) {
                        socket.send(JSON.stringify({
                            "Action": "Read",
                            "MsgID": msgId || 0,
                            "NotifyID": notifyId || 0
                        }));
                    },
                    // 修改签名
                    "sign": function (value) {
                        socket.send(JSON.stringify({
                            "Action": "Sign",
                            "Content": value
                        }));
                    },
                    // 回复一个pong
                    "pong": function () {
                        socket.send(JSON.stringify({
                            "Action": "Pong"
                        }));
                    },
                    "ping": function () {
                        socket.send(JSON.stringify({
                            "Action": "Ping"
                        }));
                    },
                    // 测试发包
                    "test": function () {
                        if (location.host == "localhost") {

                        }
                    }
                },
                // 收到信息之后的动作
                "Action": {
                    // 上线通知
                    "Online": function (res) {
                        layim.setFriendStatus(res.Key, 'online');
                    },
                    // 下线通知
                    "Offline": function (res) {
                        layim.setFriendStatus(res.Key, 'offline');
                    },
                    // 用户信息
                    "Message": function (res) {
                        var t = this;
                        layim.getMessage({
                            username: res.Name //消息来源用户名
                            , avatar: res.Avatar //消息来源用户头像
                            , id: res.Key //消息的来源ID（如果是私聊，则是用户id，如果是群聊，则是群组id）
                            , type: res["Type"] || "friend" //聊天窗口来源类型，从发送消息传递的to里面获取
                            , content: res.Content //消息内容
                            , cid: 0 //消息id，可不传。除非你要对消息进行一些操作（如撤回）
                            , mine: false //是否我发送的消息，如果为true，则会显示在右方
                            , fromid: "100000" //消息的发送者id（比如群组中的某个消息发送者），可用于自动解决浏览器多窗口时的一些问题
                            , timestamp: Number(res.Time) //服务端动态时间戳
                        });
                        config.Send.read(res.ID, res.NotifyID);
                        if (messageCallback) {
                            messageCallback.apply(t, [res]);
                        }
                    },
                    // 接收到当前装口的系统通知
                    "Tip": function (res) {
                        if (callback[res.Type]) {
                            callback[res.Type](res);
                            return;
                        }
                        var type = res["ChatType"] || "friend";
                        var msg = {
                            system: true //系统消息
                            , id: res.Key //聊天窗口ID
                            , type: type //聊天窗口类型
                            , content: res.Content
                            , avatar: avatar[res.Key] || "/images/space.gif"
                        };
                        var time = res["Time"];
                        layim.getMessage(msg);
                    },
                    // 收到服务端的ping信息
                    "Ping": function (res) {
                        socketTime = new Date();
                        config.Send.pong();
                    },
                    // 收到服务器端的返回信息
                    "Pong": function (res) {
                        socketTime = new Date();
                        if (res.Online) IM_ONLINE = res.Online;
                    }
                }
            };
        }();

        var socket;

        // 心跳时间
        var socketTime = null;

        // websocket 服务端地址
        var ws = new Array();

        // 链接socket服务端 & 绑定事件
        var conection = function () {
            if (socket) socket.close();

            socket = window["ReconnectingWebSocket"] ? new ReconnectingWebSocket(ws.join("/")) : new WebSocket(ws.join("/"));

            socket.onopen = function () {
                socketTime = new Date();
                config.Send.online(options["group"]);
            };

            socket.onmessage = function (e) {
                var data = JSON.parse(e.data);
                if (!data || !data["Action"]) {
                    console.log("收到非json的内容:" + e.data);
                    return;
                }
                var action = data["Action"];
                if (config.Action[action]) {
                    config.Action[action](data);
                } else {
                    console.log("未指定的动作：" + e.data);
                }
            };

            socket.onclose = function (e) {
                console.log("websocket被主动关闭");
                console.log(e);
            };

            socket.onerror = function (e) {
                console.log(e);
            };
        };

        var readly = function () {

            ws.push(host);
            for (var key in getAuth()) {
                ws.push(key);
                ws.push(getAuth()[key]);
            }

            // websocket链接
            !function () {
                conection();
                LAYIM = layim;
                // 计时器
                var timerIndex = 0;
                setInterval(function () {
                    if (!socket || socket.readyState != 1) {
                        document.body.setAttribute("data-layim-loading", 1);
                    } else {
                        if (timerIndex % 10 == 0) {
                            getAvatar();
                            config.Send.ping();
                            config.Send.test();
                        }
                        timerIndex++;
                        document.body.removeAttribute("data-layim-loading");
                        var timeDiff = new Date() - socketTime;

                        // 如果超过30s没有收到心跳则断开，尝试重连
                        if (timeDiff > 60 * 1000) {
                            timeDiff = new Date();
                            socket.close();
                            (function () {
                                conection();
                            }).delay(1500);
                        }
                    }
                }, 1000);
            }();

        };

        if (!mobile) {
            layim.on("ready", readly);
        } else {
            readly();
        }
        // 监听发出的信息
        layim.on('sendMessage', function (res) {
            config.Send.message(res);
        });

        layim.on("sign", function (value) {
            config.Send.sign(value);
        });

        // 切换聊天窗口增加绑定事件
        layim.on("chatChange", function (e) {
            var textarea = e.elem.find("textarea");
            textarea.on("paste", function (e) {
                var ele = e.originalEvent.clipboardData.items;
                if (!ele) return;

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
                                textarea.attr({
                                    "value": "",
                                    "disabled": false
                                })
                                if (xhr.status == 200) {
                                    var json = ex.target.response;
                                    var result = JSON.parse(json);
                                    if (result.code == 0) {
                                        textarea.val("img[" + result.data.src + "]");
                                    } else {
                                        layer.msg(result.msg || '上传失败');
                                    }
                                } else {
                                    layer.msg('图片上传错误');
                                }
                            }
                            xhr.open('POST', uploadimage, true);
                            textarea.attr({
                                "value": "正在上传...",
                                "disabled": true
                            })
                            xhr.send(fd);
                            layer.close(index);
                        }, function (index) {
                        });
                    }
                }
            });
        });
    };

    // 非移动端
    !function () {
        if (mobile) return;
        layui.use('layim', function (layim) {

            var config = {
                min: true,
                title: "在线客服",
                init: {
                    url: initUrl
                    , type: "post"
                    , data: authData
                }
                //获取群员接口（返回的数据格式见下文）
                , members: {
                    url: '//' + LAYIM_HOST + '/service/layim/member' //接口地址（返回的数据格式见下文）
                    , type: 'post' //默认get，一般可不填
                    , data: authData //额外参数
                }
                , uploadImage: {
                    url: uploadimage //接口地址
                    , type: 'post' //默认post
                }
                , isgroup: options["isgroup"]
                , "notice": options["notice"]
                , tool: []
                , copyright: false
            };

            if (options["reply"]) {
                config.tool.push({
                    alias: 'reply',
                    title: '快捷回复',
                    icon: '&#xe60a;'
                });
            }

            if (options["block"]) {
                config.tool.push({
                    alias: 'block',
                    title: '设置禁言',
                    icon: '&#xe651;'
                });
            }

            // 外部传入的工具栏
            !function () {
                if (!options["tool"]) return;
                for (var alias in options["tool"]) {
                    var tool = options["tool"][alias];
                    config.tool.push({
                        alias: alias,
                        title: tool.title,
                        icon: tool.icon
                    });
                }
            }();

            if (options["chatLog"]) config["chatLog"] = options["chatLog"];
            if (options["find"]) config["find"] = options["find"]
            if (options["msgbox"]) config["msgbox"] = options["msgbox"]

            //基础配置
            layim.config(config);

            // 快捷回复工具栏
            !function () {
                var reply = options["reply"];
                if (!reply) return;
                layim.on('tool(reply)', function (insert, send, obj) {
                    var html = new Array();
                    IM_REPLY_INSERT = insert;
                    IM_REPLY_SEND = send;

                    new Request.JSON({
                        "url": "admin/im/getreplylist",
                        "onSuccess": function (result) {
                            html.push("<div class=\"layui-collapse\">");
                            Object.forEach(result.info, function (value, key) {
                                html.push("<div \"class\"=\"layui-colla-item\">");
                                html.push("<h2 class=\"layui-colla-title\">${Category}</h2> <div class=\"layui-colla-content layui-show\">${List}</div>".toHtml({
                                    "Category": key,
                                    "List": value.map(function (item) { return "<a href=\"javascript:\" onclick=\"IM_REPLY(this);\" class=\"layui-reply-item\">" + item + "</a>"; }).join("")
                                }));
                                html.push("</div>");
                            });
                            html.push("</div>")
                            IM_REPLY_INDEX = layer.open({
                                type: 1,
                                content: html.join(""),
                                "maxWidth": 640
                            });
                        }
                    }).post();
                });
            }();

            // 禁言工具栏
            !function () {
                if (!options["block"]) return;

                layim.on('tool(block)', function (insert, send, obj) {
                    var html = new Array();
                    IM_REPLY_INSERT = insert;
                    IM_REPLY_SEND = send;

                    new Request.JSON({
                        "url": "admin/im/getreplylist",
                        "onSuccess": function (result) {
                            html.push("<div class=\"layui-collapse layim-tool-block\">");
                            html.push("<a href=\"javascript:\" onclick=\"IM_BLOCK(this);\">取消禁言</a>");
                            html.push("<a href=\"javascript:\" onclick=\"IM_BLOCK(this);\" data-time=\"60\">禁言1分钟</a>");
                            html.push("<a href=\"javascript:\" onclick=\"IM_BLOCK(this);\" data-time=\"300\">禁言5分钟</a>");
                            html.push("<a href=\"javascript:\" onclick=\"IM_BLOCK(this);\" data-time=\"600\">禁言10分钟</a>");
                            html.push("<a href=\"javascript:\" onclick=\"IM_BLOCK(this);\" data-time=\"1800\">禁言30分钟</a>");
                            html.push("<a href=\"javascript:\" onclick=\"IM_BLOCK(this);\" data-time=\"3600\">禁言1小时</a>");
                            html.push("<a href=\"javascript:\" onclick=\"IM_BLOCK(this);\" data-time=\"86400\">禁言24小时</a>");
                            html.push("</div>")
                            IM_REPLY_INDEX = layer.open({
                                type: 1,
                                content: html.join(""),
                                "maxWidth": 320
                            });
                        }
                    }).post();
                });

            }();

            layimInstall(layim);
        });
    }();

    // 移动端
    !function () {
        if (!mobile) return;
        layui.use('mobile', function () {
            var mobile = layui.mobile, layim = mobile.layim;
            //基础配置
            var config = {
                init: options["init"],
                brief: options["brief"],
                "tabIndex": options["tabIndex"] || 0,
                uploadImage: {
                    url: uploadimage
                },
                "copyright": true,
                "title": "在线客服",
                "isNewFriend": false,
                "chatTitleColor": "#D41E13",
                "tool": []
            };

            // 外部传入的工具栏
            !function () {
                if (!options["tool"]) return;
                for (var alias in options["tool"]) {
                    var tool = options["tool"][alias];
                    config.tool.push({
                        alias: alias,
                        title: tool.title,
                        iconUnicode: tool.icon
                    });
                }
            }();

            layim.config(config);

            // 工具栏事件
            !function () {
                if (!options["tool"]) return;
                for (var alias in options["tool"]) {
                    console.log(alias);
                    var tool = options["tool"][alias];
                    layim.on('tool(' + alias + ')', tool["event"]);
                }
            }();

            if (options["brief"]) {
                var group = null;
                if (options["isgroup"]) {
                    group = options["init"].group[0];
                } else {
                    group = options["init"].friend[0].list[0];
                }

                //创建一个会话
                layim.chat({
                    id: group.id
                    , name: options["isgroup"] ? group.groupname : group.username
                    , type: options["isgroup"] ? 'group' : "friend" //friend、group等字符，如果是group，则创建的是群聊
                    , avatar: group.avatar
                });
            }

            layim.on('back', function () {
                //history.back();
            });

            layimInstall(layim);
        });
    }();
};