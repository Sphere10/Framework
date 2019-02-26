﻿namespace Sphere10.Framework.Web {

	// https://stackoverflow.com/questions/38571032/how-to-get-httpcontext-current-in-asp-net-core
	public static class HttpContextEx {
		private static Microsoft.AspNetCore.Http.IHttpContextAccessor m_httpContextAccessor;

		public static void Configure(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor) {
			m_httpContextAccessor = httpContextAccessor;
		}

		public static Microsoft.AspNetCore.Http.HttpContext Current {
			get {
				return m_httpContextAccessor.HttpContext;
			}
		}
	}
}