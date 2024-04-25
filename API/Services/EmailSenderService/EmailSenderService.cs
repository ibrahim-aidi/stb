using API.Dtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;




namespace API.Services.EmailSenderService
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _config;
        public EmailSenderService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendPasswordByEmailAsync(UserForEmail user)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Your New Account Details";

            var builder = new BodyBuilder();

          
            builder.HtmlBody = $@"<!DOCTYPE html>
                <html lang=""en"">

                <head>
                    <title></title>
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        * {{
                            box-sizing: border-box;
                        }}

                        body {{
                            margin: 0;
                            padding: 0;
                            -webkit-text-size-adjust: none;
                            text-size-adjust: none;
                            background-color: #FFFFFF;
                        }}

                        a[x-apple-data-detectors] {{
                            color: inherit !important;
                            text-decoration: none !important;
                        }}

                        #MessageViewBody a {{
                            color: inherit;
                            text-decoration: none;
                        }}

                        p {{
                            line-height: inherit;
                            margin: 0;
                        }}

                        @media (max-width:520px) {{
                            .desktop_hide,
                            .desktop_hide table {{
                                display: none !important;
                            }}

                            .image_block img+div {{
                                display: none;
                            }}

                            .mobile_hide {{
                                display: none;
                                min-height: 0;
                                max-height: 0;
                                max-width: 0;
                                overflow: hidden;
                                font-size: 0px;
                            }}
                        }}
                    </style>
                </head>

                <body>
                    <table class=""nl-container"" width=""100%"" border=""0"" cellpadding=""0"" 
                        cellspacing=""0"" role=""presentation"">
                        <tbody>
                            <tr>
                                <td>
                                    <table class=""row row-1"" align=""center"" width=""100%""
                                       border=""0"" cellpadding=""0"" cellspacing=""0"" 
                                        role=""presentation"">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <table class=""row-content stack""
                                                        align=""center"" border=""0"" cellpadding=""0""
                                                        cellspacing=""0"" role=""presentation""
                                                        style=""background-color: #ffffff;
                                                        color: #000000; width: 500px; margin: 0 auto;""
                                                        width=""500"">
                                                        <tbody>
                                                            <tr>
                                                                <td class=""column column-1""
                                                                width=""100%"" style=""font-weight: 400;
                                                                text-align: left; padding-bottom: 20px; padding-top: 15px;
                                                                vertical-align: top;"">
                                                                    <table class=""image_block 
                                                                    block-1"" width=""100%"" border=""0"" cellpadding=""0""
                                                                    cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"" style=""padding-bottom:5px;padding-left:5px;
                                                                             padding-right:5px;width:100%;"">
                                                                                <div class=""alignment"" align=""center"">
                                                                                    <div class=""fullWidth"" style=""max-width: 350px;"">
                                                                                        <img src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/2966/gif-resetpass.gif"" 
                                                                                        style=""display: block; height: auto; border: 0; width: 100%;"" width=""350"" 
                                                                                        alt=""reset-password"" title=""reset-password"" height=""auto""></div>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""heading_block block-2"" width=""100%"" border=""0"" 
                                                                      cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"" style=""text-align:center;width:100%;"">
                                                                                <h1 style=""margin: 0; color: #393d47; font-family: Tahoma, Verdana,
                                                                                Segoe, sans-serif; font-size: 25px; font-weight: normal; 
                                                                                letter-spacing: normal; line-height: 120%; text-align: center;"">
                                                                                <strong>Your New Account Details</strong></h1>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""paragraph_block block-3"" width=""100%"" border=""0"" 
                                                                        cellpadding=""10"" cellspacing=""0"" role=""presentation"" 
                                                                        style=""word-break: break-word;"">
                                                                        <tr>
                                                                            <td class=""pad"">
                                                                                <div style=""color:#393d47;font-family:Tahoma,Verdana,
                                                                                 Segoe,sans-serif;font-size:14px;line-height:150%;text-align:center;"">
                                                                                    <p>Hi {user.FirstName} {user.LastName},</p>
                                                                                    <p>We are pleased to inform you that your account has been successfully created. Below are your login credentials:</p>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""button_block block-4"" width=""100%"" border=""0""
                                                                     cellpadding=""15"" cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"">
                                                                                <div class=""alignment"" align=""center""><span style=""display:inline-block;color:#FFFFFF;
                                                                                background-color:#ffc727;border-radius:5px;width:200px;padding-top:10px;
                                                                                padding-bottom:10px;font-family:Tahoma, Verdana, Segoe, sans-serif;
                                                                                font-size:18px;text-align:center;"">{user.Login}</span></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""button_block block-5"" width=""100%"" border=""0"" 
                                                                        cellpadding=""15"" cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"">
                                                                                <div class=""alignment"" align=""center""><span style=""display:inline-block;
                                                                                    color:#FFFFFF;background-color:#ffc727;border-radius:5px;width:200px;
                                                                                    padding-top:10px;padding-bottom:10px;font-family:Tahoma, Verdana, Segoe, sans-serif;
                                                                                    font-size:18px;text-align:center;"">{user.Password}</span></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </body>

                </html>";

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("EmailHost").Value,
                587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("EmailUsername").Value,
                _config.GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


    }
}
