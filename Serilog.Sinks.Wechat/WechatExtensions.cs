using Serilog.Configuration;

namespace Serilog.Sinks.Wechat
{
    /// <summary>
    /// Wechat sink extensions
    /// 微信接收器扩展
    /// </summary>
    public static class WechatExtensions
    {
        /// <summary>
        /// Wechat extension
        /// 微信扩展
        /// </summary>
        /// <param name="loggerConfiguration">Logger configuration</param>
        /// <param name="name">Identifying host name</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="formatProvider">Format provider</param>
        /// <returns>Result</returns>
        public static LoggerConfiguration Wechat(
          this LoggerSinkConfiguration loggerConfiguration, string name, IEnumerable<string> tokens, IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new Wechat(name, tokens, formatProvider));
        }
    }
}
