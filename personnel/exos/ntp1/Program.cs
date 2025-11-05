using System;
using NodaTime;
using static NodaTime.NetworkClock;
using System.Threading.Tasks;
using NodaTime.TimeZones;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace ntp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var networkClock = NetworkClock.Instance;
            networkClock.NtpServer = "pool.ntp.org";
            networkClock.CacheTimeout = Duration.FromMinutes(15);

            try
            {
                Instant ntpTime = networkClock.GetCurrentInstant();
                Instant systemTime = SystemClock.Instance.GetCurrentInstant();

                Console.WriteLine($"Heure TCP (UTC) : {ntpTime}");
                Console.WriteLine($"Heure système (UTC) : {systemTime}");

                Duration drift = systemTime - ntpTime;
                Console.WriteLine($"Drift détecté : {drift.TotalNanoseconds / 1_000_000.0:F3} ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur NetworkClock : {ex.Message}");
            }


            /*using (UdpClient client = new UdpClient())
            {
                string[] ntpServers =
                {
                    "0.pool.ntp.org",
                    "1.pool.ntp.org",
                    "time.google.com",
                    "time.cloudflare.com"
                };
                string ntpServer = "time.google.com";

                byte[] timeMessage = new byte[48];

                timeMessage[0] = 0x1B;

                IPEndPoint ntpReference = new IPEndPoint(Dns.GetHostAddresses(ntpServer)[0], 123);

                client.Connect(ntpReference);

                client.Send(timeMessage, timeMessage.Length);

                timeMessage = client.Receive(ref ntpReference);

                ulong intPart = (ulong)timeMessage[40] << 24 | (ulong)timeMessage[41] << 16 | (ulong)timeMessage[42] << 8 | (ulong)timeMessage[43];
                ulong fractPart = (ulong)timeMessage[44] << 24 | (ulong)timeMessage[45] << 16 | (ulong)timeMessage[46] << 8 | (ulong)timeMessage[47];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

                DateTime ntpTime = networkDateTime;

                Console.WriteLine($"Heure actuelle : {ntpTime.ToString("U", CultureInfo.CreateSpecificCulture("fr"))}");
                Console.WriteLine($"Heure actuelle : {ntpTime}");
                Console.WriteLine($"Heure actuelle : {ntpTime.ToString("dd/MM/yyyy")}");
                Console.WriteLine($"Heure actuelle : {ntpTime.ToString("o", CultureInfo.InvariantCulture)}");

                DateTime utcTime = ntpTime;
                DateTime systemTime = DateTime.UtcNow;
                TimeSpan timeDifference = systemTime - utcTime;
                Console.WriteLine($"Temps de différence : {timeDifference.TotalSeconds:F2} secondes");

                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.Local);
                Console.WriteLine($"Heure locale : {localTime}");

                TimeZoneInfo localSwissTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                DateTime localSwissTime = TimeZoneInfo.ConvertTimeToUtc(utcTime, localSwissTimeZone);
                Console.WriteLine($"Heure suisse : {localSwissTime}");

                TimeZoneInfo utcTimeZone = TimeZoneInfo.Utc;
                DateTime backToUtc = TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Local, utcTimeZone);
                Console.WriteLine($"Retour vers UTC : {backToUtc}");

                DisplayWorldClocks(utcTime);

                client.Close();
            } 
        }

        public static void DisplayWorldClocks(DateTime utcTime)
        {
            var TimeZones = new[]
            {
                ("UTC", TimeZoneInfo.Utc),
                ("New York", TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")),
                ("London", TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time")),
                ("Tokyo", TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time")),
                ("Sydney", TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"))
            };

            foreach ( var (name, timezone) in TimeZones )
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timezone);
                Console.WriteLine($"{name} : {localTime:yyyy-MM-dd HH:mm:ss}");
            }*/
        }
    }
}
