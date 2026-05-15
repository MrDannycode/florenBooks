using Microsoft.Data.SqlClient;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// Clasa utilitara centralizata pentru conexiunea la baza de date.
    /// Modifica connectionString cu calea corecta catre fisierul .mdf dupa instalare.
    /// </summary>
    public static class DatabaseHelper
    {
        // =====================================================================
        // ATENTIE: Modifica calea de mai jos cu locatia reala a fisierului .mdf
        // Exemplu: AttachDbFilename=C:\\Proiecte\\florenBooks\\library.mdf
        // =====================================================================
        private static readonly string ConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;" +
            @"AttachDbFilename=|DataDirectory|\library.mdf;" +
            @"Integrated Security=True";

        /// <summary>
        /// Returneaza un nou SqlConnection. Apeleaza .Open() dupa ce il primesti.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
