
var htmlFunction = {
    "money": function (value) {
        if (!value) return "<label class='layui-text-gray'>N/A</label>";
        var num = Number(value);
        if (isNaN(num)) return value;
        if (num > 0) {
            return "<label class='layui-text-green'>+" + num + "元</label>";
        } else if (num < 0) {
            return "<label class='layui-text-red'>" + num + "元</label>";
        }
        return "<label class='layui-text-black'>0.00元</label>";;
    },
    "datetime": function (value) {
        if (!value || /^1900/.test(value)) return "N/A";
        if (/:\d{2}$/.test(value)) return value.replace(/:\d{2}$/, "");
        return value;
    },
    "time": function (value) {
        if (!value || /^1900/.test(value)) return "N/A";
        if (/\d{2}:.+$/.test(value)) return /\d{2}:.+$/.exec(value);
        return value;
    },
    "img": function (value) {
        if (!value) return "";
        return "<img src='" + value + "'/>";
    },
    "n": function (value) {
        if (!value) return "N/A";
        var num = Number(value);
        if (isNaN(num)) return value;
        return num.toFixed(2);
    },
    "enum": function (name) {
        if (!GolbalSetting.enum[name]) return;
        var options = new Array();
        layui.each(GolbalSetting.enum[name], function (key, value) {
            options.push("<option value='" + key + "'>" + value + "</option>");
        });
        return options.join("");
    },
    "p": function (value) {
        var num = Number(value);
        if (isNaN(num)) return value;
        return (num * 100).toFixed(2) + "%";
    },
    "date": function (value) {
        if (/^1900/.test(value)) return "N/A";
        var regex = /^\d+[/-]\d+[/-]\d+/;
        if (!regex.test(value)) return value;
        return regex.exec(value)[0];
    }
}


// 字符串自定义扩展
!function () {
    String.prototype.toHtml = function (data, fun) {
        /// <summary>
        /// 把HTMl模板内容与对象内容进行替换。
        /// </summary>
        /// <param name="data">Object 含值的对象</param>
        /// <param name="fun">单独处理对象</param>
        if (fun == undefined) {

            var _fun = null;

            var getData = function (obj, key) {
                var keyList = key.split(".");
                for (var i = 0; i < keyList.length; i++) {
                    obj = obj[keyList[i]];
                    if (!obj) return obj;
                }
                return obj;
            };

            fun = function (key) {
                var reg = /(.*?),(\d+)/;
                var obj = null;
                if (!reg.test(key)) {
                    var _funName = null;
                    var _fun = null;
                    if (key.contains(":")) {
                        var _funName = key.substring(key.indexOf(":") + 1);
                        key = key.substring(0, key.indexOf(":"));
                    }
                    obj = getData(data, key);
                    if (obj == undefined) return obj;
                    if (htmlFunction[_funName]) obj = htmlFunction[_funName].apply(this, [obj]);
                } else {
                    obj = data[reg.exec(key)[1]];
                    if (obj == undefined) return obj;
                    var length = reg.exec(key)[2].toInt();
                    while (obj.toString().length < length) {
                        obj = "0" + obj;
                    }
                }
                return obj;
            };
        }
        var str = this;
        return str.replace(/\$\{(.+?)\}/igm, function ($, $1) {
            var obj = null;
            switch (typeOf(data)) {
                case "element":
                    obj = data.get("data-" + $1.toLowerCase());
                    break;
                default:
                    obj = fun($1);
                    break;
            }
            if (obj == undefined || obj == null) return $;
            //if (/^[1-9][\d\.]+$/.test(obj)) obj = obj.toFloat();
            return obj != undefined ? obj : $;
        });
    };
}();

// 數字擴展
!function () {
    // 保留小數（捨棄）
    Number.prototype.fixed = function (length) {
        var num = this;
        if (length == undefined) length = 2;
        var result = (Math.floor(num * Math.pow(10, length)) / Math.pow(10, length)).toString();
        if (result.indexOf(".") == -1) result += ".";
        result += "0000";
        return result.substring(0, result.indexOf(".") + length+1);
    };
}();

// 數組擴展
!function () {
    // 數組轉換成為對象
    Array.prototype.toObject = function (key, value) {
        if (!value) value = function (item) { return item; };
        var data = new Object();
        var list = this;
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var keyName = typeof key == "function" ? key(item) : item[key];
            if (!keyName) continue;
            data[keyName] = value(item);
        }
        return data;
    };

}();