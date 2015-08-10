// Copyright (c) Microsoft Corporation. All rights reserved. 

using System;
using System.Globalization;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

namespace SemanticLogging.Database.Xml.Utility
{
    internal static class EventEntryUtil
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Opt out for closing output")]
        internal static string XmlSerializePayload(EventEntry entry)
        {
            try
            {
                var settings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true   // Do not add xml declaration
                };

                var writer = new StringBuilder();
                using (var xmlWriter = XmlWriter.Create(writer, settings))
                {
                    XmlWritePayload(xmlWriter, entry);
                    xmlWriter.Flush();
                    return writer.ToString();
                }
            }
            catch (Exception e)
            {
                SemanticLoggingEventSource.Log.CustomSinkUnhandledFault(e.ToString());

                return string.Format("<Error>{0}</Error>", string.Format(CultureInfo.CurrentCulture, Properties.Resources.XmlSerializationError, e.Message));
            }
        }

        internal static void XmlWritePayload(XmlWriter writer, EventEntry entry)
        {
            writer.WriteStartElement("Payload");

            var eventSchema = entry.Schema;

            for (int i = 0; i < entry.Payload.Count; i++)
            {
                XmlWriteProperty(writer, eventSchema.Payload[i], entry.Payload[i]);
            }

            writer.WriteEndElement();
        }

        private static void XmlWriteProperty(XmlWriter writer, string propertyName, object value)
        {
            try
            {
                writer.WriteElementString(propertyName, SanitizeXml(value));
            }
            catch (Exception e)
            {
                SemanticLoggingEventSource.Log.CustomSinkUnhandledFault(e.ToString());

                // We are in Error state so abort the write operation
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.XmlSerializationError, e.Message), e);
            }
        }

        internal static string SanitizeXml(object value)
        {
            var valueType = value.GetType();
            if (valueType == typeof(Guid))
            {
                return XmlConvert.ToString((Guid)value);
            }

            return valueType.IsEnum ? ((Enum)value).ToString("D") : value.ToString();
        }
    }
}
