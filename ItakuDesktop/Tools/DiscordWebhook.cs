using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Globalization;

namespace ItakuDesktop.Tools
{
    public class DiscordWebhook
    {
        public string username;
        public string avatar_url;
        public string content;
        public List<DiscordEmbed> embeds = new List<DiscordEmbed>();
        public bool tts;

        public DiscordWebhook() { }
        public DiscordWebhook(string name, string avatar, string message, List<DiscordEmbed> embeds)
        {
            username = name;
            avatar_url = avatar;
            content = message;
            this.embeds = embeds;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static void Send(string url, string username, string avatar, string content, List<DiscordEmbed> embeds)
        {
            var webhook = new DiscordWebhook(username, avatar, content, embeds);
            var serialized = JsonConvert.SerializeObject(webhook, Formatting.Indented);
            var files = new List<DiscordFile>();

            Send(url, webhook, files.ToArray());
        }

        private static void AddField(Stream stream, string bound, string cDisposition, string cType, byte[] data)
        {
            string prefix = stream.Length > 0 ? "\r\n--" : "--";
            string fBegin = prefix + bound + "\r\n";

            byte[] fBeginBuffer = Encode(fBegin);
            byte[] cDispositionBuffer = Encode(cDisposition);
            byte[] cTypeBuffer = Encode(cType);

            stream.Write(fBeginBuffer, 0, fBeginBuffer.Length);
            stream.Write(cDispositionBuffer, 0, cDispositionBuffer.Length);
            stream.Write(cTypeBuffer, 0, cTypeBuffer.Length);
            stream.Write(data, 0, data.Length);
        }

        public static string Decode(Stream source)
        {
            using (var reader = new StreamReader(source))
                return reader.ReadToEnd();
        }

        public static byte[] Encode(string source, string encoding = "utf-8")
        {
            return Encoding.GetEncoding(encoding).GetBytes(source);
        }

        private static void SetJsonPayload(MemoryStream stream, string bound, string json)
        {
            const string cDisposition = "Content-Disposition: form-data; name=\"payload_json\"\r\n";
            const string cType = "Content-Type: application/octet-stream\r\n\r\n";
            AddField(stream, bound, cDisposition, cType, Encode(json));
        }

        private static void SetFile(MemoryStream stream, string bound, int index, DiscordFile file)
        {
            string cDisposition = "Content-Disposition: form-data; name=\"file_" + index + "\"; filename=\"" + file.FileName + "\"\r\n";
            const string cType = "Content-Type: application/octet-stream\r\n\r\n";
            AddField(stream, bound, cDisposition, cType, File.ReadAllBytes(file.FilePath));
        }

        public static void Send(string url, DiscordWebhook message, params DiscordFile[] files)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(null, "Invalid Webhook URL.");

            string bound = "------------------------" + DateTime.Now.Ticks.ToString("x");
            var webhookRequest = new WebClient();
            webhookRequest.Headers.Add("Content-Type", "multipart/form-data; boundary=" + bound);

            var stream = new MemoryStream();
            for (int i = 0; i < files.Length; i++)
                SetFile(stream, bound, i, files[i]);

            string json = message.ToString();
            SetJsonPayload(stream, bound, json);

            byte[] bodyEnd = Encode("\r\n--" + bound + "--");
            stream.Write(bodyEnd, 0, bodyEnd.Length);

            try
            {
                webhookRequest.UploadData(url, stream.ToArray());
            }
            catch (WebException ex)
            {
                throw new WebException(Decode(ex.Response.GetResponseStream()));
            }

            stream.Dispose();
        }
    }

    public class DiscordFile
    {
        public string FilePath;
        public string FileName;
        public string ContentType;

        public DiscordFile() { }
        public DiscordFile(string FilePath, string ContentType) : this(FilePath, Path.GetFileName(FilePath), ContentType) { }
        public DiscordFile(string FilePath, string FileName, string ContentType)
        {
            this.FilePath = FilePath;
            this.FileName = FileName;
            this.ContentType = ContentType;
        }
    }

    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }

    public class DiscordEmbed
    {
        public int color = ColorToHex(Color.White);
        public EmbedAuthor author;

        public string title;
        public string url;

        public string description;

        public List<EmbedField> fields = new List<EmbedField>();
        public EmbedThumbnail thumbnail;
        public EmbedImage image;
        public EmbedFooter footer;

        public DateTime timestamp;

        public class EmbedAuthor
        {
            public string name;
            public string url;
            public string icon_url;

            public EmbedAuthor() { }
            public EmbedAuthor(string name, string url, string icon)
            {
                this.name = name;
                this.url = url;
                icon_url = icon;
            }
        }

        public class EmbedField
        {
            public string name;
            public string value;
            public bool inline;

            public EmbedField() { }
            public EmbedField(string name, string value, bool inline)
            {
                this.name = name;
                this.value = value;
                this.inline = inline;
            }
        }

        public class EmbedThumbnail
        {
            public string url;

            public EmbedThumbnail() { }
            public EmbedThumbnail(string url) { this.url = url; }
        }

        public class EmbedImage
        {
            public string url;

            public EmbedImage() { }
            public EmbedImage(string url) { this.url = url; }
        }

        public class EmbedFooter
        {
            public string text;
            public string icon_url;

            public EmbedFooter() { }
            public EmbedFooter(string text, string icon) { this.text = text; icon_url = icon; }
        }

        public static int ColorToHex(Color color)
        {
            string HS =
                color.R.ToString("X2") +
                color.G.ToString("X2") +
                color.B.ToString("X2");

            return int.Parse(HS, NumberStyles.HexNumber);
        }
    }
}