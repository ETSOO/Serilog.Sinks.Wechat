# Serilog.Sinks.Wechat
ETSOO's Serilog Sink Package for message publish to Wechat service account.
亿速思维开发的用于消息发布到微信服务帐户的 Serilog Sink。
快速提醒，提高运维反应速度和效率，3分钟提醒一次，避免信息轰炸。

## 关注微信服务号 etsoo2004
![服务号二维码](https://cn.etsoo.com/qrcode.jpg "服务号二维码")

从菜单的“服务”=>“访问令牌”，获取授权码，有效期为一年。

## Serilog 配置 (.Net 6)
- 仅支持 .NET 6 +
- 在 .NET Core App 中安装 Serilog
- Install Serilog, Serilog.AspNetCore, Serilog.Sinks.Wechat, Serilog.Sinks.File(可选)
- C# 配置代码
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
```
- appsettings.json 配置代码
```json
"Serilog": {
    "MinimumLevel": {
      "Default": "Warning"
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "文本日志文件路径",
          "rollingInterval": "Month"
        }
      },
      {
        "Name": "Wechat",
        "Args": {
          "restrictedToMinimumLevel": "Warning",
          "name": "标识名称",
          "tokens": [ "关注亿速思维服务号后获取的访问令牌" ]
        }
      }
    ],
    "Enrich": [
      "FromLogContext",
      "WithExceptionDetails"
    ]
  }
```
