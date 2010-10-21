﻿using System;
using System.Web;

namespace NSinatra.ActionResults
{
    public class StringResult : ActionResult
    {
        private readonly string data;

        public StringResult(string data)
        {
            this.data = data;
        }
        
        public override void WriteToResponse(HttpContextBase context)
        {
            context.Response.Write(data);
        }
    }

    public class BinaryResult : ActionResult
    {
        private readonly byte[] data;
        private readonly string contentType;

        public BinaryResult(byte[] data, string contentType)
        {
            this.data = data;
            this.contentType = contentType;
        }

        public override void WriteToResponse(HttpContextBase context)
        {
            context.Response.ContentType = contentType;
            context.Response.BinaryWrite(data);
        }
    }
}