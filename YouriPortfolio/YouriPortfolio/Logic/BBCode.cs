using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeKicker.BBCode;
using YouriPortfolio.Models;

namespace YouriPortfolio.Logic
{
    public class BBCode
    {
        private static BBCodeParser Parser;

        public static string ParseBBCode(string input)
        {
            if (Parser == null) InitParser();
            if (input == null) return "";
            return Parser?.ToHtml(input);
        }

        public static void ParseContent(Content content)
        {
            content.Title = ParseBBCode(content.Title);
            content.ShortContent = ParseBBCode(content.ShortContent);
            content.ContentText = ParseBBCode(content.ContentText);
        }

        private static void InitParser()
        {
            Parser = new BBCodeParser(new[]
                {
                    new BBTag("h1", "<h1>", "</h1>"),
                    new BBTag("h2", "<h2>", "</h2>"),
                    new BBTag("h3", "<h3>", "</h3>"),
                    new BBTag("h4", "<h4>", "</h4>"),
                    new BBTag("br", "<br/>", "", true, false),
                    new BBTag("b", "<b>", "</b>"),
                    new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
                    new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
                    new BBTag("code", "<pre class=\"prettyprint\">", "</pre>"),
                    new BBTag("img", "<img class=\"ContentImg\" src=\"${content}\" />", "", false, true),
                    new BBTag("quote", "<blockquote>", "</blockquote>"),
                    new BBTag("list", "<ul>", "</ul>"),
                    new BBTag("*", "<li>", "</li>", true, false),
                    new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href")),
                });
        }
    }
}