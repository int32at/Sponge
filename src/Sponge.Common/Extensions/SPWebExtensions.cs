using System;
using System.Collections;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Sponge.Common.Extensions
{
    public static class SPWebExtensions
    {
        #region Property Bag

        public static void SetPropertyString(this SPWeb web, string propertyName, string value)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedSite = new SPSite(web.Site.ID))
                {
                    using (SPWeb elevatedWeb = elevatedSite.OpenWeb(web.ID))
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;

                        if (!elevatedWeb.AllProperties.ContainsKey(propertyName))
                        {
                            var properties = elevatedWeb.AllProperties;
                            foreach (DictionaryEntry property in properties)
                            {
                                if (property.Key.ToString().ToLowerInvariant() == propertyName.ToLowerInvariant())
                                {
                                    elevatedWeb.SetProperty(property.Key.ToString(), value);
                                    propertyName = null;
                                    break;
                                }
                            }

                            if (!string.IsNullOrEmpty(propertyName))
                                elevatedWeb.AddProperty(propertyName, value);
                        }
                        else
                        {
                            elevatedWeb.SetProperty(propertyName, value);
                        }

                        elevatedWeb.Update();
                        elevatedWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public static string GetPropertyString(this SPWeb web, string propertyName)
        {
            return web.AllProperties[propertyName].ToString();
        }

        public static string TryGetPropertyString(this SPWeb web, string propertyName)
        {
            string value = null;

            try
            {
                value = web.GetPropertyString(propertyName);
            }
            catch
            {
                // eat
            }

            return value;
        }

        public static string TryGetPropertyString(this SPWeb web, string propertyName, string defaultValue)
        {
            string value = null;

            try
            {
                value = web.GetPropertyString(propertyName);
            }
            catch
            {
                value = defaultValue;
            }

            return value;
        }

        public static void RemovePropertyString(this SPWeb web, string propertyName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedSite = new SPSite(web.Site.ID))
                {
                    using (SPWeb elevatedWeb = elevatedSite.OpenWeb(web.ID))
                    {
                        elevatedWeb.AllowUnsafeUpdates = true;
                        elevatedWeb.DeleteProperty(propertyName);
                        elevatedWeb.Update();
                        elevatedWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public static void TryRemovePropertyString(this SPWeb web, string propertyName)
        {
            if (web.AllProperties.ContainsKey(propertyName))
                web.RemovePropertyString(propertyName);
        }

        #endregion Property Bag

        #region Send Email

        public static void SendEmail(this SPWeb web, string to, string cc, string bcc, string subject, string body)
        {
            // recipient is mandatory
            if (string.IsNullOrEmpty(to))
                throw new ApplicationException("No recipient specified.");

            // auto-generate reply address from recipient address (assuming mailing will be used company-internal only)
            var from = string.Format("noreply{0}", to.Remove(0, to.IndexOf("@")));

            // assemble message headers
            var messageHeaders = new StringDictionary();
            messageHeaders.Add("to", to);
            messageHeaders.Add("from", from);
            messageHeaders.Add("subject", subject);
            messageHeaders.Add("content-type", "text/html");
            if (!string.IsNullOrEmpty(cc))
                messageHeaders.Add("cc", cc);
            if (!string.IsNullOrEmpty(bcc))
                messageHeaders.Add("bcc", bcc);

            // send mail (requires SMTP server installed and enabled)
            SPUtility.SendEmail(web, messageHeaders, body);
        }

        public static void SendHtmlEmail(this SPWeb web, string to, string subject, string htmlBody)
        {
            // recipient is mandatory
            if (string.IsNullOrEmpty(to))
                throw new ApplicationException("No recipient specified.");

            // send mail (requires SMTP server installed and enabled)
            SPUtility.SendEmail(web, true, true, to, subject, htmlBody);
        }

        #endregion Send Email
    }
}
