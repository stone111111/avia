window.onload = function () {
    var flv = {
        "host": "http://a8.to/Handler/VR.ashx?url=http://vr.a8.to/",
        "VRVenus": "live/c1.flv",
        "VRMars": "live/d1.flv",
        "VR3": "live/a1.flv",
        "VRRacing": "live/b1.flv",
        "VRBoat": "live/e1.flv",
        "VRBaccarat": "live/f1.flv",
        "VRRace": "live/horse.flv",
        "VRSwim": "live/swim.flv",
        "VRBike": "live/bike.flv"
    };

    var m3u8 = {
        "host": "//phone.videoipdata.com:8096/",
        "VRVenus": "live/c1.m3u8",
        "VRMars": "live/d1.m3u8",
        "VR3": "live/a1.m3u8",
        "VRRacing": "live/b1.m3u8",
        "VRBoat": "live/e1.m3u8"
    };

    var query = location.search.substr(1);
    var video = /Type=(\w+)/.exec(location.search)[1];
    var videoElement = document.getElementById('videoElement');

    var play = function (url) {
        var player = new ckplayer({
            container: '#video',//“#”代表容器的ID，“.”或“”代表容器的class
            variable: 'player',//该属性必需设置，值等于下面的new chplayer()的对象
            "autoplay": true,
            "live": true,
            video: url //视频地址
            //http://pgs.live-vr.ar02.cn/live/horse.flv?wsSecret=05611b8595a339f29865fbefd589743c&wsABSTime=5a9e20ff
        });
    };

    !function () {
        var url = /^http/.test(flv[video]) ? flv[video] : flv.host + flv[video];

        if (url.indexOf("${VRKey}") !== -1) {
            new Request({
                "url": "//a8.to/handler/config.ashx",
                "noCache": true,
                "onSuccess": function (result) {
                    if (result) {
                        url = url.replace("${VRKey}", result);
                        play(url);
                    }
                }
            }).get({
                "key": "wsSecret"
            });
        } else {
            play(url);
        }
    }();
};