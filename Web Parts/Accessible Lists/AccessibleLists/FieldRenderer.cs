using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPCLF3.WebParts
{
    public static class FieldRenderer
    {
        public static string RenderField(string fieldValue, SPField field)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            try
            {
                SPFieldType type = field.Type;
                switch (type)
                {
                    // Nik20121108 - Handles both Hyperlink and Images
                    case(SPFieldType.URL):
                        if (((SPFieldUrl)field).DisplayFormat == SPUrlFieldFormatType.Image)
                        {
                            string imageUrl = fieldValue.Split(',')[0].Trim();
                            string imageAltTag = fieldValue.Split(',')[1].Trim();
                            sb.AppendLine("<div class=\"wet-boew-lightbox\">");
                            sb.AppendLine("<ul>");
                            sb.AppendLine("<li style=\"list-style-type:none !important;\">");
                            sb.AppendLine("<a class=\"lb-item\" href=\"" + imageUrl + "\" title=\"" + imageAltTag + "\" style=\"list-style-type:none !important;\">");
                            sb.AppendLine("<img class=\"image-actual\" src=\"" + imageUrl + "\" alt=\"" + imageAltTag + "\" style=\"list-style-type:none !important;\" />");
                            sb.AppendLine("</a>");
                            sb.AppendLine("</li>");
                            sb.AppendLine("</ul>");
                            sb.AppendLine("</div>");
                        }
                        else
                            sb.AppendLine("<a href=\"" + fieldValue.Split(',')[0] + "\">" + fieldValue.Split(',')[1] + "</a>");
                        break;
                    case(SPFieldType.Text):case(SPFieldType.Note):case(SPFieldType.Number):
                        sb.AppendLine(fieldValue);
                        break;
                    case(SPFieldType.DateTime):
                        if (((SPFieldDateTime)field).DisplayFormat == SPDateTimeFieldFormatType.DateOnly)
                        {
                            sb.AppendLine(fieldValue.Split(' ')[0]);
                        }
                        else
                            sb.AppendLine(fieldValue);
                        break;
                    case(SPFieldType.Calculated):
                        fieldValue = fieldValue.Replace("string;#", "").Replace("datetime;#", "").Replace("number;#", "");
                        sb.AppendLine(fieldValue);
                        break;
                    default:
                        sb.AppendLine(fieldValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogEngine.Log(ex, "Accessible Lists");
            }

            return sb.ToString();
        }
    }
}
