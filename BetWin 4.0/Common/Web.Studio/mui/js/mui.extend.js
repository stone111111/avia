// mui 的扩展方法



// 页面格式输出
if (!window["htmlFunction"]) window["htmlFunction"] = {};

!function (ns) {

    ns["money"] = function (value) {
        var num = typeof value === "string" ? Number(value) : value;
        if (!num) return value;
        if (num > 0) return "<span class='mui-text-green'>+" + num + "</span>";
        return "<span class='mui-text-red'>" + num + "</span>";
    }

}(htmlFunction);