using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.EmailServices
{
	public class EmailSender : IEmailSender
	{
		private const string SendGridKey = "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";
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Execute(SendGridKey, subject, htmlMessage, email);
		}

		private Task Execute(string sendGridKey, string subject, string message, string email)
		{
			var client = new SendGridClient(sendGridKey);

			var msg = new SendGridMessage()
			{
				From = new EmailAddress("emrehanoglu_01@hotmail.com", "Shop App"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};

			msg.AddTo(new EmailAddress(email));
			return client.SendEmailAsync(msg);
		}
	}
}
