﻿using btnapp.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Api.V2;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SelectPdf;
using System.Net.Http;
using LZStringCSharp;

namespace btnapp.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string Username = ConfigurationManager.AppSettings["pbiUsername"];
        private static readonly string Password = ConfigurationManager.AppSettings["pbiPassword"];
        private static readonly string AuthorityUrl = ConfigurationManager.AppSettings["authorityUrl"];
        private static readonly string ResourceUrl = ConfigurationManager.AppSettings["resourceUrl"];
        private static readonly string ClientId = ConfigurationManager.AppSettings["clientId"];
        private static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"];
        private static readonly string GroupId = ConfigurationManager.AppSettings["groupId"];
        private static readonly string ReportId = ConfigurationManager.AppSettings["reportId"];

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> Report()
        {
            ViewBag.Message = "Power Bi Report";

            var result = new EmbedConfig();
            try
            {
                // Create a user password cradentials.
                var credential = new UserPasswordCredential(Username, Password);

                // Authenticate using created credentials
                var authenticationContext = new AuthenticationContext(AuthorityUrl);
                var authenticationResult = await authenticationContext.AcquireTokenAsync(ResourceUrl, ClientId, credential);

                if (authenticationResult == null)
                {
                    result.ErrorMessage = "Authentication Failed.";
                    return View(result);
                }

                var tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer");

                // Create a Power BI Client object. It will be used to call Power BI APIs.
                using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
                {
                    // Get a list of reports.
                    var reports = await client.Reports.GetReportsInGroupAsync(GroupId);

                    Report report;
                    if (string.IsNullOrEmpty(ReportId))
                    {
                        // Get the first report in the group.
                        report = reports.Value.FirstOrDefault();
                    }
                    else
                    {
                        report = reports.Value.FirstOrDefault(r => r.Id == ReportId);
                    }

                    if (report == null)
                    {
                        result.ErrorMessage = "Group has no reports.";
                        return View(result);
                    }

                    var datasets = await client.Datasets.GetDatasetByIdInGroupAsync(GroupId, report.DatasetId);
                    result.IsEffectiveIdentityRequired = datasets.IsEffectiveIdentityRequired;
                    result.IsEffectiveIdentityRolesRequired = datasets.IsEffectiveIdentityRolesRequired;
                    GenerateTokenRequest generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                    var tokenResponse = await client.Reports.GenerateTokenInGroupAsync(GroupId, report.Id, generateTokenRequestParameters);

                    if (tokenResponse == null)
                    {
                        result.ErrorMessage = "Failed to generate embed token.";
                        return View(result);
                    }

                    // Generate Embed Configuration.
                    result.EmbedToken = tokenResponse;
                    result.EmbedUrl = report.EmbedUrl;
                    result.Id = report.Id;

                    return View(result);
                }
            }
            catch (HttpOperationException exc)
            {
                result.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
            }
            catch (Exception exc)
            {
                result.ErrorMessage = exc.ToString();
            }

            return View(result);
        }


        public async Task<ActionResult> ReportALT()
        {
            ViewBag.Message = "Power Bi Report";

            var result = new EmbedConfig();
            try
            {
                // Create a user password cradentials.
                var credential = new UserPasswordCredential(Username, Password);

                // Authenticate using created credentials
                var authenticationContext = new AuthenticationContext(AuthorityUrl);
                var authenticationResult = await authenticationContext.AcquireTokenAsync(ResourceUrl, ClientId, credential);

                if (authenticationResult == null)
                {
                    result.ErrorMessage = "Authentication Failed.";
                    return View(result);
                }

                var tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer");

                // Create a Power BI Client object. It will be used to call Power BI APIs.
                using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
                {
                    // Get a list of reports.
                    var reports = await client.Reports.GetReportsInGroupAsync(GroupId);

                    Report report;
                    if (string.IsNullOrEmpty(ReportId))
                    {
                        // Get the first report in the group.
                        report = reports.Value.FirstOrDefault();
                    }
                    else
                    {
                        report = reports.Value.FirstOrDefault(r => r.Id == ReportId);
                    }

                    if (report == null)
                    {
                        result.ErrorMessage = "Group has no reports.";
                        return View(result);
                    }

                    var datasets = await client.Datasets.GetDatasetByIdInGroupAsync(GroupId, report.DatasetId);
                    result.IsEffectiveIdentityRequired = datasets.IsEffectiveIdentityRequired;
                    result.IsEffectiveIdentityRolesRequired = datasets.IsEffectiveIdentityRolesRequired;
                    GenerateTokenRequest generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                    var tokenResponse = await client.Reports.GenerateTokenInGroupAsync(GroupId, report.Id, generateTokenRequestParameters);

                    if (tokenResponse == null)
                    {
                        result.ErrorMessage = "Failed to generate embed token.";
                        return View(result);
                    }

                    // Generate Embed Configuration.
                    result.EmbedToken = tokenResponse;
                    result.EmbedUrl = report.EmbedUrl;
                    result.Id = report.Id;

                    return View(result);
                }
            }
            catch (HttpOperationException exc)
            {
                result.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
            }
            catch (Exception exc)
            {
                result.ErrorMessage = exc.ToString();
            }

            return View(result);
        }


        public async Task<ActionResult> ReportALT2()
        {
            ViewBag.Message = "Power Bi Report";

            var result = new EmbedConfig();
            try
            {
                // Create a user password cradentials.
                var credential = new UserPasswordCredential(Username, Password);

                // Authenticate using created credentials
                var authenticationContext = new AuthenticationContext(AuthorityUrl);
                var authenticationResult = await authenticationContext.AcquireTokenAsync(ResourceUrl, ClientId, credential);

                if (authenticationResult == null)
                {
                    result.ErrorMessage = "Authentication Failed.";
                    return View(result);
                }

                var tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer");

                // Create a Power BI Client object. It will be used to call Power BI APIs.
                using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
                {
                    // Get a list of reports.
                    var reports = await client.Reports.GetReportsInGroupAsync(GroupId);

                    Report report;
                    if (string.IsNullOrEmpty(ReportId))
                    {
                        // Get the first report in the group.
                        report = reports.Value.FirstOrDefault();
                    }
                    else
                    {
                        report = reports.Value.FirstOrDefault(r => r.Id == ReportId);
                    }

                    if (report == null)
                    {
                        result.ErrorMessage = "Group has no reports.";
                        return View(result);
                    }

                    var datasets = await client.Datasets.GetDatasetByIdInGroupAsync(GroupId, report.DatasetId);
                    result.IsEffectiveIdentityRequired = datasets.IsEffectiveIdentityRequired;
                    result.IsEffectiveIdentityRolesRequired = datasets.IsEffectiveIdentityRolesRequired;
                    GenerateTokenRequest generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                    var tokenResponse = await client.Reports.GenerateTokenInGroupAsync(GroupId, report.Id, generateTokenRequestParameters);

                    if (tokenResponse == null)
                    {
                        result.ErrorMessage = "Failed to generate embed token.";
                        return View(result);
                    }

                    // Generate Embed Configuration.
                    result.EmbedToken = tokenResponse;
                    result.EmbedUrl = report.EmbedUrl;
                    result.Id = report.Id;

                    return View(result);
                }
            }
            catch (HttpOperationException exc)
            {
                result.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
            }
            catch (Exception exc)
            {
                result.ErrorMessage = exc.ToString();
            }

            return View(result);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ConvertHtmlCodeToPdf(FormCollection collection)
        {
            // instantiate a html to pdf converter object

            
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

            converter.Options.DisplayHeader = true;
            converter.Options.DisplayFooter = true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;
            

            converter.Header.Height = 82;

            var imgBTNHtml = "<img src='data:image/png;base64,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'/>";
            var imgIndexCalculatorHtml = "<img src ='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAZAAAABQCAIAAAC4QrSbAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAB3RJTUUH4gUJDSAnGubSSQAAeFtJREFUeNrtvXe8VcXVP7ym7H16u+f2S+8dpAkoNmxYYo9G82g0akw1iT3GJBo1xiQmpsduTGLvCopgBURAURCklwvcXk4ve++Zef+YvffZ59xzC8jzPL/n/TCfK95y9uyZNWvWrPJda5AQAg63w+1wO9z+LzT8vz2Aw+1wO9wOt4G2wwLrcDvcDrf/M+2wwDrcDrfD7f9MOyywDrfD7XD7P9MOC6zD7XA73P7PNDqQD3EuygQTEWCEETqYtwohOC8TncQYoYH1yDkfyMcQ6r9DIQQXYgBvRQjBAIdXOlohBC9LwP+F+fZJChCCA4CAXsfGhRADIld/74IC/wgBXPCD6xPj/g/dAVKv3wEjQBijA6MDQggOkm2swQsA0dtkbe4VAAgd5Ia05+Jc9wHvi76o1KPzg5QYBXL2AWsQQnDOMcZ9kJsxjg5kFIxxhPpiMs6FAEEGwIUDb4xzADgkfQoAxjhGA9onAIIxjjDugz6cc3GIxvbl5yuEsNfa+f3h9iUb50KIfrbSQFq/i/L/+1UrI7AECADgTEh9x2Bs6679n2/bs72xOZ7IgOABv3dIXdW4kYMnjxnqcbuEEIxz0s9iCMY5AsCYAEBLR/enX+zcuquptaNbN5hKaUNd5eih9VPHDa2qCIO1jbFTUDsaY2zT9sa8bvQhCAQApaQyHKytisitKweAMEJQtCe7YsntjU2UUuhLdoPHrdZUhqPhoDkGzpB5kJQdg5wvknJtf0vH+q27t+zc19aVMAzuUklddXTciPpp40dWhAJgyn3ACJX0JkAgQJqub9q+V8r6kjnarxdCKIpSXRGsqYzIhZBqBcZYCOiXh+Vn4snMtj37CUa6wWorI0Pqq4tFGCAEu/a1dHTFCSXWqQ8CABBIqjrYCdl6QTGN5O8FFzB6WH3I7wOAeDKzbfc+ZEnYAW44IQQheNKYoZT0ZSjk8trG7Y0D77aXhhjnAa9r3MghXHCM8K79re2dcWrSoecmMicSCvrrqyIet0sOmHFOiOTq/ocjif/FjsZ0Jo8wUhU6bsQghVLnX/e2dDS1damUGIZREQ6OHFInhEBoQP0XOmlqa+roUqmi6cbQhurayggAtHR07dnfoShErmmPFSxLJMQ583nc40cOllRBCG3bs687kaaYMM7GjRgc8HnFl1iLcistBOeCEJJIZ/7z8rtPLnp/8/Z9iXQGI6nvYQGCc+HxqCMH15514twrzlvQUFPJGMe4V9WXc0EwFgBLVnzyzxeWrfpsa0t7N+McIQwgEAAXgmJSV11x9MzxV5x30tEzJwohOGcEk570TWWyF/3o3n0tnS6F8l52oxBAKfZ5PbXR8JxpY849Zd7caeMxxpxzZIlBxjkl5J3V6y/6wa+jkZDBmLMr5z4XQrhU1e9zD2uoXjB32ldPO3pYQw3jXPQyY8Y5wYRxvui9NY+9+M7qTze3d8c5FwgQmPo2p4Q01FYcM2vSFeefNGfqOCEE51CiFcn5tnfFz/72LxOpLCHEYsdSThHAKaEBr6euOjJ76pgLFh595JSxto7cLx8wzighDz/75i2/ezwaCcYS6eNmT379oV8gQDYdhOAI4Xv+8dxjLyyNhAKMSTvL3K+oMGYHV6OerIkAACHIafqLf7l1wdypALBmw9azv32nx+MSAqD8BHv0gpBusHDA98nLf4gEA2U1C84FxqiprfOUK25DAxMQsmdzOo5ZYYyT6dy8I8Ysffxuw2Cqgn/z0Av/ePKNqoqgYTCECntYFKYOIMDnc4X8/injhp5+/OzTjpkR9PsY44T0Pxg5+D1Nbade8fNkJksI4ZwvfugXs6eMlWvKuSAEbdi86/xr7wn6vOlMdvzIIe/8626fxy0GRkMBAiEQQnzj5j989NkWv8+dyxmLHvmFFFjPvbHyB7/8R000rBtMyh5bDNvbwl5Qc00wSmcyc6aOW/bPu5EAgzNKyE/v+/eLSz+sDAe7E6ll/7xr3hHjOecHbVKUCiy5bQghr7790W33//uL7XvdLpfP46oI+TXNYEIACIyxS6EI0M7Gll/+5T+Pv/DW7T+45JKvHM85L7t9GWOEkJ17m2/+zWOL3v9EMO7zecJBn25wwzAECASgUKpQEkuknnztveffXHnuSfPu+OElg+uqGOOE9JwbwphQQjEhSAjUY7PYH0uls5tiyY837njwmSWnHzvr3puuGFxXWbKHESBMCCFICNw7VyPGWXc81dzW/e5Hn//xiVe+e/Fp1191PsG451YxGKOEfLF9782/e2zZys+E4H6/tyIY0HRdZxwAEAKFUkpwZ3fyiZfefWbRBxeddszt115SUxlh5dYSISCEYIwJwb2Y8AgBBoBEOt2xNbF6/daHnl5yzslz77n+sprKiGT9vhhXCEpIXtOee2OF26WCgKDfu3LdFys/2XTU9Amcc4Ic5EKIEIIRAozsw9Ypo2wWLvmlk+AIAXG4yBAAIYRiKgQX0Kue6+wKIUSwGJBhjhBGBEHh/HEOz0kGgCIN13qdAIEIxgSb9oFsBGFKMMVYkJJ9az0rBCIol9dT6c7tjU3PLl45ftTge66/7JSjp3PO+14RAOCCYyDPvbGypSNWGQkCQEcs8fTrH8yeMla+T/awYN60I8YN37R9b8jv27Sj8e1V6888fjYXggxAYkk1YsXHG9du3BEK+JPpzNEzJsybNl43dIUqGMlZY06ERQopmG2HIHYQUyBACGOCqakpW+8nGBGMCcEU44EcG30353oL2TBGd/71qYt+eO/ufa01lSFVxZ2xRDav1VSFRw+tGzu8oaE6mtdYR3ecEFIbjXTH05ff/Ief3veE1F8KQkOAAGEwRgh5+8PPTvivW199e00o4AuHA5lcriue9HlcIwbXjB3WMGJwvculdMQS2bxWGQ75vO4nX3t3wWW3rvpsMyHYYEYZJgSQqhkA4lxwwbhgXHAhuJDeQi4ymXw8meaMVVUE/V73C299eMoVt23euU+eTiUcK7VeIUA+y7mQoQYZbuCCJ1O5ZCqrKrQmGtY0/dbf/+t7P/8b5xyQPJkFCAEgDGZQQl5ZturEb9z61opPw0F/OOBPpbJdiaTf5xk1pG7s8PphDbUqpZ3dSU3XKyNBr8f96AtLT/rGbRu27iYYM8Yc00SF6Zo/iJIm/SNcsHQml0hmhRBVFSGPR/n3K2+fcfXte5raMEZ9e525EACwbNX6DVv3eN0uLjjBkNf0ZxYtly8sJpTgjHHBmUUkbo7B9NRYH0MAggvOOJj0NNeFc8E4l084l1MAMuWDRX7ri5sLK19k/sTNb/s1L5DpPwbJGJwLZg7eXF3Ld44AkDkL82MlryuJFAlbiRSFocoOgXEhAGkaiycy2Xw+5PdWVQR3NTaf+927Hn9hKcaY9bkiUgHPa/ozi953u11yff0e9+vvrW3rjBOMBRcIIYMZLlU5+6R5WS0vufrZxSug55r1+hoAgKcXfZDP6wRj3TDOO3U+mG5+a49ZokHuK8455yC/hFx0e/Wtv4PgPV5iqq9f3r1WpGFxIQjGt/z28d889HxVNEwI7owla6KRay5aeObxR44ZXh8K+BCCRCq7c1/LovfWPvLMWy0dXeGgz62G73nwuXDIf/03zylovAg4E5SQpSvWXXzdb3SDVUdDyUzOMNip86d//awTZkwcWVsVUSjVdWN/a+faz7f/86Vlyz5c73Gp1ZXh1o74ed+9++n7bzp6xkSpo5VQAQHSDVZZEXjwzh+EAj6HQYdAAAZIZnMbtu5+6a0PV3y80evxVEeDu/e1fvtnf1ny2J0KJSWqCsGkO57+6mlH/ey7X+tOpCnGtm4PSICAtq74ik82Pb945e6mtnDAU1dd8dBzbzbUV/7sOxcxzgjBgJDBGCX0xSUrL7/5fkJQZTiQSGcFF+ecPPfiM4+dPGZYfXUFISSv6ftbO1Z9uvXxF5Yt/3iT16PWRiO79rWc8527nv/TT6aOH27rgMKhDmCE8po+tKH6H7/8nkuhvGCqyd2OUqnMJ5t2vLxs1UefbfV73bWVFRu2Nn77539Z9ODtGKM+3LHyt88sWs4YB4Q50wGI1+Na/N7aW665oLYywoXACCGEAeCWb11w9VdPJoSaeoVFRS6E1+3auH33Vbf+2e1Skpn8zIkj/3r7d7I5DSPk8IKYKpQQfOTQentUQgiMUDyTOfXo6b/88dczuTxGGAECJBxPlm5rglHA64FewnDSxVlfVbHk0TucZ7sV+hKcC7/X9cu/PP3SWx+GAr5kJvfLay9ZeOysZDqDi5RKAADGDF/xu7gQCKNEInPvDd845ZjpiWQGE2xtdCGN1t37WpYsX/fSslWZXN7v9+bz2o9/9fDYEYPnTBvbh7XOuKAEln342cZtjUG/1+AcCeF2KXv2t7+ybOWVX13IBSdA5CDPOWnOn554JZfTfF73u6vX79zbPGJwHecC9adWE4Lbu2JLVqzze12ZvDaotvr0Y2cAgDl3AQBACO6KJb/z9dO+e8kZXbEULTF3LAa0rWDGud/jshSAog0rAAYkRvtsBYHFmCAEP/j0G79/9KXqyjAC6OxOnLVgzn0/+WZ9ddRiLAEAQb936tjh08aNuOzsE75/+z/eXLEuEvBFw8E7//bUvCPGzps+wbKxOSF48859V9xyv25wn9fTnUjXVkXu/+lVC4+Z6fQQKQodNqhm2KCa80+d99wbK66/55HuZCrk9yRSuctv/sPSx+8aWl/d064RCEAIhZKp44YH/d6yG/LoGRO+/bXT/vHkop/c90ReNyrCgRWfbHrixWVXXHCyrXQgy5g3mBENB0cOqZM2Xc8FPuXo6ddeetb3b//bK++sCfk90XDwH08uvvTs44fV13DOAQQlZO2Gbd/++V8pJR6X0p3MDKuvuv+2q0+YM9UxPOFS6YjBdSMG1110xjFPvfb+jfc+nMikw0F/a0f3pTfc99bjd1ZHQ/LzJfNhnHvc6vQJIyklZV3p82dN+sGlX7nv0Rfv/MszCFBlOPD2h+ufXbzygoXzOC8V+vauwxhv39O8bOWnPq9bcO5SFM1gLkVpbO547e3VV371FMEFEIQQCBDDGmqGNdSU6YdzjLHBmRACAeKce73u8SMH922Q2qSWwXTGeUUkOH7kkAG63qy90EtDAAJUVTliwsiyf5cGeFUkxLjpzRk5pG7MsPpeHBGSCwp+D4QAI8QYHzaoZvTQhrJPzZw06oKF86+5+LQrbv7Dnqb2gNfTGU/85sHnn//LT/oIUsk/Pfn6+1INdFECADpjlOCnF6+84oJTCCEgACPEOB81tP74Iyc/s3hFVSTU1t79yrKPfviNs6VF2QfVpAvs1bfXNO5vr6oItXfHLj7jmNqqCHM4diRUwuCstrJixODaIXWM0r76LKKTuQDmjwNbyv4btkbPCcFbdu6//c9P+v0eBKgrnrz8vBOf/P0N9dVRgzFuYrGk2iyEEAZjDTWVT//xpuOPnNydTKsK0TV2zwPPgakkm/vzxl8/0hFL+r2eZCozYnD1mw//YuExMxljnDPbLhMAnAvGOGPi/FOPfv3Bn9dGw8l0LuD37GvpvOW3jwOYr3Vyojy1OQeDMbkAvLgxxg3GDMa+9bXTfnj5WclUDgBcivL8kpXggLQ4OjWPx55dyWYYLBoOPHj3D6aNG55K51yK0tmdeHnJKsn6GGPDMG649+F0VvO41XgqM2Hk4Dcfuf2EOVMZ4wUjSCAhEOfcYExwcfGZx770t59FgoFMNh8O+Tfv2nvr7x+3Vrx0nRFCnAudMVs7L5ov5wZjjPPrrjj36otOjqcyCGOC8YtvLQfoNR4iOAeAZxcvb++Mqyo1GPvZ975WG41ouq4q5OlFHwAAITKaLBAgLqRF1/PtAkxUimXaCfmb8o1xLpz+R2uucjUZL/+Wnq0fkxDJ0FVvTQAA48weg8VLvX4eoTKCTNrUzgGzwr/CMNiMiaP+dsf3VJXqhhH0eVd++sXmnXsRKm+qS5/0jsbmt1d+FvR7uuLps06cc+7J87rj6aDfs/bzrR9+shkAmGD2kXXR6ccqlDDOXC71xSUf6oZBCelTSJinyHNvrCAUM8Y8LteFp82HIh+eFWsRpvwZ4IqYsSgnv0kI45d2YEEJ0v3eB5/p7E54XWo8mTphzrQ/3fYtYYXSCCk4SRFCGGNKiGEwhdLf33plTTRk6EY04l/5yZY1G7YijHTDQAj9+9V3l6z4tCLkz+fzfq/7obuuHT64TjcMjDEhBJteW0AAGCNCMCFYN4yJo4c+8MvvUUrymh4J+V9a+uGrb6/GCEstpgdHmtABjBEuboRgKt3DAFecd2JdVSSvGaqLbt/T0t4VR6gnpMOMrfTsSg6YUqIbhs/j/tZFp+qGIQFlazZsA8vOeeLld1Z+sjkU8GZzuUjQ98ivrq2vqdQNgxAsHdVytvJBSgjGWNeNmZNG/fln1zDOdY1FQ6FnFi3/YM3nJW4OR1wGSQRJmRFiTAmRL7nygoUVoYCmay5V3bhtbyKdwbiMw14IQQjJ5vIvLl3p8bjS2fzIIbVXXXjqkUeMS2fzAb93zYatH6z9HABxYZpm0hdb7u0ITE+wMH1uSP6mfCvBwVi+EtOOk57agbR+WRz1PoZi1Q8Ly4oc4OsscgprzEWzk/8SgiklBmPzjhh34typiXRWUWgsnt64rREAyqGnTRfS82+u6OiOK5QQApd85fiLzjyWEIQxymX1ZxYvt0kmzbfj5kyeOHpIKpPzedyfbtm98pMvoC+4rJBq1NoN21av3xL0eZOZ3MxJo46aPl7OvUhoWVu+ZIJ9tvJAn0MhrwCDpcxv2LL7tXfXBgPenKYHfL67r7tURlL7CEBSSnRDHz204Rvnntiyr6UrluxsavnnC0vltDnnDz+7RFUIACTS+R9c+pWZk0frhq5Q0ocyrFBqGOzYI6d862sLk6kswRhh8sDTb5qkdHoiioBAvfMrQiBEQ03luBGD8pqmUJrMZNo7Yw6GGzglTUTrrCmjK8KBvG4olOxv6zQMplIlm8s/8PQbHpdLgEjntJu/dcHE0UN0w6C0D5SQkELwtONmXXr2CbFkmlKkG/yvTy4GgJ6HuSgH+SldUYQBYGh95YjBNbm8RhUcT6Y7uhNln5R7Y9mH67/YttfvdeeyuXNOmgsAZy2YTSlGgPKa/tRrH9jvHkgrhGwHStXiOR7UU1+moYKf8MDeXhx17J8m0yeOFJwDIA68qa2r7HSFEJSSbE577o2VXo8rlclPGTd81uRRsyaPOmLCqFQ65/e5F7+/trm9y8a4GMzwuFznnjwvn9cpxfm89uyiFX0PRy7ms2+sSGaylBBN189beLR0whYGJQY6u4GS+VA0DBYrvvDWylgi7VLURDpz1olHTh03vKwrp6RJzN63Ljr1H7+9/k8/u+Zvv7n+1GNmAgAl5KP1W9dt2uHzerI5bdTQ6iu/ejIAEEL7Hbp0BHzn4tMbaiuzuXzA6/rosy2fb91TVoUeCBmkxu7zugzOMca6ZmTzurOPwmbsh2FNLTMSDHjcbsY4xiSVzuY0HQDeW/P559safV5XOpOfPHroZecskHRAfXeHkMSa/eDSM6vCgZxmBLyu9z7asL2xGSNkJ/QMfCPJXaSq1ON2cy4IwprBdN0A6Cl0hPSGP/X6e1wI3WDRSPCck+YBwAlzpk4dOyKVyQR83jc++Fhuj7LZVH2N5MA/fGjMhoNq/wOCsiIUQBgBcAQom81DORJJtXrpinUbt+3xe73ZbO78U+ZRSimhZy2Yk8nlPS7X3qaOl5d+ZH1YYEQA4OyT5lZFQ7m87vO6l6xY19LeTXB5EIxEsXQnUoveW+v3eLN5bUhd5VknzIaS1J9DvBLikJAYgyUg3lv1uaqojHOXol54+nwYWAKU/Ex1ZeTqC0+9/LyTrvnawtOPny3l9LKVn2WymkJJOps747gjo+EgY3wgSTwIIcNg9dUVpx07I53JqVSJp1LLVn4KlugpIWm/lMAY5/Jac3u3Sghj3ONRgwGPc0UcBlc/Xcn3p7M5TdcxRlwwj9slQydLV3yq6QYmOJvXvnLCkV6PmzE+EBpijBjjo4bWLzjqiFQ641KVrnjyvdUboIdWPyA0oBAAEE+lO2MxhRKDc69b8XvcRRMFABOaSLbtbnrnow1BvzeZyhx35OSxIxp03fC41XNOmpPL626Xur+165WlHwEAFwPLyEMSzoQOSAiIPn/8b26mXwIdYNh94K5k+cnG5nbOJFgaopEg9DCTbJv0ydffBwSGwaKR0NknzZV/PfeUOdXRcF7XVZU+s3g545wSLAQgDJzz0UPrT5gzJZXJed2uxpa2195dDZb4K2nyl0s+WLdjT7PXq6bS2VPmz6itqmC8eHsWIWG/HHnFoQkRAoBEJMGOvS3b9u73uNVMLj98cPX0CSPBot0AF8QwmMGYYTDGmLSb1q7foiqUMeZS1RPmTQNwxMAHMEcAOPGoqZgQLgRBdM3nW6FH8udATDopPT/bsmvTtka322XoLBIK1ldHwSmREQyQnlKCfLZ5Z0d33KWohs5rqyJul8o4X/fFDpeiMIP73OoJc6f0N65i+oGQ8xWABCAEaPVnWwDAEZnuiU4sP2A5308+37mjsdXtcuu6XhWN1FRFoMcJxE27YHlHV1KhBCHx1dOOsV9y5gmzKyNBTTNcCn1m8QcAghI8UHSPsFzuB94kKayogujj6/9KdW/OOSbYYOydVetVVWFcUEIG1VaWmTvnGOMtu/a/+9GGgM+TSKWPO3LKqCH1jHPG2agh9ccfOTmZzvp9nk82bV/+8SbpW0TWvrrwtPkUIy44JeSFJR9CuXxSIYSEAT/7xgcYI864200vlOtuN1Tyk0nqAg6ur0XpuaA2jvAQNCz5Y19LR3c8oyo0l9fGDx8cCvgYH5B2YE4JIUoJJYRSgjFGCGVy+R372lSFaLoRDfmmjh0KNr5jIMNCGAAmjx5aEfRruuFSybbdTZpuWAvgoIoQXMaVGJdfBmPm95xzzhVKdcO4669PawajlGTy2emTRvo8bsZYcYpBQWiJco1xbhiMUiKEePCZNzHCAguDsSljhwNAZ3di9/52l0pzml5bGZ44Ws53oASUn5w+YWTA59YNQ1Xo5l3N4GQ4GzsEYDBmBVWLvzjnQiiUZnLa3f94hnFGCMrl9TlTR1NCSlIRhRCU4Ewu9/ybK3xeVyqbGzN8sJSzlGDO+Zjhg46fMzWZzvh9nk827vxg7SYAxNnAlawD01aQRX9pIGMzqoD6+Dp0Wb629+oAbV4EAAIhJHWWAuMVvpiMY2KE/vj4K2s3bPd5Pbl8flBt5ewpo8FCq9tNGhDPv7m8M5ZUqAIIvnbG0eZqCQCAr542HyGEAOVy+jOL3i/hn+PnTp0wemgmkw/4PB99umXt59t6elGEEAijz7c2frB2U8DnTqZzMyaNPmrGeDt00IMMSFieEIxQ3ytSNp/40FqWVFJib3MHNyWUGNJQeeBrV0wRhFo7upPpDCFE04y62opIyH/grAC1VRWRsH9/SyeltKM7mUimKytCFg2QlPyYYFVVAMCOwpUM5tMvdt72+yfeWb0h6PMaBlOoevk5CyxKWt5Fk1ttfE2ZzYABIQoGYzf95tH3V38e9HsNzQj6PWcumAUAbV2xTDZHCMlr+UG1laGADw6kqIj8ZG1lJBzwdSVShOKuWDyVyfm9bsdcQAhBMVEVijFCuIx3jAux6tPNP/3DE6vXbQ0EfJqme93uy85e0HMwEoazdMVnW3buj4T87Z3xs06cE/B6JEZXWn8XLjz6xSUrMUa5fP4/r7w7f+bEgU3moNgGBOfC53Gt+mzzN276g8EYsgImzpREgQQSMmctd/L8aVdfuLDfxKMDGcMBfl6iioUZiqGElOCwJFauvTv+h8de+fO/XvX73JTgjq7Mdy4+PRIKlKRhSddSJpd/ccmHPo8rnc2OHT7ohDlTwWFYLJgzbcLIQdsbW4I+95sffLK/tbOhJiphvYxxj0v9ygmzf7Fph9/v7o7nnn9z5cxJo0tUHtNhvWRFPJGprgzpycwFC+cjE/NcxmFtMB7wuV9c8uHmHfs06QnthdgE40Qqc8OV58w9YnxPSFov2N8DbmYAK5PNCzOlBSpCwS/bK0A2p+m6gRFmgod8PlVRnMMeQEMAoCo0HPDv2d/uppgxkclpzj8LAYTgRDr3m4df8LpdnHNUUD0FCGjt7N64rXHdF7vz+Xw44GectXXGbv32BcfMmsQ4xxixYi8yRpDXdMZ5VyxFKXYS12Csqa179fotj7+w7NMvdgb9XkxIe1f3NZcsnDp2BAAk0zndMFSqMM6ldD6IQh8elxoJ+du7EwTjvKan0tmCwEIAAAqhHd3xX/3jOUqIiWAUEt0nBIfWztj6Lbs++2KXYfBg0MsY74onf/mjS6dNGNkThynH9p/X3hWCG4yFg/5zT55r/94Olo8bOXjnnia/3/vmcnN7/LcVMEFcCIWQppaunXs/AIGgpxfMNH0QwTgRS0YrAuZKf7lTvEwq4IAb49ztUl5YsnLr7qZsLo8RNhcEAQKIJTM7Gvev27Rrf0tHJOQnhLR3xo+cOubGq86DHgq4PELe/OCTTdv3RcOB1s7YOSfN8XrctigxGPN41LNPmnvHn5+qjob2NXe+smzVty8+XWb/ykU595Sj/vzvRZpmeN3u199dc8u3LnBiqiW6PZXJvrzsQ69HzeZyDTXRM4+fDb1aA4IL7lKUL3buXb9lFyDUh6FPCYl3x7925jFQVLTDsvEPkdfdFFgYy5RUBAABv+fL9WnicRBCgATnwq6tMfCqF/JThBC/18UFE6Ag5Eg1sPwcBONMOvPbh14A+xgWZlqTHINCid/nVqk7nkwxzm+++oLbr/0vYSaaFL3QYDwU8L741oeL3l1bugMQcM6zOS2RyrhVJRL2cyZa2juPnTX5l9d+3cRGIYQEAhAIDs61ggBAVanP42acK5Tm83o6mwWIFP4sBKW4K5G6++/PgKOymkzgQ0gAxi5K/T4P4zyWSHMQv/jBJTdffT53FKgwH7F8JVJVTKSyJx11xOQxwxhjCCEuBAgwGPN7PWctOPKXf32q1u9taut6eelH37nkNInL+3Ic0jclBEbYBgCZ621/jwAEYIyB4IF7GPon/cE9iEBw4XG5nntzxZOvvSfzzwVwEBK2JmSiuM/jjkYC6Uw+lcnMnzX58V//OODzSLWouDcEAE++9h4g0BkLBzznnXKUvV72x84+ce4fH39FNwxVpU8v+uCqr54q0x5kxui4EYOOmz3ppaUfRsPB7Xualixfd/6pR3HOZZKDlInvrNqwZef+SCjY0RW76Ixj6qsrGGMY4x7+deGcqWQhWY7Q1BcK3wAAYIIBYVTWXyVK+jv4RuULOOP2SRVPpr98v9wssSkIxulsTi7eAKtemHNDYBgskcoQTGQdzOKinVYCE8KmM9j8hfC43KpKpYVrGEZHd7Ii5J8/a9L3Lz1j4fyZnPNSz4GVo48QYoxlmOUvllls1iISjKPhoKYbnbEUweiycxb8+obLwwG/PABlXRITWI0QwIGe+wIA5TUtlc4SjLgQbrfq93kdf7ZKn3CuECxTiyW3BL0uSggTggDK6XpHd6IiHFgwd8qPrjjnuNmTOZcpPqW+EgzwzKL3O2KJmmgFguxl5xwvSYELqfYEAC48ff4/nlqc1w1VUZ5Z/P63Lz5VThb1PZUDPE7lEwiBphu1VZFJY4ZxxnuRWQKBwBgn09lJY4ZA2QI2B9jsoaIDiAoBFPQIQRASRAHEkCACsEKJ1+PiXEj2iSWSClFGD6v7r7NOuPrCU7wed0+FVxq2m7Y1vr96Y8jvjSXTZ55w5IRRQ7gQ9vGAAbgQE0cPWXDUtJfeWhUJ+tdt2vn+2s9PmDOVc06skMiFpx/zyrJVQggE+NnFH5x/6lF2Up2k6LOL3pfp3C6XesFC6W4v5xBESHrfsoY2emj9qCF1mm7IelAWw4KV2CYAgBASS6Zrq0JQ7H+wSqV9qbKrdjM1LL/PZ8vKzljyy/frc7tUhWSyOsE4lkjnNN2tqgfEDABIM4x40tzACiE+h0NHUoJzcCl07rSxikJl5r2ikA2bd7d0drlVdyaXHzOs7g/fOHvK2KETRg2VztFeygwKEIAA53U9m8uYybqOnYARIgRTgqorI6fMn/6N8xacOHeaDIvIcz4S8isKYYwTjNu7YqLH+TmQlslpnfEUIZQz5lKov8x8ucfjOmrmRBtiQylet3FnZzzuUtRULj913PAfXHrmpDFDxw4fBADy5CyZsfSVpLO555d86Pd6srnc4LqqMcMbmlo7jSIchhBChPzeaeNGvLfm86Dfs27Tzg/Wbjpm1qS+4cS9VW/ppyEgGGdy2rzpEx688/t8gLmEAr68A+tLiDyEMc5p+pRxI2qrwrrBQCCF0ub2rvVbdnlcqsGESvAvvn/J0dPHT5sw0udxy4wdXC54B4Cee3NlLJmuqghiBMfMGh9LpLoTaUqILR4Y56GA59hZk15d9hFGSNONp17/4IQ5U+UMJMudOG/q+JFDtjU2B/yu99d8vnnnvnEjBnGrmuOOxuZ312wMeL3pTG7ahOHzjhhnp+n0pC0gIISkUtkLTz36uivPlUGn/pekl4rBh8gkRAAAwwZVEUoAGMZkV2MrfAlxKJ+rqYqEA75EMqMotLm9qyuWrK+ODlztkN7KfS0dXfEkpTSv6dXRcNBvaxymbDeYUVMZeuK31wf9Hqs2Dv5g7cazvn0nF7pCcHNr9+hh9RNHD5PBfkpwLwNAhOBkOjN/5sSvnXFMKpMrLCFCQnC3okTCgUE1lcMH1VSEAyC1dAQYmaDT6opwwOfpjCUVhexv7epOpCpCgYG7e+Qn97V0JlJphdB0XquOhn3Oo1iY1RpGDa179v6bpRXABScYv/H+xxde+2uVAqV4X0vn2OGDxg4fZDCGQGBMe77f9JUs/2Tbrv2hgN9gRiKVOfuau7go6G32hzFGmqa5XaoQoOn6E6+8c8ysSf0hFmzb4UCaFf3mzIygo77eIhlEoB41Wr9UO/CeMEbpTO6GK8894/jZjHHpCclkcyd947YNWxuDfncsmUYgjpoxUWZ6EozLJTAIQnAqnXtp6Ycel6obht/rue+Rl+579GUQQgip1AgEMmlGIEA+j0djhs/rXrpiXWNz+5C6KslCjHGfx332SXPu+MuTAV+4rSP+4pKVt1zzVZlZhQFefOvD1o5YdTQUT6XPP/VoSojBOC2b6W3aCQIAuOmH6gdKYlbFKjkgZWeHyO9pHr+DaysrI8G8Znjc6hc793V2JzBGA0c2c84Nw2CMyYi7ENytqqOG1ud1XVVwVzz5ycadYNfZGUiHggPAhs27uuMplZK8po8d3qBQYteKktYzQlhwoRsGAMhcak035s+ceP0VZ3fHMy6X2p1MXfmTP3bFUlZSYW9UEwhBXtPGjxr8X2efcNWFp3zrooXm14WnXnPRad8476SzFsyZMWlURTgg6znJmLttToaDvhGDanJ5XVXVtq7uz7fuhh4w1z4JKADg0007U5kspUQzjImjhwBAISyATCnAudANQ8KUAEA32KnHzPjO10/vjCc9Lldze9e3bvtTOpujMvGsnIkjOeepV2VQnBOCGOfxVCaVyaXS2VQmk8pkUpms/IonM3ndUBTCBfd73Uve/6SxqY0Q0uvUxEH68Iqz1uyrG3pr2Cp3c0i2gWV7HvjIZU64XAsJK9ENw+tx33frlW4X1Q3m87hv//PTb7z/sZk6W/ZeD84B4I3lH2/Zuc/tVpAAjEkqk0+mcqlMPpXJpLO5VCaXNNcll0inMcEYsEqVprauV5aa6fc2Fc875ajKcEjTDY/b9fKyj/KaRgmmBGm68dLSVR6XqulGXVXFWQuOBADSi4oqiqSTvTp9NYzKGDDSdDtU0FGMERJCDK6rGju8IZc33CptbG77aP0WABADQzZL84dSSgiRkV1Ju9lTxug6R4joOl+68lM4oPwSAAB444NPZBBbCDFz8ihwiABhV1dBSOqfsjqiQgkIceNV5x03e0pXPBUJ+Tfv2PeT+x6DvnDJwi6IK91khlXmwf6SsBpZcAFj5NR4ETKxmjMnj9E1nRKSyWlLVnwKfb6yxzIAACx+/2PpI8CYHDl1rDm2HqSRAA45DHk2/uy7Fx05ZUw8kYmGA2s/33HHn54Es3BCaeNcYIy/2NH4/prPfV4P5yKVznUn0sl0Jp5MJVKZRCqTSGXtf5PpbCyR6Y4nOReKorZ0xF56axX0efaUBfMMrAlUcCcdgmJv/2MNF3EgNRg7csrYG755biyZVilVKb3xN4+1d8VsvEgpxRAGgP+8+p4AwAhrutEZSyRSmWQ6k0hlkqlsMpVJprPyK5HMJFP5zlgip2kAXFWUZxYvN8szCIEx4oKPGzHo2NmTUqmc3+v6fNuet1etlxR9f82G9Vt2+32uRCpz8vzpg+uqeO9wS9Tj+4O3nA8dqSlYNsLJR017Z9UGjL2c82cWLT/t2FkDeV46C3c2Nv/piVddLiWT1SaMGnzN104DgJOPmvabh57XDcPvdS16b/XN3zq/tjIyECuJC04I2bW3ZcnydX6PO6/rFaHAyUdNBwf01PQjFtsvAIAQMMYppb/7yTdPvfxn2bxeEfE/8dI7x82ectEZx/RWcLngd8cIACjBBxQIkzNaeOz0Pz/xqmEYPo/75bdWXXf52ZEBWYVCpshs2LL7vTUbAl53TtNqKoPHHTkZyt5hJUpfzTj3uF2/u+XKM66+Xdf0irD/b08unj9r4hnHz+5Z+FCWSXp28YruRLo6Gool09d/85wT5kxLZrLO4hmOzwuVktb27ht/+5jOuNutPPvG8u9+/QxKsNT/y8wHQKZnHpyidTAP/T/WCEZCiOuuPOeDjz9ftnJ9NOLfumvfjfc+9ug9P+zpFJFW/6ebdy5fuykU8KQzmemTRt/2nYtymm65QYuekYA1r0e97+GX3vloQzDgXffFznc+2nDyUUdwLgjBQnBAcNFpx7yybBVCiDHx3BsrFh4zEwCeXbxC1xkAVhUqi8n0ZXX/b5OxbKNg7bczF8z53aMva5oR9Plee2f1yk++mDd9vM6YIvOVetl1jDGM6Z+feO0PDz4fqgjFO2M//eF/AYBuGNMmjJw7ffzbKz+NBAONTe1//ucrd/74MoMxhZJe+NKqjcU4pviP/3ylvSsejQQ7uhPnnjRv9LB6blfCdq5jKV0RIcRgbNLooT/7/teu/eU/opGQ26X+5L7HZ08dO2JwTTlvrnmwH3Q+iVS45k4bP23CiI837AgGvNv2NP39P2/c8u0L+pyv+VYZs7vv0ZcSyWxlJNDWnTj3pHn11VF5hVq554oOPIKxwdjsKWNuuvq8W373z+qKMCH4hl8/Mn3iqPrqCieuUrrbk+nMC2+u9Hvc2ZxeV1VxzcWn10TDfQhW+adX313zyrJVkWDws8273luz4YQ5UznjpBexLswKyQMlICos6f9Kuo2zztqBtbJONISwZLPf3Xzlgst+msnlouHgU69/MH/mxCvOP8kJ0RRWxPaFN1bEk+maaCimZy47Z8Hxc6b0XkQQ5J+64+llq9ZjhAzDeOq1908+6gjpcpKH+gnzpo4fOWR7Y3PA71m6Yl1rRzdC+K3l6/w+VzKTmT5hxLGzJkGftzrKClbokCwKkiX1D0Fqu1lJinE+emj9OSfOjSVTioI13bj5N4+l0lmFEIP1WiNNN5ii0HUbdzz5+vt1DTU+n7e2oearp803lwLgOxefjhBiQoT8vr899eayDz9VKNV11ttNCkKArhuU0peWrnr0haVBv09nzKUo3/36aVDM/31TUBbGufrCU88+aW5XLO7zuls6Yj+++0HRf32Wg6SozHO+5mun6YwJwQM+7x8ef+nDTzcrlGq60ZtbR3ChG4wS8sRLbz+7eHko6NN0FvC4pYpaJnzVy/hl0PDHl59zytHTuxPJgM+9e1/rjfc+UngGQFZBAoA33v9ky54mr9eVymRPO3Z6TTSs6Xr5XB/GGeMS33zhaccAwghz3WD/eeV9KO66HP0OzB1ul1c/OPp/uWZJywNc/D5UZ4yxwdjoYQ2//OHXM1kNAfi8rl/86cnNO/ZRQgo5yUJQQmKJ9MvLPvJ5XdmcPqSu8rRjZ0qC9LYikkonzp02elh9OpML+nxvr/p0z/5WQgjnIGHrfq/77JPmZnN5t0L3t3YuWbHu7VWf7W3pcKtqPqedd8rRhGDG+ry8Ftn/O7A89nJtoOVQ+m1OCwtuueaCwbWVmawW9HvXfL7tylv/mMvrlBAzKcq+9UAIxrjBuEJJW2f3927/WzqXpwR3xeInHX3E5LHDOOdUIZzzU+dPP/fkeZ2xhKoqAOKbt/zxw3WbFYVyIQyriKl1kwJnjAkhVEVZ9uFn3/nFXymhikK6upOXfOVYWXZ54FcD2cvw6xuvGFRblcrkKkL+xe9//NuHX8QYc8ctDz3IeJAExRgJwS88bf7CY6Z3x1Nul5I39Ctu/v2GLbtUReFCGE4KmvPlAEJV6EtLV/74Vw96PW6CUWcsccUFJx8xYQRjvMgbasGwoJwYkGhPhNDvbv5mNBzI5LSKcPD5N1b846k3HPM1MVZPvvY+Rohz7nEp551yNAAQQmT1xLJfqkIB4MSjpo4bPiiV0YI+z5LlH+/c20IIKWP2OSl5IPtfZkPZVRUZ571t15L0yYNbr3KDRge6+H1LV0qIwdll5yy44NSjO+IJj9vVFUv88K4HNN2QNiPYhROWf7xtT7PP40llsguPnVlVEWKMU9rrolBKDIMF/J6zFxyZyeVdLtrSFntxyYdg+Z2lLXneKfOioYCmG16v95Fnlz787BKPR81ren1N9KwTj4R+vYSOpZQzHeCi9F440HR9DmhlWVkfrCWwZHHohprovTd9M6/pBuPRcODVZR+dfvUvPt+2hxKCZQDAjAUgjBEl+POte8665s7Ptu4O+ryZvBYOBm666lyT/azs17uv/8aYoXXxVNrndsVTmfO/d/fDz74JAFaWdCHsQwjRDfanJ169+Ee/yeUNr1uNJdJTxw2/40dfhzLhFWHC8nqhOsaYMTakrvLu6y/TNJ1zEQ76fv3Asx+u20wIYT1k1sGBhwqPIyTB57+56Yr6mmgynQ34vE3tsTOv+eWTr7+LACQNnRMmBGu68duHX7jy1j8LgWRVmaNnTPj5974GZloscr7AZpyy3iGCMWN8zPCGO679eiaTBxB+n/f2P//7s807CSGcM4lB27B19/JPNgZ9nkQ6N2PSqHlHjIf+ktIlntbv9Zx5wqxsLu9S1dbO2ItvfQgAZUQ/Kv5n4E0ImZenKEQuXx8ytPB1KG/MPsjF70vPAgwA91x/6YiG2lQ6Gwn53/lo/T0PPAuA5PElz6T/vPYexsA5c7mU806ZB/2KEitr+vyFR4WCfl1nbpfy3FsrZW1bmd4sBB83YtD8WRMTmWzA616/Zde6jTsCXm8ynTvp6CMG11Wx/q8ak5cNAQJkl1QdyKJYoPliKsmrYbBZz2pA/ZSjQqEYpuT4c0+eu7f50pvufTQU8FZEgqs/23rSpT8988TZZy2YM3H00KDfgwDiqczW3fsXvbPm6cXLM5l8OODXdSOTyT9w51UTRw+zbW+MEGOsobrisXt/eNY1d8USmXDQl9Xy3//lA/959b2vLpw/b8b46mjY41Iy2XxTe/fytRuffO39dZt2hPxev8fVlUjWVkUev/dH0VCw/G19/XGSFEznn3LUOx+tf+iZJdXRcDyR//GvHl7y6B0yN6Kory9vXWPEOB85pO6xe354wbX3JJKZcMCbTGevvOWPjzy79KLTj541ZWxdVcSlqplsbm9r5werNzy9aPlnX+wMBwOqQtq7Y6OG1D/0q2slSFoyU48kIaeGVfpHQjDn/BvnnvjOR+uffv2D6mioO57+0d0PvfHQL1RV0XVGMH528fJEIlMVDemaftaJcwjBAynTaOWpzfvbk4s1Q/e43M+9sfx7Xz/dpSo9nV/Iyg44IIpKjI+q0D372157Z3U2p5WKbOdnzVQloVCy8JgZMvv9oJuwNaxD3TBGjLG66ui9N13+tR/dazARCQV+/8jLx8yadNzsyZpuqAr9eOP25R9vDvq8yXR21uQxR8+YCH26luwV4ZxPGj1s/swJi9/9OBLyr/9i1zsfrj95/nTOGSGYcUEInH/KvFffXsMFUxQKQjDOVYVcVHDa9D9lwbmqKht3NL75wbp4Kt3vCcEYi4b9C+ZNK9I/BXAhKMHvr97QFUvKK5EcqwkOq1PCdyCvGeNHNkwbP7KEwYqq92KMDMauvewrbpd68+8eSyfykaDfMNgTL7/zn1ffDfv9FZEgQqKzOxlPpQ1DhAOeYMAXT6aFYH+87eqvn3U8Yww7uJ8QzJgxY+KY1x/8+eW33P/5lj3RcKAyHFz7+Y6Vn3zh9bijkUDA544ls93xZCab97jV6mgorxnNHV2zJo1+/Nc/Gj28oWeoC6wUGOivRKWc6t0/uvSjdVu2NTaFg/51m7b+7P4nfv+Tq4Xj1hxkhtG/rFdQ3ip49MyJr/79Z9+46ffb9jRFI0GvW1392ZblH2/yuV1VFSGv1x1Lprq6k5m85nW7aioj2bze3NF97KzJD939/WENNazHfddgHbnFwf5eh3rvDZev3bC9qa0rHPSt/PiLu/7+3O0/uFhRaDyVfmXZap/Pndf1uurIV06ws177AfQihLjgU8YOnz9rwhvvfRwOBjZta3zvo8+t7dFzdQ6YkHIHej3utZ9v/9qP7gV5t1q5D0rdGsubn4O+TYv+qpaTmwf06v5pWu6xgYDs5al5xvGzr75w4Z//9Up1tCKXz19390NvPXZnRdgPAM+9sSKdyfoqQnndOPeUuX0UTigZM+McA5x/ylGL31mDETIYf/L1906eP12yiRR5J8+fMW5Ew859rW5VAYzT2dz08SPnz5zYK7q9zFtEwOd5eemqZxctRwiJsuLd/A3GCFKZ3FHTxy6Yd4REActPyqobbrfr7r8/Z5Z1Emb/1vU65rLaREvEktdffd608SMlhsF+VWkGv9SzvnXRqW8+fPvcI8Z1xZOJVCbo8wb9PiZES3tnU2s35zzo84UD3lQm39rZPX7koJf+dtuVXz2FcYZJiTaICCGM86njRix97M7vfv10TTfauhIKJZFQwO1SEsn0nv0dqXTW7VIrQn6EcFtHDATc8M1z33z0jtHDGxgvF4oSgguwryfpg+KymGco4PvtzVdghLM5LRIMPvDUm8+/ucI2DIWwrgMRBxuLL2JQzDifOXn0sn/effn5J2Zz2faupKLQSCjgcqldieSe/e3pTN7rcUVDAQBo7uhWCPr59y5+7YHbhjXUlLrqZNqWkBf5mFd7WmdXOQsfY8ZYTWXk3hsu13VD01go6Lvv0RcXv7sWAF5d9tHmnXu9Llc8mVkwZ+rQhhpmxkz74V2EkMQPfXXhMYwLECKvs3+/+i70NNVljTfB5b+9jbPHesq7OYW0W31ej8/j8nncfo/b73H7PG6fx+33ym9cPq/5vd/j8bndX14xEty+jFYM3PEmrIt/+i0lKGXHz39w8fSJo7rjSb/X+/nWPTf99lEA1BlLvLx0lcelpnL5uqrIV06YAzDQZCPJJ6ceM2P4kLpkOuvzuN9a8dmufa1S0ZaGfNDvPfOE2el0Vgr6XE4779R5lNKB1MIVEsonBGNMIcTvdfs8Lr/HLekv18VvLY3f5/Z7Vb/XHfB63G6Xo8QYCGtzMcbdLsXvc8tOfLJDr8fv9fi9Lr/P7fd6/Oaiu9xet1ScS7Zkz1vRkdxys6eMXfTg7U/ed8Npx83yedyZrNYZS8aT2UQ629GdTGWylNKjZ0x44I7vvv3E3QvmTmOcE0zKKSmIYMw5Dwf9v//JVW8/cfcPLj19UG1UN4yueKorkUpnc92JdCyR4hxGDq758TfPe+8/99x93WU+r6dXRzsChRJFoYpCFKVvHRURghljxx055borztF0XVWpz+v+2f3/3tfS6VIVAEAYKQpVKFUVSsmX94nI+YrqaOjvt39v2eO/uvrCU+prKjVN64onY/FUOpvriiVjiYwE+N367a++959f//Q7F0rffA/xIRV3oRCiUqIqVKGk3wgp4/z042d977/OyGl5t8vlUpSf/OHx7kTq5WWrXIqKMA54PReeeQwciEIhd9HJR02bMGqwprOKsP/d1Z9va2zGGBcb10hVqERLKgMmJkKgqkRRqEqpQqm8Qkne1STRyKYDWrqcCZbfY4LJQG5k7+O9FsVcCqWUKgoZeGIiIVihVKFUUWjfIgYhxDgL+jy/vemb4aBfCF5TVfGfV95//d3VK9d9saepLRTwaZp+6vwZg+sqGecYBkQ3KZLCAd95p8zTGff73N2J1AtLVoKFr5Z41AsWzq+tjmCMBMDwwbVnnyhlYp+vsLL9VYUolKqKIgtzyjWgZoiG0MKPlt+JYExRSUoJIVhRiKLI/SU/JtdUXiuECEHmYmJsLivGBJe/qgb1djjISiNyYi2dsU3bdu9v7ersShqcVUaCtZWR8SMHD66vxAjLS8h7WjHFTV51bX4sk81v2bV3x56W9lginkhHQoGqisDoYQ1jhtW7VBVAMMYR6u2+IGCc7dnfJvEWlOChDdV9mtYSFY9yeW1vUzsgwBjlNaO2KiwrfyVTmf1tXTLyUhHyV0fDZe8oPdDGuRDA5XxTmewXO/bu3Nva2Z1IpjKRcKCqIjBm2KBRQ+tdqiIBB2XTGiQ40zCMPU3t8rhTVTq0vnog5k86m9vX0kkQRhg03aiOhhKpjG4wBIAxHlxXpSrkQESWedf4/paOdCYnwW511ZGQ32fh4QQClNe0PfvbpTvP53ENqq3q21iTz6az2f0tXRboDJlArp5PFS+MBHYPa6gmB1vuRo65paM7nswQjBln9dXRgK+f8kpyFC0d3bFkhhLMDNZQE/X7PH3OVMh6Mo1NHdl8nhJ5X5xLVdXueJISwpiojoYiIf8B2bbyw8l0dn9rJyWYc+HxqINrq5yYXsZYY3O7YXAhhFtVB9dX9muxywl2J5KtHTFKqAPxWBy2lg5VO/kEZH6+8LjokPpqqx+0v7Ujmc7RQomRUpXcqvuAzG4BACHOeDjsra4ohZqj3rVZIS0ReYleb5+Q7vCBkdiM4woBfZjoBmPY9A6gPrs6MIliXmHaI9Ro0+GAehvoS60ssz6kuXkwmIppr+x+4PMVwsIQ9vaBAyhPZo1B2ky9srvNxAc28i9Xgc8UpP+zEPnSmZrh2z6HIYQofw+rs5OD8KL2cyQI7nzpgQjEL1sZ8dB1UmhIDACqJ7FX9j29Zk42RgdRQcXuUFgFsxAgO8m7XGni8k0WmXNW1RjoUxYZ7dGbeEUkGe6/hfVt/BqYRZfs+Q70dVwUnJ0DnK8oriCGsH2ZAMCAHSW9zAWsWmRlhiLrQB0QMc2hHgQ+ccDU6H9GFpUGupv7o0PZVuBb4VApvhzv9cvAB8E8JRMcaCs3BiEGImPKdVZuLgMSWIfb4Xa4HW7/L7QiWIN05svKLT0KvwlWiE8hQg4AZyMtRympCcLO/DiJ9h5AlArKlXAt3zg3z/gDUkcFCGz57OSYLcAuEvLy+oEfTSBkUSf5/YBNZvO91pRRud8fRJmpXkNfooAOGdCQDu7x/jo3U9h7zrff/svfEtrXU0L0XgHefsR+NfT5sS/Z+niLKGc02IQ6JK3vWfRG/GIm7GtzHdRoBwSGcWhYfbmaRQmmnAuG7PrNB9zsqR6QfSsMxhEgcggCeQMZW/FvpUVsuZr6blxwp/NoAJMUAMC5cAJ7uBCMc1lZWpaTKYzkQKzg/nzeom9Uet+DFwBwsAAoLjiyIllyJIyZxyXFUDxfURx4ErZc6nNo5SMYfc/Gav1+7Ms53fp3e9vFxJEAgXrP6DjIIZSbgLwLwUlqiUUAAECIOiKoclF6lt7uo/MBNi5E36eUqWFJaPXSFes2bmt0u1WDsXNPnldXVWEG2ADldf2ZRe8n0jkQEA56v3raMXQAISYZGdm6a9+i99Z6Pe5cPn/SUUeMHzlE5rLndePp1z5IpjO2DCo6MQUIIdxudVBNdPjg2uENNbI8q3nRGy4vVhBCS1as+2J7o9ulFmzwIjBtCV8K2Vs2l583ffzsKWMlTnV/S8ezb6zwuFUhhK6zU+YfMWb4IM7FAO49EIbOnl70bjyVcSk0kc4cO3vyzEmjee9Fk6VrS1Yyao9n93ck2xP5VC6v6QwAEEJulQbdak2Fp6EyGPK6kBn47I/pARBAMqvtao0TVHIHqpB1eOoq/NVhXy9HlfnrPa3xWDpPcFEPGEBjrD7qqwn7DyKoKoQZEIinc3vbU62xdCqr5TRDgEAIFEqDbjUa8gyqDFQGPRhha75yXggByua17c0JaxULQ+NCqAoZUx/pySFCiM37ugyD2zXOLSoJIdDohrBbVQCgNZZu7kpT4rznHSFZntinDqsJH8DVBL2uC9rXkexIZCl2lFh0kHFUfcStUiEEIIEA5XR9W3MMhECAHfJSlJWdPeWurZIgQFwghaIxDWGCiVO2yLMCI8S5aI+n93YkOhO5dE7XDSalpVslfrdaHfY1RP0Rv0fW0espSGWPbbF0c1eaWLNDfY1NIACPRw151LDPLcsWcQG9uVgtgSU4BvLEK+/+86nFgYpQJqcdMWFkXVWFrH0BCGXz+Tv+8vTOvS0et5rJ5vOafsX5J/cLyeWcY0LWbNj2o9v+Eq6uiHUnHr73R+NHDpF4xVxe+8Wf/t3Y1G5leJSg9Qt6XTjoHzG49rjZEy9YOH/KuOES6ln2VluE0BMvvv2v55b4wwFmX/xVHJjtmbBLCUl2JW6/8fLZU8ZyIQiIupqKt1Z8smjZ6mA4kEimTpx3xGsP/tylqn0rLJIgf39q8Q/vfNDvc6ez+aF1VbJ8Ra+6q5DXc8CO5tj63e3tsQzjTHob7VTn7hQ0cb55P3IrdFCl/4hRtZVBT/8gRyEAoU2Nnas2N7lVUgDFCgAkMBJ5XYyqi5w+e2Svh6LsYW/nln2dHkVxlhkhCNI5Y/6kQTVhf3+aSylDy2haS3dq/a72fR3JvGYAIOLQ1wVobd3pbfu7qEJqwt6pw6uH1gSFLUGFAIRSOWP5xr0lkXYAMJio8LtH1IbVnpXEOKze3JTMaZaugKyyQsC5GFTplwJrX3ty+ca9bpU6qkUCRkgz2Kj6yLCa8ABn2tv0EQKD8+Ub97XHswpxCiwzv13TOaVk3KAKAeZFKJm8sfLz/Wad4tLjoViCWdYaWM/aDC9AYECMC59HGVEbIrjQlazBaXC+fV/XpsbOjmTWMBhCgC2bQgDEUsBEasu+LpWSugr/1BHVgyoDZcYjBCC0tyP5wed73Qq1Cm2WFVwS5WCC3hWCg151aHVo/NDKoKfXjVbkw/J73d5QMBz0u1xaiSTCCIcC3kgw4HYpHpf7tvv/PXvKmEljhg3ksgBVVdzhQCjoMzg3sZqAAAAjFAr6I+msS1GZELquOxQiM/0CY6RQyjj/fOue1eu3/P2pRV9ZMOfWb184YnCdwQwJVS3ZLF6v2xsKhEM+ZnBdl7cBCSdZ7X1rX+RDCUl7XBLoKMsQY4wfuOsHp+y/rbUzNrSh5t21m+7+27O3X3sJ570WgWKMU0I+Wr/1zr8+VV0Zohi7FPWRe344uLaqFypJbA5OprX3N+1rbItjQApFCiHCSkCRDE4t9uOCbW+ONbYlpo+qOWJUrXCUcOixMQRCSDd4Y1vC56YUYxsxgyybSqWiLZbuTuUifnc5/jB/VAjxqNSlEhnusfc64/xAcbbyLQbnqzbv/2JPpyGEqiC3izrhzOZIiFQHRFNXan9nauyg6NETG1RKuDAPIXnmFy5wQabWJguQlRefCKkKdXNOECqR0EwUkN+EIJdKXQoVtoCQ5g9Cirn0X8oeBISaOpPxTD7goZavBzl1OYzRzqbucYMq7J2OEFJVXOBh66nCWttDKtGpegyXCeGyjCOpacprpNrjmeWf72vuThGMKMGKSsGZZSkEIKQiLO3Gve2Jfe2JkXWReRPqvW5VWh7OfUgJ9qjUpRBha7BmrhWy7saVu68IGBJL5zq2Zzfv65o5umbi0Kqy7F3idBdWBQnWUxHhTBiMGQwrlCZSyWvvfHDRQ7e7VNovskOIwi3eJX5SzjhjXCcGRqihJlqsxiNN17O5fFc8mcvrLpdSHQ0bjP37lfeXLF933y1XXbDwaMYYJqWuNFmphhnc4Ky6MizVt+KFsxzb1neEYK/HFQr45S8xxoZhNFRHf3XDNy7+8W8oIdGQ//5/vnLM7EkL5k4tm9toX1F53a8ezOb0cMjX1h6/7XsXHnfk5N70UCmtWrpSS9btTuUMt4LNEoJ2iF0gs6agddUFQsitICZg5RdN8Yx+7ORB1nTKmD8Iof2dia5UViGYC+70gQpTUqOsxna1xCKjakWfOqAQIEEZdoRalos/oACzfGM6r7/1ye6mzqRLUVyokNVSdPWmoyiFi2AB8EVjRzydO3XGcI9Lsav6ylolMgtNPs0lPrm3cdnFfSxzxsmijh8cOB75InmAHIrMLdl2NHfLqlZc8JKsSSEAY2juSnclsxUBj71sVrEwOzJQkCayuJA9+JLm+KtVprcwUfk6tLMl9vanjQbnblWRKAheQhFwrD2ASyEAYmtTV1siffL0EZVBT+ndlwK4DV0yDxU7nwzZN2abP1sZhZRghWDd4O9u2BtPa/MmNMiS+XLwsmNaMj27kzJkRibtGNeDfv/yjzfd8ef/3PXjS0uyE8s21Dt+AiFkGKw2Gl76+F1VFUHdYFjqAghlMtlYMt3Y3L7yky8Wvfvxmg1bVZdSHQ1lsrlLb7yvvTv+nYtPZ5z3zKdAZq1E9Ng9186aMlZWILKNQmElqhcdbgIQRgJkqQlBKGHMOPP42d+5eOEfHnu1OhrO5XM/vvuhZf+8qzIS7CmjJRF+fv+/16zfVlMZ6uxOnTB3yk1Xny96u/JICIxwWyyzeO1OjQm3gjh34H0tlaGEC4VAAjgg7HHTjXvaPSo5clx9WZeKFOI7mmNCWEeZjUm2M04FYIx2tcSmjajpF5YlSiKYBwiukXyZ04031u5s6057VIULZjMusq7Y6NkpBwABXrfS3JVa+ume02aNxEUksqHRSDhETC9c6ABMW+8yH+xdEll75hD4vCUR0jl9b3tSMX1kqGTWCAECnNONXS3xioCnB7xcOEcODpRTWeoVPyJ6CEeBEdrZEntr3W4CQqWEcxlMc3rvzHh74fZGs/gauFWSzOivr95xxpEjogFvT1dmWZraJ5RzIZAlPQVwhJBHpet2tPo9ypTh1SVrinv0VeSnLv4rWFU9gBksEvL96YlXXn93rUzW63OhrJUps4Ry5yABiFIMAGaCEiWU4GDAN6S++ugZE2+86vy3n7jr8Xt/VBsNdcWTbpcaCniuv+fhZxZ9ILO1SzsVSDIhIdTuk1j/UisBihJqp6pRWbAKzItwECCMCQD84nuXHDl1dCyRDPr9m3ft+8nvHgMAmY1kE91gjBD8wpIV/3hqcTQSzGT1qorg72+9SqFU8DJbSK5BJq8v/XSXzoRCEBfmNawWdlbeHOuYkmOvYQDBhUcl63a2NLYnenKquTGy2v7OFCF2rp+jC0teKxR1JnMtsTSYqNoyPGYXinCO3zLFBthMufLu+sbW7rTbRaVmUSIFehUbCAnBPS7a2Jb4ZHuzg55F6EQ4EMyBvQ0sx7Gjq+KurSLIfaciDPStANDYFk/nNGoFMUo2pKQsIXh3a4xzXrj3SDiPM2sVeqilvU22iMTWQmOEOhKZd9fvwQDYLKSFnU5eEyYuEC/GKNhHhaqgbF5/e92evM7MI6PoY7bhWfgNKpw3Nv17BMKEcClk7baWWCpXwt5lHCv9kp0SbHCGMSKYXv/rh5vaOmVJ4l4fsMKffXdtIt+dX7K6KecGYwLg/FOPfueJe2ZPGt2dSClE8bjd19/zyNbdTYRg5yUuTgS5gB599v7Vc9SMcY/H9ftbr/J6XLm8VhkO/Ovl9/750tsEE2a9knNOCdm9v/XGex9zqQpGKJvL3/XjS8eNGGQw1oeDb/WW5lgqpxDkRKzYfiKEkX21Fzj1QJv/EGCE1m5t4T3VCiEAYE97Mp3VlAKgv6BQ2GyHAAzGdzR3m9xZPkpdTorBAQTa5Qi/aOzc2Rz3qJTzUl6QmxZJFd6RU4kcYxdcuFWyYXd7LJWzCMUdnznEKQoFQ7WncXSwTY5wR0vcGaYEKFJdpTqsYNSRyDZ3pa1IlMAYsFVaAMsimsisqQnFggAckkLiB+1/7SKc5rpwsXzjPk3nCsUOy9xS2GyguSi6bdApljgHt0raE9lVm/f3Js17StVCQMkaW5FoQwIQYAw53di4p6OE6GWvkOm1YYTTmdzU8SPOO2VeZ3cy6Pfu3tdy3T0PHfwSOl9nBcUKXwhhJC+zIhiBYbDaqsgzf7p56rjh8VTG61bbuuN3/+0pgCLXj3N+9rE4kK+ejRBsGGzGxNG3ffdryXQWIeR1u372hye27WmihEg/hywSdN2vHtrf2un3ejq6E18/+4RLvnIc68V1JR9pjaW37u92KwqTiJbCX0GWcsnphsG5AKTphm7wnuMTAhRK2mLpPW3xEraQC7+zuRsjJIon57S/JB5AobixLZHTmFntqCzPQdGOKrxjQE1ghLOa8cmOFkpsP2vRSYsQ0nSmGUIA0g2WN5i5dR2qpQBAGOU1/sXeTmuVC0d0QRD3a6paJ7rDWVjKk8ghjAsK15fzX8m3dSZzLd1ppQgzUcQbUnOXkZ/tzTH72ZzGcrqR11heZzndyOvmN4zzMiq2EAiBwXhOM3I6y2mG+azG8rqhGaYv+Yt9nU2dKZdCWPGlbZJjuBBZWe0fCd1gOd2AcrkyjAuXQrbu626PpxEqvv22xxzlAZnXmaazvG5oOstphm6Vlje1LSEdAYgS3NiRNH1EVg+0uDtUUH57pzvG+N4br1i+dlNTa1dlOPjCm6v+8u/XvnvJGX2gHGy3Ucn4pYY6EOZHCFMKhsEqI6H7brnyjG/drulGyO97/d2PP9u8c+q4ERJMD/ZrDo171LzP4ntfP2P5mo0vv726qiLQ3p247u6HXv77bbK+B6X0vkdfeu2dtTXRUCyZnjxm6D3XXQYAqE+v0MbdHQZjCqWImzAiO2RpMK5SOmV09ZDKIKU4ndU/39Pe2J50KRZtLSkpj7/dzfHhNWEbAipFUmci2xpLU4pLmMZmbpvFCUbJjLavIzGqPtIboqokNaywogNoss8dTbFEJu9SqBDWnSamYYK5EFyIcYOjo+rDblXRdGNnc+yLfZ0EY2TZQXacnlK0tz0xd3wDwUVaSpG87ouN7KTlIh9K8WcGPrmBNzOxeVdLTNMNiTLpuShOIlOCG9vjOa3OrVKPSueOryt2ZyEBQDHa3hxrizlRY+anDEMMrg4OrgwYJrin4KpVCJISc8veTkKwc7KmRw8hxrhKydQxVYMrQ5SQnG7saI5t3deJEGCTPgWRjhHWGNu4u/O4qT6nv89S1QqKlWbwsYMqJgyp1HQmj4V0ztjU2NEWS0sb2SQFAhCcIpTK5LtTueqwz0ZPFDvdC1Z62UVHAAJhyOX1kN/3x59+69zv320wHg747vjzU3OmjZ8xcWTZapm9dSeK/jYgID+lhDE+Z9q480856vEX36mqCLR2pp5ZtHzquBEFr4S08A6RfWDvlvt+cuW6zTvbOuMVwcCS5Z/87uEXr7/yXErp6vVb7vnHM+GATzOYQsnvf3J1JOTv7fpv6QJP57R9nUmFYrMaH8YAIARHgLgAl6IsnDmiOmx6MSuDMLQmtOzT3ZsaO1VCuJlgygBAfn5vRzKnmXvAFiw7WmKawVwKkd5Sp7+g5xQFwI7m+Kj6SC86fZGHveDBHRiNLTuo2y5tigpBOhlWF8dOHjx2UNSODNRHA+GA5731jTKcgpDl80VICGiPZdpiaZdCuSOcb47qwBNjzb1ULPt6HNnSL/ylWAojYJzvaokRjJEVAS6omcUnrABBCEpltcb2xJiGCpdCpo6o6TlyhFAsnW/qTDqYzaSYwXl9hX/SsKqeiGX5YFNXujORVTAuHCEgz0JhcPC5lVNnDIsG7dpBMKgyMKw6sPTTxh5kB865QvGe9kQ6p/vcSo9hmp9EAIyLkFetCfucnrsRtaFXP9reFs/YiqeFQMQGNxKZfHXYVyBjr3xWZm0LIAAAWDBv2o3fPK8rnnK5lEwuf+0v/57KZAkmvWV4HbKGAAC+/pXjXCoxmHCrygdrNhrMvPm27JAlJ4s+W5+shgzG6muiv7npCsY4EzwU9P/qgWdXfbqZc/79Ox7I5Q1VVbtjyeu/ed7RMycYzKCk/BUscjxNXel0Tjejh1Z4Sy6TwfmccXXVYS+TwkwIxhmAmD2mflR9ZHC1f1iNf1hNYFh1SH4Nrw1Vhjy6YastIIFOu1viZv/FppOsF1TCUArFTV2JeDpfqtIXSI5sM8oMrjsAin002Vs8ne9MZBWLJrYugBDSDDZhSOXYQVFZPowLKcLFhCHRycOrBlUFh9UEh1YFh1aFhtaEhlYHh9UGB1eFDMahGK1qbwsBfUkt5BxW2T8BOFFRh6rJbdjclemSXsvCtpSxf1Fqk5k4PGS6FxHiFtLCbtxcUG57E1FBeUT2Ujo/z2U9WC4AYF9HQue8B/YOCYGwEMdNGRoN+pxXZTHOh9aEZ42p0Q2GHf4sOV6MUCavtXSnnPR0Bi7sg0Gan9zCPRicUUJG1IYNJiyEHXI6D7NaUUCvSMMS1mqXPaoQkrxumjqc85uuPn/FJ5veW7OxMhxYs2H7z+7/9323XMm5RAYUUcJyppWySIlrfCBNEmvm5DGjh9Rvb2x2u1w79jY3NnWMGFwrCjobAuEI7SB7LQ+uIUoIY+wrJxx59UWn/umfr1ZHQ+ls/tb7/jlqWP2GzbujFYHOePyUY2bccNU5QgiCKfTyNvm79li6zJ8Q0hmrDnlH1VcIIbCVt0gQARB+j7Jw5ojeR+gwKgE1daa6UzmHo6Tg1XUrKG/Y8gKkCUkQymrGnrb4lOHVZUL4Jh0tOIipywy8cgxqT2Q1nakKLlljwYVLIROHRq01KkBPAMQxkwaX71EAQtCRyEqXQlEQ3kHkXkZT5IArRjYUPV9iSEMvm+KA2o7mbs4FECIER+ZJigQI6aLVdFt8mKoBxbi5Kx1P50I+N/Sw1rE1NBOgY7omTSehDXax3Y4OXQ4BQHt3Bglk6bmFP2iMDa8JN0T9XHC7hKapEoIYN7jyi8aueCZPHdqQBZ0RrbH0yLpIgSNtxdERp5ZhBIQklE9Iq59gbB+VTqxCT6lQegdyX84JJOMFZuMCKCX3//Tqyog/nctFI8EHn37j+TdXEkIMxkt7cVq1hZdZ50RZB1dvo0CIC+F2qeNGDdZ0nRKczuR3NDaD4+g2+0col88JEJlcvtxXLpPLmd9n87y/G+5kvO/2718ya8qoeCId8ns/2bTzyVffC4f86Uyuvipy/61XSwWzz52MACCWzpVDTgHjoq7CTzByZM7Zk0b23MpFNou629kcY9zmV5PRuQCVktnjGpCJPDa1TtkzxWhHc3d5OVT8JkvDGtBiyecS6Rw3sc6mVJL8anBeFfJG/B4ooAfAYmtLQSgzWZtdhOMpc3D9cFJhUwjbsVJCQScX2aEJx7MH3CRuIqsZ+zoSCrF0VWtdNZ0NrvSPH1KpmZpLYa6YQE4zdrXEofdtKezkFkeIT5QHEVkNgcF4Mpt3GJLCQryBEMhKPyrqQvofVEqGVAcZ47Y0tHAJnGAUT+egQKyCgmLG04FzS4TZPUv9oz2RlfurONYJ0vXupH0pcLTf5bafJBjJu21/dd03rvrpn1yKqijKLb99bNaU0UPqqgaWJ+x85wGwg+ACCBo+qFY3OCE4r2utnd2OLpCw3HeX3fQHSmiPFzlYEJkOzhf/9tPRQ+v7yDSSLnaf13X/rd867cqfa7rudqly1+d1/a4fXzZ8cI3BjKLXlekEhICsxkukkR3hqgx6SjmleGH6FhQIoWxe39seV4iJ7rN9RYzxykrPmIaKjXs6WmMplRBwYGEoQR3xbGssU9tLLnSJbgQCDVzfSOV0VGJEAAIEXIiwzw09sEg2rfqer4Vg7KnL99ucLiPU829g1ZW0wOUHIKPLNCEAoX3tiUQmryoljgvBQQytDkVDnnXbWrmFHrb+iCjGO1q6p4yo6S1z3pL/Jpy9kLTTi5tDzier6XlDOPMObYJQjKNBdx+UjAbcNl8VuZwQyuQY48ICcktRaivsAgmCwVTvZYqVEKDpxuZ9XTuaulSKhZM9wNQpg17VOZjS3dXPfijG11FCGOMXn3ncBx9vfOTZpXWVkX2tHdfe+cCLf7kVDcwP4AghHwCKRs4qEvRJWKZh8Fg8Y3KGPToBAqF4Mi0EAGDJfmA70R31gYUQjqtV+xoFIdgwjBmTRt3+w0uu+9UjIT8FhLrjqW+ef/KFp803DINSKvqjoc6YrhsIOYphW2E7TLDHdfBX7En36p72RCqru4rjUIAQF2JodRAAhlYHm7tSSEHAzfdKzI3B2Y6mWG3YV5rMjKw4htmTFfEa8ILxHphUYarqqIePdiCtoK6Xbsz+E4acFoqD+E6new+Pu2VSfSmg1/bmWDHkVQAA4xD0uOoqfD63Wh32tMbSCkECIbCMLEJQRzzX0pWsjwZ6wfGXyGtkzqe3wQoBCOlMcM6ciBHZD+eCUuyi5Q9dyRVej6vkqlQLRgd53eBCEAelEcKOI0qolG7Z17W3LWGvQDqvp3O6SrCFaCiEyzkXXrcS8bmcYyi55svGH5RPqO2ZlCGN3Huuv3za+BFdiVQ0HFj07prfPvQ8QpgxbsPvSiKCdocynicOSFwVjcf8Py8CI5o2BALI5LRkJpfKZJOpbCqdTWVyyXQulc6lMtlkJptIZZPpXDKdTWWyVg99y1lzI3fHUuZrhFAo3b2/La/plBIh+o912tQrlieSY2wsxMFsDfnMjuaY9BJYDAAIBOPc51aGVAUBYFhN2KUQOzcQLAOJYrynLa4ZBi6f5IEsdeyAh2R9w50QFqndHvTVzcICMBbtYcse73WMqHQz23qcs5cSf2sPMPaBjFMAQqg7lWvuSinETmcw320w1hAN+NwqAAypCXEuAGFnaEJe57GjOWaNoOcwijEGyPLg9h12N9HJZWxoC+UN5ZhQAAC2a1yUdCiKCYlKOVx6ZrN5vSOR7UxmO5KZzmRW05lbIcVmvVkSy2CsvsLvdRdVbqA9xtN7K8eu0lAK+b1/vO2qM751h6bzcNB/zwPPzz1i/FEzJuQ1nQKB3tQt+z570ZsC28dIIK8b0qAjBPu8rtKOAQDg6gtPGVJfremGXdSh4KqwqCoAMELV0RD0VxiPMUYJfWvFurv//ozf65Zj9vvcb3yw5hd/+s+vrruMcUZ7BBxKGiVYoQRAOA4fK52dc82w7/I7MJklN0ZXItPSlVIILlI2EdYNNrIu6PeoXIiKgLuuwt/YlnCaJwKAEohntL3tyZF1kaLXm0fll4R8m7WcinHPkMlrBzNb585wAkct3aSv/py6o5UE3pP9kFmhwDztv4RuJQDQrpZYTjPMEleOYxEhNKIuLL8fWRv+bGebvJREPoYBhABKUGNb0n68XFKDvUZIYhT6TCxBAEAxwgi40x1hjYczZrC+/Ll5zQAhBWuJboscF54VApbWOC0zAqFiiKIZIbVNHonD4lxgTCYPryp5ewkOq09fXclqW40QbDDjyKnjbr3mqzf99vHqilA8n/3R3Q+89dhdfvPGJCssVa6vAuJxwCwgB7m/tYNiwrlQKY1GQuA8bC1yXXb2gqnjRwykBo45wt6nz4UghLR1xq6752FCCSU4k9OEEG6XWhkJ/+Vfrx89Y+Lpx80sW8vB8QYgGCsEcQ5A7NnYuwbFUjmoCfU2AOhlI1o4UrSjNZ7XmcdRy0nSgSBUHw3ojOU05lZJQzSwpy1e8Cvb0WnBdzTHR9YVA7Isw9lyh5t9Dny93Cp1op1k3Fp2k8zke5tUb/PlAmR9GLB8JEW4dIEMJrN2sEMOCisKhpAj3dqOOZRdKShsswN2szqXhguxsyVOi+8YRggYYwGPWuF3ZfM6E8Kl4Mqgp6kzqVAz1CcAkBCUkGQ2v68jOao+0lOyl2QXFoApovwSyc+7VYUSohl68b2OCBBoOo+n8xG/u+c5In/uTuUZ5yoQsPAH9l/dKrWO/L5EnjMVyfaAFf0eUFbTZ4+tL0FsQe/VGsovjg0UKPk1IZQLfu03zl7xyebX31ldVRH8bPPun9z3z7/8/NtyKjKA3mPgCEBYe22gDCFMIJz4YvteRSGMMbeqDm+oAUs/QoUPgizaaTCOe91fptjs7eJGm46CcyDkxnsf3bprf0000tzR9c3zTo6Efb99+MWqihAh6MZ7H50+cURdVUVJieTiJRcIUNDn3t+Vsj0n1qZAGKOmrtS0kTUlvGJaB70LU7mojLM9rQmCS/UFLoAQsmZr89qtzfYvVSr1OzMEIxUKheCmrkQikw96XRaeEFnpTZZbVwCARF0MYLEALL+p5Xm19EoBoBDcGs9l87rHVXzXvBB93FEmf0sJxggzzuySKQWjQ9Oyed2lUBseLf+fyRu6wWz5W6h2AEWeLwQlyqSNtDhgNUu+paUr3ZXMUOLkQSSEQBhpBn9t9U7BhYTgGwwoKarsars9djZ3l0X2FpzUtnfOPk16H6+qEK9LSWV14gCsAAgMSAjY35EcVhMqIxwBAKCpK4kRErJys8NhxhgPeF22uVsig0qaM+PCiW0ChDgXOV2fPKx65uhaKDH5y0UJHUPrZRXKflzyxO9uvuKzzTs7uhOVFaHHnl965JSxl55zglVNAZc3ig8wZiwEYIy27tr/xc59Hpea142G6sjwwdX23Jx2ByYIJHMfrK8ELHOaEvLA04ufXvR+VSQUT2XGDGu480eXhAL+D9Zs/HTzrnDAt3Nv8w33PPqv311n7Z/ytEMAVSHvxkZkKyx2wEUh0NSZau1O10R8smi1/YjG2Pb9MV6mDKEQIEbWRrxupakz05nImgCZ4oocAnhOY0IIhLD00JgFcu1MGym9MGRybFdrfOrw6qJqcc793KOYlK0kF2ZY3CqDHhm1BLPKkDyoACGczmjbW+KTh1ZyIezTXno2dzZ1p/MGwWBGBSxfv8GMIVVBn1slCDELAGFrFghBTmP7OlJhv0dGIZCplEFTZzKT11WFlCQdIhAYo4I3DVkJ54WaSCUmcj81VkuIsaM5ZjBQiC1XbRQlYoxnTMSkWVMQ2xO1qA0AlJCmrnQ8nQ/5XCUxXCs92bE0UgT0bhfLD0QD7ubutIpk4ArJE5mDoJTsbI1NHVnjdytOlLxkyKbOZHNXWqHUKs0knDOtDLgdzFkm/mFDwmxXRHE2PuKCuxQ6d1z9pGFVQuL9oH+BhQ5C+cUYM2YMqa/+3c3fvPjHv/Wo4HGrP7nv8YXHzvC4XbzYdC9a20K4fkAnmKzm/NLSVR2xRE00mOhKzJ4yNuj3MWZlCAuw83i/lPPBaoxzSsiGLbtv/9OTAZ+HMQHAf/+Tb0o79I+3XbPwmz/LaXpFOPDcGx8cPWvCNRct7C2tUo6mOux1UWxd7ePwIyBsMLZq8/7TZo9USCFzEBDavLf7vfWNCsVOFysCwYVQFTqsOgQSlyg4QlQUv1L2jW0mRsLUa4s+ZQaSCYZdLfEpw6owgsIIe6h7jtkU/7mY3pI/KwKegNeVyOSkloEshDICIBSt2946qMIXCXjs1ByEUFt3eumnexjn2PwNFtJnD1hj7CtHjo4EPKqCczo4FBJTAFOCN+zuGF4b8rlV+UeCUU4zPtvZhjG2xYFtihtMBL2KnaopLDhTAZFfILgVLhwAW0nfaE4zGtsTCsHcUoOsXDlJH1Go3FBwE/ZQrjFk8vqu1vi0EdVlzwRUfE7DAPZSfTSwqbFDALbNNzkSikU6q6/atP/E6cOw4wjBGGXy+opN+wUXiKKSPCghhEppXYXPZjowLavSPa8zbjCOEZIu1GJZIzgXXjcdO7gCLB9+iUlYUg/LpnW5EIOAvoNohFDG+JknHPm9/zq9ozvudbvbu+LX/vKBTD5PMBZlHpZKoY1K77/JWi6tHd2PvbDU51YZB4TR2SfNsajqXKtDIKpAuq4wyuv6D+96IJ7KeFyuznji+18/86SjpjPGGGNHTBhx2/e+lkxlQIiA33fHn/69btMOCfjo2ZukfFXIWxXyMsYkxsLpendR0tSVXrRmZ3s8I00wg7GNezrWbm3yualLJW6VuhXiUohLIW5VIYTURXwBryuT1/d1JGjPMgDOhDXbfLJ3hoWgsTcKJag9nmmLZZyRJ1FaDsmUOTL1P5XVU1k9ldVSWS2Z1ZJZLWV9JbNaKpunBA+pDhlGqeUvqxvmNO2Nj3ftbo0zGagVfG97/O3P9hAMHpWqCnWp1KVit0rdiqJQHPG7q0NeAIgGvIUL3QooUEQxTmTyr6/eubs1lslpmXx+b3ti0ZodsZR9YbqTUwQTPOBRSosWlH4rALjORFbTU9m8NUc9VZisnsrqyWw+mc1zwQGkHw0a2xOJTI4QSfgS9diSgNL47cMrKIBgtKslVqaUUHEsyXYH9+ViRAgABlUG/B6XpLmF4jc9eirFO5q7F63e3hHPWBldfF976rWPdnQms5SWILQRQkg3eHXYGw16HcXXCjxjHpSAdJ0NqgqcOG3YpGGVhlEmGksJae1Of7ajzf5T3yZhn+bZANxMsv7Xz7938Yfrtqz9fGs0Enxr5ac79zUH/V7ORB8omYGodIybOInb7v/Xrn2tlZFgLJmeNWn0wmNnAgDGzqDbIRJXluvql39+avnaTTWVkc5Y8piZE3763QulHwcAcc6/e8npKz7e+OJbqyojoe5E6tq7HnjjoTu8Hlc54AySavbYQdH9nSkVevqbhEshzV3plz7cFva5VErSOT2eySsEI2Tf5GweXAIB53xwVQgA9rQlk1ndpZRx1jkFIhTH+0ptOyEQQrrOdrTEaiK+3vCS8imV0i/2dm7d3217MRwOavMiK53xyoDnK3NHjx9csamxgwmBe6w1xTiV1d74eFfYp7pdSl4zYukcAiDYLj1Y8MloOhtaE/a6FQCoq/Bta+oqWFgF1LVQCO5O59/4eJfPpQBC6ZyOABTTbWdrKEJYXpv6aLCEHe3dYtnMiBLa2p145oMt5sXdPciCrOPt3KPGeF2q/OXOlrhVFdL56iK2Lx4/cvylEP1UKG6Lp1u7M3UVRcheUx90mITFUJIyTaotbpWOrI+s297qcRG7nJzND4pC9rSnmrq2hnxuhZJcXo9nNASgUiIFMVihHrCcgBOGRMECA0KpVm6+2OCiNuwfM6hiVEOkO5lv6koWSnFZA1MpXb+7Y0RduCLg6RkVxT3m0nsbgFBBCBhnbpf6x59eHfL7dN2gBG3ZtR9jJKw7x8oSsA9cgzDLtHOMECH4zr8+9a+X3okE/YwLzsUNV54vE/2KDRSnSn/wjTFOCHn93TV//OcrFZFgNq8Fg57f33q1S1UZ58hxuepvb75y+KCaVCYbCfpWfbr19j/9G6yi4yVNOqZGN4Rrwl7NMEpKr4HpzMIYoe5Uvrkrlc7pLoUUaiRYKyEL0fjdrlF1IQDY1RJDgHpqxshR7M2s4AbIUcqt4Bm1f1QIamxLaAaznGjyJCxxWwFCiHGR11le53ndyOmGphuawTTD0AyuMaYZTNMNnXMueMTvHtMQ1nSjZ9UdAUAwUiiKZ7TmzlQ8nVcwoZiUFsaR78Ro7KCIfG5EXcjvURk385hs+KLcxZQglZKcbmQ1XSVYIcRZ5BGsXcQ5uBU6rCZYxOJO77/Do804aDrTda7rLK8bed2QRankV94wNN3I6+axYq5gZ5IU1N7CUjsr8FnfyF8CxnYpO9t0BQTAGexq7S4aJ9g5mIXROgnbd5syrMrvUeQFOdbbzMa5cFGMEO5KZVu7U4msphBMiFOkmplAGCNdZ/VR38j6sHWDZIHMRak2AAibkEmM0KyxtRhh269ls7U0aFZvaylaCXv7lJtIX6pUf1RABFPG+JRxw3/x/YvT2TzC2KUq0E/1GIQAOGMAYN2CwRmTF1FzeWcJIbijO3HVT/9499+eDQV9mODWztiVF5x0xvGz5E02RZeIIJNYjDMAsDtk3PpixV+cM8Y459YxKACEzOJuau264Z5HCMYE42Q6e8cP/mvSmKEG41YVHYQxNhirr6743S3f5IwbjEXD/r89+cbr764hBBuM9TCFkRBAMJ4zvr6kKIojSVUACEqQqhBMLCiQU7YJQIA1nU8cFvW61a5UrjWWVqitRVtOBAQ641nNsDdV0ZdmGEZRKrzEYhJCYunc/s5kEas5LSmHZ4tgRDAi8qJwq9k/YIwlUwKImaPrw36P5sj1t/eY/I8S5FIwxUhIALMDwigAEKI53RhWEx5cGRRCcAFelzq6oULTGQbELcvQGqx5awRGiJgOe25SBtnIR4QRyhlsVH0o4vfY6rA1uWLfGHApUAhGBAFGcuKYYEQxphhLCmCMCS7kXe1ujWc15oz7meqQgJzOcmYpPiNrLUdON3Iay+atcrZmkQUBSHABFKM9rUl5yBXJJkd+TPFO7XWzSrCFz63MGluvMwGAQYCzzIzlBRYKwQolEpNRFEg1P4U4B0rwnHGDJM2ctqmdsFk0KCQAgHFRG/GPaYjkteL7boQQQrgVursltqO5GyGwdqXZDsQkHHAjBHPOr7rw1JWfbH7y9fei4aBhsBJd1/rOwuZgpKgKyLztYtLnNX3T9sbX3ln9r1febWxqrwgFEILW9u6vnjrv3huvKLdUYHtVFUrleAYYJRTWdU82nW+495Gd+1trKkKtnd0Xnn7MlReczHtcb0UJNgxj4TEzv3PJafc9+lJ1NEQJuv7Xj0yfOLKuqqJnWqVkuIZoYM64hg827vcoFBAvEMay2YoxloXoLwJAGGV1Y3B1cMqwagDY3RrL5jWPqtjIA2RGmsWgykCFz804dx6g8kOEkI54urUrTQgquNIslWtHU3x4TRiKRZWDvMLMu7Wyb527qNgWFjLc5nXR4yYPWbRmp8EFxRJmhZzgLBu5btus8uwRAgggnTGvW507vl52KhW1GSNr9rUlOpM5l4J5EeS7jK5h7TnTbMQIa4yHfe6Zo+tsF2/JIyUcVeTFtZ0+xVBY+yfO+e7WGLViJwVlVgDCaHxDpUIwL+QuF0xphGB3ayKT1wv1WQUAAKEolsnv7UiNrA0X25ZOAIG837N/VKPMZxg/qKIznv50Z7vXRYXgSGAb2WbFUsFeIAviJYt5yWWFvMGPmzykJuwtgeAWhXSQ7VsrBMcBYMbo2l1tcd1gMuHfjm8KEAShtdtaB1cGVaV4r5VhRJsGX7r9+qbLP9m0Y09Tm8etFvVeeIWQ5TWSqexPf/8vj0u1Lp4TCFAinW7vSuzZ37Znf1sinQ74fdUVoVQ2l0hmLz1nwZ9/do1LpeVAT5ZrWcAfHn+5rrJCN4y+4zoY41Qm+5UTZp8yfwbjHCMkI4P/eGrxC2+urIoEE6nsqKEN995weS8dIEwwgPj59y/+aP2W1eu3VQQDu/c1//hXDz15343lwjoCIeBCTBlendfYmm0tKkWEYFFcmb7UNVMw3CCbNyr87uOnDqUEc853NSesgo1O/xRQgo6eMKhnLBwsGEpbLP3Kqu1Oc0XGsBSC93cmk9l8wOOyo2b20Hr4jKxQvMMLU8y+Qkas66P+E6YOefuzPbrBFUplTaQSUpYAr0EIeYkpxfjEI4aEvC4uuNTRuOCqQo6fNvS11Ttyed2lEN7zSHf2XOgWEGC5T06YOtjrMuP3zsxb83MW/kA4OigRzcX9F7AQrbFMRzxjl2mzwHSQZ2Jwhfe4yb0VzxEIIYyb1m1vIdbFiw5XGt/RFBtZGy4bJpQiRfq8kEM/7bUhEIIfNWEQ47Chsd1DSQ/T37kOjtt3pCXIOGMwf+KgCUOivV1sXpSJZco+8wVcCL9HnTq8etXm/ZQoQpg+FrmEhODORPbTna2zxxbdC0ULYwfASGCMMcYEl8kvQRhjhDHi/V4JBSbKgVVXhH5/65XnfvcuEEBIkU0DVqwUI6RQkteMh599Uwg7kCQAkETHqJSqLlLlCqezuZaO2OC6ynuuu+zKC08Vps5fqjohJBDG0gf//BsrDMaRfX6VLrFFBUJSXfHqaPiU+TOE4BwQJeSzzbvu+POToYCXCc4F+81NV9RURhiT1b56zBdhzrnbpf7hJ1cvvPJnuXy+MhJ5ednqfzy1+FsXLexxTzWyDmeYNbYu6FVXfLE/lzdUlRSq2ZoxfmF/K7FBBheazhoq/QumDQu4KQC0xtKdCRMaY5eHRwg0xodEAyGfi1vFZIo2BoAQUBnyVATdHbGMQomlYCEQQAhk8lpjW3LiUJflJxG4EC60altj2/QEMJOcwDLM7IMUFUgkxIi6sMdF31u/tyuVVRVpMArzliOLDxESXACWSiJAXjOCXteCqUNkArC94lLEVAY9Z8we8c5ne9vjKZVSbJdoKagGUJiaCTgSed2oCLhPmDqkJuIXJZsNCelXEoXgqNSDEJhpOra2C+AAPSPrrkn5zM7mmM4FpQVxLk8JLsSIuggAyKOxhJHkQTK8JrRxd7tJQDOMjkCASmlzZzKZ1QIe1VmLEVkLh8zkJGvyfTYkb+4T6NjJg0Nede22Fs1gCqUSC+Y4/KT6b1Y9Qgg4h2zeCHjUo6c1DK+NFBPQVhWxE6jl4AabHxAATB5WtaOpuyuVkzgeUwIIAARuFW/Y3T6yriIaLFz0awksIQAgrxu5TD7j1rL5HGOl8aNsLp/J5vO6nsvp/QosACCEGIydMGfqDVeed9sf/lURCqazecN5IZgQ2ayWzmqGwbgAl6rY4Xe5DQzGOBfpXC6VFW63a+ywQeeePPfSs4+vq44yzuRVICWbEAA0zchmchlFNRijlCpKj6XrIbrMm58pBQDBgSokkc5ceev9rZ3xUMDb1Rm/7bsXLjxmRt9pN9KZNWXc8F98/5Jv/+KvAS8jCN147yNTxg2fO21cj/Qgc/W4EGMHR6vD3jXbWxtb4xpjCCGCEUYCmbtDSLwVM4ALFnC7ZoyqnTK8ihLEOBAMW/Z153QOiBeiRQAIId1glk1Xxmq2vNR4aFVoX0cKrOJ/IA0xDpyLzfu6Jg6tZJxrBpPsK5B1k6NkXod32tzrFvgGIQIAOhOaods6JkaIC1FX4T/nqDEfb2vesr8rm2cYgWWyIyv+IoQAnXPOQaFkwuDokWPrvW6lpzaNTJnlPWvOqHU7Wzc1dmbzOkKIEuepamp8HARjnHHhUemUEdGZo6s9apkr0RnneZ0hMItZFJACpsSxcbQO/BQAWMadwQVGoDO2vTmOBGgGs6gkAIAZzKPSIVUByTBljG0EAFAT8UYC7tbuNCVYALfUNoQxSmS17c3dR4yokYc640JnBjWQlcmEAATiyDC44P2KLHMNhRDTRtY0RAMfb2/d2x7PGYwQQu3tJUz5J4TQheCcuVRl0pDKmWNqvW61RLeSqrzBhc4YkYY/kjwMCCOdcfP6W0uqUIKnj659Y+0uJJiwjjp5PyHGKKcZKzftO3POyFINS/48bFDtjKmjwwFvXmc+n10TBwEAIXjymKHRSFA39HEjGwD6VTcBAKRacd0V5+za27pl117dqKsMB+3XYYInjx1WHQ3Zx3th0QQgDKGATyG0tio0bsSQmZNGTh47zON2CSEYY7iHq8taLRg+qGbmlNFBn48LXgi4Fyn7zt8gAEEwiSVStZVhsETZs4uXM4MvmDM1lckMH1x787cuABAY954kKKlJCBfiqgtP3bR974frNgf9nlgq+88Xl82dNq6vMluCRwLuk48Y1pnIbG+ON3Um4mnNYFxWR5LpJm5ViQRcQyqDo+rDXpcq9zTBSGcsoxkNUb/F/LapgihGQ6oltftaqxF14ca2eCFB3f4fAhAildVqI968zlQTMCG5DZscXND3BVhJPFL7l+KYMR7yuR3MIs9toSp43oRBk4ZX72qJ7W1PdCdzeZ1xIUypCFilJBxy1UX9o2R4G0D0bnRwwRWKjxxbP3FIdEdLrKkj2ZnM5zSdC0ACCcQRIISwR8URv6s+6h9RFwl5XdKhXdwnAoCQ1zWsJuSixFFbUuJx7GADt4CDwvkgIA4cAGFVIe3xjEfFIZ/fRqFKga3rrKEq6HOXEZR2k38aN6jSYMJFzeJu5pog0JkrmdbsZQ141Zqw36WQgiNMCIxQXmeyVNFA9qmkYVXYc+rM4e2x9I6W7uauTDyd0xkXvDBtl0prAp6GCt/IukjIV5aA5uv8bqU67HWZ+9pUuDFCed2QpSns9wLA8NrwpKHRtpgje8kM+3JA7qxmNHel6yr8UvQVuQxlCWdkRppxsfkmOLNRQKisWVSe+mBety63H8bOLDTBSi4YcpxFCKCHOiMY4zL21McLOZd1rkv67HPBrJoNdqGfXF5XKDXNHLtQ7MCK4EiSStQYxqBLNbv3MTvTFOTjWc1IZjSdmQUaPSr1exRVofL1XAhsmVoyXmbVSEEOeVNesSr3convsuus2NaEWX90gLMeeBPWnK2eRU5jqaye05lcCIXigEf1qLQEddEfyQufyetGOq9n84bN3V4X9bkVl0KdDxzyqdnNRrTazV4be+36JZJkY2T5BOxuZPp3wQcMwik7nW88kAkW0ZALkbOYUGqWLpUE3KpbJU4ffy/9CyFKwcYAhQmgUgoUfV44pIC0A4Sl+gD0AC7+d7SDZg45Vqv8BGCE/9sY7FCOX27Ig5ov2GKrt27/GzfZwMc54JhM35/sl1BW2PAABtY3iQZOQ+e2gR4BxL76P0QBq/+t1jcTwoEvyqFtZS5Es13CZWcCcJCjLbvtCxpj2cEdLFGc8LaBP9QTwNkbKQ5kDAezusLxn0mJ/9Ob4MDnC/2HuPrrs9Tv/P9zGh7y9t+xKF++/U9oWIfb4Xa4HW6HpB180ZXD7XA73A63/+F2WGAdbofb4fZ/pv1/r64UHHhPlW0AAAAldEVYdGRhdGU6Y3JlYXRlADIwMTgtMDUtMDlUMTM6MzI6MzktMDQ6MDDfNfdVAAAAJXRFWHRkYXRlOm1vZGlmeQAyMDE4LTA1LTA5VDEzOjMyOjM5LTA0OjAwrmhP6QAAAABJRU5ErkJggg=='/>";

            var headerImagesHtml = "<div style='float: left;'>" + imgBTNHtml + "</div>";
            headerImagesHtml += "<div style='float: right;'>" +  imgIndexCalculatorHtml + "</div>";
            headerImagesHtml += "<div style='clear:both; border-bottom: 1px solid; padding-top: 5px; padding-bottom: 5px; border-color: gray; '></div>";

            PdfHtmlSection headerHtml = new PdfHtmlSection(headerImagesHtml, "www.businesstravelnews.com/corporate-travel-calculator");
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            headerHtml.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);

            var spaces = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            var footerText = "BTN Corporate Travel Index Calculator";
            footerText += spaces + $"Exported {DateTime.Now.ToString("MM/dd/yyyy")}";
            footerText += spaces + "<a href='www.businesstravelnews.com/corporate-travel-calculator'>www.businesstravelnews.com/corporate-travel-calculator</a>";
            var footerHtmlText = $"<div style='clear:both;border-bottom: 1px solid;padding-top: 5px;padding-bottom: 0px;border-color: gray;'></div><div style='height: 2em;display: flex;align-items: center;justify-content: center;'>{ footerText }</div>>";

            // add some html content to the footer
            PdfHtmlSection footerHtml = new PdfHtmlSection(footerHtmlText, "www.businesstravelnews.com/corporate-travel-calculator");
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            
            converter.Footer.Add(footerHtml);
            
            // page numbers can be added using a PdfTextSection object
            PdfTextSection text = new PdfTextSection(0, 20, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
            text.HorizontalAlign = PdfTextHorizontalAlign.Right;
            converter.Footer.Add(text);

            var htmlStringCompressed = collection["hdnHtmlCode"];

            var htmlString = LZString.DecompressFromUTF16(htmlStringCompressed);
            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString);
            // save pdf document
            byte[] pdf = doc.Save();
            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "report.pdf";
            return fileResult;
        }
    }
}