using Newtonsoft.Json;
using RamDam.BackEnd.Core.Models.Api;
using RamDam.BackEnd.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.RamDamApi
{
    public class RamDamMovie: IEquatable<Movie>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_gmt { get; set; }
        public Guid Guid { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Modified_gmt { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public Title Title { get; set; }
        public Content Content { get; set; }
        public int Featured_media { get; set; }
        public string Template { get; set; }
        public int[] Categories { get; set; }
        public MovieAcf Acf { get; set; }
        public Yoast_Head_Json Yoast_head_json { get; set; }
        public MovieLinks _links { get; set; }

        public bool Equals(Movie movie)
        {
            return movie.IdRamDamMovie == this.Id
                && movie.Length.Equals(System.Web.HttpUtility.HtmlDecode(this.Acf.Length).GetHours())
                && movie.Realisator.Equals(System.Web.HttpUtility.HtmlDecode(this.Acf.Realisator.Replace("ć", "c")))
                && movie.Title.Equals(System.Web.HttpUtility.HtmlDecode(this.Title.Rendered))
                && (string.IsNullOrEmpty(movie.Link)?"":movie.Link).Equals(System.Web.HttpUtility.HtmlDecode(this.Link))
                && this.CheckScheduling(movie.Scheduling, (Screening[])this.Acf.Screening);
        }

        public bool CheckScheduling(List<Scheduling> schedulings, Screening[] screenings)
        {
            foreach (var screening in screenings)
            {
                var hour = screening.Hour.Length == 1 ? $"0{screening.Hour}" : screening.Hour;
                var minute = screening.Minute.Length == 1 ? $"0{screening.Minute}" : screening.Minute;
                var dt = DateTime.ParseExact($"{screening.Day} {hour}:{minute}", "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
                if (schedulings.Find(schedule => DateTime.Compare(schedule.StartTime, dt) == 0) == null)
                    return false;
            }
            return true;
        }
    }

    public class MovieAcf
    {
        public object Screening { get; set; }
        //public Screening[] Screening { get; set; }
        public bool Gallery { get; set; }
        [JsonProperty("buy-ticket-link")]
        public string BuyTicketLink { get; set; }
        [JsonProperty("trailer-embed-code")]
        public string TrailerEmbedCode { get; set; }
        public bool Cover { get; set; }
        public string Realisator { get; set; }
        public string Producer { get; set; }
        public string Origin { get; set; }
        public string Cast { get; set; }
        public string Length { get; set; }
        public string Distributor { get; set; }
        public string Language { get; set; }
        public bool Ratings { get; set; }
    }

    public class Screening
    {
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public bool Guest { get; set; }
    }

    public class MovieLinks
    {
        public Self[] Self { get; set; }
        public Collection[] Collection { get; set; }
        public About[] About { get; set; }
        [JsonProperty("wp:featuredmedia")]
        public WpFeaturedmedia[] WpFeaturedMedia { get; set; }
        [JsonProperty("wp:attachment")]
        public WpAttachment[] WpAttachment { get; set; }
        [JsonProperty("wp:term")]
        public WpTerm[] WpTerm { get; set; }
        public Cury[] Curies { get; set; }
    }

    public class WpTerm
    {
        public string Taxonomy { get; set; }
        public bool Embeddable { get; set; }
        public string Href { get; set; }
    }

    public class Yoast_Head_Json
    {
        public string Title { get; set; }
        public Robots Robots { get; set; }
        public string Canonical { get; set; }
        public string Og_locale { get; set; }
        public string Og_type { get; set; }
        public string Og_title { get; set; }
        public string Og_description { get; set; }
        public string Og_url { get; set; }
        public string Og_site_name { get; set; }
        public Og_Image[] Og_image { get; set; }
        public string Twitter_card { get; set; }
        public Twitter_Misc Twitter_misc { get; set; }
        public Schema Schema { get; set; }
    }

    public class Robots
    {
        public string Index { get; set; }
        public string Follow { get; set; }
        public string Maxsnippet { get; set; }
        public string Maximagepreview { get; set; }
        public string Maxvideopreview { get; set; }
    }

    public class Og_Image
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }

    public class Twitter_Misc
    {
        public string Duréedelectureestimée { get; set; }
    }

    public class Schema
    {
        public string Context { get; set; }
        public Graph[] Graph { get; set; }
    }

    public class Graph
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Potentialaction[] PotentialAction { get; set; }
        public string InLanguage { get; set; }
        public string ContentUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Ispartof IsPartOf { get; set; }
        public Primaryimageofpage PrimaryImageOfPage { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime DateModified { get; set; }
        public Breadcrumb Breadcrumb { get; set; }
        public Itemlistelement[] ItemListElement { get; set; }
    }

    public class Ispartof
    {
        public string Id { get; set; }
    }

    public class Primaryimageofpage
    {
        public string Id { get; set; }
    }

    public class Breadcrumb
    {
        public string Id { get; set; }
    }

    public class Potentialaction
    {
        public string Type { get; set; }
        public object Target { get; set; }
        public string Queryinput { get; set; }
    }

    public class Itemlistelement
    {
        public string Type { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Item { get; set; }
    }


}
