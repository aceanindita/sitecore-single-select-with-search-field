using Sitecore;
using Sitecore.Buckets.FieldTypes;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;

namespace SharedSource.Customizations
{
    public class SingleSelectWithSearch : BucketList
    {
        protected override void DoRender(HtmlTextWriter output)
        {
            ArrayList selected;
            OrderedDictionary unselected;
            GetSelectedItems(GetItems(Sitecore.Context.ContentDatabase.GetItem(ItemID)), out selected, out unselected);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (DictionaryEntry dictionaryEntry in unselected)
            {
                Item obj = dictionaryEntry.Value as Item;
                if (obj != null)
                {
                    stringBuilder.Append(obj.DisplayName + ",");
                    stringBuilder.Append(GetItemValue(obj) + ",");
                }
            }
            RenderStartLocationInput(output);
            output.Write("<input type='hidden' width='100%' id='multilistValues" + (object)ClientID + "' value='" + stringBuilder + "' style='width: 200px;margin-left:3px;'>");
            ServerProperties["ID"] = ID;
            string str1 = string.Empty;
            if (ReadOnly)
                str1 = " disabled='disabled'";
            output.Write("<input id='" + ID + "_Value' type='hidden' value='" + StringUtil.EscapeQuote(Value) + "' />");
            output.Write("<table" + GetControlAttributes() + ">");
            output.Write("<tr>");
            output.Write("<td class='scContentControlMultilistCaption' width='50%' colspan='4'>" + Translate.Text("All") + "</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td valign='top' height='100%' colspan='4'>");
            output.Write("<div style='width:200%;overflow:hidden;height:30px'><input type='text' width='100%' class='scIgnoreModified bucketSearch inactive' value='" + TypeHereToSearch + "' id='filterBox" + ClientID + "' " + (Sitecore.Context.ContentDatabase.GetItem(ItemID).Access.CanWrite() ? string.Empty : "disabled") + ">");
            output.Write("<span id='prev" + ClientID + "' class='hovertext' style='cursor:pointer;' onMouseOver=\"this.style.color='#666'\" onMouseOut=\"this.style.color='#000'\"> <img width='10' height='10' src='/sitecore/shell/Applications/Buckets/images/right.png' style='margin-top: 1px;'> " + Translate.Text("prev") + " |</span>");
            output.Write("<span id='next" + ClientID + "' class='hovertext' style='cursor:pointer;' onMouseOver=\"this.style.color='#666'\" onMouseOut=\"this.style.color='#000'\"> " + Translate.Text("next") + " <img width='10' height='10' src='/sitecore/shell/Applications/Buckets/images/left.png' style='margin-top: 1px;'>  </span>");
            output.Write("<span id='refresh" + ClientID + "' class='hovertext' style='cursor:pointer;' onMouseOver=\"this.style.color='#666'\" onMouseOut=\"this.style.color='#000'\"> " + Translate.Text("refresh") + " <img width='10' height='10' src='/sitecore/shell/Applications/Buckets/images/refresh.png' style='margin-top: 1px;'>  </span>");
            output.Write("<span id='goto" + ClientID + "' class='hovertext' style='cursor:pointer;' onMouseOver=\"this.style.color='#666'\" onMouseOut=\"this.style.color='#000'\"> " + Translate.Text("go to item") + " <img width='10' height='10' src='/sitecore/shell/Applications/Buckets/images/text.png' style='margin-top: 1px;'>  </span>");
            output.Write("<span style='padding-left:34px;'><strong>" + Translate.Text("Page Number") + ": </strong></span><span id='pageNumber" + ClientID + "'></span></div>");
            string str2 = !UIUtil.IsIE() || UIUtil.GetBrowserMajorVersion() != 9 ? "10" : "11";
            output.Write("<select id=\"" + ID + "_unselected\" class=\"scContentControlMultilistBox\" size=\"" + str2 + "\"" + str1 + " >");
            foreach (DictionaryEntry dictionaryEntry in unselected)
            {
                Item obj = dictionaryEntry.Value as Item;
                if (obj != null)
                {
                    string str3 = OutputString(obj);
                    output.Write("<option value='" + GetItemValue(obj) + "'>" + str3 + "</option>");
                }
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td class='scContentControlMultilistCaption' width='100%'>Selected</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td valign='top' height='100%' colspan='4'>");
            output.Write("<select id='" + ID + "_selected' class='scContentControlMultilistBox scSingleSelectWithSearchSelectedBox' size='10'" + str1 + ">");
            for (int index = 0; index < selected.Count; ++index)
            {
                Item obj1 = selected[index] as Item;
                if (obj1 != null)
                {
                    string str3 = OutputString(obj1);
                    output.Write("<option value='" + GetItemValue(obj1) + "'>" + str3 + "</option>");
                }
                else
                {
                    string path = selected[index] as string;
                    if (path != null)
                    {
                        Item obj2 = Sitecore.Context.ContentDatabase.GetItem(path);
                        string str3 = obj2 == null ? path + ' ' + Translate.Text("[Item not found]") : OutputString(obj2);
                        output.Write("<option value='" + path + "'>" + str3 + "</option>");
                    }
                }
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("<div style='border:1px solid #999999;font:8pt tahoma;display:none;padding:2px;margin:4px 0px 4px 0px;height:14px' id='" + ID + "_all_help'></div>");
            output.Write("<div style='border:1px solid #999999;font:8pt tahoma;display:none;padding:2px;margin:4px 0px 4px 0px;height:14px' id='" + ID + "_selected_help'></div>");
            output.Write("</table>");
            RenderScript(output);
        }

        protected override void RenderScript(HtmlTextWriter output)
        {
            string str = "<script type='text/javascript'>\r\n                                    (function() {\r\n                                        if (!document.getElementById('SingleSelectWithSearchJs')) {\r\n                                            var head = document.getElementsByTagName('head')[0];\r\n                                            head.appendChild(new Element('script', { type: 'text/javascript', src: '/sitecore/shell/Controls/SingleSelectWithSearch/SingleSelectWithSearch.js', id: 'SingleSelectWithSearchJs' }));\r\n                                            head.appendChild(new Element('link', { rel: 'stylesheet', href: '/sitecore/shell/Controls/SingleSelectWithSearch/SingleSelectWithSearch.css' }));\r\n                                        }\r\n                                        var stopAt = Date.now() + 5000;\r\n                                        var timeoutId = setTimeout(function() {\r\n                                            if (Sitecore.InitSingleSelectWithSearch) {\r\n                                                Sitecore.InitSingleSelectWithSearch(" + ScriptParameters + ");\r\n                                                clearTimeout(timeoutId);\r\n                                            } else if (Date.now() > stopAt) {\r\n                                                clearTimeout(timeoutId);\r\n                                            }\r\n                                        }, 100);\r\n                                    }());\r\n                              </script>";
            output.Write(str);
        }
    }
}
