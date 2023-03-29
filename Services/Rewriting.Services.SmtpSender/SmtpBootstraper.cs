using Microsoft.Extensions.DependencyInjection;
using Rewriting.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.SmtpSender;

public static class SmtpBootstraper
{
    public static IServiceCollection AddSMTPSender(this IServiceCollection services)
    {
        services.AddSettings();
        services.AddSingleton<ISmtpSender, SmtpSender>();
        return services;
    }

    public static IServiceCollection AddSmtpStub(this IServiceCollection services)
    {
        services.AddSettings();
        services.AddSingleton<ISmtpSender, SmtpStub>();
        return services;
    }

    private static IServiceCollection AddSettings(this IServiceCollection services)
    {
        var settings = SettingsLoader.Load<SmtpSettings>("SMTP");
        services.AddSingleton(settings);

        return services;
    }
}
