/// <reference path="tools.source.js" />
/*  頁面視圖擴展
 * zIndex : 100-200 
 * */
if (!window["moo"]) window["moo"] = {};

!function (ns) {

    ns.view = new Class({
        Implements: [Options, Events],
        options: {
        },
        elem: {
            zindex: 100,
            container: null,
            // 打开的视图 { id:id,elem:div.view,active:false }
            cache: [],
            views: {}
        },
        initialize: function (container, options) {
            var t = this;
            t.elem.container = container = document.id(container);
            t.setOptions(options);

            // 全局事件
            !function () {

            }();
        },
        // 开启一个新页面
        open: function (url, options) {
            var t = this;
            if (!options) options = {};
            if (!options.id) options.id = url.replace(/\//g, "-").replace(/\.\w+$/, "");

            var item = t.elem.cache.find(function (item) { return item.id == options.id; });
            if (item && item.active) return;

            if (!item) {
                var elem = new Element("div", {
                    "class": "moo-view " + (t.elem.cache.length == 0 ? "active" : ""),
                    "styles": {
                        "z-index": t.elem.zindex++
                    },
                    "id": options.id
                });
                item = {
                    id: options.id,
                    elem: elem,
                    done: options.done,
                    scroll: options.scroll,
                    data : options.data
                };
                item.elem.inject(t.elem.container);
                t.elem.cache.push(item);
                new Request({
                    url: url,
                    onSuccess: function (responseHTML) {
                        item.elem.innerHTML = responseHTML;
                        t.setActive(options.id);

                        // 滚动事件
                        !function () {
                            var scroll = item.elem.getElement("content.moo-scroll");
                            if (!scroll) return;

                            if (item.scroll) {
                                scroll.addEvent("scroll", function (e) {
                                    var pageHeight = Math.max(scroll.offsetHeight, scroll.scrollHeight);
                                    var viewportHeight = scroll.getHeight();
                                    var scrollHeight = scroll.scrollTop;
                                    if (viewportHeight + scrollHeight == pageHeight) {
                                        item.scroll.apply(item, [{ action: "bottom" }]);
                                    }
                                });
                            }
                        }();

                        if (item.done) item.done.apply(item, [options]);
                    }
                }).get();
            }

        },
        // 设置为当前页面
        setActive: function (id) {
            var t = this;
            var active = t.elem.cache.find(function (item) { return item.active; });
            if (active) {
                if (active.id == id) return;
                active.active = false;
                active.elem.removeClass("active");
            }

            var item = t.elem.cache.find(function (item) { return item.id == id; });
            if (!item) return;
            item.elem.addClass("active");
            item.active = true;
            var index = t.elem.cache.indexOf(item);
        },
        // 後退一個頁面
        back: function () {
            var t = this;
            if (t.elem.cache.length == 1) return;

            var last = t.elem.cache.getLast();
            last.elem.removeClass("active");
            last.elem.dispose.delay(250, last.elem);
            delete t.elem.views[last.id];
            t.elem.cache.pop();

            last = t.elem.cache.getLast();
            last.elem.addClass("active");
            last.active = true;
        }
    });

}(moo);