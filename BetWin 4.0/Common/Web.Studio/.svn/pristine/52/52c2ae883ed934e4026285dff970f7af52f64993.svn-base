// 显示结果
function ShowResult(data, count) {
    var result = new Array();
    result.push("<table border=1><thead><tr><th>内容</th><th>赔率</th><th>概率</th></tr></thead><tbody>");
    for (var name in data) {
        var value = data[name];

        result.push("<tr><td>" + name + "</td><td>" + (count / value).toFixed(4) + "倍" + "</td><td>" + value + "/" + count + "</td></tr>");
    }
    result.push("</tbody></table>");

    document.writeln(result.join("\n"));
}