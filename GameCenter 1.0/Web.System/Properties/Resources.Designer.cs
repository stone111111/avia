﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.System.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Web.System.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;root ID=&quot;dfbf69171ca5424dbfda5c3db85a1a05&quot;&gt;
        ///  &lt;menu name=&quot;首页&quot; href=&quot;/&quot; icon=&quot;am-icon-home&quot; ID=&quot;448a10b912cc45999e18b026c6838f1a&quot;&gt;&lt;/menu&gt;
        ///  &lt;menu name=&quot;商户管理&quot; icon=&quot;am-icon-adn&quot; ID=&quot;791442629d764214a65a0d4068a85d8c&quot;&gt;
        ///    &lt;menu name=&quot;商户列表&quot; href=&quot;site/list&quot; ID=&quot;1b92b2841b414a4aa5870a4ce36fd0cc&quot;&gt;
        ///      &lt;action name=&quot;商户详情&quot; ID=&quot;bdefb24f44454cb5a0d1dc611e02a49e&quot; /&gt;
        ///      &lt;action name=&quot;游戏设置&quot; ID=&quot;b44d4a8481b846a4809136e2903bd270&quot; /&gt;
        ///      &lt;action name=&quot;域名管理&quot; ID=&quot;42ba02f81d1245b2ac89ac6a6a9eb88a&quot; /&gt;
        ///      &lt;action nam [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string Permission {
            get {
                return ResourceManager.GetString("Permission", resourceCulture);
            }
        }
    }
}
