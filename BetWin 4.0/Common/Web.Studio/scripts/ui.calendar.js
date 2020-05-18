if (!window["UI"]) window["UI"] = {};

(function (ns) {

    ns.Calendar = new Class({
        html: '<div class="UI-calendar-title"><a href="javascript:" class="UI-calendar-previousyear">«</a><a href="javascript:" class="UI-calendar-previousmonth">‹</a><input type="text" class="UI-calendar-year" size="4" /> 年<input type="text" class="UI-calendar-month" size="2" /> 月<a href="javascript:" class="UI-calendar-nextmonth">›</a><a href="javascript:" class="UI-calendar-nextyear">»</a></div><div class="UI-calendar-weeks"></div><div class="UI-calendar-month"></div>',
        Implements: [Events, Options],
        options: {
            date: new Date()
        },
        initialize: function (el, options) {
            el = $(el);
            if (el == null) return;
            $import("UI.Calendar.css", false);
            var t = this;
            t.setOptions(options);
            t.handler = new Element("div", {
                "class": "UI-calendar",
                "html": t.html,
                "events": {
                    "mousedown": function (e) { e.stopPropagation(); }
                }
            });
            ["日", "一", "二", "三", "四", "五", "六"].each(function (item) {
                new Element("span", { text: item, "class": "UI-calendar-item" }).inject(t.handler.getElement(".UI-calendar-weeks"));
            });
            t.handler.getElement(".UI-calendar-title > .UI-calendar-previousmonth").addEvent("click", function () { t.addMonth(-1); });
            t.handler.getElement(".UI-calendar-title > .UI-calendar-previousyear").addEvent("click", function () { t.addMonth(-12); });
            t.handler.getElement(".UI-calendar-title > .UI-calendar-nextmonth").addEvent("click", function () { t.addMonth(1); });
            t.handler.getElement(".UI-calendar-title > .UI-calendar-nextyear").addEvent("click", function () { t.addMonth(12); });
            t.handler.getElement(".UI-calendar-title > .UI-calendar-year").addEvents({
                "focus": function (e) {
                    this.store("value", this.get("value"));
                },
                "click": function (e) {
                    this.select();
                },
                "blur": function () {
                    if (!/^[1|2](\d{3})$/.test(this.get("value"))) {
                        this.set("value", this.retrieve("value"));
                        return;
                    }
                    t.options.date.setFullYear(this.get("value"));
                    t.getDate();
                }
            });

            t.handler.getElement(".UI-calendar-title > .UI-calendar-month").addEvents({
                "focus": function (e) {
                    this.store("value", this.get("value"));
                },
                "click": function (e) {
                    this.select();
                },
                "blur": function () {
                    if (!/^[1-9]$|^1[0-2]$/.test(this.get("value"))) {
                        this.set("value", this.retrieve("value"));
                        return;
                    }
                    t.options.date.setMonth(this.get("value").toInt() - 1);
                    t.getDate();
                }
            });

            t.element = el;
            t.getDate();

            el.addEvent("focus", t.onFocus.bind(t));
        },
        getDate: function () {
            var t = this;
            if (!t.options.date) t.options.date = new Date();
            var year = t.options.date.getFullYear();
            var month = t.options.date.getMonth() + 1;
            var day = t.options.date.getDate();
            t.handler.getElement("input.UI-calendar-year").set("value", year);
            t.handler.getElement("input.UI-calendar-month").set("value", month);
            t.handler.getElement("div.UI-calendar-month").set("html", "");
            for (var i = 1; i <= t.options.date.getLastDate() ; i++) {
                new Element("a", {
                    "href": "javascript:",
                    "text": i,
                    "class": "UI-calendar-item" + (i == day ? " on" : ""),
                    "events": {
                        "click": function () {
                            t.options.date.setDate(this.get("text"));
                            t.setValue();
                            t.fireEvent("selected", [t.options.date]);
                        }
                    }
                }).inject(t.handler.getElement("div.UI-calendar-month"));
            }
            for (var i = 0; i < (t.options.date.getFirstDay() % 7) ; i++) {
                new Element("span", {
                    "class": "UI-calendar-item"
                }).inject(t.handler.getElement("div.UI-calendar-month"), "top");
            }
            t.fillWeek();
        },
        // 渲染星期六和星期日的字体为红色
        fillWeek: function () {
            var t = this;
            var month = t.handler.getElement("div.UI-calendar-month");
            t.handler.getElements(".UI-calendar-item").each(function (item, index) {
                var day = index % 7;
                if (day == 0 || day == 6) item.setStyle("color", "#ff0000");
            });
        },
        // 给赋值
        setValue: function () {
            var t = this;
            var year = t.options.date.getFullYear();
            var month = t.options.date.getMonth() + 1;
            var date = t.options.date.getDate();
            var value = t.element.get("value");
            t.element.set("value", year + "-" + month + "-" + date);
            $(document.body).fireEvent("mousedown");
            if (value != t.element.get("value")) {
                t.element.fireEvent("change");
            }
        },
        // 增加或缺减少月份
        addMonth: function (step) {
            var t = this;
            t.options.date = new Date(t.options.date.getFullYear(), t.options.date.getMonth() + step, t.options.date.getDate());
            t.getDate();
        },
        // el 控件的获取焦点事件
        onFocus: function (e) {
            var t = this;
            var body = $(document.body);
            t.handler.inject(body);
            t.fillWeek();
            var date = e.target.get("value").toDate();
            if (date) { date.setMonth(date.getMonth() - 1); t.options.date = date; t.getDate(); }

            var bodyLeft = body.getSize().x;
            var left = t.element.getLeft() + t.handler.getSize().x;
            left = left > bodyLeft - 14 ? bodyLeft - 14 - t.handler.getSize().x : left - t.handler.getSize().x;
            t.handler.setStyles({
                "left": left,
                "top": t.element.getTop() + t.element.getSize().y
            });

            e.stopPropagation();
            body.addEvent("mousedown", t.onMouseDown.bind(t));
        },
        onMouseDown: function () {
            var t = this;
            t.handler.dispose();
            $(document.body).removeEvent("mousedown", t.onMouseDown);
        },
        // 选中日期之后的回调函数 对外开放
        selected: null
    });
})(UI);