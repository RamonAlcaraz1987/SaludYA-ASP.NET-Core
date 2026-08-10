using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace SaludYa.Helpers
{
    /// <summary>
    /// Hashea contraseñas con PBKDF2 (mismo algoritmo usado por la API
    /// y por el panel web de Razor Pages, así un usuario puede iniciar
    /// sesión indistintamente desde la app o desde la web).
    /// </summary>
    public static class PasswordHasher
    {
        public static string Hash(string clave, IConfiguration configuration)
        {
            string salt = configuration["Salt"] ?? "SaludYaSalt2026";

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: clave,
                salt: Encoding.ASCII.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}