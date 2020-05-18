// 切换效果
!function (ns) {
    ns["Tab"] = new Class({
        "Implements": [Events, Options],
        "options": {
            "nav": new Array(),
            "frame": new Array(),
            "activeName": "active",
            "event": "click",
            "display": 0
        },
        "initialize": function (options) {
            var t = this;
            t.setOptions(options);

            var nav_active = null;
            var frame_active = null;
            t.options.nav.each(function (nav, index) {
                nav.addEvent(t.options.event, function () {
                    if (nav_active == nav) return;
                    if (nav_active) nav_active.removeClass(t.options.activeName);
                    if (frame_active) frame_active.removeClass(t.options.activeName);
                    nav_active = nav;
                    frame_active = t.options.frame[index];
                    nav_active.addClass("active");
                    frame_active.addClass("active");
                    frame_active.fire.delay(100, frame_active);
                });
                if (index == t.options.display) {
                    nav.fireEvent(t.options.event);
                }
            });
        }
    });
}(BW);