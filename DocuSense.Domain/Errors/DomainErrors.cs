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

        public static class KeycloakErrors
        {
            public static Error UserAlreadyExist => Error.Validation("Keycloak.UserAlreadyExist", "Bu email adresi ile kayıtlı bir kullanıcı zaten mevcut");

            public static Error InvalidUserData => Error.Validation("Keycloak.InvalidUserData", "Kullanıcı bilgileri geçersiz veya eksik.");

            public static Error TokenAcquisitionFailed =>
                Error.Failure("Keycloak.TokenFailed", "Kimlik doğrulama servisi ile iletişim kurulamadı.");

            public static Error InsufficientPermissions =>
                Error.Unauthorized("Keycloak.Forbidden", "Kullanıcı oluşturma yetkisi bulunamadı.");

            public static Error ServiceUnavailable =>
                Error.Failure("Keycloak.Unavailable", "Kimlik doğrulama servisi şu an kullanılamıyor.");
        }
    }
}