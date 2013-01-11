using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPCLF3.WebParts.SimpleItemView
{
    public class AccessibleListItem
    {
        private SPListItem _item;
        private string _fieldsToDisplay;
        private string _selectedFieldsOrder;

        public string FieldsToDisplay
        {
            get{ return this._fieldsToDisplay; }
            set{ this._fieldsToDisplay = value; }
        }

        public string SelectedFieldsOrder
        {
            get { return this._selectedFieldsOrder; }
            set { this._selectedFieldsOrder = value; }
        }

        public SPListItem ListItem
        {
            get { return this._item; }
        }

        public AccessibleListItem(SPListItem item, string fieldsToDisplay, string selectedFieldsOrder)
        {
            this._item = item;
            this.FieldsToDisplay = fieldsToDisplay;
            this.SelectedFieldsOrder = selectedFieldsOrder;
        }

        public string RenderAsHtml()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            try
            {
                SPFieldCollection fields = this.ListItem.Fields;

                sb.AppendLine("<table class=\"wet-boew-zebra\">");
                sb.AppendLine("<tr><th>Property</th><th>Value</th></tr>");

                if (this.FieldsToDisplay != null && this.SelectedFieldsOrder != null)
                {
                    int countSelectedfields = this.FieldsToDisplay.Split(';').Length - 1;
                    string[] fieldsInOrder = new string[countSelectedfields];
                    string[] order = this.SelectedFieldsOrder.Split(';');

                    int count = 0;
                    foreach (SPField field in fields)
                    {
                        if ((this.FieldsToDisplay == null || this.FieldsToDisplay.Contains(field.Title + ";")) && !field.Hidden)
                        {
                            if (order[count] != string.Empty)
                            {
                                fieldsInOrder[int.Parse(order[count]) - 1] = field.Title;
                                count++;
                            }
                        }                        
                    }

                    foreach (string fieldTitle in fieldsInOrder)
                    {
                        try
                        {
                            string fieldValue = string.Empty;
                            if(this.ListItem[fieldTitle] != null)
                                fieldValue = this.ListItem[fieldTitle].ToString();

                            string line = "<tr><td>" + fieldTitle + "</td><td>" + FieldRenderer.RenderField(fieldValue, this.ListItem.Fields.GetField(fieldTitle)) + "</td></tr>";
                            sb.AppendLine(line);
                        }                        
                        catch (Exception ex)
                        {
                            LogEngine.Log(ex, "Accessible Lists");
                        }
                    }
                }
                sb.AppendLine("</table>");
            }
            catch (Exception ex)
            {
                LogEngine.Log(ex, "Accessible Lists");
            }
            return sb.ToString();
        }
    }
}
