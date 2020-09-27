// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpResponse"/> to work with the 'Trailer' response header.
    /// </summary>
    public static class ResponseTrailerExtensions
    {
        /// <summary>
        /// Adds the given trailer name to the 'Trailer' response header. This must happen before the response headers are sent.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="trailerName">The trailer name to add to the 'Trailer' response header</param>
        public static void DeclareTrailer(this HttpResponse response, string trailerName)
        {
            response.Headers.AppendCommaSeparatedValues(HeaderNames.Trailer, trailerName);
        }

        /// <summary>
        /// Indicates if the server supports sending trailer headers for this response.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <returns><see langword="true"/> if the server supports sending trailer headers for this response,
        /// otherwise <see langword="false"/>.</returns>
        public static bool SupportsTrailers(this HttpResponse response)
        {
            var feature = response.HttpContext.Features.Get<IHttpResponseTrailersFeature>();
            return feature?.Trailers != null && !feature.Trailers.IsReadOnly;
        }

        /// <summary>
        /// Adds the given trailer header to the trailers collection to be sent at the end of the response body.
        /// Check <see cref="SupportsTrailers" /> or an InvalidOperationException may be thrown.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="trailerName">The name of the trailer header.</param>
        /// <param name="trailerValues">The trailer values to append.</param>
        public static void AppendTrailer(this HttpResponse response, string trailerName, StringValues trailerValues)
        {
            var feature = response.HttpContext.Features.Get<IHttpResponseTrailersFeature>();
            if (feature?.Trailers == null || feature.Trailers.IsReadOnly)
            {
                throw new InvalidOperationException("Trailers are not supported for this response.");
            }

            feature.Trailers.Append(trailerName, trailerValues);
        }
    }
}
