namespace DocuSense.Domain.Errors
{
    public static class DomainErrors
    {
        public static class File
        {
            public static Error NotFound => Error.NotFound("File.NotFound", "Dosya Bulunamadı!");
        }

        public static class User
        {
            public static Error Validation => Error.Validation("User.InvalidCredentials", "Email veya Şifre Hatalı!");
            public static Error Unauthorized => Error.Unauthorized("User.Unauthorized", "Kullanıcının Oturumu Sonlanmış Giriş Sayfasına Yönlendiriliyor!");
        }

        public static class Chat
        {
            public static Error NotFound => Error.NotFound("Chat.NotFound", "Sohbet Bulunamadı!");
            public static Error Unauthorized => Error.Unauthorized("User.Unauthorized", "Kullanıcı bulunamadı!");
        }
    }
}