/**
 * mui router   路由配置
 * @param {type} $
 * @returns {undefined}
 */

(function ($, window) {

    var setter = $.setter;

    var Class = function (path) {
        /*
         * returns 
         * #/set/provider/dns/ID=2
         * path: (3) ["set", "provider", "dns"]
         * search: {ID: "2"}
         * hash: ""
         * href: "/set/provider/dns/ID=2"
        */
        var that = this;
        var hash = path || location.hash;

        that.path = [];
        that.search = {};
        that.hash = (hash.match(/[^#](#.*$)/) || [])[1] || '';
        that.href = hash.replace(/^[#]/, "") || "/";

        //提取 Hash 结构（路径和参数)
        that.href.split('/').forEach(function (item, index) {
            /^\w+=/.test(item) ? function () {
                item = item.split('=');
                that.search[item[0]] = item[1];
            }() : function () {
                if (!item && index === 0) return;
                that.path.push(item || setter.entry);
            }();
        });
    };

    // 是否是设定的首页
    Class.prototype.isHomePage = function (href) {
        var that = this;
        href = href || that.href;
        var isHomePage = false;
        setter.homePage.forEach(function (item, index) {
            if (!isHomePage && item.length && item[0] === href) isHomePage = true;
        });
        return isHomePage;
    };

    $.router = function (path) {
        return new Class(path);
    };


})(mui, window);