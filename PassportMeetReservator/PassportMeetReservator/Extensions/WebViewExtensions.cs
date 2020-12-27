using CefSharp;
using CefSharp.WinForms;
using System.Threading;
using System.Threading.Tasks;

namespace PassportMeetReservator.Extensions
{
    public static class WebViewExtensions
    {
        public static async Task<bool> SetTextToViewOfClassWithNumber(this ChromiumWebBrowser browser, string className, int number, string text)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"if(views.length > {number})" +
                    "{" +
                        $"views[{number}].value = '{text}';" +
                        $"let e = document.createEvent('HTMLEvents');" +
                        $"e.initEvent('change', true, true);" +
                        $"views[{number}].dispatchEvent(e);" +
                    "}" +
                    $"views.length > {number};" +
                "}"
            );

            return (bool)result.Result;
        }

        public static async Task<string> SelectByIndex(this ChromiumWebBrowser browser, string selectorClassName, int number)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{selectorClassName}');" +
                    $"if(views.length == 1 && views[0].options.length > {number})" +
                    "{" +
                        $"views[0].selectedIndex = {number};" +
                        $"let e = document.createEvent('HTMLEvents');" +
                        $"e.initEvent('change', true, true);" +
                        $"views[0].dispatchEvent(e);" +
                    "}" +
                    "let found = null;" +
                    $"if(views.length == 1 && views[0].options.length > {number})" +
                        $"found = views[0].options[{number}].text;" +
                    $"found.toString()" +
                "}"
            );

            return (string)result.Result;
        }

        public static async Task<bool> SelectByValue(this ChromiumWebBrowser browser, string selectorClassName, string value)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let found = false;" +
                    $"let views = document.getElementsByClassName('{selectorClassName}');" +
                    $"if(views.length == 1)" +
                    "{" +
                        "for(let i = 0; i < views[0].options.length; ++i)" +
                        "{" +
                            $"if(views[0].options[i].text == '{value}')" +
                            "{" +
                                "views[0].selectedIndex = i;" +
                                "let e = document.createEvent('HTMLEvents');" +
                                "e.initEvent('change', true, true);" +
                                "views[0].dispatchEvent(e);" +
                                "found = true;" +
                                "break;" +
                            "}" +
                        "}" +
                    "}" +
                    "found == true" +
                "}"
            );

            return (bool)result.Result;
        }

        public static async Task<bool> TryClickAnyViewOfClassWithoutText(this ChromiumWebBrowser browser, string className, string avoidText)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = false;" +
                    "for(let view of views) {" +
                        $"if(view.textContent.indexOf('{avoidText}') == -1)" + " {" +
                            "view.click();" +
                            "found = true;" +
                            "break;" +
                         "}" +
                    "}" +
                    "found;" +
                "}"
            );
            return (bool)result.Result;
        }

        public static async Task<bool> TryClickViewOfClassWithText(this ChromiumWebBrowser browser, string className, string text)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = false;" +
                    "for(let view of views) {" +
                        $"if(view.textContent.indexOf('{text}') != -1)" + " {" +
                            "view.click();" +
                            "found = true;" +
                            "break;" +
                         "}" +
                    "}" +
                    "found;" +
                "}"
            );
            return (bool)result.Result;
        }

        public static async Task<bool> TryClickViewOfClassWithNumber(this ChromiumWebBrowser browser, string className, int number, string forbiddenClass, bool parent = false)
        {
            string parentText = parent ? ".parentNode" : "";
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    $"if(found.length > {number})" +
                        $"found[{number}]{parentText}.click();" +
                    $"found.length > {number};" +
                "}"
            );

            return (bool)result.Result;
        }

        public static async Task<bool> TryFindViewOfClassWithoutClass(this ChromiumWebBrowser browser, string className, string forbiddenClass)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    $"found.length > {0}" +
                "}"
            );

            return (bool)result.Result;
        }

        public static async Task<bool> TryFindView(this ChromiumWebBrowser browser, string className)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"views.length != 0;" +
                "}"
            );

            return (bool)result.Result;
        }

        public static async Task<bool> TryFindViewOfClassWithText(this ChromiumWebBrowser browser, string className, string text)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => view.textContent.indexOf('{text}') != -1);" +
                    "found.length != 0;" +
                "}"
            );

            return (bool)result.Result;
        }

        public static async Task<int> GetCountOfViewsOfClass(this ChromiumWebBrowser browser, string className, string forbiddenClass)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    "found.length;" +
                "}"
            );

            return (int)result.Result;
        }

        public static async Task<bool> CheckItemOfClassVisible(this ChromiumWebBrowser browser, string className)
        {
            JavascriptResponse result = await browser.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"views.length == 1 && views[0].style.display != 'none'" +
                "}"
            );

            return (bool)result.Result;
        }

        public static async Task<bool> ClickViewOfClassWithText(this ChromiumWebBrowser browser, string className, string text, int attempts, int delay, CancellationToken token)
        {
            for (int i = 0; i < attempts; ++i)
            {
                if (await browser.TryClickViewOfClassWithText(className, text))
                    return true;

                token.ThrowIfCancellationRequested();
                await Task.Delay(delay);
            }

            return false;
        }

        public static async Task<bool> ClickAnyViewOfClassWithoutText(this ChromiumWebBrowser browser, string className, string avoidText, int attempts, int delay, CancellationToken token)
        {
            for (int i = 0; i < attempts; ++i)
            {
                if (await browser.TryClickAnyViewOfClassWithoutText(className, avoidText))
                    return true;

                token.ThrowIfCancellationRequested();
                await Task.Delay(delay);
            }

            return false;
        }

        public static async Task ClickViewOfClassWithNumber(this ChromiumWebBrowser browser, string className, int number, string forbiddenClass, int delay, CancellationToken token, bool parent = false)
        {
            while (!await browser.TryClickViewOfClassWithNumber(className, number, forbiddenClass, parent))
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(delay);
            }
        }

        public static async Task<bool> WaitForView(this ChromiumWebBrowser browser, string className, int attempts, int delay, CancellationToken token)
        {
            for (int i = 0; i < attempts; ++i)
            {
                if (await browser.TryFindView(className))
                    return true;

                token.ThrowIfCancellationRequested();
                await Task.Delay(delay);
            }

            return false;
        }

        public static async Task ClickViewOfClass(this ChromiumWebBrowser browser, string className, int delay, CancellationToken token)
        {
            await browser.ClickViewOfClassWithNumber(className, 0, "", delay, token);
        }

        private static class BrowserUtil
        {
            public static string GetViewsOfClassWithText(string className, string text)
            {
                return $"Array.prototype.filter.call({GetViewsOfClass(className)}, view => view.textContent.indexOf('{text}') != -1)";
            }

            public static string GetViewsOfClass(string className)
            {
                return $"document.getElementsByClassName('{className}')";
            }
        }
    }
}
