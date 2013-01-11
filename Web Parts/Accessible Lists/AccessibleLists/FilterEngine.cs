using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPCLF3.WebParts
{
    // Nik - The purpose of the Filter Engine is to filter a list using an SPQuery based on parameters received in the QueryString. The querystring will
    //       pass the internal name of fields, along with their expected value, will generate and SPQuery on the fly, and will filter the results on screen.
    //       This class only takes care of generating the SPQuery from the QueryString.
    class FilterEngine
    {
        private System.Collections.Specialized.NameValueCollection _queryString;
        private string _CAMLQuery;
        private SPList _list;
        public FilterEngine(SPList list)
        {
            this._list = list;
        }
        public FilterEngine(System.Collections.Specialized.NameValueCollection queryString, SPList List)
        {
            this._queryString = queryString;
            this._list = List;
        }

        public System.Collections.Specialized.NameValueCollection QueryString
        {
            get { return this._queryString; }
            set { this._queryString = value; }
        }

        public SPList List
        {
            get { return this._list; }
        }

        public string CAMLQuery
        {
            get{
                if (_CAMLQuery ==  null)
                    GenerateCAMLQuery();
                return this._CAMLQuery;
            }
            set
            {
                this._CAMLQuery = value;
            }
        }

        private void GenerateCAMLQuery()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("<Where>");
            try
            {                
                int count = this.QueryString.Count;

                string curFieldName = string.Empty;
                int goodCount = 0;
                for (int i = 0; i < count - 1; i++)
                {
                    curFieldName = QueryString.Keys[i];
                    if (curFieldName != null && this.List.Fields.TryGetFieldByStaticName(curFieldName) != null)
                        goodCount++;
                }

                for (int i = 0; i < goodCount - 1; i++)
                {
                    curFieldName = QueryString.Keys[i];
                    if(curFieldName != null && this.List.Fields.TryGetFieldByStaticName(curFieldName) != null)
                        sb.Append("<And>");
                }

                int index = 1;
                foreach (string part in QueryString.AllKeys)
                {
                    curFieldName = part;
                    if (part != null && this.List.Fields.TryGetFieldByStaticName(curFieldName) != null)
                    {
                        sb.Append("<Contains><FieldRef Name='" + part + "' /><Value Type='Text'>" + QueryString[part] + "</Value></Contains>");
                        if (count >= 2 && index > 1)
                            sb.Append("</And>");
                        index++;
                    }
                }
            }
            catch(Exception ex)
            {
                LogEngine.Log(ex, "Accessible Lists - FilterEngine");
            }
            sb.Append("</Where>");
            this._CAMLQuery = sb.ToString();
        }
    }
}
