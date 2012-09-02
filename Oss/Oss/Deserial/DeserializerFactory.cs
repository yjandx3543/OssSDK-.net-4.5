﻿using Oss.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal abstract class DeserializerFactory
    {
        protected DeserializerFactory()
        {
        }

        protected abstract IDeserializer<Stream, T> CreateContentDeserializer<T>();
        public IDeserializer<HttpResponseMessage, Task<ErrorResult>> CreateErrorResultDeserializer()
        {
            return new SimpleResponseDeserializer<ErrorResult>(this.CreateContentDeserializer<ErrorResult>());
        }

        public IDeserializer<HttpResponseMessage, Task<AccessControlList>> CreateGetAclResultDeserializer()
        {
            return new GetAclResponseParser(this.CreateContentDeserializer<AccessControlPolicy>());
        }

        //public IDeserializer<ServiceResponse, ObjectMetadata> CreateGetObjectMetadataResultDeserializer()
        //{
        //    return new GetObjectMetadataResponseDeserializer();
        //}

        //public IDeserializer<ServiceResponse, OssObject> CreateGetObjectResultDeserializer(GetObjectRequest request)
        //{
        //    return new GetObjectResponseDeserializer(request);
        //}

        public IDeserializer<HttpResponseMessage, Task<IEnumerable<Bucket>>> CreateListBucketResultDeserializer()
        {
            return new ListBucketResultDeserializer(this.CreateContentDeserializer<ListAllMyBucketsResult>());
        }

        //public IDeserializer<ServiceResponse, ObjectListing> CreateListObjectsResultDeserializer()
        //{
        //    return new ListObjectsResponseDeserializer(this.CreateContentDeserializer<ListBucketResult>());
        //}

        public IDeserializer<HttpResponseMessage, PutObjectResult> CreatePutObjectReusltDeserializer()
        {
            return new PutObjectResponseDeserializer();
        }

        //public static DeserializerFactory GetFactory()
        //{
        //    return GetFactory(null);
        //}

        public static DeserializerFactory GetFactory(string contentType = null)
        {
            if (contentType == null)
            {
                contentType = "text/xml";
            }
            if (contentType.Contains("xml"))
            {
                return new XmlDeserializerFactory();
            }
            return null;
        }
    }
}