using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ContactCenter.Data;
using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using wCyber.Helpers.Web;

namespace ContactCenter.Web
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString AccountStatusHtml(this User user)
        {
            if (user.IsEmailConfirmed) return new HtmlString($"{user.IsActive.ToHtml()} {(user.IsActive ? "ACTIVE" : "DEACTIVATED")}");
            else return new HtmlString("<i class='fa fa-user-clock text-secondary'></i> AWAITING ACTIVATION");
        }
        public static HtmlString ToHtml(this bool value, string text = null)
            => new($"<i class='fa fa-{(value ? "check-circle text-success" : "times-circle text-danger")}'></i> {text}");

        public static string ToPercentColor(this decimal value)
        {
            if (value <= 100 / 3) return "danger";
            if (value < 200 / 3) return "warning";
            return "success";
        }

        public static string ToLongDateTime(this DateTime value) => value.ToString("dd/MMM/yyy HH:mm");
        public static string ToShortDate(this DateTime value) => value.ToString("dd/MMM/yy");

        public static HtmlString ToCallerId(this Call call)
            => new($"<i class='fa fa-arrow-trend-{(call.IsInbound?"down text-info":"up text-success")}'></i>{call.Contact.Id}");

        public static HtmlString ToHtml(this AgentStatus status, bool showText = true)
            => status switch
            {
                AgentStatus.ONLINE => new HtmlString($"<i class='fa fa-circle' style='color: limegreen'></i> {(showText ? status.ToEnumString() : "")}"),
                AgentStatus.OFFLINE => new HtmlString($"<i class='fa fa-circle text-warning'></i> {(showText ? status.ToEnumString() : "")}"),
                AgentStatus.DEACTIVATED => new HtmlString($"<i class='fa fa-circle text-danger'></i> {(showText ? status.ToEnumString() : "")}"),
                _ => new HtmlString($"<i class='fa fa-circle text-success'></i> {(showText ? status.ToEnumString() : "")}"),
            };

        //public static HtmlString ToHtml(this TicketStatus status)
        //    => status switch
        //    {
        //        TicketStatus.PENDING => new HtmlString($"<i class='fa fa-clock text-muted'></i> {status.ToEnumString()}"),
        //        TicketStatus.CANCELLED => new HtmlString($"<i class='fa fa-circle-xmark text-danger'></i> {status.ToEnumString()}"),
        //        TicketStatus.IN_PROGRESS => new HtmlString($"<i class='fa fa-play text-primary'></i> {status.ToEnumString()}"),
        //        TicketStatus.RESOLVED => new HtmlString($"<i class='fa fa-square-check text-success'></i> {status.ToEnumString()}"),
        //        _ => new HtmlString($"<i class='fa fa-clock text-muted'></i> {status.ToEnumString()}"),
        //    };
    }
}
