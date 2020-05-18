layui.define(['laytpl', 'layer', 'element', 'util'], function (exports) {
    exports('setter', {
        container: 'LAY_app' //容器ID
        , base: layui.cache.base //记录layuiAdmin文件夹所在路径
        , views: layui.cache.base + 'views/' //视图所在目录
        , apiurl: layui.cache.apiurl || null   // API接口的跨域地址
        , apipath: "x-path"    // API接口请求的路径字段名字，如果开启则放置于header中
        , entry: 'index' //默认视图文件名
        , engine: '.html' //视图文件后缀名
        , pageTabs: true //是否开启页面选项卡功能。单页版不推荐开启

        , name: 'BetWin 4.0'
        , tableName: 'BW4' //本地存储表名
        , MOD_NAME: 'admin' //模块事件名

        , debug: true //是否开启调试模式。如开启，接口异常时会抛出异常 URL 等信息

        , interceptor: true //是否开启未登入拦截

        //自定义请求字段
        , request: {
            tokenName: "Authorization" //自动携带 token 的字段名。可设置 false 不携带。
        }

        //自定义响应字段
        , response: {
            statusName: 'success' //数据状态的字段名称
            , statusCode: {
                ok: 1 //数据状态一切正常的状态码
                , logout: function (res) {
                    if (res.success === 0 && res.info) {
                        switch (res.info.Error) {
                            case "Permission":
                                layer.msg(res.msg, { icon: 2 });
                                break;
                        }
                        return res.info.Error === "Login";
                    }
                }//登录状态失效的状态码
            }
            , msgName: 'msg' //状态信息的字段名称
            , dataName: 'info' //数据详情的字段名称
        }

        //独立页面路由，可随意添加（无需写参数）
        , indPage: [
            '/Login',
            '/Register'
        ]

        //扩展的第三方模块
        , extend: [
            'echarts', //echarts 核心包
            'echartsTheme' //echarts 主题
        ]

        //主题配置
        , theme: {
            //内置主题配色方案
            color: [{
                main: '#03152A'
                , selected: '#3B91FF'
                , alias: 'default' //藏蓝
            }]

            //初始的颜色索引，对应上面的配色方案数组索引
            //如果本地已经有主题色记录，则以本地记录为优先，除非请求本地数据（localStorage）
            , initColorIndex: 0
        }
    });
});
