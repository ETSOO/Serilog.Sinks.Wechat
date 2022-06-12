using com.etsoo.WeiXinService;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.Wechat
{
    /// <summary>
    /// Wechat sink
    /// 微信接收器
    /// </summary>
    public class Wechat : ILogEventSink
    {
        const string Service = "Serilog";

        private readonly IEnumerable<string> tokens;
        private readonly IFormatProvider formatProvider;
        private DateTime lastRunAt;

        protected DateTime LastRunAt
        {
            get { lock (this) return lastRunAt; }
            set { lock (this) lastRunAt = value; }
        }

        /// <summary>
        /// Constructor
        /// 构造函数
        /// </summary>
        /// <param name="tokens">Tokens</param>
        /// <param name="formatProvider">Format provider</param>
        public Wechat(IEnumerable<string> tokens, IFormatProvider formatProvider = null)
        {
            this.tokens = tokens;
            this.formatProvider = formatProvider;
        }

        /// <summary>
        /// Emit log event
        /// 触发日志事件
        /// </summary>
        /// <param name="logEvent">Log event</param>
        public void Emit(LogEvent logEvent)
        {
            // 忽略三分钟内的重复记录
            var ts = DateTime.Now - LastRunAt;
            if (ts.TotalMinutes < 3) return;

            // 更新时间
            LastRunAt = DateTime.Now;

            // 发送的数据
            var data = new LogAlertDto
            {
                Tokens = tokens.ToArray(),
                Service = Service,
                Id = logEvent.GetHashCode().ToString(),
                Level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(formatProvider),
                Datetime = logEvent.Timestamp
            };

            // 发送
            // Fire and forget
            _ = Task.Run(async () =>
            {
                using var client = new HttpClient();
                var response = await ServiceUtils.SendLogAlertAsync(data, client);
                if (!response.IsSuccessStatusCode)
                {
                    // 记录日志
                    var code = (int)response.StatusCode;
                    var status = response.StatusCode.ToString();
                    var content = await response.Content.ReadAsStringAsync();
                    Log.Warning("Serilog.Sinks.Wechat failed - {status} ({code}), {content}", status, code, content);
                }
            });
        }
    }
}
