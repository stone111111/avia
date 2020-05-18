// 已经打开的窗口
var frames = new Object();
var Frame = new Class({
    "Implements": [Events, Options],
    "options": {
        "name": null,
        "id": null,
        "href": null
    },
    "dom": {
        "content": null,
        "items": null,
        "taskbar": null,
        // 当前窗口
        "item": null,
        // 当前任务条
        "task": null
    },
    "initialize": function (options) {
        var t = this;
        t.setOptions(options);
        if (frames[t.options.id]) {
            frames[t.options.id].Open();
            return;
        }

        t.dom.content = $("frame-content");
        t.dom.items = $("frame-items");
        t.dom.taskbar = $("frame-taskbar")

        t.dom.item = new Element("div", {
            "class": "frame-item",
            "data-id": t.options.id,
            "data-bind-type": "control",
            "data-bind-action": t.options.href,
            "data-bind-callback": "enum,tip"
        });

        t.dom.item.inject(t.dom.items);

        t.dom.task = new Element("div", {
            "class": "frame-task",
            "data-id": t.options.id,
            "events": {
                "click": function () {
                    t.Open();
                }
            }
        });

        new Element("span", {
            "text": t.options.name
        }).inject(t.dom.task);

        new Element("a", {
            "href": "javascript:",
            "class": "am-icon-close am-icon-btn",
            "events": {
                "click": function (e) {
                    e.stopPropagation();
                    t.Close();
                }
            }
        }).inject(t.dom.task);

        t.dom.task.inject(t.dom.taskbar);

        frames[t.options.id] = t;

        if (!t.dom.taskbar.get("data-event-move")) {
            $$("#frame-taskbar-list > a").addEvent("click", function () {
                if (this.hasClass("task-left")) t.showTask("left");
                if (this.hasClass("task-right")) t.showTask("right");
            });
            t.dom.taskbar.set("data-event-move", true);
        }
    },
    "Open": function () {
        var t = this;
        sessionStorage.setItem("frame", t.options.id);
        t.resize();

        t.dom.taskbar.getElements("[data-id]").each(function (item) {
            if (item.get("data-id") == t.options.id) {
                item.addClass("current");
            } else {
                item.removeClass("current");
            }
        });
        new BW.BindEvent(t.dom.item);
        t.taskbar();
        t.showTask();
        Object.each(frames, function (frame, key) {
            if (key == t.options.id) {
                frame.dom.item.addClass("current");
            } else {
                frame.dom.item.removeClass("current");
            }
        });
    },
    "Close": function () {
        var t = this;
        var next = null;
        next = t.dom.task.getNext(".frame-task");
        if (next == null) next = t.dom.task.getPrevious(".frame-task");

        t.dom.task.dispose();
        t.dom.item.dispose();
        frames[t.options.id] = t = null;

        if (next != null) frames[next.get("data-id")].Open();
    },
    "resize": function () {
        var t = this;
        var size = t.dom.content.getSize();
        var width = 0;
        var index = 0;
        var itemIndex = 0;
        Object.forEach(frames, function (value, key) {
            if (!value) return;
            width += size.x;
            value.dom.item.setStyles({
                "width": size.x,
                "height": UI.getSize().y - 48
            });
            if (value == t) itemIndex = index;
            index++;
        });
        t.dom.items.setStyles({
            "width": width,
            "margin-left": t.dom.item.getAllPrevious().length * size.x * -1
        });
    },
    // 设置状态栏的位置
    "taskbar": function () {
        var t = this;
        var list = t.dom.taskbar.getElements(".frame-task");
        t.dom.taskbar.setStyles({
            "width": list.length * 100
        });
        var current = t.dom.taskbar.getElement(".current");
        if (current == null) return;

    },
    // 显示状态栏任务
    "showTask": function (action) {
        var t = this;
        var taskbar = $("frame-taskbar");
        var container = taskbar.getParent();
        var containerWidth = container.getStyle("width").toInt();
        var taskbarWitdh = taskbar.getStyle("width").toInt();

        if (taskbarWitdh < containerWidth) return;
        var marginLeft = taskbar.getStyle("margin-left").toInt();
        var minLeft = containerWidth - taskbarWitdh;

        switch (action) {
            case "right":
                taskbar.setStyle("margin-left", Math.max(marginLeft - 100, minLeft));
                break;
            case "left":
                taskbar.setStyle("margin-left", Math.min(marginLeft + 100, 0));
                break;
            default:
                var current = t.dom.taskbar.getElement(".current");
                if (current == null) return;
                var currentLeft = (current.getPosition(taskbar).x - containerWidth / 2) * -1;

                if (currentLeft > 0) currentLeft = 0;
                if (currentLeft < minLeft) currentLeft = minLeft;
                taskbar.setStyle("margin-left", currentLeft);

                break;
        }

    }
});
// 打开一个窗口
var openFrame = function (name, href, id) {
    var frame = frames[id];
    if (!frame) {
        frame = new Frame({
            "name": name,
            "href": href,
            "id": id
        });
    }
    frame.Open();
}

