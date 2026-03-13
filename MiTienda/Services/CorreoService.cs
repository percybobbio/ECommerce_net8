using MiTienda.Models;
using System.Net;
using System.Net.Mail;

namespace MiTienda.Services
{
    public class CorreoService
    {
        private readonly string _correoRemitente = "percybobbio@gmail.com";
        private readonly string _claveRemitente = "sgvtrmpudlgtjbrs";

        public async Task EnviarReciboPorCorreoAsync(string correoDestino, byte[] pdfBytes, int idVenta, List<CarritoItemVM> carrito)
        {
            try
            {
                string filasTabla = "";
                decimal total = 0;

                foreach(var item in carrito)
                {
                    filasTabla += $@"
                        <tr>
                            <td style='padding: 10px; border-bottom: 1px solid #eee;'>{item.Nombre}</td>
                            <td style='padding: 10px; border-bottom: 1px solid #eee;'>{item.Cantidad}</td>
                            <td style='padding: 10px; border-bottom: 1px solid #eee;'>{(item.Precio * item.Cantidad).ToString("C2")}</td>
                        </tr>";
                    total += item.Precio * item.Cantidad;
                }

                MailMessage mensaje = new MailMessage(_correoRemitente, correoDestino);
                mensaje.Subject = $"🎉 ¡Tu pedido #{idVenta} está confirmado! - MiTienda";
                mensaje.Body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 30px; border: 1px solid #eaeaea; border-radius: 10px; background-color: #ffffff;'>
                        <div style='text-align: center; margin-bottom: 20px;'>
                            <h2 style='color: #0d6efd; margin: 0;'>¡Gracias por tu compra! 🛍️</h2>
                        </div>
                        
                        <p style='color: #333; font-size: 16px;'>Hola,</p>
                        <p style='color: #333; font-size: 16px;'>Tu pedido <strong>#{idVenta}</strong> ha sido procesado con éxito. Aquí tienes el resumen de tu compra:</p>
                        
                        <table style='width: 100%; border-collapse: collapse; margin-top: 20px; margin-bottom: 20px;'>
                            <thead>
                                <tr style='background-color: #f8f9fa;'>
                                    <th style='padding: 10px; text-align: left; border-bottom: 2px solid #dee2e6;'>Producto</th>
                                    <th style='padding: 10px; text-align: center; border-bottom: 2px solid #dee2e6;'>Cant.</th>
                                    <th style='padding: 10px; text-align: right; border-bottom: 2px solid #dee2e6;'>Subtotal</th>
                                </tr>
                            </thead>
                            <tbody>
                                {filasTabla}
                            </tbody>
                        </table>
                        
                        <h3 style='text-align: right; color: #198754; margin-bottom: 30px;'>Total Pagado: {total.ToString("C2")}</h3>
                        
                        <p style='color: #333; font-size: 16px;'>Para tus registros, hemos adjuntado a este correo tu comprobante electrónico en formato PDF.</p>
                        
                        <hr style='border: none; border-top: 2px dashed #eee; margin: 30px 0;'>
                        <p style='color: #777; font-size: 14px; text-align: center;'>Si tienes alguna duda, responde a este correo. ¡Estamos para ayudarte! 🚀</p>
                    </div>";

                mensaje.IsBodyHtml = true;

                using (MemoryStream ms = new MemoryStream(pdfBytes))
                {
                    Attachment adjunto = new Attachment(ms, $"Recibo_Venta_{idVenta}.pdf", "application/pdf");
                    mensaje.Attachments.Add(adjunto);

                    using (SmtpClient clienteSmtp = new SmtpClient("smtp.gmail.com"))
                    {
                        clienteSmtp.Port = 587;
                        clienteSmtp.Credentials = new NetworkCredential(_correoRemitente, _claveRemitente);
                        clienteSmtp.EnableSsl = true;
                        await clienteSmtp.SendMailAsync(mensaje);
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error o manejarlo según tus necesidades
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                throw; // Re-lanzamos la excepción para que pueda ser manejada por quien llame a este método
            }
        }
    }
}
