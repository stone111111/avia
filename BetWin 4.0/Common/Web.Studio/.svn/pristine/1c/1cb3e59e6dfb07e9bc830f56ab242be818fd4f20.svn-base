// 切换效果
if (!window["UI"]) window["UI"] = new Array();
!function (ns) {

    ns["Tab"] = new Class({
        Implements: [Events, Options],
        options: {
            // 容器
            "container": null,
            // 元素
            "list": []
        },
        data: {
            "el": null,
            // 外部容器的宽度
            "width": 0,
            // 内容的宽度
            "contentWidth": 0
        },
        initialize: function (el, options) {
            var t = this;
            t.data.el = el = $(el);
            t.data.width = el.getWidth().toInt();
            t.setOptions(options);
            t.options.list.each(function (item) {
                t.data.contentWidth += item.getWidth().toInt();
            });
            t.options.container.setStyle("width", t.data.contentWidth);
        },
        // 设置选中
        "select": function (ele) {
            var t = this;
            var index = t.options.list.indexOf(ele);
            if (index == -1 || t.data.contentWidth <= t.data.width) return;
            var width = 0;
            t.options.list.filter(function (item, itemIndex) {
                if (itemIndex >= index) return;
                width += item.getWidth().toInt();
            });
            var maxWidth = t.data.contentWidth - t.data.width;
            var best = width - t.data.width / 2 + ele.getWidth().toInt() / 2;
            if (best < 0) {
                base = 0;
            } else if (best > maxWidth) {
                best = maxWidth;
            }
            var left = t.data.el.scrollLeft;
            if (left == best) return;
            var step = (best - left) / 10
            var count = 0;
            var timer = setInterval(function () {
                count++;
                if (count == 10) { clearInterval(timer); return; }
                left += step;
                t.data.el.scrollTo(left, 0);
            }, 10);
        }
    });
}(UI);