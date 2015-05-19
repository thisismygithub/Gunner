﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


    public class GoogleTracker
    {
        // 參考網址：https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
        // 參考網址：http://www.spyriadis.net/2014/07/google-analytics-measurement-protocol-track-events-c/

        private string googleURL = "http://www.google-analytics.com/collect";
        private string googleVersion = "1";
        private string googleTrackingID = "UA-39999999-1";
        private string googleClientID = "555";
        
        public GoogleTracker()
        {
            //googleClientID = Guid.NewGuid().ToString();
            googleClientID = HttpContext.Current.Session.SessionID.ToString();
        }

        public GoogleTracker(string trackingID)
        {
            this.googleTrackingID = trackingID;
            //googleClientID = Guid.NewGuid().ToString();
            googleClientID = HttpContext.Current.Session.SessionID.ToString();
        }

        public void trackEvent(string category, string action, string label, string value)
        {
            Hashtable ht = baseValues();

            ht.Add("t", "event");                   // Event hit type
            ht.Add("ec", category.ToLower());                 // Event Category. Required.
            ht.Add("ea", action.ToLower());                   // Event Action. Required.
            if (label != null)
                ht.Add("el", label.ToLower()); // Event label.
            if (value != null)
                ht.Add("ev", value.ToLower()); // Event value.

            postData(ht);
        }
        public void trackPage(string hostname, string page, string title)
        {
            Hashtable ht = baseValues();

            ht.Add("t", "pageview");                // Pageview hit type.
            ht.Add("dh", hostname);                 // Document hostname.
            ht.Add("dp", page);                     // Page.
            ht.Add("dt", title);                    // Title.

            postData(ht);
        }

        public void ecommerceTransaction(string id, string affiliation, string revenue, string shipping, string tax, string currency)
        {
            Hashtable ht = baseValues();

            ht.Add("t", "transaction");       // Transaction hit type.
            ht.Add("ti", id);                 // transaction ID.            Required.
            ht.Add("ta", affiliation);        // Transaction affiliation.
            ht.Add("tr", revenue);            // Transaction revenue.
            ht.Add("ts", shipping);           // Transaction shipping.
            ht.Add("tt", tax);                // Transaction tax.
            ht.Add("cu", currency);           // Currency code.

            postData(ht);
        }
        public void ecommerceItem(string id, string name, string price, string quantity, string code, string category, string currency)
        {
            Hashtable ht = baseValues();

            ht.Add("t", "item");              // Item hit type.
            ht.Add("ti", id);                 // transaction ID.            Required.
            ht.Add("in", name);               // Item name.                 Required.
            ht.Add("ip", price);              // Item price.
            ht.Add("iq", quantity);           // Item quantity.
            ht.Add("ic", code);               // Item code / SKU.
            ht.Add("iv", category);           // Item variation / category.
            ht.Add("cu", currency);           // Currency code.

            postData(ht);
        }

        public void trackSocial(string action, string network, string target)
        {
            Hashtable ht = baseValues();

            ht.Add("t", "social");                // Social hit type.
            ht.Add("dh", action);                 // Social Action.         Required.
            ht.Add("dp", network);                // Social Network.        Required.
            ht.Add("dt", target);                 // Social Target.         Required.

            postData(ht);
        }

        public void trackException(string description, bool fatal)
        {
            Hashtable ht = baseValues();

            ht.Add("t", "exception");             // Exception hit type.
            ht.Add("dh", description);            // Exception description.         Required.
            ht.Add("dp", fatal ? "1" : "0");      // Exception is fatal?            Required.

            postData(ht);
        }

        private Hashtable baseValues()
        {
            Hashtable ht = new Hashtable();
            ht.Add("v", googleVersion);         // Version.
            ht.Add("tid", googleTrackingID);    // Tracking ID / Web property / Property ID.
            ht.Add("cid", googleClientID);      // Anonymous Client ID.
            return ht;
        }
        private bool postData(Hashtable values)
        {
            string data = "";
            foreach (var key in values.Keys)
            {
                if (data != "")
                    data += "&";
                if (values[key] != null)
                    data += key.ToString() + "=" + HttpUtility.UrlEncode(values[key].ToString());
            }

            using (var client = new WebClient())
            {
                var result = client.UploadString(googleURL, "POST", data);
            }

            return true;
        }
    }

