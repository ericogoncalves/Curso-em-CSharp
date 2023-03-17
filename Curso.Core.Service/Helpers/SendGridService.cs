using Curso.Core.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace SendMail
{
    public class SendGridService
    {
        private string apiKey { get; }
        public SendGridService()
        {
            apiKey = "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";
        }

        public async Task<dynamic> SendMail(string body, Tuple<string, string> sender, string template, string subject,
            List<EmailAddress> emails, string companyName = "", int? companyId = 0, string role = "", Dictionary<string, string> substitutions = null,
            List<Attachment> attachments = null, string code = "")
        {
            if (String.IsNullOrWhiteSpace(body)) body = "<p></p>";

            var listSubjects = new List<string>();

            var total = emails.Count * 1.0;
            var pageSize = 800;
            var pages = Math.Truncate(total / pageSize);
            pages += total % pageSize > 0 ? 1 : 0;
            var currentPage = 0;
            Response result = null;
            List<string> responses = new List<string>();

            while (currentPage <= pages)
            {
                var items = emails.Skip(currentPage * pageSize).Take(pageSize).ToList();
                if (items.Count > 0)
                {
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress(sender.Item1, sender.Item2);
                    var plainTextContent = HtmlToPlainText.Instance.Convert(body);
                    var htmlContent = body;

                    List<Dictionary<string, string>> lstSubstitutions = new List<Dictionary<string, string>>();
                    foreach (var item in items)
                    {
                        lstSubstitutions.Add(new Dictionary<string, string> { { "-Name-", item.Name } });
                        //substitutions.Add(new Dictionary<string, string> { { "-Code-", code } });
                        listSubjects.Add(subject);
                        lstSubstitutions.Add(new Dictionary<string, string> { { "-Email-", item.Email } });
                    }

                    var msg = MailHelper.CreateMultipleEmailsToMultipleRecipients(from, items, listSubjects, plainTextContent, htmlContent, lstSubstitutions);
                    msg.SetTemplateId(template.Replace("\n", ""));
                    msg.ReplyTo = from;
                    msg.AddSubstitutions(substitutions, 0);

                    if (attachments != null)
                        msg.Attachments = attachments;

                    result = await client.SendEmailAsync(msg);
                    var resp = result.Body.ReadAsStringAsync().Result;
                    responses.Add(resp);
                }
                currentPage++;
                Thread.Sleep(500);
            }

            return responses;
        }

        public async Task<dynamic> SendDynamicMail(string body, Tuple<string, string> sender, string template, string subject,
            EmailAddress emails, object substitutions = null, List<Attachment> attachments = null, string code = "")

        {
            var sendGridClient = new SendGridClient(apiKey);

            var sendGridMessage = new SendGridMessage();

            if (!string.IsNullOrEmpty(subject))
                sendGridMessage.SetSubject(subject);

            if (attachments != null)
                sendGridMessage.Attachments = attachments;

            sendGridMessage.SetFrom(sender.Item1, sender.Item2);
            sendGridMessage.AddTo(emails);
            sendGridMessage.SetTemplateId(template.Replace("\n", ""));
            sendGridMessage.SetTemplateData(substitutions);

            var response = sendGridClient.SendEmailAsync(sendGridMessage).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Console.WriteLine("Email sent");
            }

            return response;
        }

        public async Task<dynamic> SendMail_Old(string body, Tuple<string, string> sender, string template, string subject,
            List<EmailAddress> emails, string companyName = "", int? companyId = 0, string role = "")
        {
            if (String.IsNullOrWhiteSpace(body)) body = "<p></p>";

            var listSubjects = new List<string>();
            //return await Task<Response>.Run(() =>
            //{
            var total = emails.Count * 1.0;
            var pageSize = 800;
            var pages = Math.Truncate(total / pageSize);
            pages += total % pageSize > 0 ? 1 : 0;
            var currentPage = 0;
            Response result = null;
            List<string> responses = new List<string>();

            while (currentPage <= pages)
            {
                var items = emails.Skip(currentPage * pageSize).Take(pageSize).ToList();
                if (items.Count > 0)
                {
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress(sender.Item1, sender.Item2);
                    var plainTextContent = HtmlToPlainText.Instance.Convert(body);
                    var htmlContent = body;

                    List<Dictionary<string, string>> substitutions = new List<Dictionary<string, string>>();
                    foreach (var item in items)
                    {
                        substitutions.Add(new Dictionary<string, string> { { "-Name-", item.Name } });
                        /*if(!string.IsNullOrWhiteSpace(companyName))
                            substitutions.Add(new Dictionary<string, string> { { "-Customer-", companyName } });
                        if(!string.IsNullOrWhiteSpace(role))
                            substitutions.Add(new Dictionary<string, string> { { "-Role-", role } });
                        if(companyId > 0)
                            substitutions.Add(new Dictionary<string, string> { { "-CustomerId-", companyId.ToString() } });*/
                        listSubjects.Add(subject);
                    }

                    var msg = MailHelper.CreateMultipleEmailsToMultipleRecipients(from, items, listSubjects, plainTextContent, htmlContent, substitutions);
                    msg.SetTemplateId(template.Replace("\n", ""));
                    msg.ReplyTo = from;
                    result = client.SendEmailAsync(msg).GetAwaiter().GetResult();
                    var resp = result.Body.ReadAsStringAsync().Result;
                    responses.Add(resp);
                }
                currentPage++;
                Thread.Sleep(500);
            }

            return responses;
            //});
        }

        /// <summary>
        /// E-mail generico
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        /// <param name="link"></param>
        public void SendMailSimpleGeneric(string email, string nome, string link, string title, string templateId)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = title;
                var templateEmailToken = templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };

                var body = "";
                var substitution = new { Nome = nome };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailSimpleGeneric {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        /// <summary>
        /// E-mail de boas vindas
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        /// <param name="link"></param>
        public void SendMailWelcome(string email, string nome, string link, string templateId = "")
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Seja bem vindo(a)";
                var templateEmailToken = (string.IsNullOrEmpty(templateId) ? "d-eb1c97eb8ca3489080a78bb01eb25fa5" : templateId);
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };

                var body = "";
                var substitution = new { Nome = nome };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailWelcome {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }
        public void SendMailAddCustomerUserCompany(string email, string nome, string id)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Seu cadastro foi criado com sucesso.";
                var templateEmailToken = "d-6bce2674bf344244b409eb8b4bc0fc2d";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new { Email = email, Nome = nome, Url = id };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
                throw;
            }
        }

        public void SendMailAddPageCompany(string nome, string email, int processoSeletivo, Guid companyId, string nomeEmpresa, Guid id)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "Olá " + nome + " você foi convidado para participar do processo seletivo da empresa " + nomeEmpresa;
                var templateEmailToken = "d-8782617e94d444d6a43c6213e3bb73c5";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new { Nome = nome, Email = email, Processo = processoSeletivo, Company = companyId, NomeEmpresa = nomeEmpresa, InviteId = id };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
                throw;
            }
        }
        public void SendMailAddCustomerUserExistCompany(string email, string company)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "Curso - Seja Bem Vindo.";
                var templateEmailToken = "d-02f6b4c1774740cbb360aa4818409d3b";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new { Email = email, Empresa = company };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Lembretes de questionário, venda e abandono de carrinho
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        /// <param name="linkWhatsApp"></param>
        /// <param name="templateId"></param>
        public void SendRememberTask(string email, string nome, string linkWhatsApp, string templateId)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Lembrete";
                var templateEmailToken = templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new { Nome = nome, LinkWhatsApp = linkWhatsApp };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
                throw;
            }
        }
        public void SendMailTaskRemember(string email, string nome, string title, string dataVencimento, string templateId)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Lembrete.";
                var templateEmailToken = templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new { Nome = nome, TituloTarefa = title, DataVencimento = dataVencimento };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Lembrete de pagamento
        /// </summary>
        public void SendMailPaymentPixBoletoPending(string email, string nome, string templateId, string pixCopiaCola, string codigoBarras, string valor = "")
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Lembrete de Pagamento";
                var templateEmailToken = templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    Data = DateTime.Now.ToString("dd/MM/yyyy"),
                    PixCopiaCola = pixCopiaCola,
                    CodigoBarras = codigoBarras,
                    Valor = valor
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailPaymentPixBoletoPending {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Confirmação de pagamento
        /// </summary>
        public void SendMailPaymentPixBoletoPaid(string email, string nome, string templateId, string valor)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Pagamento Efetuado com Sucesso";
                var templateEmailToken = templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    Data = DateTime.Now.ToString("dd/MM/yyyy"),
                    Valor = valor
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailPaymentPixBoletoPaid {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Lembrete de pagamento Boleto
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        /// <param name="link"></param>
        public void SendMailPaymentBoleto(string email, string nome, string codigoBarras, string valor)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Lembrete de Pagamento por Boleto";
                var templateEmailToken = "d-2336651b212d4c51a4378c46d1e4e87e";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    Data = DateTime.Now.ToString("dd/MM/yyyy"),
                    CodigoBarras = codigoBarras,
                    Valor = valor
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailPaymentBoleto {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Cartão de Crédito Aprovado
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        /// <param name="link"></param>
        public void SendMailPaymentCreditCard(string email, string nome, string valor, string formaPagamento, string ultimosDigitosCartao)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Assinatura Aprovada";
                var templateEmailToken = "d-e8575855879d4558a8ac99447f32d967";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    Data = DateTime.Now.ToString("dd/MM/yyyy"),
                    Valor = valor,
                    FormaPagamento = formaPagamento,
                    UltimosDigitosCartao = ultimosDigitosCartao
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailPaymentCreditCard {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Cartão de Crédito Negado
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        /// <param name="link"></param>
        public void SendMailPaymentCreditCardNegado(string email, string nome, string pixCopiaCola, string formaPagamento)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Assinatura Outras Opções de Pagamento";
                var templateEmailToken = "d-1cd8cf78279b4e768d55e6a19b4a9d70";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    Data = DateTime.Now.ToString("dd/MM/yyyy"),
                    PixCopiaCola = pixCopiaCola,
                    FormaPagamento = formaPagamento
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailPaymentCreditCardNegado {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        public void SendMailGiftPaid(string email, string nomeDestinatario, string nomeRemetente, string message, string giftId, string templateId, string title = "", string valor = "", string formaPagamento = "")
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - " + (string.IsNullOrEmpty(title) ? "Você recebeu um presente muito especial" : title);
                var templateEmailToken = templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nomeRemetente,
                    NomeDestinatario = nomeDestinatario,
                    Mensagem = message,
                    GiftId = giftId,
                    Data = DateTime.Now.ToString("dd/MM/yyyy"),
                    Valor = valor,
                    FormaPagamento = formaPagamento
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailPaymentCreditCardNegado {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        public void SendMailObjective(string email, string nome, string title, string templateId)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - " + (string.IsNullOrEmpty(title) ? "Você criou seu primeiro objetivo" : title);
                var templateEmailToken = templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    TituloObjetivo = title
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailPaymentCreditCardNegado {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }
        public void SendMailcursoonTip(string email, string nome, string title, Guid Id)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - " + (string.IsNullOrEmpty(title) ? "Sua dica de evolução" : title);
                var templateEmailToken = "d-75af28c72b974415bc4005acc54dd883";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    Titulo = title,
                    Id = Id
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailcursoonTip {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }
        public void SendMailGetMember(string email, string nome, string nickname)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Compartilhe a sua Evolução!";
                var templateEmailToken = "d-c12703f137d541d1a65dcb1f4fedd9e2";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome,
                    NickName = nickname,
                    Cupom = nickname
                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailGetMember {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }
        public void SendMailInviteUser(string email, string nome, string message, string nomeRemetente)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - Você recebeu um convite especial!";
                var templateEmailToken = "d-7af844df9e6a4dffa458a94a3aee3623";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nomeRemetente,
                    Message = message,
                    NomeDestinatario = nome

                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailInviteUser {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }
        public void SendMailCheckout(string email, string nome, string templateId = "", string title = "")
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "curso - " + (string.IsNullOrEmpty(title) ? "Não vamos desistir de você!" : title);
                var templateEmailToken = string.IsNullOrEmpty(templateId) ? "d-586bee9c63534dd2975e7076944e77c9" : templateId;
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };


                var body = "";
                var substitution = new
                {
                    Nome = nome


                };

                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailCheckout {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }

        public void SendMailCourse(string nome, string email, string code)
        {
            try
            {
                var senderEmail = ConfigureSettings.GetEmailConfig().Sender;
                var subjectEmails = "Seu código - 30 minutos para mudar";
                var templateEmailToken = "d-47ca06a343064cfbad06729b6f76ef7f";
                if (senderEmail.Equals(""))
                    throw new Exception("No Repply Email not configured in Settings");
                if (subjectEmails.Equals(""))
                    throw new Exception("Subject Email not configured in Settings");
                if (templateEmailToken.Equals(""))
                    throw new Exception("Template Email Token not configured in Settings");

                var toUser = new EmailAddress
                {
                    Email = email,
                    Name = email
                };

                var body = "";
                var substitution = new { Email = email, Nome = nome, Code = code };


                var sender = new Tuple<string, string>(senderEmail, subjectEmails);
                var client = new SendGridService()
                    .SendDynamicMail(body
                        .Replace("¤", " "), sender, templateEmailToken, subjectEmails, toUser, substitution, null).Result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao enviar email SendMailGetMember {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log($"{ex.StackTrace}");
            }
        }
    }

}

public class Metrics
{
    public int blocks { get; set; }
    public int bounce_drops { get; set; }
    public int bounces { get; set; }
    public int clicks { get; set; }
    public int deferred { get; set; }
    public int delivered { get; set; }
    public int invalid_emails { get; set; }
    public int opens { get; set; }
    public int processed { get; set; }
    public int requests { get; set; }
    public int spam_report_drops { get; set; }
    public int spam_reports { get; set; }
    public int unique_clicks { get; set; }
    public int unique_opens { get; set; }
    public int unsubscribe_drops { get; set; }
    public int unsubscribes { get; set; }
}

public class Stat
{
    public Metrics metrics { get; set; }
}

public class StatisticsSandgrid
{
    public string date { get; set; }
    public List<Stat> stats { get; set; }
}

public class SendGridMessagesReturn
{
    public List<SendGridMessages> Messages { get; set; }
}

public class SendGridMessages
{
    [JsonProperty("clicks_count")]
    public int ClickCounts { get; set; }

    [JsonProperty("from_email")]
    public string From { get; set; }

    [JsonProperty("last_event_time")]
    public DateTimeOffset LastEventTime { get; set; }

    [JsonProperty("msg_id")]
    public string MessageId { get; set; }

    [JsonProperty("opens_count")]
    public int OpensCount { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("to_email")]
    public string ToEmail { get; set; }
}

public class HtmlToPlainText
{
    public static HtmlToPlainText Instance => new HtmlToPlainText();

    // Static data tables
    protected static Dictionary<string, string> _tags;
    protected static HashSet<string> _ignoreTags;

    // Instance variables
    protected TextBuilder _text;
    protected string _html;
    protected int _pos;

    // Static constructor (one time only)
    static HtmlToPlainText()
    {
        _tags = new Dictionary<string, string>();
        _tags.Add("address", "\n");
        _tags.Add("blockquote", "\n");
        _tags.Add("div", "\n");
        _tags.Add("dl", "\n");
        _tags.Add("fieldset", "\n");
        _tags.Add("form", "\n");
        _tags.Add("h1", "\n");
        _tags.Add("/h1", "\n");
        _tags.Add("h2", "\n");
        _tags.Add("/h2", "\n");
        _tags.Add("h3", "\n");
        _tags.Add("/h3", "\n");
        _tags.Add("h4", "\n");
        _tags.Add("/h4", "\n");
        _tags.Add("h5", "\n");
        _tags.Add("/h5", "\n");
        _tags.Add("h6", "\n");
        _tags.Add("/h6", "\n");
        _tags.Add("p", "\n");
        _tags.Add("/p", "\n");
        _tags.Add("table", "\n");
        _tags.Add("/table", "\n");
        _tags.Add("ul", "\n");
        _tags.Add("/ul", "\n");
        _tags.Add("ol", "\n");
        _tags.Add("/ol", "\n");
        _tags.Add("/li", "\n");
        _tags.Add("br", "\n");
        _tags.Add("/td", "\t");
        _tags.Add("/tr", "\n");
        _tags.Add("/pre", "\n");

        _ignoreTags = new HashSet<string>();
        _ignoreTags.Add("script");
        _ignoreTags.Add("noscript");
        _ignoreTags.Add("style");
        _ignoreTags.Add("object");
    }

    /// <summary>
    /// Converts the given HTML to plain text and returns the result.
    /// </summary>
    /// <param name="html">HTML to be converted</param>
    /// <returns>Resulting plain text</returns>
    public string Convert(string html)
    {
        // Initialize state variables
        _text = new TextBuilder();
        _html = html;
        _pos = 0;

        // Process input
        while (!EndOfText)
        {
            if (Peek() == '<')
            {
                // HTML tag
                bool selfClosing;
                string tag = ParseTag(out selfClosing);

                // Handle special tag cases
                if (tag == "body")
                {
                    // Discard content before <body>
                    _text.Clear();
                }
                else if (tag == "/body")
                {
                    // Discard content after </body>
                    _pos = _html.Length;
                }
                else if (tag == "pre")
                {
                    // Enter preformatted mode
                    _text.Preformatted = true;
                    EatWhitespaceToNextLine();
                }
                else if (tag == "/pre")
                {
                    // Exit preformatted mode
                    _text.Preformatted = false;
                }

                string value;
                if (_tags.TryGetValue(tag, out value))
                    _text.Write(value);

                if (_ignoreTags.Contains(tag))
                    EatInnerContent(tag);
            }
            else if (Char.IsWhiteSpace(Peek()))
            {
                // Whitespace (treat all as space)
                _text.Write(_text.Preformatted ? Peek() : ' ');
                MoveAhead();
            }
            else
            {
                // Other text
                _text.Write(Peek());
                MoveAhead();
            }
        }
        // Return result
        return HttpUtility.HtmlDecode(_text.ToString());
    }

    // Eats all characters that are part of the current tag
    // and returns information about that tag
    protected string ParseTag(out bool selfClosing)
    {
        string tag = String.Empty;
        selfClosing = false;

        if (Peek() == '<')
        {
            MoveAhead();

            // Parse tag name
            EatWhitespace();
            int start = _pos;
            if (Peek() == '/')
                MoveAhead();
            while (!EndOfText && !Char.IsWhiteSpace(Peek()) &&
                Peek() != '/' && Peek() != '>')
                MoveAhead();
            tag = _html.Substring(start, _pos - start).ToLower();

            // Parse rest of tag
            while (!EndOfText && Peek() != '>')
            {
                if (Peek() == '"' || Peek() == '\'')
                    EatQuotedValue();
                else
                {
                    if (Peek() == '/')
                        selfClosing = true;
                    MoveAhead();
                }
            }
            MoveAhead();
        }
        return tag;
    }

    // Consumes inner content from the current tag
    protected void EatInnerContent(string tag)
    {
        string endTag = "/" + tag;

        while (!EndOfText)
        {
            if (Peek() == '<')
            {
                // Consume a tag
                bool selfClosing;
                if (ParseTag(out selfClosing) == endTag)
                    return;
                // Use recursion to consume nested tags
                if (!selfClosing && !tag.StartsWith("/"))
                    EatInnerContent(tag);
            }
            else MoveAhead();
        }
    }

    // Returns true if the current position is at the end of
    // the string
    protected bool EndOfText
    {
        get { return (_pos >= _html.Length); }
    }

    // Safely returns the character at the current position
    protected char Peek()
    {
        return (_pos < _html.Length) ? _html[_pos] : (char)0;
    }

    // Safely advances to current position to the next character
    protected void MoveAhead()
    {
        _pos = Math.Min(_pos + 1, _html.Length);
    }

    // Moves the current position to the next non-whitespace
    // character.
    protected void EatWhitespace()
    {
        while (Char.IsWhiteSpace(Peek()))
            MoveAhead();
    }

    // Moves the current position to the next non-whitespace
    // character or the start of the next line, whichever
    // comes first
    protected void EatWhitespaceToNextLine()
    {
        while (Char.IsWhiteSpace(Peek()))
        {
            char c = Peek();
            MoveAhead();
            if (c == '\n')
                break;
        }
    }

    // Moves the current position past a quoted value
    protected void EatQuotedValue()
    {
        char c = Peek();
        if (c == '"' || c == '\'')
        {
            // Opening quote
            MoveAhead();
            // Find end of value
            int start = _pos;
            _pos = _html.IndexOfAny(new char[] { c, '\r', '\n' }, _pos);
            if (_pos < 0)
                _pos = _html.Length;
            else
                MoveAhead();    // Closing quote
        }
    }

    /// <summary>
    /// A StringBuilder class that helps eliminate excess whitespace.
    /// </summary>
    protected class TextBuilder
    {
        private StringBuilder _text;
        private StringBuilder _currLine;
        private int _emptyLines;
        private bool _preformatted;

        // Construction
        public TextBuilder()
        {
            _text = new StringBuilder();
            _currLine = new StringBuilder();
            _emptyLines = 0;
            _preformatted = false;
        }

        /// <summary>
        /// Normally, extra whitespace characters are discarded.
        /// If this property is set to true, they are passed
        /// through unchanged.
        /// </summary>
        public bool Preformatted
        {
            get
            {
                return _preformatted;
            }
            set
            {
                if (value)
                {
                    // Clear line buffer if changing to
                    // preformatted mode
                    if (_currLine.Length > 0)
                        FlushCurrLine();
                    _emptyLines = 0;
                }
                _preformatted = value;
            }
        }

        /// <summary>
        /// Clears all current text.
        /// </summary>
        public void Clear()
        {
            _text.Length = 0;
            _currLine.Length = 0;
            _emptyLines = 0;
        }

        /// <summary>
        /// Writes the given string to the output buffer.
        /// </summary>
        /// <param name="s"></param>
        public void Write(string s)
        {
            foreach (char c in s)
                Write(c);
        }

        /// <summary>
        /// Writes the given character to the output buffer.
        /// </summary>
        /// <param name="c">Character to write</param>
        public void Write(char c)
        {
            if (_preformatted)
            {
                // Write preformatted character
                _text.Append(c);
            }
            else
            {
                if (c == '\r')
                {
                    // Ignore carriage returns. We'll process
                    // '\n' if it comes next
                }
                else if (c == '\n')
                {
                    // Flush current line
                    FlushCurrLine();
                }
                else if (Char.IsWhiteSpace(c))
                {
                    // Write single space character
                    int len = _currLine.Length;
                    if (len == 0 || !Char.IsWhiteSpace(_currLine[len - 1]))
                        _currLine.Append(' ');
                }
                else
                {
                    // Add character to current line
                    _currLine.Append(c);
                }
            }
        }

        // Appends the current line to output buffer
        protected void FlushCurrLine()
        {
            // Get current line
            string line = _currLine.ToString().Trim();

            // Determine if line contains non-space characters
            string tmp = line.Replace("&nbsp;", String.Empty);
            if (tmp.Length == 0)
            {
                // An empty line
                _emptyLines++;
                if (_emptyLines < 2 && _text.Length > 0)
                    _text.AppendLine(line);
            }
            else
            {
                // A non-empty line
                _emptyLines = 0;
                _text.AppendLine(line);
            }

            // Reset current line
            _currLine.Length = 0;
        }

        /// <summary>
        /// Returns the current output as a string.
        /// </summary>
        public override string ToString()
        {
            if (_currLine.Length > 0)
                FlushCurrLine();
            return _text.ToString();
        }
    }
}

