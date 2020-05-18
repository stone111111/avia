using System;
using System.Collections.Generic;
namespace BW{
public static class Permission{
public static readonly Dictionary<string, string> NAME = new Dictionary<string, string>(){ {"448a10b912cc45999e18b026c6838f1a","首页"},
{"bdefb24f44454cb5a0d1dc611e02a49e","商户详情.商户列表.商户管理"},
{"b44d4a8481b846a4809136e2903bd270","游戏设置.商户列表.商户管理"},
{"42ba02f81d1245b2ac89ac6a6a9eb88a","域名管理.商户列表.商户管理"},
{"4cad625e53c5496797e89287e06aca5b","证书管理.商户列表.商户管理"},
{"21b23934cf8242da95d3343011405879","安全设置.商户列表.商户管理"},
{"b53299289b4a45e0a218b55cd23f5c05","模板配置.商户列表.商户管理"},
{"43032f835b414e79a413605882bd237e","商户报表.商户列表.商户管理"},
{"1b92b2841b414a4aa5870a4ce36fd0cc","商户列表.商户管理"},
{"0205d3e55ef941fd81cc0da0c54e0574","会员列表.商户管理"},
{"7a5b5f47840d423d9d61f8530f683038","条件配置.商户管理"},
{"584692b7cabf4d209f2a90366c104496","游戏订单.商户管理"},
{"40e514995f994f27837717b9654f5b8f","商户报表.商户管理"},
{"791442629d764214a65a0d4068a85d8c","商户管理"},
{"52d562ec4b0f400f9749f4de3c561bcb","视图管理.视图模板"},
{"4710ad11afd34927816e9e9adfe982d0","模板管理.视图模板"},
{"7e730d16ca2c4498b8082631eacc38d8","视图模板"},
{"53e9cb20f5d94ef28f180bc4d6052501","客户留言.运营报表"},
{"e52d375c8b4c4de7adee564b486274a9","商户账单.运营报表"},
{"0c5d8b7a8e2c4e2c85e95afa7ccfd8a5","报表管理.运营报表"},
{"96d30bf5cad34e58b857e661bfad63a1","运营报表"},
{"2b68b2d8521b432b98f7f66a98529002","账号管理.系统配置"},
{"1287d1904d1540be8c4d8d25334bde7c","充值接口.系统配置"},
{"bad0a783149b4e85a4c09c554e9c9aa9","提现接口.系统配置"},
{"f0035a1c24994feabf0635597887a332","系统公告.系统配置"},
{"de860eaafb834b8f85503f86db247e60","游戏配置.系统配置"},
{"9b5af3a3093340499e21b1f19b9d9ee5","会员报表.报表配置.系统配置"},
{"defa2ab7eec344f3876c15ccb899cec7","商户报表.报表配置.系统配置"},
{"7fba3625dee64ad380e816ab3d5df52b","平台报表.报表配置.系统配置"},
{"430b8a07acbc420ea76a14263ad6d08f","报表配置.系统配置"},
{"2cfbc3ee7f364cc8881db0f95501a6c4","系统配置"},
{"dc77af78cf094049930d779e908b9fb3","参数配置.系统运维"},
{"59b7960d009a4dd68efcafef554304d9","系统监控.系统运维"},
{"ad0d0d84a1a04cc1b38e57a160478c82","操作日志.日志管理.系统运维"},
{"15155b07a1d54906967731798fa1287a","错误日志.日志管理.系统运维"},
{"76cded50d58f4dfd99118e38c674cf72","游戏日志.日志管理.系统运维"},
{"55351b7f30ce445db752fa1ff913207f","支付支持.日志管理.系统运维"},
{"fc3b53aa051c4a1387a40b6eab520c57","提现日志.日志管理.系统运维"},
{"bdc821d4fe4546058d60527a3b64a3da","日志管理.系统运维"},
{"f01567ac14ab40179b1d4a6f99338c5c","CDN供应商.供应商管理.系统运维"},
{"ced2e4158157476cb6da050e00049a85","DNS供应商.供应商管理.系统运维"},
{"ac903510557b4c2d870de88af2a02e28","短信供应商.供应商管理.系统运维"},
{"9cb1da08abeb4b1f8acadc060e2b0cf4","游戏供应商.供应商管理.系统运维"},
{"8e13fd4407cc4c69bc1b708d5c20ab04","供应商管理.系统运维"},
{"1bf0652997934515a40b2b1b8db87c0a","系统运维"} };
 public static class 首页   { 
public const string Value = "448a10b912cc45999e18b026c6838f1a";
}
 public static class 商户管理   { 
public const string Value = "791442629d764214a65a0d4068a85d8c";
 public static class 商户列表   { 
public const string Value = "1b92b2841b414a4aa5870a4ce36fd0cc";
public const string 商户详情 = "bdefb24f44454cb5a0d1dc611e02a49e";public const string 游戏设置 = "b44d4a8481b846a4809136e2903bd270";public const string 域名管理 = "42ba02f81d1245b2ac89ac6a6a9eb88a";public const string 证书管理 = "4cad625e53c5496797e89287e06aca5b";public const string 安全设置 = "21b23934cf8242da95d3343011405879";public const string 模板配置 = "b53299289b4a45e0a218b55cd23f5c05";public const string 商户报表 = "43032f835b414e79a413605882bd237e";}
 public static class 会员列表   { 
public const string Value = "0205d3e55ef941fd81cc0da0c54e0574";
}
 public static class 条件配置   { 
public const string Value = "7a5b5f47840d423d9d61f8530f683038";
}
 public static class 游戏订单   { 
public const string Value = "584692b7cabf4d209f2a90366c104496";
}
 public static class 商户报表   { 
public const string Value = "40e514995f994f27837717b9654f5b8f";
}
}
 public static class 视图模板   { 
public const string Value = "7e730d16ca2c4498b8082631eacc38d8";
 public static class 视图管理   { 
public const string Value = "52d562ec4b0f400f9749f4de3c561bcb";
}
 public static class 模板管理   { 
public const string Value = "4710ad11afd34927816e9e9adfe982d0";
}
}
 public static class 运营报表   { 
public const string Value = "96d30bf5cad34e58b857e661bfad63a1";
 public static class 客户留言   { 
public const string Value = "53e9cb20f5d94ef28f180bc4d6052501";
}
 public static class 商户账单   { 
public const string Value = "e52d375c8b4c4de7adee564b486274a9";
}
 public static class 报表管理   { 
public const string Value = "0c5d8b7a8e2c4e2c85e95afa7ccfd8a5";
}
}
 public static class 系统配置   { 
public const string Value = "2cfbc3ee7f364cc8881db0f95501a6c4";
 public static class 账号管理   { 
public const string Value = "2b68b2d8521b432b98f7f66a98529002";
}
 public static class 充值接口   { 
public const string Value = "1287d1904d1540be8c4d8d25334bde7c";
}
 public static class 提现接口   { 
public const string Value = "bad0a783149b4e85a4c09c554e9c9aa9";
}
 public static class 系统公告   { 
public const string Value = "f0035a1c24994feabf0635597887a332";
}
 public static class 游戏配置   { 
public const string Value = "de860eaafb834b8f85503f86db247e60";
}
 public static class 报表配置   { 
public const string Value = "430b8a07acbc420ea76a14263ad6d08f";
 public static class 会员报表   { 
public const string Value = "9b5af3a3093340499e21b1f19b9d9ee5";
}
 public static class 商户报表   { 
public const string Value = "defa2ab7eec344f3876c15ccb899cec7";
}
 public static class 平台报表   { 
public const string Value = "7fba3625dee64ad380e816ab3d5df52b";
}
}
}
 public static class 系统运维   { 
public const string Value = "1bf0652997934515a40b2b1b8db87c0a";
 public static class 参数配置   { 
public const string Value = "dc77af78cf094049930d779e908b9fb3";
}
 public static class 系统监控   { 
public const string Value = "59b7960d009a4dd68efcafef554304d9";
}
 public static class 日志管理   { 
public const string Value = "bdc821d4fe4546058d60527a3b64a3da";
 public static class 操作日志   { 
public const string Value = "ad0d0d84a1a04cc1b38e57a160478c82";
}
 public static class 错误日志   { 
public const string Value = "15155b07a1d54906967731798fa1287a";
}
 public static class 游戏日志   { 
public const string Value = "76cded50d58f4dfd99118e38c674cf72";
}
 public static class 支付支持   { 
public const string Value = "55351b7f30ce445db752fa1ff913207f";
}
 public static class 提现日志   { 
public const string Value = "fc3b53aa051c4a1387a40b6eab520c57";
}
}
 public static class 供应商管理   { 
public const string Value = "8e13fd4407cc4c69bc1b708d5c20ab04";
 public static class CDN供应商   { 
public const string Value = "f01567ac14ab40179b1d4a6f99338c5c";
}
 public static class DNS供应商   { 
public const string Value = "ced2e4158157476cb6da050e00049a85";
}
 public static class 短信供应商   { 
public const string Value = "ac903510557b4c2d870de88af2a02e28";
}
 public static class 游戏供应商   { 
public const string Value = "9cb1da08abeb4b1f8acadc060e2b0cf4";
}
}
}
}}
