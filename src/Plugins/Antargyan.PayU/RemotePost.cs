using SmartStore.Core;
using SmartStore.Core.Infrastructure;
using System.Collections.Specialized;
using System.Web;

namespace SmartStore.Web.Framework
{
    public class RemotePost
    {
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;
        private readonly NameValueCollection _inputValues;

        public string Url { get; set; }

        public string Method { get; set; }

        public string FormName { get; set; }

        public string AcceptCharset { get; set; }

        public bool NewInputForEachValue { get; set; }

        public NameValueCollection Params
        {
            get
            {
                return this._inputValues;
            }
        }

        public RemotePost()
            : this((HttpContextBase)EngineContext.Current.Resolve<HttpContextBase>((string)null), (IWebHelper)EngineContext.Current.Resolve<IWebHelper>((string)null))
        {
        }

        public RemotePost(HttpContextBase httpContext, IWebHelper webHelper)
        {
            this._inputValues = new NameValueCollection();
            this.Url = "http://www.someurl.com";
            this.Method = "post";
            this.FormName = "formName";
            this._httpContext = httpContext;
            this._webHelper = webHelper;
        }

        public void Add(string name, string value)
        {
            this._inputValues.Add(name, value);
        }

        public void Post()
        {
            this._httpContext.Response.Clear();
            this._httpContext.Response.Write("<html><head>");
            this._httpContext.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", (object)this.FormName));
            if (!string.IsNullOrEmpty(this.AcceptCharset))
                this._httpContext.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" accept-charset=\"{3}\">", new object[4]
        {
          (object) this.FormName,
          (object) this.Method,
          (object) this.Url,
          (object) this.AcceptCharset
        }));
            else
                this._httpContext.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", (object)this.FormName, (object)this.Method, (object)this.Url));
            if (this.NewInputForEachValue)
            {
                foreach (string str in this._inputValues.Keys)
                {
                    string[] values = this._inputValues.GetValues(str);
                    if (values != null)
                    {
                        foreach (string s in values)
                            this._httpContext.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", (object)HttpUtility.HtmlEncode(str), (object)HttpUtility.HtmlEncode(s)));
                    }
                }
            }
            else
            {
                for (int index = 0; index < this._inputValues.Keys.Count; ++index)
                    this._httpContext.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", (object)HttpUtility.HtmlEncode(this._inputValues.Keys[index]), (object)HttpUtility.HtmlEncode(this._inputValues[this._inputValues.Keys[index]])));
            }
            this._httpContext.Response.Write("</form>");
            this._httpContext.Response.Write("</body></html>");
            this._httpContext.Response.End();
            this._webHelper.IsCurrentConnectionSecured();
        }
    }
}
