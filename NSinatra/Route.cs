using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NSinatra
{
    internal class Route
    {
        public string Verb { get; private set; }
        public string PathPattern { get; private set; }
        public Func<object, ActionResult> Action { get; private set; }

        public Route(string verb, string pathPattern, Func<object, ActionResult> action)
        {
            Verb = verb;
            PathPattern = pathPattern;
            Action = action;
        }

        public bool DoesMatchUrl(string url)
        {
            var pattern = Regex.Replace(PathPattern, "/:([^/]*)", "/([^/]*)");
            return Regex.IsMatch(url, "^" + pattern + "$");
        }

        public IEnumerable<KeyValuePair<string, object>> GetMatchingPartsFromUrl(string url)
        {
            var urlParts = url.Split('/');
            var parts = PathPattern.Split('/');
            for (var i = 0; i < parts.Length; i++)
            {
                if (!parts[i].StartsWith(":")) continue;

                var key = parts[i].Substring(1);
                var value = urlParts[i];
                yield return new KeyValuePair<string, object>(key, value);
            }
        }
    }
}