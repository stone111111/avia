/* 与业务关联的view 继承 */

!function ($, view) {
    var setter = $.setter;

    var Class = function () {

    };

    var admin = function () {
        return new Class();
    };

    Class.prototype.req = function (options) {
        view.req(options);
    };

    // 清除本地登录信息并且跳转到登录页面
    admin.exit = function () {
        //清空本地记录的 token
        $.store(setter.tableName, {
            key: setter.request.tokenName
            , remove: true
        });

        if (setter.logout) setter.logout();
    };

    // 弹出框
    Class.prototype.popup = function (views, options) {
        var that = this;
        if (!options) options = {};

        // 开启遮罩层
        that.shadeElem = null;
        if (options.shade) {
            that.shadeElem = document.createElement("view-mask");
            that.shadeElem.style.zIndex = setter.viewIndex++;
            document.body.appendChild(that.shadeElem);
            if (options.shadeClose) {
                that.shadeElem.addEventListener("tap", function () {
                    that.close();
                });
            }
        }

        var elem = document.createElement("view");
        elem.style.zIndex = setter.viewIndex++;
        elem.className = "layui-layer ready";
        // 准备开始
        if (options.type) {
            elem.classList.add("layui-layer-" + options.type);
            delete options.type;
        }

        if (options.anim) {
            elem.classList.add("layui-anim-" + options.anim);
            delete options.anim;
        }

        var then = null;
        if (options.then) {
            then = options.then;
            delete options.then;
        }
        document.body.appendChild(elem);
        that.view = view(elem).render(views, options).then(function () {
            that.view = this;
            that.view.container.classList.remove("ready");
            that.view.events = options.events;
            that.view.close = that.close;
            if (then) then.apply(that);
        });

        return that;
    };

    // 关闭窗体
    Class.prototype.close = function () {
        var that = this;
        if (that.shadeElem) {
            document.body.removeChild(that.shadeElem);
        }
        that.view.container.classList.add("closing");
        setTimeout(function () {
            document.body.removeChild(that.view.container);
        }, 250);
    };

    //关闭窗体

    // 对外抛出
    $.admin = admin;

}(mui, mui.view);