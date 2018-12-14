//-----------------------------------------------------------------------
// <copyright file="SoftwareService2WebServiceClient.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

#if !__MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using System.ServiceModel.Channels;
using Sphere10.Framework;

namespace Sphere10.Application {


	public class SoftwareService2WebServiceClient : ISphere10SoftwareService2, IDisposable {
		public const string DefaultEndPointUri = "http://www.sphere10.com/OnlineServices/SoftwareService2.svc";
		public const TransferMode DefaultTransferMode = TransferMode.Streamed;
		public const int DefaultMaxBufferSize = 65536;
		public const int DefaultMaxReceivedMessageSize = 104857600;

		public SoftwareService2WebServiceClient()
			: this(DefaultEndPointUri) {
		}

		public SoftwareService2WebServiceClient(string uri) {
			// Any channel setup code goes here
			EndpointAddress address = new EndpointAddress(uri);
			BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
			binding.TransferMode = DefaultTransferMode;
			binding.MaxBufferSize = DefaultMaxBufferSize;
			binding.MaxReceivedMessageSize = DefaultMaxReceivedMessageSize;
			ChannelFactory<ISphere10SoftwareService2> factory = new ChannelFactory<ISphere10SoftwareService2>(binding, address);
			Service = factory.CreateChannel();

		}

		private ISphere10SoftwareService2 Service { get; set; }

		public Result RequestFeature(FeatureRequest featureRequest) {
			return Service.RequestFeature(featureRequest);
		}

		public Result SendComment(ProductComment comment) {
			return Service.SendComment(comment);
		}

		public Result SubmitBugReport(BugReport bugReport) {
			return Service.SubmitBugReport(bugReport);
		}

		public void Test() {
			Service.Test();
		}

		public void Dispose() {
			if (Service is IDisposable) {
				((IDisposable)Service).Dispose();
			}
		}
	}
}
#endif
