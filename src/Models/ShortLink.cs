using Microsoft.AspNetCore.WebUtilities;
using System;

namespace ShortLinkApi.Models
{
    public class ShortLink
    {
        public string GetUrlChunk()
        {
            return WebEncoders.Base64UrlEncode(BitConverter.GetBytes(Id));
        }

        public static int GetId(string urlChunk)
        {
            return BitConverter.ToInt32(WebEncoders.Base64UrlDecode(urlChunk),0);
        }

        public int Id { get; set; }

        public string Url { get; set; }
    }
}