using System;
using System.Net;
using System.Net.Mail;
namespace Form
{
    public class SendEmail
    {

        public static string SendMessage(string Email)
        {
            var Code = new Random().Next(0, 1000000).ToString("D6");
            var Domain = "@" + Email.Split("@")[1];
            var HideEmail = Email.Substring(0, 2) + "*****" + Domain;
            var Message = GetMessage(Code, HideEmail);
            var mail = new MailMessage("malzamatyplatform@gmail.com", Email, "تغيير كلمة مرور", Message)
            {
                IsBodyHtml = true
            };
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("malzamatyplatform@gmail.com", "123*Qwer"),
                EnableSsl = true
            };
            smtpClient.Send(mail);
            return Code;
        }
        public static string GetMessage(string Code, string Email)
        {
            return @"<html>
<Body>
<table dir=""rtl"">
      <tbody><tr><td id = ""m_-1042003640292068949i1"" style = ""padding:0;font-family:'Segoe UI Semibold','Segoe UI Bold','Segoe UI','Helvetica Neue Medium',Arial,sans-serif;font-size:17px;color:#707070""> حساب ملزمتي </ td></ tr>
           
                 <tr><td id = ""m_-1042003640292068949i2"" style = ""padding:0;font-family:'Segoe UI Light','Segoe UI','Helvetica Neue Medium',Arial,sans-serif;font-size:41px;color:#2672ec""> رمز إعادة تعيين كلمة مرور</ td></ tr>
                    
                          <tr><td id = ""m_-1042003640292068949i3"" style = ""padding:0;padding-top:25px;font-family:'Segoe UI',Tahoma,Verdana,Arial,sans-serif;font-size:14px;color:#2a2a2a""> الرجاء استخدام هذا الرمز لإعادة تعيين كلمة مرور حساب &lrm;<a dir = ""ltr"" id = ""m_-1042003640292068949iAccount"" class=""m_-1042003640292068949link"" style=""color:#2672ec;text-decoration:none"" href=""mailto:" + Email + @" target=""_blank"">" + Email + @"</a>&nbsp; على ملزمتي.</td></tr>
      <tr><td id = ""m_-1042003640292068949i4"" style= ""padding:0;padding-top:25px;font-family:'Segoe UI',Tahoma,Verdana,Arial,sans-serif;font-size:14px;color:#2a2a2a""> فيما يلي الرمز الخاص بك: 
      <span id = ""CodeId"" style= ""font-family:'Segoe UI Bold','Segoe UI Semibold','Segoe UI','Helvetica Neue Medium',Arial,sans-serif;font-size:14px;font-weight:bold;color:#2a2a2a""> " + Code + @" </span>
                                    
                                          </td></tr>
      <tr><td id = ""m_-1042003640292068949i5"" style= ""padding:0;padding-top:25px;font-family:'Segoe UI',Tahoma,Verdana,Arial,sans-serif;font-size:14px;color:#2a2a2a"">
                                    
                                          <tr><td id= ""m_-1042003640292068949i6"" style= ""padding:0;padding-top:25px;font-family:'Segoe UI',Tahoma,Verdana,Arial,sans-serif;font-size:14px;color:#2a2a2a""> شكرًا،</td></tr>
      <tr><td id = ""m_-1042003640292068949i7"" style= ""padding:0;font-family:'Segoe UI',Tahoma,Verdana,Arial,sans-serif;font-size:14px;color:#2a2a2a""> فريق حساب ملزمتي</td></tr>
</tbody></table>

</Body>
</html>";
        }
    }
}
