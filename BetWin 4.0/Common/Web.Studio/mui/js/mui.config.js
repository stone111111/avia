/**
 * mui config   系统配置
 * @param {type} $
 * @returns {undefined}
 */

(function ($, window) {

    var Class = function () { };

    $.setter = {
        container: "LAY_app",
        base: "./src/",
        views: "./views/",
        entry: "index", //默认视图文件名
        engine: '.html', //视图文件后缀名
        version: "1.0",   // 版本号
        //自定义请求字段
        request: {
            tokenName: "Authorization" //自动携带 token 的字段名。可设置 false 不携带。
        },
        tableName: 'muiStore', //本地存储表名
        response: {
            statusCode: {
                // 登录状态失效的状态码
                logout: function (res) {
                    return !res.success && res.info && (res.info.ErrorType === "Login" || res.info.ErrorType === "Authorization");
                }
            }
        },
        //　退出的动作
        logout: function () {
            location.hash = '/user/login';
        },
        // 独立页面
        indPage: [
            '/user/login', //登入页
            "/user/register"
        ],
        // 主页的入口页面
        homePage: [],
        // 视图层的初始索引值
        viewIndex: 19891014
    };

    $.config = function (options) {
        $.setter = $.extend($.setter, options);
        return new Class();
    };

    // 初始化系统
    Class.prototype.use = function () {
        var view = $.view,
            router = $.router(),
            path = router.path,
            setter = $.setter;

        //默认读取主页
        if (!path.length) path = [''];
        if (!path[path.length - 1]) {
            path[path.length - 1] = setter.entry;
        }

        var bodyElem = null;

        // 加载新页面
        var newPage = function () {
            var path = $.router().path;
            var oldView = bodyElem.querySelector("view");

            var viewElem = document.createElement("view");
            document.getElementById("LAY_app_body").appendChild(viewElem);
            view(viewElem).render(path.join('/')).done(function () {
                if (oldView) {
                    bodyElem.removeChild(oldView);
                }
            });
        };

        view(setter.container).render('layout').done(function () {
            bodyElem = document.getElementById("LAY_app_body");
            newPage();
        });

        // 链接
        mui(document.body).on("tap", "[lay-href]", function () {
            location.hash = this.getAttribute("lay-href");
        });

        // 全局方法
        mui(document.body).on("tap", "[lay-adminevent]", function () {
            var event = this.getAttribute("lay-adminevent");
            console.log(event);
        });

        window.addEventListener("hashchange", function (e) {
            newPage();
        });
    };

    // 工具方法 -- 本地存储的封装
    $.store = function (tableName, options) {
        if (typeof options === "string") options = { key: options };
        if (!options || !options.key) return null;
        var data = localStorage.getItem(tableName);
        try {
            if (!data) {
                data = {};
            } else {
                data = JSON.parse(data);
            }
        } catch{
            data = {};
        }
        if (options.value) {
            data[options.key] = value;
            localStorage.setItem(tableName, JSON.stringify(data));
        } else if (options.remove) {
            delete data[options.key];
            localStorage.setItem(tableName, JSON.stringify(data));
        }
        return data[options.key] || null;
    };

    // 全局缓存
    $.cache = {

    };

})(mui, window);