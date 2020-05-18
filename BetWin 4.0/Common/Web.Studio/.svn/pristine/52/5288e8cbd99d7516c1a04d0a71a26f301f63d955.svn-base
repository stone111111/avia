/// <reference path="mootools.js" />
/// <reference path="mootools-more.source.js" />

if (!window["UI"]) window["UI"] = new Object();

(function (ns) {

    ns.Canvas = new Class({
        "Implements": [Events, Options],
        "options": {
            // 每秒多少帧
            "frame": 20
        },
        "dom": {
            "canvas": null,
            //2D运行对象
            "context": null,
            //当前运行的精灵
            "spirit": new Array(),
            "width": 0,
            "height": 0
        },
        "initialize": function (el, options) {
            var t = this;
            t.setOptions(options);
            t.dom.canvas = el = $(el);
            t.dom.context = t.dom.canvas.getConext("2d");
            t.dom.width = t.dom.canvas.get("width").toInt();
            t.dom.height = t.dom.canvas.get("height").toInt();
        },
        // 把精灵加载到场景中
        "show": function () {
            var t = this;
            // 清除场景
            t.dom.canvas.empty();
            t.dom.spirit.each(function (item, index) {

            });
        }
    });

    // 精灵对象
    ns.Spirit = new Class({
        "Implements": [Events, Options],
        "options": {
            // 资源路径
            "src": null,
            "width": 0,
            "height": 0,
            "x": 0,
            "y": 0,
            // 场景深度
            "index": 0
        },
        "initialize": function (options) {
            var t = this;
            t.setOptions(options);
        }
    });

})(UI);