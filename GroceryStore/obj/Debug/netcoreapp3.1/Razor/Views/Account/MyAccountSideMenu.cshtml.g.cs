#pragma checksum "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\Account\MyAccountSideMenu.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2dcebf7cfa964d09113144ad4ccb49d089f43eac"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_MyAccountSideMenu), @"mvc.1.0.view", @"/Views/Account/MyAccountSideMenu.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\_ViewImports.cshtml"
using GroceryStore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\_ViewImports.cshtml"
using GroceryStore.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2dcebf7cfa964d09113144ad4ccb49d089f43eac", @"/Views/Account/MyAccountSideMenu.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ddd50d0179ac9bea7de6074dacf685930840d048", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_MyAccountSideMenu : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<tblUsers>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div class=\"dash__box dash__box--bg-white dash__box--shadow u-s-m-b-30\">\r\n    <div class=\"dash__pad-1\">\r\n\r\n        <span class=\"dash__text u-s-m-b-16\">\r\n            Hello,\r\n            ");
#nullable restore
#line 7 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\Account\MyAccountSideMenu.cshtml"
        Write(Model.FirstName + " " + Model.LastName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </span>\r\n        <ul class=\"dash__f-list\">\r\n            <li>\r\n                <a class=\"dash-active\"");
            BeginWriteAttribute("href", " href=\"", 353, "\"", 394, 1);
#nullable restore
#line 11 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\Account\MyAccountSideMenu.cshtml"
WriteAttributeValue("", 360, Url.Action("MyAccount","Account"), 360, 34, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Manage My Account</a>\r\n            </li>\r\n            <li>\r\n                <a");
            BeginWriteAttribute("href", " href=\"", 474, "\"", 515, 1);
#nullable restore
#line 14 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\Account\MyAccountSideMenu.cshtml"
WriteAttributeValue("", 481, Url.Action("MyProfile","Account"), 481, 34, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">My Profile</a>\r\n            </li>\r\n            <li>\r\n\r\n                <a");
            BeginWriteAttribute("href", " href=\"", 590, "\"", 633, 1);
#nullable restore
#line 18 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\Account\MyAccountSideMenu.cshtml"
WriteAttributeValue("", 597, Url.Action("AddressBook","Account"), 597, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Address Book</a>\r\n            </li>\r\n            <li>\r\n\r\n                <a");
            BeginWriteAttribute("href", " href=\"", 710, "\"", 752, 1);
#nullable restore
#line 22 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\Account\MyAccountSideMenu.cshtml"
WriteAttributeValue("", 717, Url.Action("TrackOrder","Account"), 717, 35, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Track Order</a>\r\n            </li>\r\n            <li>\r\n\r\n                <a");
            BeginWriteAttribute("href", " href=\"", 828, "\"", 867, 1);
#nullable restore
#line 26 "C:\Users\ermft\source\repos\GroceryStore\GroceryStore\Views\Account\MyAccountSideMenu.cshtml"
WriteAttributeValue("", 835, Url.Action("MyOrder","Account"), 835, 32, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">My Orders</a>\r\n            </li>\r\n        </ul>\r\n    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<tblUsers> Html { get; private set; }
    }
}
#pragma warning restore 1591