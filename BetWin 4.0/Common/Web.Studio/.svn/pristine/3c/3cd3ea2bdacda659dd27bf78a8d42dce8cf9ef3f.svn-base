// 主机配置文件
!function () {
    var ns = "LAYIM_HOST";
    if (window[ns]) return;
    // 默认的主机地址
    window["GET_LAYIM_HOST"] = function (host) {
        if (!host) return;
        if (typeof (host) == "string") {
            var server = host.split('|');
            host = {
                "http:": server[0],
                "https:": server[server.length - 1]
            };
        }
        window[ns] = host[location.protocol];
    };
    window["GET_LAYIM_HOST"]({
        "http:": "im.a8.to:8080",
        "https:": "im.a8.to:8443"
    });
}();