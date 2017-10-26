using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace YouriPortfolio.Logic
{
    public class VideoExtract
    {
        private const string YoutubeLinkRegex = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";
        private const string GfyCatLinkRegex = "(?:.+?)?(?:gfycat\\.com/)(?:gifs/detail/)?((?!gifs)[a-zA-Z0-9]+)?(?:-)?(?:.+)?";
        private static Regex ytRegexExtractId = new Regex(YoutubeLinkRegex, RegexOptions.Compiled);
        private static Regex gfcatRegexExtractId = new Regex(GfyCatLinkRegex, RegexOptions.Compiled);
        private static string[] validAuthorities = { "youtube.com", "www.youtube.com", "youtu.be", "www.youtu.be", "www.gfycat.com", "gfycat.com", "giant.gfycat.com", "thumbs.gfycat.com", };

        public static string ExtractYoutubeIdFromUri(Uri uri)
        {
            try
            {
                string authority = new UriBuilder(uri).Uri.Authority.ToLower();

                //check if the url is a youtube url
                if (validAuthorities.Contains(authority))
                {
                    //and extract the id
                    var regRes = ytRegexExtractId.Match(uri.ToString());
                    if (regRes.Success)
                    {
                        return regRes.Groups[1].Value;
                    }
                }
            }
            catch { }


            return null;
        }

        public static string ExtractGfyCatIdFromUri(Uri uri)
        {
            try
            {
                string authority = new UriBuilder(uri).Uri.Authority.ToLower();

                //check if the url is a youtube url
                if (validAuthorities.Contains(authority))
                {
                    //and extract the id
                    var regRes = gfcatRegexExtractId.Match(uri.ToString());
                    if (regRes.Success)
                    {
                        return regRes.Groups[1].Value;
                    }
                }
            }
            catch { }


            return null;
        }
    }
}