if (!window["UI"]) window["UI"] = {};

layui.use(["jquery", "form", "table"], function () {
    var apiurl = "//" + location.host + "/";
    var $ = layui.$,
        form = layui.form,
        table = layui.table;

    $.ajax({
        url: apiurl + "System/Config/Init",
        method: "post",
        success: function (res) {

            form.set({
                upload: {
                    url: apiurl + "System/Config/LayUpload",
                    kindeditor: apiurl + "System/Config/Kindupload"
                }
            });

            table.set({
                "method": "post",
                "limits": [20, 50, 100, 250, 500, 1000],
                "headers": {
                    "Authorization": layui.data("BW4").Authorization
                }
            });

            GolbalSetting.enum = res.info.Enum;
            GolbalSetting.ImgServer = res.info.ImgServer;

            UI.GetImage = function (src) {
                if (!src) return GolbalSetting.ImgServer + "/images/space.png";
                if (/^http/.test(src)) return src;
                return GolbalSetting.ImgServer + src;
            };

            layui.config({
                base: "//" + location.host + '/src/',
                apiurl: apiurl,
                version: new Date().getTime()
            }).use('index');
        }
    });
});

// 公共弹出框
!function (ns) {

    // 商户的资料弹出框
    ns.Site = function (site, siteName) {
        var siteId = 0;
        if (typeof site === "number") {
            siteId = site;
        } else {
            siteId = site.SiteID;
            siteName = site.SiteName;
        }
        if (!siteId) return;
        if (!siteName) siteName = siteId;

        layui.use(["admin", "view"], function () {
            var admin = layui.admin,
                view = layui.view;

            admin.popup({
                title: siteId + " / " + siteName,
                area: GolbalSetting.area.xl,
                shadeClose: false,
                id: "common-viewsite-" + siteId,
                skin: "diag site",
                content: "正在加载...",
                success: function () {
                    view(this.id).render("diag/site/index", {
                        SiteID: siteId
                    });
                }
            });
        });
    };

    // 设定编辑模式
    ns.SetEditMode = function (formObj) {
        if (typeof formObj === "string") formObj = document.getElementById(formObj);
        if (!formObj) return;
        formObj.classList.add("editmode");
        var header = formObj.querySelector(".layui-card-header");

        layui.use(["betwin"], function () {
            var $ = layui.$,
                betwin = layui.betwin;

            var id = formObj.getAttribute("id");
            $(header).append('<button type="button" data-edit="edit" class="layui-btn layui-btn-right layui-btn-primary"><i class="am-icon-edit"></i> 编辑</button>');
            $(header).append('<button type="reset" data-edit="reset" class="layui-btn layui-btn-right layui-btn-primary"><i class="am-icon-reply"></i> 取消</button>');
            $(header).append('<button type="submit" data-edit="save" class="layui-btn layui-btn-right" lay-submit lay-filter="' + id + '-submit"><i class="am-icon-save"></i> 保存</button>');

            betwin.form.render(formObj.getAttribute("lay-filter"));

            var editMode = function (mode) {
                mode ? formObj.removeAttribute("readonly") : formObj.setAttribute("readonly", "readonly");
                var elements = formObj.querySelectorAll("input,select,textarea");
                for (var i = 0; i < elements.length; i++) {
                    var elem = elements[i];
                    switch (elem.tagName + ":" + (elem.getAttribute("type") || "")) {
                        case "INPUT:text":
                        case "TEXTAREA:":
                            mode ? elem.removeAttribute("readonly") : elem.setAttribute("readonly", "readonly");
                            break;
                        case "INPUT:checkbox":
                        case "SELECT:":
                            mode ? elem.removeAttribute("disabled") : elem.setAttribute("disabled", "disabled");
                            break;
                    }
                }
                betwin.form.render(formObj.getAttribute("lay-filter"));
            };

            editMode(false);

            $(formObj).on("click", "button[data-edit]", function (e) {
                var action = this.getAttribute("data-edit");
                switch (action) {
                    case "edit":
                        editMode(true);
                        break;
                    case "reset":
                        editMode(false);
                        break;
                }
            });

            betwin.form.submit(id + "-submit",
                function (data) {
                    var checkbox = formObj.querySelectorAll("input[type='checkbox']");
                    for (var i = 0; i < checkbox.length; i++) {
                        var elem = checkbox[i];
                        var name = elem.getAttribute("name");
                        if (!data[name] || typeof data[name] === "string") {
                            data[name] = [];
                        }
                        if (elem.checked) data[name].push(elem.value);
                    }
                    for (var key in data) {
                        if (Array.isArray(data[key])) {
                            data[key] = data[key].join(",");
                        }
                    }
                    return data;
                }, {
                success: function (res) {
                    editMode(false);
                    return false;
                }
            });
        });
    };

}(UI);

// 工具类
!function (ns) {

    // 复制到剪切板
    ns.Clipboard = function (content, tip) {
        if (!tip) tip = "已复制到剪切板";

        var textarea = document.createElement("textarea");
        textarea.style.position = "absolute";
        textarea.value = content || "";
        document.body.appendChild(textarea);
        textarea.select();
        document.execCommand('copy');
        document.body.removeChild(textarea);

        layer.msg(tip);
    };

}(Utils);

!function (ns) {

    // 显示站点信息
    ns.Site = function (site, siteName) {
        var siteId = 0;
        if (typeof site === "number") {
            siteId = site;
        } else {
            siteId = site.SiteID;
            siteName = site.SiteName;
        }
        var html = [];
        html.push("<a href=\"javascript:UI.Site(" + siteId + ",'" + siteName + "');\" class=\"link-blue\">" + siteId + "</a>");
        if (siteName !== siteId) {
            html.push("<hr class='xs' />");
            html.push("<a href=\"javascript:UI.Site(" + siteId + ",'" + siteName + "');\" class=\"link-blue\">" + siteName + "</a>");
        }
        return html.join("");
    };

}(htmlFunction);